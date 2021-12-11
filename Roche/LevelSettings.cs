namespace Roche
{
    public class LevelSettings
    {
        public string LevelName { get; set; } = "Bedrock level";
        public GameMode GameMode { get; set; } = GameMode.Survival;
        public LevelType LevelType { get; set; } = LevelType.Default;
        public string LevelSeed { get; set; }
    }
}
