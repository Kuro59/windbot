using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindBot.Game.AI.Decks
{
    [Deck("Exodia", "AI_Exodia")]
    public class ExodiaExecutor : DefaultExecutor
    {       

        List<int> ExodiaParts = new List<int>();
        List<int> CounterCards = new List<int>();

        public enum CardId
        {
            DragonBlanc = 89631144,
            BrasDroit = 7902349,
            JambeDroit = 8124921,
            JambeGauche = 44519536,
            BrasGauche = 70903634,
            Biblio = 70791313,
            Tete = 33396948,
            PierreBlanche = 79814787,
            DayPiece = 33782437,
            UpstartGobelin = 70368879,
            BambouDor = 39701395,
            BambouBois = 41587307,
            Transaction = 38120068,
            CarteHarmonie = 39701395,
            MaitrisePuissanceMagique = 75014062,
            MailletMagique = 85852291,
            TableMatToon = 89997728,
            PotDualite = 98645731,
            MondeDesToons = 15259703,
            Citadelle = 39910367
        }
     
        public ExodiaExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            ExodiaParts.Add((int)CardId.Tete);
            ExodiaParts.Add((int)CardId.BrasDroit);
            ExodiaParts.Add((int)CardId.BrasGauche);
            ExodiaParts.Add((int)CardId.JambeDroit);
            ExodiaParts.Add((int)CardId.JambeGauche);

            CounterCards.Add((int)CardId.Citadelle);
            CounterCards.Add((int)CardId.Biblio);

            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);

            AddExecutor(ExecutorType.Activate, (int)CardId.Transaction);
            AddExecutor(ExecutorType.Activate, (int)CardId.DayPiece);
            AddExecutor(ExecutorType.Activate, (int)CardId.CarteHarmonie);
            AddExecutor(ExecutorType.Activate, (int)CardId.UpstartGobelin);
            AddExecutor(ExecutorType.Activate, (int)CardId.BambouBois);
            AddExecutor(ExecutorType.Activate, (int)CardId.BambouDor);
            AddExecutor(ExecutorType.Activate, (int)CardId.MaitrisePuissanceMagique, ActivateSpell);
            AddExecutor(ExecutorType.Activate, (int)CardId.Citadelle);
            AddExecutor(ExecutorType.Activate, (int)CardId.TableMatToon, ActivateSpell);
            AddExecutor(ExecutorType.Activate, (int)CardId.MondeDesToons, ActivateSpell);
            AddExecutor(ExecutorType.Activate, (int)CardId.PotDualite, PotDualite);
            AddExecutor(ExecutorType.Activate, (int)CardId.Citadelle);

            
            AddExecutor(ExecutorType.Activate, (int)CardId.MailletMagique, MailletMagique);

            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.PierreBlanche);
            AddExecutor(ExecutorType.Summon, (int)CardId.Biblio);

            AddExecutor(ExecutorType.Activate, (int)CardId.PierreBlanche);
            AddExecutor(ExecutorType.Activate, (int)CardId.Biblio);

            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        private bool MailletMagique()
        {
            List<ClientCard> CardToDeck = new List<ClientCard>();
            foreach (ClientCard Card in Duel.Fields[0].Hand)
            {
                if (!IsExodiaPart(Card))
                    CardToDeck.Add(Card);

            }
            AI.SelectCard(CardToDeck);
            return true;
        }

        private bool IsExodiaPart(ClientCard card)
        {
            if (ExodiaParts.Contains(card.Id))
                return true;
            return false;
        }

        private bool ActivateSpell()
        {
            List<ClientCard> Monster = Duel.Fields[0].GetMonsters();
            foreach (ClientCard card in Monster)
                if (IsCounterCard(card))
                    return true;

            List<ClientCard> Spell = Duel.Fields[0].GetSpells();
            foreach (ClientCard card in Spell)
                if (IsCounterCard(card))
                    return true;
            return false;
        }

        private bool IsCounterCard(ClientCard card)
        {
            if (CounterCards.Contains(card.Id))
                return true;
            return false;
        }

        private bool PotDualite()
        {
            List<int> cards = ExodiaParts;
            if (cards.Count > 0)
            {
                AI.SelectCard(cards);
                return true;
            }
            return false;
        }
    }
}
