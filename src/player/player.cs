using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace CustomRounds;

public static class PlayerUtils
{
    static public void Health(this CCSPlayerController player, int health)
    {
        if (player.PlayerPawn == null || player.PlayerPawn.Value == null)
        {
            return;
        }

        player.Health = health;
        player.PlayerPawn.Value.Health = health;

        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
    }
    static public void MaxHealth(this CCSPlayerController player, int maxhealth)
    {
        if (player.PlayerPawn == null || player.PlayerPawn.Value == null)
        {
            return;
        }

        player.MaxHealth = maxhealth;
        player.PlayerPawn.Value.MaxHealth = maxhealth;
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