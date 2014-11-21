using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Weapon:GameObject
    {
        public bool swingWeapon;
        public int damage;

        int frameWidth = 32, frameHeight = 32, frame = 0;
        double animationCounter;
        int frameTime = 40;

        public Player.Direction dir = Player.Direction.left;

        public Weapon(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            damage = 1;

            spriteRec = new Rectangle(0, 0, 32, 32);
            vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);

            hitbox = new Rectangle(
                (int)pos.X + offsetX - (int)vectorOrigin.X,
                (int)pos.Y + offsetY - (int)vectorOrigin.Y,
                spriteRec.Width - offsetX * 2,
                spriteRec.Height - offsetY * 2);
        }

        public void Update(GameTime gameTime, Vector2 playerPos, Player.Direction playerDirection)
        {
            this.dir = playerDirection;
            if (dir == Player.Direction.left)
            {
                this.pos = playerPos - Vector2.UnitX * 25; //- Vector2.UnitY * 10;
                //rotation = -0.3f;
            }
            else if (dir == Player.Direction.right)
            {
                this.pos = playerPos + Vector2.UnitX * 25; // -Vector2.UnitY * 18;
                //rotation = 0.3f;
            }

            if (swingWeapon)
            {
               animationCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
               if (animationCounter > frameTime)
               {
                   frame++;
                   if (frame > 2)
                   {
                       frame = 0;
                       swingWeapon = false;
                   }
                   animationCounter = 0;
               }
            }

            spriteRec = new Rectangle(frame * frameWidth, 32 * (int)dir, 32, 32);

            hitbox = new Rectangle(
                (int)pos.X + offsetX - (int)vectorOrigin.X,
                (int)pos.Y + offsetY - (int)vectorOrigin.Y,
                spriteRec.Width - offsetX * 2,
                spriteRec.Height - offsetY * 2);
        }

        public void Swing()
        {
            swingWeapon = true;
        }
    }
}
