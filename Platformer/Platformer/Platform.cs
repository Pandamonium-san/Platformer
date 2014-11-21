using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Platform:GameObject
    {
        public bool isSolid;

        public Platform(Texture2D texture, Vector2 pos, int width, int height):base(texture, pos)
        {
            isSolid = true;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        public void Update()
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        }
    }
}
