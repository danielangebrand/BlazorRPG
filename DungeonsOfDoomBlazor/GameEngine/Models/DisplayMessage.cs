namespace DungeonsOfDoomBlazor.GameEngine.Models
{
    public class DisplayMessage
    {
        public string Title { get; } = string.Empty;
        public IList<string> Messages { get; }
        public DisplayMessage(string title, string message)
        {
            Title = title;
            Messages = new List<string> { message };
        }
        public DisplayMessage(string title, IList<string> messages)
        {
            Title = title;
            Messages = messages;
        }
    }
}
