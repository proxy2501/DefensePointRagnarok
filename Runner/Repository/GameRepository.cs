using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This repository provides functionality to save and load GameObjects in the game.
    /// </summary>
    public class GameRepository : IGameRepository
    {
        #region Fields
        private readonly IDatabaseProvider provider;
        private readonly IGameMapper mapper;
        private IDbConnection connection;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="provider">An IDatabaseProvider to provide a database connection.</param>
        /// <param name="mapper">An IGameMapper to map reader data.</param>
        public GameRepository(IDatabaseProvider provider, IGameMapper mapper)
        {
            this.provider = provider;
            this.mapper = mapper;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Opens a connection to the database. Call this before using services that this class provides.
        /// </summary>
        public void Open()
        {
            if (connection == null)
            {
                connection = provider.CreateConnection();
            }

            connection.Open();

            CreateDatabaseTables();
        }

        /// <summary>
        /// Closes the connection to the database. Call this after using services that this class provides.
        /// </summary>
        public void Close()
        {
            connection.Close();
        }

        /// <summary>
        /// Clears all data stored in the repository.
        /// When saving game data, call this method first to
        /// </summary>
        public void ClearData()
        {
            string query = @"
                DELETE FROM GameObject;
                DELETE FROM Component;
                VACUUM;
            ";

            SQLiteCommand cmd = new SQLiteCommand(query, (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds a list of GameObjects to the repository. 
        /// </summary>
        /// <param name="gameObjects">A List of GameObjects to be added to the repository.</param>
        public void AddGameObjects(List<GameObject> gameObjects)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                // Add gameobject to GameObject table.
                string query = $@"
                    INSERT INTO GameObject (position_x, position_y)
                    VALUES
                        ({gameObjects[i].Transform.Position.X}, {gameObjects[i].Transform.Position.Y})
                    ;
                ";

                SQLiteCommand cmd = new SQLiteCommand(query, (SQLiteConnection)connection);
                cmd.ExecuteNonQuery();

                // Add components to Component table with an id reference to the game object.
                List<Component> components = gameObjects[i].GetAllComponents();

                foreach (Component component in components)
                {
                    AddComponent(component, i+1);
                }
            }
        }

        /// <summary>
        /// Gets a list of all GameObjects in the repository.
        /// </summary>
        /// <returns>A List of GameObjects.</returns>
        public List<GameObject> GetGameObjects()
        {
            string query = $@"
                SELECT
                    *
                FROM
                    GameObject
                LEFT JOIN Component ON GameObject.id = Component.gameobject_id
                ;
            ";
            SQLiteCommand cmd = new SQLiteCommand(query, (SQLiteConnection)connection);
            SQLiteDataReader reader = cmd.ExecuteReader();

            List<GameObject> result = mapper.MapGameObjectsFromReader(reader);

            return result;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Creates all tables in the database if they don't already exist.
        /// </summary>
        private void CreateDatabaseTables()
        {
            // Create GameObject table
            string query = @"
                CREATE TABLE IF NOT EXISTS GameObject(
                    id INTEGER PRIMARY KEY,
                    position_x FLOAT,
                    position_y FLOAT
                );
            ";

            SQLiteCommand cmd = new SQLiteCommand(query, (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();

            // Create Component table
            query = @"
                CREATE TABLE IF NOT EXISTS Component(
                    gameobject_id INTEGER,
                    type string,
                    sprite_number INTEGER,
                    path string,
                    velocity_x FLOAT,
                    velocity_y FLOAT,
                    health INTEGER,
                    speed FLOAT,
                    damage INTEGER,
                    range FLOAT,
                    shot_interval FLOAT,
                    projectile_type string,
                    FOREIGN KEY(gameobject_id) REFERENCES GameObject(id)
                );
            ";

            cmd = new SQLiteCommand(query, (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds a Component to the repository with a specified gameobject_id.
        /// </summary>
        /// <param name="component">A Component to add to the repository.</param>
        /// <param name="gameobject_id">An integer denoting the gameobject_id to assign to the component.</param>
        private void AddComponent(Component component, int gameobject_id)
        {
            // Instantiate default values.
            string sprite_number = "NULL";
            string path = "NULL";
            string velocity_x = "NULL";
            string velocity_y = "NULL";
            string health = "NULL";
            string speed = "NULL";
            string damage = "NULL";
            string range = "NULL";
            string shot_interval = "NULL";
            string projectile_type = "NULL";

            // Save values according to component type.
            string type = component.ToString();

            switch (type)
            {
                case "Animator":
                    sprite_number = (component as Animator).TextureSpriteAmount.ToString();
                    path = "'" + (component as Animator).TextureSpriteName + "'";
                    break;

                case "Enemy":
                    velocity_x = (component as Enemy).Velocity.X.ToString();
                    velocity_y = (component as Enemy).Velocity.Y.ToString();
                    health = (component as Enemy).Health.ToString();
                    break;

                case "Projectile":
                    velocity_x = (component as Projectile).Velocity.X.ToString();
                    velocity_y= (component as Projectile).Velocity.Y.ToString();
                    speed = (component as Projectile).Speed.ToString();
                    damage = (component as Projectile).Damage.ToString();
                    break;

                case "SpriteRenderer":
                    path = "'" + (component as SpriteRenderer).SpritePath + "'";
                    break;

                case "Tower":
                    range = (component as Tower).Range.ToString();
                    shot_interval = (component as Tower).ShotInterval.ToString();
                    projectile_type = "'" + (component as Tower).ProjectileType + "'";
                    break;

                case "TownHall":
                    health = (component as TownHall).Health.ToString();
                    break;
            }

            // Enter component into Component table.
            string query = $@"
                INSERT INTO Component
                    VALUES
                        (
                            {gameobject_id},
                            '{type}',
                            {sprite_number},
                            {path},
                            {velocity_x},
                            {velocity_y},
                            {health},
                            {speed},
                            {damage},
                            {range},
                            {shot_interval},
                            {projectile_type}
                        )
                ;
            ";

            SQLiteCommand cmd = new SQLiteCommand(query, (SQLiteConnection)connection);
            cmd.ExecuteNonQuery();
        }
        #endregion
    }
}
