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
    /// This interface defines functionality for classes that need to listen to game events.
    /// </summary>
    public interface IGameListener
    {
        /// <summary>
        /// Defines how listener handles events.
        /// </summary>
        /// <param name="gameEvent">The notifying GameEvent.</param>
        /// <param name="component">A Component that is involved in the event (if applicable).</param>
        /// <param name="node">A Node that triggered the event (if applicable).</param>
        void OnNotify(GameEvent gameEvent, Component component = null, Node node = null);
    }
}
