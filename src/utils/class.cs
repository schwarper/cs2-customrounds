namespace CustomRounds;

public partial class CustomRounds
{
    public class Round
    {
        public string Name { get; set; } = string.Empty;
        public string[] Weapons { get; set; } = Array.Empty<string>();
        public string Shortcut { get; set; } = string.Empty;
        public bool OnlyHeadshot { get; set; } = false;
        public bool KnifeDamage { get; set; } = false;
        public bool NoScope { get; set; } = false;
        public int Health { get; set; } = 100;
        public float Speed { get; set; } = 1.0f;
        public string Cmd { get; set; } = string.Empty;
        public string CenterMsg { get; set; } = string.Empty;
    }
}