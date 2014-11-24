using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Dagger:MeleeWeapon
    {

        public Dagger(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            if (texture == ObjectManager.daggerTexture)
            {
                weaponID = 1;

                damage = 1;
                cooldown = 150;
                weight = 2;

                frameWidth = 32;
                frameHeight = 32;
                maxFrames = 3;
                frameInterval = 50;

                vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);
            }
        }
    }
}
