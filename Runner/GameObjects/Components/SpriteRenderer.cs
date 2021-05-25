using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{/// <summary>
/// Author: Daniel Lund Justesen
/// 
/// Component for drawing out other gameobjects if component is used
/// </summary>
    public class SpriteRenderer: Component 
    {
        private Texture2D sprite;
        private Vector2 origin;


        public Texture2D Sprite { get => sprite; set => sprite = value; }
        public Vector2 Origin { get => origin; set => origin = value; }
        public float Scale { get; set; } = 1f;

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// The root path of the sprite.
        /// </summary>
        public string SpritePath { get; private set; }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// The layer depth at which the sprite is drawn.
        /// </summary>
        public float LayerDepth { get; set; }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Used to determine if layerDepth should be updated before drawing.
        /// If associated GameObject does not move on the Y-axis, layerDepth can remain the same.
        /// </summary>
        public bool CanMoveInGameWorld { get; set; } = true;

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Constructor that sets a path for sprite texture.
        /// </summary>
        /// <param name="spritePath">A string specifying the path to the sprite texture.</param>
        public SpriteRenderer(string spritePath)
        {
            SpritePath = spritePath;
        }

        public override string ToString()
        {
            return "SpriteRenderer";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            /// Contributor: Mikkel Emil Nielsen-Man
            // If sprites moves, set layer depth to match its position on the screen.
            if (CanMoveInGameWorld)
            {
                float spriteBasePosition = GameObject.Transform.Position.Y - Origin.Y + sprite.Height;
                LayerDepth = (spriteBasePosition / ScreenManager.ScreenDimensions.Y);
            }
            /// End constribution: Mikkel Emil Nielsen-Man

            spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, LayerDepth);
        }

        public void SetSprite(string spritePath)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spritePath);
            Origin = new Vector2(Sprite.Width/2, Sprite.Height/2);
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Creates a shallow clone of SpriteRenderer.
        /// </summary>
        /// <returns>A SpriteRenderer that is a clone of this SpriteRenderer.</returns>
        public SpriteRenderer Clone()
        {
            return (SpriteRenderer)this.MemberwiseClone();
        }

        public override void Awake()
        {
            SetSprite(SpritePath);
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Code that needs to run when GameObject starts but after it awakes.
        /// </summary>
        public override void Start()
        {
            float spriteBasePosition = GameObject.Transform.Position.Y - Origin.Y + sprite.Height;
            LayerDepth = (spriteBasePosition / ScreenManager.ScreenDimensions.Y);
        }
    }
}
