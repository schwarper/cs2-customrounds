using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public static class Round
{
    public class RoundInfo
    {
        public required string Name { get; set; }
        public required string[] Weapons { get; set; }
        public required string Shortcut { get; set; }
        public bool? OnlyHeadshot { get; set; }
        public bool? KnifeDamage { get; set; }
        public bool? NoScope { get; set; }
        public bool? NoBuy { get; set; }
        public bool? UnlimitedAmmo { get; set; }
        public int? Health { get; set; }
        public int? Kevlar { get; set; }
        public bool? Helmet { get; set; }
        public bool? TPOnKill { get; set; }
        public int? MaxHealth { get; set; }
        public float? Speed { get; set; }
        public string? Cmd { get; set; }
        public string? CenterMsg { get; set; } = "html_customround";
    }

    public static RoundInfo? GlobalCurrentRound { get; set; } = null;
    public static RoundInfo? GlobalNextRound { get; set; } = null;
    public static bool GlobalIsVoteInProgress { get; set; } = false;
    public static int GlobalRoundCount { get; set; } = 0;

    public static void Set(RoundInfo round)
    {
        GlobalCurrentRound = round;
        GlobalNextRound = null;

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
        GlobalNextRound = round;

        GlobalRoundCount = roundtime > 0 ? roundtime : -1;
    }

    public static void Reset(bool giveweapons)
    {
        GlobalCurrentRound = null;
        GlobalNextRound = null;
        GlobalRoundCount = 0;

        Library.SetBuyzoneInput("Enable");
        Server.ExecuteCommand(Instance.Config.RoundEndExecuteCommands);

        if (giveweapons)
        {
            List<CCSPlayerController> players = Utilities.GetPlayers();

            foreach (CCSPlayerController player in players)
            {
                Library.GiveDefaultWeapon(player);
            }
        }
    }
}