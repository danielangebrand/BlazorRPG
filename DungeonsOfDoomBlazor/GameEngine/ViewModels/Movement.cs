using DungeonsOfDoomBlazor.GameEngine.Models.World;
using DungeonsOfDoomBlazor.GameEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Blazorise;
using System.Reflection.Metadata.Ecma335;
using Microsoft.JSInterop;

namespace DungeonsOfDoomBlazor.GameEngine.ViewModels
{
    public class Movement
    {
        IGameSession session;
        private World world;
        public Location CurrentLocation { get; private set; }
        public EventCallback<Location> LocationChanged { get; set; }
        public Movement(World world)
        {
            this.world = world ?? throw new ArgumentNullException(nameof(world));
            this.CurrentLocation = world.GetHomeLocation(world);
        }

        public bool CanMoveNorth =>
            this.world.HasLocationAt(this.CurrentLocation.X, this.CurrentLocation.Y + 1);

        public bool CanMoveEast =>
            this.world.HasLocationAt(this.CurrentLocation.X + 1, this.CurrentLocation.Y);

        public bool CanMoveSouth =>
            this.world.HasLocationAt(this.CurrentLocation.X, this.CurrentLocation.Y - 1);

        public bool CanMoveWest =>
            this.world.HasLocationAt(this.CurrentLocation.X - 1, this.CurrentLocation.Y);

        public void MoveNorth() =>
            this.MoveBase(this.CurrentLocation.X, this.CurrentLocation.Y + 1);

        public void MoveEast() =>
            this.MoveBase(this.CurrentLocation.X + 1, this.CurrentLocation.Y);

        public void MoveSouth() =>
            this.MoveBase(this.CurrentLocation.X, this.CurrentLocation.Y - 1);

        public void MoveWest() =>
            this.MoveBase(this.CurrentLocation.X - 1, this.CurrentLocation.Y);

        private void MoveBase(int x, int y)
        {
            if (this.world.HasLocationAt(x, y))
            {
                this.CurrentLocation = this.world.LocationAt(x, y);
                this.LocationChanged.InvokeAsync(this.CurrentLocation);
            }
        }
        public void UpdateLocation(Location newLocation) => this.CurrentLocation = newLocation;
        public void EnterNewWorld(World w) =>
            LocationChanged.InvokeAsync(world.GetHomeLocation(w));
        

        //public async Task HandleKeyDown(KeyboardEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case "ArrowUp":
        //            if (CanMoveNorth) MoveNorth();
        //            break;
        //        case "ArrowDown":
        //            if (CanMoveSouth) MoveSouth();
        //            break;
        //        case "ArrowLeft":
        //            if (CanMoveWest) MoveWest();
        //            break;
        //        case "ArrowRight":
        //            if (CanMoveEast) MoveEast();
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
