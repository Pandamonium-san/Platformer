using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Button
    {
        public Rectangle rec;
        public Color color, defaultColor, highlightColor;
        protected Vector2 origin;
        protected Vector2 pos;
        protected float alpha;

        public Button(Vector2 pos, int x, int y)
        {
            this.pos = pos;
            origin = new Vector2(x/2,y/2);

            this.rec = new Rectangle((int)(pos.X - origin.X), (int)(pos.Y - origin.Y), x, y);
            defaultColor = Color.White;
            highlightColor = Color.Red;
            alpha = 1f;


        }

        public virtual bool ButtonClicked()
        {
            if (rec.Contains(KeyMouseReader.LeftClickPos))
                return true;
            else
                return false;
        }

        public virtual void Update()
        {
            if (rec.Contains(KeyMouseReader.mousePos))
                color = highlightColor;
            else
                color = defaultColor;
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Game1.colorTexture, rec, null, new Color(55,55,55,155) * alpha, 0f, Vector2.Zero, SpriteEffects.None, 1f);   //Draw hitbox
        }
    }
}
