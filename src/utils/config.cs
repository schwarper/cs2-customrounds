using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public class CustomRoundsConfig : BasePluginConfig
{
    [JsonPropertyName("default_ct_weapons")] public string[] DefaultCTWeapons { get; set; } = new string[] { "knife", "deagle", "awp" };
    [JsonPropertyName("default_t_weapons")] public string[] DefaultTWeapons { get; set; } = new string[] { "knife", "deagle", "awp" };
    [JsonPropertyName("round_end_cmd")] public string RoundEndCmd { get; set; } = "sv_gravity 800";
    [JsonPropertyName("admin_flag")] public string AdminFlag { get; set; } = "@css/generic";
    [JsonPropertyName("vote_round_count")] public int VoteRoundCount { get; set; } = 5;
    [JsonPropertyName("rounds")]
    public Dictionary<string, Round> Rounds { get; set; } = new Dictionary<string, Round>()
    {
        { "1", new Round { Name = "[Hs] Deagle Round", Weapons = new string[] {"knife", "deagle"}, Shortcut = "dr", OnlyHeadshot = true } },
        { "2", new Round { Name = "[No Scope] Awp Round", Weapons = new string[] {"knife", "awp"}, Shortcut = "nr", NoScope = true } },
        { "3", new Round { Name = "[Hs] AK47 Round", Weapons = new string[] {"knife", "ak47"}, Shortcut = "ar", OnlyHeadshot = true } },
        { "4", new Round { Name = "Knife Round", Weapons = new string[] {"knife"}, Shortcut = "kr", KnifeDamage = true, Speed = 2, Health = 30 } },
        { "5", new Round { Name = "Zeus Round", Weapons = new string[] {"zeus"}, Shortcut = "zr", Speed = 2, Cmd = "mp_taser_recharge_time 0.1" } },
        { "6", new Round { Name = "Glock Round", Weapons = new string[] {"knife", "glock"}, Shortcut = "gr" } },
        { "7", new Round { Name = "[Hs] M4A4 Round", Weapons = new string[] {"knife", "m4a4"}, Shortcut = "mr", OnlyHeadshot = true } },
        { "8", new Round { Name = "[Hs] USP Round", Weapons = new string[] {"knife", "usp"}, Shortcut = "ur", OnlyHeadshot = true } },
        { "9", new Round { Name = "SSG Round", Weapons = new string[] {"knife", "ssg08"}, Shortcut = "sr", Cmd = "sv_gravity 200" } },
    };
}

public partial class CustomRounds : BasePlugin, IPluginConfig<CustomRoundsConfig>
{
    public CustomRoundsConfig Config { get; set; } = new CustomRoundsConfig();

    public void OnConfigParsed(CustomRoundsConfig config)
    {
        Config = config;
    }
}

