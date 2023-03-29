namespace DungeonsOfDoomBlazor.GameEngine.Models
{
    public class MonsterEncounter
    {
        public int MonsterId { get; set; }
        public int ChanceOfEncountering { get; set; }
        public MonsterEncounter(int monsterId, int chanceOfEncounter)
        {
            MonsterId = monsterId;
            ChanceOfEncountering = chanceOfEncounter;
        }
    }
}
