using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    class Jumper:DynamicGameObject
    {
        float jumpStrength = -20f;

        public Jumper(Texture2D texture, Vector2 pos):base(texture, pos)
        {
            acceleration = 20f; //pixels per second
            offsetY = 15;
            offsetX = 17;
        }

        public override void Update(GameTime gameTime)
        {
            Movement(gameTime);
            base.Update(gameTime);
        }

        private void Movement(GameTime gameTime)
        {
            if (OnGround())
                acceleration = 40f;
            else
                acceleration = 20f;

            if(Keyboard.GetState().IsKeyDown(Keys.Right))
                velocity.X += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(Keyboard.GetState().IsKeyDown(Keys.Left))
                velocity.X -= acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (KeyMouseReader.KeyPressed(Keys.Up) && OnGround())
                Jump();
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !OnGround() && velocity.Y < 0)
                velocity.Y -= .3f;
        }

        private void Jump()
        {
            velocity.Y = jumpStrength;
        }
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.colorTexture, hitbox, null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            base.Draw(spriteBatch);
        }
    }
}
