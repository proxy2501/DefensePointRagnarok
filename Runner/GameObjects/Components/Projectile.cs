using Microsoft.Xna.Framework;
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
    /// This component defines the attributes of projectiles in the game.
    /// </summary>
    public class Projectile : Component, IGameListener
    {
        #region Fields
        // A reference to the SpriteRenderer attached to this game object.
        private SpriteRenderer spriteRenderer;
        #endregion

        #region Properties
        /// <summary>
        /// The damage of the projectile.
        /// </summary>
        public int Damage { get; private set; }

        /// <summary>
        /// The speed of the projectile.
        /// </summary>
        public float Speed { get; private set; }

        /// <summary>
        /// The velocity (direction) of the projectile.
        /// </summary>
        public Vector2 Velocity { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that sets speed an velocity.
        /// </summary>
        /// <param name="speed">A float determining the projectile's speed.</param>
        /// <param name="damage">An integer determining the projectile's damage.</param>
        public Projectile(float speed, int damage)
        {
            this.Speed = speed;
            this.Damage = damage;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Update method. Runs code that should execute every frame.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Move();
            OutOfBounds();
        }

        /// <summary>
        /// Creates a shallow clone of Projectile.
        /// </summary>
        /// <returns>A Projectile component that is a shallow clone.</returns>
        public Projectile Clone()
        {
            return (Projectile)this.MemberwiseClone();
        }

        /// <summary>
        /// Returns the name of this component.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Projectile";
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Defines how this IGameListener handles events.
        /// </summary>
        /// <param name="gameEvent">The notifying GameEvent.</param>
        /// <param name="component">A Component that is involved in the event (if applicable).</param>
        /// <param name="node">A Node that triggered the event (if applicable).</param>
        public void OnNotify(GameEvent gameEvent, Component component = null, Node node = null)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy")
            {
                Enemy enemy = (Enemy)component.GameObject.GetComponent("Enemy");
                enemy.TakeDamage(Damage);
                GameObject.Destroy();
            }
            else if(gameEvent.Title == "Collision" && component.GameObject.Tag == "Boss")
            {
                Boss bossComponent = (Boss)component.GameObject.GetComponent("Boss");
                bossComponent.TakeDamage(Damage);
                GameObject.Destroy();
            }
        }

        /// <summary>
        /// Code that needs to run when GameObject awakes.
        /// </summary>
        public override void Awake()
        {
            GameObject.Tag = "Projectile";
            Collider collider = (Collider)GameObject.GetComponent("Collider");
            collider.CollisionEvent.Attach(this);
            collider.CheckCollisionEvents = true;
        }

        /// <summary>
        /// Code that needs to run when GameObject starts but after it awakes.
        /// </summary>
        public override void Start()
        {
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Moves the projectile according to its speed and velocity.
        /// </summary>
        private void Move()
        {
            GameObject.Transform.Translate(Velocity * Speed * GameWorld.Instance.DeltaTime);
        }

        /// <summary>
        /// Deletes the projectile if it goes off screen.
        /// </summary>
        private void OutOfBounds()
        {
            if (GameObject.Transform.Position.X < 0 - spriteRenderer.Origin.X ||
                GameObject.Transform.Position.X > ScreenManager.ScreenDimensions.X + spriteRenderer.Origin.X ||
                GameObject.Transform.Position.Y < 0 - spriteRenderer.Origin.Y ||
                GameObject.Transform.Position.Y > ScreenManager.ScreenDimensions.Y + spriteRenderer.Origin.Y)
            {
                Destroy();
            }
        }
        #endregion
    }
}
