using CounterStrikeSharp.API.Core;

namespace CustomRounds;

public partial class CustomRounds : BasePlugin, IPluginConfig<CustomRoundsConfig>
{
    public override string ModuleName => "Custom Rounds";
    public override string ModuleVersion => "0.0.7";
    public override string ModuleAuthor => "schwarper";

    public override void Load(bool hotReload)
    {
        LoadCommands();
        LoadEvents();
    }

    public void OnConfigParsed(CustomRoundsConfig config)
    {
        Config = config;
    }
}