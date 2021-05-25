using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Runner
{
    //Author: Magnus E. Poulsen
    public class PlaceTowerButton : Button
    {
        public PlaceTowerButton(Vector2 position) : base(position)
        {
            mouseOffTexturePath = "Buttons/Respawn1";
            mouseOverTexturePath = "Buttons/Respawn2";
            downTexturePath = "Buttons/Respawn3";
        }

        public PlaceTowerButton(Vector2 position, string text) : base(position, text)
        {
            mouseOffTexturePath = "Buttons/Respawn1";
            mouseOverTexturePath = "Buttons/Respawn2";
            downTexturePath = "Buttons/Respawn3";
            fontPath = "Buttons/Font";
            fontColor = Color.White;
        }

        //    private MouseState previousMS = Mouse.GetState();
        //    private MouseState currentMS;

        //    private string text;
        //    private SpriteFont font;
        //    private string fontPath = "SpriteFont";
        //    private Vector2 textDimension;
        //    private Color textColor = Color.White;

        //    private Texture2D texture, inactiveTexture, activeTexture, downTexture;
        //    private string inactivePath = "Buttons/Respawn1";
        //    private string activePath = "Buttons/Respawn2";
        //    private string downPath = "Buttons/Respawn3";
        //    private Rectangle collisionBox;
        //    private Vector2 origin;
        //    private bool isDown;

        //    public Vector2 Position { get; set; }
        //    public bool Activated { get; set; }

        //    public PlaceTowerButton(string text)
        //    {
        //        Position = Vector2.Zero;
        //        this.text = text;
        //        isDown = false;
        //        Activated = false;
        //    }

        //    public void LoadContent()
        //    {
        //        activeTexture = ScreenManager.contentmanager.Load<Texture2D>(activePath);
        //        inactiveTexture = ScreenManager.contentmanager.Load<Texture2D>(inactivePath);
        //        downTexture = ScreenManager.contentmanager.Load<Texture2D>(downPath);
        //        font = ScreenManager.contentmanager.Load<SpriteFont>(fontPath);
        //        textDimension = font.MeasureString(text);
        //        texture = inactiveTexture;
        //        origin = new Vector2(texture.Width / 2, texture.Height / 2);
        //        collisionBox = new Rectangle((int)(Position.X - origin.X), (int)(Position.Y - origin.Y), texture.Width, texture.Height);
        //    }

        //    public void Update()
        //    {
        //        HandleInput();
        //    }

        //    public void Draw(SpriteBatch spriteBatch)
        //    {
        //        spriteBatch.Draw(texture, Position, null, Color.White, 0, origin, 1, SpriteEffects.None, 1f);
        //        spriteBatch.DrawString(font, text, Position - (textDimension / 2), textColor);
        //    }

        //    public void HandleInput()
        //    {
        //        currentMS = Mouse.GetState();
        //        Rectangle mouseRectangle = new Rectangle(currentMS.X, currentMS.Y, 1, 1);

        //        if(mouseRectangle.Intersects(collisionBox))
        //        {
        //            if (currentMS.LeftButton == ButtonState.Pressed && previousMS.LeftButton == ButtonState.Released && isDown)
        //            {
        //                isDown = true;
        //            }

        //            else if (currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed && isDown)
        //            {
        //                isDown = false;
        //                Activated = true;
        //            }

        //            if (!isDown && texture != activeTexture)
        //            {
        //                texture = activeTexture;
        //            }

        //            else if (isDown && texture != downTexture)
        //            {
        //                texture = inactiveTexture;
        //            }
        //        }

        //        else
        //        {
        //            texture = inactiveTexture;

        //            if (currentMS.LeftButton == ButtonState.Released && previousMS.LeftButton == ButtonState.Pressed && isDown)
        //            {
        //                isDown = false;
        //            }
        //        }

        //        previousMS = currentMS;
        //    }

        //    public void DrawCollision(SpriteBatch spriteBatch)
        //    {
        //        Rectangle top = new Rectangle((int)Position.X - (int)origin.X, (int)Position.Y - (int)origin.Y, collisionBox.Width, 1);
        //        Rectangle bottom = new Rectangle((int)Position.X - (int)origin.X, (int)Position.Y - (int)origin.Y + collisionBox.Height, collisionBox.Width, 1);
        //        Rectangle right = new Rectangle((int)Position.X - (int)origin.X + collisionBox.Width, (int)Position.Y - (int)origin.Y, 1, collisionBox.Height);
        //        Rectangle left = new Rectangle((int)Position.X - (int)origin.X, (int)Position.Y - (int)origin.Y, 1, collisionBox.Height);
        //    }

        //    public void DrawMousePosision(SpriteBatch spriteBatch)
        //    {
        //        Rectangle top = new Rectangle((int)currentMS.X, (int)currentMS.Y, 1, 1);
        //    }
    }
}
