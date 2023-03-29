using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using System.Xml.Linq;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Characters
{
    public class Monster : Character
    {
        private string _name;
        public string Image { get; set; }
        public int RewardExperiencePoints { get; set; }
        public static int MonsterCounter { get; set; }
        public override int Health
        {
            get => base.Health;
            set
            {
                base.Health = value;
                if (base.Health <= 0)
                {
                    MonsterCounter--;
                }
            }
        }
        public string KillMessage { get; set; }
        public override string Name
        {
            get => IsAlive ? _name : $"The corpse of a {_name}";

            set { _name = value; }
        }
        //public Monster(string name, int health, int damage) : base()
        //{
        //    MonsterCounter++;
        //    Name = name;
        //    Health = health;
        //    Damage = damage;
        //}
        public Monster() : base()
        {
            MonsterCounter++;
        }

        //public static Monster Create()
        //{
        //    if (RandomUtils.Percentage())
        //    {
        //        var m = new StonedGolem();
        //        //var item = GetItem();

        //        //if (item != null) m.Backpack.Add(item);

        //        return m;
        //    }
        //    else
        //    {
        //        var m = new ShriekingGeek();
        //        //var item = GetItem();

        //        //if (item != null) m.Backpack.Add(item);
        //        return m;
        //    }

        //}

        //private static Item GetItem() => Item.RandomItem();

    }
}
