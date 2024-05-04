using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public class CustomRoundsConfig : BasePluginConfig
{
    [JsonPropertyName("cr_prefix")] public string Prefix { get; set; } = " {red}[CSS]";
    [JsonPropertyName("default_ct_weapons")] public string[] DefaultCTWeapons { get; set; } = ["weapon_knife", "weapon_deagle", "weapon_awp"];
    [JsonPropertyName("default_t_weapons")] public string[] DefaultTWeapons { get; set; } = ["weapon_knife", "weapon_deagle", "weapon_awp"];
    [JsonPropertyName("round_end_execute_commands")] public string RoundEndCmd { get; set; } = "sv_gravity 800";
    [JsonPropertyName("max_round_length_for_vote")] public int VoteRoundLength { get; set; } = 5;
    [JsonPropertyName("vote_round_count")] public int VoteRoundCount { get; set; } = 5;
    [JsonPropertyName("admin_flag")] public string AdminFlag { get; set; } = "@css/generic";
    [JsonPropertyName("onspawn_delay")] public float OnSpawnDelay { get; set; } = 0.1f;
    [JsonPropertyName("how_many_rounds")] public int HowManyRounds { get; set; } = 2;
    [JsonPropertyName("rounds")] public Dictionary<string, RoundInfo> Rounds { get; set; } = [];
}

