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
        public override string Name
        {
            get => IsAlive ? _name : $"the remains of a {_name}";

            set { _name = value; }
        }
        public Monster() : base()
        {
            MonsterCounter++;
        }
    }
}
