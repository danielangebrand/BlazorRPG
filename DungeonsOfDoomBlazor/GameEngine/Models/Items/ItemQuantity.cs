using DungeonsOfDoomBlazor.GameEngine.Factories;
using System.Security.Cryptography.X509Certificates;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Items
{
    public class ItemQuantity
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string QuantityItemDescription => $"{ItemFactory.GetItemName(ItemId)} (x{Quantity})";
    }
}
