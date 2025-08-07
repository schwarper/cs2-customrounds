using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;

namespace CustomRounds;

public class CustomRounds : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "Custom Rounds";
    public override string ModuleVersion => "1.5";
    public override string ModuleAuthor => "schwarper";

    public static CustomRounds Instance { get; set; } = new();
    public Config Config { get; set; } = new Config();

    public override void Load(bool hotReload)
    {
        Event.Load();
        Command.Load(Instance.Config);
    }

    public override void Unload(bool hotReload)
    {
        Event.Unload();
    }

    public void OnConfigParsed(Config config)
    {
        Instance = this;

        config.Tag = config.Tag.ReplaceColorTags();

        Config = config;
    }

}
