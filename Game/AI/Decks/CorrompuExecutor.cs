using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindBot.Game.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Corrompu", "AI_Corrompu")]
    public class CorrompuExecutor : DefaultExecutor
    {
        public enum CardId
        {
            DragonDeLaVeriteCorrompu = 37115575,
            CyberDragonUltimeCorrompu = 1710476,
            DragonArcEnCielCorrompu = 598988,
            DragonBlancAuxYeuxBleusCorrompu = 9433350,
            Barbaros = 78651105,
            DragonPoussiereDetoileCorrompu = 36521459,
            DragonNoirAuxYeuxRougeCorrompu = 55343236,
            RouageDimensionelCorrompu = 74509280,
            Transaction = 38120068,
            TrouNoir = 53129443,
            EpeesRevelationLumiere = 72302403,
            TerraFormation = 73628505,
            GraalInterdit = 25789292,
            MondeCorrompu = 27564031,
            SceauDorichalque = 48179391,
            TrappeSansFond = 29401950,
            PrisonDimensionelle = 70342110,
            ForceDeMirroir = 44095762,
            DelugeCorrompu = 53063039,
            AbsorptionCompetence = 82732705,
            DragonParadoxeCorrumpu = 8310162
        }

        List<int> CorrompuCard = new List<int>();
        public CorrompuExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            CorrompuCard.Add((int)CardId.DragonArcEnCielCorrompu);
            CorrompuCard.Add((int)CardId.DragonBlancAuxYeuxBleusCorrompu);
            CorrompuCard.Add((int)CardId.DragonDeLaVeriteCorrompu);
            CorrompuCard.Add((int)CardId.DragonNoirAuxYeuxRougeCorrompu);
            CorrompuCard.Add((int)CardId.CyberDragonUltimeCorrompu);
            CorrompuCard.Add((int)CardId.DragonPoussiereDetoileCorrompu);

            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);

            AddExecutor(ExecutorType.Activate, (int)CardId.TrouNoir, DefaultDarkHole);
            AddExecutor(ExecutorType.Activate, (int)CardId.Transaction);
            AddExecutor(ExecutorType.Activate, (int)CardId.EpeesRevelationLumiere, SwordsOfRevealingLight);
            AddExecutor(ExecutorType.Activate, (int)CardId.TerraFormation, Terraforming);
            AddExecutor(ExecutorType.Activate, (int)CardId.GraalInterdit, GraalInterdit);
            AddExecutor(ExecutorType.Activate, (int)CardId.MondeCorrompu, MondeCorrompu);
            AddExecutor(ExecutorType.Activate, (int)CardId.SceauDorichalque, IsFieldEmpty);

            AddExecutor(ExecutorType.Activate, (int)CardId.TrappeSansFond, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.PrisonDimensionelle, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.ForceDeMirroir, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.AbsorptionCompetence);
            AddExecutor(ExecutorType.Activate, (int)CardId.DelugeCorrompu, DelugeCorrompu);

            AddExecutor(ExecutorType.Summon, (int)CardId.RouageDimensionelCorrompu, RouageDimensionelCorrompu);
            AddExecutor(ExecutorType.Summon, (int)CardId.Barbaros, Barbaros);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.CyberDragonUltimeCorrompu, CorrompuMonsterWithoutStardust);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonArcEnCielCorrompu, CorrompuMonsterWithoutStardust);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonBlancAuxYeuxBleusCorrompu, SynchroCorrompu);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonNoirAuxYeuxRougeCorrompu, CorrompuMonsterWithoutStardust);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonPoussiereDetoileCorrompu, DragonPoussiereDetoileCorrompu);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonDeLaVeriteCorrompu);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonParadoxeCorrumpu, DragonParadoxe);

            AddExecutor(ExecutorType.Activate, (int)CardId.DragonParadoxeCorrumpu);
            AddExecutor(ExecutorType.Activate, (int)CardId.DragonDeLaVeriteCorrompu);
        }

        private bool Terraforming()
        {
            if (Duel.Fields[0].HasInHand((int)CardId.SceauDorichalque) || Duel.Fields[0].HasInHand((int)CardId.MondeCorrompu))
                return false;
            if (Duel.Fields[0].SpellZone[5] != null)
                return false;
            if (!Duel.Fields[0].HasInHand(CorrompuCard))
                AI.SelectCard((int)CardId.MondeCorrompu);
            return true;
        }

        private bool MondeCorrompu()
        {
            if (Duel.Fields[0].SpellZone[5] != null)
                return false;
            return true;
        }

        private bool DragonParadoxe()
        {
            if (Duel.Fields[0].HasInSpellZone((int)CardId.MondeCorrompu))
                return true;
            return false;
        }

        private bool SwordsOfRevealingLight()
        {
            return AI.Utils.IsEnnemyBetter(true, false);            
        }

        private bool GraalInterdit()
        {
            if (Duel.Fields[0].HasInMonstersZone(CorrompuCard) && !Duel.Fields[0].HasInSpellZone((int)CardId.AbsorptionCompetence))
            {
                AI.SelectCard(Duel.Fields[0].GetMonsters().GetHighestAttackMonster());
                return true;
            }
            return false;
        }

        private bool IsFieldEmpty()
        {
            if (Card.Location == CardLocation.Hand)
                return DefaultField();
            return false;
        }

        private bool DelugeCorrompu()
        {
            if (AI.Utils.IsEnnemyBetter(true, false))
            {
                AI.SelectCard(Duel.Fields[1].GetMonsters().GetHighestAttackMonster());
                return true;
            }
            return false;
        }

        private bool Barbaros()
        {
            foreach (ClientCard card in Duel.Fields[0].SpellZone)
                if (card != null && Card.Id == (int)CardId.AbsorptionCompetence && Card.Position == (int)Game.Enums.CardPosition.FaceUp)
                    return true;
            return false;
        }

        private bool CorrompuMonsterWithoutStardust()
        {
            if (Duel.Fields[0].HasInHand((int)CardId.DragonPoussiereDetoileCorrompu))
                return false;
            foreach (ClientCard card in Duel.Fields[0].SpellZone)
                if (card != null && card.Id == (int)CardId.AbsorptionCompetence && card.Position == (int)Game.Enums.CardPosition.FaceUp)
                    return true;
            if (IsFieldEmpty())
                return false;
            return true;
        }

        private bool SynchroCorrompu()
        {
            if (Duel.Fields[0].HasInHand((int)CardId.DragonPoussiereDetoileCorrompu))
                return false;
            foreach (ClientCard card in Duel.Fields[0].SpellZone)
                if (card != null && card.Id == (int)CardId.AbsorptionCompetence && card.Position == (int)Game.Enums.CardPosition.FaceUp)
                    return true;
            if (IsFieldEmpty() || Duel.Fields[0].HasInHand((int)CardId.RouageDimensionelCorrompu) && Duel.Fields[0].HasInSpellZone((int)CardId.MondeCorrompu))
                return false;
            return true;
        }

        private bool DragonPoussiereDetoileCorrompu()
        {
            foreach (ClientCard card in Duel.Fields[0].SpellZone)
                if (card != null && card.Id == (int)CardId.AbsorptionCompetence && card.Position == (int)Game.Enums.CardPosition.FaceUp)
                    return true;
            if (IsFieldEmpty())
                return false;
            if (Duel.Fields[0].HasInMonstersZone((int)CardId.DragonPoussiereDetoileCorrompu) || Duel.Fields[0].HasInHand((int)CardId.RouageDimensionelCorrompu) && Duel.Fields[0].HasInSpellZone((int)CardId.MondeCorrompu))
                return false;
            return true;
        }

        private bool RouageDimensionelCorrompu()
        {
            foreach (ClientCard card in Duel.Fields[0].Hand)
                if (card != null && CorrompuCard.Contains(card.Id) && card.Level == 8 && Duel.Fields[0].HasInSpellZone((int)CardId.MondeCorrompu))
                    return true;
            return false;
        }
    }
}