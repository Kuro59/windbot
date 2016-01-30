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
using System.Timers;

namespace WindBot
{
    public class Program
    {
        public const short ProVersion = 0x1336;

        public static Random Rand;
        public static string Version = "v0.3.0.1";
        public static TcpClient clientSocket = new TcpClient();
        public static NetworkStream serverStream;
        public static int port;

        public static bool connected = false;

        public static string pseudo = "Kaibot";
        public static string mdp = "BotBCA";
        public static string Endder = "\r\n";

        public static List<GameClient> Game = new List<GameClient>();
        public static string[] Deck;

        private static System.Timers.Timer timer;

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        
        internal static void Main()
        {
            string message = string.Empty;
            Random randNum = new Random();
            int nombre = randNum.Next(9);

            switch (nombre)
            {
                case 0:
                    message = "Topic Officiel des Echanges : http://www.otk-expert.fr/forum/?action=viewtopic&t=17649";
                    break;

                case 1:
                    message = "N'oubliez pas d'aller visiter le forum de l'académie de duel ! http://ygo-academie.com/index.php";
                    break;

                case 2:
                    message = "Topic Officiel des Bugs : http://ygo-academie.clicforum.fr/f180-Bugs-et-Suggestions.htm";
                    break;

                case 3:
                    message = "Topic Officiel des Suggestions : http://ygo-academie.clicforum.fr/f180-Bugs-et-Suggestions.htm";
                    break;

                case 4:
                    message = "Des problèmes sur un deck ? Venez consulter nos analyses ou demander de l'aide ici -> http://ygo-academie.clicforum.fr/index.php";
                    break;

                case 5:
                    message = "Venez vous tenir au courant des dernierers news du jeu sur : http://battlecityalpha.fr.nf/BCA/Blog/";
                    break;

                case 6:
                    message = "Vous cherchez à gagner des points : http://battlecityalpha.fr.nf/BCA/GetPoints.html";
                    break;

                case 7:
                    message = "Rejoignez la communauté de notre partenaire OTK-Expert ! C'est ici : http://otk-expert.fr/";
                    break;

                case 8:
                    message = "Le jeu n'est pas gratuit, nous avons des frais de serveurs, nom de domaines etc ! Alors si vous voulez améliorer VOTRE expérience de jeu et vous aussi participer à Battle City Alpha ! http://battlecityalpha.xyz/Donation.html";
                    break;
            }
            EnvoyerMsg("bot|" + message);
        }

