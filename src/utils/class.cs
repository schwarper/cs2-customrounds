namespace CustomRounds;

public partial class CustomRounds
{
    public class Round
    {
        public required string Name { get; set; }
        public required string[] Weapons { get; set; }
        public required string Shortcut { get; set; }
        public bool OnlyHeadshot { get; set; } = false;
        public bool KnifeDamage { get; set; } = false;
        public bool NoScope { get; set; } = false;
        public bool NoBuy { get; set; } = true;
        public int Health { get; set; } = 100;
        public float Speed { get; set; } = 1.0f;
        public string Cmd { get; set; } = string.Empty;
        public string CenterMsg { get; set; } = "html_customround";
    }
}