using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace TestClient
{
	public class gdclient
	{
		string _clientName;
		TcpClient _client;

		public class GameState
		{
			bool Playing;
			string? Winner;
			List<string>? player1;
			List<string>? player2;
			string? discard1;
			string? discard2;
		}

		public gdclient(string name, string ip, int port)
		{
			_clientName = name;
			_client = new TcpClient(ip, port);

			WriteString(_client, _clientName);

			var thread = new Thread(HandleRes);
			thread.Start();
		}

		public void HandleRes()
		{
			while (true)
			{
				try
				{
					var jRes = _client.ReadString();
					JsonSerializer.Deserialize<GameState>(jRes);
				} catch
				{
					Console.WriteLine("Error");
				}
			}
		}

		public void WriteString(TcpClient client, string msg)
		{
            var bytes = Encoding.ASCII.GetBytes(msg);
            var stream = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }

		public string ReadString(TcpClient client)
		{
            var stream = client.GetStream();
            var bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, bytes.Length);
            var msg = Encoding.ASCII.GetString(bytes);
            return msg.Substring(0, msg.IndexOf("\0", StringComparison.Ordinal));
        }
	}
}

