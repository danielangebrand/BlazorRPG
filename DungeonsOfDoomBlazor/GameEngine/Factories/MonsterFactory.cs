using DungeonsOfDoomBlazor.GameEngine.Factories.DTO;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Services;
using DungeonsOfDoomBlazor.Helpers;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class MonsterFactory
    {
        //static readonly IDiceService _service = DiceService.Instance;
        const string _resourceNamespace = "DungeonsOfDoomBlazor.GameEngine.Data.monsters.json";
        static readonly IList<MonsterTemplate> _monsterTemplates = JsonSerializationHelper.DeserializeResourceStream<MonsterTemplate>(_resourceNamespace);
        public static Monster GetMonster(int monsterId, IDiceService? dice = null )
        {
            dice ??= DiceService.Instance;
            var template = _monsterTemplates.First(p => p.Id == monsterId);
            var weapon = ItemFactory.CreateGameItem(template.WeaponId);
            var monster = new Monster(template.Id, template.Name, template.Image, template.Dex,
                template.Str, template.AC, template.MaxHP, weapon,
                template.RewardXP, template.Gold, template.DeathMessage);

            foreach(var loot in template.LootItems) 
            {
                AddLootItem(monster, loot.Id, loot.Perc, dice);
            }
            return monster;
        }

        static void AddLootItem(Monster m, int id, int p, IDiceService dice)
        {
            if (dice.Roll("1d100").Value <= p) m.Inventory.AddItem(ItemFactory.CreateGameItem(id));
        }
    }
}
