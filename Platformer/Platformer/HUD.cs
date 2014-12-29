using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class HUD
    {
        Rectangle rec;
        int hudHeight = 64;

        int maxHealth;
        HUDObject playerHealth;

        public HUD(int windowX, int windowY)
        {
            rec = new Rectangle(0, windowY - hudHeight, windowX, hudHeight);

            playerHealth = new HUDObject(new Vector2(8, 8), "Health", false);
        }

        public void Update(int playerHealth, int maxHealth)
        {
            this.maxHealth = maxHealth;
            this.playerHealth.value = playerHealth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Game1.hudFont, playerHealth.text + "  " + playerHealth.value.ToString() + @"/" + maxHealth.ToString(), new Vector2(10,0), Color.Green);
            for (int i = 0; i < maxHealth; i++)
            {
                spriteBatch.Draw(Game1.lifeHeart, playerHealth.pos + new Vector2(i * 20, 20), new Rectangle(16,0,16,16), Color.White);
            }
            for (int i = 0; i < playerHealth.value; i++)
            {
                spriteBatch.Draw(Game1.lifeHeart, playerHealth.pos + new Vector2(i * 20, 20), new Rectangle(0, 0, 16, 16), Color.White);
            }
            spriteBatch.End();
        }
    }
}
