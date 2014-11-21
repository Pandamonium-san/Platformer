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
        public Rectangle hitbox, spriteRec;
        public Vector2 vectorOrigin;
        public float rotation = 0f;
        public float alpha = 1f;
        public Color color = Color.White;
        public bool followingMouse;

        public GameObject(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            spriteRec = new Rectangle(0, 0, texture.Width, texture.Height);
            hitbox = new Rectangle((int)pos.X + offsetX, (int)pos.Y + offsetY, texture.Width - offsetX * 2, texture.Height - offsetY * 2);
        }

        public bool FellOff(int worldSizeY)
        {
            if (pos.Y > worldSizeY)
                return true;
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.colorTexture, hitbox, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.2f); //hitbox
            spriteBatch.Draw(texture, pos, spriteRec, color * alpha, rotation, vectorOrigin, 1f, SpriteEffects.None, 0.1f);
        }

    }
}
