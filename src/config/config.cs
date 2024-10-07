using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;
using static CustomRounds.Round;

namespace CustomRounds;

public class Config : BasePluginConfig
{
    [JsonPropertyName("Tag")] public string Tag { get; set; } = "{red}[CSS] ";
    [JsonPropertyName("DefaultCTWeapons")] public string[] DefaultCTWeapons { get; set; } = ["weapon_knife", "weapon_deagle", "weapon_awp"];
    [JsonPropertyName("DefaultTWeapons")] public string[] DefaultTWeapons { get; set; } = ["weapon_knife", "weapon_deagle", "weapon_awp"];
    [JsonPropertyName("RoundEndExecuteCommands")] public string RoundEndExecuteCommands { get; set; } = "sv_gravity 800";
    [JsonPropertyName("HowManyRoundsInVote")] public int HowManyRoundsInVote { get; set; } = 5;
    [JsonPropertyName("VoteRoundCount")] public int VoteRoundCount { get; set; } = 5;
    [JsonPropertyName("AdminFlag")] public string AdminFlag { get; set; } = "@css/generic";
    [JsonPropertyName("OnSpawnDelay")] public float OnSpawnDelay { get; set; } = 0.1f;
    [JsonPropertyName("HowManyRoundsLast")] public int HowManyRoundsLast { get; set; } = 2;
    [JsonPropertyName("HtmlDisplayTime")] public float HtmlDisplayTime { get; set; } = -1;
    [JsonPropertyName("DuelSupport")] public bool DuelSupport { get; set; } = true;
    [JsonPropertyName("Rounds")] public Dictionary<string, RoundInfo> Rounds { get; set; } = [];
}