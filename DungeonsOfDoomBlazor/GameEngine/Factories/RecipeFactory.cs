using DungeonsOfDoomBlazor.GameEngine.Factories.DTO;
using DungeonsOfDoomBlazor.GameEngine.Models;
using DungeonsOfDoomBlazor.Helpers;

namespace DungeonsOfDoomBlazor.GameEngine.Factories
{
    internal static class RecipeFactory
    {
        const string _resourceNamespace = "DungeonsOfDoomBlazor.GameEngine.Data.recipes.json";
        static readonly IList<RecipeTemplate> _recipeTemplates = JsonSerializationHelper.DeserializeResourceStream<RecipeTemplate>(_resourceNamespace);

        public static Recipe GetRecipeById(int id)
        {
            var template = _recipeTemplates.First(p => p.Id == id);
            var recipe = new Recipe(template.Id, template.Name);

            foreach (var req in template.Ingredients) recipe.AddIngredient(req.Id, req.Qty);
            foreach(var item in template.OutputItems) recipe.AddOutputItem(item.Id, item.Qty);
            return recipe;
        }
    }
}
