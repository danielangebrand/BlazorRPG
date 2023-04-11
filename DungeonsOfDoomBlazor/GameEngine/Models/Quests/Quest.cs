using DungeonsOfDoomBlazor.GameEngine.Models.Items;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Quests
{
    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<ItemQuantity> ItemsToComplete { get; set; } = Array.Empty<ItemQuantity>();
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public IList<ItemQuantity> RewardItems { get; set; } = Array.Empty<ItemQuantity>();
        public DisplayMessage ToDisplayMessage()
        {
            var messages = new List<string>()
            {
                Description,
                "Items to complete the quest:"
            };

            foreach (ItemQuantity q in ItemsToComplete)
            {
                messages.Add(q.QuantityItemDescription);
            }
            messages.Add("Rewards for quest completion:");
            messages.Add($"{RewardExperiencePoints} XP");
            messages.Add($"{RewardGold} gold");
            foreach (ItemQuantity q in RewardItems) 
            {
                messages.Add(q.QuantityItemDescription);
            }
            return new DisplayMessage($"Quest Added - {Name}", messages);
        }
    }
}
