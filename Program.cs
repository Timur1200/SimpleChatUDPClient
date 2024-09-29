    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
namespace ConsoleAppClient
{  //клиент
    class Program
    {
        static async Task Main(string[] args)
        {
            UdpClient udpClient = new UdpClient(0); // Автоматически назначаем порт
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Loopback, 11000);

            Console.WriteLine("Введите ваше имя:");
            string userName = Console.ReadLine();

            // Запускаем задачу для прослушивания входящих сообщений
            Task receiveTask = Task.Run(async () =>
            {
                while (true)
                {
                    var receivedResults = await udpClient.ReceiveAsync();
                    string receivedMessage = Encoding.UTF8.GetString(receivedResults.Buffer);
                    Console.WriteLine($"Сообщение от {receivedResults.RemoteEndPoint}: {receivedMessage}");
                }
            });

            while (true)
            {
                Console.WriteLine("Введите сообщение для отправки на сервер:");
                string message = Console.ReadLine();
                string fullMessage = $"{userName}: {message}";

                byte[] messageBytes = Encoding.UTF8.GetBytes(fullMessage);
                await udpClient.SendAsync(messageBytes, messageBytes.Length, serverEndPoint);
                Console.WriteLine("Сообщение отправлено на сервер.");
            }
        }
    }


}