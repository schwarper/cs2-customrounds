using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public static class Library
{
    public static void PrintToChat(CCSPlayerController player, string message, params object[] args)
    {
        using (new WithTemporaryCulture(player.GetLanguage()))
        {
            StringBuilder builder = new(Instance.Config.Tag);
            builder.AppendFormat(Instance.Localizer[message], args);
            player.PrintToChat(builder.ToString());
        }
    }
    public static void PrintToChatAll(string message, params object[] args)
    {
        List<CCSPlayerController> players = Utilities.GetPlayers().Where(p => p.IsValid && !p.IsBot).ToList();

        foreach (CCSPlayerController player in players)
        {
            using (new WithTemporaryCulture(player.GetLanguage()))
            {
                StringBuilder builder = new(Instance.Config.Tag);
                builder.AppendFormat(Instance.Localizer[message], args);
                player.PrintToChat(builder.ToString());
            }
        }
    }
    public static void PrintToCenterHtml(CCSPlayerController player)
    {
        using (new WithTemporaryCulture(player.GetLanguage()))
        {
            StringBuilder builder = new(Instance.Localizer[Round.GlobalCurrentRound!.CenterMsg!, Round.GlobalCurrentRound.Name]);

            foreach (string weapon in Round.GlobalCurrentRound.Weapons)
            {
                if (weapon.Contains("knife") && Round.GlobalCurrentRound.KnifeDamage is not true)
                {
                    continue;
                }

                builder.Append(Instance.Localizer["html_png", weapon.Replace("weapon_", "")]);
            }

            if (Round.GlobalCurrentRound.NoScope is true)
            {
                builder.Append(Instance.Localizer["html_png", "noscope"]);
            }

            if (Round.GlobalCurrentRound.OnlyHeadshot is true)
            {
                builder.Append(Instance.Localizer["html_png", "headshot"]);
            }

            player.PrintToCenterHtml(builder.ToString());
        }
    }

    public static void GetTeamScore(out int tscore, out int ctscore)
    {
        tscore = 0;
        ctscore = 0;

        var csteammanager = Utilities.FindAllEntitiesByDesignerName<CCSTeam>("cs_team_manager").ToList();

        if (csteammanager.Count > 0)
        {
            foreach (CCSTeam team in csteammanager)
            {
                switch (team.TeamNum)
                {
                    case 2: tscore = team.Score; break;
                    case 3: ctscore = team.Score; break;
                }
            }
        }
    }

    public static void GiveDefaultWeapon(CCSPlayerController player)
    {
        if (player.Team == CsTeam.CounterTerrorist && Instance.Config.DefaultCTWeapons.Length > 0)
        {
            player.RemoveWeapons();
            player.GiveWeapon(Instance.Config.DefaultCTWeapons);
        }
        else if (Instance.Config.DefaultTWeapons.Length > 0)
        {
            player.RemoveWeapons();
            player.GiveWeapon(Instance.Config.DefaultTWeapons);
        }
    }

    public static void SetBuyzoneInput(string input)
    {
        var buyzones = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("func_buyzone").ToList();

        if (buyzones.Count > 0)
        {
            foreach (CBaseEntity buyzone in buyzones)
            {
                buyzone.AcceptInput(input);
            }
        }
    }
}