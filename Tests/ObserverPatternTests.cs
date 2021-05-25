using Runner;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class is for testing functionality associated with the Observer Pattern.
    /// </summary>
    [TestClass]
    public class ObserverPatternTests
    {
        private class TestListener : IGameListener
        {
            public bool HasBeenNotified = false;

            public void OnNotify(GameEvent gameEvent, Component component = null, Node node = null)
            {
                HasBeenNotified = true;
            }
        }

        [TestMethod]
        public void AfterAttachingListener_ListenerCanBeNotified()
        {
            // Arrange
            GameEvent testEvent = new GameEvent("TestEvent");
            TestListener listener = new TestListener();

            testEvent.Attach(listener);

            // Act
            testEvent.Notify();

            // Assert
            Assert.IsTrue(listener.HasBeenNotified);
        }

        [TestMethod]
        public void AfterDetachingListener_ListenerCannotBeNotified()
        {
            // Arrange
            GameEvent testEvent = new GameEvent("TestEvent");
            TestListener listener = new TestListener();

            testEvent.Attach(listener);
            testEvent.Detatch(listener);

            // Act
            testEvent.Notify();

            // Assert
            Assert.IsFalse(listener.HasBeenNotified);
        }
    }
}