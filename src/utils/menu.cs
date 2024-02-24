using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace CustomRounds;

public partial class CustomRounds
{
    private void StartRoundVote(CCSPlayerController player)
    {
        if (Config.Rounds == null)
        {
            return;
        }

        CenterHtmlMenu menu = new("Custom Rounds")
        {
            PostSelectAction = PostSelectAction.Close
        };

        Dictionary<Round, int> GetRandomRounds()
        {
            Dictionary<Round, int> rounds = new();

            int roundcount = Math.Min(5, Config.Rounds?.Values.Count ?? 0);

            while (rounds.Count < roundcount)
            {
                Round round = Config.Rounds!.Values.ElementAt(random.Next(Config.Rounds.Values.Count));

                if (!rounds.ContainsKey(round))
                {
                    rounds.Add(round, 0);
                }
            }

            return rounds;
        }

        Dictionary<Round, int> rounds = GetRandomRounds();

        List<CCSPlayerController> players = new();

        foreach (KeyValuePair<Round, int> wk in rounds)
        {
            Round round = wk.Key;
            int vote = wk.Value;

            menu.AddMenuOption(round.Name, (player, option) =>
            {
                if (!players.Contains(player))
                {
                    rounds[round]++;
                    players.Add(player);
                }
            });
        }

        foreach (CCSPlayerController target in Utilities.GetPlayers().Where(p => p.Valid()))
        {
            MenuManager.OpenCenterHtmlMenu(this, target, menu);
        }

        AddTimer(15.0f, () =>
        {
            Round round = rounds.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;

            GlobalNextRound = round;

            PrintToChatAll("Next round is", round.Name);

            foreach (CCSPlayerController player in Utilities.GetPlayers().Where(p => p.Valid()))
            {
                MenuManager.CloseActiveMenu(player);
            }
        });
    }
}