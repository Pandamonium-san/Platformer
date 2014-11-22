using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class ParticleEngine2D
    {
        private Random rnd;
        List<Particle> particles;
        public Vector2 spawnLocation;

        public ParticleEngine2D()
        {
            rnd = new Random();
            particles = new List<Particle>();
        }

        public void CreateParticle(Texture2D texture, Vector2 pos, Vector2 velocity, Color color, float size, float lifeTime)
        {
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(rnd.NextDouble()*2-1);

            particles.Add(new Particle(texture, pos, velocity, angle, angularVelocity, size, color, lifeTime));
        }
        public void CreateParticle(Texture2D texture, Vector2 pos, Vector2 velocity, Color color, float size)
        {
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(rnd.NextDouble() * 2 - 1);
            float lifeTime = 200 + rnd.Next(400);

            particles.Add(new Particle(texture, pos, velocity, angle, angularVelocity, size, color, lifeTime));
        }

        public void Update(GameTime gameTime)
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].lifeTime <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
                p.Draw(spriteBatch);
        }
    }
}
