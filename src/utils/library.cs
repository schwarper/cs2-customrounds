using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text;

namespace CustomRounds;

public partial class CustomRounds : BasePlugin
{
    private void PrintToChat(CCSPlayerController player, string message, params object[] args)
    {
        using (new WithTemporaryCulture(player.GetLanguage()))
        {
            StringBuilder builder = new(Localizer["Prefix"]);
            builder.AppendFormat(Localizer[message], args);
            player.PrintToChat(builder.ToString());
        }
    }

    private void PrintToChatAll(string message, params object[] args)
    {
        foreach (CCSPlayerController player in Utilities.GetPlayers().Where(p => p != null && p.Valid()))
        {
            using (new WithTemporaryCulture(player.GetLanguage()))
            {
                StringBuilder builder = new(Localizer["Prefix"]);
                builder.AppendFormat(Localizer[message], args);
                player.PrintToChat(builder.ToString());
            }
        }
    }

    private void PrintToCenterHtml(CCSPlayerController player)
    {
        using (new WithTemporaryCulture(player.GetLanguage()))
        {
            StringBuilder builder = new(Localizer[GlobalCurrentRound!.CenterMsg, GlobalCurrentRound.Name]);

            foreach (string weapon in GlobalCurrentRound.Weapons)
            {
                if (weapon.Equals("knife") && !GlobalCurrentRound.KnifeDamage)
                {
                    continue;
                }

                builder.Append(Localizer["html_png", weapon]);
            }

            if (GlobalCurrentRound.NoScope)
            {
                builder.Append(Localizer["html_png", "noscope"]);
            }

            if (GlobalCurrentRound.OnlyHeadshot)
            {
                builder.Append(Localizer["html_png", "headshot"]);
            }

            player.PrintToCenterHtml(builder.ToString());
        }
    }

    private static int GetTeamScore(int teamNum)
    {
        foreach (CCSTeam team in Utilities.FindAllEntitiesByDesignerName<CCSTeam>("cs_team_manager"))
        {
            if (team.TeamNum == teamNum)
            {
                return team.Score;
            }
        }

        return 0;
    }

    private void GiveDefaultWeapon(CCSPlayerController player)
    {
        if (player.Team == CsTeam.CounterTerrorist && Config.DefaultCTWeapons.Length > 0)
        {
            player.RemoveWeapons();
            player.GiveWeapon(Config.DefaultCTWeapons);
        }
        else if (Config.DefaultTWeapons.Length > 0)
        {
            player.RemoveWeapons();
            player.GiveWeapon(Config.DefaultTWeapons);
        }
    }

    private void SetBuyzoneInput(string input)
    {
        IEnumerable<CBaseEntity> Sites = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("func_buyzone");

        foreach (CBaseEntity entity in Sites)
        {
            entity.AcceptInput(input);
        }
    }
}