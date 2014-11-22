using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Layer
    {
        public Vector2 parallax;
        public List<Sprite> sprites;
        private Camera2D cam;

        public Layer(Camera2D cam, Vector2 parallax)
        {
            this.cam = cam;
            this.parallax = parallax;
            sprites = new List<Sprite>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cam.GetViewMatrix(parallax));
            foreach (Sprite sprite in sprites)
                sprite.Draw(spriteBatch);
            spriteBatch.End();
        }

    }
}
