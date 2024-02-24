using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
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

            player.RemoveWeapons();

            if (GlobalCurrentRound == null)
            {
                if (player.Team == CsTeam.CounterTerrorist)
                {
                    player.GiveWeapon(Config.DefaultCTWeapons);
                }
                else
                {
                    player.GiveWeapon(Config.DefaultTWeapons);
                }

                return HookResult.Continue;
            }

            player.GiveWeapon(GlobalCurrentRound.Weapons);

            if (GlobalCurrentRound.Health > 0)
            {
                player.Health(GlobalCurrentRound.Health);
            }

            if (GlobalCurrentRound.Speed > 0)
            {
                player.PlayerPawn?.Value?.Speed(GlobalCurrentRound.Speed);
            }

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
                player.Health(100);
            }

            if (GlobalCurrentRound.KnifeDamage && !@event.Weapon.Contains("knife"))
            {
                player.Health(100);
            }

            return HookResult.Continue;
        });

        RegisterEventHandler<EventRoundEnd>((@event, info) =>
        {
            if (GlobalNextRound != null)
            {
                GlobalCurrentRound = GlobalNextRound;
                GlobalNextRound = null;

                Server.ExecuteCommand(GlobalCurrentRound.Cmd);
            }
            else
            {
                GlobalCurrentRound = null;

                Server.ExecuteCommand(Config.RoundEndCmd);
            }

            return HookResult.Continue;
        });
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

        PrintToCenterHtml(player, GlobalCurrentRound.CenterMsg, GlobalCurrentRound.Name);
    }
}