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

        /*
         * METODER ATT TILLÄMPA SÅSMÅNINGOM:
        public string ChangeName()
        {
            Console.WriteLine("Press any key to continue.. ");
            Console.ReadKey(true);
            return "Weener Sausage";
        }
        public void FreezeGame()
        {
            Console.WriteLine(Description);
            for (int j = 60; j > -1; j--)
            {
                Thread.Sleep(1000);
                Console.WriteLine(j);
            }
            Console.WriteLine("Press any key to continue.. ");
            Console.ReadKey(true);
        }
        */
        //public static readonly GameItem Empty = new GameItem();
        //public bool AffectsPlayerInstance { get; set; } = false;
        //public bool Freeze { get; set; } = false;
        //public bool FemaleOnly { get; set; } = false;
        //public bool MaleOnly { get; set; } = false;
    }
}
