using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Tracing;

namespace DungeonsOfDoomBlazor.GameEngine.ViewModels
{
    public class MerchantVM
    {
        public Merchant? Merchant { get; set; } = null;
        public Player? Player { get; set; } = null;
        public string ErrorMessage { get; set; } = string.Empty;
        public EventCallback InventoryChanged { get; set; }
        public void OnSellItem(GameItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            if (Player != null && Merchant != null)
            {
                Player.ReceiveGold(item.Price);
                Merchant.Inventory.AddItem(item);
                Player.Inventory.RemoveItem(item);

                InventoryChanged.InvokeAsync(null);
            }
        }

        public void OnBuyItem(GameItem item)
        {
            _ = item ?? throw new ArgumentException(nameof(item));
            if (Player != null && Merchant != null)
            {
                ErrorMessage = string.Empty;
                if (Player.Gold >= item.Price)
                {
                    Player.SpendGold(item.Price);
                    Player.Inventory.AddItem(item);
                    Merchant.Inventory.RemoveItem(item);
                    InventoryChanged.InvokeAsync(null);
                }
                else ErrorMessage = "Not enough gold!";
            }
        }
    }
}
