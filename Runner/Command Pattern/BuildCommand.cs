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
    /// This class defines the behavior of a command that creates towers and places them in the game world.
    /// </summary>
    public class BuildCommand : Command
    {
        #region Fields
        // The type of tower to be created.
        private string towerType;
        #endregion

        #region 
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="towerType"></param>
        public BuildCommand(string towerType)
        {
            ExecutionType = "Single";
            this.towerType = towerType;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Specifies the behavior of this command when executed.
        /// </summary>
        /// <param name="player">A Player object that executes this command.</param>
        public override void Execute(Player player)
        {
            if (player.KnockedBack) return;
            
            player.BuildTower(towerType);
        }
        #endregion
    }
}
