using DungeonsOfDoomBlazor.GameEngine.Actions;
using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class ItemFactory
    {
        static List<GameItem> standardGameItems = new List<GameItem>();

        //    new Weapon(1001, "Pointy Stick", "Pokeballs are trending. Everything with poke is pretty neat! Atk: +1-2.", 5, "1d2"),
        //    new Weapon(1002, "Rusty 'ol S", "Your grandfathers sword. This could come in handy.. ", 1, "1d5"),
        //    new Weapon(1003, "Gentlemens Club", "Welcome to the patriarchy! Atk: +5-10. Restriction: Usable by men only.",50, "3d4"),
        //    new Weapon(1004, "Dagger of Swagger", "Swag is all you need! Atk: +7-15.",200, "3d5"),
        //    BuildMiscellaneousItem(9001, "Snake fang", "FANK YOU!", 1),
        //    BuildMiscellaneousItem(9002, "Snakeskin", "", 2);

        static ItemFactory()
        {
            BuildWeapon(1001, "Pointy Stick", "Pokeballs and Pokemon are trending. Everything with poke is pretty neat! Atk +1-2", 5, "1d2");
            BuildWeapon(1002, "Rusty 'ol S", "Your grandfathers sword. This could come in handy.. ", 1, "1d5");
            BuildWeapon(1003, "Gentlemens Club", "Welcome to the patriarchy! Atk: +5-10. Restriction: Usable by men only.", 50, "3d4");
            BuildWeapon(1004, "Dagger of Swagger", "Swag is all you need! Atk: +7-15.", 200, "3d5");
            BuildMiscellaneousItem(9001, "Snake fang", "", 1);
            BuildMiscellaneousItem(9002, "Snakeskin", "", 2);
            BuildMiscellaneousItem(9003, "Rat tail", "", 1);
            BuildMiscellaneousItem(9004, "Rat fur", "", 2);
            BuildMiscellaneousItem(9005, "Spider fang", "", 1);
            BuildMiscellaneousItem(9006, "Spider silk", "", 2);
            BuildMiscellaneousItem(9007, "Dirty Underwear", "This odor makes monsters smell me from a mile away.. ", -10);
            BuildMiscellaneousItem(9008, "GitHub login", "Hm.. This might come in handy if I encounter JavaScript in the future..", 2);
        }

        public static GameItem CreateGameItem(int id)
        {
            var standardItem = standardGameItems.FirstOrDefault(i => i.Id == id);
            return standardItem.Clone();
        }
        public static void BuildMiscellaneousItem(int id, string name, string description, int price)
        {
            standardGameItems.Add(new GameItem(id, ItemCategory.Miscellaneous, name, description, price));
        }
        static void BuildWeapon(int id, string name, string description, int price, string dmg)
        {
            var wpn = new GameItem(id, ItemCategory.Weapon, name, description, price, true);
            wpn.Action = new Attack(wpn, dmg);
            standardGameItems.Add(wpn);
        }
    }
}