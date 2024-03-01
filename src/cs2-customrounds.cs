using CounterStrikeSharp.API.Core;

namespace CustomRounds;

public partial class CustomRounds : BasePlugin
{
    public override string ModuleName => "Custom Rounds";
    public override string ModuleVersion => "0.0.4";
    public override string ModuleAuthor => "schwarper";

    public override void Load(bool hotReload)
    {
        LoadCommands();
        LoadEvents();
    }
}