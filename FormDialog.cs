using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace chatClient
{
    public partial class chatForm : Form
    {
        private delegate void printer(string data);
        private delegate void cleaner();
        printer Printer;
        cleaner Cleaner;
        private Socket serverSocket;
        private Thread clientThread;
        private const string serverHost = "localhost";
        private const int serverPort = 9933;

        public chatForm()
        {
            InitializeComponent();
            Printer = new printer(print);
            Cleaner = new cleaner(clearChat);
            connect();
            clientThread = new Thread(listner);
            clientThread.IsBackground = true;
            clientThread.Start();
        }


        private void listner() 
        {
            while (serverSocket.Connected)
            {
                byte[] buffer = new byte[8196];
                int bytesRec = serverSocket.Receive(buffer);
                string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
                if (data.Contains("#updatechat"))
                {
                    UpdateChat(data); //обнова чата
                    continue;
                }
            }
        }
        private void connect()
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(serverHost);
                IPAddress ipAddress = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, serverPort);
                serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Connect(ipEndPoint);
            }
            catch { print("Сервер недоступен!"); }
        }

        private void clearChat()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(Cleaner);
                return;
            }
            chatBox.Clear();
        }
        private void UpdateChat(string data)
        {
            //#updatechat&userName~data|username~data
            clearChat();
            string[] messages = data.Split('&')[1].Split('|');
            int countMessages = messages.Length;
            if (countMessages <= 0) return;
            for (int i = 0; i < countMessages; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(messages[i])) continue;
                    print(String.Format("{0} : {1}", messages[i].Split('~')[0], messages[i].Split('~')[1]));
                }
                catch { continue; }
            }
        }
        private void send(string data) //отправка на сервер
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int bytesSent = serverSocket.Send(buffer);
            }
            catch
            {
                print("Связь с сервером прервалась...");
            }
        }
        private void print(string msg)
        {

            if (this.InvokeRequired)
            { 
                this.Invoke(Printer, msg);
                return;
            }
            if (chatBox.Text.Length == 0)
                chatBox.AppendText(msg);
            else
                chatBox.AppendText(Environment.NewLine + msg);
        }

        private void enterChat_Click(object sender, EventArgs e)
        {
            string Name = userName.Text;
            if (string.IsNullOrEmpty(Name)) return;
            send("#setname&" + Name); // отправка имени на сервер
            chatBox.Enabled = true;
            chat_msg.Enabled = true;
            chat_send.Enabled = true;
            userName.Enabled = false;
            enterChat.Enabled = false;
        }

        private void chat_send_Click(object sender, EventArgs e)
        {
            sendMessage();
        }

        public static bool flag = false;
        private void sendMessage()
        {
            ChatBot bot = new ChatBot();
            try
            {
                int c = 0;
                string data = chat_msg.Text;
                if (string.IsNullOrEmpty(data)) return;
                if (data.Contains("Крио") || data.Contains("крио"))//вопрос точно должен содержать один знак или цифру(главное чтобы все не были буквамии пробелами!!!!)
                {
                    char[] ch = data.ToCharArray();
                    for (int i = 0; i < ch.Length; i++)
                    {
                        if ((Char.IsLetter(ch[i]) && ch[i] >= 'А' && ch[i] <= 'я') || (Char.IsLetter(ch[i]) && ((ch[i] == 'Ё') || (ch[i] == 'ё'))) || ch[i] == ' ')
                        {
                            c++;
                        }
                    }
                    if (c == ch.Length - 1)
                    {
                        if (bot.Que(data, @"C:\Users\Люба\Desktop\chatClient\question_answer.txt") == 1)
                        {
                            flag = true;//запись ответа сразу без проверок
                            send("#newmsg&" + data + "введите ответ: ");
                        }
                        else
                        {
                            flag = false;
                            send("#newmsg&" + data);
                        }
                    }
                    if (c != ch.Length - 1 && flag == true) //ответа в базе нет
                    {
                        send("#newmsg&" + "ответ бота " + bot.Ans(data, @"C:\Users\Люба\Desktop\chatClient\question_answer.txt"));
                    }
                    if (c != ch.Length - 1 && flag == false) //ответ есть
                    {
                        send("#newmsg&" + "ответ бота " + bot.Anss(data, @"C:\Users\Люба\Desktop\chatClient\question_answer.txt"));
                    }
                }
                else
                {
                    send("#newmsg&" + data);//отправка сообщения на сервер
                }
                chat_msg.Text = string.Empty;
            }
            catch { MessageBox.Show("Ошибка при отправке сообщения!"); }
        }

        private void chatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                sendMessage();
        }

        private void chat_msg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                sendMessage();
        }

        private void Gui_userName_Click(object sender, EventArgs e)
        {

        }

        private void VScrollBar1_Scroll(object sender, ScrollEventArgs e1)
        {
        }

        private void ChatBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Chat_msg_TextChanged(object sender, EventArgs e)
        {

        }

        private void UserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void Gui_chat_Click(object sender, EventArgs e)
        {

        }
    }
}
