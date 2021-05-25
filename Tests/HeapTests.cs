using Runner;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class is for testing functionality associated with the Heap datasatructure.
    /// </summary>
    [TestClass]
    public class HeapTests
    {
        private class TestHeapItem : IHeapItem<TestHeapItem>
        {
            public int Priority { get; set; }

            public int HeapIndex { get; set; }

            public TestHeapItem(int priority) { this.Priority = priority; }

            public int CompareTo(TestHeapItem other)
            {
                return Priority.CompareTo(other.Priority);
            }
        }

        [TestMethod]
        public void AfterAddingItem_ItemCanBeFound()
        {
            // Arrange
            Heap<TestHeapItem> heap = new Heap<TestHeapItem>(5);

            TestHeapItem item = new TestHeapItem(1);

            heap.Add(item);

            // Act
            bool result = heap.Contains(item);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AfterAddingItems_HeapHasCorrectCount()
        {
            // Arrange
            Heap<TestHeapItem> heap = new Heap<TestHeapItem>(5);
            TestHeapItem item1 = new TestHeapItem(1);
            TestHeapItem item2 = new TestHeapItem(2);
            TestHeapItem item3 = new TestHeapItem(3);
            TestHeapItem item4 = new TestHeapItem(4);
            TestHeapItem item5 = new TestHeapItem(5);

            heap.Add(item1);
            heap.Add(item2);
            heap.Add(item3);
            heap.Add(item4);
            heap.Add(item5);

            // Act
            int result = heap.Count();

            // Assert
            Assert.AreEqual(result, 5);
        }

        [TestMethod]
        public void AfterAddingItems_PopReturnsHighestPriorityItem()
        {
            // Arrange
            Heap<TestHeapItem> heap = new Heap<TestHeapItem>(5);
            TestHeapItem item1 = new TestHeapItem(13);
            TestHeapItem item2 = new TestHeapItem(26);
            TestHeapItem item3 = new TestHeapItem(43);
            TestHeapItem item4 = new TestHeapItem(254);
            TestHeapItem item5 = new TestHeapItem(14);

            heap.Add(item1);
            heap.Add(item2);
            heap.Add(item3);
            heap.Add(item4);
            heap.Add(item5);

            // Act
            TestHeapItem result = heap.PopFirst();

            // Assert
            Assert.AreSame(result, item4);
        }

        [TestMethod]
        public void WhenPoppingItems_ItemsArePoppedInOrderOfPriority()
        {
            // Arrange
            Heap<TestHeapItem> heap = new Heap<TestHeapItem>(5);
            TestHeapItem item1 = new TestHeapItem(13);
            TestHeapItem item2 = new TestHeapItem(26);
            TestHeapItem item3 = new TestHeapItem(43);
            TestHeapItem item4 = new TestHeapItem(254);
            TestHeapItem item5 = new TestHeapItem(14);

            heap.Add(item1);
            heap.Add(item2);
            heap.Add(item3);
            heap.Add(item4);
            heap.Add(item5);

            // Act
            TestHeapItem result1 = heap.PopFirst();
            TestHeapItem result2 = heap.PopFirst();
            TestHeapItem result3 = heap.PopFirst();
            TestHeapItem result4 = heap.PopFirst();
            TestHeapItem result5 = heap.PopFirst();

            // Assert
            Assert.AreSame(result1, item4);
            Assert.AreSame(result2, item3);
            Assert.AreSame(result3, item2);
            Assert.AreSame(result4, item5);
            Assert.AreSame(result5, item1);
        }

        [TestMethod]
        public void AfterUpdatingItem_ItemHasCorrectPlaceInHeap()
        {
            // Arrange
            Heap<TestHeapItem> heap = new Heap<TestHeapItem>(5);
            TestHeapItem item1 = new TestHeapItem(154);
            TestHeapItem item2 = new TestHeapItem(26);
            TestHeapItem item3 = new TestHeapItem(43);
            TestHeapItem item4 = new TestHeapItem(75);
            TestHeapItem item5 = new TestHeapItem(1);

            heap.Add(item1);
            heap.Add(item2);
            heap.Add(item3);
            heap.Add(item4);
            heap.Add(item5);

            item2.Priority = 300;
            heap.UpdateItem(item2);

            // Act
            TestHeapItem result = heap.PopFirst();

            // Assert
            Assert.AreSame(result, item2);
        }
    }
}