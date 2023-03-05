//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⡟⠁⢀⣤⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠹⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⣼⣿⣿⣿⣧⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠈⠿⡿⣿⠿⠿⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠰⠿⠧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⢰⣿⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⢠⠃⠹⣷⡀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⢀⠏⠀⠀⢻⣧⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⡞⠒⠒⠒⠒⢿⣇⠀⠀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠀⠀⣀⣴⣇⡀⠀⠀⢀⣈⣿⣆⡀⠀⠀⠀⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢲⣶⠆⠀⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣶⣶⣿⣾⣶⡀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢻⣿⣿⣿⡟⠀⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⣧⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠛⠁⢀⣰⣿⣿⣿⣿⣿
//⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿

using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace SpeedServer
{
    class Program
    {
        public static Dictionary<string, TcpClient> Players = new Dictionary<string, TcpClient>();
        public static List<string> PlayerNames = new List<string>();
        public static int clientCount = 0;

        public static HandleGame? GameState;

        static void Main(string[] args)
        {
            var serverSocket = new TcpListener(IPAddress.Any, 88);
            serverSocket.Start();
            Console.WriteLine("Speed Client Started at Port: 88");

            while (true)
            {
                try
                {
                    var clientSocket = serverSocket.AcceptTcpClient();
                    var data = clientSocket.ReadString();
                    if (Players.ContainsKey(data))
                    {
                        data = data + "1";
                    }
                    Console.WriteLine(data + " joined.");
                    if (clientCount < 1)
                    {
                        var newPlayer = new HandlePlayer();
                        Players.Add(data, clientSocket);
                        PlayerNames.Add(data);
                        newPlayer.StartClient(clientSocket, data);
                    } else if (clientCount < 2)
                    {
                        var newPlayer = new HandlePlayer();
                        Players.Add(data, clientSocket);
                        PlayerNames.Add(data);
                        newPlayer.StartClient(clientSocket, data);

                        var startMessage = new StartingMessage()
                        {
                            Status = "Starting",
                            Player1Name = PlayerNames[0],
                            Player2Name = PlayerNames[1]
                        };

                        var startMessageJson = JsonSerializer.Serialize<StartingMessage>(startMessage);

                        Console.WriteLine(startMessageJson);
                        Players.Broadcast(startMessageJson);


                        // Wait for godot to process json
                        Thread.Sleep(100);

                        GameState = new HandleGame(Players);
                    } else
                    {
                        var gameFullMessage = new GameFullMessage()
                        {
                            Status = "Full",
                            message = "Game is full, please wait for the next game."
                        };
                        clientSocket.WriteString(JsonSerializer.Serialize<GameFullMessage>(gameFullMessage));
                        clientSocket.Close();
                    }
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Client aborted connected");
                }
            }
        }

        public static void CloseClient(TcpClient client)
        {
            client.Close();
        }

        public static void RestartGame()
        {
            foreach (KeyValuePair<string, TcpClient> player in Players)
            {
                player.Value.Close();
            }
        }
    }
}
