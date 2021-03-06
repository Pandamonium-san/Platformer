﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    abstract class Actor:GameObject
    {

        public float speed;
        public bool dead, invulnerable;
        public double invulnerableCount, invulnerableTime = 200;
        public int currentHealth, maxHealth;

        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; if (currentHealth > maxHealth) currentHealth = maxHealth; if (currentHealth < 0) currentHealth = 0; }
        }


        public Actor(Texture2D texture, Vector2 pos):base(texture, pos)
        {

        }

        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
            Gravity(gameTime);
            Friction(gameTime);
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
            Invulnerability(gameTime);

            base.Update(gameTime);
        }

        public void MoveAsFarAsPossible(GameTime gameTime)  //...before you collide with a platform
        {
            AddVelocityToPosition(gameTime);
            pos = PossiblePosition(oldPos, pos, hitbox);
        }

        public Vector2 PossiblePosition(Vector2 currentPos, Vector2 targetPos, Rectangle hitbox)    //Splits movement for one frame into multiple smaller steps
        {
            Vector2 movement = targetPos - currentPos;
            Vector2 furthestPossiblePos = currentPos;
            int numberOfSteps = (int)(movement.Length()) + 1;
            Vector2 oneStep = movement / numberOfSteps;

            for (int i = 0; i <= numberOfSteps; i++)
            {
                Vector2 attemptedPos = currentPos + oneStep * i;
                Rectangle newHitbox = new Rectangle(
                    (int)attemptedPos.X + offsetX - (int)vectorOrigin.X,
                    (int)attemptedPos.Y + offsetY - (int)vectorOrigin.Y,
                    hitbox.Width,
                    hitbox.Height);

                if (!CollidingWithPlatform(newHitbox))
                    furthestPossiblePos = attemptedPos;
                else
                {
                    if (movement.X != 0 && movement.Y != 0)     //Moves the remaining diagonal amount
                    {
                        int stepsLeft = numberOfSteps - (i - 1);

                        Vector2 remainingHorizontalMovement = oneStep.X * Vector2.UnitX * stepsLeft;
                        Vector2 finalPosIfHorizontal = furthestPossiblePos + remainingHorizontalMovement;
                        furthestPossiblePos = PossiblePosition(furthestPossiblePos, finalPosIfHorizontal, hitbox);

                        Vector2 remainingVerticalMovement = oneStep.Y * Vector2.UnitY * stepsLeft;
                        Vector2 finalPosIfVertical = furthestPossiblePos + remainingVerticalMovement;
                        furthestPossiblePos = PossiblePosition(furthestPossiblePos, finalPosIfVertical, hitbox);
                    }
                    break;
                }
            }
            return furthestPossiblePos;
        }

        public virtual void StopMovingIfBlocked()
        {
            Vector2 lastMovement = pos - oldPos;
            if (lastMovement.X == 0)
                velocity.X = 0;
            if (lastMovement.Y == 0)
                velocity.Y = 0;
        }
        public bool PixelCollision(GameObject go)
        {
            Color[] dataA = new Color[spriteRec.Width * spriteRec.Height];
            texture.GetData(0,
                spriteRec,
                dataA,
                0,
                spriteRec.Width * spriteRec.Height);

            Color[] dataB = new Color[go.spriteRec.Width * go.spriteRec.Height];
            go.texture.GetData(0,
                go.spriteRec,
                dataB,
                0,
                go.spriteRec.Width * go.spriteRec.Height);

            int top = Math.Max(hitbox.Top, go.hitbox.Top);
            int bottom = Math.Min(hitbox.Bottom, go.hitbox.Bottom);
            int left = Math.Max(hitbox.Left, go.hitbox.Left);
            int right = Math.Min(hitbox.Right, go.hitbox.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - hitbox.Left) + (y - hitbox.Top) * hitbox.Width];
                    Color colorB = dataB[(x - go.hitbox.Left) + (y - go.hitbox.Top) * go.hitbox.Width];
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool OnGround()
        {
            Rectangle rec = hitbox;
            rec.Width -= 1;
            rec.Offset(0, 1);
            return (CollidingWithPlatform(rec));
        }

        private void Friction(GameTime gameTime)
        {
            velocity *= (float)Math.Pow(0.95f, 60 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (OnGround())
                velocity *= (float)Math.Pow(0.95f, 60 * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected virtual void Knockback(Direction dir)
        {
            if (dir == Direction.left)
                velocity = new Vector2(-5, -5);
            else
                velocity = new Vector2(5, -5);
        }

        protected void Invulnerability(GameTime gameTime)
        {
            if (invulnerable)
            {
                if (alpha == 1f)
                    alpha = 0.2f;
                else
                    alpha = 1f;
                invulnerableCount += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (invulnerableCount > invulnerableTime)
            {
                invulnerable = false;
                alpha = 1f;
                invulnerableCount = 0;
            }
        }

        protected void Animate(GameTime gameTime)
        {
            frameTime += gameTime.ElapsedGameTime.TotalMilliseconds * Math.Abs(velocity.X / 5);

            if (frameTime > frameInterval && OnGround())
            {
                frame++;
                frameTime = 0;
                if (frame >= maxFrames)
                    frame = 0;
            }
        }

    }
}
