using System.IO;

namespace Roche
{
    public class ServerConfiguration
    {
        public string ServerName { get; set; } = "Dedicated Server";
        public string WorldName { get; set; } = "Bedrock level";
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
        public bool ActivateCheats { get; set; }
        public bool OnlineMode { get; set; } = true;
        public int SimulationDistance { get; set; } = 4;
        public PlayerPermission DefaultPermission { get; set; } = PlayerPermission.Member;
        public bool RequireResourcePacks { get; set; }
        public bool EnableContentLogFile { get; set; }

        public GameMode GameMode { get; set; }
        public bool ForceGameMode { get; set; }
        public WorldType WorldType { get; set; } = WorldType.Default;
        public string Seed { get; set; }

        public static ServerConfiguration Load(string path)
        {
            var config = new ServerConfiguration();

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
                        config.ServerName = value;
                        break;

                    //case "gamemode":
                    //    switch (value)
                    //    {
                    //        case "survival":
                    //            config.GameMode = GameMode.Survival;
                    //            break;

                    //        case "creative":
                    //            config.GameMode = GameMode.Creative;
                    //            break;

                    //        case "adventure":
                    //            config.GameMode = GameMode.Adventure;
                    //            break;
                    //    }
                    //    break;

                    case "difficulty":
                        switch (value)
                        {
                            case "peaceful":
                                config.Difficulty = Difficulty.Peaceful;
                                break;

                            case "easy":
                                config.Difficulty = Difficulty.Easy;
                                break;

                            case "normal":
                                config.Difficulty = Difficulty.Normal;
                                break;

                            case "hard":
                                config.Difficulty = Difficulty.Hard;
                                break;
                        }
                        break;

                    case "allow-cheats":
                        config.ActivateCheats = value == "true";
                        break;

                    case "online-mode":
                        config.OnlineMode = value != "false";
                        break;

                    case "tick-distance":
                        config.SimulationDistance = int.Parse(value);
                        break;

                    //case "level-name":
                    //    config.WorldName = value;
                    //    break;

                    case "default-player-permission-level":
                        switch (value)
                        {
                            case "visitor":
                                config.DefaultPermission = PlayerPermission.Visitor;
                                break;

                            case "member":
                                config.DefaultPermission = PlayerPermission.Member;
                                break;

                            case "operator":
                                config.DefaultPermission = PlayerPermission.Operator;
                                break;
                        }
                        break;

                    case "texturepack-required":
                        config.RequireResourcePacks = value == "true";
                        break;

                    case "content-log-file-enabled":
                        config.EnableContentLogFile = value == "true";
                        break;
                }
            }

            return config;
        }

        public void Save(string path)
        {
            using var writer = new StringWriter();

            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length > 0
                        && line[0] != '#')
                    {
                        var key = line.Split('=', 2)[0];

                        switch (key)
                        {
                            case "server-name":
                                line = key + "=" + ServerName;
                                break;

                            //case "gamemode":
                            //    switch (GameMode)
                            //    {
                            //        case GameMode.Survival:
                            //            line = key + "=survival";
                            //            break;

                            //        case GameMode.Creative:
                            //            line = key + "=creative";
                            //            break;

                            //        case GameMode.Adventure:
                            //            line = key + "=adventure";
                            //            break;
                            //    }
                            //    break;

                            case "difficulty":
                                switch (Difficulty)
                                {
                                    case Difficulty.Peaceful:
                                        line = key + "=peaceful";
                                        break;

                                    case Difficulty.Easy:
                                        line = key + "=easy";
                                        break;

                                    case Difficulty.Normal:
                                        line = key + "=normal";
                                        break;

                                    case Difficulty.Hard:
                                        line = key + "=hard";
                                        break;
                                }
                                break;

                            case "allow-cheats":
                                line = key + "=" + (ActivateCheats ? "true" : "false");
                                break;

                            case "online-mode":
                                line = key + "=" + (OnlineMode ? "true" : "false");
                                break;

                            case "tick-distance":
                                line = key + "=" + SimulationDistance;
                                break;

                            //case "level-name":
                            //    line = key + "=" + WorldName;
                            //    break;

                            case "default-player-permission-level":
                                switch (DefaultPermission)
                                {
                                    case PlayerPermission.Visitor:
                                        line = key + "=visitor";
                                        break;

                                    case PlayerPermission.Member:
                                        line = key + "=member";
                                        break;

                                    case PlayerPermission.Operator:
                                        line = key + "=operator";
                                        break;
                                }
                                break;

                            case "texturepack-required":
                                line = key + "=" + (RequireResourcePacks ? "true" : "false");
                                break;

                            case "content-log-file-enabled":
                                line = key + "=" + (EnableContentLogFile ? "true" : "false");
                                break;
                        }
                    }

                    writer.WriteLine(line);
                }
            }

            File.WriteAllText(path, writer.ToString());
        }
    }

    public enum PlayerPermission
    {
        Visitor,
        Member,
        Operator
    }
}
