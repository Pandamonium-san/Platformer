using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class MeleeWeapon:Weapon
    {


        public MeleeWeapon(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            frameWidth = 176;
            frameHeight = 169;
            maxFrames = 9;

            //frameWidth = 32;
            //frameHeight = 32;
            //maxFrames = 3;

            frameInterval = 100;
            vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);
        }

        protected override void Attacking(GameTime gameTime)
        {
            if (attacking)
            {
                frameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameTime > frameInterval)
                {
                    frame++;
                    frameTime = 0;
                    if (frame >= maxFrames)
                    {
                        frame = 0;
                        attacking = false;
                    }
                }
            }
        }
    }
}
