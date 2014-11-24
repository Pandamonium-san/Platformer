using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    class Player:Actor
    {
        public Weapon equippedWeapon;
        public Vector2 acceleration;
        public Vector2 dropOffPos;

        float jumpStrength;
        float groundSpeed;
        float midAirSpeed;

        public bool running, dying, weaponIsEquipped;
        bool rightKeyPressed, leftKeyPressed;
        double inputCountR, inputTimeR = 500;
        double inputCountL, inputTimeL = 500;

        public Player(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            invulnerableTime = 750;
            health = 10;
            jumpStrength = -15f;
            groundSpeed = 20f / 60;
            midAirSpeed = 10f / 60;

            offsetY = 0;
            offsetX = 4;
            spriteOriginX = 96;
            spriteOriginY = 161;
            spriteRec = new Rectangle(frame * frameWidth + spriteOriginX, frameHeight * (int)dir + spriteOriginY, frameWidth, frameHeight);
            vectorOrigin = new Vector2(frameWidth / 2, frameHeight / 2);
            hitbox = new Rectangle(
                (int)pos.X + offsetX - (int)vectorOrigin.X,
                (int)pos.Y + offsetY - (int)vectorOrigin.Y,
                spriteRec.Width - offsetX * 2,
                spriteRec.Height - offsetY * 2);
        }

        public override void Update(GameTime gameTime)
        {
            if (!dead)
            {
                velocity += acceleration * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Movement(gameTime);
                PlayerDeath();

                base.Update(gameTime);
                Weapon(gameTime);

                if (FellOff())
                {
                    PlayerFell();
                }
                if (OnGround())
                    dropOffPos = pos;
            }
        }

        private void Movement(GameTime gameTime)
        {
            acceleration = Vector2.Zero;
            if (running && !OnGround())
                speed = midAirSpeed * 1.5f;
            else if (running && OnGround())
            {
                speed = groundSpeed * 1.5f;
                GenerateSmokeParticle();
            }
            else if (OnGround())
                speed = groundSpeed;
            else
                speed = midAirSpeed;
            

            Run(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                acceleration.X = speed;
                dir = Direction.right;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                acceleration.X = -speed;
                dir = Direction.left;
            }
            if (KeyMouseReader.KeyPressed(Keys.Up) && OnGround())
                Jump();
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !OnGround() && velocity.Y < 0)
                velocity.Y -= .4f;
            
        }

        private void Jump()
        {
            velocity.Y = jumpStrength;
        }

        private void Run(GameTime gameTime)
        {
            if (rightKeyPressed)
            {
                if (inputCountR < inputTimeR && KeyMouseReader.KeyPressed(Keys.Right))
                    running = true;
                inputCountR += gameTime.ElapsedGameTime.TotalMilliseconds;
                if(inputCountR > inputTimeR)
                {
                    inputCountR = 0;
                    rightKeyPressed = false;
                }
            }
            if (leftKeyPressed)
            {
                if (inputCountL < inputTimeL && KeyMouseReader.KeyPressed(Keys.Left))
                    running = true;
                inputCountL += gameTime.ElapsedGameTime.TotalMilliseconds;
                if(inputCountL > inputTimeL)
                {
                    inputCountL = 0;
                    leftKeyPressed = false;
                }
            }

            if (!Keyboard.GetState().IsKeyDown(Keys.Left) && !Keyboard.GetState().IsKeyDown(Keys.Right))
                running = false;

            if (KeyMouseReader.KeyPressed(Keys.Right) && !rightKeyPressed)
                rightKeyPressed = true;
            if (KeyMouseReader.KeyPressed(Keys.Left) && !leftKeyPressed)
                leftKeyPressed = true;
        }

        public override void StopMovingIfBlocked()
        {
            Vector2 lastMovement = pos - oldPos;
            if (lastMovement.X == 0)
            {
                velocity.X = 0;
                running = false;
            }
            if (lastMovement.Y == 0)
                velocity.Y = 0;
        }

        public void TakeDamage(int damage)
        {
            running = false;
            health -= damage;
            if (health <= 0)
            { dying = true; invulnerableCount = -2000; }
            invulnerable = true;
            Knockback();
        }

        public void PlayerFell()
        {
            health -= 1;
            if (health <= 0)
                dying = true;
            pos = MapHandler.startingPos;
            invulnerable = true;
            invulnerableCount = -1000;
            pos = MapHandler.startingPos;
            velocity = Vector2.Zero;
        }

        private void Weapon(GameTime gameTime)
        {
            if (equippedWeapon != null)
            {
                equippedWeapon.UpdateEquippedWeapon(gameTime, this);
                Attack();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            equippedWeapon = weapon;
            equippedWeapon.equipped = true;
            weaponIsEquipped = true;
        }

        public void Attack()
        {
            if (equippedWeapon != null && KeyMouseReader.KeyPressed(Keys.Z) && !equippedWeapon.attacking)
                equippedWeapon.Attack();
        }

        private void PlayerDeath()
        {
            if (dying)
            {
                if(!dead)
                    GenerateDeathParticle();
                if(!invulnerable)
                    dead = true;
            }
        }

        private void GenerateSmokeParticle()
        {
            Vector2 particleVelocity = new Vector2(Game1.rnd.Next(-10, 10) / 20f, Game1.rnd.Next(-20, 5) / 20f);
            if (Game1.rnd.Next(1, 10) == 1)
                ObjectManager.particleEngine.CreateParticle(ObjectManager.smokeTexture, pos + Vector2.UnitY * 16, particleVelocity, Color.White, 1f);
        }

        private void GenerateDeathParticle()
        {
            Color color = new Color(Game1.rnd.Next(200, 255), Game1.rnd.Next(1, 40), Game1.rnd.Next(1, 40));
            Vector2 particleVelocity = new Vector2(Game1.rnd.Next(-10, 10) / 20f, Game1.rnd.Next(-2, 40) / 20f);
            float lifeTime = lifeTime = 500 + Game1.rnd.Next(100);
            float size = 4f;

            ObjectManager.particleEngine.CreateParticle(Game1.colorTexture, pos, particleVelocity, color, size, lifeTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Game1.colorTexture, hitbox, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            if (!dead)
            {
                if(equippedWeapon!=null)
                equippedWeapon.Draw(spriteBatch);
                base.Draw(spriteBatch);
            }
        }
    }
}
