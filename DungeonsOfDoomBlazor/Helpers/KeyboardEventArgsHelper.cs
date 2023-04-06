using DungeonsOfDoomBlazor.GameEngine.ViewModels;
using Microsoft.AspNetCore.Components.Web;

namespace DungeonsOfDoomBlazor.Helpers
{
    public static class KeyboardEventArgsHelper
    {
        public static KeyProcessingEventArgs ToKeyProcessingEventArgs(this KeyboardEventArgs args)
        {
            _ = args ?? throw new ArgumentNullException(nameof(args));

            return new KeyProcessingEventArgs
            {
                AltKey = args.AltKey,
                Code = args.Code,
                CtrlKey = args.CtrlKey,
                Key = args.Key,
                Location = args.Location,
                MetaKey = args.MetaKey,
                Repeat = args.Repeat,
                ShiftKey = args.ShiftKey
            };
        }
    }
}
