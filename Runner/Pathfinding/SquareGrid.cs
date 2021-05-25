using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class defines a grid of squares that represent positions within the game world.
    /// </summary>
    public class SquareGrid
    {
        #region Fields
        // The size of the game world in pixels.
        private Vector2 worldDimensions;

        // The size of the grid in pixels.
        private Vector2 gridDimensions;

        // The radius of each node in pixels.
        private float nodeRadius, nodeWidth;

        // The number of nodes horizontally in the grid.
        private int numberOfNodesX;

        // The number of nodes vertically in the grid.
        private int numberOfNodesY;

        // Texture for drawing the grid.
        private Texture2D texture;

        // Color for nodes that are walkable.
        private Color walkableColor = new Color(100, 100, 100, 125);

        // Color for nodes that are unwalkable.
        private Color unwalkableColor = new Color(255, 0, 0, 125);

        // Color for nodes that are highlighted
        private Color hightlightedColor = new Color(160, 160, 160, 125);
        #endregion

        #region Properties
        /// <summary>
        /// A two-dimentional array that holds all nodes in the grid.
        /// </summary>
        public Node[,] Nodes { get; private set; }

        /// <summary>
        /// Determines if the grid is drawn or not.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// (DEBUG) If true, nodes can be set to unwalkable if clicked.
        /// </summary>
        public bool ClickToBlockNodes { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor. Creates a grid of squares that spans the entire game world.
        /// </summary>
        /// <param name="worldDimensions">A Vector2 that contains the size dimensions of the game world.</param>
        /// <param name="nodeWidth">A float specifying the desired width of all nodes within the grid.</param>
        public SquareGrid(Vector2 worldDimensions, float nodeWidth )
        {
            this.worldDimensions = worldDimensions;
            gridDimensions = worldDimensions;
            this.nodeWidth = nodeWidth;
            nodeRadius = nodeWidth / 2;

            // Calculate the number of nodes required to fill the grid, given the specified node width.
            numberOfNodesX = (int)Math.Round(gridDimensions.X / this.nodeWidth);
            numberOfNodesY = (int)Math.Round(gridDimensions.Y / this.nodeWidth);

            CreateGrid();
        }
        #endregion

        #region Pubic Methods
        /// <summary>
        /// LoadContent method.
        /// </summary>
        /// <param name="contentManager">A ContentManager.</param>
        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("Grid/GridTexture");
        }

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="gameTime">GameTime.</param>
        public void Update(GameTime gameTime)
        {
            if (!ClickToBlockNodes) return;

            MouseState ms = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(ms.Position, new Point(1));

            foreach (Node node in Nodes)
            {
                if (node.NodeRectangle.Intersects(mouseRectangle) && ms.LeftButton == ButtonState.Pressed)
                {
                    node.Walkable = false;
                }
            }
        }

        /// <summary>
        /// Draw method. Draws the grid if set to visible.
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch in which to draw the grid.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            // Draw all nodes in grid.
            foreach (Node node in Nodes)
            {
                // Create a destination rectangle that is 1 pixel smaller on every side than the node itself.
                // This is done so the border between nodes is visible.
                Rectangle destinationRectangle = new Rectangle(
                    node.NodeRectangle.X + 1,
                    node.NodeRectangle.Y + 1,
                    node.NodeRectangle.Width - 2,
                    node.NodeRectangle.Height - 2
                );

                // A color for the node when drawn.
                Color nodeColor = new Color();

                // Set color according to walkability.
                if (node.Walkable)
                {
                    nodeColor = walkableColor;
                }
                else
                {
                    nodeColor = unwalkableColor;
                }

                // Draw node.
                spriteBatch.Draw(texture, destinationRectangle, null, nodeColor, 0, Vector2.Zero, SpriteEffects.None, 0.01f);
            }
        }

        /// <summary>
        /// Takes a position in the game world and finds its corresponding node.
        /// </summary>
        /// <param name="position">A Vector2. A position in the game world.</param>
        /// <returns>The node corresponding to the position.</returns>
        public Node GetNodeFromPosition(Vector2 position)
        {
            // Calculate the position's percentage of total world size.
            float percentX = MathHelper.Clamp(position.X / worldDimensions.X, 0, 1);
            float percentY = MathHelper.Clamp(position.Y / worldDimensions.Y, 0, 1);

            // Use the percentage to derive the node indexes.
            int xIndex = (int)Math.Round((numberOfNodesX - 1) * percentX);
            int yIndex = (int)Math.Round((numberOfNodesY - 1) * percentY);

            // Return the node.
            return Nodes[xIndex, yIndex];
        }

        /// <summary>
        /// Checks for nodes that intersect with the input rectangle.
        /// </summary>
        /// <param name="rectangle">A Rectangle.</param>
        /// <returns>A list of nodes that intersect with the input rectangle.</returns>
        public List<Node> GetIntersectingNodes(Rectangle rectangle)
        {
            List<Node> intersectingNodes = new List<Node>();

            foreach (Node node in Nodes)
            {
                if (node.NodeRectangle.Intersects(rectangle))
                {
                    intersectingNodes.Add(node);
                }
            }

            return intersectingNodes;
        }

        /// <summary>
        /// Takes a position in the game world and fixates it to the center of nearest node.
        /// </summary>
        /// <param name="position">A Vector2. A position in the game world.</param>
        /// <returns>The position at the center of the nearest node.</returns>
        public Vector2 FixatePosition(Vector2 position)
        {
            return GetNodeFromPosition(position).Position;
        }

        /// <summary>
        /// Finds all nodes that are adjecent to a node in the grid.
        /// </summary>
        /// <param name="node">A Node whose adjacencies are to be found.</param>
        /// <returns>A list of Nodes that are adjecent in the grid to the input Node.</returns>
        public List<Node> GetAdjacentNodes(Node node)
        {
            List<Node> adjacencies = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Skip the node on index [0,0], as it is the center node (hence not an adjacency node).
                    if (x == 0 && y == 0) continue;

                    // Create values for the potential adjacent node.
                    int adjacentX = node.GridX + x;
                    int adjacentY = node.GridY + y;

                    // Add adjacent node if it lies within the grid.
                    if (adjacentX >= 0 && adjacentX < numberOfNodesX && adjacentY >= 0 && adjacentY < numberOfNodesY)
                    {
                        adjacencies.Add(Nodes[adjacentX, adjacentY]);
                    }
                }
            }

            return adjacencies;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Instansiates and populates the grid with nodes.
        /// Gives each node a position in the game word that corresponds with its grid index.
        /// </summary>
        private void CreateGrid()
        {
            // Instantiate Grid as a two-dimensional array of size equal to the number of nodes.
            Nodes = new Node[numberOfNodesX, numberOfNodesY];

            for (int x = 0; x < numberOfNodesX; x++)
            {
                for (int y = 0; y < numberOfNodesY; y++)
                {
                    // Calculate new node position according to its index.
                    Vector2 nodePosition = new Vector2(nodeWidth * x + nodeRadius, nodeWidth * y + nodeRadius);

                    // Calculate new node rectangle according to its index.
                    Rectangle nodeRectangle = new Rectangle(
                        (int)(nodePosition.X - nodeRadius),
                        (int)(nodePosition.Y - nodeRadius),
                        (int)nodeWidth,
                        (int)nodeWidth
                    );

                    // Enter new node into the grid.
                    Nodes[x, y] = new Node(nodePosition, nodeRectangle, x, y);
                }
            }
        }
        #endregion
    }
}
