using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{/// <summary>
/// Author: Daniel Lund Justesen
/// 
/// Core structure for Components and what functions can be used/is needed
/// </summary>
    public abstract class Component
    {
        public GameObject GameObject { get; set; }
        public bool IsEnabled { get; set; } = true;

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Destroys this component.
        /// </summary>
        public virtual void Destroy()
        {

        }
    }
}
