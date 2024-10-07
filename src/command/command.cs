using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using static CustomRounds.CustomRounds;
using static CustomRounds.Library;
using static CustomRounds.Menu;
using static CustomRounds.Round;

namespace CustomRounds;

public static class Command
{
    public static void Load(Config config)
    {
        Instance.AddCommand("css_voteround", "Vote round", Command_VoteRound);
        Instance.AddCommand("css_roundend", "Round end", Command_RoundEnd);
        Instance.AddCommand("css_shortlist", "Short list", Command_ShortList);

        foreach (RoundInfo round in config.Rounds.Values)
        {
            Instance.AddCommand($"css_{round.Shortcut}", round.Name, (player, command) =>
            {
                if (player == null)
                {
                    return;
                }

                if (!AdminManager.PlayerHasPermissions(player, config.AdminFlag))
                {
                    SendMessageToPlayer(player, "No access");
                    return;
                }

                if (command.GetArg(1) == "-i")
                {
                    SetNext(round, -1);
                    SendMessageToAllPlayers("Admin set inf round", player.PlayerName, round.Name);
                }
                else
                {
                    if (!int.TryParse(command.GetArg(1), out int roundtime))
                    {
                        roundtime = 1;
                    }

                    SetNext(round, roundtime);
                    SendMessageToAllPlayers("Admin set round", player.PlayerName, round.Name);
                }
            });
        }
    }

    public static void Command_VoteRound(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        if (!AdminManager.PlayerHasPermissions(player, Instance.Config.AdminFlag))
        {
            SendMessageToPlayer(player, "No access");
            return;
        }

        StartRoundVote();
        SendMessageToAllPlayers("Admin start vote", player.PlayerName);
    }

    public static void Command_RoundEnd(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        if (!AdminManager.PlayerHasPermissions(player, Instance.Config.AdminFlag))
        {
            SendMessageToPlayer(player, "No access");
            return;
        }

        Reset(true);
        SendMessageToAllPlayers("Admin reset round", player.PlayerName);
    }

    public static void Command_ShortList(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        if (!AdminManager.PlayerHasPermissions(player, Instance.Config.AdminFlag))
        {
            SendMessageToPlayer(player, "No access");
            return;
        }

        Server.NextFrame(() =>
        {
            foreach (RoundInfo round in Instance.Config.Rounds.Values)
            {
                player.PrintToConsole($"{round.Name} - !{round.Shortcut}");
            }
        });
    }
}