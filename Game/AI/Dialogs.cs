using System.Collections.Generic;

namespace WindBot.Game.AI
{
    public class Dialogs
    {
        private GameClient _game;

        private string[] _duelstart;
        private string[] _newturn;
        private string[] _endturn;
        private string[] _directattack;
        private string[] _attack;
        private string[] _activate;
        private string[] _summon;
        private string[] _setmonster;
        private string[] _chaining;
        
        public Dialogs(GameClient game)
        {
            _game = game;
            _duelstart = new[]
                {
                    "Bonne chance, amusons nous. Je suis en version alpha, et mes decks aussi. Merci pour votre compréhension",

                };
            _newturn = new[]
                {
                    "C'est mon tour, je pioche !.",
                    "Mon tour, et je pioche...",
                    "Je pioche !"
                };
            _endturn = new[]
                {
                    "Je termine mon tour.",
                    "Mon tour est terminé.",
                    "A toi."
                };
            _directattack = new[]
                {
                    "{0}, attaque direct!",
                    "{0}, attaque le directement.",
                    "{0}, il est sans défense, attaque!",
                    "{0}, attaque ses points de vie.",
                    "{0}, attaque ses points de vie directement.!",
                    "{0}, attaque le grace à une attaque direct!",
                    "{0}, utilise toute ta puissance contre ses points de vies!",
                    "{0}, il est faible attaque!",
                    "Mon {0} va donner une claque à tes points de vies!",
                    "Montre lui ton pouvoir, attaque {0}!",
                    "Tu ne peux m'arrêter. {0}, attaque."
                };
            _attack = new[]
                {
                    "{0}, attaque ce {1}!",
                    "{0}, détruis ce {1}!",
                    "{0}, c'est {1} qu'il faut attaquer!",
                    "{0}, élimine ce {1}!",
                    "{0}, montre ton pouvoir à {1}!"
                };
            _activate = new[]
                {
                    "Je vais activer {0}.",
                    "Je vais utiliser l'effet de {0}.",
                    "J'utilise le pouvoir de {0}."
                };
            _summon = new[]
                {
                    "J'invoque {0}.",
                    "Viens, {0}!",
                    "Apparait, {0}!",
                    "J'invoque le puissant {0}.",
                    "J'appelle {0} pour ce battre!",
                    "C'est à toi, {0}.",
                    "C'est parti pour l'invoquation de {0}."
                };
            _setmonster = new[]
                {
                    "Je prépare ma défense.",
                    "Je pose un monstre.",
                    "Je pose un monstre face cacher."
                };
            _chaining = new[]
                {
                    "Regarde ça ! J'active {0}.",
                    "J'utilise le pouvoir de {0}.",
                    "Prépare toi! J'active {0}.",
                    "Je ne crois pas. {0}, active toi!",
                    "Il me semble que tu as oublier ma carte, {0}.",
                    "As-tu pris en compte le fait que j'ai en main {0}?"
                };
        }

        public void SendDuelStart()
        {
            InternalSendMessage(_duelstart);
            InternalSendMessage(new[] { "Signaler les bugs à Tic-Tac-Toc ou TicTacTocProd@gmail.com" });
            InternalSendMessage(new[]{"Je suis en version alpha (0.0.24), mes decks aussi. Soyez compréhensifs..."});
        }

        public void SendNewTurn()
        {
            InternalSendMessage(_newturn);
        }

        public void SendEndTurn()
        {
            InternalSendMessage(_endturn);
        }

        public void SendDirectAttack(string attacker)
        {
            InternalSendMessage(_directattack, attacker);
        }

        public void SendAttack(string attacker, string defender)
        {
            InternalSendMessage(_attack, attacker, defender);
        }

        public void SendActivate(string spell)
        {
            InternalSendMessage(_activate, spell);
        }

        public void SendSummon(string monster)
        {
            InternalSendMessage(_summon, monster);
        }

        public void SendSetMonster()
        {
            InternalSendMessage(_setmonster);
        }

        public void SendChaining(string card)
        {
            InternalSendMessage(_chaining, card);
        }

        private void InternalSendMessage(IList<string> array, params object[] opts)
        {
            string message = string.Format(array[Program.Rand.Next(array.Count)], opts);
            _game.Chat(message);
        }
    }
}