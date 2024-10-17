using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Utils;
using System.Runtime.InteropServices;
using static CounterStrikeSharp.API.Core.Listeners;
using static CustomRounds.CustomRounds;
using static CustomRounds.Round;

namespace CustomRounds;

public static class Event
{
    public static readonly string[] GlobalScopeWeapons =
    [
        "weapon_ssg08",
        "weapon_awp",
        "weapon_scar20",
        "weapon_g3sg1",
        "weapon_sg553",
        "weapon_sg556",
        "weapon_aug",
        "weapon_ssg08"
    ];

    private static float GlobalHtmlDisplayTime = 0;
    private static bool IsWindows;

    public static void Load()
    {
        IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        Instance.RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
        Instance.RegisterEventHandler<EventRoundStart>(OnRoundStart);
        Instance.RegisterEventHandler<EventRoundEnd>(OnRoundEnd);
        Instance.RegisterEventHandler<EventWeaponFire>(OnWeaponFire);
        Instance.RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);

        Instance.RegisterListener<OnTick>(OnTick);

        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(OnTakeDamage, HookMode.Pre);

        if (IsWindows)
        {
            VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Hook(OnCanUseFunc, HookMode.Pre);
        }
        else
        {
            VirtualFunctions.CCSPlayer_ItemServices_CanAcquireFunc.Hook(OnWeaponCanAcquire, HookMode.Pre);
        }
    }

    public static void Unload()
    {
        Instance.RemoveListener<OnTick>(OnTick);
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(OnTakeDamage, HookMode.Pre);

        if (IsWindows)
        {
            VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Unhook(OnCanUseFunc, HookMode.Pre);
        }
        else
        {
            VirtualFunctions.CCSPlayer_ItemServices_CanAcquireFunc.Unhook(OnWeaponCanAcquire, HookMode.Pre);
        }
    }

    public static void OnTick()
    {
        if (GlobalCurrentRound == null)
        {
            return;
        }

        for (int i = 0; i < Server.MaxPlayers; i++)
        {
            CCSPlayerController? player = Utilities.GetEntityFromIndex<CCSPlayerController>(i + 1);

            if (player?.IsValid is not true || player.IsBot || player.DesignerName != Library.playerdesignername)
            {
                continue;
            }

            OnTick_CenterMsg(player);

            if (player.PawnIsAlive)
            {
                OnTick_NoScope(player);
            }
        }
    }

    private static bool CanUseWeapon(CCSWeaponBaseVData? vdata)
    {
        if (vdata == null || GlobalCurrentRound == null)
        {
            return true;
        }

        return GlobalCurrentRound.Weapons.Any(vdata.Name.StartsWith);
    }

    public static HookResult OnCanUseFunc(DynamicHook hook)
    {
        var weapon = hook.GetParam<CBasePlayerWeapon>(1);
        var vdata = weapon.As<CCSWeaponBase>().VData;

        if (!CanUseWeapon(vdata))
        {
            weapon.Remove();
            hook.SetReturn(false);
            return HookResult.Handled;
        }

        return HookResult.Continue;
    }

    public static HookResult OnWeaponCanAcquire(DynamicHook hook)
    {
        CCSWeaponBaseVData vdata = VirtualFunctions.GetCSWeaponDataFromKeyFunc.Invoke(-1, hook.GetParam<CEconItemView>(1).ItemDefinitionIndex.ToString()) ?? throw new Exception("Failed to get CCSWeaponBaseVData");

        if (!CanUseWeapon(vdata))
        {
            hook.SetReturn(AcquireResult.NotAllowedByProhibition);
            return HookResult.Handled;
        }

        return HookResult.Continue;
    }

    public static HookResult OnTakeDamage(DynamicHook hook)
    {
        if (GlobalCurrentRound == null)
        {
            return HookResult.Continue;
        }

        CTakeDamageInfo info = hook.GetParam<CTakeDamageInfo>(1);

        if (info.Ability.Value?.DesignerName.Contains("knife") is true && GlobalCurrentRound.KnifeDamage is not true)
        {
            hook.SetReturn(false);
            return HookResult.Handled;
        }

        static unsafe HitGroup_t GetHitGroup(DynamicHook hook)
        {
            nint info = hook.GetParam<nint>(1);
            nint v4 = *(nint*)(info + 0x78);

            if (v4 == nint.Zero)
            {
                return HitGroup_t.HITGROUP_INVALID;
            }

            nint v1 = *(nint*)(v4 + 16);

            HitGroup_t hitgroup = HitGroup_t.HITGROUP_GENERIC;

            if (v1 != nint.Zero)
            {
                hitgroup = (HitGroup_t)(*(uint*)(v1 + 56));
            }

            return hitgroup;
        }

        if (GlobalCurrentRound.OnlyHeadshot is true && GetHitGroup(hook) != HitGroup_t.HITGROUP_HEAD)
        {
            hook.SetReturn(false);
            return HookResult.Handled;
        }

        return HookResult.Continue;
    }

    public static HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        if (@event.Userid is not CCSPlayerController player)
        {
            return HookResult.Continue;
        }

        Instance.AddTimer(Instance.Config.OnSpawnDelay, () =>
        {
            if (!player.IsValid)
            {
                return;
            }

            if (GlobalCurrentRound == null)
            {
                Library.GiveDefaultWeapon(player);

                return;
            }

            if (player.PlayerPawn.Value is not CCSPlayerPawn playerPawn)
            {
                return;
            }

            player.RemoveWeapons();

            foreach (string weapon in GlobalCurrentRound.Weapons)
            {
                player.GiveNamedItem(weapon);
            }

            player.GiveWeapon(GlobalCurrentRound.Weapons);

            if (GlobalCurrentRound.MaxHealth is int maxhealth)
            {
                player.MaxHealth(playerPawn, maxhealth);
            }

            if (GlobalCurrentRound.Health is int health and > 0)
            {
                player.Health(playerPawn, health);
            }

            if (GlobalCurrentRound.Kevlar is int kevlar and > 0)
            {
                playerPawn.Kevlar(kevlar);
            }

            if (GlobalCurrentRound.Helmet is bool helmet)
            {
                playerPawn.Helmet();
            }

            if (GlobalCurrentRound.Speed is float speed)
            {
                player.PlayerPawn?.Value?.Speed(speed);
            }
        });

        return HookResult.Continue;
    }

    public static HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if (GlobalRoundCount == -1)
        {
            return HookResult.Continue;
        }

        float htmlTime = Instance.Config.HtmlDisplayTime;
        GlobalHtmlDisplayTime = htmlTime == -1 ? -1 : Server.CurrentTime + htmlTime;

        return HookResult.Continue;
    }

    public static HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        if (GlobalRoundCount == -1)
        {
            return HookResult.Continue;
        }

        if (Instance.Config.VoteRoundCount > 0)
        {
            Library.GetTeamScore(out int tscore, out int ctscore);

            if ((tscore + ctscore) > 0 && (tscore + ctscore) % Instance.Config.VoteRoundCount == 0)
            {
                Menu.StartRoundVote();
            }
        }

        if (GlobalNextRound != null)
        {
            GlobalHtmlDisplayTime = Server.CurrentTime - 1;
            Set(GlobalNextRound);
            return HookResult.Continue;
        }

        if (GlobalCurrentRound == null)
        {
            return HookResult.Continue;
        }

        GlobalRoundCount--;

        if (GlobalRoundCount == 0)
        {
            Reset(false);
        }

        return HookResult.Continue;
    }

    public static HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo info)
    {
        if (GlobalCurrentRound == null || GlobalCurrentRound.UnlimitedAmmo is not true)
        {
            return HookResult.Continue;
        }

        if (@event.Userid?.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value is not CBasePlayerWeapon activeweapon)
        {
            return HookResult.Continue;
        }

        activeweapon.Clip1 += 1;

        return HookResult.Continue;
    }

    public static HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        if (GlobalCurrentRound == null || !Instance.Config.DuelSupport)
        {
            return HookResult.Continue;
        }

        var players = Utilities.GetPlayers()
            .Where(p => p.PawnIsAlive && p != @event.Userid)
            .Take(3)
            .ToList();

        if (players.Count == 2 && players[0].Team != players[1].Team)
        {
            Reset(false);
        }

        return HookResult.Continue;
    }

    public static void OnTick_NoScope(CCSPlayerController player)
    {
        if (GlobalCurrentRound!.NoScope is not true)
        {
            return;
        }

        if (player.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value is not CBasePlayerWeapon activeweapon)
        {
            return;
        }

        if (!GlobalScopeWeapons.Contains(activeweapon.DesignerName))
        {
            return;
        }

        activeweapon.NextSecondaryAttackTick = Server.TickCount + 500;
    }

    public static void OnTick_CenterMsg(CCSPlayerController player)
    {
        if (string.IsNullOrEmpty(GlobalCurrentRound!.CenterMsg))
        {
            return;
        }

        var htmldisplaytime = GlobalHtmlDisplayTime;

        if (htmldisplaytime != -1 && htmldisplaytime - Server.CurrentTime < 0)
        {
            return;
        }

        Library.PrintToCenterHtml(player);
    }
}