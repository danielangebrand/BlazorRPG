using DungeonsOfDoomBlazor.GameEngine.Models.Items;

namespace DungeonsOfDoomBlazor.GameEngine.Models
{
    public class GroupedInventoryItem
    {
        public GameItem Item { get; set; } = GameItem.Empty;
        public int Quantity { get; set; }
    }
}
