using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    ///  This interface must be implemented by classes that are to be stored in the Heap datastructure.
    /// </summary>
    /// <typeparam name="T">The type of element to be made viable for storing in a heap</typeparam>
    public interface IHeapItem<T> : IComparable<T>
    {
        #region Properties
        /// <summary>
        /// An index used to determine the item's position in the heap.
        /// </summary>
        int HeapIndex { get; set; }
        #endregion
    }
}
