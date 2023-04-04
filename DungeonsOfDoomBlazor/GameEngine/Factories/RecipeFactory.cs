using DungeonsOfDoomBlazor.GameEngine.Models;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    public class RecipeFactory
    {
        static readonly List<Recipe> Recipes = new List<Recipe>();
        public RecipeFactory()
        {
            Recipe snus = new Recipe(1, "Snus Recipe");
            snus.AddIngredient(3001, 1);
            snus.AddIngredient(3002, 1);
            snus.AddIngredient(3003, 1);
            snus.AddOutputItem(2001, 1);
            Recipes.Add(snus);
        }

        public static Recipe GetRecipeById(int id)
        {
            return Recipes.First(x => x.Id == id);
        }
    }
}
