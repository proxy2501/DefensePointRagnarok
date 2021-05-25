using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Runner
{
    public static class ScreenManager
    {
        public static ContentManager contentmanager { get; private set; }
        public static Vector2 ScreenDimensions { get; private set; } = new Vector2(1920, 1080);
        public static GameScreen CurrentScreen { get; set; }
        public static bool EndAndQuit { get; private set; } = false;


        public static void Initialize()
        {
            CurrentScreen = new SplashScreen();
        }

        public static void LoadContent(ContentManager contentManager)
        {
            contentmanager = new ContentManager(contentManager.ServiceProvider, "Content");
            CurrentScreen.LoadContent();
        }

        public static void UnloadContent()
        {
            CurrentScreen.UnloadContent();
            contentmanager.Unload();
        }

        public static void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentScreen.Draw(spriteBatch);
        }

        public static void ChangeScreenTo(GameScreen gameScreen)
        {
            CurrentScreen.UnloadContent();
            CurrentScreen = gameScreen;
            CurrentScreen.LoadContent();

#if DEBUG
            if (gameScreen.GetType() != typeof(PlayScreen))
            {
                PathManager.Instance.Grid.IsVisible = false;
                PathManager.Instance.Grid.ClickToBlockNodes = false;
            }
#endif
        }

        public static void EndAndExitGame()
        {
            EndAndQuit = true;
        }
    }
}
