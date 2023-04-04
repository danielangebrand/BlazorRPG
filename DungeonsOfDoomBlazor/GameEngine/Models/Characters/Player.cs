using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Characters
{
    public class Player : Character
    {
        public IList<Recipe> Recipes { get; set; } = new List<Recipe>();
        public int X { get; set; }
        public int Y { get; set; }
        public string CharacterClass { get; set; } = string.Empty;
        public Gender G { get; set; }
        public int ExperiencePoints { get; set; } = 0;
        public override int MaxHealth { get; set; } = 30;
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
        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.Id == recipe.Id)) Recipes.Add(recipe);
        }
        public void AddXP(int xp)
        {
            if (xp > 0)
            {
                ExperiencePoints += xp;
                SetLevelAndMaxHP();
            }
        }

        private void SetLevelAndMaxHP()
        {
            int lvl = Level;
            Level = (ExperiencePoints / 100) + 1;
            if (Level != lvl) MaxHealth = Level * 10;
        }
    }
}
