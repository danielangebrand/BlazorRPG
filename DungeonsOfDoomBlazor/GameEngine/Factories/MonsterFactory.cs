using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Services;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class MonsterFactory
    {
        static readonly IDiceService _service = DiceService.Instance;
        public static Monster GetMonster(int id)
        {
            switch (id)
            {
                case 1:
                    Monster snake = new Monster
                    {
                        Name = "Snake",
                        Image = "/images/monsters/snake.png",
                        Health = 4,
                        MaxHealth = 4,
                        RewardExperiencePoints = 5,
                        Gold = 1,
                        DamageRoll = "1d2",
                        KillMessage = "does a little dance over your paralyzed body and slowly devours you."
                    };

                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);
                    return snake;

                case 2:
                    Monster rat = new Monster()
                    {
                        Name = "Rat",
                        Image = "/images/monsters/rat.png",
                        Health = 5,
                        MaxHealth = 5,
                        RewardExperiencePoints = 5,
                        Gold = 1,
                        DamageRoll = "1d2",
                        KillMessage = "eats you alive."

                    };

                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);
                    return rat;

                case 3:
                    Monster giantSpider = new Monster()
                    {
                        Name = "Giant Spider",
                        Image = "/images/monsters/giantspider.png",
                        Health = 10,
                        MaxHealth = 10,
                        RewardExperiencePoints = 10,
                        Gold = 3,
                        DamageRoll = "1d4",
                        KillMessage = "kills you and lay some eggs inside your body."

                    };
                    AddLootItem(giantSpider, 9005, 25);
                    AddLootItem(giantSpider, 9006, 75);
                    return giantSpider;

                case 4:
                    Monster geek = new Monster()
                    {
                        Name = "Shrieking Geek",
                        Image = "/images/monsters/geek.png",
                        Health = 10,
                        MaxHealth = 10,
                        RewardExperiencePoints = 100,
                        Gold = 50,
                        DamageRoll = "2d10",
                        KillMessage = "wins the argument and declares you stupid."
                    };

                    AddLootItem(geek, 9007, 25);
                    AddLootItem(geek, 9008, 75);
                    return geek;

                default:
                    throw new ArgumentOutOfRangeException(nameof(id));
            }

        }

        private static void AddLootItem(Monster m, int id, int p)
        {
            if (_service.Roll("1d100").Value <= p) m.Inventory.AddItem(ItemFactory.CreateGameItem(id));
        }
    }
}
