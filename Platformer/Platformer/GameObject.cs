using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    abstract class GameObject : StaticGameObject
    {
        public Vector2 velocity;
        public Vector2 oldPos;

        public enum Direction { left, right }
        public Direction dir = Direction.right;

        protected int frameWidth = 32, frameHeight = 32, frame, maxFrames = 3;
        protected int spriteOriginX, spriteOriginY;
        protected double frameInterval = 100, frameTime = 0;

        public bool hasDirection = true;

        public GameObject(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            hitbox = new Rectangle(
                (int)pos.X + offsetX - (int)vectorOrigin.X,
                (int)pos.Y + offsetY - (int)vectorOrigin.Y,
                spriteRec.Width - offsetX * 2,
                spriteRec.Height - offsetY * 2);
        }

        public bool CollidingWithPlatform(Rectangle hitbox)
        {
            foreach (var p in ObjectManager.platforms)
                if (velocity.Y >= 0 || p.isSolid)
                    if (hitbox.Intersects(p.hitbox))
                        return true;
            return false;
        }

        protected void AddVelocityToPosition(GameTime gameTime)
        {
            oldPos = pos;
            pos += velocity * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected void Gravity(GameTime gameTime)
        {
            velocity.Y += .6f * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool FellOff()
        {
            if (pos.Y > MapHandler.worldSize.Height)
                return true;
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (hasDirection)
            spriteRec = new Rectangle(frame * frameWidth + spriteOriginX, frameHeight * (int)dir + spriteOriginY, frameWidth, frameHeight);
            else
                spriteRec = new Rectangle(frame * frameWidth + spriteOriginX, spriteOriginY, frameWidth, frameHeight);

            //spriteBatch.Draw(Game1.colorTexture, hitbox, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.2f); //hitbox
            spriteBatch.Draw(texture, pos, spriteRec, color * alpha, rotation, vectorOrigin, 1f, SpriteEffects.None, layerDepth);
        }

    }
}
