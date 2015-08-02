using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindBot.Game.AI.Decks
{
    [Deck("Nekroz", "AI_Nekroz")]
    public class NekrozExecutor : DefaultExecutor
    {  
        public enum CardId
        {
            PrincesseDansanteNekroz = 52738610,
            SenjuMilleMain = 23401839,
            ManjuMilleMain = 95492061,
            Shurit = 88240999,
            Cancrelat = 23434538,
            ArmureInvincible = 88240999,
            Trishula = 52068432,
            Valkyrus = 25857246,
            Gungnir = 74122412,
            Brionac = 26674724,
            Unicore = 89463537,
            Solais = 99185129,
            FantomeDuChaos = 30312361,
            Raigeki = 12580477,
            RenfortDeLarmee = 32807846,
            Transaction = 38120068,
            PreparationDesRites = 96729612,
            Miroir = 14735698,
            Kaleidoscope = 51124303,
            Cycle = 97211663,
            Typhon = 5318639,
            Decret = 51452091,
            Exciton = 46772449,
            Herault = 79606837
        }

        List<int> NekrozRituelCard = new List<int>();
        List<int> NekrozSpellCard = new List<int>();

        public NekrozExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            NekrozRituelCard.Add((int)CardId.Solais); 
            NekrozRituelCard.Add((int)CardId.Unicore);
            NekrozRituelCard.Add((int)CardId.ArmureInvincible);
            NekrozRituelCard.Add((int)CardId.Brionac);
            NekrozRituelCard.Add((int)CardId.Trishula);
            NekrozRituelCard.Add((int)CardId.Gungnir);
            NekrozRituelCard.Add((int)CardId.Valkyrus);

            NekrozSpellCard.Add((int)CardId.Miroir);
            NekrozSpellCard.Add((int)CardId.Kaleidoscope);
            NekrozSpellCard.Add((int)CardId.Cycle);

            AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);

            AddExecutor(ExecutorType.Activate, (int)CardId.Raigeki, DefaultRaigeki);
            AddExecutor(ExecutorType.Activate, (int)CardId.RenfortDeLarmee, Renfort);
            AddExecutor(ExecutorType.Activate, (int)CardId.Transaction);
            AddExecutor(ExecutorType.Activate, (int)CardId.PreparationDesRites);
            AddExecutor(ExecutorType.Activate, (int)CardId.Miroir);
            AddExecutor(ExecutorType.Activate, (int)CardId.Kaleidoscope);
            AddExecutor(ExecutorType.Activate, (int)CardId.Cycle);
            AddExecutor(ExecutorType.Activate, (int)CardId.Typhon, DefaultMysticalSpaceTyphoon);
            AddExecutor(ExecutorType.Activate, (int)CardId.Decret);

            AddExecutor(ExecutorType.SummonOrSet, (int)CardId.PrincesseDansanteNekroz, Princesse);
            AddExecutor(ExecutorType.MonsterSet, (int)CardId.Shurit, Shurit);
            AddExecutor(ExecutorType.Summon, (int)CardId.SenjuMilleMain, Senju);
            AddExecutor(ExecutorType.Summon, (int)CardId.ManjuMilleMain, Manju);
            AddExecutor(ExecutorType.Summon, (int)CardId.FantomeDuChaos, Fantome);

            AddExecutor(ExecutorType.Activate, (int)CardId.Unicore, Unicore);
            AddExecutor(ExecutorType.Activate, (int)CardId.ArmureInvincible, Armure);
            AddExecutor(ExecutorType.Activate, (int)CardId.Valkyrus, Valkyrus);
            AddExecutor(ExecutorType.Activate, (int)CardId.Gungnir, Gungnir);
            AddExecutor(ExecutorType.Activate, (int)CardId.Brionac, Brionac);
            AddExecutor(ExecutorType.Activate, (int)CardId.Solais, Solais);
            AddExecutor(ExecutorType.Activate, (int)CardId.Exciton, Exciton);
            AddExecutor(ExecutorType.Activate, (int)CardId.FantomeDuChaos, FantomeEffect);
            AddExecutor(ExecutorType.Activate, (int)CardId.Cancrelat);
            AddExecutor(ExecutorType.Activate, (int)CardId.SenjuMilleMain, SenjuEffect);
            AddExecutor(ExecutorType.Activate, (int)CardId.ManjuMilleMain, Brionac);
            AddExecutor(ExecutorType.Activate, (int)CardId.Herault);

            AddExecutor(ExecutorType.SpSummon, (int)CardId.Trishula);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.ArmureInvincible);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Valkyrus);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Gungnir);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Brionac);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Unicore);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Solais);
            AddExecutor(ExecutorType.SpSummon, (int)CardId.Exciton, Exciton);
        }

        private bool Senju()
        {
            if (!Duel.Fields[0].HasInHand(NekrozRituelCard) || Duel.Fields[0].HasInHand((int)CardId.Shurit) || !Duel.Fields[0].HasInHand(NekrozSpellCard))  
                return true;
            foreach (ClientCard Card in Duel.Fields[0].Hand)
                if (Card.Id == (int)CardId.Kaleidoscope && !Duel.Fields[0].HasInHand((int)CardId.Unicore))
                    return true;
                else if (Card.Id == (int)CardId.Trishula || Card.Id == (int)CardId.ArmureInvincible && !Duel.Fields[0].HasInHand((int)CardId.Miroir) || !Duel.Fields[0].HasInHand((int)CardId.Shurit))
                    return true;
            return false;
        }

        private bool Renfort()
        {
            if (!Duel.Fields[0].HasInGraveyard((int)CardId.Shurit) && !Duel.Fields[0].HasInHand((int)CardId.Shurit))
            {
                AI.SelectCard((int)CardId.Shurit);
                return true;
            }
            return false;
        }

        private bool Manju()
        {
                if (!Duel.Fields[0].HasInHand((int)CardId.SenjuMilleMain) || !Duel.Fields[0].HasInHand((int)CardId.Shurit))
                return true;
            return false;
        }

        private bool Princesse()
        {
            if (!Duel.Fields[0].HasInHand((int)CardId.SenjuMilleMain) && !Duel.Fields[0].HasInHand((int)CardId.ManjuMilleMain))
                return true;
            return false;
        }

        private bool Fantome()
        {
            if (Duel.Fields[0].HasInGraveyard((int)CardId.Shurit) && Duel.Fields[0].HasInHand(NekrozSpellCard) && Duel.Fields[0].HasInHand(NekrozRituelCard))
                return true;
            return false;
        }

        private bool FantomeEffect()
        {
            AI.SelectCard((int)CardId.Shurit);
            return true;
        }

        private bool Shurit()
        {
            if (!Duel.Fields[0].HasInHand((int)CardId.SenjuMilleMain) && !Duel.Fields[0].HasInHand((int)CardId.ManjuMilleMain) && !Duel.Fields[0].HasInHand((int)CardId.PrincesseDansanteNekroz))
                return true;
            return false;
        }

        private bool Trishula()
        {
            if (AI.Utils.IsAllEnnemyBetterThanValue(2700, true) && Duel.Fields[0].HasInHand((int)CardId.ArmureInvincible))
                return false;
            return true;
        }

        private bool Armure()
        {
            if (AI.Utils.IsAllEnnemyBetterThanValue(3300, true))
            {
                AI.SelectCard((int)CardId.ArmureInvincible);
                return true;
            }
            return false;
        }

        private bool Exciton()
        {
            if (AI.Utils.IsAllEnnemyBetterThanValue(Duel.Fields[0].GetMonsters().GetHighestAttackMonster().Attack, true))
            {
                return true;
            }
            return false;
        }

        private bool Valkyrus()
        {
            if (Duel.Phase == Game.Enums.Phase.Battle)
                return true;
            return false;
        }

        private bool Gungnir()
        {           
            if (AI.Utils.IsOneEnnemyBetterThanValue(Duel.Fields[0].GetMonsters().GetHighestAttackMonster().Attack,true) && Duel.Phase == Game.Enums.Phase.Main1)
            {
                AI.SelectCard(Duel.Fields[1].GetMonsters().GetHighestAttackMonster());
                return true;
            }
            return false;
        }

        private bool Brionac()
        {
            if (!Duel.Fields[0].HasInHand((int)CardId.Shurit))
            {
                AI.SelectCard((int)CardId.Shurit);
                return true;
            }
            else if (!Duel.Fields[0].HasInHand(NekrozSpellCard))
            {
                AI.SelectCard((int)CardId.Miroir);
                return true;
            }
            else if (AI.Utils.IsOneEnnemyBetterThanValue(3300, true) && !Duel.Fields[0].HasInHand((int)CardId.Trishula))
            {
                AI.SelectCard((int)CardId.Trishula);
                return true;
            }
            else if (AI.Utils.IsAllEnnemyBetterThanValue(2700,true) && !Duel.Fields[0].HasInHand((int)CardId.ArmureInvincible))
            {
                AI.SelectCard((int)CardId.ArmureInvincible);
                return true;
            }
            else if (Duel.Fields[0].HasInHand((int)CardId.Unicore) && !Duel.Fields[0].HasInHand((int)CardId.Kaleidoscope))
            {
                AI.SelectCard((int)CardId.Kaleidoscope);
                return true;
            }
            else if (!Duel.Fields[0].HasInHand((int)CardId.Unicore) && Duel.Fields[0].HasInHand((int)CardId.Kaleidoscope))
            {
                AI.SelectCard((int)CardId.Unicore);
                return true;
            }
            return false;
        }

        private bool SenjuEffect()
        {
            if (AI.Utils.IsOneEnnemyBetterThanValue(3300, true) && !Duel.Fields[0].HasInHand((int)CardId.Trishula))
            {
                AI.SelectCard((int)CardId.Trishula);
                return true;
            }
            else if (AI.Utils.IsAllEnnemyBetterThanValue(2700, true) && !Duel.Fields[0].HasInHand((int)CardId.ArmureInvincible))
            {
                AI.SelectCard((int)CardId.ArmureInvincible);
                return true;
            }
            else if (!Duel.Fields[0].HasInHand((int)CardId.Unicore) && Duel.Fields[0].HasInHand((int)CardId.Kaleidoscope))
            {
                AI.SelectCard((int)CardId.Unicore);
                return true;
            }
            return false;
        }

        private bool Unicore()
        {
            if (Duel.Fields[0].HasInGraveyard((int)CardId.Shurit))
            {
                AI.SelectCard((int)CardId.Shurit);
                return true;
            }
            return false;
        }

        private bool Solais()
        {
            if (!Duel.Fields[0].HasInHand(NekrozSpellCard))
            {
                AI.SelectCard((int)CardId.Miroir);
                return true;
            }
            return false;
        }

        private bool IsTheLastPossibility()
        {
            if (!Duel.Fields[0].HasInHand((int)CardId.ArmureInvincible) && !Duel.Fields[0].HasInHand((int)CardId.Trishula))
                return true;
            return false;
        }

        private bool SelectNekrozWhoInvoke()
        {
             List<int> NekrozCard = new List<int>();
             foreach (ClientCard Card in Duel.Fields[0].Hand)
                 if (NekrozRituelCard.Contains((int)Card.Id))
                     NekrozCard.Add(Card.Id);

             foreach (int Id in NekrozCard)
             {
                    if (Id == (int)CardId.Trishula && AI.Utils.IsAllEnnemyBetterThanValue(2700, true) && Duel.Fields[0].HasInHand((int)CardId.ArmureInvincible))
                    {
                        AI.SelectCard((int)CardId.Trishula);
                        return true;
                    }
                    else if (Id == (int)CardId.ArmureInvincible)
                    {
                        AI.SelectCard((int)CardId.ArmureInvincible);
                        return true;
                    }
                    else if (Id == (int)CardId.Unicore && Duel.Fields[0].HasInHand((int)CardId.Kaleidoscope) && !Duel.Fields[0].HasInGraveyard((int)CardId.Shurit))
                    {
                        AI.SelectCard((int)CardId.Unicore);
                        return true;
                    }
                    else if (Id == (int)CardId.Valkyrus)
                    {
                        if (IsTheLastPossibility())
                        {
                            AI.SelectCard((int)CardId.Valkyrus);
                            return true;
                        }
                    }
                    else if (Id == (int)CardId.Gungnir)
                    {
                        if (IsTheLastPossibility())
                        {
                            AI.SelectCard((int)CardId.Gungnir);
                            return true;
                        }
                    }
                    else if (Id == (int)CardId.Solais)
                    {
                        if (IsTheLastPossibility())
                        {
                            AI.SelectCard((int)CardId.Solais);
                            return true;
                        }
                    } 
             } 
            return false;
        }
    }
}
