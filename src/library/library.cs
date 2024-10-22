using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Localization;
using System.Text;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public static class Library
{
    public const string playerdesignername = "cs_player_controller";

    public static void SendMessageToPlayer(CCSPlayerController player, string messageKey, params object[] args)
    {
        using (new WithTemporaryCulture(player.GetLanguage()))
        {
            LocalizedString message = Instance.Localizer[messageKey, args];
            VirtualFunctions.ClientPrint(player.Handle, HudDestination.Chat, Instance.Config.Tag + message, 0, 0, 0, 0);
        }
    }

    public static void SendMessageToAllPlayers(string messageKey, params object[] args)
    {
        for (int i = 0; i < Server.MaxPlayers; i++)
        {
            CCSPlayerController? player = Utilities.GetEntityFromIndex<CCSPlayerController>(i + 1);

            if (player?.IsValid is not true || player.IsBot || player.DesignerName != playerdesignername)
            {
                continue;
            }

            SendMessageToPlayer(player, messageKey, args);
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

    public static void RemoveAllWeapons(this CCSPlayerController player)
    {
        if (player.PlayerPawn.Value is not CCSPlayerPawn playerPawn)
        {
            return;
        }

        var armor = playerPawn.GetKevlar();
        var helmet = playerPawn.HasHelmet();

        player.RemoveWeapons();

        if (armor > 0)
        {
            playerPawn.SetKevlar(armor);
        }

        if (helmet)
        {
            playerPawn.GiveHelmet();
        }
    }

    public static void GiveDefaultWeapon(CCSPlayerController player)
    {
        if (player.Team == CsTeam.CounterTerrorist && Instance.Config.DefaultCTWeapons.Length > 0)
        {
            player.RemoveAllWeapons();
            player.GiveWeapon(Instance.Config.DefaultCTWeapons);
        }
        else if (Instance.Config.DefaultTWeapons.Length > 0)
        {
            player.RemoveAllWeapons();
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

    public static void Health(this CCSPlayerController player, CCSPlayerPawn playerPawn, int health)
    {
        player.Health = health;
        playerPawn.Health = health;

        Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_iHealth");
    }
    public static void SetKevlar(this CCSPlayerPawn playerPawn, int kevlar)
    {
        playerPawn.ArmorValue = kevlar;
    }
    public static int GetKevlar(this CCSPlayerPawn playerPawn)
    {
        return playerPawn.ArmorValue;
    }
    public static void GiveHelmet(this CCSPlayerPawn playerPawn)
    {
        if (playerPawn.ItemServices != null)
        {
            new CCSPlayer_ItemServices(playerPawn.ItemServices.Handle).HasHelmet = true;
        }
    }
    public static bool HasHelmet(this CCSPlayerPawn playerPawn)
    {
        if (playerPawn.ItemServices != null)
        {
            return new CCSPlayer_ItemServices(playerPawn.ItemServices.Handle).HasHelmet;
        }

        return false;
    }
    public static void MaxHealth(this CCSPlayerController player, CCSPlayerPawn playerPawn, int maxhealth)
    {
        player.MaxHealth = maxhealth;
        playerPawn.MaxHealth = maxhealth;
    }
    public static void Speed(this CCSPlayerPawn pawn, float speed)
    {
        pawn.VelocityModifier = speed;
    }
    public static void GiveWeapon(this CCSPlayerController player, string[] weapons)
    {
        foreach (string weapon in weapons)
        {
            player.GiveNamedItem(weapon);
        }
    }
}