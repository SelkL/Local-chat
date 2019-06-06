using System;
using System.Collections.Generic; 
using System.Net.Sockets;
using System.Text;

namespace serverChat
{
    public static class Server
    {
        public static List<Client> Clients = new List<Client>();
        public static void NewClient(Socket handle) // подключение нового клиента
        {
            try
            {
                Client newClient = new Client(handle);
                Clients.Add(newClient);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Новенький ^_^:");
                Console.ResetColor();
                Console.WriteLine(" {0}", handle.RemoteEndPoint);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка соединения с новым клиентом: {0}", e.Message);
                Console.ResetColor();
            }
        }


        public static void EndClient(Client client) // отключение клиента
        {
            try
            {
                client.End();
                Clients.Remove(client);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Пользователь {0} вышел из чата", client.UserName);
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка при выходе пользователя: {0}", e.Message);
                Console.ResetColor();
            }
        }


        public static void UpdateAllChats() //обнова всех окон чатов при new message
        {
            try
            {
                int count_users = Clients.Count; //сколько в листе клиентов
                for (int i = 0; i < count_users; i++)
                {
                    Clients[i].UpdateChat(); //из класса Client
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка обновления чатов: {0}", e.Message);
                Console.ResetColor();
            }
        }
        
    }
}
