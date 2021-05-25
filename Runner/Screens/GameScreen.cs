using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public abstract class GameScreen
    {
        protected ContentManager contentManager;
        protected string backgroundPath;
        protected Texture2D backgroundImage;

        public virtual void LoadContent()
        {
            contentManager = new ContentManager(ScreenManager.contentmanager.ServiceProvider, "Content");

            backgroundImage = contentManager.Load<Texture2D>(backgroundPath);
        }

        public virtual void UnloadContent()
        {
            contentManager.Unload();
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None,0f);
        }
    }
}
