using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindBot.Game.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("BlueEyes", "AI_BlueEyes")]
    public class BlueEyesExecutor : DefaultExecutor
    {
        public enum CardId
        {
            BlueEyesDragon = 89631143,
            DragonEtincelant1 = 11091375,
            DragonSombreMetalique = 88264978,
            DragonMirage = 15960641,
            Kaibaman = 34627841,
            KomodoDragon = 35629124,
            DragonAppat = 2732323,
            Pierre = 79814787,
            Maiden = 88241506,
            Raigeki = 12580477,
            Transaction = 38120068,
            CartesHarmonie = 39701395,
            TrouNoir = 53129443,
            UpstartGobelin = 70368879,
            MiroirDragon = 71490127,
            EpeesRevelationLumiere = 72302403,
            DestructionDeMain = 74519184,
            SilverCry = 87025064,
            Decret = 51452091,
            DragonAzur = 40908371,
            ChevalierDragon = 1639384,
            Dragon5Tetes = 99267150,
            UltimeDragonBlanc = 23995346,
            DragonTonnerre = 698785
        }

        public BlueEyesExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);

            AddExecutor(ExecutorType.Activate, (int)CardId.Decret, Decret);
            AddExecutor(ExecutorType.Activate, (int)CardId.Raigeki, DefaultRaigeki);
            AddExecutor(ExecutorType.Activate, (int)CardId.Transaction, Transaction);
            AddExecutor(ExecutorType.Activate, (int)CardId.CartesHarmonie, Harmonie);
            AddExecutor(ExecutorType.Activate, (int)CardId.TrouNoir, DefaultDarkHole);
            AddExecutor(ExecutorType.Activate, (int)CardId.UpstartGobelin);
            AddExecutor(ExecutorType.Activate, (int)CardId.MiroirDragon, MiroirDragon);
            AddExecutor(ExecutorType.Activate, (int)CardId.EpeesRevelationLumiere, DefaultSwordsOfRevealingLight);
            AddExecutor(ExecutorType.Activate, (int)CardId.DestructionDeMain, DestructionDeMain);
            AddExecutor(ExecutorType.Activate, (int)CardId.SilverCry);

            AddExecutor(ExecutorType.Summon, (int)CardId.Pierre, HasDragonSombreInHand);
            AddExecutor(ExecutorType.Summon, (int)CardId.DragonAppat, HasDragonSombreInHand);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.Pierre, Pierre);
            AddExecutor(ExecutorType.Summon, (int)CardId.Maiden);
            AddExecutor(ExecutorType.Summon, (int)CardId.Kaibaman, HasDragonInHand);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.KomodoDragon, HasDragonInHand);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.DragonAppat, Appat);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.Kaibaman, DragonEtincelant1OuMirage);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.DragonEtincelant1, DragonEtincelant1OuMirage);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.DragonMirage, DragonEtincelant1OuMirage);

            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonSombreMetalique);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonTonnerre);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonAzur);

            AddExecutor(ExecutorType.Activate, (int)CardId.Maiden);
            AddExecutor(ExecutorType.Activate, (int)CardId.Pierre);
            AddExecutor(ExecutorType.Activate, (int)CardId.Kaibaman);
            AddExecutor(ExecutorType.Activate, (int)CardId.DragonAzur, AppatEffect);
            AddExecutor(ExecutorType.Activate, (int)CardId.DragonSombreMetalique, DragonSombre);
            AddExecutor(ExecutorType.Activate, (int)CardId.DragonTonnerre, DefaultDarkHole);
            AddExecutor(ExecutorType.Activate, (int)CardId.KomodoDragon, KomodoEffect);
            AddExecutor(ExecutorType.Activate, (int)CardId.DragonAppat, AppatEffect);
        }

        private bool Decret()
        {
            foreach (ClientCard card in Duel.Fields[0].SpellZone)
                if (card != null && card.Id == Card.Id && Card.Position != (int)CardPosition.FaceUp)
                    return true;
            return false;
        }

        private bool Transaction()
        {
            if (Duel.Fields[0].HasInHand((int)CardId.Kaibaman) || Duel.Fields[0].HasInHand((int)CardId.KomodoDragon) || Duel.Fields[0].HasInMonstersZone((int)CardId.KomodoDragon))
                return false;
            return true;
        }

        private bool Harmonie()
        {
            if (Duel.Fields[0].GetMonsterCount() == 0 || Duel.Fields[0].HasInHand((int)CardId.Maiden) && !Duel.Fields[0].HasInMonstersZone((int)CardId.Maiden))
                return false;
            return true;
        }

        private bool MiroirDragon()
        {
            int DragonCount = 0;
            foreach (ClientCard card in Duel.Fields[0].Graveyard)
                if (card != null && card.Race == (int)CardRace.Dragon)
                    DragonCount++;
            foreach (ClientCard card in Duel.Fields[0].MonsterZone)
                if (card != null && card.Race == (int)CardRace.Dragon && card.Id != (int)CardId.Dragon5Tetes)
                    DragonCount++;

            if (DragonCount >= 5)  
                AI.SelectCard((int)CardId.Dragon5Tetes);
            else 
                AI.SelectCard((int)CardId.UltimeDragonBlanc);
            return true;
        }

        private bool DestructionDeMain()
        {            
            List<int> SelectedCards = new List<int>();
            ClientField Bot = Duel.Fields[0];
            if (Bot.HasInHand((int)CardId.KomodoDragon) && Bot.HasInHand((int)CardId.BlueEyesDragon) && Bot.Hand.Count > 2)
            {
                SelectedCards.Add((int)CardId.KomodoDragon);
                foreach (ClientCard card in Bot.Hand)
                    if (card != null && card.Id != (int)CardId.BlueEyesDragon && SelectedCards.Count != 2)
                        SelectedCards.Add(card.Id);
                AI.SelectCard(SelectedCards);
                return true;
            }
            else if (Bot.HasInHand((int)CardId.DragonAppat) && !Bot.HasInGraveyard((int)CardId.BlueEyesDragon) && Bot.Hand.Count > 2)
            {
                SelectedCards.Add((int)CardId.BlueEyesDragon);
                foreach (ClientCard card in Bot.Hand)
                    if (card != null && card.Id != (int)CardId.DragonAppat && SelectedCards.Count != 2)
                        SelectedCards.Add(card.Id);
                AI.SelectCard(SelectedCards);
                return true;
            }
            else if (Bot.HasInHand((int)CardId.Kaibaman) && Bot.HasInHand((int)CardId.BlueEyesDragon) || (Bot.HasInHand((int)CardId.SilverCry) || Bot.HasInHand((int)CardId.DragonAppat)) && Bot.HasInGraveyard((int)CardId.BlueEyesDragon))
                return false;
            else if (Bot.HasInHand((int)CardId.Maiden) || Bot.HasInHand((int)CardId.DragonEtincelant1) && !AI.Utils.IsOneEnnemyBetterThanValue(1900, true) || Bot.HasInHand((int)CardId.Pierre) || Bot.HasInHand((int)CardId.DragonSombreMetalique))
                return false;
            return true;            
        }

        private bool DragonEtincelant1OuMirage()
        {
            ClientField Bot = Duel.Fields[0];
            if (Bot.HasInHand((int)CardId.DragonSombreMetalique) && !Bot.HasInHand((int)CardId.Pierre) && !Bot.HasInHand((int)CardId.Maiden) && !(Bot.HasInHand((int)CardId.DragonAppat) && Bot.HasInGraveyard((int)CardId.BlueEyesDragon)) && !(Bot.HasInHand((int)CardId.Kaibaman) || Bot.HasInHand((int)CardId.KomodoDragon) && Bot.HasInHand((int)CardId.BlueEyesDragon)))
                return true;
            return false;
        }

        private bool Pierre()
        {
            ClientField Bot = Duel.Fields[0];
            if (!Bot.HasInHand((int)CardId.Maiden) && !(Bot.HasInHand((int)CardId.DragonAppat) && Bot.HasInGraveyard((int)CardId.BlueEyesDragon)) && !(Bot.HasInHand((int)CardId.Kaibaman) || Bot.HasInHand((int)CardId.KomodoDragon) && Bot.HasInHand((int)CardId.BlueEyesDragon)))
                return true;
            return false;
        }

        private bool HasDragonInHand()
        {
            return Duel.Fields[0].HasInHand((int)CardId.BlueEyesDragon);
        }

        private bool HasDragonSombreInHand()
        {
            return Duel.Fields[0].HasInHand((int)CardId.DragonSombreMetalique);
        }

        private bool Appat()
        {
                  ClientField Bot = Duel.Fields[0];
                  if (Bot.HasInGraveyard((int)CardId.BlueEyesDragon))
                      return true;
                  else if (!Bot.HasInHand((int)CardId.Maiden) && !(Bot.HasInHand((int)CardId.Kaibaman) || Bot.HasInHand((int)CardId.KomodoDragon) && Bot.HasInHand((int)CardId.BlueEyesDragon)))
                    return true;
                return false;
        }

        private bool KomodoEffect()
        {
                  ClientField Bot = Duel.Fields[0];
                  if (Bot.HasInHand((int)CardId.BlueEyesDragon))
                  {
                      AI.SelectCard((int)CardId.BlueEyesDragon);
                      return true;
                  }
                  else if (Bot.HasInHand((int)CardId.DragonEtincelant1))
                  {
                      AI.SelectCard((int)CardId.DragonEtincelant1);
                      return true;
                  }
                  return false;
        }

        private bool AppatEffect()
        {
            ClientField Bot = Duel.Fields[0];
            if (Bot.HasInGraveyard((int)CardId.BlueEyesDragon))
                AI.SelectCard((int)CardId.BlueEyesDragon);
            return true;
        }

        private bool DragonSombre()
        {
            ClientField Bot = Duel.Fields[0];
            int Selected = 0;
            if (Bot.HasInGraveyard((int)CardId.BlueEyesDragon) || Bot.HasInHand((int)CardId.BlueEyesDragon))
                Selected = (int)CardId.BlueEyesDragon;
            else if (Bot.HasInGraveyard((int)CardId.DragonTonnerre) || Bot.HasInHand((int)CardId.DragonTonnerre))
                Selected = (int)CardId.DragonTonnerre;
            else if (Bot.HasInGraveyard((int)CardId.UltimeDragonBlanc) || Bot.HasInHand((int)CardId.UltimeDragonBlanc))
                Selected = (int)CardId.UltimeDragonBlanc;
            else if (Bot.HasInGraveyard((int)CardId.Dragon5Tetes) || Bot.HasInHand((int)CardId.Dragon5Tetes))
                Selected = (int)CardId.Dragon5Tetes;
            else if (Bot.HasInGraveyard((int)CardId.DragonMirage) || Bot.HasInHand((int)CardId.DragonMirage))
                Selected = (int)CardId.DragonMirage;
            else if (Bot.HasInGraveyard((int)CardId.DragonEtincelant1) || Bot.HasInHand((int)CardId.DragonEtincelant1))
                Selected = (int)CardId.DragonEtincelant1;
            if (Selected != 0)
            {
                AI.SelectCard(Selected);
                return true;
            }
            return false;
        }
    }
}