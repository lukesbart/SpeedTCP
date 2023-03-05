using System;
using System.Net.Sockets;
using System.Text.Json;

namespace SpeedServer
{
	internal class HandlePlayer
	{
		private TcpClient? clientSocket;
		private string? clientName;
		private bool connected;
		private bool acked = false;

		public class player
		{
			public string? Name { get; set; }
			public string? Message {get; set;}
		}

		public class playerState
		{
			public string? Message { get; set; }
		}

		public class initMessage
		{
			public string? Status { get; set; }
			public int? playerNumber { get; set; }
			public string? message { get; set; }
		}

		public class playerAck
		{
			public bool acked { get; set; }
		}

		private player? PlayerT;


		public void StartClient(TcpClient client, string name)
		{
			PlayerT = new player { Name = name.Trim('\n') }; 
			clientSocket = client;
			clientName = name;
			connected = true;
			Program.clientCount++;

			var welcomeMessage = new initMessage { Status = "Setup", playerNumber = Program.PlayerNames.IndexOf(clientName) + 1, message = "You are player number " + (Program.PlayerNames.IndexOf(clientName) + 1)};

			var welcomeMessageJson = JsonSerializer.Serialize<initMessage>(welcomeMessage);

			Thread.Sleep(100);

			clientSocket.WriteString(welcomeMessageJson);

			Thread.Sleep(100);

            var thread = new Thread(DoPlayer);
			thread.Start();
		}

		

		private void DoPlayer()
		{
			while(connected)
			{
				try
				{	
                    string clientData = clientSocket.ReadString().Trim('\n');
					Console.WriteLine(clientData);

					var move = JsonSerializer.Deserialize<Game.PlayerState>(clientData);
					Program.GameState.HandleMove(Program.PlayerNames.IndexOf(clientName), move.Move);
					
					Console.WriteLine(PlayerT.Name + " made a move " + move.Move[0] + " to " + move.Move[1]);

					Program.GameState.GameState();

					//Prevent infinite loop of printing blank strings
					if (clientData == "" || clientData == "\n")
					{
						throw new Exception();
					}

					var playerMessage = JsonSerializer.Deserialize<playerState>(clientData);
					PlayerT.Message = playerMessage.Message;

					Console.WriteLine(PlayerT.Message + "\n");
				}
				catch (IOException e)
				{
					Console.WriteLine(e.Message);
					HandleDisconnect();
					break;
				}
				catch (SocketException)
				{
					HandleDisconnect();
                    break;
                }
				catch (JsonException e)
				{
					if (e.Message.Contains("The input does not contain any JSON tokens."))
					{
						HandleDisconnect();
						break;
					}
					clientSocket.WriteString("Invalid move");
				}
				catch (Exception)
				{
					HandleDisconnect();
					break;
				}
			}
		}

		public void HandleDisconnect()
		{
			connected = false;
			Program.clientCount--;
            Console.WriteLine(PlayerT.Name + " left. \n There are now " + Program.clientCount + " person(s) here");
			Program.Players.Remove(PlayerT.Name);
			Program.PlayerNames.Remove(PlayerT.Name);
			this.clientSocket.Close();
			Program.RestartGame();
        }
	}
}