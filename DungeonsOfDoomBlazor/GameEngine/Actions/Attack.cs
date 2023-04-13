using DungeonsOfDoomBlazor.GameEngine.Models;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Services;
using DungeonsOfDoomBlazor.Helpers;

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
            string message;
            int dmg = _diceService.Roll(_dmgDice).Value;

            if (!AttackSucceeded(actor, target))
            {
                message = $"{actorName} missed {targetName}.";
            }
            else
            {
                target.TakeDamage(dmg);
                message = $"{actorName} hit {targetName} for {dmg} point{(dmg > 1 ? "s" : "")}.";
            }
            return new DisplayMessage(title, message);
        }

        private bool AttackSucceeded(Character actor, Character target)
        {
            int actorBonus = AbilityCalculator.CalculateBonus(actor.Strength);
            //int actorAttack = _diceService.Roll(20).Value + actorBonus + actor.Level;
            int actorAttack = _diceService.Roll("1d20").Value + actorBonus + actor.Level;
            int targetArmor = target.ArmorClass + AbilityCalculator.CalculateBonus(target.Dexterity);
            return actorAttack >= targetArmor;
        }
    }
}
