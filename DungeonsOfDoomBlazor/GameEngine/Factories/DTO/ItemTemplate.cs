using DungeonsOfDoomBlazor.GameEngine.Models.Enum;

namespace DungeonsOfDoomBlazor.GameEngine.Factories.DTO
{
    public class ItemTemplate
    {
        public int Id { get; set; }
        public ItemCategory Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Damage { get; set; } = string.Empty;
        public int Heals { get; set; }
        //public bool? MaleOnly { get; set; } = false;
    }
}
