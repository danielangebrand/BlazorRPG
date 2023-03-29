namespace DungeonsOfDoomBlazor.GameEngine.Models.World
{
    public class World
    {
        private readonly IList<Location> locations;

        public World(IEnumerable<Location> locs)
        {
            this.locations = locs is null ? new List<Location>() : locs.ToList();
        }

        public Location LocationAt(int x, int y)
        {
            var loc = locations.FirstOrDefault(r => r.X == x && r.Y == y);
            return loc ?? throw new ArgumentOutOfRangeException("Coordinates", "Provided coordinates doesn't exist in this game.");
        }

        public bool HasLocationAt(int x, int y) => locations.Any(l => l.X == x && l.Y == y);
        public Location GetHomeLocation() => this.LocationAt(0, -1);
    }
}
