using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
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

                Console.WriteLine("Сервер: " + message);

                int[] numbers = message.Split(',').Select(int.Parse).ToArray();
                Array.Sort(numbers);
                string sortedMsg = string.Join(",", numbers);

                byte[] data = Encoding.UTF8.GetBytes(sortedMsg);
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Відправлено на сервер (відсортований): " + sortedMsg);

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }

            Console.ReadKey();
        }
    }
}
