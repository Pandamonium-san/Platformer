using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Menu
    {
        public string currentMap = MapHandler.lvl1;
        public List<TextButton> titleButtons;
        public TextButton start, playButton, edit, exit, back;
        TextButton[] levelButtons;
        float centerX, centerY;

        public enum Screen { title, levelSelect, gameOver, paused }
        public Screen screen;

        public bool play, editing;
        string winLoseText;
        Color winLoseColor;
        string titleText = "platformer_title";

        public Menu(GameWindow window)
        {
            centerX = window.ClientBounds.Width / 2;
            centerY = window.ClientBounds.Height / 2;
            titleButtons = new List<TextButton>();
            titleButtons.Add(start = new TextButton(Game1.font, new Vector2(centerX, centerY+50),"START"));
            titleButtons.Add(exit = new TextButton(Game1.font, new Vector2(centerX, centerY+200), "EXIT"));
            screen = Screen.title;
        }

        public void FindCenter(GameWindow window)
        {
            centerX = window.ClientBounds.Width / 2;
            centerY = window.ClientBounds.Height / 2;
        }

        public void Update()
        {
            switch (screen)
            {
                case Screen.title:
                    foreach (TextButton b in titleButtons)
                        b.Update();
                    if (start.ButtonClicked())
                        LoadLevelSelect();
                    break;
                case Screen.levelSelect:
                    foreach (TextButton b in levelButtons)
                        b.Update();
                    playButton.Update();
                    back.Update();
                    edit.Update();
                    if (levelButtons[0].ButtonClicked())
                        currentMap = MapHandler.lvl1;
                    else if (levelButtons[1].ButtonClicked())
                        currentMap = MapHandler.lvl2;
                    else if (levelButtons[2].ButtonClicked())
                        currentMap = MapHandler.lvl3;

                    else if (playButton.ButtonClicked())
                        play = true;
                    else if (edit.ButtonClicked())
                        editing = true;
                    else if (back.ButtonClicked())
                        LoadTitleScreen();
                    break;
                case Screen.gameOver:
                    playButton.Update();
                    back.Update();
                    if (playButton.ButtonClicked())
                        play = true;
                    else if (back.ButtonClicked())
                        LoadTitleScreen();
                    break;
                case Screen.paused:
                    playButton.Update();
                    back.Update();
                    if (back.ButtonClicked())
                        LoadTitleScreen();
                    break;
            }
        }

        public void LoadTitleScreen()
        {
            titleButtons = new List<TextButton>();
            titleButtons.Add(start = new TextButton(Game1.font, new Vector2(centerX, centerY + 50), "START"));
            titleButtons.Add(exit = new TextButton(Game1.font, new Vector2(centerX, centerY + 200), "EXIT"));
            screen = Screen.title;
        }

        public void LoadGameOverScreen()
        {
            winLoseText = "You have been defeated...";
            winLoseColor = Color.Red;
            playButton = new TextButton(Game1.font, new Vector2(centerX, centerY), "Try again");
            back = new TextButton(Game1.font, new Vector2(centerX, centerY + 50), "Back to main menu");
            screen = Screen.gameOver;
        }
        public void LoadVictoryScreen()
        {
            winLoseText = "You defeated the bad guys!";
            winLoseColor = Color.Green;
            playButton = new TextButton(Game1.font, new Vector2(centerX, centerY), "Play again");
            back = new TextButton(Game1.font, new Vector2(centerX, centerY + 50), "Back to main menu");
            screen = Screen.gameOver;
        }

        public void LoadLevelSelect()
        {
            levelButtons = new TextButton[3];
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i] = new TextButton(Game1.font, new Vector2(centerX, centerY - 100 + i * 50), "LEVEL " + (i + 1));
            }

            playButton = new TextButton(Game1.font, new Vector2(centerX, centerY + 50), "PLAY");
            edit = new TextButton(Game1.font, new Vector2(centerX, centerY + 100), "EDIT");
            back = new TextButton(Game1.font, new Vector2(centerX, centerY + 150), "Back to main menu");
            screen = Screen.levelSelect;
        }

        public void LoadPauseScreen()
        {
            playButton = new TextButton(Game1.font, new Vector2(centerX, centerY - 50), "Resume");
            back = new TextButton(Game1.font, new Vector2(centerX, centerY), "Back to main menu");
            screen = Screen.paused;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            switch (screen)
            {
                case Screen.title:
                    foreach (TextButton b in titleButtons)
                        b.Draw(spriteBatch);
                    spriteBatch.DrawString(Game1.titleFont, titleText, new Vector2(centerX, centerY - 50) - Game1.titleFont.MeasureString(titleText)/2, Color.White);
                    break;
                case Screen.levelSelect:
                    foreach (TextButton b in levelButtons)
                        b.Draw(spriteBatch);
                    playButton.Draw(spriteBatch);
                    back.Draw(spriteBatch);
                    edit.Draw(spriteBatch);
                    spriteBatch.DrawString(Game1.font, "Current map: " + currentMap, Vector2.Zero, Color.White);
                    break;
                case Screen.gameOver:
                    playButton.Draw(spriteBatch);
                    back.Draw(spriteBatch);
                    spriteBatch.DrawString(Game1.font, winLoseText, new Vector2(centerX, centerY - 100)-Game1.font.MeasureString(winLoseText)/2, winLoseColor);
                    break;
                case Screen.paused:
                    spriteBatch.Draw(Game1.colorTexture, new Rectangle(0, 0, (int)centerX*2, (int)centerY*2), Color.Black * 0.5f);
                    playButton.Draw(spriteBatch);
                    back.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
        }
    }
}
