using ShootSandbox.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootSandbox.Weapons
{
    abstract class Weapon
    {
        public EntityShip Wielder { get; protected set; }

        public Weapon(EntityShip Wielder)
        {
            this.Wielder = Wielder;
        }


        public abstract Entity Fire();

        public virtual void Tick() { }
    }
}
