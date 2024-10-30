using System;
using System.Net.Sockets;
using System.Text;

public class Client
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        try
        {
            string serverIp = "127.0.0.1";
            int serverPort = 80;

            TcpClient client = new TcpClient(serverIp, serverPort);

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            string reversedMessage = Reverse(message);

            Console.WriteLine("Сервер: " + message);
            Console.WriteLine("Перегорнуте повідомлення: " + reversedMessage);

            stream.Close();
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);
        }

        Console.ReadKey();
    }

    static string Reverse(string str)
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}