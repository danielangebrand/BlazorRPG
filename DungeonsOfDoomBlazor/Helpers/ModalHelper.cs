using Blazorise;

namespace DungeonsOfDoomBlazor.Helpers
{
    public class ModalHelper
    {
        public Modal? ModalRef { get; set; }
        public void ShowModal() => ModalRef?.Show();
        public void HideModal() => ModalRef?.Hide();
    }
}
