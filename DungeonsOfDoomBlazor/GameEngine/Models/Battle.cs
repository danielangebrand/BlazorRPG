using Blazorise;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Services;

namespace DungeonsOfDoomBlazor.GameEngine.Models
{
    public class Battle
    {
        private readonly DisplayMessageBroker _messageBroker = DisplayMessageBroker.Instance;
        private readonly Action _onPlayerKilled;
        private readonly Action _onOpponentKilled;
        private readonly IDiceService _diceService;

        public Battle(Action onPlayerKilled, Action onOpponentKilled, IDiceService diceService)
        {
            _onOpponentKilled = onOpponentKilled;
            _onPlayerKilled = onPlayerKilled;
            _diceService = diceService;
        }

        public void Attack(Player player, Monster opponent)
        {
            _ = player ?? throw new ArgumentNullException(nameof(player));
            _ = opponent ?? throw new ArgumentNullException(nameof(opponent));

            if (FirstAttacker(player, opponent) == Combatant.Player)
            {
                AttackOpponent(player, opponent);
            }
            else 
            {
                bool battleContinues = AttackPlayer(player, opponent);
                if (battleContinues) 
                {
                    AttackOpponent(player, opponent);
                }
            }
        }

        private Combatant FirstAttacker(Player player, Monster opponent)
        {
            int playerBonus = Helpers.AbilityCalculator.CalculateBonus(player.Dexterity);
            int opponentBonus = Helpers.AbilityCalculator.CalculateBonus(opponent.Dexterity);
            int playerInitiative = _diceService.Roll("1d20").Value + playerBonus;
            int opponentInititative = _diceService.Roll("1d20").Value + opponentBonus;

            return (playerInitiative >= opponentInititative) ? Combatant.Player : Combatant.Opponent;
        }

        private bool AttackOpponent(Player player, Monster opponent)
        {
            if (player.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage(new DisplayMessage("Combat Warning", "You must select a weapon to attack."));
                return false;
            }
            var message = player.UseCurrentWeaponOn(opponent);
            _messageBroker.RaiseMessage(message);
            if (!opponent.IsAlive)
            {
                OnOpponentKilled(player, opponent);
                return false;
            }

            return true;
        }

        private bool AttackPlayer(Player player, Monster opponent)
        {
            var message = opponent.UseCurrentWeaponOn(player);
            _messageBroker.RaiseMessage(message);

            if (!player.IsAlive)
            {
                OnPlayerKilled(player, opponent);
                return false;
            }
            return true;
        }

        private void OnPlayerKilled(Player player, Monster opponent)
        {
            _messageBroker.RaiseMessage(new DisplayMessage($"{player.Name} Defeated", $"The {opponent.Name} {opponent.DeathMessage}."));
            _messageBroker.RaiseMessage(new DisplayMessage("You wake up in your home.", player.DeathMessage));
            player.FullHeal();
            _onPlayerKilled.Invoke();
        }

        private void OnOpponentKilled(Player player, Monster opponent)
        {
            var messages = new List<string>();
            messages.Add($"You defeated the {opponent.Name}!");
            player.AddXP(opponent.RewardExperiencePoints);
            messages.Add($"You recieve {opponent.RewardExperiencePoints} xp.");

            player.ReceiveGold(opponent.Gold);
            messages.Add($"You recieve {opponent.Gold} gold");

            foreach (GameItem item in opponent.Inventory.Items)
            {
                player.Inventory.AddItem(item);
                messages.Add($"You recieved {item.Name}");
            }

            _messageBroker.RaiseMessage(new DisplayMessage("Monster Defeated", messages));
            _onOpponentKilled.Invoke();
        }
    }
}
