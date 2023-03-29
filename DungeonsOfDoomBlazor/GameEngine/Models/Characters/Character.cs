using DungeonsOfDoomBlazor.GameEngine.Models.Items;
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
        public int MaxHealth { get; set; }
        public int Gold { get; set; }
        public int Level { get; set; }
        public virtual string Name { get; set; }

        public virtual int Health
        {
            get => _health;
            set { _health = value; }
        }
        public int Damage { get; set; }
        public bool IsAlive { get => Health > 0; }
        public string DamageRoll { get; set; } = string.Empty;
    }
}
