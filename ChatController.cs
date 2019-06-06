using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    public static class ChatController
    {
        private const int maxMessage = 1000;
        public static List<message> Chat = new List<message>();
        public struct message
        {
            public string userName;
            public string data;
            public message(string name, string msg)
            {
                userName = name;
                data = msg;
            }
        }
        public static void AddMessage(string userName, string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(msg)) return;
                int countMessages = Chat.Count;
                if (countMessages > maxMessage) ClearChat();
                message newMessage = new message(userName, msg);
                Chat.Add(newMessage);
                Console.WriteLine("Новое сообщение {0}", userName);
                Server.UpdateAllChats();
            }
            catch (Exception e) { Console.WriteLine("Ошибка добавлением сообщения: {0}", e.Message); }
        }

        
        public static void ClearChat()
        {
            Chat.Clear();
        }
        

        public static string GetChat()
        {
            try
            {
                string data = "#updatechat&";
                int countMessages = Chat.Count;
                if (countMessages <= 0) return string.Empty;
                for (int i = 0; i < countMessages; i++)
                {
                    data += String.Format("{0}~{1}|", Chat[i].userName, Chat[i].data);//преобразование строки и вставка в другую
                }
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка в getChat: {0}", e.Message);
                return string.Empty;
            }
        }
    }
}
