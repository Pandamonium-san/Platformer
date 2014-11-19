using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    abstract class GameObject : Sprite
    {
        public int offsetX, offsetY;
        public Rectangle hitbox;
        public GameObject(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            hitbox = new Rectangle((int)pos.X + offsetX, (int)pos.Y + offsetY, texture.Width - offsetX * 2, texture.Height - offsetY * 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, Color.White);
        }

    }
}
