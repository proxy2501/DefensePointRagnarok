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
    /// This class is part of the Observer design pattern.
    /// It defines the functionality of a game event that IGameListeners can subscribe to.
    /// </summary>
    public class GameEvent
    {
        #region Fields
        // A list of all IGameListeners that subscribe to this event.
        private List<IGameListener> listeners = new List<IGameListener>();
        #endregion

        #region Properties
        /// <summary>
        /// Name of the event.
        /// </summary>
        public string Title { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that sets the title of the event.
        /// </summary>
        /// <param name="title">A string that becomes the title of the event.</param>
        public GameEvent(string title)
        {
            Title = title;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Subscribes an IGameListener to this event.
        /// </summary>
        /// <param name="listener">An IGameListener to be added to the list of subscribes.</param>
        public void Attach(IGameListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Unsubscribes an IGameListener from this event.
        /// </summary>
        /// <param name="listener">An IGameListener to be removed from the list of subscribes.</param>
        public void Detatch(IGameListener listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Notifies all subscribers to this event.
        /// </summary>
        public void Notify()
        {
            foreach (IGameListener listener in listeners)
            {
                listener.OnNotify(this);
            }
        }

        /// <summary>
        /// Notifies all subscribers to this event.
        /// </summary>
        /// <param name="other">(Optional) A component involved in this event.</param>
        public void Notify(Component other)
        {
            foreach (IGameListener listener in listeners)
            {
                listener.OnNotify(this, other);
            }
        }

        /// <summary>
        /// Notifies all subscribers to this event.
        /// </summary>
        /// <param name="node">(Optional) A node that triggers this event.</param>
        public void Notify(Node node)
        {
            foreach (IGameListener listener in listeners)
            {
                listener.OnNotify(this, null, node);
            }
        }
        #endregion
    }
}
