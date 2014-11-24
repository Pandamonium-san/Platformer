using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    abstract class Weapon:GameObject
    {
        public bool attacking, equipped;
        public int damage;
        Vector2 oldPos, velocity;

        public Player.Direction dir = Player.Direction.left;

        public Weapon(Texture2D texture, Vector2 pos)
            : base(texture, pos)
        {
            damage = 1;
            layerDepth = 0.05f;
            spriteRec = new Rectangle(frame * frameWidth, frameHeight * (int)dir, frameWidth, frameHeight);
            vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);

            hitbox = new Rectangle(
                (int)pos.X + offsetX - (int)vectorOrigin.X,
                (int)pos.Y + offsetY - (int)vectorOrigin.Y,
                spriteRec.Width - offsetX * 2,
                spriteRec.Height - offsetY * 2);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!equipped)
            {
                pos += velocity;
                velocity.Y += .6f;
                if (CollidingWithPlatform(hitbox))
                {
                    velocity = Vector2.Zero;
                    pos = oldPos;
                }
            }

            spriteRec = new Rectangle(frame * frameWidth, frameHeight * (int)dir, frameWidth, frameHeight);
            hitbox = new Rectangle(
                (int)pos.X + offsetX - (int)vectorOrigin.X,
                (int)pos.Y + offsetY - (int)vectorOrigin.Y,
                spriteRec.Width - offsetX * 2,
                spriteRec.Height - offsetY * 2);

            oldPos = pos;
        }

        public void UpdateEquippedWeapon(GameTime gameTime, Player player)
        {
            if (equipped)
            {
                WeaponIsHeld(player);
                Attacking(gameTime);
            }
            Update(gameTime);
        }

        public void PrepareToDropWeapon()
        {
            frame = 0;
            equipped = false;
        }

        private void WeaponIsHeld(Player player)
        {
            this.dir = player.dir;
            if (dir == Player.Direction.left)
            {
                this.pos = player.pos - Vector2.UnitX * spriteRec.Width/2;
            }
            else if (dir == Player.Direction.right)
            {
                this.pos = player.pos + Vector2.UnitX * spriteRec.Width / 2;
            }
        }

        public bool CollidingWithPlatform(Rectangle hitbox)
        {
            foreach (var p in ObjectManager.platforms)
                if (p.isSolid && hitbox.Intersects(p.hitbox))
                        return true;
            return false;
        }

        protected abstract void Attacking(GameTime gameTime);

        public void Attack()
        {
            attacking = true;
        }
    }
}
