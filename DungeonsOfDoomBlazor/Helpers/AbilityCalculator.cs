namespace DungeonsOfDoomBlazor.Helpers
{
    public static class AbilityCalculator
    {
        private const int defaultAbilityScore = 10;
        public static int CalculateBonus(int score) => (int)Math.Floor((score - defaultAbilityScore) / 2.0);
    }
}
