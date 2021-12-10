namespace Roche
{
    public partial class NewWorld
    {
        public string WorldName { get; set; } = "Bedrock level";
        public GameMode GameMode { get; set; } = GameMode.Survival;
        public WorldType WorldType { get; set; } = WorldType.Default;
        public string Seed { get; set; }
    }

    public enum GameMode
    {
        Survival = 0,
        Creative = 1,
        Adventure = 2
    }

    public enum WorldType
    {
        Flat,
        // Legacy,
        Default
    }
}
