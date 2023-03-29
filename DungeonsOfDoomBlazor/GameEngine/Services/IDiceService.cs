using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;

namespace DungeonsOfDoomBlazor.GameEngine.Services
{
    public interface IDiceService
    {
        IDice Dice { get; }
        IDiceConfiguration Configuration { get; }
        IDieRollTracker? RollTracker { get; }
        enum RollerType
        {
            Random = 0,
            Crypto,
            MathNet
        }
        void Configure(RollerType rollType, bool enableTracker = false);
        DiceResult Roll();
        DiceResult Roll(string diceNotation);
    }
}
