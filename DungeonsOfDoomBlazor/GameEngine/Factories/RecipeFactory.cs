using DungeonsOfDoomBlazor.GameEngine.Models;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class RecipeFactory
    {
        static readonly List<Recipe> recipes = new List<Recipe>();
        static RecipeFactory()
        {
            Recipe snus = new Recipe(1, "Snus Recipe");
            snus.AddIngredient(3001, 1);
            snus.AddIngredient(3002, 1);
            snus.AddIngredient(3003, 1);
            snus.AddOutputItem(2001, 1);
            recipes.Add(snus);
        }

        public static Recipe GetRecipeById(int id)
        {
            return recipes.First(x => x.Id == id);
        }
    }
}
