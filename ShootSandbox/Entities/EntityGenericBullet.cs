using ShootSandbox.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShootSandbox.Entities
{
    class EntityGenericBullet : EntityProjectile
    {

        public EntityGenericBullet(EntityShip Source, Image Sprite, float Scale, int Lifetime, float Angle, float Force, float PushForce) : base(Source, Sprite, Scale, Lifetime, Angle, Force, PushForce)
        {
          
        }

        public EntityGenericBullet(EntityShip Source, Image Sprite, float Scale, int Lifetime, float PushForce) : base(Source, Sprite, Scale, Lifetime, PushForce, true) {  }
        
        
        public override EntityProjectile Clone(float Angle, float Force)
        {
            return new EntityGenericBullet(Source, Sprite, Scale, Lifetime, Angle, Force, PushForce);
        }

        public override void CollideWith(Entity e)
        {
            if (!IsEnemy(e)) return;
            if (!(e is EntityShip)) return;
            if (!Hitbox.IntersectsWith(e.Hitbox)) return;

            EntityShip eShip = e as EntityShip;

            this.Kill();
            eShip.Push(Util.MultiplyVector(Velocity, PushForce));
            eShip.Damage(0);
        }


    }
}
