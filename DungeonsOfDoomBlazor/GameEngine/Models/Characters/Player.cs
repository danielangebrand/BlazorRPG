using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Characters
{
    public class Player : Character
    {

        public int X { get; set; }
        public int Y { get; set; }
        public string CharacterClass { get; set; } = string.Empty;
        public Gender G { get; set; }
        public int ExperiencePoints { get; set; } = 0;
        public IList<QuestStatus> Quests { get; set; } = new List<QuestStatus>();

        //public Player(string name, int health, int damage, int x, int y, int gender) : base()
        //{
        //    Health = health;
        //    Damage = damage;
        //    Name = name;
        //    X = x;
        //    Y = y;
        //    this.G = SetGender(gender);
        //}
        private Gender SetGender(int gender)
        {
            Gender g = (Gender)gender;
            return g;
        }
    }
}
