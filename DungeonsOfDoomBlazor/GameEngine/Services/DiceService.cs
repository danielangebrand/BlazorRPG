using D20Tek.DiceNotation;
using D20Tek.DiceNotation.DieRoller;

namespace DungeonsOfDoomBlazor.GameEngine.Services
{
    public class DiceService : IDiceService
    {
        private static readonly IDiceService _instance = new DiceService();
        public static IDiceService Instance => _instance;
        public DiceService()
        {
            
        }

        public IDice Dice { get; } = new Dice();
        public IDieRoller DieRoller { get; private set; } = new RandomDieRoller();
        public IDieRollTracker? RollTracker { get; private set; } = null;
        public IDiceConfiguration Configuration => Dice.Configuration;
        public void Configure(IDiceService.RollerType rollerType, bool enableTracker = false)
        {
            RollTracker = enableTracker ? new DieRollTracker() : null;

            DieRoller = rollerType switch
            {
                IDiceService.RollerType.Random => new RandomDieRoller(RollTracker),
                IDiceService.RollerType.Crypto => new CryptoDieRoller(RollTracker),
                IDiceService.RollerType.MathNet => new MathNetDieRoller(RollTracker),
                _ => throw new ArgumentOutOfRangeException(nameof(rollerType)),
            };
        }

        public DiceResult Roll() => Dice.Roll(DieRoller);
        public DiceResult Roll(string diceNotation) => Dice.Roll(diceNotation, DieRoller);

    }
}
