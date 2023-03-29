namespace DungeonsOfDoomBlazor.GameEngine.Models.Items
{
    public class Weapon : GameItem
    {
        public string DamageRoll { get; private set; } = string.Empty;

        public Weapon(int id, string name, string description, int price, string damageRoll)
            : base(id, name, description, price, true)
        {
            DamageRoll = damageRoll;
        }
        public Weapon()
        {
            
        }

        public override GameItem Clone() => new Weapon(Id, Name, Description, Price, DamageRoll);
    }
}
