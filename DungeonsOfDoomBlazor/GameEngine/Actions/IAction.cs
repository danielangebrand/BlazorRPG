using DungeonsOfDoomBlazor.GameEngine.Models;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;

namespace DungeonsOfDoomBlazor.GameEngine.Actions
{
    public interface IAction
    {
        DisplayMessage Execute(Character actor, Character target);
    }
}
