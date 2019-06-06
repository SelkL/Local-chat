using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace serverChat
{
    public class Client
    {
        private string userName;
        private Socket handler;
        private Thread userThread;
        public Client(Socket socket) //стартует поток клиента и заказываем обработочку
        {
            handler = socket;
            userThread = new Thread(listner);
            userThread.IsBackground = true;
            userThread.Start();
        }
        
        public string UserName // чисто для передачи Server и Program
        {
            get { return userName; }
        }
        
        private void listner()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRec = handler.Receive(buffer); //получает в буффер данные из сокета
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec); // чтобы читабельно было
                    handleCommand(data); //для обработочки
                }
                catch
                {
                    Server.EndClient(this);
                    return;
                } //молча перекрыть клиента
            }
        }


        public void End() 
        {
            try
            {
                handler.Close(); //закрыть сокет
                userThread.Abort(); //закрытие потока
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка закрытия: {0}", e.Message);
            }
        }



        private void handleCommand(string data)
        {
            if (data.Contains("#setname"))
            {
                userName = data.Split('&')[1];
                UpdateChat();
                return;
            }
            if (data.Contains("#newmsg"))
            {
                string message = data.Split('&')[1];
                ChatController.AddMessage(userName,message);
                return;
            }
        }
        public void UpdateChat() //обновить чат отправителя
        {
            string command = ChatController.GetChat();

            try
            {
                int bytesSent = handler.Send(Encoding.UTF8.GetBytes(command));
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка команды: {0}", e.Message); Server.EndClient(this);
            }
        }
    }
}
