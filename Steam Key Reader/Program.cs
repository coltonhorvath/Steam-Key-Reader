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
        static void Main()
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            Uri URL = new Uri("https://reddit.com");
            webClient.OpenReadAsync(URL);
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
                const string pattern = @"([help])";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                TextReader tR = new StreamReader(e.Result);
                string content = tR.ReadToEnd();
                MatchCollection mC = regex.Matches(content);

                foreach (Match match in mC)
                {
                    Console.WriteLine(match.Value);
                }
                tR.Close();
                Console.WriteLine("Done");
                Console.ReadKey();
            }
        }
    }
}
