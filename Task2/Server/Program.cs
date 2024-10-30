using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        TcpListener Listener;

        public Server(int Port)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start();

            Console.WriteLine("Сервер увімкнений...");

            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                Console.WriteLine("Клієнт під'єднаний!");

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        private void HandleClient(object clientObject)
        {
            TcpClient client = (TcpClient)clientObject;
            NetworkStream stream = client.GetStream();

            int[] numbers = { 5, 2, 8, 1, 9, 4, 7, 3, 6 };
            string message = string.Join(",", numbers);
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Відправлено до клієнта: " + message);

            data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            string sortedMsg = Encoding.UTF8.GetString(data, 0, bytesRead);
            Console.WriteLine("Отримано від клієнта (відсортований): " + sortedMsg);

            stream.Close();
            client.Close();
            Console.WriteLine("З'єднання з клієнтом від'єднано");
        }

        ~Server()
        {
            if (Listener != null)
            {
                Listener.Stop();
            }
        }

        static void Main(string[] args)
        {
            new Server(80);
        }
    }
}