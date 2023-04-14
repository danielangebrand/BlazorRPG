using DungeonsOfDoomBlazor.GameEngine.Models.Characters;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    static class MerchantFactory
    {
        static readonly List<Merchant> _merchant = new List<Merchant>();
        
        static MerchantFactory()
        {
            _merchant.Add(CreateMerchant(101, "Sussie"));
            _merchant.Add(CreateMerchant(102, "Per-Olsson"));
            _merchant.Add(CreateMerchant(103, "Snoop Dawg"));
        }
        public static Merchant GetMerchantById(int id) => _merchant.FirstOrDefault(m => m.Id == id);
        static Merchant CreateMerchant(int id, string name)
        {
            var m = new Merchant(id, name);
            m.Inventory.AddItem(ItemFactory.CreateGameItem(1001));
            m.Inventory.AddItem(ItemFactory.CreateGameItem(1003));
            m.Inventory.AddItem(ItemFactory.CreateGameItem(1004));
            return m;
        }
    }
}
