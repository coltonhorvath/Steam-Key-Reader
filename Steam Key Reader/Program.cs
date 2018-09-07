using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using SteamKit2;

namespace Steam_Key_Reader
{
    class Program
    {
        static string url = "https://twitter.com/madelineshook";
        static int counter = 0;

        static void Main()
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            Uri URL = new Uri(url);
            webClient.OpenReadAsync(URL);
            arrayTest();
            Console.ReadKey();
        }

        public static void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine(e.Error.Message);
            }
            else
            {
                //@"a href=""(?<link>.+?)"""
                const string pattern = "retweeted";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                TextReader tR = new StreamReader(e.Result);
                string content = tR.ReadToEnd();
                MatchCollection mC = regex.Matches(content);

                foreach (Match match in mC)
                {
                    //Console.WriteLine(match.Value);
                    Console.WriteLine(match.Value);
                    counter += 1;
                    Console.WriteLine(counter);
                }
                tR.Close();
                Console.WriteLine("Done");
                Console.ReadKey();
            }
        }

        public static void arrayTest()
        {
            string test = "This is a test string, 102";
            string[] array = test.Split(',');
            foreach (string testy in array)
            {
                Console.WriteLine(testy);
            }
        }
    }
}
