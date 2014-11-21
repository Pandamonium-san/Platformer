using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class HUDObject
    {
        public Vector2 pos;
        public string text;
        public int value;

        public bool right;

        public HUDObject(Vector2 pos, string text, bool right)
        {
            this.pos = pos;
            this.right = right;
            if (right)
                this.pos.X = pos.X - Game1.hudFont.MeasureString(text).X;
            this.text = text;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.hudFont, text + value.ToString(), pos, Color.White);
        }

    }
}
