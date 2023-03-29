using DungeonsOfDoomBlazor.GameEngine.Models.Items;

namespace DungeonsOfDoomBlazor.GameEngine.Models
{
    public class Inventory
    {
        private readonly List<GameItem> backpack = new List<GameItem>();
        private readonly List<GroupedInventoryItem> backpackGrouped = new List<GroupedInventoryItem>();
        public IEnumerable<GameItem> Weapons => backpack.Where(w => w is Weapon);
        public IReadOnlyList<GameItem> Items => backpack.AsReadOnly();
        public IReadOnlyList<GroupedInventoryItem> GroupedItems => backpackGrouped.AsReadOnly();
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
                backpackGrouped.Add(new GroupedInventoryItem { Item = item, Quantity = 1 });
            }
            else
            {
                if (backpackGrouped.All(g => g.Item.Id != item.Id))
                {
                    backpackGrouped.Add(new GroupedInventoryItem { Item = item, Quantity = 0 });
                }
                backpackGrouped.First(g => g.Item.Id == item.Id).Quantity++;
            }
        }

        public void RemoveItem(GameItem item) 
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            backpack.Remove(item);

            GroupedInventoryItem gItemToRemove = backpackGrouped.FirstOrDefault(g => g.Item == item);
            if (gItemToRemove != null)
            {
                if (gItemToRemove.Quantity == 1) backpackGrouped.Remove(gItemToRemove);
                else gItemToRemove.Quantity--;
            }
        }
    }
}
