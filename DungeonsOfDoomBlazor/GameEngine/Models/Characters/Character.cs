using DungeonsOfDoomBlazor.GameEngine.Models.Items;
using MathNet.Numerics.Optimization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DungeonsOfDoomBlazor.GameEngine.Models.Characters
{
    public abstract class Character
    {
        private int _health;
        //private string _name;

        public Inventory Inventory { get; } = new Inventory();
        public virtual int MaxHealth { get; set; }
        public int Gold { get; set; }
        public int Level { get; set; }
        public GameItem? CurrentWeapon { get; set; }
        public bool HasCurrentWeapon => CurrentWeapon != null;
        public virtual string Name { get; set; } = string.Empty;

        public virtual int Health
        {
            get => _health;
            set { _health = value; }
        }
        public int Damage { get; set; }
        public bool IsAlive => Health > 0;
        public string DamageRoll { get; set; } = string.Empty;
        public void TakeDamage(int damage) => Health = (damage > 0) ? Health -= damage : Health;
        public void Heal(int heal) => Health = (heal > 0) ? (Health += heal > MaxHealth ? MaxHealth : Health += heal) : Health;
        public void FullHeal() => Health = MaxHealth;
        public void ReceiveGold(int gold) => Gold = (gold > 0) ? Gold += gold : Gold;
        public void SpendGold(int gold) => Gold = (gold > 0 && gold <= Gold) ? Gold -= gold : throw new ArgumentOutOfRangeException(nameof(gold), $"{Name} only has {Gold} gold, and cannot spend {gold} gold");
        public GameItem? CurrentConsumable { get; set; }
        public bool HasCurrentConsumable => CurrentConsumable != null;
        public DisplayMessage UseCurrentConsumable(Character character)
        {
            if (CurrentConsumable == null) throw new InvalidOperationException("CurrentConsumable cannot be null.");

            Inventory.RemoveItem(CurrentConsumable);
            return CurrentConsumable.PerformAction(this, character);
        }
        public DisplayMessage UseCurrentWeaponOn(Character character)
        {
            if (CurrentWeapon != null) return CurrentWeapon.PerformAction(this, character);
            else throw new InvalidOperationException("Current weapon cannot be null.");
        }

    }
}
