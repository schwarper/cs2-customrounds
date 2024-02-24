using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using System.Text;

namespace CustomRounds;

public partial class CustomRounds : BasePlugin
{
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

    private void PrintToCenterHtml(CCSPlayerController player, string message, params object[] args)
    {
        using (new WithTemporaryCulture(player.GetLanguage()))
        {
            StringBuilder builder = new();
            builder.AppendFormat(Localizer[message], args);
            player.PrintToCenterHtml(builder.ToString());
        }
    }
}