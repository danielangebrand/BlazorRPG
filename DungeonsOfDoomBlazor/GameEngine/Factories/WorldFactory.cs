using DungeonsOfDoomBlazor.GameEngine.Factories.DTO;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;
using DungeonsOfDoomBlazor.GameEngine.Models.World;
using DungeonsOfDoomBlazor.Helpers;
using System.Xml.Linq;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class WorldFactory
    {
        const string _resourceNamespace = "DungeonsOfDoomBlazor.GameEngine.Data.locations.json";
        internal static World CreateWorld()
        {
            var locationTemplates = JsonSerializationHelper.DeserializeResourceStream<LocationTemplate>(_resourceNamespace);
            var world = new World();
            
            foreach (var template in locationTemplates) 
            {
                var merchant = (template.MerchantId is null) ? null : MerchantFactory.GetMerchantById(template.MerchantId.Value);
                var loc = new Location(template.X, template.Y, template.Name, template.Description,
                    template.Image, merchant);
                foreach (var questId in template.Quests)
                {
                    loc.QuestsAvailableHere.Add(QuestFactory.GetQuestById(questId));
                }
                foreach (var encounter in template.Monsters)
                {
                    loc.AddMonsterEncounter(encounter.Id, encounter.Perc);
                }
                world.AddLocation(loc);
            }
            return world;
        }
    }
}