using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Platform:StaticGameObject
    {
        public int platformID;
        public bool isSolid;

        public Platform(Texture2D texture, Vector2 pos, int platformID)
            : base(texture, pos)
        {
            this.platformID = platformID;

            spriteRec = new Rectangle(64 * (platformID - 1), 0, 64, 64);
            if (platformID >= 4)
                spriteRec = new Rectangle(192 + (platformID - 4) * 32, 0, 32, 32);
            if (platformID >= 6)
                spriteRec = new Rectangle(192 + (platformID - 6) * 32, 32, 32, 32);

            isSolid = true;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, spriteRec.Width, spriteRec.Height);
        }

        public void Update()
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, hitbox, spriteRec, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        }
    }
}
