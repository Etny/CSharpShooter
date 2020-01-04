using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShootSandbox.Entities
{
    abstract class Entity
    {
        public int ID { get; private set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public bool IsFriendly { get; protected set; }
        public Vector2 Location { get; protected set; }
        public Vector2 Velocity { get; protected set; }
        public bool IsSolid { get; protected set; }
        public float Friction { get; protected set; }
        public float Rotation { get; protected set; }
        public World World { get; private set; }

        public float MaxSpeed {
            get
            {
                return maxSpeed;
            }

            protected set
            {
                maxSpeed = value;

                MaxVel = new Vector2(MaxSpeed, MaxSpeed);
                MinVel = new Vector2(-MaxSpeed, -MaxSpeed);
            }
        }

        private float maxSpeed;
        protected Vector2 MinVel, MaxVel;

        public Rectangle Hitbox
        {
            get
            {
                return hitbox;
            }

            protected set
            {
                hitbox = value;
            }
        }

        private Rectangle hitbox;
        protected bool limitedByWorldBounds = true;

        private int MaxX = GameWindow.Width, MaxY = GameWindow.Height;

        public Entity(World World, int Width, int Height)
        {
            this.ID = World.getUniqueID();

            this.World = World;
            this.Width = Width;
            this.Height = Height;
            this.IsFriendly = false;

            this.Width = Width;
            this.Height = Height;

            Location = Vector2.Zero;
            Velocity = Vector2.Zero;

            Hitbox = new Rectangle((int)(Location.X - (Width / 2)), (int)(Location.Y - (Height / 2)), Width, Height);

            Friction = .03f;

            MaxSpeed = 8f;
        }

        public Entity(World World) : this(World, 0, 0) { }
        
        public abstract void Draw(Graphics g);

        public virtual void Push(Vector2 force)
        {
            Velocity = Vector2.Clamp(Vector2.Add(Velocity, force), MinVel, MaxVel);
        }

        public virtual bool IsEnemy(Entity e)
        {
            return !(e.IsFriendly == IsFriendly);
        }

        public virtual void Kill()
        {
            World.killEntity(this);
        }

        /*Collision is currently very basic as the hitboxes of the entities don't
         * rotate with them. I'll update this later to use to some fancy polygon
         * collision system using SAT (https://en.wikipedia.org/wiki/Hyperplane_separation_theorem),
         * but this will do for now*/
        public virtual void CollideWith(Entity e) { }

        protected virtual bool CheckCollision(Vector2 NewLocation)
        {   
            return true;
        }

        public virtual void Tick()
        {
            var NewLocation = Vector2.Add(Location, Velocity);

            hitbox.X = (int)(NewLocation.X - (Width / 2));
            hitbox.Y = (int)(NewLocation.Y - (Height / 2));

            if(CheckCollision(NewLocation))
                Location = NewLocation;

            if (limitedByWorldBounds)
            {
                if (Location.X < 0 || Location.X > MaxX)
                {
                    Location = Vector2.Clamp(Location, new Vector2(0, Location.Y), new Vector2(MaxX, Location.Y));
                    Velocity = new Vector2(0, Velocity.Y);
                }

                if (Location.Y < 0 || Location.Y > MaxY)
                {
                    Location = Vector2.Clamp(Location, new Vector2(Location.X, 0), new Vector2(Location.X, MaxY));
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }


            float velX = Math.Abs(Velocity.X);
            float velY = Math.Abs(Velocity.Y);
            float velXDelta = 0;
            float velYDelta = 0;

            if (velX > 0)
               velXDelta = (velX > Friction ? Friction : velX) * (Velocity.X < 0 ? 1 : -1);

            if (velY > 0)
                velYDelta = (velY > Friction ? Friction : velY) * (Velocity.Y < 0 ? 1 : -1);

            Velocity = new Vector2(Velocity.X + velXDelta, Velocity.Y + velYDelta);

            hitbox.X = (int)(Location.X - (Width / 2));
            hitbox.Y = (int)(Location.Y - (Height / 2));
        }

    }
}
