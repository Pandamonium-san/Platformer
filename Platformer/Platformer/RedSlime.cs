using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class RedSlime : Slime
    {
        float jumpCooldown;
        bool jumped;

        public RedSlime(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            monsterID = 2;

            jumpStrength = new Vector2(0, -10);
            jumpCooldown = 3000;
            damage = 1;
            health = 5;
            speed = 6f / 60;
            velocity.X = speed;
        }

        public override void Update(GameTime gameTime)
        {
           if(Game1.rnd.Next(240)==1)
                Jump();
           if (LedgeInFront() && Game1.rnd.Next(4)==1)
                Jump();

            JumpCooldown(gameTime);
            base.Update(gameTime);
        }

        protected override void Jump()
        {
            if (OnGround() && !jumped)
            {
                hitbox.Y -= 1;
                velocity += jumpStrength + Vector2.UnitX * velocity * 4;
                jumped = true;
            }
        }

        private void JumpCooldown(GameTime gameTime)
        {
            if (jumped)
            {
                jumpCooldown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (jumpCooldown <= 0)
                {
                    jumpCooldown = 3000;
                    jumped = false;
                }
            }
        }

        protected override void SetParticleColor()
        {
            hitColor = new Color(Game1.rnd.Next(200, 255), Game1.rnd.Next(1, 40), Game1.rnd.Next(1, 40));
            deathColor = new Color(Game1.rnd.Next(200, 255), Game1.rnd.Next(1, 40), Game1.rnd.Next(1, 40));
        }

    }
}
