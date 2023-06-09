﻿using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;

namespace DungeonsOfDoomBlazor.GameEngine.Models
{
    public class Inventory
    {
        private readonly List<GameItem> backpack = new List<GameItem>();
        private readonly List<GroupedInventoryItem> backpackGrouped = new List<GroupedInventoryItem>();
        public IList<GameItem> Weapons => Items.Where(i => i.Category == ItemCategory.Weapon).ToList();
        public IReadOnlyList<GameItem> Items => backpack.AsReadOnly();
        public IReadOnlyList<GroupedInventoryItem> GroupedItems => backpackGrouped.AsReadOnly();
        public IList<GameItem> Consumables => Items.Where(i => i.Category == ItemCategory.Consumable).ToList();

        public Inventory()
        {
        }

        public Inventory(IEnumerable<GameItem> items)
        {
            if (items == null) return;
            foreach (GameItem i in items) AddItem(i);
        }

        public void AddItem(GameItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            backpack.Add(item);

            if (item.IsUnique)
            {
                backpackGrouped.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (backpackGrouped.All(g => g.Item.Id != item.Id))
                {
                    backpackGrouped.Add(new GroupedInventoryItem(item, 0));
                }
                backpackGrouped.First(g => g.Item.Id == item.Id).Quantity++;
            }
        }

        public void RemoveItem(GameItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            backpack.Remove(item);
            if (!item.IsUnique)
            {
                GroupedInventoryItem gItemToRemove = backpackGrouped.FirstOrDefault(g => g.Item.Id == item.Id);
                if (gItemToRemove != null)
                {
                    if (gItemToRemove.Quantity == 1) backpackGrouped.Remove(gItemToRemove);
                    else gItemToRemove.Quantity--;
                }
            }
        }

        public void RemoveItems(IList<ItemQuantity> itemQuantities)
        {
            _ = itemQuantities ?? throw new ArgumentNullException(nameof(itemQuantities));
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    RemoveItem(Items.First(item => item.Id == itemQuantity.ItemId));
                }
            }
        }
        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items)
        {
            return items.All(item => Items.Count(i => i.Id == item.ItemId) >= item.Quantity);
        }
    }
}
