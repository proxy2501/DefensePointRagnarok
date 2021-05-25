using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class utilizes the A* algorithm to find a path between two nodes in a square grid.
    /// </summary>
    public class Pathfinder
    {
        #region Fields
        // Reference to the square grid on which the pathfinder operates.
        private SquareGrid grid;

        // The cost of moving horizontally of vertically between two adjacent nodes.
        private int straightCost = 10;

        // The cost of moving diagonally between two adjacent nodes.
        private int diagonalCost = 14;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="grid">A SquareGrid for this pathfinder to operate on.</param>
        public Pathfinder(SquareGrid grid)
        {
            this.grid = grid;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Finds a path between two positions in the game world using the A* algorithm.
        /// </summary>
        /// <param name="startPosition">A Vector2 denoting the starting position.</param>
        /// <param name="endPosition">A Vector2 detoning the destination position.</param>
        public Vector2[] FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            // Get start and end nodes.
            Node startNode = grid.GetNodeFromPosition(startPosition);
            Node endNode = grid.GetNodeFromPosition(endPosition);

            // Instantiate open and closed sets to facilitate the A* algorithm.
            Heap<Node> openSet = new Heap<Node>(grid.Nodes.Length);
            HashSet<Node> closedSet = new HashSet<Node>();

            // Add start node to open set and commence A* algorithm.
            openSet.Add(startNode);

            while (openSet.Count() > 0)
            {
                // Get node from open set with lowest FCost and remove it from open set.
                Node currentNode = openSet.PopFirst();

                // Add current node to closed set.
                closedSet.Add(currentNode);

                // If path has been found, retrace and return path.
                if (currentNode == endNode)
                {
                    Vector2[] path = RetracePath(startNode, endNode);

                    return path;
                }

                foreach (Node adjacentNode in grid.GetAdjacentNodes(currentNode))
                {
                    // Skip unwalkable nodes or nodes that have already been visited.
                    if (!adjacentNode.Walkable || closedSet.Contains(adjacentNode)) continue;

                    // Calculate the GCost of moving to the adjacent node through current node.
                    int newGCost = currentNode.GCost + CalculateMovementCost(currentNode, adjacentNode);

                    if (newGCost < adjacentNode.GCost || !openSet.Contains(adjacentNode))
                    {
                        adjacentNode.GCost = newGCost;
                        adjacentNode.HCost = CalculateMovementCost(adjacentNode, endNode);
                        adjacentNode.Parent = currentNode;

                        if (!openSet.Contains(adjacentNode))
                        {
                            openSet.Add(adjacentNode);
                        }
                        else
                        {
                            openSet.UpdateItem(adjacentNode);
                        }
                    }
                }
            }

            // If openSet is empty, no path found: return null.
            return null;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Gets the cost of moving bewteen two nodes on the grid, assuming a direct path.
        /// </summary>
        /// <param name="nodeA">The origin node.</param>
        /// <param name="nodeB">The destination node.</param>
        /// <returns></returns>
        private int CalculateMovementCost(Node nodeA, Node nodeB)
        {
            int distanceX = Math.Abs(nodeA.GridX - nodeB.GridX);
            int distanceY = Math.Abs(nodeA.GridY - nodeB.GridY);

            if (distanceX > distanceY)
            {
                return distanceY * diagonalCost + (distanceX - distanceY) * straightCost;
            }
            else
            {
                return distanceX * diagonalCost + (distanceY - distanceX) * straightCost;
            }
        }

        /// <summary>
        /// Retraces the steps of the A* algorothm, generating a list of waypoints for an entity to follow. 
        /// </summary>
        /// <param name="startNode">The starting node in the path request.</param>
        /// <param name="endNode">The end node in the path request.</param>
        /// <returns>An array of Vector2 waypoints sorted in order from start to end.</returns>
        private Vector2[] RetracePath(Node startNode, Node endNode)
        {
            List<Vector2> parentChain = new List<Vector2>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                parentChain.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            // TODO: Add path simplification method (?)
            Vector2[] path = parentChain.ToArray();
            Array.Reverse(path);

            return path;
        }
        #endregion
    }
}
