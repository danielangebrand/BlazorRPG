using DungeonsOfDoomBlazor.GameEngine.ViewModels;

namespace DungeonsOfDoomBlazor.GameEngine.Models.World
{
    public class World
    {
        IGameSession ViewModel;
        private readonly IList<Location> locations;
        public string Name { get; set; }


        public World(IEnumerable<Location>? locs = null)
        {
            this.locations = locs is null ? new List<Location>() : locs.ToList();
        }
        internal void AddLocation(Location location) => locations.Add(location);
        public Location LocationAt(int x, int y)
        {
            var loc = locations.FirstOrDefault(r => r.X == x && r.Y == y);
            return loc ?? throw new ArgumentOutOfRangeException("Coordinates", "Provided coordinates doesn't exist in this game.");
        }

        public bool HasLocationAt(int x, int y) => locations.Any(l => l.X == x && l.Y == y);
        public Location GetHomeLocation(World world) => world.Name == "Village" ? this.LocationAt(0, -1) : this.LocationAt(0, 0);
        public IList<Location> GetLocations() => this.locations;
    }
}
