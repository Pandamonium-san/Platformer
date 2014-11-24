using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Skeleton:Monster
    {
        public ProjectileWeapon bow;
        public bool attacking;

        public Skeleton(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            monsterID = 3;

            frameWidth = 32;
            frameHeight = 32;
            spriteOriginX = 96;
            spriteOriginY = 32;
            maxFrames = 3;

            damage = 1;
            health = 10;

            speed = 4f / 60;
            velocity.X = speed;
            ranged = true;

            bow = new Bow(ObjectManager.bowTexture, pos);
            bow.equipped = true;

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
            Weapon(gameTime);
            if (Game1.rnd.Next(300) == 1)
                Jump();
            if (OnGround() && (WallInFront() || LedgeInFront()))
                TurnAround();
            base.Update(gameTime);
        }

        protected override void Knockback(Direction dir)
        {
            if (this.dir == dir)
                TurnAround();
            base.Knockback(dir);
        }

        protected override void SetParticleColor()
        {
            hitColor = new Color(Game1.rnd.Next(200, 255), Game1.rnd.Next(200, 255), Game1.rnd.Next(200, 255));
            deathColor = new Color(Game1.rnd.Next(200, 255), Game1.rnd.Next(200, 255), Game1.rnd.Next(200, 255));
        }

        private void Weapon(GameTime gameTime)
        {
            if (bow != null)
            {
                projectiles = bow.projectiles;
                bow.UpdateEquippedWeapon(gameTime, this);
                if (Game1.rnd.Next(200) == 1)
                Attack();
            }
        }

        public void Attack()
        {
            if (!bow.attacking)
            {
                bow.Attack();
                attacking = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            bow.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

    }
}
