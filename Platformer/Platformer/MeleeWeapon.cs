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
        bool backwards;

        public MeleeWeapon(Texture2D texture, Vector2 pos):base(texture, pos)
        {
        }

        protected override void Attacking(GameTime gameTime)
        {
            if (!OnCooldown())
            {
                frame = 0;
                attacking = false;
                backwards = false;
            }
            if (attacking)
            {
                frameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameTime > frameInterval)
                {
                    if (!backwards)
                    {
                        ++frame;
                        if (frame >= maxFrames)
                        {
                            frame = maxFrames - 1;
                            backwards = true;
                        }
                    }
                    else
                    {
                        --frame;
                        if (frame <= 0)
                        {
                            frame = 0;
                            backwards = false;
                            attacking = false;
                        }
                    }
                    
                    frameTime = 0;
                }
            }
        }
    }
}
