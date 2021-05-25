using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This singleton class offers pathfinding-related functionality to entities in the game world.
    /// </summary>
    public class PathManager
    {
        #region Fields
        // The instance of this class.
        private static PathManager instance;
        #endregion

        #region Properties
        /// <summary>
        /// Singleton pattern property of this class' instance.
        /// </summary>
        public static PathManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PathManager();
                }

                return instance;
            }
        }

        /// <summary>
        /// a square grid that spans the entire game world.
        /// Entities access this to calculate a path.
        /// </summary>
        public SquareGrid Grid { get; private set; }

        /// <summary>
        /// Lock for pathing thread synchronization.
        /// Entities must obtain this lock before pathfinding on Grid.
        /// </summary>
        public object GridLock { get; private set; } = new object();

        /// <summary>
        /// Event for notifying entities that a node has become unwalkable.
        /// </summary>
        public GameEvent NodeBlockedEvent { get; private set; } = new GameEvent("NodeBlocked");

        /// <summary>
        /// Event for notifying entities that a node has become walkable.
        /// </summary>
        public GameEvent NodeClearedEvent { get; private set; } = new GameEvent("NodeCleared");
        #endregion

        #region Constructors
        /// <summary>
        /// Private constructor.
        /// </summary>
        private PathManager()
        {
            Grid = new SquareGrid(ScreenManager.ScreenDimensions, 60f);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// LoadContent method.
        /// </summary>
        /// <param name="contentManager">A ContentManager.</param>
        public void LoadContent(ContentManager contentManager)
        {
            Grid.LoadContent(contentManager);
        }

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="gameTime">A GameTime</param>
        public void Update(GameTime gameTime)
        {
            Grid.Update(gameTime);
        }

        /// <summary>
        /// Draw method. Draws the grid in debug mode.
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch in which to draw the grid.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Grid.Draw(spriteBatch);
        }

        public void ResetGrid()
        {
            foreach (Node node in Grid.Nodes)
            {
                node.Walkable = true;
            }
        }
        #endregion
    }
}