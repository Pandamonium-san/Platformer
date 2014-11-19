using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera2D cam;
        public static Texture2D colorTexture;
        public static SpriteFont debugFont;
        public static Random rnd = new Random();
        ObjectManager objectManager;
        MapEditor mapEditor;

        enum GameState { Title, Playing, GameOver, MapEditor, Paused }
        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            colorTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            colorTexture.SetData<Color>(new Color[] { Color.White });
            debugFont = Content.Load<SpriteFont>("DebugFont");
            cam = new Camera2D();

            objectManager = new ObjectManager();
            objectManager.LoadContent(Content);

            mapEditor = new MapEditor();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            KeyMouseReader.Update(GraphicsDevice, cam);
            cam.Update();
            mapEditor.Update(Window, ObjectManager.world);
            switch (gameState)
            {
                case GameState.Title:
                    break;
                case GameState.Playing:
                    break;
                case GameState.GameOver:
                    break;
                case GameState.MapEditor:
                    break;
                case GameState.Paused:
                    break;
                default:
                    break;
            }
            
            objectManager.Update(gameTime);
            //cam.pos = objectManager.player.pos;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cam.get_transformation(GraphicsDevice));
            objectManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            DrawDebugInfo(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawDebugInfo(SpriteBatch spriteBatch)
        {
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", objectManager.player.pos.X, objectManager.player.pos.Y);
            string velocityInText = string.Format("Current velocity: ({0:0.0}, {1:0.0})", objectManager.player.velocity.X, objectManager.player.velocity.Y);
            string camPosInText = string.Format("Position of Mouse: ({0:0.0}, {1:0.0})", KeyMouseReader.mousePos.X, KeyMouseReader.mousePos.Y);
            string isOnGround = string.Format("On ground: {0}", objectManager.player.OnGround());

            spriteBatch.DrawString(debugFont, positionInText, new Vector2(10, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(debugFont, velocityInText, new Vector2(10, 20), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(debugFont, isOnGround, new Vector2(10, 40), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(debugFont, camPosInText, new Vector2(10, 60), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
