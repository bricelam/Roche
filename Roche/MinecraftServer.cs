using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Roche
{
    internal class MinecraftServer
    {
        Process _process;
        string _version;

        MinecraftServer(Process process)
            => _process = process;

        public static MinecraftServer Start(string path)
        {
            // TODO: Stop on exit
            var server = new MinecraftServer(
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = path,
                        //UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                    }));
            server.WaitForStart();

            return server;
        }

        // TODO: Whitelist, Players(list, op, deop, kick)

        public void AllowCheats(bool allow = true)
        {
            _process.StandardInput.WriteLine("changesetting allow-cheats " + (allow ? "true" : "false"));
        }

        public void SetDifficulty(Difficulty difficulty)
        {
            _process.StandardInput.WriteLine("changesetting difficulty " + (int)difficulty);
        }

        public void Stop()
        {
            _process.StandardInput.WriteLine("stop");
            _process.WaitForExit();
        }

        void WaitForStart()
        {
            foreach (var message in ReadLine(_process.StandardOutput))
            {
                if (message.StartsWith("Version "))
                {
                    _version = message[8..];
                }
                else if (message == "Server started.")
                {
                    break;
                }
            }
        }

        static IEnumerable<string> ReadLine(StreamReader reader)
        {
            string message;
            while ((message = reader.ReadLine()) != null)
            {
                if (message.Length > 0
                    && message[0] == '[')
                {
                    message = message[31..];
                }

                yield return message;
            }
        }
    }
}
