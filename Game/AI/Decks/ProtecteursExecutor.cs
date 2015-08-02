using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindBot.Game.Enums;

namespace WindBot.Game.AI.Decks
{
    [Deck("Protecteurs", "AI_Protecteurs")]
    public class ProtecteursExecutor : DefaultExecutor
    {
        public enum CardId
        {
            DevinProtecteurs = 25524823,
            DragonPoussiereDetoileCorrompu = 36521459,
            Oracle = 3825890,
            Chef = 62473983,
            RaiOh = 71564252,
            Commandant = 17393207,
            Assailant = 25262697,
            Serviteur = 30213599,
            Espion = 24317029,
            Recruteur = 93023479,
            InvitationDesTenebres = 1475311,
            TrouNoir = 53129443,
            SacrificeRoyal = 72405967,
            Stelle = 99523325,
            Typhon = 5318639,
            LivreDeLaLune = 14087893,
            TempleCaches = 70000776,
            ValleeMortuaire = 47355498,
            TrappeSansFond = 29401950,
            Rite = 30450531,
            Hommage = 53582587,
            Prison = 70342110,
            MirrorForce = 84749824,
            TombeImperial = 90434657
        }

        public ProtecteursExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {

            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);

            AddExecutor(ExecutorType.Activate, (int)CardId.InvitationDesTenebres);
            AddExecutor(ExecutorType.Activate, (int)CardId.TrouNoir, DefaultDarkHole);
            AddExecutor(ExecutorType.Activate, (int)CardId.SacrificeRoyal);
            AddExecutor(ExecutorType.Activate, (int)CardId.Stelle);
            AddExecutor(ExecutorType.Activate, (int)CardId.Typhon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, (int)CardId.LivreDeLaLune, DefaultBookOfMoon);
            AddExecutor(ExecutorType.Activate, (int)CardId.TempleCaches, TempleCaches);
            AddExecutor(ExecutorType.Activate, (int)CardId.ValleeMortuaire, ValleeMortuaire);

            AddExecutor(ExecutorType.Activate, (int)CardId.TrappeSansFond, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.MirrorForce, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.Prison, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.Rite, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.TombeImperial, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.Hommage, DefaultTorrentialTribute);

            AddExecutor(ExecutorType.Summon, (int)CardId.DevinProtecteurs);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.DragonPoussiereDetoileCorrompu, DragonPoussiereDetoileCorrompu);
            AddExecutor(ExecutorType.Summon, (int)CardId.Oracle);
            AddExecutor(ExecutorType.Summon, (int)CardId.Chef);
            AddExecutor(ExecutorType.Summon, (int)CardId.RaiOh);
            AddExecutor(ExecutorType.Summon, (int)CardId.Commandant, CommandantSummon);
            AddExecutor(ExecutorType.Summon, (int)CardId.Assailant);
            AddExecutor(ExecutorType.Summon, (int)CardId.Serviteur);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.Espion);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.Recruteur);

            AddExecutor(ExecutorType.Activate, (int)CardId.DevinProtecteurs);
            AddExecutor(ExecutorType.Activate, (int)CardId.Oracle);
            AddExecutor(ExecutorType.Activate, (int)CardId.Chef);
            AddExecutor(ExecutorType.Activate, (int)CardId.Commandant, Commandant);
            AddExecutor(ExecutorType.Activate, (int)CardId.Assailant, Assailant);
            AddExecutor(ExecutorType.Activate, (int)CardId.Serviteur, Serviteur);
            AddExecutor(ExecutorType.Activate, (int)CardId.Espion, EspionOuRecruteur);
            AddExecutor(ExecutorType.Activate, (int)CardId.Recruteur, EspionOuRecruteur);
        }

        private bool TempleCaches()
        {
            if (Card.Location == CardLocation.Hand && Duel.Fields[0].HasInSpellZone((int)Card.Id))
                return false;
            return true;
        }

        private bool ValleeMortuaire()
        {
            if (Duel.Fields[0].SpellZone[5] != null)
                return false;
            return true;
        }

        private bool DragonPoussiereDetoileCorrompu()
        {
            if (Duel.Fields[0].SpellZone[5] != null)
                return true;
            return false;
        }

        private bool Commandant()
        {
            if (!Duel.Fields[0].HasInHand((int)CardId.ValleeMortuaire) && !Duel.Fields[0].HasInSpellZone((int)CardId.ValleeMortuaire))
                return true;
            return false;
        }

        private bool CommandantSummon()
        {
            return !Commandant();
        }

        private bool Assailant()
        {
            if (!Card.IsAttack())
                return false;
            foreach (ClientCard card in Duel.Fields[1].MonsterZone)
                if (card.IsDefense() && card.Defense > 1500 && card.Attack < 1500 || card.Attack > 1500 && card.Defense < 1500)
                    return true;
            return false;
        }

        private bool Serviteur()
        {
            int bestatk = Duel.Fields[0].GetMonsters().GetHighestAttackMonster().Attack;
            if (AI.Utils.IsOneEnnemyBetterThanValue(bestatk, true))
            {
                AI.SelectCard(Duel.Fields[1].GetMonsters().GetHighestAttackMonster());
                return true;
            }
            return false;
        }

        private bool EspionOuRecruteur()
        {
            int number = 0;
            foreach (ClientCard card in Duel.Fields[0].Hand)
                if (card.Id == (int)CardId.Serviteur)
                    number++;
            foreach (ClientCard card in Duel.Fields[0].Graveyard)
                if (card.Id == (int)CardId.Serviteur)
                    number++;
            foreach (ClientCard card in Duel.Fields[0].MonsterZone)
                if (card.Id == (int)CardId.Serviteur)
                    number++;
            foreach (ClientCard card in Duel.Fields[0].Banished)
                if (card.Id == (int)CardId.Serviteur)
                    number++;

            if (number < 3)
                AI.SelectCard((int)CardId.Serviteur);
            return true;
        }
    }
}