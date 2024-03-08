using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace CustomRounds;

public static class PlayerUtils
{
    static public bool Valid(this CCSPlayerController player)
    {
        return player.IsValid && player.SteamID.ToString().Length == 17;
    }
    static public void Health(this CCSPlayerController player, int health)
    {
        if (player.PlayerPawn == null || player.PlayerPawn.Value == null)
        {
            return;
        }

        player.Health = health;
        player.PlayerPawn.Value.Health = health;

        if (health > 100)
        {
            player.MaxHealth = health;
            player.PlayerPawn.Value.MaxHealth = health;
        }

        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
    }
    static public void Speed(this CCSPlayerPawn pawn, float speed)
    {
        pawn.VelocityModifier = speed;
    }
    static public void GiveWeapon(this CCSPlayerController player, string[] weapons)
    {
        foreach (string weapon in weapons)
        {
            player.GiveNamedItem(weapon);
        }
    }
}