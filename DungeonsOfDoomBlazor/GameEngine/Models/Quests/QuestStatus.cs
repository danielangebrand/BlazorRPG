namespace DungeonsOfDoomBlazor.GameEngine.Models.Quests
{
    public class QuestStatus
    {
        public Quest PlayerQuest { get; set; }
        public bool IsCompleted { get; set; } = false;
        public QuestStatus(Quest q)
        {
            PlayerQuest = q;
        }
    }
}
