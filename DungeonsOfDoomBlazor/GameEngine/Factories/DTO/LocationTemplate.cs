namespace DungeonsOfDoomBlazor.GameEngine.Factories.DTO
{
    public class LocationTemplate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int? MerchantId { get; set; }
        public IEnumerable<int> Quests { get; set; } = new List<int>();
        public IEnumerable<MonsterEncounterItem> Monsters { get; set; } = new List<MonsterEncounterItem>();
    }
}
