using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Runner
{
    class GameOverScreen : GameScreen
    {
        #region Fields
        private TitleScreenButton startGameButton, loadGameButton, endGameButton;
        private List<TitleScreenButton> tsButtons;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for titel screen, with needed informations for buttons
        /// </summary>
        public GameOverScreen()
        {
            backgroundPath = "Background/GameOverBackground";
            startGameButton = new TitleScreenButton(Vector2.Zero, "Start Over");
            loadGameButton = new TitleScreenButton(Vector2.Zero, "Load Game");
            endGameButton = new TitleScreenButton(Vector2.Zero, "Exit Game");
            tsButtons = new List<TitleScreenButton>();
            tsButtons.Add(startGameButton);
            tsButtons.Add(loadGameButton);
            tsButtons.Add(endGameButton);
            AllignButtons();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads, draws , updates all the buttons, and button presses on screen in game
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            foreach (TitleScreenButton button in tsButtons)
            {
                button.LoadContent(contentManager);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (TitleScreenButton button in tsButtons)
            {
                button.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (TitleScreenButton button in tsButtons)
            {
                button.Update(gameTime);
            }
            HandleInput();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// handles input from user, and changes screen, according to button presses
        /// </summary>
        private void HandleInput()
        {
            if (startGameButton.State == GameButtonState.Activated)
            {
                ScreenManager.ChangeScreenTo(new PlayScreen(true));
            }
            else if (loadGameButton.State == GameButtonState.Activated)
            {
                ScreenManager.ChangeScreenTo(new PlayScreen(false));
                GameWorld.Instance.LoadObjectsFromRepo();
            }
            else if (endGameButton.State == GameButtonState.Activated)
            {
                ScreenManager.EndAndExitGame();
            }
        }
        /// <summary>
        /// Alligns buttons on screen
        /// </summary>
        private void AllignButtons()
        {
            Vector2 upperButtonPosition = new Vector2(ScreenManager.ScreenDimensions.X / 2, 400);
            for (int i = 0; i < tsButtons.Count; i++)
            {
                tsButtons[i].Position = upperButtonPosition + new Vector2(0, i * 120);
            }
        }
        #endregion
    }
}
