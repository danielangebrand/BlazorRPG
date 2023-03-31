using DungeonsOfDoomBlazor.GameEngine.Models;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;

namespace DungeonsOfDoomBlazor.GameEngine.Actions
{
    public class Heal : IAction
    {
        private readonly GameItem _item;
        private readonly int _hpToHeal;
        public Heal(GameItem item, int hpToHeal)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            if (item.Category != Models.Enum.ItemCategory.Consumable) throw new ArgumentOutOfRangeException(nameof(item.Category));

            if (hpToHeal <= 0) throw new ArgumentOutOfRangeException($"{item.Name} must have a healing effect.");

            _hpToHeal = hpToHeal;
        }
        public DisplayMessage Execute(Character actor, Character target)
        {
            _ = actor ?? throw new ArgumentNullException(nameof(actor));
            _ = target ?? throw new ArgumentNullException(nameof(target));

            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "yourself." : $"the {target.Name.ToLower()}.";

            target.Heal(_hpToHeal);
            return new DisplayMessage("Heal Effect", $"{actorName} heal {_hpToHeal} point{(_hpToHeal > 1 ? "s" : "")} on {targetName}");
        }
    }
}
