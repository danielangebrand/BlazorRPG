using DungeonsOfDoomBlazor.GameEngine.Models.Quests;
using DungeonsOfDoomBlazor.GameEngine.Models.World;
using System.Xml.Linq;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            var locations = new List<Location>()
            {
                new Location
                {
                    X = -2,
                    Y = -1,
                    Name = "Farmer's Field",
                    Description = "There are rows of corn growing here, with giant rats between them.",
                    Image = "/images/locations/Farmfields.png"
                },
                                new Location
                {
                    X = -1,
                    Y = -1,
                    Name = "Farmer's House",
                    Description = "Your friendly neighbour.",
                    Image = "/images/locations/Farmhouse.png",
                    MerchantHere = MerchantFactory.GetMerchantById(102)
                },
                                                new Location
                {
                    X = -0,
                    Y = -1,
                    Name = "Home",
                    Description = "This is your home.",
                    Image = "/images/locations/home.png"
                },
                new Location
                {
                    X = -1,
                    Y = 0,
                    Name = "Merchant Shop",
                    Description = "The shop of Sussie, the trader",
                    Image = "/images/locations/trader.png",
                    MerchantHere = MerchantFactory.GetMerchantById(101)       
                },

                new Location
                {
                    X = 0,
                    Y = 0,
                    Name = "Town Square",
                    Description = "You see a fountain here.",
                    Image = "/images/locations/TownSquare.png"
                },
                new Location
                {
                    X = 1,
                    Y = 0,
                    Name = "Town Gate",
                    Description = "There is a gate here, protecting the town from unimaginable horrors.",
                    Image = "/images/locations/TownGate.png"
                },
                new Location
                {
                    X = 2,
                    Y = 0,
                    Name = "Spider Forest",
                    Description = "The trees in this forest are covered with spider webs.",
                    Image = "/images/locations/SpiderForest.png"
                },
                new Location
                {
                    X = 0,
                    Y = 1,
                    Name = "Herbalist's Hut",
                    Description = "You see a small hut, with plants drying from the roof.",
                    Image = "/images/locations/HerbalistsHut.png",
                    MerchantHere = MerchantFactory.GetMerchantById(103),
                    QuestsAvailableHere = new List<Quest> { QuestFactory.GetQuestById(1) }
                    
                },
                new Location
                {
                    X = 0,
                    Y = 2,
                    Name = "Herbalist's Garden",
                    Description = "There are many plants here, with snakes hiding behind them.",
                    Image = "/images/locations/HerbalistsGarden.png"
                },
            };
            var world = new World(locations);
            world.LocationAt(-2, -1).AddMonsterEncounter(2, 100);
            world.LocationAt(2, 0).AddMonsterEncounter(3, 100);
            world.LocationAt(0, 2).AddMonsterEncounter(1, 100);
            world.LocationAt(1, 0).AddMonsterEncounter(4, 100);
            return world;
        }
    }
}
