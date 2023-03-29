using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;
using System.Xml.Linq;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    static class QuestFactory
    {
        static readonly IList<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItems = new List<ItemQuantity>();

            itemsToComplete.Add(new ItemQuantity { ItemId = 9002, Quantity = 5 });
            rewardItems.Add(new ItemQuantity { ItemId = 1002, Quantity = 1 });

            _quests.Add(new Quest
            {
                Id = 1,
                Name = "Clear the herb garden",
                Description = "Defeat the snakes in the Herbalist's garden",
                ItemsToComplete = itemsToComplete,
                RewardGold = 25,
                RewardExperiencePoints = 10,
                RewardItems = rewardItems
            });
        }

        public static Quest GetQuestById(int id)
        {
            return _quests.First(quest => quest.Id == id);
        }
    }
}
