using System;
using System.Net.Sockets;
using System.Text;

namespace SpeedServer
{
	public static class HandleStreams
	{
		public static string ReadString(this TcpClient client)
		{
            var stream = client.GetStream();
            var bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, bytes.Length);
            var msg = Encoding.ASCII.GetString(bytes);
            return msg.Substring(0, msg.IndexOf("\0", StringComparison.Ordinal));
        }

		public static void WriteString(this TcpClient client, string msg)
		{
            var bytes = Encoding.ASCII.GetBytes(msg);
            var stream = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }

        public static void Broadcast(this Dictionary<string, TcpClient> clients, string msg)
        {
            foreach (TcpClient client in clients.Values)
            {
                client.WriteString(msg);
            }
        }
	}
}

