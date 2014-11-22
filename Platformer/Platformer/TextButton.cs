using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class TextButton:Button
    {
        String name;
        SpriteFont font;

        public TextButton(SpriteFont font, Vector2 pos, String name):base(pos, 0, 0)
        {
            this.font = font;
            this.name = name;
            origin = font.MeasureString(name) / 2;

            this.rec = new Rectangle((int)(pos.X - origin.X), (int)(pos.Y - origin.Y), (int)(font.MeasureString(name).X), (int)(font.MeasureString(name).Y));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(font, name, pos, color, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
