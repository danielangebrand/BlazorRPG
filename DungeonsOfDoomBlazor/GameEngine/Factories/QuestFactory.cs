using DungeonsOfDoomBlazor.GameEngine.Factories.DTO;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;
using DungeonsOfDoomBlazor.Helpers;
using System.Xml.Linq;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    static class QuestFactory
    {
        const string _resourceNameSpace = "DungeonsOfDoomBlazor.GameEngine.Data.quests.json";
        static readonly IList<QuestTemplate> _questTemplates = JsonSerializationHelper.DeserializeResourceStream<QuestTemplate>(_resourceNameSpace);
        public static Quest GetQuestById(int id)
        {
            var template = _questTemplates.First(p => p.Id == id);
            var quest = new Quest(template.Id, template.Name, template.Description, template.RewardGold, template.RewardXP);

            foreach(var requirements in template.Requirements) 
            {
                quest.ItemsToComplete.Add(new ItemQuantity
                {
                    ItemId = requirements.Id,
                    Quantity = requirements.Qty
                });
            }
            foreach(var item in template.RewardItems) 
            {
                quest.RewardItems.Add(new ItemQuantity
                {
                    ItemId = item.Id,
                    Quantity = item.Qty
                }); 
            }
            return quest;
        }
    }
}

        //static readonly IList<Quest> _quests = new List<Quest>();
            //switch (id)
            //{
            //    case 1:
            //        List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            //        List<ItemQuantity> rewardItems = new List<ItemQuantity>();

            //        itemsToComplete.Add(new ItemQuantity { ItemId = 9002, Quantity = 5 });
            //        rewardItems.Add(new ItemQuantity { ItemId = 1002, Quantity = 1 });
            //        return new Quest
            //        {
            //            Id = 1,
            //            Name = "Clear the herb garden",
            //            Description = "Defeat the snakes in the Herbalist's garden",
            //            ItemsToComplete = itemsToComplete,
            //            RewardGold = 25,
            //            RewardExperiencePoints = 10,
            //            RewardItems = rewardItems
            //        };
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(id));
            //}