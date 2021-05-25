using Runner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace Tests
{
    //Author: Arturas Tatariskinas

    [TestClass]
    public class TownHallTest : Component
    {
        private int health;

        public TownHallTest()
        {
            health = 100;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        [TestMethod]
        public void TownHallTakeDamage()
        {
            // Arrange
            var th = new TownHall(100);
            int expected = 90;
            
            // Act
            th.TakeDamage(10);
            
            // Assert
            Assert.AreEqual(th.Health,expected);
        }
    }
}
