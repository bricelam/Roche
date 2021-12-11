using System;
using System.IO;

namespace Roche
{
    internal static class Paths
    {
        static Paths()
        {
            ServerDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                @".roche");
            ServerPropertiesPath = Path.Combine(
                ServerDir,
                "server.properties");
            ServerPath = Path.Combine(
                ServerDir,
                "bedrock_server.exe");
            WorldsDir = Path.Combine(
                ServerDir,
                "worlds");
        }

        public static string ServerDir { get; }
        public static string ServerPropertiesPath { get; }
        public static string ServerPath { get; }
        public static string WorldsDir { get; }
    }
}
