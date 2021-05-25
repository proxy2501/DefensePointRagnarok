using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    class SplashScreen : GameScreen
    {
        public SplashScreen()
        {
            backgroundPath = "Background/SpalshScreenBackground";
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();
        }

        public void HandleInput()
        {
            if(InputHandler.MouseLeftClick() || InputHandler.MouseRightClick())
            {
                ScreenManager.ChangeScreenTo(new TitleScreen());
            }
        }
    }
}
