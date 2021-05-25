using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    class TitleScreenButton : Button
    {
        public TitleScreenButton(Vector2 position) : base(position)
        {
            mouseOffTexturePath = "Buttons/Respawn1";
            mouseOverTexturePath = "Buttons/Respawn2";
            downTexturePath = "Buttons/Respawn3";
        }

        public TitleScreenButton(Vector2 position, string text) : base(position, text)
        {
            mouseOffTexturePath = "Buttons/Respawn1";
            mouseOverTexturePath = "Buttons/Respawn2";
            downTexturePath = "Buttons/Respawn3";
            fontPath = "Buttons/Font";
            fontColor = Color.White;
        }
    }
}
