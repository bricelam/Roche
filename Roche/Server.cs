using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Roche
{
    internal class Server
    {
        Process _process;
        Thread _listenThread;
        string _version;
        ObservableCollection<Player> _players = new();

        Server(Process process)
            => _process = process;

        public string Version
            => _version;

        public IEnumerable<Player> Players
            => _players;

        public static Server Start(string path)
        {
            // TODO: Stop on exit
            var server = new Server(
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
            foreach (var message in ReadLogLine(_process.StandardOutput))
            {
                if (message.StartsWith("Version "))
                {
                    _version = message[8..];
                }
                else if (message == "Server started.")
                {
                    _listenThread = new Thread(ListenToLog);
                    _listenThread.Start();
                    break;
                }
            }
        }

        void ListenToLog()
        {
            foreach (var message in ReadLogLine(_process.StandardOutput))
            {
                if (message.StartsWith("Player connected: "))
                {
                    var (name, xuid) = ParsePlayer(message[18..]);
                    _players.Add(
                        new Player
                        {
                            Name = name,
                            Xuid = xuid
                        });
                }
                else if (message.StartsWith("Player disconnected: "))
                {
                    var (name, xuid) = ParsePlayer(message[21..]);
                    var player = _players.Where(p => p.Xuid == xuid && p.Name == name).FirstOrDefault();
                    _players.Remove(player);
                }
            }

            static (string name, string xuid) ParsePlayer(string value)
            {
                var index = value.IndexOf(", xuid: ");

                return (value[..index], value[(index + 8)..]);
            }
        }

        static IEnumerable<string> ReadLogLine(StreamReader reader)
        {
            string message;
            while ((message = reader.ReadLine()) != null)
            {
                // Strip timestamp and level
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
