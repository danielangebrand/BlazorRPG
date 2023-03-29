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
        public Player CurrentPlayer { get; private set; }

        public Location CurrentLocation { get; private set; }
        public World CurrentWorld { get; private set; }

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
            CurrentWorld = WorldFactory.CreateWorld();
            Movement = new Movement(this.CurrentWorld);
            CurrentLocation = this.Movement.CurrentLocation;
            if (!CurrentPlayer.Inventory.Weapons.Any()) CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(1001));
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

                    var messageLines = new List<string>
                    {
                        quest.Description,
                        "Items to complete the quest:"
                    };

                    foreach (ItemQuantity q in quest.ItemsToComplete)
                    {
                        messageLines.Add($"{ItemFactory.CreateGameItem(q.ItemId).Name} (x{q.Quantity})");
                    }

                    messageLines.Add("Rewards for quest completion:");
                    messageLines.Add($" {quest.RewardExperiencePoints} XP");
                    messageLines.Add($" {quest.RewardGold} gold");

                    foreach (ItemQuantity quantity in quest.RewardItems)
                    {
                        messageLines.Add($"{quantity.Quantity} {ItemFactory.CreateGameItem(quantity.ItemId).Name} (x{quantity.Quantity})");
                    }

                    AddDisplayMessage($"Quest Added - {quest.Name}", messageLines);
                }
            }
        }
        //private void GetQuestsAtLocation()
        //{
        //    foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
        //    {

        //        if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.Id == quest.Id))
        //        {
        //            CurrentPlayer.Quests.Add(new QuestStatus(quest));

        //            var messageLines = new List<string>
        //            {
        //                quest.Description,
        //                "Items to complete the quest: "
        //            };

        //            foreach (ItemQuantity q in quest.ItemsToComplete)
        //            {
        //                messageLines.Add($"{ItemFactory.CreateGameItem(q.ItemId).Name} (x{q.Quantity})");
        //            }

        //            messageLines.Add("Rewards for quest completion:");
        //            messageLines.Add($" {quest.RewardExperiencePoints} XP");
        //            messageLines.Add($" {quest.RewardGold} gold");

        //            foreach (ItemQuantity quantity in quest.RewardItems)
        //            {
        //                messageLines.Add($"{quantity.Quantity} {ItemFactory.CreateGameItem(quantity.ItemId).Name} (x{quantity.Quantity})");
        //            }


        //            AddDisplayMessage($"Quest Added - {quest.Name}", messageLines);
        //        }
        //    }
        //}

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
                        // Remove the quest completion items from the player's inventory
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.Inventory.RemoveItem(
                                    CurrentPlayer.Inventory.Items.First(
                                        item => item.Id == itemQuantity.ItemId));
                            }
                        }

                        // give the player the quest rewards
                        var messageLines = new List<string>();
                        CurrentPlayer.ExperiencePoints += quest.RewardExperiencePoints;
                        messageLines.Add($"You receive {quest.RewardExperiencePoints} experience points");

                        CurrentPlayer.Gold += quest.RewardGold;
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

        public void AttackCurrentMonster(Weapon? currentWeapon)
        {
            if (CurrentMonster == null) return;

            if (currentWeapon == null) AddDisplayMessage("Warning!", "You have no selected weapon.");
            int dmgToMonster = _diceService.Roll(currentWeapon.DamageRoll).Value;

            if (dmgToMonster == 0) AddDisplayMessage($"{CurrentPlayer.Name} combat:", $"You missed the {CurrentMonster.Name}");
            else CurrentMonster.Health -= dmgToMonster;
            AddDisplayMessage($"{CurrentPlayer.Name} combat: ", $"You hit the {CurrentMonster.Name} for {dmgToMonster}. Remaining health: {CurrentMonster.Health}");

            if (!CurrentMonster.IsAlive)
            {
                var messageLines = new List<string>();
                messageLines.Add($"You've defeated the {CurrentMonster.Name}!");
                CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
                messageLines.Add($"You've recieved {CurrentMonster.RewardExperiencePoints}XP!");
                CurrentPlayer.Gold += CurrentMonster.Gold;
                messageLines.Add($"You found ${CurrentMonster.Gold}!");

                foreach (GameItem item in CurrentMonster.Inventory.Items)
                {
                    CurrentPlayer.Inventory.AddItem(item);
                    messageLines.Add($"You've recieved a {item.Name}");
                }
                AddDisplayMessage("Monster Defeated", messageLines);
                GetMonsterAtCurrentLocation();
            }
            else
            {
                int dmgToPlayer = _diceService.Roll(CurrentMonster.DamageRoll).Value;
                if (dmgToPlayer == 0) AddDisplayMessage($"{CurrentMonster.Name} Combat", $"{CurrentMonster.Name} attempts an attack, but stumbles and performs a moonwalk.");
                else CurrentPlayer.Health -= dmgToPlayer;
                AddDisplayMessage($"{CurrentMonster.Name} Combat", $"The {CurrentMonster.Name} hit you for {dmgToPlayer}. Remaining health: {CurrentPlayer.Health}");
            }
            if (CurrentPlayer.Health <= 0)
            {
                AddDisplayMessage($"{CurrentPlayer.Name} Defeated", $"The {CurrentMonster.Name} {CurrentMonster.KillMessage}");
                CurrentPlayer.Health = CurrentPlayer.MaxHealth; //
                this.OnLocationChanged(_currentWorld.LocationAt(0, -1));
            }
        }

        //public void AddXP()
        //{
        //    this.CurrentPlayer.ExperiencePoints += 10;
        //}
    }
}
