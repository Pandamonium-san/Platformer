using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class ObjectManager
    {
        public static Texture2D tileTexture, swordTexture;
        public static World world;

        public Jumper player;

        public void LoadContent(ContentManager Content)
        {
            tileTexture = Content.Load<Texture2D>("tile");
            swordTexture = Content.Load<Texture2D>("sword");
            world = new World();
            world.LoadWorld();

            player = new Jumper(swordTexture, Vector2.One * 50);
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            world.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }

    }
}
