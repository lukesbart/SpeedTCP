using System;
using System.Net.Sockets;
using System.Text.Json;

namespace SpeedServer
{
// Maintain game state and handle broadcasts off of that

	public class HandleGame
	{
		public static Dictionary<string, TcpClient>? _players;
		public static string? _message;
		public static Game.Game? GamePlay;

		public HandleGame(Dictionary<string, TcpClient> players) 
		{
			_players = players;

			Console.WriteLine("Game engine starting");

			GamePlay = new Game.Game();
			GameState();
		}

		public bool HandleMove(int playerCount, String[] move)
		{
			var moveValid = GamePlay.ValidateMove(playerCount, move);
			if (moveValid)
			{
				GamePlay.Move(move, playerCount);
			}
			return moveValid;
		}

		public void GameState()
		{
			GamePlay.CheckWinner();
			var gameStateMessage = GamePlay.ShowGameState();
			var gameStateJson = JsonSerializer.Serialize<Game.GameState>(gameStateMessage);
			Console.WriteLine(gameStateJson);
			_players.Broadcast(gameStateJson);
        }
	}
}

