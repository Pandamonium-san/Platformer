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
        public int damage, weaponID;
        protected float cooldown, cooldownCount;
        public float weight;

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

        public override void Update(GameTime gameTime)
        {
            if (!equipped)
            {
                AddVelocityToPosition(gameTime);
                Gravity(gameTime);
                if (CollidingWithPlatform(hitbox))
                {
                    velocity = Vector2.Zero;
                    pos = oldPos;
                }
            }

            base.Update(gameTime);
        }

        public void UpdateEquippedWeapon(GameTime gameTime, Actor actor)
        {
            if (equipped)
            {
                cooldownCount -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                WeaponIsHeld(actor);
                Attacking(gameTime);
            }
            Update(gameTime);
        }

        public void PrepareToDropWeapon()
        {
            frame = 0;
            equipped = false;
        }

        private void WeaponIsHeld(Actor actor)
        {
            this.dir = actor.dir;
            if (dir == Player.Direction.left)
            {
                this.pos = actor.pos - Vector2.UnitX * spriteRec.Width / 2 - Vector2.UnitX * 5;
            }
            else if (dir == Player.Direction.right)
            {
                this.pos = actor.pos + Vector2.UnitX * spriteRec.Width / 2 + Vector2.UnitX * 5;
            }
        }

        protected abstract void Attacking(GameTime gameTime);

        public void Attack()
        {
            if (!OnCooldown())
            {
                attacking = true;
                cooldownCount = cooldown;
            }
        }

        protected bool OnCooldown()
        {
            if (cooldownCount > 0)
                return true;
            else
                return false;
        }
    }
}
