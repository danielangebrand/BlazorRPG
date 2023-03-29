namespace DungeonsOfDoomBlazor.GameEngine.Models.Items
{
    public class GameItem
    {
        public static readonly GameItem Empty = new GameItem();
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public int Price { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int? HealthBuff { get; set; }
        public string Buff { get; set; }
        public bool AffectsPlayerInstance { get; set; } = false;
        public bool Freeze { get; set; } = false;
        public bool IsUnique { get; set; }
        public bool FemaleOnly { get; set; } = false;
        public bool MaleOnly { get; set; } = false;

        public GameItem(int id, string name, string description, int price, bool isUnique = false)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            IsUnique = isUnique;
        }
        public GameItem()
        {
            
        }
        //public GameItem RandomItem()
        //{
        //    switch (RandomUtils.Dice())
        //    {
        //        case < 2:
        //            return new Sword();
        //        case < 4:
        //            return new ChickenOnYourHead();
        //        case < 6:
        //            return new GimpSuite();

        //        case < 7:
        //            return new JavaScript();

        //        case < 8:
        //            return new DaggerSwagger();

        //        case < 9:
        //            return new ChangeOfName();
        //        default: return null;
        //    }
        //}

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
        public virtual GameItem Clone() => new GameItem(Id, Name, Description, Price);
    }
}
