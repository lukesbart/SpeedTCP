using System.Net.Sockets;
using System.Text.Json;

namespace TestClient;

public class Program
{
    private static TcpClient client;

    public class sendMessage
    {
        public string? Message { get; set; }
    }

    static void Main()
    {
        Console.Write("Enter a username: ");
        string playerName = Console.ReadLine();

        var processMessage = new sendMessage();

        client = new TcpClient("127.0.0.1", 88);
        client.WriteString(playerName);

        var thread = new Thread(HandleResponse);
        thread.Start();

        while (true)
        {
            try
            {
                string message = Console.ReadLine();
                if (message == "/e")
                {
                    client.Close();
                    System.Environment.Exit(0);
                }
                //processMessage.Message = message;
                //message = JsonSerializer.Serialize<sendMessage>(processMessage);
                client.WriteString(message);
                
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Server closed connection");
                break;
            }
        }
    }

    static void HandleResponse()
    {
        while (true)
        {
            try
            {
                string res = client.ReadString();
                if (res == "")
                {
                    throw new Exception();
                }
                Console.WriteLine(res); 
            } catch
            {
                Console.WriteLine("Server closed connection");
                System.Environment.Exit(0);
                break;
            }
        }
    }
}