        public static void Main(string[] args)
        {

            UselessThings();
            timer = new System.Timers.Timer();
            timer.Interval = 3600 * 1000;
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
            timer.Start();
            Rand = new Random();
            CardsManager.Init();
            DecksManager.Init();
            Deck = new[] { "OldSchool", "Horus", "Exodia", "DamageBurn", "Dragunity", "Zexal Weapons", "Rank V" , "CyberDragon", "Exodia", "Corrompu", "BlueEyes", "Protecteurs", "Nekroz", "Blackwing"};
            try
            {
                clientSocket.Connect("127.0.0.1", 9001);
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

<<<<<<< HEAD
        private static void Run(string info)
        {
            int DeckNum = Rand.Next(13);
#if !DEBUG
            GameClient clientA = new GameClient("Kaibot", Deck[DeckNum], "127.0.0.1", port, info);
#endif
#if DEBUG
            GameClient clientA = new GameClient("Kaibot", Deck[DeckNum], "127.0.0.1", port, info);            
#endif
            clientA.Start();
            Console.WriteLine("Connexion réussi au port : " + port + " Deck -> " + Deck[DeckNum]);
            Game.Add(clientA);
        }
=======
        public static void Init(string databasePath)
        {
            Rand = new Random();
            DecksManager.Init();
            InitCardsManager(databasePath);
        }
        
        private static void Run()
        {
            Init("cards.cdb");
>>>>>>> 2241def41d8259ce023e94cede8593c51925cc93

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

            try
            {
                List<GameClient> DuelFinished = new List<GameClient>();
                while (connected)
                {
                        foreach (GameClient DuelBot in Game)
                        {
                            if (!DuelBot.Connection.IsConnected)
                                DuelFinished.Add(DuelBot);
                            else
                                DuelBot.Tick();
                        }

                    while (DuelFinished.Count > 0)
                    {
                        Game.Remove(DuelFinished[0]);
                        DuelFinished.Remove(DuelFinished[0]);
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Vous avez été déconnecté du serveur !" + Environment.NewLine + ex.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        static public void Actualiser()
        {
            List<string> PacketComplet = new List<string>();
            List<string> PacketIncomplet = new List<string>();
            List<GameClient> DuelFinished = new List<GameClient>();
            while (connected)
            {
                try
                {
                    byte[] inStream = new byte[120000];
                    int quantitelue = serverStream.Read(inStream, 0, inStream.Length);
                    string returndata = System.Text.Encoding.UTF8.GetString(inStream, 0, quantitelue);

                    int PackLength = returndata.IndexOf(Endder);
                    int StartIndex = 0;
                    while (PackLength != 0)
                    {
                        if (PacketIncomplet.Count > 0)
                        {
                            while (PacketIncomplet.Count > 0)
                            {
                                if (PackLength != -1)
                                {
                                    PacketIncomplet[0] = PacketIncomplet[0] + returndata.Substring(StartIndex, PackLength - StartIndex);
                                    PacketComplet.Add(PacketIncomplet[0]);
                                    PacketIncomplet.Remove(PacketIncomplet[0]);
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            if (PackLength != -1)
                                PacketComplet.Add(returndata.Substring(StartIndex, PackLength - StartIndex));
                            else if (PackLength == -1)
                            {
                                PacketIncomplet.Add(returndata.Substring(StartIndex).Trim());
                                break;
                            }
                        }
                        StartIndex = PackLength + 2;
                        if (StartIndex > returndata.Length)
                            break;
                        PackLength = returndata.IndexOf(Endder, StartIndex);
                    }

                    while (PacketComplet.Count > 0)
                    {
                        PacketHandler(PacketComplet[0]);
                        PacketComplet.Remove(PacketComplet[0]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Vous avez été déconnecté du serveur !" + Environment.NewLine + ex.ToString());
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }
        private static void PacketHandler(string Packet)
        {
            string[] valid = Packet.Split('|');
            switch (valid[0])
            {
                case "salt":
                    EnvoyerMsg("id|" + pseudo + "|" + sha256_hash(mdp) + "#" + sha256_hash(valid[1]) + "|" + GetKEY() + "|");
                    break;

                case "id":
                    if (valid[1] == "ok")
                        Console.WriteLine("Bot connecté.");
                    else
                        Console.WriteLine("Identification échouée.");
                    EnvoyerMsg("ReadyToLoad|");
                    break;

                case "pm":
                    string[] info = valid[1].Split(' ');
                    if (info[0] == "gpoint")
                    {
                        if (valid[2] == "♝ Altor" || valid[2] == "♞ BlackArrow")
                        {
                            EnvoyerMsg("givepoint|" + info[1] + "|" + info[2]);
                        }
                    }
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
                    break;
            }
        }

        static public void EnvoyerMsg(string message)
        {
            try
            {
                byte[] outStream = System.Text.Encoding.UTF8.GetBytes(message + Endder);
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
                byte[] outStream = System.Text.Encoding.UTF8.GetBytes("pm|" + message + "|" + pseudo + "|" + Endder);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

<<<<<<< HEAD
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
=======
        private static void InitCardsManager(string databasePath)
        {
            string currentPath = Path.GetFullPath(".");
            string absolutePath = Path.Combine(currentPath, databasePath);
            NamedCardsManager.Init(absolutePath);
>>>>>>> 2241def41d8259ce023e94cede8593c51925cc93
        }
    }
}
