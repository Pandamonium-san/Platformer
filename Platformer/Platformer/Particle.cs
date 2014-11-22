using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Particle:Sprite
    {
        public Vector2 velocity;
        public float angle, angularVelocity;
        public float size;
        public Color color;
        public float lifeTime;

        public Particle(Texture2D texture, Vector2 pos, Vector2 velocity,
            float angle, float angularVelocity, float size, Color color, float lifeTime)
            : base(texture, pos)
        {
            this.velocity = velocity;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.size = size;
            this.color = color;
            this.lifeTime = lifeTime;
        }

        public void Update(GameTime gameTime)
        {
            lifeTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            pos += velocity;
            angle += angularVelocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle spriteRec = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(spriteRec.Width / 2, spriteRec.Height / 2);

            spriteBatch.Draw(texture, pos, spriteRec, color, angle, origin, size, SpriteEffects.None, 0.7f);
        }

    }
}
