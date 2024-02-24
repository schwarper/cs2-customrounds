using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace CustomRounds;

public partial class CustomRounds
{
    public void LoadCommands()
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

                    GlobalNextRound = round;

                    PrintToChatAll("Admin set round", player.PlayerName, round.Name);
                });
            }
        }
    }

    [ConsoleCommand("css_voteround")]
    [RequiresPermissions("@css/generic")]
    [CommandHelper(minArgs: 0, "Start voting for the next custom round", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Command_VoteRound(CCSPlayerController? player, CommandInfo command)
    {
        if (player == null)
        {
            return;
        }

        StartRoundVote(player);
    }
}