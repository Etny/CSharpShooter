using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ShootSandbox
{
    static class Program
    {
        public static GameWindow Window;
        public static Random Random;

        private static World world;

        private static System.Threading.Timer timer;

        private static Thread DrawThread;

        static void Main()
        {
            Window = new GameWindow(DrawThread);
            Random = new Random();
            world = new World();


           // GameWindow.onDraw += world.Draw;

            timer = new System.Threading.Timer(world.Update);
            timer.Change(10, 14);

            DrawThread = new Thread(new ThreadStart(Window.DrawLoop));
            DrawThread.IsBackground = true;
            DrawThread.Start();

            Application.Run(Window);  
        }


    }

   
}
