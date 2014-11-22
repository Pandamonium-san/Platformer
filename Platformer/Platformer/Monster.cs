using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Monster : Actor
    {
        public int damage;

        public Monster(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            dir = Direction.right;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Move()
        {
            if (dir == Direction.right)
                speed = Math.Abs(speed);
            else if (dir == Direction.left)
                speed = -Math.Abs(speed);
            velocity.X += speed;
        }

        public void TakeDamage(Player player, int damage)
        {
            health -= damage;
            if (health <= 0)
                dead = true;
            invulnerable = true;
            Knockbacked(player);
            for (int i = 0; i < 5; i++)
            {
                
                GenerateHitParticle();
            }
            if (dead)
                for (int i = 0; i < 15; i++)
                {
                    GenerateDeathParticle();
                }
        }

        private void GenerateDeathParticle()
        {
            Color color = new Color(Game1.rnd.Next(1, 40), Game1.rnd.Next(200, 255), Game1.rnd.Next(1, 40));
            Vector2 particleVelocity = new Vector2(Game1.rnd.Next(-20, 20) / 20f, Game1.rnd.Next(-20, 20) / 20f);
            float lifeTime = lifeTime = 400 + Game1.rnd.Next(800);
            float size = 4f;
            ObjectManager.particleEngine.CreateParticle(Game1.colorTexture, pos, particleVelocity, color, size, lifeTime);
        }

        private void GenerateHitParticle()
        {
            Color color = new Color(Game1.rnd.Next(1, 40), Game1.rnd.Next(200, 255), Game1.rnd.Next(1, 40));
            Vector2 particleVelocity = new Vector2(Game1.rnd.Next(-40, 40) / 20f, Game1.rnd.Next(-40, 40) / 20f);
            float lifeTime = lifeTime = 200 + Game1.rnd.Next(100);
            float size = 2f;
            ObjectManager.particleEngine.CreateParticle(Game1.colorTexture, pos, particleVelocity, color, size, lifeTime);
        }

        public void Knockbacked(Player player)
        {
            if (player.dir == Player.Direction.left)
            {
                velocity = new Vector2(-5, -5);
            }
            else
            {
                velocity = new Vector2(5, -5);
            }
        }

        protected bool LedgeInFront()
        {
            Rectangle testRec;
            if (dir == Direction.right)
            {
                testRec = new Rectangle(hitbox.X + hitbox.Width, hitbox.Y + hitbox.Height, 1, 1);
                testRec.Offset(2, 1);
            }
            else
            {
                testRec = new Rectangle(hitbox.X, hitbox.Y + hitbox.Height, 1, 1);
                testRec.Offset(-2, 1);
            }
            return !CollidingWithPlatform(testRec);
        }

        protected bool WallInFront()
        {
            Rectangle testRec;
            if (dir == Direction.right)
            {
                testRec = new Rectangle(hitbox.X + hitbox.Width, hitbox.Y + hitbox.Height, 1, 1);
                testRec.Offset(2, -1);
            }
            else
            {
                testRec = new Rectangle(hitbox.X, hitbox.Y + hitbox.Height, 1, 1);
                testRec.Offset(-2, -1);
            }

            return CollidingWithPlatform(testRec);
        }

        protected void TurnAround()
        {
            if (dir == Direction.right)
                dir = Direction.left;
            else if (dir == Direction.left)
                dir = Direction.right;
        }
    }
}
