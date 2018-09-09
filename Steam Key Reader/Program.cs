﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using SteamKit2;
using RedditSharp;

namespace Steam_Key_Reader
{
    class Program
    {
        static string url = "https://twitter.com/madelineshook";
        static int counter = 0;
        static SteamClient SteamClient;
        static CallbackManager manager;
        static SteamUser steamUser;
        static bool isRunning;
        static string user, pass;
           
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            Uri URL = new Uri(url);
            webClient.OpenReadAsync(URL);
            //Console.ReadKey();

            if (args.Length < 2)
            {
                Console.WriteLine("Sample1: No username and password specified!");
                return;
            }

            user = args[0];
            pass = args[1];

            SteamClient = new SteamClient();
            manager = new CallbackManager(SteamClient);
            steamUser = SteamClient.GetHandler<SteamUser>();
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            isRunning = true;

            Console.WriteLine("Connecting to Steam...");

            SteamClient.Connect();
            Console.WriteLine("big test");

            while (isRunning)
            {
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
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
                const string pattern = "(kejrgnk)";
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
            }
        }

        static void OnConnected(SteamClient.ConnectedCallback callback)
        {
            user = "coltonhorvath";
            pass = "ndp6r6ndp6r6";
            Console.WriteLine("Connected to Steam! Logging in '{0}'...", user);

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = user,
                Password = pass,
            });
        }

        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine("Disconnected from Steam");

            isRunning = false;
        }

        static void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback.Result != EResult.OK)
            {
                if (callback.Result == EResult.AccountLogonDenied)
                {
                    Console.WriteLine("Unable to logon to Steam: This account is SteamGuard protected.");

                    isRunning = false;
                    return;
                }

                Console.WriteLine("Unable to logon to Steam: {0} / {1}", callback.Result, callback.ExtendedResult);

                isRunning = false;
                return;
            }

            Console.WriteLine("Successfully logged on!");
            steamUser.LogOff();
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            Console.WriteLine("Logged off of Steam: {0}", callback.Result);
        }
    }
}
