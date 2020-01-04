using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShootSandbox.Entities
{
    abstract class EntityProjectile : Entity
    {

        public Image Sprite { get; protected set; }
        public EntityShip Source { get; protected set; }
        public int Lifetime { get; protected set; }
        public float PushForce { get; protected set; }

        protected float Scale;

        public EntityProjectile(EntityShip Source, Image Sprite, float Scale, int Lifetime, float Angle, float Force, float PushForce) : base(Source.World, (int)(Sprite.Width * Scale), (int)(Sprite.Height * Scale))
        {
            this.Sprite = Util.ResizeImage(Sprite, (int)(Sprite.Width * Scale), (int)(Sprite.Height * Scale));
            this.Lifetime = Lifetime;
            this.Source = Source;
            this.Scale = Scale;
            this.PushForce = PushForce;

            this.IsSolid = false;
            this.IsFriendly = Source.IsFriendly;
            this.Friction = 0;
            this.Rotation = Source.Rotation;
            this.Velocity = Util.DirectedVector(Force, Angle);
            this.limitedByWorldBounds = false;
            this.Location = Source.GetWeaponLocation();
        }

        public EntityProjectile(EntityShip Source, Image Sprite, float Scale, int Lifetime, float PushForce, bool Clone) : base(Source.World, Sprite.Width, Sprite.Height)
        {
            this.Sprite = Sprite;
            this.Lifetime = Lifetime;
            this.Source = Source;
            this.Scale = Scale;
            this.PushForce = PushForce;
        }

        public abstract override void CollideWith(Entity e);

        public abstract EntityProjectile Clone(float Angle, float Force);

        protected override bool CheckCollision(Vector2 NewLocation)
        {
            var collide = World.GetColliding(this);

            if (collide != null && IsEnemy(collide))
            {
                CollideWith(collide);
            }

            return true;
        }

        public override void Draw(Graphics g)
        {
            Bitmap drawImage = Util.RotateImage(Sprite, Rotation);

            g.DrawImage(drawImage, Location.X - (drawImage.Width / 2), Location.Y - (drawImage.Height / 2));
        }


        public override void Tick()
        {
            base.Tick();

            if (--Lifetime <= 0)
                Kill();
        }
    }
}
