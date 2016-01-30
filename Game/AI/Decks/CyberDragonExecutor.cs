using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindBot.Game.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("CyberDragon", "AI_Zen")]
    public class CyberDragonExecutor : DefaultExecutor
    {
        ClientField Bot;
        bool LienDePuissanceEffect = false;

        public enum CardId
        {
            CyberDragonBarriere = 4162088,
            CyberDragonLaser = 68774379,
            CyberDragon = 70095154,
            CyberDragonDrei = 59281922,
            CyberPhoenix = 3370104,
            CyberVouivre = 67159705,
            CyberDragonProto = 26439287,
            CyberKirin = 76986005,
            CyberDragonNoyau = 23893227,
            NagaCyber = 3657444,
            Raigeki = 12580477,
            DarkHole = 53129443,
            CapsuleDuneAutreDimension = 11961740,
            Polymerisation = 24094653,
            LienDePuissance = 37630732,
            ExplosionDevolution = 52875873,
            UniteDeGenerationPhotonique = 66607691,
            Defusion = 95286165,
            TrappeSansFond = 29401950,
            ForceDeMirroir = 44095762,
            UniteReflectiveDattaque = 91989718,
            TechnoCacheCybernetique = 92773018,
            AppelDeLetreHante = 97077563,
            Disfonction = 6137095,
            CyberDragonJumele = 74157028,
            CyberDragonUltime = 1546123,
            CyberDragonNova = 58069384
        }

        public CyberDragonExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            Bot = Duel.Fields[0];

            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);

            AddExecutor(ExecutorType.SpellSet, (int)CardId.Defusion);

            AddExecutor(ExecutorType.Activate, (int)CardId.CapsuleDuneAutreDimension, Capsule);
            AddExecutor(ExecutorType.Activate, (int)CardId.Raigeki, DefaultRaigeki);
            AddExecutor(ExecutorType.Activate, (int)CardId.Polymerisation, Polymeration);
            AddExecutor(ExecutorType.Activate, (int)CardId.LienDePuissance, LienDePuissance);
            AddExecutor(ExecutorType.Activate, (int)CardId.ExplosionDevolution, ExplosionDevolution);
            AddExecutor(ExecutorType.Activate, (int)CardId.DarkHole, DefaultDarkHole);
            AddExecutor(ExecutorType.Activate, (int)CardId.UniteDeGenerationPhotonique);
            AddExecutor(ExecutorType.Activate, (int)CardId.Defusion, Defusion);

            AddExecutor(ExecutorType.Activate, (int)CardId.TrappeSansFond, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.ForceDeMirroir, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.UniteReflectiveDattaque);
            AddExecutor(ExecutorType.Activate, (int)CardId.Disfonction);
            AddExecutor(ExecutorType.Activate, (int)CardId.AppelDeLetreHante, DefaultCallOfTheHaunted);

            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.CyberDragonDrei, NoCyberDragonSpSummon);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.CyberPhoenix, NoCyberDragonSpSummon);
            AddExecutor(ExecutorType.Summon, (int)CardId.NagaCyber, NoCyberDragonSpSummon);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.CyberDragonNoyau, NoCyberDragonSpSummon);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.CyberVouivre, CyberVouivre);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.CyberDragonProto, CyberDragonProto);
            AddExecutor(ExecutorType.Summon, (int)CardId.CyberKirin, CyberKirin);

            AddExecutor(ExecutorType.SpSummon, (int)CardId.CyberDragon);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.CyberDragonUltime);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.CyberDragonJumele);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.CyberDragonLaser);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.CyberDragonBarriere);

            AddExecutor(ExecutorType.Activate, (int)CardId.CyberDragonLaser);
            AddExecutor(ExecutorType.Activate, (int)CardId.CyberDragonBarriere);
            AddExecutor(ExecutorType.Activate, (int)CardId.CyberDragonDrei);
            AddExecutor(ExecutorType.Activate, (int)CardId.CyberPhoenix);
            AddExecutor(ExecutorType.Activate, (int)CardId.CyberKirin);
            AddExecutor(ExecutorType.Activate, (int)CardId.CyberVouivre, CyberVouivreEffect);
            AddExecutor(ExecutorType.Activate, (int)CardId.NagaCyber);
        }

        private bool CyberDragonInHand()  { return Duel.Fields[0].HasInHand((int)CardId.CyberDragon); }
        private bool CyberDragonInGraveyard()  { return Duel.Fields[0].HasInGraveyard((int)CardId.CyberDragon); }
        private bool CyberDragonInMonsterZone() { return Duel.Fields[0].HasInMonstersZone((int)CardId.CyberDragon); }
        private bool CyberDragonIsBanished() { return Duel.Fields[0].HasInBanished((int)CardId.CyberDragon); }

        private bool Capsule()
        {
            List<int> SelectedCard = new List<int>();
            SelectedCard.Add((int)CardId.LienDePuissance);
            SelectedCard.Add((int)CardId.DarkHole);
            SelectedCard.Add((int)CardId.Raigeki);
            AI.SelectCard(SelectedCard);
            return true;
        }

        private bool Polymeration()
        {
            if (Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragonProto) + Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragonDrei) + Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragonDrei) + Bot.GetCountCardInZone(Bot.Hand, (int)CardId.CyberDragon) >= 3)
                AI.SelectCard((int)CardId.CyberDragonUltime);
            else
                AI.SelectCard((int)CardId.CyberDragonJumele);
            return true;
        }

        private bool LienDePuissance()
        {
            LienDePuissanceEffect = true;
            if (Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragonProto) + Bot.GetCountCardInZone(Bot.Hand, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.Graveyard, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.Hand, (int)CardId.CyberDragonNoyau) + Bot.GetCountCardInZone(Bot.Graveyard, (int)CardId.CyberDragonNoyau) + Bot.GetCountCardInZone(Bot.Graveyard, (int)CardId.CyberDragonDrei) + Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragonDrei) >= 3)
                AI.SelectCard((int)CardId.CyberDragonUltime);
            else
                AI.SelectCard((int)CardId.CyberDragonJumele);
            return true;
        }

        private bool ExplosionDevolution()
        {
            if (!AI.Utils.IsOneEnnemyBetterThanValue(Bot.MonsterZone.GetHighestAttackMonster().Attack, false))
                return false;
            else
                AI.SelectCard(Duel.Fields[1].MonsterZone.GetHighestAttackMonster());     
            return true;
        }

        private bool NoCyberDragonSpSummon()
        {
            if (CyberDragonInHand() && (Bot.GetMonsterCount() == 0 && Duel.Fields[1].GetMonsterCount() != 0))
                return false;
            return true;
        }

        private bool CyberVouivre()
        {
            if (CyberDragonInHand() && (Bot.GetMonsterCount() == 0 && Duel.Fields[1].GetMonsterCount() != 0) || (Bot.HasInHand((int)CardId.CyberDragonDrei) || Bot.HasInHand((int)CardId.CyberPhoenix)) && !AI.Utils.IsOneEnnemyBetterThanValue(1800,true))
                return false;
            return true;
        }

        private bool CyberDragonProto()
        {
            if (Bot.GetCountCardInZone(Bot.Hand, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragonNoyau) >= 1 && Bot.HasInHand((int)CardId.Polymerisation) || Bot.GetCountCardInZone(Bot.Hand, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.MonsterZone, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.Graveyard, (int)CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.Graveyard, (int)CardId.CyberDragonNoyau) >= 1 && Bot.HasInHand((int)CardId.LienDePuissance))
                return true;
            if (CyberDragonInHand() && (Bot.GetMonsterCount() == 0 && Duel.Fields[1].GetMonsterCount() != 0) || (Bot.HasInHand((int)CardId.CyberDragonDrei) || Bot.HasInHand((int)CardId.CyberPhoenix)) && !AI.Utils.IsOneEnnemyBetterThanValue(1800, true))
                return false;
            return true;
        }

        private bool CyberKirin()
        {
            return LienDePuissanceEffect;
        }

        private bool CyberVouivreEffect()
        {
            if (Card.Location == CardLocation.Hand)
                return true;
            else if (Card.Location == CardLocation.SpellZone)
            {
                if (AI.Utils.IsOneEnnemyBetterThanValue(Bot.GetMonsters().GetHighestAttackMonster().Attack, true))
                    if (ActivateDescription == AI.Utils.GetStringId((int)CardId.CyberVouivre, 2))
                        return true;
                return false;
            }
            return false;
        }

        private bool Defusion()
        {
            if (Duel.Phase == Phase.Battle)
            {
                if (!Bot.HasAttackingMonster())
                    return true;
            }
            return false;
        }
    }
}