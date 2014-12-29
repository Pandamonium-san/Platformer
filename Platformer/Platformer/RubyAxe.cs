using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class RubyAxe:MeleeWeapon
    {

        public RubyAxe(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            if (texture == ObjectManager.axeTexture)
            {
                weaponID = 2;

                damage = 3;
                cooldown = 1300;
                weight = 7;


                weaponOffset = new Vector2(-5,-15);
                frameWidth = 88;
                frameHeight = 84;
                maxFrames = 9;
                frameInterval = 50;

                vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);
            }
        }
    }
}
