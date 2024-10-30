using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{
    TcpListener Listener;

    public Server(int Port)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Listener = new TcpListener(IPAddress.Any, Port);
        Listener.Start();

        Console.WriteLine("Сервер увімкненй...");

        while (true)
        {
            TcpClient client = Listener.AcceptTcpClient();
            Console.WriteLine("Кліент під'єднанний!");

            Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
            clientThread.Start(client);
        }
    }

    private void HandleClient(object clientObject)
    {
        TcpClient client = (TcpClient)clientObject;
        NetworkStream stream = client.GetStream();

        string message = "Hello";
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Console.WriteLine("Відправленно до клієнта " + message);

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