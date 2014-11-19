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
        protected float zoom;
        public Matrix transform;
        public Vector2 pos;
        protected float rotation;

        private int speed = 10;

        public Camera2D()
        {
            zoom = 1.0f;
            rotation = 0f;
            pos = Vector2.Zero;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                Move(-Vector2.UnitY * speed);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                Move(Vector2.UnitX * speed);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                Move(-Vector2.UnitX * speed);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                Move(Vector2.UnitY * speed);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad7))
                Rotation += 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
                Rotation -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.Add))
                Zoom += 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
                Zoom -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
            { zoom = 1f; rotation = 0f; pos = new Vector2(320, 320); }
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            pos += amount;
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            Viewport Viewport = graphicsDevice.Viewport;
            transform =
                Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
                                            Matrix.CreateRotationZ(Rotation) *
                                            Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                            Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5f, Viewport.Height * 0.5f, 0));
            return transform;
        }

    }
}
