using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShootSandbox
{
    public partial class GameWindow : Form
    {
        public static int Width = 1280, Height = 720;

        public static Bitmap buffer;
        private static Graphics bufferGraphics;

        public delegate void MouseEvent(MouseEventArgs args);
        public delegate void KeyEvent(KeyEventArgs args);
        public delegate void DrawEvent();


        public static event DrawEvent onDraw;
        public static event MouseEvent mouseMove;
        public static event MouseEvent mouseDown;
        public static event MouseEvent mouseUp;
        public static event KeyEvent keyDown;
        public static event KeyEvent keyUp;

        Stopwatch timer;
        float target = 8;
        Thread drawThread;


        public GameWindow(Thread t)
        {
            InitializeComponent();

            drawThread = t;

            buffer = new Bitmap(Width, Height);
            bufferGraphics = Graphics.FromImage(buffer);

            this.DoubleBuffered = true;
            this.MouseMove += onMouseMove;
            this.KeyDown += onKeypress;
            this.KeyUp += onKeyUp;
            this.MouseDown += onMouseDown;
            this.MouseUp += onMouseUp;

            timer = new Stopwatch();
        }

        public void DrawLoop()
        {
            while (true)
            {
                timer.Restart();

                //onDraw();
                Invalidate();

                timer.Stop();

                int sleepTime = (int)(target - timer.ElapsedMilliseconds);

                try
                {
                    Thread.Sleep(sleepTime);
                }catch(Exception e)
                {
                    Console.WriteLine("Thread.Sleep(sleepTime) failed with sleepTime being {0}. Error: {1}", sleepTime, e.Message);
                }
            }
        }

        public static void Commit(Bitmap b)
        {
            bufferGraphics.DrawImage(b, 0, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(buffer, Point.Empty);
            base.OnPaint(e);
        }

        private void onMouseMove(Object sender, MouseEventArgs args)
        {
            mouseMove(args);
        }

        private void onMouseDown(Object sender, MouseEventArgs args)
        {
            mouseDown(args);
        }

        private void onMouseUp(Object sender, MouseEventArgs args)
        {
            mouseUp(args);
        }

        private void onKeypress(Object sender, KeyEventArgs args)
        {
            keyDown(args);
        }

        private void onKeyUp(Object sender, KeyEventArgs args)
        {
            keyUp(args);
        }
    }

}
