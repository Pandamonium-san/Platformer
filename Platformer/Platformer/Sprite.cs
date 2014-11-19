using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    abstract class Sprite
    {
        public Texture2D texture;
        public Vector2 pos;

        public Sprite(Texture2D texture, Vector2 pos)
        {
            this.texture = texture;
            this.pos = pos;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
