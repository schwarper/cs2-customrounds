using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace CustomRounds;

public partial class CustomRounds
{
    private void LoadCommands()
    {
        if (Config.Rounds != null)
        {
            foreach (Round round in Config.Rounds.Values)
            {
                AddCommand($"css_{round.Shortcut}", round.Name, (player, command) =>
                {
                    if (player == null)
                    {
                        return;
                    }

                    if (!AdminManager.PlayerHasPermissions(player, Config.AdminFlag))
                    {
                        PrintToChat(player, "No access");
                        return;
                    }

                    GlobalNextRound = round;

                    if (command.GetArg(1) == "-i")
                    {
                        GlobalInfRound = true;
                        PrintToChatAll("Admin set inf round", player.PlayerName, round.Name);
                    }
                    else
                    {
                        GlobalInfRound = false;
                        PrintToChatAll("Admin set round", player.PlayerName, round.Name);
                    }
                });
            }
        }
    }

    [ConsoleCommand("css_voteround")]
    [CommandHelper(minArgs: 0, "Start voting for the next custom round", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_VoteRound(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        if (!AdminManager.PlayerHasPermissions(player, Config.AdminFlag))
        {
            PrintToChat(player, "No access");
            return;
        }

        StartRoundVote();
    }

    [ConsoleCommand("css_roundend")]
    [CommandHelper(minArgs: 0, "Ends custom round", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_RoundEnd(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        if (!AdminManager.PlayerHasPermissions(player, Config.AdminFlag))
        {
            PrintToChat(player, "No access");
            return;
        }

        GlobalCurrentRound = null;
        GlobalNextRound = null;
        GlobalInfRound = false;

        foreach (CCSPlayerController target in Utilities.GetPlayers().Where(target => target != null && target.IsValid))
        {
            GiveDefaultWeapon(target);
        }
    }

    [ConsoleCommand("css_shortlist")]
    [CommandHelper(minArgs: 0, "Shows shortcut lists", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_ShortList(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        if (!AdminManager.PlayerHasPermissions(player, Config.AdminFlag))
        {
            PrintToChat(player, "No access");
            return;
        }

        Server.NextFrame(() =>
        {
            foreach (Round round in Config.Rounds.Values)
            {
                player.PrintToConsole($"{round.Name} - !{round.Shortcut}");
            }
        });
    }
}