using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    abstract class Monster : Actor
    {
        public List<Projectile> projectiles;

        public int damage;
        public int monsterID;
        protected Vector2 jumpStrength = new Vector2(0,-10);
        public bool ranged;

        protected Color hitColor, deathColor;

        public Monster(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            invulnerableTime = 200;
            dir = Direction.right;
        }

        public override void Update(GameTime gameTime)
        {
            if (FellOff())
                dead = true;
            base.Update(gameTime);
        }

        public void Move(GameTime gameTime)
        {
            if (dir == Direction.right)
                speed = Math.Abs(speed);
            else if (dir == Direction.left)
                speed = -Math.Abs(speed);
            velocity.X += speed * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected virtual void Jump()
        {
            if (OnGround())
            {
                hitbox.Y -= 1;
                velocity += jumpStrength + Vector2.UnitX * velocity * 4;
            }
        }

        public void TakeDamage(Direction dir, int damage)
        {
            if (invulnerable)
                return;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                dead = true;
            invulnerable = true;
            Knockback(dir);
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

        protected abstract void SetParticleColor();

        protected virtual void GenerateDeathParticle()
        {
            SetParticleColor();
            Vector2 particleVelocity = new Vector2(Game1.rnd.Next(-20, 20) / 20f, Game1.rnd.Next(-20, 20) / 20f);
            float lifeTime = lifeTime = 400 + Game1.rnd.Next(800);
            float size = 4f;
            ObjectManager.particleEngine.CreateParticle(Game1.colorTexture, pos, particleVelocity, hitColor, size, lifeTime);
        }

        protected virtual void GenerateHitParticle()
        {
            SetParticleColor();
            Vector2 particleVelocity = new Vector2(Game1.rnd.Next(-40, 40) / 20f, Game1.rnd.Next(-40, 40) / 20f);
            float lifeTime = lifeTime = 200 + Game1.rnd.Next(100);
            float size = 2f;
            ObjectManager.particleEngine.CreateParticle(Game1.colorTexture, pos, particleVelocity, deathColor, size, lifeTime);
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
