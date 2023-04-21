using DungeonsOfDoomBlazor.GameEngine.Actions;
using DungeonsOfDoomBlazor.GameEngine.Factories.DTO;
using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.Helpers;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class ItemFactory
    {
        static List<GameItem> standardGameItems = new List<GameItem>();
        const string _resourceNameSpace = "DungeonsOfDoomBlazor.GameEngine.Data.items.json";

        static ItemFactory()
        {
            Load();
        }

        static void Load()
        {
            var templates = JsonSerializationHelper.DeserializeResourceStream<ItemTemplate>(_resourceNameSpace);
            foreach (var tmp in templates)
            {
                switch (tmp.Category)
                {
                    case ItemCategory.Weapon:
                        BuildWeapon(tmp.Id, tmp.Name, tmp.Description, tmp.Price, tmp.Damage);
                        break;
                    case ItemCategory.Consumable:
                        BuildHealingItem(tmp.Id, tmp.Name, tmp.Description, tmp.Price, tmp.Heals);
                        break;
                    //case ItemCategory.Armor:
                    //    //BuildArmor(tmp.Id, tmp.Name, tmp.Description, tmp.Price, tmp.Heals);
                    //    break;
                    default:
                        BuildMiscellaneousItem(tmp.Id, tmp.Name, tmp.Description, tmp.Price);
                        break;
                }
            }
        }

        private static void BuildHealingItem(int id, string name, string description, int price, int healEffect)
        {
            GameItem consumable = new GameItem(id, ItemCategory.Consumable, name, description, price);
            consumable.SetAction(new Heal(consumable, healEffect));
            standardGameItems.Add(consumable);
        }

        public static GameItem CreateGameItem(int id)
        {
            var standardItem = standardGameItems.FirstOrDefault(i => i.Id == id);
            return standardItem.Clone();
        }
        static void BuildMiscellaneousItem(int id, string name, string description, int price) =>
            standardGameItems.Add(new GameItem(id, ItemCategory.Miscellaneous, name, description, price));
        static void BuildWeapon(int id, string name, string description, int price, string dmg)
        {
            var wpn = new GameItem(id, ItemCategory.Weapon, name, description, price, true);
            wpn.SetAction(new Attack(wpn, dmg));
            standardGameItems.Add(wpn);
        }
        public static string GetItemName(int id) => standardGameItems.FirstOrDefault(i => i.Id == id)?.Name ?? "";
    }
}