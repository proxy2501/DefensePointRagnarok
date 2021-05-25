using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{/// <summary>
/// Author: Daniel Lund Justesen
/// 
/// Class that acts as the core for Command Pattern
/// </summary>
    public abstract class Command
    {
        #region Properties
        /// <summary>
        ///Author: Mikkel Emil Nielsen-Man
        ///
        /// Indicates the type of execution required for the command.
        /// "Single": Executed once per button press.
        /// "Continuous": Executed once every frame while the button is down.
        /// </summary>
        public string ExecutionType { get; protected set; }
        #endregion

        public abstract void Execute(Player player);
    }
}
