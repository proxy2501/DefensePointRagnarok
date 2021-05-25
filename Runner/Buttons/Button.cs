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
    public enum GameButtonState { Up, Down, Activated }
    public abstract class Button
    {
        public Vector2 Position { get; set; }
        protected Vector2 origin;
        protected Rectangle collisionBox;
        protected string buttonText;
        protected SpriteFont font;
        protected string fontPath;
        private Vector2 textDimensions;
        protected Color fontColor;

        protected Texture2D currentTexture;

        protected Texture2D mouseOffTexure, mouseOverTexture, downTexure;
        protected string mouseOffTexturePath, mouseOverTexturePath, downTexturePath;

        public GameButtonState State { get; private set; }

        public Button(Vector2 position)
        {
            Position = position;
            State = GameButtonState.Up;
            buttonText = null;
        }

        public Button(Vector2 position, string text)
        {
            Position = position;
            State = GameButtonState.Up;
            buttonText = text;
        }

        public virtual void LoadContent(ContentManager content)
        {
            // Load and set button textures & parameters.
            mouseOffTexure = content.Load<Texture2D>(mouseOffTexturePath);
            mouseOverTexture = content.Load<Texture2D>(mouseOverTexturePath);
            downTexure = content.Load<Texture2D>(downTexturePath);
            currentTexture = mouseOffTexure;
            origin = new Vector2(currentTexture.Width / 2, currentTexture.Height / 2);
            collisionBox = new Rectangle((int)(Position.X - origin.X), (int)(Position.Y - origin.Y), currentTexture.Width, currentTexture.Height);

            // Load and set text font.
            if (buttonText != null)
            {
                font = content.Load<SpriteFont>(fontPath);
                textDimensions = font.MeasureString(buttonText);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            HandleInput();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentTexture, Position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0.99f);

            if (buttonText != null)
            {
                spriteBatch.DrawString(font, buttonText, Position - (textDimensions / 2), fontColor, 0, Vector2.Zero, 1, SpriteEffects.None, 1f);
            }
        }

        private void HandleInput()
        {
            // If state is set to 'activated', set it to 'up' in preparation for a fresh check.
            if (State == GameButtonState.Activated)
            {
                State = GameButtonState.Up;
            }

            if (InputHandler.MouseIntersects(collisionBox)) // Case: The mouse intersects with the button.
            {
                // Set button state according to input.
                if (InputHandler.MouseLeftClick() && State != GameButtonState.Down)
                {
                    State = GameButtonState.Down;
                }
                else if (InputHandler.MouseLeftRelease() && State == GameButtonState.Down)
                {
                    State = GameButtonState.Activated;
                }

                // Set texture according to button state.
                if (State != GameButtonState.Down && currentTexture != mouseOverTexture)
                {
                    currentTexture = mouseOverTexture;
                }
                else if (State == GameButtonState.Down && currentTexture != downTexure)
                {
                    currentTexture = downTexure;
                }
            }
            else // Case: The mouse does not intersect with the button.
            {
                // Set button state according to input.
                if (InputHandler.MouseLeftRelease() && State == GameButtonState.Down)
                {
                    State = GameButtonState.Up;
                }

                // Set texture according to state.
                if (currentTexture != mouseOffTexure)
                {
                    currentTexture = mouseOffTexure;
                }
            }
        }


    }
}
