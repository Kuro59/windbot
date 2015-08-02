using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using WindBot.Game;
using WindBot.Game.AI;
using WindBot.Game.Data;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Linq; 

namespace WindBot
{
    public class Program
    {
        public const short ProVersion = 0x1335;

        public static Random Rand;
        public static string Version = "v0.0.0.4";
        public static TcpClient clientSocket = new TcpClient();
        public static NetworkStream serverStream;
        public static int port;

        public static bool connected = false;

        public static string pseudo = "Kaibot";
        public static string mdp = "X";

        public static List<GameClient> Game = new List<GameClient>();
        public static string[] Deck;

        public static void Main(string[] args)
        {

            UselessThings();
            Rand = new Random();
            CardsManager.Init();
            DecksManager.Init();
            Deck = new []{ "Horus","OldSchool","Frog","Dragunity","DamageBurn", "Blackwing", "Exodia", "Nekroz", "Corrompu", "Protecteurs", "BlueEyes", "CyberDragon"};
            try
            {
                clientSocket.Connect("127.0.0.1", 1234);
                connected = true;
                serverStream = clientSocket.GetStream();
                Thread actualiser = new Thread(Actualiser);
                actualiser.IsBackground = true;
                actualiser.Start();
                Thread duel = new Thread(Duel);
                duel.IsBackground = true;
                duel.Start();
                EnvoyerMsg("salt|");
                string Cmd = Console.ReadLine();
                if (Cmd == "stop")
                    EnvoyerMsg("deco|");
                else
                    Console.ReadLine();
            }
            catch (Exception ex)
            {
                connected = false;
                Console.WriteLine("Erreur :" + ex.ToString());
                return;
            } 
        }

        private static void Run(string info)
        {
            int DeckNum = Rand.Next(11);
            #if !DEBUG
                GameClient clientA = new GameClient("Kaibot", Deck[DeckNum], "127.0.0.1", port, info);
            #endif
            #if DEBUG
            GameClient clientA = new GameClient("Kaibot", Deck[11], "127.0.0.1", port, info);            
            #endif
            clientA.Start();
            Console.WriteLine("Connexion réussi au port : " + port + " Deck -> " + Deck[11]);
            Game.Add(clientA);
        }

        private static void Run(string pseudo, string deck, string info)
        { 
#if !DEBUG
                GameClient clientA = new GameClient(pseudo, deck, "127.0.0.1", port, info);
#endif
#if DEBUG
            GameClient clientA = new GameClient(pseudo, deck, "127.0.0.1", port, info);
#endif
            clientA.Start();
            Console.WriteLine("Connexion réussi au port : " + port + " Pseudo -> " + pseudo + " Deck -> " + deck);
            Game.Add(clientA);
        }

        private static void UselessThings()
        {
            Console.Title = "Kaibot " + Version;

            Console.WriteLine(@" __  __         __ __           __   ");
            Console.WriteLine(@"|  |/  |.---.-.|__|  |--.-----.|  |_  ");
            Console.WriteLine(@"|     < |  _  ||  |  _  |  _  ||   _|");
            Console.WriteLine(@"|__|\__||___._||__|_____|_____||____| ");
            Console.WriteLine(@"");
            Console.WriteLine(@"© Tic-Tac-Toc                                                  Version " + Version);
            Console.WriteLine(@"");


        }

        static public void Duel()
        {
            while (connected)
            {
                try
                {
                    foreach (GameClient DuelBot in Game)
                    {
                        if ((!DuelBot.Connection.IsConnected || DuelBot.Connection == null) && DuelBot.Connection.HasJoined)
                        {
                            Game.Remove(DuelBot);
                        }
                        else
                        {
                            DuelBot.Tick();
                        }
                    }
                    Thread.Sleep(1);
                }
                catch (Exception)
                {
                }
            }
        }

        static public void Actualiser()
        {            
            while (connected)
            {
                foreach (GameClient DuelBot in Game)
                {
                    if (!DuelBot.Connection.IsConnected)
                    {
                        Game.Remove(DuelBot);
                    }
                    else
                    {
                        DuelBot.Tick();
                    }
                }
                try
                {                 

                    byte[] inStream = new byte[10025];
                    int quantitelue = serverStream.Read(inStream, 0, inStream.Length);
                    string returndata = System.Text.Encoding.UTF8.GetString(inStream, 0, quantitelue);
                    string[] valid = returndata.Split('|');


                    switch (valid[0])
                    {
                        case "salt":
                            EnvoyerMsg("id|" + pseudo + "|" + sha256_hash(mdp + valid[1]) + "|" + GetKEY() + "|");
                            break;

                        case "id":
                            if (valid[1] == "ok")
                                Console.WriteLine("Bot connecté.");
                            else
                                Console.WriteLine("Identification échouée.");
                            break;

                        case "request":
                            if (valid[1] == "duel")
                            {
                                if (valid[5] != "0" || valid[4] != "TCG" || valid[3] != "Single")
                                {
                                    EnvoyerPM("Je n'accepte que les duel TCG Single Non Classé, réinvitez moi avec ces conditions pour que cela fonctionne", valid[2]);
                                    break;
                                }
                                EnvoyerMsg("requestreponse|oui|" + valid[2] + "|" + valid[3] + "|" + valid[4] + "|" + "0" + "|");  
                            }
                            break;

                        case "DuelStart":
                            port = Convert.ToInt32(valid[1]);
                            Run("000");
                            break;

                        case "bot":
                            if (valid[1] == "stop")
                                Environment.Exit(0);
                            else if (valid[1] == "msg")
                                EnvoyerMsg("bot|" + valid[2]);
                            else if (valid[1] == "TagDuel")                            
                            {
                                Thread.Sleep(5000);
                                port = Convert.ToInt32(valid[3]);
                                switch (valid[2])
                                {
                                    case "Crow":
                                        Run("Crow", "Blackwing", "002");
                                        break;
                                    case "Yugi":
                                        Run("Yugi", "OldSchool", "002");
                                        break;
                                    case "Kaiba":
                                        Run("Kaiba", "BlueEyes", "002");
                                        break;
                                    case "Zen":
                                        Run("Zen", "CyberDragon", "002");
                                        break;
                                    case "Paradoxe":
                                        Run("Paradoxe", "Corrompu", "002");
                                        break;
                                    case "Marik":
                                        Run("Marik", "Protecteurs", "002");
                                        break;
                                    default:
                                        Run("002");
                                        break;
                                }
                                Thread.Sleep(100);
                                Run("Yusei", "Dragunity", "002");
                                Run("Joey", "Horus", "002");                            
                            }
                            break;
                    }
                    returndata.Remove(0);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Vous avez été déconnecté du serveur !" + Environment.NewLine + ex.ToString());
                    Console.ReadKey();
                    Environment.Exit(0);
                }



            }
        }

        static public void EnvoyerMsg(string message)
        {
            try
            {
                byte[] outStream = System.Text.Encoding.UTF8.GetBytes(message);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static public void EnvoyerPM(string message, string pseudo)
        {
            try
            {
                byte[] outStream = System.Text.Encoding.UTF8.GetBytes("pm|" + message + "|" + pseudo + "|");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static string GetKEY()
        {
            if (RegEditor.Read("Software\\BCA\\", "KEY") != null)
                return RegEditor.Read("Software\\BCA\\", "KEY");

            else
                Console.WriteLine("Erreur de registre..");
            return "Erreur";
        }

        public static String sha256_hash(String value)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }
    }
}
