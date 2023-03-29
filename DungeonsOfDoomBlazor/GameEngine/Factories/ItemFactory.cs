using DungeonsOfDoomBlazor.GameEngine.Models.Items;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class ItemFactory
    {
        private static List<GameItem> standardGameItems = new List<GameItem>
        {
            new Weapon(1, "Pointy Stick", "Pokeballs are trending. Everything with poke is pretty neat! Atk: +1-2.", 5, "1d2"),
            new Weapon(2, "Rusty 'ol S", "Your grandfathers sword. This could come in handy.. ", 1, "1d5"),
            new Weapon(3, "Gentlemens Club", "Welcome to the patriarchy! Atk: +5-10. Restriction: Usable by men only.",50, "3d4"),
            new Weapon(4, "Dagger of Swagger", "Swag is all you need! Atk: +7-15.",200, "3d5"),
            new GameItem(9001, "Snake fang", "FANK YOU!", 1),
            new GameItem(9002, "Snakeskin", "", 2),
            new GameItem(9003, "Rat tail", "", 1),
            new GameItem(9004, "Rat fur", "", 2),
            new GameItem(9005, "Spider fang","", 1),
            new GameItem(9006, "Spider silk","", 2),
            new GameItem(9007, "Dirty Underwear","This odor makes monsters smell me from a mile away.. ", -10),
            new GameItem(9008, "GitHub login","Hm.. This might come in handy if I encounter JavaScript in the future..", 2),
        };

        public static GameItem? CreateGameItem(int id)
        {
            var standardItem = standardGameItems.FirstOrDefault(i => i.Id == id);
            return standardItem?.Clone();
        }
    }
}
