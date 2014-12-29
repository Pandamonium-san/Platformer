using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Gargoyle : Monster
    {
        float projectileSpeed = 4f;
        Vector2 playerPos;

        bool leaping;
        float timer = 250f;

        public Gargoyle(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            monsterID = 4;

            frameWidth = 64;
            frameHeight = 64;
            spriteOriginX = 0;
            spriteOriginY = 0;
            maxFrames = 4;

            damage = 2;
            maxHealth = 20;
            CurrentHealth = maxHealth;

            speed = 8f / 60;
            velocity.X = speed;
            ranged = true;

            projectiles = new List<Projectile>();

            spriteRec = new Rectangle(frame * frameWidth + spriteOriginX, frameWidth * (int)dir + spriteOriginY, frameWidth, frameHeight);
            vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);
            hitbox = new Rectangle(
                (int)pos.X + offsetX - (int)vectorOrigin.X,
                (int)pos.Y + offsetY - (int)vectorOrigin.Y,
                spriteRec.Width - offsetX * 2,
                spriteRec.Height - offsetY * 2);
            frameInterval = 50;
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            

            if(projectiles != null)
                foreach (Projectile p in projectiles)
                    p.Update(gameTime);

            if (Game1.rnd.Next(100 + CurrentHealth * 10) == 1)
                Shoot(playerPos);

            if(leaping)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer <= 0)
                {
                    leaping = false;
                    Shoot(playerPos);
                    timer = Game1.rnd.Next(100,400);
                }
            }

            if (OnGround() && WallInFront())
                TurnAround();
            base.Update(gameTime);
        }

        public void TrackPlayer(Player player)
        {
            playerPos = player.pos;
        }

        protected override void Knockback(Direction dir)
        {
            if (Game1.rnd.Next(2) == 1)
            {
                Leap(dir);
                return;
            }
            base.Knockback(dir);
        }

        private void Leap(Direction dir)
        {
            if (dir == Direction.left)
            {
                velocity = new Vector2(10, -15);
                this.dir = Direction.right;
            }
            else
            {
                velocity = new Vector2(-10, -15);
                this.dir = Direction.left;
            }
            leaping = true;
        }

        protected override void SetParticleColor()
        {
            hitColor = new Color(Game1.rnd.Next(40, 80), Game1.rnd.Next(0, 20), Game1.rnd.Next(40, 80));
            deathColor = new Color(Game1.rnd.Next(40, 80), Game1.rnd.Next(0, 20), Game1.rnd.Next(40, 80));
        }

        public void Shoot(Vector2 target)
        {
            Vector2 direction = target - pos;
            if (direction.Length() >= 720)
                return;
                direction.Normalize();
                Projectile p = new Projectile(ObjectManager.ballTexture, pos, direction * projectileSpeed, 1, 750f, true, false, new Rectangle(0,0,19,19));
                projectiles.Add(p);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(projectiles != null)
            foreach(Projectile p in projectiles)
                p.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

    }
}
