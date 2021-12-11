using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Roche
{
    public class ServerProperties
    {
        public string ServerName { get; set; } = "Dedicated Server";
        public string LevelName { get; set; } = "Bedrock level";
        public GameMode GameMode { get; set; }
        public LevelType LevelType { get; set; } = LevelType.Default;
        public string LevelSeed { get; set; }
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
        public PlayerPermissionLevel DefaultPlayerPermissionLevel { get; set; } = PlayerPermissionLevel.Member;
        public int TickDistance { get; set; } = 4;
        public bool AllowCheats { get; set; }
        public bool OnlineMode { get; set; } = true;

        public static ServerProperties Load(string path)
        {
            var properties = new ServerProperties();

            using var reader = new StreamReader(File.OpenRead(path));
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0
                    && line[0] == '#')
                    continue;

                var item = line.Split('=', 2);
                var key = item[0];
                var value = item.Length == 2 ? item[1] : null;

                switch (key)
                {
                    case "server-name":
                        properties.ServerName = value;
                        break;

                    case "level-name":
                        properties.LevelName = value;
                        break;

                    case "gamemode":
                        switch (value)
                        {
                            case "survival":
                                properties.GameMode = GameMode.Survival;
                                break;

                            case "creative":
                                properties.GameMode = GameMode.Creative;
                                break;

                            case "adventure":
                                properties.GameMode = GameMode.Adventure;
                                break;
                        }
                        break;

                    case "level-type":
                        switch (value)
                        {
                            case "flat":
                                properties.LevelType = LevelType.Flat;
                                break;

                            case "legacy":
                                properties.LevelType = LevelType.Legacy;
                                break;

                            case "default":
                                properties.LevelType = LevelType.Default;
                                break;
                        }
                        break;

                    case "level-seed":
                        properties.LevelSeed = value;
                        break;

                    case "difficulty":
                        switch (value)
                        {
                            case "peaceful":
                                properties.Difficulty = Difficulty.Peaceful;
                                break;

                            case "easy":
                                properties.Difficulty = Difficulty.Easy;
                                break;

                            case "normal":
                                properties.Difficulty = Difficulty.Normal;
                                break;

                            case "hard":
                                properties.Difficulty = Difficulty.Hard;
                                break;
                        }
                        break;

                    case "default-player-permission-level":
                        switch (value)
                        {
                            case "visitor":
                                properties.DefaultPlayerPermissionLevel = PlayerPermissionLevel.Visitor;
                                break;

                            case "member":
                                properties.DefaultPlayerPermissionLevel = PlayerPermissionLevel.Member;
                                break;

                            case "operator":
                                properties.DefaultPlayerPermissionLevel = PlayerPermissionLevel.Operator;
                                break;
                        }
                        break;

                    case "tick-distance":
                        properties.TickDistance = int.Parse(value);
                        break;

                    case "allow-cheats":
                        properties.AllowCheats = value == "true";
                        break;

                    case "online-mode":
                        properties.OnlineMode = value != "false";
                        break;
                }
            }

            return properties;
        }

        public void Save(string path)
        {
            using var writer = new StringWriter();

            var properties = new Dictionary<string, string>
            {
                ["server-name"] = ServerName,
                ["level-name"] = LevelName,
                ["gamemode"] = GameMode switch
                {
                    GameMode.Survival => "survival",
                    GameMode.Creative => "creative",
                    GameMode.Adventure => "adventure",
                    _ => null
                },
                ["level-type"] = LevelType switch
                {
                    LevelType.Flat => "flat",
                    LevelType.Legacy => "legacy",
                    LevelType.Default => "default",
                    _ => null
                },
                ["level-seed"] = LevelSeed,
                ["difficulty"] = Difficulty switch
                {
                    Difficulty.Peaceful => "peaceful",
                    Difficulty.Easy => "easy",
                    Difficulty.Normal => "normal",
                    Difficulty.Hard => "hard",
                    _ => null
                },
                ["default-player-permission-level"] = DefaultPlayerPermissionLevel switch
                {
                    PlayerPermissionLevel.Visitor => "visitor",
                    PlayerPermissionLevel.Member => "member",
                    PlayerPermissionLevel.Operator => "operator",
                    _ => null
                },
                ["tick-distance"] = TickDistance.ToString(CultureInfo.InvariantCulture),
                ["allow-cheats"] = AllowCheats
                    ? "true"
                    : "false",
                ["online-mode"] = OnlineMode
                    ? "true"
                    : "false"
            };

            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0
                        && line[0] != '#')
                    {
                        var key = line.Split('=', 2)[0];
                        var value = properties[key];

                        if (value != null)
                            line = key + "=" + value;

                        writer.WriteLine(line);

                        properties.Remove(key);
                    }
                }
            }

            foreach (var (key, value) in properties)
                writer.WriteLine(key + "=" + value);

            File.WriteAllText(path, writer.ToString());
        }
    }
}
