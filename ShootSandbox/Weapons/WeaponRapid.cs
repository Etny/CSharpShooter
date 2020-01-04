using ShootSandbox.Entities;
using ShootSandbox.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootSandbox.Weapons
{
    class WeaponRapid : Weapon
    {

        public EntityProjectile Projectile { get; protected set; }

        public int Cooldown { get; set; }
        public int MaxCoolDown { get; set; }

        public float ProjectileDeviation { get; set; }
        public float ProjectileForce { get; set; }
        public float PushbackForce { get; set; }
        public float PushbackForceDeviation { get; set; }
        public float PushbackDirectionDeviation { get; set; }

        public WeaponRapid(EntityShip Wielder, EntityProjectile projectile) : base(Wielder)
        {
            this.MaxCoolDown = 20;
            this.Projectile = projectile;
        }

        public override Entity Fire()
        {
            if (Cooldown > 0) 
                return null;

            float angle = Wielder.Rotation + Util.RandomFloat(ProjectileDeviation);

            Entity bullet = Wielder.World.SpawnEntity(Projectile.Clone(angle, ProjectileForce));
            Wielder.Push(Util.InvertVector(Util.DirectedVector(PushbackForce + Util.RandomFloat(PushbackForceDeviation), angle + Util.RandomFloat(PushbackDirectionDeviation))));

            Cooldown = MaxCoolDown;

            return bullet;
        }

        public override void Tick()
        {
            if (Cooldown > 0)
                Cooldown--;
        }
    }
}
