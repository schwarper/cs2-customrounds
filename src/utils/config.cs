using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;
using static CustomRounds.CustomRounds;

namespace CustomRounds;

public class CustomRoundsConfig : BasePluginConfig
{
    [JsonPropertyName("default_ct_weapons")] public string[] DefaultCTWeapons { get; set; } = Array.Empty<string>();
    [JsonPropertyName("default_t_weapons")] public string[] DefaultTWeapons { get; set; } = Array.Empty<string>();
    [JsonPropertyName("round_end_cmd")] public string RoundEndCmd { get; set; } = string.Empty;
    [JsonPropertyName("rounds")] public Dictionary<string, Round> Rounds { get; set; } = new Dictionary<string, Round>();
}

public partial class CustomRounds : BasePlugin, IPluginConfig<CustomRoundsConfig>
{
    public CustomRoundsConfig Config { get; set; } = new CustomRoundsConfig();

    public void OnConfigParsed(CustomRoundsConfig config)
    {
        Config = config;
    }
}

