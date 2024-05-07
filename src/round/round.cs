using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public static class Round
{
    public static void Set(RoundInfo round)
    {
        Instance.GlobalCurrentRound = round;
        Instance.GlobalNextRound = null;

        if (round.NoBuy is true)
        {
            Library.SetBuyzoneInput("Disable");
        }
        else
        {
            Library.SetBuyzoneInput("Enable");
        }

        if (!string.IsNullOrEmpty(round.Cmd))
        {
            Server.ExecuteCommand(round.Cmd);
        }
    }

    public static void SetNext(RoundInfo round, int roundtime)
    {
        Instance.GlobalNextRound = round;

        if (roundtime > 0)
        {
            Instance.GlobalRoundCount = roundtime;
        }
        else
        {
            Instance.GlobalRoundCount = -1;
        }
    }

    public static void Reset(bool giveweapons)
    {
        Instance.GlobalCurrentRound = null;
        Instance.GlobalNextRound = null;
        Instance.GlobalRoundCount = 0;

        Library.SetBuyzoneInput("Enable");
        Server.ExecuteCommand(Instance.Config.RoundEndCmd);

        if (giveweapons)
        {
            var players = Utilities.GetPlayers();

            foreach (CCSPlayerController player in players)
            {
                Library.GiveDefaultWeapon(player);
            }
        }
    }
}