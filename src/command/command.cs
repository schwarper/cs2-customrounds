using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public static class Command
{
    public static void Load(CustomRoundsConfig config)
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
                    Library.PrintToChat(player, "No access");
                    return;
                }

                if (command.GetArg(1) == "-i")
                {
                    Round.SetNext(round, -1);
                    Library.PrintToChatAll("Admin set inf round", player.PlayerName, round.Name);
                }
                else
                {
                    if (!int.TryParse(command.GetArg(1), out int roundtime))
                    {
                        roundtime = 1;
                    }

                    Round.SetNext(round, roundtime);
                    Library.PrintToChatAll("Admin set round", player.PlayerName, round.Name);
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
            Library.PrintToChat(player, "No access");
            return;
        }

        Menu.StartRoundVote();
        Library.PrintToChatAll("Admin start vote", player.PlayerName);
    }

    public static void Command_RoundEnd(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        if (!AdminManager.PlayerHasPermissions(player, Instance.Config.AdminFlag))
        {
            Library.PrintToChat(player, "No access");
            return;
        }

        Round.Reset(true);
        Library.PrintToChatAll("Admin reset round", player.PlayerName);
    }

    public static void Command_ShortList(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        if (!AdminManager.PlayerHasPermissions(player, Instance.Config.AdminFlag))
        {
            Library.PrintToChat(player, "No access");
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