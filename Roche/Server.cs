using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Roche
{
    public class Server
    {
        Process _process;
        TaskCompletionSource _startCompletionSource;
        Thread _listenThread;

        public Server(string path)
        {
            _process = new Process
            {
                StartInfo = new()
                {
                    FileName = path,
                    //UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                }
            };
            _listenThread = new Thread(
                () =>
                {
                    string message;
                    while ((message = _process.StandardOutput.ReadLine()) != null)
                    {
                        // Strip timestamp and level
                        if (message.Length > 0
                            && message[0] == '[')
                        {
                            message = message[31..];
                        }

                        if (message == "Server started.")
                        {
                            _startCompletionSource?.SetResult();
                            _startCompletionSource = null;
                        }
                        else if (message.StartsWith("Player connected: "))
                        {
                            PlayerConnected.Invoke(this, GetPlayerEventArgs(message[18..]));
                        }
                        else if (message.StartsWith("Player disconnected: "))
                        {
                            PlayerDisconnected.Invoke(this, GetPlayerEventArgs(message[21..]));
                        }
                    }

                    static PlayerEventArgs GetPlayerEventArgs(string value)
                    {
                        var index = value.IndexOf(", xuid: ");

                        return new PlayerEventArgs
                        {
                            Name = value[..index],
                            Xuid = value[(index + 8)..]
                        };
                    }
                });
        }

        public event EventHandler<PlayerEventArgs> PlayerConnected;
        public event EventHandler<PlayerEventArgs> PlayerDisconnected;

        public Task StartAsync()
        {
            _process.Start();
            _startCompletionSource = new TaskCompletionSource();
            _listenThread.Start();

            return _startCompletionSource.Task;
        }

        public void AllowCheats(bool allow = true)
            => _process.StandardInput.WriteLine("changesetting allow-cheats " + (allow ? "true" : "false"));

        public void SetDifficulty(Difficulty difficulty)
            => _process.StandardInput.WriteLine("changesetting difficulty " + (int)difficulty);

        public void MakeOperator(string player)
            => _process.StandardInput.WriteLine("op " + player);

        public void MakeMember(string player)
            => _process.StandardInput.WriteLine("deop " + player);

        public void SetGameMode(string player, GameMode mode)
            => _process.StandardInput.WriteLine(
                "gamemode " + (mode switch
                {
                    GameMode.Survival => "survival",
                    GameMode.Creative => "creative",
                    GameMode.Adventure => "adventure",
                    _ => throw new Exception("Unexpected mode: " + mode)
                }) + " " + player);

        public void Stop()
        {
            _process.StandardInput.WriteLine("stop");
            _process.WaitForExit();
            _listenThread.Join();
        }
    }
}
