using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{/// <summary>
/// Author: Daniel Lund Justesen
/// 
/// Handles all actions regarding Command Pattern
/// </summary>
    class InputHandler
    {
        private KeyboardState newKS, oldKS;

        public static MouseState PreviousMS = Mouse.GetState();
        public static MouseState NewMS;

        private static Rectangle mouseCollisionBox;

        private static InputHandler instance;
        public static InputHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputHandler();
                }
                return instance;
            }

        }

        public Player Entity { get; set; }

        private Dictionary<Keys, Command> keybinds = new Dictionary<Keys, Command>();

        private InputHandler()
        {
            //Keybinds for Player
            keybinds.Add(Keys.D, new MoveCommand(new Vector2(1,0)));
            keybinds.Add(Keys.A, new MoveCommand(new Vector2(-1,0)));
            keybinds.Add(Keys.W, new MoveCommand(new Vector2(0,-1)));
            keybinds.Add(Keys.S, new MoveCommand(new Vector2(0,1)));
            keybinds.Add(Keys.Space, new BuildCommand("Standard"));

        }

        /// <summary>
        /// Author: Daniel Lund Justesen
        /// Contribuor: Mikkel Emil Nielsen-Man
        /// 
        /// Checks for key presses and executes the commands in keybinds according to their type.
        /// </summary>
        public void Execute()
        {
            if (ScreenManager.CurrentScreen.GetType() != typeof(PlayScreen)) return;

            newKS = Keyboard.GetState();

            foreach (Keys item in keybinds.Keys)
            {
                switch (keybinds[item].ExecutionType)
                {
                    case "Continuous":
                        if (newKS.IsKeyDown(item))
                        {
                            keybinds[item].Execute(Entity);
                        }
                        break;

                    case "Single":
                        if (newKS.IsKeyDown(item) && !oldKS.IsKeyDown(item))
                        {
                            keybinds[item].Execute(Entity);
                        }
                        break;
                }
            }

            oldKS = newKS;
        }

        public static bool MouseLeftClick()
        {
            if (NewMS.LeftButton == ButtonState.Pressed && PreviousMS.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MouseLeftRelease()
        {
            if (NewMS.LeftButton == ButtonState.Released && PreviousMS.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MouseRightClick()
        {
            if (NewMS.RightButton == ButtonState.Pressed && PreviousMS.RightButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MouseRightRelease()
        {
            if (NewMS.RightButton == ButtonState.Released && PreviousMS.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool MouseIntersects(Rectangle other)
        {
            // Save current location of mouse as a 1x1 rectangle.
            mouseCollisionBox = new Rectangle(NewMS.X, NewMS.Y, 1, 1);

            // Check for intersection between mouse location and other rectangle.
            if (mouseCollisionBox.Intersects(other))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Vector2 MousePosition()
        {
            return NewMS.Position.ToVector2();
        }
    }
}
