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
        private double dropOffInterval;

        float jumpStrength;
        float baseJumpStrength;
        float baseSpeed;

        public bool attacking, running, dying, weaponIsEquipped;
        bool rightKeyPressed, leftKeyPressed;
        double inputCountR, inputTimeR = 500;
        double inputCountL, inputTimeL = 500;

        public Player(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            invulnerableTime = 750;
            maxHealth = 15;
            CurrentHealth = maxHealth;
            baseJumpStrength = 15f;
            baseSpeed = 20f / 60;

            offsetY = 0;
            offsetX = 4;
            spriteOriginX = 96*3;
            spriteOriginY = 160;
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

                SetSpeedAndJumpDependingOnFactors();
                Movement(gameTime);
                PlayerDeath();

                base.Update(gameTime);
                Weapon(gameTime);

                if (FellOff())
                    PlayerFell();

                if (OnGround() && dropOffInterval <= 0)
                {
                    dropOffPos = pos;
                    dropOffInterval = 1;
                }
                dropOffInterval -= gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void Movement(GameTime gameTime)
        {
            acceleration = Vector2.Zero;

            Run(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                acceleration.X = speed;
                if (!attacking)
                    dir = Direction.right;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                acceleration.X = -speed;
                if (!attacking)
                    dir = Direction.left;
            }
            if ((KeyMouseReader.KeyPressed(Keys.Up) || KeyMouseReader.KeyPressed(Keys.Space)) && OnGround())
                Jump();
            if ((Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.Space)) && !OnGround() && velocity.Y < 0)
                velocity.Y -= .4f;

        }

        private void SetSpeedAndJumpDependingOnFactors()
        {
            float currentSpeed = baseSpeed;

            if (equippedWeapon != null)
            currentSpeed = baseSpeed - equippedWeapon.weight/60f;

            if (running)
                currentSpeed *= 1.5f;
            if (!OnGround())
                currentSpeed *= 0.5f;

            if (running && OnGround())
                GenerateSmokeParticle();

            speed = currentSpeed;

            float currentJumpStrength = baseJumpStrength;
            if (equippedWeapon != null)
                currentJumpStrength -= equippedWeapon.weight / 2;
            if (currentJumpStrength <= 7)
                currentJumpStrength = 7;

            jumpStrength = currentJumpStrength;
        }

        private void Jump()
        {
            velocity.Y = -jumpStrength;
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

        public void TakeDamage(Direction dir, int damage)
        {
            if (invulnerable)
                return;
            running = false;
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            { dying = true; invulnerableCount = -2000; }
            invulnerable = true;
            Knockback(dir);
        }

        public void PlayerFell()
        {
            CurrentHealth -= 1;
            if (CurrentHealth <= 0)
                dead = true;
            invulnerable = true;
            invulnerableCount = -1000;
            if(!dead)
            pos = dropOffPos;
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
            if (equippedWeapon != null && Keyboard.GetState().IsKeyDown(Keys.Z) && !equippedWeapon.attacking)
            {
                equippedWeapon.Attack();
                attacking = true;
            }
            if (equippedWeapon == null || !equippedWeapon.attacking)
                attacking = false;
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
