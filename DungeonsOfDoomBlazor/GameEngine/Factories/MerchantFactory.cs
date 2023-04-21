using DungeonsOfDoomBlazor.GameEngine.Factories.DTO;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.Helpers;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    static class MerchantFactory
    {

        const string _resourceNamespace = "DungeonsOfDoomBlazor.GameEngine.Data.merchants.json";
        static readonly List<Merchant>  merchants = new List<Merchant>();
        
        static MerchantFactory()
        {
            IList<MerchantTemplate> merchantTemplates = JsonSerializationHelper.DeserializeResourceStream<MerchantTemplate>(_resourceNamespace);
            foreach (var template in merchantTemplates) 
            { 
                var m = new Merchant(template.Id, template.Name);
                foreach (var item in template.Inventory)
                {
                    for (int i = 0; i < item.Qty; i++)
                    {
                        m.Inventory.AddItem(ItemFactory.CreateGameItem(item.Id));
                    }
                }
                merchants.Add(m);
            }
        }
        public static Merchant GetMerchantById(int id) => merchants.FirstOrDefault(m => m.Id == id);
    }
}
