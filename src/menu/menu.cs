using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using static CustomRounds.CustomRounds;
using static CustomRounds.Round;

namespace CustomRounds;

public static class Menu
{
    public static Random Random { get; set; } = new();
    public static void StartRoundVote()
    {
        if (Instance.Config.Rounds == null)
        {
            return;
        }

        if (GlobalIsVoteInProgress)
        {
            return;
        }

        CenterHtmlMenu menu = new("Custom Rounds", Instance)
        {
            PostSelectAction = PostSelectAction.Nothing
        };

        static Dictionary<RoundInfo, int> GetRandomRounds()
        {
            Dictionary<RoundInfo, int> rounds = [];

            int roundcount = Math.Min(Instance.Config.HowManyRoundsInVote, Instance.Config.Rounds?.Values.Count ?? 0);

            while (rounds.Count < roundcount)
            {
                RoundInfo round = Instance.Config.Rounds!.Values.ElementAt(Random.Next(Instance.Config.Rounds.Values.Count));

                rounds.TryAdd(round, 0);
            }

            return rounds;
        }

        Dictionary<RoundInfo, int> rounds = GetRandomRounds();

        List<CCSPlayerController> players = [];

        foreach (KeyValuePair<RoundInfo, int> wk in rounds)
        {
            RoundInfo round = wk.Key;
            int vote = wk.Value;

            menu.AddMenuOption($"{round.Name} [{rounds[round]}]", (player, option) =>
            {
                if (!players.Contains(player))
                {
                    rounds[round]++;
                    players.Add(player);

                    option.Text = $"{round.Name} [{rounds[round]}]";
                }
            });
        }

        List<CCSPlayerController> allplayers = Utilities.GetPlayers();

        foreach (CCSPlayerController target in allplayers)
        {
            MenuManager.OpenCenterHtmlMenu(Instance, target, menu);
        }

        GlobalIsVoteInProgress = true;

        Instance.AddTimer(15.0f, () =>
        {
            RoundInfo round = rounds.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;

            SetNext(round, Instance.Config.HowManyRoundsLast);

            Library.PrintToChatAll("Next round is", round.Name);

            allplayers = Utilities.GetPlayers();

            foreach (CCSPlayerController player in allplayers)
            {
                MenuManager.CloseActiveMenu(player);
            }

            GlobalIsVoteInProgress = false;
        });
    }
}