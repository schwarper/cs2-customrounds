using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using static CounterStrikeSharp.API.Core.Listeners;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public static class Event
{
    public static void Load()
    {
        Instance.RegisterListener<OnTick>(OnTick);
        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Hook(CanUseFunc, HookMode.Pre);
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(OnTakeDamage, HookMode.Pre);

        Instance.RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
        Instance.RegisterEventHandler<EventRoundEnd>(OnRoundEnd);
        Instance.RegisterEventHandler<EventWeaponFire>(OnWeaponFire);
        //Instance.RegisterEventHandler<EventGrenadeThrown>(OnGrenadeThrown);
    }

    public static void Unload()
    {
        Instance.RemoveListener<OnTick>(OnTick);
        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Unhook(CanUseFunc, HookMode.Pre);
        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(OnTakeDamage, HookMode.Pre);
    }

    public static void OnTick()
    {
        if (Instance.GlobalCurrentRound == null)
        {
            return;
        }

        var players = Utilities.GetPlayers();

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
        if (Instance.GlobalCurrentRound == null)
        {
            return HookResult.Continue;
        }

        CBasePlayerWeapon clientweapon = hook.GetParam<CBasePlayerWeapon>(1);

        foreach (string weapon in Instance.GlobalCurrentRound.Weapons)
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
        if (Instance.GlobalCurrentRound == null)
        {
            return HookResult.Continue;
        }

        var info = hook.GetParam<CTakeDamageInfo>(1);

        if (info.Ability.Value?.DesignerName.Contains("knife") is true && Instance.GlobalCurrentRound.KnifeDamage is not true)
        {
            hook.SetReturn(false);
            return HookResult.Handled;
        }

        static unsafe HitGroup_t GetHitGroup(DynamicHook hook)
        {
            var info = hook.GetParam<nint>(1);
            nint v4 = *(nint*)(info + 0x68);
            nint v1 = *(nint*)(v4 + 16);

            var hitgroup = HitGroup_t.HITGROUP_GENERIC;

            if (v1 != nint.Zero)
            {
                hitgroup = (HitGroup_t)(*(uint*)(v1 + 56));
            }

            return hitgroup;
        }

        if (Instance.GlobalCurrentRound.OnlyHeadshot is true && GetHitGroup(hook) != HitGroup_t.HITGROUP_HEAD)
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

        _ = Instance.AddTimer(Instance.Config.OnSpawnDelay, () =>
        {
            if (Instance.GlobalCurrentRound == null)
            {
                Library.GiveDefaultWeapon(player);

                return;
            }

            player.RemoveWeapons();

            foreach (string weapon in Instance.GlobalCurrentRound.Weapons)
            {
                player.GiveNamedItem(weapon);
            }

            player.GiveWeapon(Instance.GlobalCurrentRound.Weapons);

            if (Instance.GlobalCurrentRound.MaxHealth is int maxhealth)
            {
                player.MaxHealth(maxhealth);
            }

            if (Instance.GlobalCurrentRound.Health is int health and > 0)
            {
                player.Health(health);
            }

            if (Instance.GlobalCurrentRound.Speed is float speed)
            {
                player.PlayerPawn?.Value?.Speed(speed);
            }
        });

        return HookResult.Continue;
    }

    public static HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        if (Instance.GlobalRoundCount == -1)
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

        if (Instance.GlobalNextRound != null)
        {
            Round.Set(Instance.GlobalNextRound);
            return HookResult.Continue;
        }

        if (Instance.GlobalCurrentRound == null)
        {
            return HookResult.Continue;
        }

        Instance.GlobalRoundCount--;

        if (Instance.GlobalRoundCount == 0)
        {
            Round.Reset(false);
        }

        return HookResult.Continue;
    }

    public static HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo info)
    {
        if (Instance.GlobalCurrentRound == null || Instance.GlobalCurrentRound.UnlimitedAmmo is not true)
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

    /*
    public static HookResult OnGrenadeThrown(EventGrenadeThrown @event, GameEventInfo info)
    {
        if (Instance.GlobalCurrentRound == null || Instance.GlobalCurrentRound.UnlimitedGrenade is not true)
        {
            return HookResult.Continue;
        }

        CCSPlayerController? player = @event.Userid;

        if (player == null || !player.IsValid)
        {
            return HookResult.Continue;
        }

        string weaponname = @event.Weapon;

        Server.NextFrame(() =>
        {
            player?.PlayerPawn.Value?.ItemServices?.As<CCSPlayer_ItemServices>().GiveNamedItem<CEntityInstance>($"weapon_{weaponname}");

        });

        return HookResult.Continue;
    }
    */

    public static void OnTick_NoScope(CCSPlayerController player)
    {
        if (Instance.GlobalCurrentRound!.NoScope is not true)
        {
            return;
        }

        CBasePlayerWeapon? activeweapon = player.PlayerPawn?.Value?.WeaponServices?.ActiveWeapon.Value;

        if (activeweapon == null)
        {
            return;
        }

        if (!Instance.GlobalScopeWeapons.Contains(activeweapon.DesignerName))
        {
            return;
        }

        activeweapon.NextSecondaryAttackTick = Server.TickCount + 500;
    }

    public static void OnTick_CenterMsg(CCSPlayerController player)
    {
        if (string.IsNullOrEmpty(Instance.GlobalCurrentRound!.CenterMsg))
        {
            return;
        }

        if (player is null || !player.IsValid || player.IsBot || player.IsHLTV)
          return;
        
        Library.PrintToCenterHtml(player);
    }
}
