using DungeonsOfDoomBlazor.GameEngine.Models;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Services;

namespace DungeonsOfDoomBlazor.GameEngine.Actions
{
    public class Attack : IAction
    {
        private readonly GameItem _item;
        private readonly IDiceService _diceService;
        private readonly string _dmgDice;
        public Attack(GameItem item, string dmgDice, IDiceService? diceService = null)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            _diceService = diceService ?? DiceService.Instance;

            if (item.Category != ItemCategory.Weapon) throw new ArgumentException($"{item.Name} is not a weapon!");
            if (string.IsNullOrWhiteSpace(dmgDice)) throw new ArgumentException("Damage dice must be of valid dice notation");

            _dmgDice = dmgDice;
        }
        public DisplayMessage Execute(Character actor, Character target)
        {
            _ = actor ?? throw new ArgumentNullException(nameof(actor));
            _ = target ?? throw new ArgumentNullException(nameof(target));

            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";
            string title = (actor is Player) ? $"{actor.Name} Combat" : "Monster Combat";

            int dmg = _diceService.Roll(_dmgDice).Value;
            string message;
            if (dmg == 0) message = $"{actorName} missed {targetName}.";
            else
            {
                target.TakeDamage(dmg);
                message = $"{actorName} hit {targetName} for {dmg} point{(dmg > 1 ? "s" : "")}.";
            }
            //string message = dmg == 0 ? $"{actorName} missed {targetName}." : (target.TakeDamage(dmg), $"{actorName} hit {targetName} for {dmg} point{(dmg > 1 ? "s" : "")}.");
            return new DisplayMessage(title, message);
        }
    }
}
