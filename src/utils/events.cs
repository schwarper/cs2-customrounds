using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using static CounterStrikeSharp.API.Core.Listeners;

namespace CustomRounds;

public partial class CustomRounds
{
    private void LoadEvents()
    {
        RegisterListener<OnTick>(() =>
        {
            if (GlobalCurrentRound == null)
            {
                return;
            }

            foreach (CCSPlayerController? player in Utilities.GetPlayers().Where(player => player.Valid() && player.PawnIsAlive))
            {
                OnTick_NoScope(player);
                OnTick_CenterMsg(player);
            }
        });

        RegisterEventHandler<EventPlayerSpawn>((@event, info) =>
        {
            CCSPlayerController player = @event.Userid;

            if (player == null || !player.Valid())
            {
                return HookResult.Continue;
            }

            AddTimer(0.1f, () =>
            {
                if (GlobalCurrentRound == null)
                {
                    GiveDefaultWeapon(player);

                    return;
                }

                player.RemoveWeapons();

                player.GiveWeapon(GlobalCurrentRound.Weapons);

                if (GlobalCurrentRound.Health > 0)
                {
                    player.Health(GlobalCurrentRound.Health);
                }

                if (GlobalCurrentRound.Speed > 0)
                {
                    player.PlayerPawn?.Value?.Speed(GlobalCurrentRound.Speed);
                }
            });

            return HookResult.Continue;
        });

        RegisterEventHandler<EventPlayerHurt>((@event, info) =>
        {
            if (GlobalCurrentRound == null)
            {
                return HookResult.Continue;
            }

            CCSPlayerController player = @event.Userid;

            if (player == null || !player.Valid())
            {
                return HookResult.Continue;
            }

            if (GlobalCurrentRound.OnlyHeadshot && @event.Hitgroup != 1)
            {
                player.Health(@event.Health + @event.DmgHealth);
            }

            if (!GlobalCurrentRound.KnifeDamage && @event.Weapon.Contains("knife"))
            {
                player.Health(@event.Health + @event.DmgHealth);
            }

            return HookResult.Continue;
        });

        RegisterEventHandler<EventRoundEnd>((@event, info) =>
        {
            if (Config.VoteRoundCount > 0 && !GlobalInfRound)
            {
                int tscore = GetTeamScore(2);
                int ctscore = GetTeamScore(3);

                if ((tscore + ctscore) > 0 && (tscore + ctscore) % Config.VoteRoundCount == 0)
                {
                    StartRoundVote();
                }
            }

            if (GlobalNextRound != null && !GlobalInfRound)
            {
                GlobalCurrentRound = GlobalNextRound;
                GlobalNextRound = null;

                if (GlobalCurrentRound.NoBuy)
                {
                    SetBuyzoneInput("Disable");
                }

                Server.ExecuteCommand(GlobalCurrentRound.Cmd);
            }
            else
            {
                GlobalCurrentRound = null;

                SetBuyzoneInput("Enable");

                Server.ExecuteCommand(Config.RoundEndCmd);
            }

            return HookResult.Continue;
        });

        VirtualFunctions.CCSPlayer_WeaponServices_CanUseFunc.Hook((DynamicHook hook) =>
        {
            if (GlobalCurrentRound == null)
            {
                return HookResult.Continue;
            }

            CBasePlayerWeapon clientweapon = hook.GetParam<CBasePlayerWeapon>(1);

            if (GlobalCurrentRound.Weapons.Contains(clientweapon.DesignerName[7..]))
            {
                hook.SetReturn(false);
                return HookResult.Handled;
            }

            return HookResult.Continue;
        }, HookMode.Pre);
    }

    private void OnTick_NoScope(CCSPlayerController player)
    {
        if (!GlobalCurrentRound!.NoScope)
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

    private void OnTick_CenterMsg(CCSPlayerController player)
    {
        if (string.IsNullOrEmpty(GlobalCurrentRound!.CenterMsg))
        {
            return;
        }

        PrintToCenterHtml(player);
    }
}