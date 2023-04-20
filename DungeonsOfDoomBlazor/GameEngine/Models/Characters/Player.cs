using DungeonsOfDoomBlazor.GameEngine.Models.Enum;
using DungeonsOfDoomBlazor.GameEngine.Models.Quests;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Characters
{
    public class Player : Character
    {
        public static readonly Player Empty = new Player(string.Empty, string.Empty, 10, 10, 10, 10, 0, "");
        public IList<Recipe> Recipes { get; set; } = new List<Recipe>();
        public IList<QuestStatus> Quests { get; set; } = new List<QuestStatus>();
        //behöver ta bort XY..
        public int X { get; set; }
        public int Y { get; set; }
        public string CharacterClass { get; set; } = string.Empty;
        public Gender G { get; set; } = Gender.Undecided;
        public int ExperiencePoints { get; private set; }
        private Gender SetGender(int gender)
        {
            Gender g = (Gender)gender;
            return g;
        }

        public Player(string name, string characterClass, int dex, int str, int ac, int maxHP, int gold, string deathMessage)
            : base(1, name, dex, str, ac, maxHP,maxHP,gold, deathMessage)
        {
            CharacterClass = characterClass;
        }
        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.Id == recipe.Id))
            {
                Recipes.Add(recipe); 
            }
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
