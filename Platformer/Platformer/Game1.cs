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

        public static Texture2D colorTexture, lifeHeart;
        public static SpriteFont debugFont, hudFont, titleFont, font;
        public static Random rnd = new Random();

        Camera2D cam;
        HUD hud;
        ObjectManager objectManager;
        MapEditor mapEditor;
        Menu menu;

        public enum GameState { Title, Playing, MapEditor, Paused }
        GameState gameState = GameState.Title;

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
            lifeHeart = Content.Load<Texture2D>("heart");

            debugFont = Content.Load<SpriteFont>("DebugFont");
            hudFont = Content.Load<SpriteFont>("hudfont");
            titleFont = Content.Load<SpriteFont>("titlefont");
            font = Content.Load<SpriteFont>("font1");

            menu = new Menu(Window);
            objectManager = new ObjectManager();
            objectManager.LoadContent(Content);
            cam = new Camera2D();
            hud = new HUD(1280, 720);
            mapEditor = new MapEditor();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            KeyMouseReader.Update(GraphicsDevice, cam);


            switch (gameState)
            {
                case GameState.Title:
                    menu.Update();
                    cam.pos = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                    if (menu.play)
                    {
                        StartMap(menu.currentMap);
                        gameState = GameState.Playing;
                    }
                    if (menu.editing)
                    {
                        StartMap(menu.currentMap);
                        menu.editing = false;
                        gameState = GameState.MapEditor;
                    }
                    if (menu.exit.ButtonClicked() && menu.screen == Menu.Screen.title)
                        this.Exit();
                    break;
                case GameState.Playing:
                    objectManager.Update(gameTime);
                    cam.pos = objectManager.player.pos;
                    cam.RestrictCamera(MapHandler.worldSizeX, MapHandler.worldSizeY);
                    hud.Update(objectManager.player.health);
                    if (KeyMouseReader.KeyPressed(Keys.P))
                    {
                        menu.LoadPauseScreen();
                        gameState = GameState.Paused;
                    }
                    if (objectManager.win)
                    {
                        menu.LoadVictoryScreen();
                        gameState = GameState.Title;
                    }
                    if (objectManager.lose)
                    {
                        menu.LoadGameOverScreen();
                        gameState = GameState.Title;
                    }
                    break;
                case GameState.MapEditor:
                    cam.ControlCamera();
                    mapEditor.Update(Window, objectManager);
                    if (KeyMouseReader.KeyPressed(Keys.Enter))
                    {
                        MapHandler.SaveWorldToText(objectManager, menu.currentMap);
                        gameState = GameState.Title;
                    }
                    break;
                case GameState.Paused:
                    menu.Update();
                    if (menu.playButton.ButtonClicked() || KeyMouseReader.KeyPressed(Keys.P))
                        gameState = GameState.Playing;
                    if (menu.back.ButtonClicked())
                        gameState = GameState.Title;
                    break;
                default:
                    break;
            }
            

            base.Update(gameTime);
        }

        protected void StartMap(string path)
        {
            objectManager.Start(path);
            cam.pos = objectManager.player.pos;
            menu.play = false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (gameState)
            {
                case GameState.Title:
                    menu.Draw(spriteBatch);
                    break;
                case GameState.Playing:
                    objectManager.Draw(spriteBatch, GraphicsDevice, cam);
                    hud.Draw(spriteBatch);
                    break;
                case GameState.MapEditor:
                    objectManager.Draw(spriteBatch, GraphicsDevice, cam);
                    break;
                case GameState.Paused:
                    objectManager.Draw(spriteBatch, GraphicsDevice, cam);
                    menu.Draw(spriteBatch);
                    break;
            }

            DrawDebugInfo(spriteBatch);

            base.Draw(gameTime);
        }

        private void DrawDebugInfo(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", objectManager.player.pos.X, objectManager.player.pos.Y);
            string velocityInText = string.Format("Current velocity: ({0:0.0}, {1:0.0})", objectManager.player.velocity.X, objectManager.player.velocity.Y);
            string mousePosInText = string.Format("Position of Mouse: ({0:0.0}, {1:0.0})", KeyMouseReader.mousePos.X, KeyMouseReader.mousePos.Y);
            string camPos = string.Format("Camera position: ({0:0.0}, {1:0.0})", cam.pos.X, cam.pos.Y);
            string isOnGround = string.Format("On ground: {0}", objectManager.player.OnGround());
            string isRunning = string.Format("Running: {0}", objectManager.player.running);

            //spriteBatch.DrawString(debugFont, positionInText, new Vector2(10, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(debugFont, isRunning, new Vector2(10, 20), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(debugFont, camPos, new Vector2(10, 40), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(debugFont, mousePosInText, new Vector2(10, 60), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
