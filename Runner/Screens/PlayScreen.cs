using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This subclass of GameScreen is the screen on which the game is played.
    /// </summary>
    public class PlayScreen : GameScreen
    {
        #region Fields
        // Timer for spacing out enemy spawns.
        private float enemySpawnTimer;

        // Determins the length of time between enemy spawns (seconds).
        private float enemySpawnInterval = 2f;

        private PlaceTowerButton saveButton, exitButton;
        private List<PlaceTowerButton> buttons;

        #endregion

        #region Properties
        /// <summary>
        /// Amount of gold available to the player.
        /// </summary>
        public static int Gold { get; set; }

        public static int tHealth { get; set; } // Addition by: Arturas Tatariskinas

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="newGame">True new game. False if loaded game.</param>
        public PlayScreen(bool newGame)
        {
            // Set screen background image path.
            backgroundPath = "Background/TempBackground";

            // Set starting gold to zero.
            Gold = 0;

            PathManager.Instance.ResetGrid();
#if DEBUG
            PathManager.Instance.Grid.IsVisible = true;
            PathManager.Instance.Grid.ClickToBlockNodes = true;
#endif

            saveButton = new PlaceTowerButton(Vector2.Zero, "Save Game");
            exitButton = new PlaceTowerButton(Vector2.Zero, "Exit Game");
            buttons = new List<PlaceTowerButton>();
            buttons.Add(saveButton);
            buttons.Add(exitButton);
            AlignButtons();

            if (newGame)
            {
                SetUpNewGame();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads initial content of the screen.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            foreach (PlaceTowerButton button in buttons)
            {
                button.LoadContent(contentManager);
            }
        }

        /// <summary>
        /// Unloads all screen content.
        /// </summary>
        public override void UnloadContent()
        {
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (tHealth <= 0)
            {
                // Destroy all current GameObjects in the list.
                for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
                {
                    GameWorld.Instance.GameObjects[i].Destroy();
                }

                GameWorld.Instance.GameObjects.Clear();

                ScreenManager.ChangeScreenTo(new GameOverScreen());
            }

            SpawnEnemies();

            foreach (PlaceTowerButton button in buttons)
            {
                button.Update(gameTime);
            }
            CheckButtons();
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            string displayGoldMsg = "Gold: " + Gold;
            Vector2 goldMsgDims = GameWorld.Instance.SysMsgFont.MeasureString(displayGoldMsg);
            spriteBatch.DrawString(GameWorld.Instance.SysMsgFont, displayGoldMsg, new Vector2(ScreenManager.ScreenDimensions.X / 2, 30), Color.Black, 0, goldMsgDims, 1, SpriteEffects.None, 1);
            
            // Addition by: Arturas Tatariskinas
            string displayHealthMsg = "Health: " + tHealth;
            Vector2 HealthMsgDims = GameWorld.Instance.SysMsgFont.MeasureString(displayHealthMsg);
            spriteBatch.DrawString(GameWorld.Instance.SysMsgFont, displayHealthMsg, new Vector2(ScreenManager.ScreenDimensions.X / 2, 50), Color.Black, 0, HealthMsgDims, 1, SpriteEffects.None, 1);
            // Addition end

            foreach (PlaceTowerButton button in buttons)
            {
                button.Draw(spriteBatch);
            }

        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Spawns enemies based on their spawn timer and the time since last spawn.
        /// </summary>
        private void SpawnEnemies()
        {
            if (enemySpawnTimer >= enemySpawnInterval)
            {
                // Instantite enemy and fixate it to a grid position.
                GameObject go = EnemyFactory.Instance.Create("Enemy");
                go.Transform.Position = PathManager.Instance.Grid.FixatePosition(go.Transform.Position);

                // Set enemy's pathing target.
                Enemy e = (Enemy)go.GetComponent("Enemy");

                // Add enemy object.
                GameWorld.Instance.AddGameObject(go);
                enemySpawnTimer = 0;
            }
            else
            {
                enemySpawnTimer += GameWorld.Instance.DeltaTime;
            }
        }

        /// <summary>
        /// Author: Daniel Lund Justesen
        /// 
        /// If conditions for boss spawn is true then boss will be spawned
        /// </summary>
        private void SpawnBoss()
        {
            GameObject go = new GameObject();
            SpriteRenderer sr = new SpriteRenderer("Sprites/Temp/Enemy");
            sr.Scale = 1.5f;
            go.AddComponent(sr);
            go.Transform.Position = new Vector2(1920, GameWorld.Instance.GraphicsDevice.Viewport.Height / 2);
            go.AddComponent(new Collider());
            go.AddComponent(new Boss(60, new Vector2(-1,0)));

            GameWorld.Instance.AddGameObject(go);
            //bossSpawned = true;
        }

        private void CheckButtons()
        {
            if(saveButton.State == GameButtonState.Activated)
            {
                GameWorld.Instance.SaveObjectsToRepo();
            }
            else if(exitButton.State == GameButtonState.Activated)
            {
                ScreenManager.EndAndExitGame();
            }
        }

        private void AlignButtons()
        {
            Vector2 firstButtonPosition = new Vector2(85, 40);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Position = firstButtonPosition + new Vector2(i * 160, 0);
            }
        }

        /// <summary>
        /// Sets up the PlayScreen for a new game.
        /// </summary>
        private void SetUpNewGame()
        {
            // Create TownHall.
            GameObject go = new GameObject();

            TownHall th = new TownHall(100);
            SpriteRenderer sr = new SpriteRenderer("Sprites/Temp/60px/TownHall");
            go.AddComponent(th);
            go.AddComponent(sr);
            go.AddComponent(new Collider()); // Addition by Mikkel
            GameWorld.Instance.AddGameObject(go);

            // Create Player.
            go = new GameObject();
            Player pr = new Player();
            sr = new SpriteRenderer("Sprites/Temp/60px/Player");
            go.AddComponent(pr);
            go.AddComponent(sr);
            go.AddComponent(new Collider()); // Addition by Mikkel
            go.Transform.Position = new Vector2((int)ScreenManager.ScreenDimensions.X / 2, (int)ScreenManager.ScreenDimensions.Y / 2);
            GameWorld.Instance.AddGameObject(go);

            // Create starting tower.
            go = TowerFactory.Instance.Create("Standard", new Vector2(600, 300));
            GameWorld.Instance.AddGameObject(go);
        }
        #endregion
    }
}
