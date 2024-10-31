using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DeepL;

namespace Server
{
    class Server
    {
        private static string authKey = "API here";
        private static Translator translator = new Translator(authKey);
        private static int port = 12345;

        static async Task Main(string[] args)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(localAddr, port);
            server.Start();
            Console.WriteLine($"Сервер запущено на порту {port}");

            TcpClient client = await server.AcceptTcpClientAsync();
            Console.WriteLine("Підключено клієнта");

            using (NetworkStream stream = client.GetStream())
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    try
                    {
                        DeepL.Model.TextResult translationResult = await translator.TranslateTextAsync(data, LanguageCode.English, LanguageCode.Ukrainian);
                        string translatedText = translationResult.Text;
                        byte[] response = Encoding.UTF8.GetBytes(translatedText);
                        await stream.WriteAsync(response, 0, response.Length);
                    }
                    catch (Exception ex)
                    {
                        byte[] errorResponse = Encoding.UTF8.GetBytes($"Помилка: {ex.Message}");
                        await stream.WriteAsync(errorResponse, 0, errorResponse.Length);
                    }
                }

                Console.WriteLine("З'єднання з клієнтом закрито.");
            }
        }
    }
}
