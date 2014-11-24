using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    abstract class ProjectileWeapon:Weapon
    {
        public List<Projectile> projectiles;
        protected float baseProjectileSpeed;
        protected float range;

        public ProjectileWeapon(Texture2D texture, Vector2 pos):base(texture,pos)
        {
            projectiles = new List<Projectile>();
        }

        protected float ProjectileSpeed()
        {
            float currentSpeed = baseProjectileSpeed;

            if (dir == Direction.left)
                currentSpeed *= -1;

            return currentSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < projectiles.Count(); i++)
            {
                projectiles[i].Update(gameTime);
                if (projectiles[i].lifeTime <= 0)
                {
                    projectiles.Remove(projectiles[i]);
                    --i;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in projectiles)
                p.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

    }
}
