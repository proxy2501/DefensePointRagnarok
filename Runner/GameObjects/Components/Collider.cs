using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This component is used by game objects that need to collide.
    /// </summary>
    public class Collider : Component
    {
        #region Fields
        // Size of the collsion box.
        private Vector2 size;

        // Origin of the collision box.
        private Vector2 origin;

        private SpriteRenderer spriteRenderer;

        // Texture used for drawing the collision box.
        private Texture2D texture;

        // Event for handling collisions.
        public GameEvent CollisionEvent { get; private set; } = new GameEvent("Collision");
        #endregion

        #region Properties
        /// <summary>
        /// Tells the game object whether to check for collisions or not.
        /// </summary>
        public bool CheckCollisionEvents { get; set; }

        /// <summary>
        /// Calculates and returns the game object's collision box.
        /// </summary>
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    (int)(GameObject.Transform.Position.X - origin.X),
                    (int)(GameObject.Transform.Position.Y - origin.Y),
                    (int)size.X,
                    (int)size.Y
                );
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for Colliders that do not need to be notified of collisions.
        /// </summary>
        public Collider()
        {
            CheckCollisionEvents = false;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Code that needs to run when GameObject awakes.
        /// </summary>
        public override void Awake()
        {
            GameWorld.Instance.Colliders.Add(this);
        }

        /// <summary>
        /// Code that needs to run when GameObject starts but after it awakes.
        /// </summary>
        public override void Start()
        {
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            this.origin = spriteRenderer.Origin;
            size = new Vector2(spriteRenderer.Sprite.Width * spriteRenderer.Scale, spriteRenderer.Sprite.Height * spriteRenderer.Scale);
            texture = GameWorld.Instance.Content.Load<Texture2D>("Sprites/CollisionBox");
        }

        /// <summary>
        /// Destroys this component.
        /// </summary>
        public override void Destroy()
        {
            GameWorld.Instance.Colliders.Remove(this);
        }

        /// <summary>
        /// Draws the collissionbox around GameObject in debug mode.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG
            // TODO: Ask a teacher: Why Vector2.Zero and not origin? Maddening.
            spriteBatch.Draw(texture, CollisionBox, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0.5f);
#endif
        }

        /// <summary>
        /// Dertermines what happens when this collider hits another collider.
        /// </summary>
        /// <param name="other">The other Collider that hits this Collider.</param>
        public void OnCollision(Collider other)
        {
            if (!CheckCollisionEvents) return;
            if (other == this) return;

            if (CollisionBox.Intersects(other.CollisionBox))
            {
                CollisionEvent.Notify(other); // Notify all subscribers to collisionEvent.
            }
        }

        /// <summary>
        /// Returns the name of this component.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Collider";
        }
        #endregion
    }
}
