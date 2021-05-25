using Microsoft.Xna.Framework;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class defines the properties of a node that is contained within a square grid used for pathfinding.
    /// </summary>
    public class Node : IHeapItem<Node>
    {
        #region Fields
        /// Determines if the node is walkable, allowing game obects to path over it.
        private bool walkable;
        #endregion

        #region Properties
        /// <summary>
        /// The position in the game world that the node represents.
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// The rectangle that defines the node's dimensions and location in the game world.
        /// </summary>
        public Rectangle NodeRectangle { get; private set; }

        /// <summary>
        /// The node's horizontal index in a two-dimensional grid array.
        /// </summary>
        public int GridX { get; private set; }

        /// <summary>
        /// The node's vertical index in a two-dimensional grid array.
        /// </summary>
        public int GridY { get; private set; }

        /// <summary>
        /// Implemented from the IHeapItem interface.
        /// </summary>
        public int HeapIndex { get; set; }

        /// <summary>
        /// Public accessor for the walkable field.
        /// Triggers a game event when set, to allow pathing game objects to acquire a new path.
        /// </summary>
        public bool Walkable 
        {
            get
            {
                return walkable;
            }
            set
            {
                walkable = value;

                if (walkable)
                {
                    PathManager.Instance.NodeClearedEvent.Notify(this);
                }
                else
                {
                    PathManager.Instance.NodeBlockedEvent.Notify(this);
                }
            }
        }

        /// <summary>
        /// The GCost of the node, used in AStar pahfinding.
        /// </summary>
        public int GCost { get; set; }

        /// <summary>
        /// The HCost of the node, used in AStar pahfinding.
        /// </summary>
        public int HCost { get; set; }

        /// <summary>
        /// The FCost of the node, used in AStar pahfinding.
        /// </summary>
        public int FCost { get { return GCost + HCost; } }

        /// <summary>
        /// The parent of the node, used in AStar pahfinding.
        /// </summary>
        public Node Parent { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="position">A Vector2. The position in the game world that the node represents.</param>
        /// <param name="nodeRectangle">A Rectangle that defines the node's dimensions and locaiton in the game world.</param>
        /// <param name="gridX">An integer giving the node a vertical index in the two-dimentional grid array.</param>
        /// <param name="gridY">An integer giving the node a horizontal index in the two-dimentional grid array.</param>
        public Node(Vector2 position, Rectangle nodeRectangle, int gridX, int gridY)
        {
            Position = position;
            NodeRectangle = nodeRectangle;
            GridX = gridX;
            GridY = gridY;
            walkable = true;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Implemented from the IComparable interface.
        /// Compares this Node to another Node based on FCost first, HCost second.
        /// </summary>
        /// <param name="other">A Node to compare to.</param>
        /// <returns>1 if this Node has lower FCost, HCost than other Node.</returns>
        /// <returns>0 if this Node has same FCost, HCost as other Node.</returns>
        /// <returns>-1 if this Node has higher FCost, HCost than other Node.</returns>
        public int CompareTo(Node other)
        {
            // Compare FCost.
            int result = other.FCost.CompareTo(FCost);

            if (result == 0)
            {
                // If FCost are same, compare HCost.
                result = other.HCost.CompareTo(HCost);
            }

            // Return the comparison result.
            return result;
        }
        #endregion
    }
}
