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

        HUDObject playerHealth;

        public HUD(int windowX, int windowY)
        {
            rec = new Rectangle(0, windowY - hudHeight, windowX, hudHeight);

            playerHealth = new HUDObject(new Vector2(8, 8), "", false);
        }

        public void Update(int playerHealth)
        {
            this.playerHealth.value = playerHealth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < playerHealth.value; i++)
            {
                spriteBatch.Draw(Game1.lifeHeart, playerHealth.pos + new Vector2(i * 20, 0), null, Color.White);
            }
            spriteBatch.End();
        }
    }
}
