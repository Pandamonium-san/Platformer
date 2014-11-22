using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Sprite
    {
        public Texture2D texture;
        public Vector2 pos;

        public Sprite(Texture2D texture, Vector2 pos)
        {
            this.texture = texture;
            this.pos = pos;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(texture != null)
            spriteBatch.Draw(texture, pos, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
