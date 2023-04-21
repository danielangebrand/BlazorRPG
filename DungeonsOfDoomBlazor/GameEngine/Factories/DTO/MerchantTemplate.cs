namespace DungeonsOfDoomBlazor.GameEngine.Factories.DTO
{
    public class MerchantTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<IdQuantityItem> Inventory { get; set; } = new List<IdQuantityItem>();
    }
}
