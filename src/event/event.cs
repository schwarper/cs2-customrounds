using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
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

    public static float GlobalHtmlDisplayTime = 0;

    public static void Load()
    {
        Instance.RegisterListener<OnTick>(OnTick);
        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Hook(CanUseFunc, HookMode.Pre);
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(OnTakeDamage, HookMode.Pre);

        Instance.RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
        Instance.RegisterEventHandler<EventRoundStart>(OnRoundStart);
        Instance.RegisterEventHandler<EventRoundEnd>(OnRoundEnd);
        Instance.RegisterEventHandler<EventWeaponFire>(OnWeaponFire);
        Instance.RegisterListener<OnEntitySpawned>(OnEntitySpawned);
    }

    public static void Unload()
    {
        Instance.RemoveListener<OnTick>(OnTick);
        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Unhook(CanUseFunc, HookMode.Pre);
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(OnTakeDamage, HookMode.Pre);
    }

    public static void OnTick()
    {
        if (GlobalCurrentRound == null)
        {
            return;
        }

        List<CCSPlayerController> players = Utilities.GetPlayers().Where(p => p.IsValid && !p.IsBot).ToList();

        foreach (CCSPlayerController? player in players)
        {
            OnTick_CenterMsg(player);

            if (player.PawnIsAlive)
            {
                OnTick_NoScope(player);
            }
        }
    }

    public static HookResult CanUseFunc(DynamicHook hook)
    {
        if (GlobalCurrentRound == null)
        {
            return HookResult.Continue;
        }

        CBasePlayerWeapon clientweapon = hook.GetParam<CBasePlayerWeapon>(1);

        foreach (string weapon in GlobalCurrentRound.Weapons)
        {
            string weaponname = weapon;

            if (weapon.Contains("usp"))
            {
                weaponname = "hkp2000";
            }
            else if (weapon.Contains("m4a1"))
            {
                weaponname = "m4a1";
            }

            if (clientweapon.DesignerName.Contains(weaponname))
            {
                return HookResult.Continue;
            }
        }

        clientweapon.Remove();
        hook.SetReturn(false);

        return HookResult.Handled;
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
            nint v4 = *(nint*)(info + 0x68);

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
        CCSPlayerController? player = @event.Userid;

        if (player == null)
        {
            return HookResult.Continue;
        }

        Instance.AddTimer(Instance.Config.OnSpawnDelay, () =>
        {
            if (GlobalCurrentRound == null)
            {
                Library.GiveDefaultWeapon(player);

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
                player.MaxHealth(maxhealth);
            }

            if (GlobalCurrentRound.Health is int health and > 0)
            {
                player.Health(health);
            }

            if (GlobalCurrentRound.Kevlar is int kevlar and > 0)
            {
                player.Kevlar(kevlar);
            }

            if (GlobalCurrentRound.Helmet is bool helmet)
            {
                player.Helmet();
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

        CBasePlayerWeapon? activeweapon = @event.Userid?.PlayerPawn.Value?.WeaponServices?.ActiveWeapon.Value;

        if (activeweapon == null)
        {
            return HookResult.Continue;
        }

        activeweapon.Clip1 += 1;

        return HookResult.Continue;
    }

    public static void OnEntitySpawned(CEntityInstance entity)
    {
        if (GlobalCurrentRound == null || GlobalCurrentRound.NoScope != true)
            return;

        if (entity == null || entity.Entity == null)
            return;

        if (string.IsNullOrEmpty(entity.DesignerName) || !entity.DesignerName.StartsWith("weapon_"))
            return;

        if (!GlobalCurrentRound.Weapons.Contains(entity.DesignerName))
        {
            entity.Remove();
        }
    }

    public static void OnTick_NoScope(CCSPlayerController player)
    {
        if (GlobalCurrentRound!.NoScope is not true)
        {
            return;
        }

        CBasePlayerWeapon? activeweapon = player.PlayerPawn?.Value?.WeaponServices?.ActiveWeapon.Value;

        if (activeweapon == null)
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

        if (GlobalHtmlDisplayTime != -1)
        {
            if (GlobalHtmlDisplayTime - Server.CurrentTime < 0)
            {
                return;
            }
        }

        Library.PrintToCenterHtml(player);
    }
}