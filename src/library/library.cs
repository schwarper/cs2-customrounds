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
            StringBuilder builder = new(Instance.Config.Prefix);
            builder.AppendFormat(Instance.Localizer[message], args);
            player.PrintToChat(builder.ToString());
        }
    }
    public static void PrintToChatAll(string message, params object[] args)
    {
        var players = Utilities.GetPlayers();

        foreach (CCSPlayerController player in players)
        {
            using (new WithTemporaryCulture(player.GetLanguage()))
            {
                StringBuilder builder = new(Instance.Config.Prefix);
                builder.AppendFormat(Instance.Localizer[message], args);
                player.PrintToChat(builder.ToString());
            }
        }
    }
    public static void PrintToCenterHtml(CCSPlayerController player)
    {
        using (new WithTemporaryCulture(player.GetLanguage()))
        {
            StringBuilder builder = new(Instance.Localizer[Instance.GlobalCurrentRound!.CenterMsg!, Instance.GlobalCurrentRound.Name]);

            foreach (string weapon in Instance.GlobalCurrentRound.Weapons)
            {
                if (weapon.Contains("knife") && Instance.GlobalCurrentRound.KnifeDamage is not true)
                {
                    continue;
                }

                builder.Append(Instance.Localizer["html_png", weapon.Replace("weapon_", "")]);
            }

            if (Instance.GlobalCurrentRound.NoScope is true)
            {
                builder.Append(Instance.Localizer["html_png", "noscope"]);
            }

            if (Instance.GlobalCurrentRound.OnlyHeadshot is true)
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

        var csteammanager = Utilities.FindAllEntitiesByDesignerName<CCSTeam>("cs_team_manager");

        foreach (CCSTeam team in csteammanager)
        {
            switch (team.TeamNum)
            {
                case 1: tscore = team.Score; break;
                case 2: ctscore = team.Score; break;
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
        IEnumerable<CBaseEntity> buyzones = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("func_buyzone");

        foreach (CBaseEntity buyzone in buyzones)
        {
            buyzone.AcceptInput(input);
        }
    }
}