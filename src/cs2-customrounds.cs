using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;

namespace CustomRounds;

public class CustomRounds : BasePlugin, IPluginConfig<CustomRoundsConfig>
{
    public override string ModuleName => "Custom Rounds";
    public override string ModuleVersion => "0.0.9";
    public override string ModuleAuthor => "schwarper";

    public readonly string[] GlobalScopeWeapons =
    [
        "weapon_ssg08",
        "weapon_awp",
        "weapon_scar20",
        "weapon_g3sg1",
        "weapon_sg553",
        "weapon_sg556",
        "weapon_aug",
        "weapon_ssg08"
    ];

    public class RoundInfo
    {
        public required string Name { get; set; }
        public required string[] Weapons { get; set; }
        public required string Shortcut { get; set; }
        public bool? OnlyHeadshot { get; set; }
        public bool? KnifeDamage { get; set; }
        public bool? NoScope { get; set; }
        public bool? NoBuy { get; set; }
        public bool? UnlimitedAmmo { get; set; }
        //public bool? UnlimitedGrenade { get; set; }
        public int? Health { get; set; }
        public int? MaxHealth { get; set; }
        public float? Speed { get; set; }
        public string? Cmd { get; set; }
        public string? CenterMsg { get; set; } = "html_customround";
    }

    public static CustomRounds Instance { get; set; } = new();
    public RoundInfo? GlobalCurrentRound { get; set; } = null;
    public RoundInfo? GlobalNextRound { get; set; } = null;
    public bool GlobalIsVoteInProgress { get; set; } = false;
    public int GlobalRoundCount { get; set; } = 0;
    public CustomRoundsConfig Config { get; set; } = new CustomRoundsConfig();
    public Random Random { get; set; } = new();

    public override void Load(bool hotReload)
    {
        Event.Load();
    }

    public override void Unload(bool hotReload)
    {
        Event.Unload();
    }

    public void OnConfigParsed(CustomRoundsConfig config)
    {
        Instance = this;

        config.Prefix = StringExtensions.ReplaceColorTags(config.Prefix);

        Command.Load(config);
        Config = config;
    }
}