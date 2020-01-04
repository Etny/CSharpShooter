using ShootSandbox.Properties;
using ShootSandbox.Weapons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShootSandbox.Entities
{
    class EntityPlayerShip : EntityShip
    {
        private InputManager input;
        public float Acceleration { get; protected set; }


        public EntityPlayerShip(World World, InputManager input) : base(World, "Player", 2, Resources.Player)
        {
            this.input = input;

            this.IsFriendly = true;

            WeaponRapid Weapon = new WeaponRapid(this, new EntityGenericBullet(this, Resources.Bullet1, 1, 24, .05f))
            {
                ProjectileDeviation = 15,
                ProjectileForce = 14,
                MaxCoolDown = 6
            };

            this.Weapon = Weapon;

            Acceleration = .6f;

        }
    
        public override void Tick()
        {
            Rotation = Util.AngleBetween(input.MouseLocation, Util.Vector2ToPoint(Location));

            float moveX = 0, moveY = 0;

            if (input.IsPressed(Keys.W))
                moveY -= Acceleration;

            if (input.IsPressed(Keys.S))
                moveY += Acceleration;

            if (input.IsPressed(Keys.A))
                moveX -= Acceleration;

            if (input.IsPressed(Keys.D))
                moveX += Acceleration;

            if (moveX != 0 || moveY != 0)
                Velocity = Vector2.Clamp(Vector2.Add(Velocity, new Vector2(moveX, moveY)), MinVel, MaxVel);

            if(input.MousePressed) Weapon.Fire();

            base.Tick();
        }


    }
}
