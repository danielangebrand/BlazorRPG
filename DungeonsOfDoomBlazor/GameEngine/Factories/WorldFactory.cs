using DungeonsOfDoomBlazor.GameEngine.Factories;
using DungeonsOfDoomBlazor.GameEngine.Factories.DTO;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;
using DungeonsOfDoomBlazor.GameEngine.Models.World;
using DungeonsOfDoomBlazor.Helpers;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class WorldFactory
    {
        const string _resourceNamespaceLocations = "DungeonsOfDoomBlazor.GameEngine.Data.locations.json";
        const string _resourceNamespaceDungeons = "DungeonsOfDoomBlazor.GameEngine.Data.dungeons.json";
        internal static List<World> CreateWorld()
        {
            List<World> worlds = new();

            //village
            var villageTemplate = JsonSerializationHelper.DeserializeResourceStream<LocationTemplate>(_resourceNamespaceLocations);
            var village = CreateWorldFromTemplate(villageTemplate);
            village.Name = "Village";
            worlds.Add(village);

            //dungeons
            var dungeonTemplates = JsonSerializationHelper.DeserializeResourceStream<LocationTemplate>(_resourceNamespaceDungeons);
            var dungeon = CreateWorldFromTemplate(dungeonTemplates);
            dungeon.Name = "Dungeons of Doom";
            worlds.Add(dungeon);
            return worlds;
        }

        private static World CreateWorldFromTemplate(IList<LocationTemplate> locationTemplates)
        {
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