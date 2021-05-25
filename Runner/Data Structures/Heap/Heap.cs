using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class defines a Heap datastructure.
    /// Any item in the Heap has at most 2 children, each of which have a lower priority than their parent.
    /// Therefore the item with the highest priority will always be at the top of the heap.
    /// How items are prioritized is defined by their implementation of the IHeapItem interface.
    /// </summary>
    /// <typeparam name="T">An element type that implements the IHeapItem interface.</typeparam>
    public class Heap<T> where T : IHeapItem<T>
    {
        #region Fields
        // An array of items stored in the heap.
        T[] items;

        // The number of items currently in the heap.
        int currentItemCount;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">The size of the heap. Determines how many items the heap can hold at maximum.</param>
        public Heap(int size)
        {
            items = new T[size];
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds an item to the heap.
        /// </summary>
        /// <param name="item">An item to be added.</param>
        public void Add(T item)
        {
            // Add new item to the end of the heap.
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;

            // Sort the item upwards in the heap until its position matches its priority.
            SortUp(item);

            // Change the heap's item count to reflect the addition of a new item.
            currentItemCount++;
        }

        /// <summary>
        /// Removes and returns the item at the top of the heap.
        /// </summary>
        /// <returns>The item in the heap with highest priority according to its implementetion of the IComparable interface.</returns>
        public T PopFirst()
        {
            // Cache the first item in the heap.
            T firstItem = items[0];

            // Set the last item in the heap as the new first item.
            items[0] = items[currentItemCount - 1];
            items[0].HeapIndex = 0;

            // Reduce the heap's item count by one.
            currentItemCount--;

            // Sort the new first item downwards in the heap until its position matches its priority.
            SortDown(items[0]);

            return firstItem;
        }

        /// <summary>
        /// Determines if an item exists in the heap.
        /// </summary>
        /// <param name="item">An item to find in the heap.</param>
        /// <returns>True if the heap contains the item.</returns>
        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        /// <summary>
        /// Returns the number of items in the heap.
        /// </summary>
        /// <returns>An integer equal to the number of items in the heap.</returns>
        public int Count()
        {
            return currentItemCount;
        }

        /// <summary>
        /// Updates the heap position of an item whose priority has changed.
        /// </summary>
        /// <param name="item">An item to update.</param>
        public void UpdateItem(T item)
        {
            SortUp(item);
            SortDown(item);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Sorts an item upwards in the heap until its parent has a higher priority.
        /// </summary>
        /// <param name="item">An item to sort.</param>
        private void SortUp(T item)
        {
            // Identify the index of this item's parent.
            int parentIndex = (item.HeapIndex - 1) / 2;

            // Repeat: Swap the item with its parent until its parent has higher priority.
            while (true)
            {
                // Get the parent item.
                T parentItem = items[parentIndex];

                if (item.CompareTo(parentItem) > 0)
                {
                    // Swap item with its parent if parent has a lower priority.
                    Swap(item, parentItem);
                }
                else
                {
                    // Stop sorting if parent has higher priority.
                    break;
                }

                // Identify new parent's index.
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        /// <summary>
        /// Sorts an item downwards in the heap until its children both have lower priority, or until it has no children.
        /// </summary>
        /// <param name="item">An item to be sorted.</param>
        private void SortDown(T item)
        {
            while (true)
            {
                // Set initial swap candidate to self.
                int swapCandidate = 0;

                // Identify the child indexes.
                int childAIndex = item.HeapIndex * 2 + 1;
                int childBIndex = item.HeapIndex * 2 + 2;

                // Set highest priority existing child as swap candidate.
                if (childAIndex < currentItemCount)
                {
                    swapCandidate = childAIndex;

                    if (childBIndex < currentItemCount)
                    {
                        if (items[childAIndex].CompareTo(items[childBIndex]) < 0)
                        {
                            swapCandidate = childBIndex;
                        }
                    }

                    // Compare parent with swap candidate. Swap if parent has lower priority.
                    if (item.CompareTo(items[swapCandidate]) < 0)
                    {
                        Swap(item, items[swapCandidate]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Swaps the positions of two items in the heap.
        /// </summary>
        /// <param name="itemA">An item to be swapped with item B.</param>
        /// <param name="itemB">An item to be swapped with item A.</param>
        private void Swap(T itemA, T itemB)
        {
            // Swap items in the array.
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;

            // Swap HeapIndexes of items.
            int tempHeapIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = tempHeapIndex;
        }
        #endregion
    }
}
