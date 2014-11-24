using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Projectile:GameObject
    {
        public int damage;
        public float lifeTime;
        public bool throughWalls = false;

        public Projectile(Texture2D texture, Vector2 pos, Vector2 velocity, int damage, float range, bool throughWalls, bool hasDirection, Rectangle spriteRec):base(texture, pos)
        {
            this.velocity = velocity;
            this.damage = damage;
            this.throughWalls = throughWalls;
            this.hasDirection = hasDirection;

            if (velocity.X < 0)
                dir = Direction.left;

            lifeTime = range / velocity.Length();

            frameWidth = spriteRec.Width;
            frameHeight = spriteRec.Height;
        }

        public override void Update(GameTime gameTime)
        {
            if (!throughWalls)
                CollisionWithWall();
            AddVelocityToPosition(gameTime);
            lifeTime -= 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        private void CollisionWithWall()
        {
            if (CollidingWithPlatform(hitbox))
                lifeTime = 0;
        }
    }
}
