using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsOfDoomBlazor
{
    public static class RandomUtils
    {
        public static bool Percentage() => BigDice() > 50 ? true : false;
        public static int Dice() => Random.Shared.Next(1, 10);
        public static int BigDice() => Random.Shared.Next(1, 100);
    }
}
