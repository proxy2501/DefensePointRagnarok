using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    /// This singleton class creates Projectile objects for use in the game.
    /// It uses the Factory, Prototype, and Singleton design patterns.
    /// </summary>
    public class ProjectileFactory : Factory
    {
        #region Fields
        // The instance of this class.
        private static ProjectileFactory instance;

        // The template for standard projectiles.
        private Projectile standardProjectile;

        // The template SpriteRenderer for standard projectiles.
        private SpriteRenderer standardRenderer;
        #endregion

        #region Properties
        /// <summary>
        /// Singleton pattern property of this class' instance.
        /// </summary>
        public static ProjectileFactory Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ProjectileFactory();
                }
                return instance;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Private constructor.
        /// </summary>
        private ProjectileFactory()
        {
            // Create prototype for a standard projectile.
            CreatePrototype(ref standardRenderer, ref standardProjectile, "Sprites/Temp/60px/Projectile", 500, 25);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a new Projectile game object.
        /// </summary>
        /// <param name="type">A string specifying the type of projectile to create.</param>
        /// <returns>A GameObject of the specified projectile type</returns>
        public override GameObject Create(string type)
        {
            GameObject go = new GameObject();

            switch (type)
            {
                case "Standard":
                    Projectile projectileClone = standardProjectile.Clone();
                    go.AddComponent(new Collider());
                    go.AddComponent(projectileClone);
                    go.AddComponent(standardRenderer.Clone());
                    break;
            }

            return go;
        }

        /// <summary>
        /// Creates a new Projectile game object at the specified position.
        /// </summary>
        /// <param name="type">A string specifying the type of projectile to create.</param>
        /// <param name="position">A Vector2 specifying where to create the projecile.</param>
        /// <param name="velocity">A Vector2 specifying the velocity of the projecile.</param>
        /// <returns>A GameObject of the specified projectile type</returns>
        public GameObject Create(string type, Vector2 position, Vector2 velocity)
        {
            GameObject go = new GameObject();

            switch (type)
            {
                case "Standard":
                    Projectile projectileClone = standardProjectile.Clone();
                    projectileClone.Velocity = velocity;
                    go.AddComponent(new Collider());
                    go.AddComponent(projectileClone);
                    go.AddComponent(standardRenderer.Clone());
                    go.Transform.Position = position;
                    break;
            }

            return go;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates a prototype by instantiating its template fields.
        /// </summary>
        /// <param name="sr">The SpriteRenderer template to be instantiated.</param>
        /// <param name="projectile">The Projectile template to be instantiated.</param>
        /// <param name="spritePath">The path to the sprite texture.</param>
        /// <param name="speed">The speed of the projectile.</param>
        private void CreatePrototype(ref SpriteRenderer sr, ref Projectile projectile, string spritePath, float speed, int damage)
        {
            projectile = new Projectile(speed, damage);
            sr = new SpriteRenderer(spritePath);
        }
        #endregion
    }
}
