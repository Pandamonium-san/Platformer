using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Powerup:StaticGameObject
    {

        public Powerup(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            spriteRec = new Rectangle(0, 0, 16, 16);
        }
    }
}
