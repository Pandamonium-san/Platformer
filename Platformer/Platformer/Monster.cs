using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Monster:Actor
    {
        public int damage;

        public Monster(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            damage = 1;
            health = 3;
            offsetX = 2;
            offsetY = 6;
            speed = 10f/60;
            velocity.X = speed;
        }

        public override void Update(GameTime gameTime)
        {
            velocity.X += speed;
            if (WallInFront() || LedgeInFront())
                speed = -speed;

            base.Update(gameTime);
        }

        public void TakeDamage(Player player, int damage)
        {
            health -= damage;
            if (health <= 0)
                dead = true;
            invulnerable = true;
            Knockback(player);
        }
        public void Knockback(Player player)
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

        private bool LedgeInFront()
        {
            Rectangle testRec;
            if (velocity.X > 0)
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

        private bool WallInFront()
        {
            Rectangle testRec;
            if (velocity.X > 0)
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
    }
}
