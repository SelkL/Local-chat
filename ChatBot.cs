using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace chatClient
{
    public class ChatBot
    {
        string q, path; //вопрос; путь; ответпользователя (обучение)
        List<string> sempls = new List<string>();//вопрос-ответ

        //генерация ответа
        public int Que(string qw, string pat)
        {
            int h = 1;
            path = pat;
            
            try
            {
                sempls.AddRange(File.ReadAllLines(path));
            }
            catch
            {

            }

            q = qw;
            for (int i = 1; i < sempls.Count; i += 2)
            {
                if (qw == sempls[i])
                {
                    h = 0;
                }
            }

            if (h == 1)
            {
                sempls.Add(qw);
                File.WriteAllLines(path, sempls.ToArray());
            }
            return h;
        }


        public string Ans(string ans, string pat)
        {
            path = pat;
            try
            {
                sempls.AddRange(File.ReadAllLines(path));
            }
            catch
            {

            }

            path = pat;
            sempls.Add(ans);
            File.WriteAllLines(path, sempls.ToArray());
            return ans;
        }

        public string Anss(string ans, string pat)
        {
            ChatBot at = new ChatBot();
            path = pat;

            try
            {
                sempls.AddRange(File.ReadAllLines(path));
            }
            catch
            {

            }

            ans = ans.ToLower();
            for (int i = 1; i < sempls.Count; i += 2)
            {
                if (ans == sempls[i])
                {
                    return at.Mes(true, ans);
                }
            }

            return at.Mes(false, "");
        }

        public string Mes(bool flag, string ans)
        {
            string str;
            if (flag)
            {
                str = ans;
            }
            else
            {
                str = "введите снова: ";
            }
            return str;
        }

    }
}
