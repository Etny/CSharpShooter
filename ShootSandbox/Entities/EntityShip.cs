using ShootSandbox.Properties;
using ShootSandbox.Weapons;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShootSandbox.Entities
{
    class EntityShip : Entity
    {
        

        public Image Sprite { get; protected set; }
        public String Name { get; protected set; }
        public Weapon Weapon { get; protected set; }
        public int Health { get; protected set; }
        public float Weight { get; protected set; }

        public float FrictionStationary { get; protected set; }
        public float FrictionMoving { get; protected set; }

        protected float stationaryThreshold = 1f;
        protected float[] tintValues = { 1, 1, 1 };
        protected float tintRecovery = .2f;
        protected bool tint = false;

        public EntityShip(World World, String name, int width, int height, Image sprite, Vector2 startingLocation) : base(World, width, height)
        {
            Name = name;

            IsSolid = true;
            Weight = .5f;

            Weapon = null;
            Health = 100;

            Location = startingLocation;
            Location = new Vector2(GameWindow.Width / 2, GameWindow.Height / 2);

            Sprite = Util.ResizeImage(sprite, width, height);

            FrictionMoving = .08f;
            FrictionStationary = 3f;

            Friction = .08f;
        }

        public EntityShip(World World, String name, int width, int height, Image sprite) : this(World, name, width, height, sprite, new Vector2(GameWindow.Width / 2, GameWindow.Height / 2)) { }

        public EntityShip(World World, String name, float scale, Image sprite, Vector2 startLocation) : this(World, name, (int)(sprite.Width * scale), (int)(sprite.Height * scale), sprite, startLocation) { }

        public EntityShip(World World, String name, float scale, Image sprite) : this(World, name, (int)(sprite.Width * scale), (int) (sprite.Height * scale), sprite, new Vector2(GameWindow.Width / 2, GameWindow.Height / 2)) { }

        public virtual Vector2 GetWeaponLocation()
        {
            return new Vector2(Location.X + (float)((Width / 2) * Math.Cos(Util.DegreeToRadian(Rotation))), Location.Y + (float)((Width / 2) * Math.Sin(Util.DegreeToRadian(Rotation))));
        }

        public virtual void Damage(int dmg)
        {
            Health -= dmg;
            tintValues[0] = 3f;
        }

        protected override bool CheckCollision(Vector2 NewLocation)
        {
            var collide = World.GetColliding(this);

            if (collide != null)
            {
                CollideWith(collide);

                if (collide.IsSolid)
                    return false;
            }

            return true;
        }

        public override void CollideWith(Entity e)
        {
            base.CollideWith(e);

            if (!e.IsSolid) return;

            e.Push(Util.MultiplyVector(Velocity, Weight));
            Velocity = Util.MultiplyVector(Velocity, -.7f);
        }

        public override void Draw(Graphics g)
        {
            // g.FillRectangle(Brushes.Red, Hitbox);

            Bitmap drawImage = Util.RotateImage(Sprite, Rotation);

            if (tint)
                g.DrawImage(Util.TintImage(drawImage, tintValues[0], tintValues[1], tintValues[2]), Location.X - (drawImage.Width / 2), Location.Y - (drawImage.Height / 2));
            else
                g.DrawImage(drawImage, Location.X - (drawImage.Width / 2), Location.Y - (drawImage.Height / 2));          
        }

        public override void Tick()
        {
            if (!(Weapon == null)) Weapon.Tick();

            for (int i = 0; i < 2; i++)
            {
                if (tintValues[i] == 1) continue;

                tintValues[i] = Util.MoveTowards(tintValues[i], 1, tintRecovery);
                tint = true;
            }

            /*
            if (Velocity.Length() < stationaryThreshold)
                Friction = FrictionStationary;
            else
                Friction = FrictionMoving;
             */

            base.Tick();
        }
    }
}
