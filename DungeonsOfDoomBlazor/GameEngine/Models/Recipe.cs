using DungeonsOfDoomBlazor.GameEngine.Models.Items;

namespace DungeonsOfDoomBlazor.GameEngine.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IList<ItemQuantity> Ingredients { get; set; } = new List<ItemQuantity>();
        public IList<ItemQuantity> Output { get; set; } = new List<ItemQuantity>();
        public Recipe(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddIngredient(int id, int quantity)
        {
            if (!Ingredients.Any(x => x.ItemId == id))
                Ingredients.Add(new ItemQuantity { ItemId = id, Quantity = quantity });
        }

        public void AddOutputItem(int id, int quantity)
        {
            if (!Output.Any(x => x.ItemId == id)) 
                Output.Add(new ItemQuantity { ItemId = id, Quantity = quantity});
        }
        public DisplayMessage ToDisplayMessage()
        {
            var messages = new List<string> 
            {
                "Ingredients:"
            };
            foreach (var item in Ingredients) 
            {
                messages.Add(item.QuantityItemDescription);
            }
            messages.Add("Creates:");
            foreach (var item in Output)
            {
                messages.Add(item.QuantityItemDescription);
            }
            return new DisplayMessage($"Recipe Added - {Name}", messages);
        }
    }
}
