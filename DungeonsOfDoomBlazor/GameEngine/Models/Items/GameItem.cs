using DungeonsOfDoomBlazor.GameEngine.Actions;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using System.Reflection.Metadata.Ecma335;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Items
{
    public class GameItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; }
        public int Price { get; }
        public bool IsUnique { get; set; }
        public IAction? Action { get; private set; }
        public ItemCategory Category { get; }

        public GameItem(int id, ItemCategory category, string name, string description, int price, bool isUnique = false, IAction? action = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            IsUnique = isUnique;
            Category = category;
            Action = action;
            
        }
        internal DisplayMessage PerformAction(Character actor, Character target)
        {
            if (Action is null)
            {
                throw new InvalidOperationException("CurrentWeapon.Action cannot be null");
            }
            else return Action.Execute(actor, target);
        }

        internal void SetAction(IAction? action)
        {
            this.Action = action;
        }
        public virtual GameItem Clone() => new GameItem(Id, Category, Name, Description, Price, IsUnique, Action);

    }
}
