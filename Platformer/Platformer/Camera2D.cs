using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Camera2D
    {
        public float zoom;
        public Vector2 origin;
        public float rotation;
        private Vector2 pos;
        private Rectangle? limits;

        private Viewport viewport;
        private int speed = 10;

        public Rectangle? Limits
        {
            get { return limits; }
            set
            {
                if(value != null)
                {
                    limits = new Rectangle
                    {
                        X = value.Value.X,
                        Y = value.Value.Y,
                        Width = System.Math.Max(viewport.Width, value.Value.Width),
                        Height = System.Math.Max(viewport.Height, value.Value.Height)
                    };
                    pos = pos;
                }
                else
                {
                    limits = null;
                }
            }
        }

        public Vector2 Position
        {
            get { return pos; }
            set
            {
                pos = value;

                if (limits != null && zoom == 1f && rotation == 0f)
                {
                    pos.X = MathHelper.Clamp(pos.X, limits.Value.X, limits.Value.X +
                        limits.Value.Width - viewport.Width);
                    pos.Y = MathHelper.Clamp(pos.Y, limits.Value.Y, limits.Value.Y +
                        limits.Value.Height - viewport.Height);
                }
            }
        }

        public Camera2D(Viewport viewport)
        {
            this.viewport = viewport;
            origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            zoom = 1.0f;
            rotation = 0f;
            pos = Vector2.Zero;
        }

        public void ControlCamera()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                Position -= Vector2.UnitY * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                Position += Vector2.UnitX * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                Position -= Vector2.UnitX * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                Position += Vector2.UnitY * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad7))
                rotation += 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
                rotation -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.Add))
                Zoom += 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
                Zoom -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
            { zoom = 1f; rotation = 0f; }
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            return Matrix.CreateTranslation(new Vector3(-pos * parallax, 0)) *
                                            Matrix.CreateRotationZ(rotation) *
                                            Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                            Matrix.CreateTranslation(new Vector3(origin, 0));
        }

    }
}
