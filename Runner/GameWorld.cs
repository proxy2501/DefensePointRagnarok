using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Runner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private static GameWorld instance;

        private GameMapper mapper = new GameMapper();
        private SQLiteDatabaseProvider provider = new SQLiteDatabaseProvider("Data Source=gameobjects.db;Version=3;New=True");
        private GameRepository repo;

        public static GameWorld Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }
        /// <summary>
        /// List holding all game objects that currently exist in the game.
        /// </summary>
        public List<GameObject> GameObjects { get; private set; } = new List<GameObject>();

        /// <summary>
        /// List of active colliders in the game.
        /// </summary>
        public List<Collider> Colliders { get; set; } = new List<Collider>();

        /// <summary>
        /// Time passed since last frame.
        /// </summary>
        public float DeltaTime { get; private set; }

        /// <summary>
        /// Font for displaying on-sceen system messges.
        /// </summary>
        public SpriteFont SysMsgFont { get; private set; }

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Initialize repository.
            repo = new GameRepository(provider, mapper);

            // Screen changes
            graphics.PreferredBackBufferWidth = (int)ScreenManager.ScreenDimensions.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.ScreenDimensions.Y;
            graphics.IsFullScreen = true;
            graphics.HardwareModeSwitch = false;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            ScreenManager.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Load system messages font.
            SysMsgFont = Content.Load<SpriteFont>("SpriteFont");

            // Load manager classes content.
            ScreenManager.LoadContent(Content);
            PathManager.Instance.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            ScreenManager.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputHandler.Instance.Execute();
            InputHandler.NewMS = Mouse.GetState();

            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update(gameTime);
            }

            Collider[] tmpColliders = Colliders.ToArray();

            for (int i = 0; i < tmpColliders.Length; i++)
            {
                for (int j = 0; j < tmpColliders.Length; j++)
                {
                    tmpColliders[i].OnCollision(tmpColliders[j]);
                }
            }

            if (ScreenManager.EndAndQuit)
            {
                Exit();
            }

            // Update manager classes.
            ScreenManager.Update(gameTime);
            PathManager.Instance.Update(gameTime);

            InputHandler.PreviousMS = InputHandler.NewMS;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.FrontToBack); // 0.0f = furthest back, 1.0f = furthest in front.

            // Draw game objects.
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Draw(spriteBatch);
            }

            // Draw manager classes content.
            ScreenManager.Draw(spriteBatch);
            PathManager.Instance.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);

            
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Adds a game object to the list of active game objects.
        /// </summary>
        /// <param name="gameObject">A GameObject to be added.</param>
        public void AddGameObject(GameObject gameObject)
        {
            gameObject.Awake();
            gameObject.Start();
            GameObjects.Add(gameObject);
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Removes a game object from the list of active game objects.
        /// </summary>
        /// <param name="gameObject">A GameObject to be removed.</param>
        public void RemoveGameObject(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Searches the GameObjects list for an item with the input tag and returns the first match.
        /// </summary>
        /// <param name="tag">A string specifying the tag to look for</param>
        /// <returns>A GameObject with a tag that matches the search query.</returns>
        public GameObject FindGameObjectWithTag(string tag)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                if (gameObject.Tag == tag)
                {
                    return gameObject;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Gets all game objects stored in reporitory and enters them into the GameObjects list.
        /// </summary>
        public void LoadObjectsFromRepo()
        {
            // Destroy all current GameObjects in the list.
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Destroy();
            }

            // Get GameObjects from repo.
            repo.Open();
            GameObjects = repo.GetGameObjects();
            repo.Close();

            // Initialize GameObjects.
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Awake();
                GameObjects[i].Start();
            }
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Stores all 
        /// </summary>
        public void SaveObjectsToRepo()
        {
            repo.Open();
            repo.ClearData();
            repo.AddGameObjects(GameObjects);
            repo.Close();
        }
    }
}
