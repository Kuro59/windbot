using System.Collections.Generic;
using WindBot.Game.Enums;
namespace WindBot.Game.AI.Decks
{
    [Deck("Blackwing", "AI_Blackwing")]
    public class BlackwingExecutor : DefaultExecutor
    {
        public enum CardId
        {
            KrisLaFissure = 81105204,
            Sirocco = 75498415,
            Shura = 58820853,
            Bora = 49003716,
            Kalut = 85215458,
            MisralLeTourbillon = 2009101,
            Blizzard = 22835145,
            MistralLeBouclier = 46710683,
            Raigeki = 12580477,
            DarkHole = 53129443,
            MysticalSpaceTyphoon = 5318639,
            TourbillonNoir = 91351370,
            MirrorForce = 44095762,
            CorbeauxDelta = 59839761,
            DimensionalPrison = 70342110,
            VentDargent = 33236860,
            AileSombre = 9012916,
            Maitre = 69031175,
            Arsenal = 76913983,
            Gram = 17377751
        }

        public BlackwingExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);

            AddExecutor(ExecutorType.Activate, (int)CardId.MysticalSpaceTyphoon, MysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, (int)CardId.DarkHole, DefaultDarkHole);
            AddExecutor(ExecutorType.Activate, (int)CardId.Raigeki, DefaultRaigeki);
            AddExecutor(ExecutorType.Activate, (int)CardId.TourbillonNoir, TourbillonNoir);
            
