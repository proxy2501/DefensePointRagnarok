using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Data.SQLite;
using Runner;


namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class implements the IGameMapper interface.
    /// It provides a series of strategies for translating table information
    /// into useful classes that are recognizable in the game domain.
    /// </summary>
    public class GameMapper : IGameMapper
    {
        #region Public Methods
        /// <summary>
        /// Extracts data from a SQLite data reader and maps it to a list of GameObjects.
        /// </summary>
        /// <param name="reader">An SQLiteDataReader.</param>
        /// <returns>A list of GameObjects with the values and components extracted from the reader.</returns>
        public List<GameObject> MapGameObjectsFromReader(SQLiteDataReader reader)
        {
            // Instantiate default values for temp variables.
            List<GameObject> result = new List<GameObject>();
            GameObject gameObject = new GameObject();

            // Set initial ID to 0, so the first tuple will register as a new GameObject. (primary key is 1-indexed))
            int activeObjectID = 0;

            while (reader.Read())
            {
                // Get the game object ID of this tuple.
                int thisObjectID = reader.GetInt32(0);

                if (thisObjectID != activeObjectID) // If true, reader has reached a new GameObject.
                {
                    // Set active game object ID to this.
                    activeObjectID = thisObjectID;

                    // Create new GameObject and add to result.
                    gameObject = new GameObject();
                    float positionX = reader.GetFloat(1);
                    float positionY = reader.GetFloat(2);
                    gameObject.Transform.Position = new Vector2(positionX, positionY);
                    result.Add(gameObject);
                }

                // Add the component from this tuple to current GameObject.
                gameObject.AddComponent(MapComponentFromReader(reader));
            }

            return result;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Extracts a single tuple of data from a SQLite data reader and maps it to a Component according to its type.
        /// </summary>
        /// <param name="reader">An SQLiteDataReader.</param>
        /// <returns>A Component with the values extracted from the reader.</returns>
        private Component MapComponentFromReader(SQLiteDataReader reader)
        {
            // Instantiate temp variables.
            Component result = null;
            string type = null; // Column 4
            int spriteNumber; // Column 5
            string path; // Column 6
            float velocityX; // Column 7
            float velocityY; // Column 8
            int health; // Column 9
            float speed; // Column 10
            int damage; // Column 11
            float range; // Column 12
            float shot_interval; // Column 13
            string projectile_type; // Column 14

            // Get the type of component. If null, return.
            try
            {
                type = reader.GetString(4);
            }
            catch
            {
                return null;
            }

            switch (type)
            {
                case "Animator":
                    spriteNumber = reader.GetInt32(5);
                    path = reader.GetString(6);
                    result = new Animator(spriteNumber, path);
                    break;

                case "Collider":
                    result = new Collider();
                    break;

                case "Enemy":
                    velocityX = reader.GetFloat(7);
                    velocityY = reader.GetFloat(8);
                    health = reader.GetInt32(9);
                    result = new Enemy(new Vector2(velocityX, velocityY)) { Health = health };
                    break;

                case "Player":
                    result = new Player();
                    break;

                case "Projectile":
                    velocityX = reader.GetFloat(7);
                    velocityY = reader.GetFloat(8);
                    speed = reader.GetFloat(10);
                    damage = reader.GetInt32(11);
                    result = new Projectile(speed, damage) { Velocity = new Vector2(velocityX, velocityY) };
                    break;

                case "SpriteRenderer":
                    path = reader.GetString(6);
                    result = new SpriteRenderer(path);
                    break;

                case "Tower":
                    range = reader.GetFloat(12);
                    shot_interval = reader.GetFloat(13);
                    projectile_type = reader.GetString(14);
                    result = new Tower(range, shot_interval, projectile_type);
                    break;

                case "TownHall":
                    health = reader.GetInt32(9);
                    result = new TownHall(health);
                    break;
            }

            return result;
        }
        #endregion
    }
}
