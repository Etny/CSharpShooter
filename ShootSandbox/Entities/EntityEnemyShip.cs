using ShootSandbox.Properties;
using ShootSandbox.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootSandbox.Entities
{
    class EntityEnemyShip : EntityShip
    {

        public EntityEnemyShip(World World) : base(World, "Enemy1", 3.2f, Resources.Enemy1)
        {
            Location = new System.Numerics.Vector2(300, 300);

            WeaponRapid Weapon = new WeaponRapid(this, new EntityGenericBullet(this, Resources.Bullet2, 2, 64, 3))
            {
                ProjectileForce = 8
            };

            this.Weapon = Weapon;
        }

        public override void Tick()
        {
           // Weapon.Fire();
            base.Tick();
        }

    }
}
