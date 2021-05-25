using Runner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class SquareGridTests
    {
        [TestMethod]
        public void AfterCreatingGrid_NodeCanBeFoundByPosition()
        {
            // Arrange
            SquareGrid grid = new SquareGrid(new Vector2(1920, 1080), 60f);
            Node someNode = grid.Nodes[6,3];

            // Act
            Node result = grid.GetNodeFromPosition(someNode.Position);

            // Assert
            Assert.AreSame(someNode, result);
        }

        [TestMethod]
        public void AfterCreatingGrid_CorrectNumberOfAdjacentNodesCanBeFound()
        {
            // Arrange
            SquareGrid grid = new SquareGrid(new Vector2(1920, 1080), 60f);
            Node topLeftNode = grid.Nodes[0, 0];
            Node middleLeftNode = grid.Nodes[0, 9];
            Node centerNode = grid.Nodes[16, 9];

            // Act
            List<Node> topLeftResult = grid.GetAdjacentNodes(topLeftNode);
            List<Node> middleLeftResult = grid.GetAdjacentNodes(middleLeftNode);
            List<Node> centerResult = grid.GetAdjacentNodes(centerNode);

            // Assert
            Assert.AreEqual(topLeftResult.Count, 3);
            Assert.AreEqual(middleLeftResult.Count, 5);
            Assert.AreEqual(centerResult.Count, 8);
        }

        [TestMethod]
        public void AfterCreatingGrid_UnwalkableNodesAreAdjacencies()
        {
            // Arrange
            SquareGrid grid = new SquareGrid(new Vector2(1920, 1080), 60f);
            Node someNode = grid.Nodes[16, 9];
            grid.Nodes[16,8].Walkable = false;
            grid.Nodes[15,8].Walkable = false;
            grid.Nodes[15, 10].Walkable = false;

            // Act
            List<Node> result = grid.GetAdjacentNodes(someNode);

            // Assert
            Assert.AreEqual(result.Count, 8);
        }

        [TestMethod]
        public void AfterCreatingGrid_GameWorldPositionCanBeFixatedToANode()
        {
            // Arrange
            SquareGrid grid = new SquareGrid(new Vector2(1920, 1080), 60f);
            Vector2 somePosition = new Vector2(865, 123);
            Vector2 closestNodePosition = grid.GetNodeFromPosition(somePosition).Position;

            // Act
            Vector2 result = grid.FixatePosition(somePosition);

            // Assert
            Assert.AreEqual(result, closestNodePosition);
        }
    }
}
