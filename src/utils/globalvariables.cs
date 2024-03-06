namespace CustomRounds;

public partial class CustomRounds
{
    private readonly string[] GlobalScopeWeapons =
    {
        "weapon_ssg08",
        "weapon_awp",
        "weapon_scar20",
        "weapon_g3sg1",
        "weapon_sg553",
        "weapon_sg556",
        "weapon_aug",
        "weapon_ssg08"
    };

    private Round? GlobalCurrentRound = null;
    private Round? GlobalNextRound = null;
    private bool GlobalIsVoteInProgress = false;
    private bool GlobalInfRound = false;
    private static readonly Random random = new();
}