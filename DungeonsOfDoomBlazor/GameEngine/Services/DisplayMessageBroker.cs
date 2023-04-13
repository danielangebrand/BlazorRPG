using DungeonsOfDoomBlazor.GameEngine.Models;

namespace DungeonsOfDoomBlazor.GameEngine.Services
{
    public class DisplayMessageBroker
    {

        //Denna är singleton
        public event EventHandler<DisplayMessage>? OnMessageRaised;
        public static DisplayMessageBroker Instance => displayMessageBroker;


        private static readonly DisplayMessageBroker displayMessageBroker = new DisplayMessageBroker();

        private DisplayMessageBroker()
        {

        }

        public void RaiseMessage(DisplayMessage message)
        {
            if (OnMessageRaised != null) OnMessageRaised.Invoke(this, message);
        }
    }
}
