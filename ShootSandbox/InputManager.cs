using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShootSandbox
{
    class InputManager
    {
        public Point MouseLocation { get; protected set; }
        public bool MousePressed { get; protected set; }

        private List<Keys> pressed = new List<Keys>();

        public InputManager()
        {
            GameWindow.mouseMove += MouseMove;
            GameWindow.keyDown += KeyDown;
            GameWindow.keyUp += KeyUp;
            GameWindow.mouseDown += MouseDown;
            GameWindow.mouseUp += MouseUp;
        }

        private void MouseMove(MouseEventArgs args)
        {
            this.MouseLocation = args.Location;
        }

        private void MouseDown(MouseEventArgs args)
        {
            if (args.Button != MouseButtons.Left) return;

            MousePressed = true;
        }

        private void MouseUp(MouseEventArgs args)
        {
            if (args.Button != MouseButtons.Left) return;

            MousePressed = false;
        }

        private void KeyDown(KeyEventArgs args)
        {
            if (!pressed.Contains(args.KeyCode))
                pressed.Add(args.KeyCode);
        }

        private void KeyUp(KeyEventArgs args)
        {
            if (pressed.Contains(args.KeyCode))
                pressed.Remove(args.KeyCode);
        }

        public Boolean IsPressed(Keys k)
        {
            return pressed.Contains(k);
        }

        public void Tick()
        {
          
        }



    }
}
