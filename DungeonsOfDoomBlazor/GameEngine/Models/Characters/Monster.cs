using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using System.Xml.Linq;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Characters
{
    public class Monster : Character
    {
        public string Image { get; } = string.Empty;
        public int RewardExperiencePoints { get; }
        public static int MonsterCounter { get; set; }

        public Monster(int id, string name, string imageName,
            int dex, int str, int ac, int maxHP, GameItem wpn, int rewardXP, int gold, string deathMessage)
            : base(id, name, dex, str, ac, maxHP, maxHP, gold, deathMessage)
        {
            MonsterCounter++;
            Image = imageName;
            CurrentWeapon = wpn;
            RewardExperiencePoints = rewardXP;
        }
    }
}
