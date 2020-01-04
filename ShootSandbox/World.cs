using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using ShootSandbox.Entities;
using System.Threading;

namespace ShootSandbox
{
    class World
    {
        public LinkedList<Entity> Entities = new LinkedList<Entity>();
        private List<Entity> EntitiesRemove = new List<Entity>();
        private List<Entity> EntitiesAdd = new List<Entity>();

        Bitmap DrawBuffer;
        Graphics g = null;
        InputManager inputManager;

        private int nextId = 0;
        
        public World()
        {
            DrawBuffer = new Bitmap(GameWindow.Width, GameWindow.Height);

            g = Graphics.FromImage(DrawBuffer);
            inputManager = new InputManager();

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;


            Entities.AddLast(new EntityPlayerShip(this, inputManager));
            Entities.AddLast(new EntityEnemyShip(this));
        }

        public void killEntity(Entity e)
        {
            EntitiesRemove.Add(e);
           // Entities.Remove(e);
        }

        public Entity SpawnEntity(Entity e)
        {
            EntitiesAdd.Add(e);
           // Entities.AddLast(e);
            return e;
        }

        public Entity GetColliding(Entity entity)
        {
            foreach (Entity e in Entities)
            {
                if (e.ID == entity.ID) continue;
                if (!entity.Hitbox.IntersectsWith(e.Hitbox)) continue;


                return e;
            }

            return null;
        }

        public int getUniqueID()
        {
            return ++nextId;
        }

        public void Draw()
        {
            g.Clear(Color.White);

          //  Entity[] temp = new Entity[Entities.Count];
          //  Entities.CopyTo(temp);

            foreach (Entity e in Entities)
                e.Draw(g);

            GameWindow.Commit(DrawBuffer);
        }

        public void Tick()
        {
            inputManager.Tick();

            foreach (Entity e in EntitiesAdd) 
                Entities.AddLast(e);
            

            foreach (Entity e in EntitiesRemove)
                Entities.Remove(e);

            EntitiesAdd.Clear();
            EntitiesRemove.Clear();
          

            foreach (Entity e in Entities)
                e.Tick();
        }

        public void Update(Object stateInfo)
        {
            Tick();
            Draw();
        }
    }
}
