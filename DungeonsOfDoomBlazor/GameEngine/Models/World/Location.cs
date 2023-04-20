using DungeonsOfDoomBlazor.GameEngine.Factories;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;
using DungeonsOfDoomBlazor.GameEngine.Services;

namespace DungeonsOfDoomBlazor.GameEngine.Models.World
{
    public class Location
    {
        public IList<MonsterEncounter> MonstersHere { get; } = new List<MonsterEncounter>();
        public Merchant? MerchantHere { get; set; } = null;
        public bool HasMerchant => MerchantHere != null;
        public IList<Quest> QuestsAvailableHere { get; } = new List<Quest>();
        public GameItem ItemInRoom { get; set; }
        public int X { get; }
        public int Y { get; }
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public string Image { get; } = string.Empty;
        public Location(int x, int y, string name, string description, string image, Merchant? merchant)
        {
            X = x;
            Y = y;
            Name = name;
            Description = description;
            Image = image;
            MerchantHere = merchant;

        }
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
