using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        private static string serverIP = "127.0.0.1";
        private static int port = 12345;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.Write("Введіть англійське слово або речення: ");
                string input = Console.ReadLine();

                try
                {
                    using (TcpClient client = new TcpClient(serverIP, port))
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(input);
                        stream.Write(data, 0, data.Length);

                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Console.WriteLine($"Переклад: {response}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
        }
    }
}