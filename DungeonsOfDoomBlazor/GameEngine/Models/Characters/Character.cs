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
        public Character
            (int id, string name, int dex, int str, int ac,
            int currentHP, int maxHP, int gold, string deathMessage)
        {
            Id = id;
            Name = name;
            Dexterity = dex;
            Strength = str;
            ArmorClass = ac;
            Health = maxHP;
            MaxHealth = maxHP;
            Gold = gold;
            DeathMessage = deathMessage;

        }
        public int Id { get; }
        public virtual string Name { get; } = string.Empty;
        public virtual int Health { get; private set; }
        public virtual int MaxHealth { get; protected set; }
        public int Dexterity { get; set; } = 10;
        public int Strength { get; set; } = 10;
        public int ArmorClass { get; set; } = 10;
        public int Gold { get; private set; }
        public int Level { get; protected set; } = 1;
        public string DeathMessage { get; } = string.Empty;
        public Inventory Inventory { get; } = new Inventory();
        public GameItem? CurrentWeapon { get; set; }
        public bool HasCurrentWeapon => CurrentWeapon != null;
        public bool IsAlive => Health > 0;
        public void TakeDamage(int damage) => Health = (damage > 0) ? Health -= damage : Health;

        //Här fuckar det ur.
        public void Heal(int heal) => Health = (heal > 0) ? (Health + heal > MaxHealth ? MaxHealth : Health += heal) : Health;
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
