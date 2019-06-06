using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace serverChat
{
    public class Program
    {
        private const string localhost = "localhost";
        private const int serverPort = 9933;
        private static Thread serverThread; //сервашные потоки


        public static int Main()
        {
            string str = "sqmes";
            Program p = new Program();

            serverThread = new Thread(startServer); 
            serverThread.IsBackground = true;
            serverThread.Start(); //поток сервака
            int lp = p.SM(str);

            while (true)
            {
                handlerCommands(Console.ReadLine()); // ждем сообщения
                str = "sqmes_1";
            }
        }

        public int SM(string str)
        {
            int i = 0;
            if (str == "sqmes")
            {
                i++;
            }
            return i;
        }

        public static void handlerCommands(string cmd) //обработка команды
        {
            cmd = cmd.ToLower(); 
            if (cmd.Contains("/getusers")) 
            {
                int count_users = Server.Clients.Count();
                for (int i = 0; i < count_users; i++)
                {
                    Console.WriteLine("{0}: {1}", i, Server.Clients[i].UserName);
                }
            }
        }


        private static void startServer() // ачало работы сервака; адресовка; 
        {
            IPHostEntry ip_host = Dns.GetHostEntry(localhost);
            IPAddress ip_address = ip_host.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ip_address, serverPort);
            Socket socket = new Socket(ip_address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndPoint);
            socket.Listen(1000);
            Console.WriteLine("Запущен сервер с IP: {0}", ipEndPoint);
            while(true) // list пользователей
            {
                try
                {
                    Socket user = socket.Accept();
                    Server.NewClient(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка создания клиента: {0}", e.Message);
                }
            }

        }
    }
}
