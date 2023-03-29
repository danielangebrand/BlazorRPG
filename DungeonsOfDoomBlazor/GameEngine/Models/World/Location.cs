using DungeonsOfDoomBlazor.GameEngine.Factories;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Services;

namespace DungeonsOfDoomBlazor.GameEngine.Models.World
{
    public class Location
    {
        public IList<MonsterEncounter> MonstersHere { get; set; } = new List<MonsterEncounter>();
        public Merchant? MerchantHere { get; set; } = null;
        public bool HasMerchant => MerchantHere != null;
        public IList<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();
        public GameItem ItemInRoom { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public void AddMonsterEncounter(int monsterId, int chanceOfEncounter)
        {
            if (MonstersHere.Any(m => m.MonsterId == monsterId))
            {
                MonstersHere.First(m => m.MonsterId == monsterId).ChanceOfEncountering = chanceOfEncounter;
            }
            else
            {
                MonstersHere.Add(new MonsterEncounter(monsterId, chanceOfEncounter));
            }
        }
        public bool HasMonster() => MonstersHere.Any();
        public Monster GetMonster()
        {
            if (HasMonster() == false) throw new InvalidOperationException();

            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);
            var result = DiceService.Instance.Roll(totalChances.ToString());
            int runningTotal = 0;
            foreach (MonsterEncounter monsterEncounter in MonstersHere) 
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;
                if (result.Value <= runningTotal) return MonsterFactory.GetMonster(monsterEncounter.MonsterId);
            }

            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterId);
        }
    }
}
