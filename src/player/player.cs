using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace CustomRounds;

public static class PlayerUtils
{
    static public void Health(this CCSPlayerController player, int health)
    {
        var playerPawn = player.PlayerPawn.Value;

        if (playerPawn == null)
        {
            return;
        }

        player.Health = health;
        playerPawn.Health = health;

        Utilities.SetStateChanged(playerPawn, "CBaseEntity", "m_iHealth");
    }
    static public void MaxHealth(this CCSPlayerController player, int maxhealth)
    {
        var playerPawn = player.PlayerPawn.Value;

        if (playerPawn == null)
        {
            return;
        }

        player.MaxHealth = maxhealth;
        playerPawn.MaxHealth = maxhealth;
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