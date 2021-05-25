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
/// Gives all Gameobjects a position and the ability to move them around in the game
/// </summary>
    public class Transform
    {
        private Vector2 position;

        public Vector2 Position { get => position; set => position = value; }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Property for game object's rotation.
        /// </summary>
        public float Rotation { get; set; }

        public void Translate(Vector2 translation)
        {
            if(!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                position += translation;
            }
        }
    }
}
