using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Bow:ProjectileWeapon
    {

        public Bow(Texture2D texture, Vector2 pos):base(texture,pos)
        {
            weaponID = 3;

            damage = 1;
            range = 500f;
            baseProjectileSpeed = 7f;
            cooldown = 700;
            weight = 3;
            
            frameWidth = 15;
            frameHeight = 44;
            vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);

        }

        protected override void Attacking(GameTime gameTime)
        {
            if (attacking)
            {
                projectiles.Add(new Projectile(ObjectManager.arrowTexture, pos, new Vector2(ProjectileSpeed(), 0), damage, range, false, true, new Rectangle(0, 0, 36, 8)));
                attacking = false;
            }
        }

    }
}
