using DungeonsOfDoomBlazor.GameEngine.Models;
using DungeonsOfDoomBlazor.GameEngine.Models.Characters;
using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using DungeonsOfDoomBlazor.GameEngine.Models.World;

namespace DungeonsOfDoomBlazor.GameEngine.ViewModels
{
    public interface IGameSession
    {
        Player CurrentPlayer { get; }
        Location CurrentLocation { get; }
        Movement Movement { get; }
        Monster? CurrentMonster { get; }
        Merchant? CurrentMerchant { get; }
        IList<DisplayMessage> Messages { get; }
        bool HasMonster { get; }
        void OnLocationChanged(Location location);
        void ChangeGender();
        void AttackCurrentMonster(GameItem? currentWeapon);
        void ConsumeCurrentItem(GameItem? item);
        void CraftItemUsing(Recipe recipe);
        void ProcessKeyPress(KeyProcessingEventArgs args);
        void AddDisplayMessage(DisplayMessage message);
    }
}
