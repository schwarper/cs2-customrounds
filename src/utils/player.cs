using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace CustomRounds;

public static class PlayerUtils
{
    private static readonly Dictionary<string, CsItem> GlobalWeaponDictionary = new()
    {
        { "zeus", CsItem.Taser },
        { "taser", CsItem.Taser },
        { "bumpmine", CsItem.Bumpmine },
        { "smoke", CsItem.SmokeGrenade },
        { "smokegrenade", CsItem.SmokeGrenade },
        { "flash", CsItem.Flashbang },
        { "flashbang", CsItem.Flashbang },
        { "hg", CsItem.HEGrenade },
        { "he", CsItem.HEGrenade },
        { "hegrenade", CsItem.HEGrenade },
        { "molotov", CsItem.Molotov },
        { "inc", CsItem.IncendiaryGrenade },
        { "incgrenade", CsItem.IncendiaryGrenade },
        { "decoy", CsItem.Decoy },
        { "ta", CsItem.TAGrenade },
        { "tagrenade", CsItem.TAGrenade },
        { "firebomb", CsItem.Firebomb },
        { "diversion", CsItem.Diversion },
        { "knife", CsItem.Knife },
        { "deagle", CsItem.Deagle },
        { "glock", CsItem.Glock },
        { "usp", CsItem.USPS },
        { "usp_silencer", CsItem.USPS },
        { "hkp2000", CsItem.HKP2000 },
        { "elite", CsItem.Elite },
        { "tec9", CsItem.Tec9 },
        { "p250", CsItem.P250 },
        { "cz75a", CsItem.CZ75 },
        { "fiveseven", CsItem.FiveSeven },
        { "revolver", CsItem.Revolver },
        { "mac10", CsItem.Mac10 },
        { "mp9", CsItem.MP9 },
        { "mp7", CsItem.MP7 },
        { "p90", CsItem.P90 },
        { "mp5", CsItem.MP5SD },
        { "mp5sd", CsItem.MP5SD },
        { "bizon", CsItem.Bizon },
        { "ump45", CsItem.UMP45 },
        { "xm1014", CsItem.XM1014 },
        { "nova", CsItem.Nova },
        { "mag7", CsItem.MAG7 },
        { "sawedoff", CsItem.SawedOff },
        { "m249", CsItem.M249 },
        { "negev", CsItem.Negev },
        { "ak", CsItem.AK47 },
        { "ak47", CsItem.AK47 },
        { "m4s", CsItem.M4A1S },
        { "m4a1s", CsItem.M4A1S },
        { "m4a1_silencer", CsItem.M4A1S },
        { "m4", CsItem.M4A1 },
        { "m4a1", CsItem.M4A1 },
        { "galil", CsItem.Galil },
        { "galilar", CsItem.Galil },
        { "famas", CsItem.Famas },
        { "sg556", CsItem.SG556 },
        { "awp", CsItem.AWP },
        { "aug", CsItem.AUG },
        { "ssg08", CsItem.SSG08 },
        { "scar20", CsItem.SCAR20 },
        { "g3sg1", CsItem.G3SG1 }
    };

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
        foreach (string weapondesignername in weapons)
        {
            if (GlobalWeaponDictionary.TryGetValue(weapondesignername, out CsItem weapon))
            {
                player.GiveNamedItem(weapon);
            }
        }
    }
}