using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{/// <summary>
/// Author: Daniel Lund Justesen
/// 
/// Specific type of Command for movement only
/// </summary>
    class MoveCommand : Command
    {
        private Vector2 velocity;

        public MoveCommand(Vector2 velocitybob)
        {
            ExecutionType = "Continuous"; // Contributor: Mikkel Emil Nielsen-Man
            this.velocity = velocitybob;
        }

        public override void Execute(Player player)
        {
            if (player.KnockedBack) return; // Contributor: Mikkel Emil Nielsen-Man
            
            player.Move(velocity);
        }
    }
}
