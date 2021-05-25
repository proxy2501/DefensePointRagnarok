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
    /// This singleton class creates Tower objects in the game world.
    /// It uses the Factory and Singleton design pattern.
    /// </summary>
    public class TowerFactory : Factory
    {
        #region Fields
        // The instance of this class.
        private static TowerFactory instance;
        #endregion

        #region Properties
        /// <summary>
        /// Singleton pattern property of this class' instance.
        /// </summary>
        public static TowerFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TowerFactory();
                }

                return instance;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Private constructor.
        /// </summary>
        private TowerFactory()
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a new Tower game object at a defaut position.
        /// </summary>
        /// <param name="type">The type of tower to create.</param>
        /// <returns>A GameObject of the specified tower type.</returns>
        public override GameObject Create(string type)
        {
            GameObject go = new GameObject();
            go.Transform.Position = Vector2.Zero;

            switch (type)
            {
                case "Standard":
                    SpriteRenderer sr = new SpriteRenderer("Sprites/Temp/60px/Tower");
                    go.AddComponent(sr);
                    go.AddComponent(new Tower());
                    break;
            }

            return go;
        }

        /// <summary>
        /// Creates a new Tower game object at the specified position.
        /// </summary>
        /// <param name="type">A string specifying the type of tower to create.</param>
        /// <param name="position">A Vector2 specifying where to create the tower.</param>
        /// <returns>A GameObject of the specified tower type.</returns>
        public GameObject Create(string type, Vector2 position)
        {
            GameObject go = new GameObject();
            go.Transform.Position = position;

            switch (type)
            {
                case "Standard":
                    SpriteRenderer sr = new SpriteRenderer("Sprites/Temp/60px/Tower");
                    go.AddComponent(sr);
                    go.AddComponent(new Tower(500, 0.5f, "Standard"));
                    break;
            }

            return go;
        }
        #endregion
    }
}
