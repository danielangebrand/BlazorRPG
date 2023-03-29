using DungeonsOfDoomBlazor.GameEngine.Models.Characters;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    static class MerchantFactory
    {
        private static readonly List<Merchant> _merchant = new List<Merchant>();
        
        static MerchantFactory()
        {
            _merchant.Add(CreateMerchant(101, "Sussie"));
            _merchant.Add(CreateMerchant(102, "Per-Olsson"));
            _merchant.Add(CreateMerchant(103, "Snoop Dawg"));
        }
        public static Merchant GetMerchantById(int id) => _merchant.FirstOrDefault(m => m.Id == id);
        static Merchant CreateMerchant(int id, string name)
        {
            var m = new Merchant
            {
                Id = id,
                Name = name,
                Level = 0,
                Gold = 100,
                MaxHealth = 999,
                Health = 999,
            };
            m.Inventory.AddItem(ItemFactory.CreateGameItem(1001));
            return m;
        }
    }
}
