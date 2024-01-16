using DungeonsOfDoomBlazor.GameEngine.Factories;
using DungeonsOfDoomBlazor.GameEngine.Models;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;
using DungeonsOfDoomBlazor.GameEngine.Models.World;
using DungeonsOfDoomBlazor.GameEngine.Services;

namespace DungeonsOfDoomBlazor.GameEngine.ViewModels
{
    internal class GameSession : IGameSession
    {
        readonly int _maximumMessagesCount = 100;
        readonly IDiceService _diceService = DiceService.Instance;
        readonly Battle _battle;
        readonly Dictionary<string, Action> _userInputAction = new Dictionary<string, Action>();
        readonly IList<World> _worldList;
        public Player CurrentPlayer { get; private set; }
        public bool PlayerCreated { get; private set; }
        public World CurrentWorld { get; set; }


        public Models.World.Location CurrentLocation { get; private set; }

        public Movement Movement { get; private set; }
        public Monster? CurrentMonster { get; private set; }
        public bool HasMonster => CurrentMonster != null;
        public IList<DisplayMessage> Messages { get; } = new List<DisplayMessage>();

        public Merchant? CurrentMerchant { get; set; }

        public bool PlayedCreated = false;

        public GameSession()
        {
            InitializeUserInputActions();
            _battle = new Battle(
                () => OnLocationChanged(CurrentWorld.GetHomeLocation(CurrentWorld)),
                () => GetMonsterAtCurrentLocation(),
                _diceService);
            CurrentPlayer = new Player("Mojo Man", "Nerd", _diceService.Roll("6d3").Value, _diceService.Roll("6d3").Value, 10, 30, 0,
            "You wake up, bruised and with your mojo hurt from the fight. Yet, you think to yourself: 'En sån här chans får man ba' en gång i live''");
            _worldList = WorldFactory.CreateWorld();
            CurrentWorld = _worldList[0];

            Movement = new Movement(CurrentWorld);

            CurrentLocation = this.Movement.CurrentLocation;
        }
        public Inventory GetStartingEquipment()
        {
            if (!CurrentPlayer.Inventory.Weapons.Any()) CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(1001));

            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(2001));
            CurrentPlayer.LearnRecipe(RecipeFactory.GetRecipeById(1));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.Inventory.AddItem(ItemFactory.CreateGameItem(3003));
            return CurrentPlayer.Inventory;

        }
        public GameSession(int maxMessageCount, IDiceService? diceService = null) : this()
        {
            _maximumMessagesCount = maxMessageCount;
            _diceService = diceService ?? DiceService.Instance;
        }
        public void SetPlayer(Player player)
        {
            CurrentPlayer = player;
            PlayerCreated = true;
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

        public void OnLocationChanged(Models.World.Location newLocation)
        {
            _ = newLocation ?? throw new ArgumentNullException(nameof(newLocation));

            CurrentLocation = newLocation;
            Movement.UpdateLocation(CurrentLocation);
            GetMonsterAtCurrentLocation();
            CompleteQuestsAtLocation();
            if (!CurrentPlayer.CompletedQuest())
                GetQuestsAtLocation();

            CurrentMerchant = CurrentLocation.MerchantHere;
        }

        private void GetQuestsAtLocation()
        {
            if (!CurrentPlayer.CompletedQuest())
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
        }

        private void CompleteQuestsAtLocation()
        {
            if (CurrentLocation.QuestsAvailableHere.Any())
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
                            CurrentPlayer.QuestsCompleted++;
                            if (CurrentPlayer.CompletedQuest())
                                GetMainQuest();
                        }
                    }
                }
            }
        }
        public Quest GetMainQuest()
        {
            int questId = CurrentPlayer.QuestsCompleted + 2;
            Quest quest = QuestFactory.GetQuestById(questId);
            CurrentPlayer.Quests.Add(new QuestStatus(quest));
            AddDisplayMessage(quest.ToDisplayMessage());
            return quest;
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
            if (CurrentMonster != null)
            {
                CurrentPlayer.CurrentWeapon = currentWeapon;
                _battle.Attack(CurrentPlayer, CurrentMonster);
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
            //if (_userInputAction.ContainsKey(key)) _userInputAction[key].Invoke();
            if (_userInputAction.TryGetValue(key, out Action value)) value.Invoke();
        }
        public void EnterNewWorld() => Movement.EnterNewWorld(CurrentWorld == _worldList[0] ? _worldList[1] : _worldList[0]);
  
    }
}