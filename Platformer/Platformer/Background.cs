using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Background
    {
        Texture2D bgTexture;
        List<Layer> layers;
        Layer layer1;
        Camera2D cam;

        public Background(Camera2D cam)
        {
            this.cam = cam;
            layers = new List<Layer>();
            layer1 = new Layer(cam, new Vector2(0.5f, 0.5f));
        }

        public void LoadBackground(ContentManager Content)
        {
            bgTexture = Content.Load<Texture2D>("Mountains1");

            layers.Add(layer1);

            layer1.sprites.Add(new Sprite(bgTexture, new Vector2(-cam.origin.X, -cam.origin.Y)));
            layer1.sprites.Add(new Sprite(bgTexture, new Vector2(-cam.origin.X + bgTexture.Width, -cam.origin.Y)));
            layer1.sprites.Add(new Sprite(bgTexture, new Vector2(-cam.origin.X - bgTexture.Width, -cam.origin.Y)));

        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int a = (int)cam.Position.X / (int)(bgTexture.Width / layer1.parallax.X);
                layer1.sprites[0].pos.X = a * bgTexture.Width - cam.origin.X;
                layer1.sprites[1].pos.X = (a + 1) * bgTexture.Width - cam.origin.X;
                layer1.sprites[2].pos.X = (a + 2) * bgTexture.Width - cam.origin.X;

            //layers[0].sprites[0].pos = cam.pos - cam.origin;

            foreach (Layer layer in layers)
                layer.Draw(spriteBatch);
        }
    }
}
