using DungeonsOfDoomBlazor.GameEngine.Factories;
using DungeonsOfDoomBlazor.GameEngine.Models;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;
using DungeonsOfDoomBlazor.GameEngine.Models.World;
using DungeonsOfDoomBlazor.GameEngine.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoomBlazor.GameEngine.ViewModels
{
    internal class GameSession : IGameSession
    {
        readonly int _maximumMessagesCount = 100;
        readonly World _currentWorld;
        readonly IDiceService _diceService;
        private readonly Dictionary<string, Action> _userInputAction = new Dictionary<string, Action>();
        public Player CurrentPlayer { get; private set; } 

        public Location CurrentLocation { get; private set; }

        public Movement Movement { get; private set; }
        public Monster? CurrentMonster { get; private set; }
        public bool HasMonster => CurrentMonster != null;
        public IList<DisplayMessage> Messages { get; } = new List<DisplayMessage>();

        public Merchant? CurrentMerchant { get; set; }


        public GameSession(int maxMessageCount) : this()
        {
            _maximumMessagesCount = maxMessageCount;
        }

        public GameSession(IDiceService? diceService = null)
        {
            _diceService = diceService ?? DiceService.Instance;
            InitializeUserInputActions();
            CurrentPlayer = new Player
            {
                Name = "Mojo man",
                CharacterClass = CharacterClass.Nerd.ToString(),
                Health = 30,
                Damage = 10,
                X = 0,
                Y = 0,
                G = Gender.Undecided
            };
            _currentWorld = WorldFactory.CreateWorld();
            Movement = new Movement(_currentWorld);
            CurrentLocation = this.Movement.CurrentLocation;
            if (!CurrentPlayer.Inventory.Weapons.Any()) CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(1001));

            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(2001));
            CurrentPlayer.LearnRecipe(RecipeFactory.GetRecipeById(1));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3003));
        }

        private void InitializeUserInputActions()
        {
            _userInputAction.Add("W", () => Movement.MoveNorth());
            _userInputAction.Add("A", () => Movement.MoveWest());
            _userInputAction.Add("S", () => Movement.MoveSouth());
            _userInputAction.Add("D", () => Movement.MoveEast()); 
            _userInputAction.Add("ARROWUP", () => Movement.MoveNorth());
            _userInputAction.Add("ARROWLEFT", () => Movement.MoveWest());
            _userInputAction.Add("ARROWDOWN", () => Movement.MoveSouth());
            _userInputAction.Add("ARROWRIGHT", () => Movement.MoveEast());
        }

        public void ChangeGender()
        {
            Gender g = (Gender)Random.Shared.Next(1, 4);
            this.CurrentPlayer.G = g;
        }

        public void OnLocationChanged(Location newLocation)
        {
            _ = newLocation ?? throw new ArgumentNullException(nameof(newLocation));

            CurrentLocation = newLocation;
            Movement.UpdateLocation(CurrentLocation);
            GetMonsterAtCurrentLocation();
            CompleteQuestsAtLocation();
            GetQuestsAtLocation();
            CurrentMerchant = CurrentLocation.MerchantHere;
        }

        private void GetQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.Id == quest.Id))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    AddDisplayMessage(quest.ToDisplayMessage());
                }
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.Id == quest.Id && !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.Inventory.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        CurrentPlayer.Inventory.RemoveItems(quest.ItemsToComplete);

                        var messageLines = new List<string>();
                        CurrentPlayer.AddXP(quest.RewardExperiencePoints);
                        messageLines.Add($"You receive {quest.RewardExperiencePoints} experience points");

                        CurrentPlayer.ReceiveGold(quest.RewardGold);
                        messageLines.Add($"You receive {quest.RewardGold} gold");

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemId);

                            CurrentPlayer.Inventory.AddItem(rewardItem);
                            messageLines.Add($"You receive a {rewardItem.Name}");
                        }

                        AddDisplayMessage($"Quest Completed - {quest.Name}", messageLines);

                        // mark the quest as completed
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }


        private void GetMonsterAtCurrentLocation()
        {
            CurrentMonster = CurrentLocation.HasMonster() ? CurrentLocation.GetMonster() : null;
            if (CurrentMonster != null) AddDisplayMessage("Monster Encountered:", $"You see a {CurrentMonster.Name} here!");
        }

        private void AddDisplayMessage(string title, string message) =>
    AddDisplayMessage(title, new List<string> { message });

        private void AddDisplayMessage(string title, IList<string> messages)
        {
            AddDisplayMessage(new DisplayMessage(title, messages));
        }

        public void AddDisplayMessage(DisplayMessage message)
        {
            this.Messages.Insert(0, message);

            if (Messages.Count > _maximumMessagesCount)
            {
                Messages.Remove(Messages.Last());
            }
        }

        public void AttackCurrentMonster(GameItem? currentWeapon)
        {
            if (CurrentMonster == null) return;

            if (currentWeapon == null)
            {
                AddDisplayMessage("Combat Warning!", "You have no selected weapon.");
                return;
            }
            CurrentPlayer.CurrentWeapon = currentWeapon;
            var message = currentWeapon.PerformAction(CurrentPlayer, CurrentMonster);
            AddDisplayMessage(message);

            if (!CurrentMonster.IsAlive)
            {
                Death(CurrentMonster);
                GetMonsterAtCurrentLocation();
            }
            else
            {
                message = CurrentMonster.UseCurrentWeaponOn(CurrentPlayer);
                AddDisplayMessage(message);
            }
            if (!CurrentPlayer.IsAlive)
            {
                Death(CurrentPlayer);
            }
        }
        private void Death(Character character)
        {
            if (character == CurrentPlayer)
            {
                AddDisplayMessage($"{CurrentPlayer.Name} Defeated", $"The {CurrentMonster.Name} {CurrentMonster.KillMessage}");
                this.OnLocationChanged(_currentWorld.LocationAt(0, -1));
                AddDisplayMessage("You wake up in your home", $"Someone found you and carried you home. Your wounds have healed and you think to yourself, 'En sån här chans får man bara en gång i live´..' ");
                CurrentPlayer.FullHeal();
            }
            else if (character == CurrentMonster)
            {
                var messageLines = new List<string>();
                messageLines.Add($"You've defeated the {CurrentMonster.Name}!");

                CurrentPlayer.AddXP(CurrentMonster.RewardExperiencePoints);
                messageLines.Add($"You've recieved {CurrentMonster.RewardExperiencePoints}XP!");

                CurrentPlayer.ReceiveGold(CurrentMonster.Gold);
                messageLines.Add($"You found ${CurrentMonster.Gold}!");

                foreach (GameItem item in CurrentMonster.Inventory.Items)
                {
                    CurrentPlayer.Inventory.AddItem(item);
                    messageLines.Add($"You've recieved a {item.Name}");
                }
                AddDisplayMessage("Monster Defeated", messageLines);
            }
        }

        public void ConsumeCurrentItem(GameItem? item)
        {
            if (item is null || item.Category != ItemCategory.Consumable)
            {
                AddDisplayMessage("Item Warning!", "You must select a consumable to use.");
                return;
            }
            CurrentPlayer.CurrentConsumable = item;
            var message = CurrentPlayer.UseCurrentConsumable(CurrentPlayer);
            AddDisplayMessage(message);
        }
        public void CraftItemUsing(Recipe recipe)
        {
            _ = recipe ?? throw new ArgumentNullException(nameof(recipe));
            var messages = new List<string>();

            if (CurrentPlayer.Inventory.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.Inventory.RemoveItems(recipe.Ingredients);
                foreach (ItemQuantity itemQuantity in recipe.Output)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++) 
                    { 
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemId);
                        CurrentPlayer.Inventory.AddItem(outputItem);
                        messages.Add($"You craft one {outputItem.Name}");
                    }
                    AddDisplayMessage("Item Creation", messages);
                }
            }
            else
            {
                messages.Add("You do not have the required ingredients: ");
                foreach (ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    messages.Add($" {itemQuantity.Quantity} {ItemFactory.GetItemName(itemQuantity.ItemId)}");
                }
                AddDisplayMessage("Item Creation", messages);
            }
        }

        public void ProcessKeyPress(KeyProcessingEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            var key = args.Key.ToUpper();
            if (_userInputAction.ContainsKey(key)) _userInputAction[key].Invoke();
        }
    }
}
