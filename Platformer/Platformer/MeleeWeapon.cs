using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    abstract class MeleeWeapon:Weapon
    {


        public MeleeWeapon(Texture2D texture, Vector2 pos):base(texture, pos)
        {
        }

        protected override void Attacking(GameTime gameTime)
        {
            if (!OnCooldown())
            {
                frame = 0;
                attacking = false;
            }
            if (attacking)
            {
                frameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameTime > frameInterval)
                {
                    frame++;
                    frameTime = 0;
                    if (OnCooldown() && frame >= maxFrames)
                    {
                        frame = maxFrames - 1;
                        attacking = false;
                    }
                }
            }
        }
    }
}