            AddExecutor(ExecutorType.SpSummon, (int)CardId.KrisLaFissure);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.KrisLaFissure);
            AddExecutor(ExecutorType.Summon, (int)CardId.Sirocco, Sirocco);
            AddExecutor(ExecutorType.Summon, (int)CardId.Shura, Shura);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.Shura);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Bora);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.Bora);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.Kalut, Kalut);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.MisralLeTourbillon);
            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.MisralLeTourbillon);
            AddExecutor(ExecutorType.Summon, (int)CardId.Blizzard, Blizzard);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.MistralLeBouclier);

            AddExecutor(ExecutorType.SpSummon, (int)CardId.VentDargent);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Maitre);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Gram);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Arsenal);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.AileSombre);

            AddExecutor(ExecutorType.Activate, (int)CardId.MirrorForce, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.DimensionalPrison, DefaultUniqueTrap);
            AddExecutor(ExecutorType.Activate, (int)CardId.CorbeauxDelta, CorbeauxDelta);
            AddExecutor(ExecutorType.Activate, (int)CardId.CorbeauxDelta, DefaultUniqueTrap);

            AddExecutor(ExecutorType.Activate, (int)CardId.Blizzard);
            AddExecutor(ExecutorType.Activate, (int)CardId.Shura);
            AddExecutor(ExecutorType.Activate, (int)CardId.Kalut, KaluhSiroccoEffect);
            AddExecutor(ExecutorType.Activate, (int)CardId.Sirocco, KaluhSiroccoEffect);
            AddExecutor(ExecutorType.Activate, (int)CardId.MisralLeTourbillon, MistralLeTourbillonEffect);
            AddExecutor(ExecutorType.Activate, (int)CardId.VentDargent);
            AddExecutor(ExecutorType.Activate, (int)CardId.AileSombre);
            AddExecutor(ExecutorType.Activate, (int)CardId.Maitre);
            AddExecutor(ExecutorType.Activate, (int)CardId.Arsenal);
            AddExecutor(ExecutorType.Activate, (int)CardId.Gram);

            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
        }

        public override bool OnPreBattleBetween(ClientCard attacker, ClientCard defender)
        {
            if (defender.IsMonsterInvincible() && !defender.IsMonsterDangerous() && attacker.Id == 83104731)
                return true;
            return base.OnPreBattleBetween(attacker, defender);
        }

        private bool MysticalSpaceTyphoon()
        {
            foreach (ClientCard card in CurrentChain)
                if (card != null && card.Id == (int)CardId.MysticalSpaceTyphoon)
                    return false;

            return StampingDestruction();
        }

        private bool StampingDestruction()
        {
            List<ClientCard> spells = Duel.Fields[1].GetSpells();
            if (spells.Count == 0)
                return false;

            ClientCard selected = null;
            foreach (ClientCard card in spells)
            {
                if (card.IsSpellNegateAttack())
                {
                    selected = card;
                    break;
                }
            }

            if (selected == null)
            {
                foreach (ClientCard card in spells)
                {
                    if (Duel.Player == 1 && !card.HasType(CardType.Continuous))
                        continue;
                    selected = card;
                    if (Duel.Player == 0 && card.IsFacedown())
                        break;
                }
            }

            if (selected == null)
                return false;
            AI.SelectCard(selected);
            return true;
        }

        private bool Shura()
        {
            if (Duel.Fields[0].HasInMonstersZone((int)CardId.Sirocco) && Duel.Fields[0].GetMonsters().GetHighestAttackMonster().Attack < 3800)
                return true;
            return false;
        }

        private bool TourbillonNoir()
        {
            if (Card.Location == CardLocation.Hand && Duel.Fields[0].HasInSpellZone(Card.Id))
                return false;
            if (ActivateDescription == AI.Utils.GetStringId((int)Card.Id,0))
                AI.SelectCard((int)CardId.MisralLeTourbillon);
            return true;
        }

        private bool Sirocco()
        {
            int OpponentMonster = Duel.Fields[1].GetMonsterCount();
            int AIMonster = Duel.Fields[0].GetMonsterCount();
            if (OpponentMonster != 0 && AIMonster == 0)
                return true;
            return false;
        }

        private bool Bora()
        {
            List<ClientCard> monster = Duel.Fields[0].GetMonsters();
            foreach (ClientCard card in monster)
                if (card != null && card.Id == (int)CardId.KrisLaFissure || card.Id == (int)CardId.Kalut || card.Id == (int)CardId.MisralLeTourbillon || card.Id == (int)CardId.Bora || card.Id == (int)CardId.Sirocco || card.Id == (int)CardId.Shura || card.Id == (int)CardId.Blizzard)
                    return true;
            return false;
        }

        private bool Kalut()
        {
            foreach (ClientCard card in Duel.Fields[0].Hand)
                if (card != null && card.Id == (int)CardId.KrisLaFissure || card.Id == (int)CardId.MisralLeTourbillon || card.Id == (int)CardId.Bora || card.Id == (int)CardId.Sirocco || card.Id == (int)CardId.Shura || card.Id == (int)CardId.Blizzard)
                    return false;
            return true;
        }

        private bool Blizzard()
        {
            foreach (ClientCard card in Duel.Fields[0].Graveyard)
                if (card != null && card.Id == (int)CardId.Kalut || card.Id == (int)CardId.Bora || card.Id == (int)CardId.Shura || card.Id == (int)CardId.KrisLaFissure)
                    return true;
            return false;
        }

        private bool CorbeauxDelta()
        {
            int Count = 0;

            List<ClientCard> monster = Duel.Fields[0].GetMonsters();
            foreach (ClientCard card in monster)
                if (card != null && card.Id == (int)CardId.KrisLaFissure || card.Id == (int)CardId.Kalut || card.Id == (int)CardId.MisralLeTourbillon || card.Id == (int)CardId.Bora || card.Id == (int)CardId.Sirocco || card.Id == (int)CardId.Shura || card.Id == (int)CardId.Blizzard)
                    Count++;

            if (Count == 3)
                return true;
            return false;
        }

        private bool MistralLeTourbillonEffect()
        {
            if (Card.Position == (int)Game.Enums.CardPosition.FaceUp)
            {
                AI.SelectCard(Duel.Fields[1].GetMonsters().GetHighestAttackMonster());
                return true;
            }
            return false;
        }

        private bool KaluhSiroccoEffect()
        {
            if (Duel.Fields[1].GetMonsters().GetHighestAttackMonster().IsFacedown() || Duel.Fields[1].GetMonsters().GetHighestDefenseMonster().IsFacedown() || Duel.Fields[1].GetMonsterCount() == 0)
                return false;
            if (Duel.Fields[0].GetMonsters().GetHighestAttackMonster().Attack < Duel.Fields[1].GetMonsters().GetHighestAttackMonster().Attack)
                return true;
            if (Duel.Fields[0].GetMonsters().GetHighestAttackMonster().Attack < Duel.Fields[1].GetMonsters().GetHighestDefenseMonster().Defense)
                return true;
            return false;
        }
    }
}