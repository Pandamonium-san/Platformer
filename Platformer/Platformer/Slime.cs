using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Slime:Monster
    {

        public Slime(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            monsterID = 1;

            frameWidth = 32;
            frameHeight = 32;
            damage = 1;
            maxHealth = 3;
            CurrentHealth = maxHealth;
            offsetX = 2;
            offsetY = 6;
            speed = 5f / 60;
            velocity.X = speed;

            spriteRec = new Rectangle(frame * frameWidth + spriteOriginX, 32 * (int)dir + spriteOriginY, frameWidth, frameHeight);
            vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);
            hitbox = new Rectangle(
                (int)pos.X + offsetX - (int)vectorOrigin.X,
                (int)pos.Y + offsetY - (int)vectorOrigin.Y,
                spriteRec.Width - offsetX * 2,
                spriteRec.Height - offsetY * 2);
            maxFrames = 4;
            frameInterval = 50;
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);
            if (OnGround() && (WallInFront() || LedgeInFront()))
                TurnAround();
            base.Update(gameTime);
        }

        protected override void SetParticleColor()
        {
            hitColor = new Color(Game1.rnd.Next(1, 40), Game1.rnd.Next(200, 255), Game1.rnd.Next(1, 40));
            deathColor = new Color(Game1.rnd.Next(1, 40), Game1.rnd.Next(200, 255), Game1.rnd.Next(1, 40));
        }

    }
}
