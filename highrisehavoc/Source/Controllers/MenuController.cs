using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using highrisehavoc.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using highrisehavoc.Source.Renderers;
using Android.Content.Res;

namespace highrisehavoc.Source.Controllers
{
    public class MenuController
    {

        public Vector2 TextureScale;
        public Button startGameButton;
        public Button stopGameButton;
        public Button pauseGameButton;
        public Button optionsButton;
        public Button scoresButton;

        public Button musicToggleButton;
        public Button soundToggleButton;

        private ButtonRenderer startGameButtonRenderer;
        private ButtonRenderer stopGameButtonRenderer;
        private ButtonRenderer pauseGameButtonRenderer;
        private ButtonRenderer optionsButtonRenderer;
        private ButtonRenderer scoresButtonRenderer;

        private ButtonRenderer musicToggleButtonRenderer;
        private ButtonRenderer soundToggleButtonRenderer;

        public ButtonController startGameButtonController;
        public ButtonController stopGameButtonController;
        public ButtonController pauseGameButtonController;
        public ButtonController optionsButtonController;
        public ButtonController scoresButtonController;

        public ButtonController musicToggleButtonController;
        public ButtonController soundToggleButtonController;

        public MenuController(int screenWidth, int screenHeight, Vector2 textureScale)
        {
            startGameButton = new Button(new Rectangle(0, 0, (int)(430 * textureScale.X), (int)(190 * textureScale.Y)), new Vector2((screenWidth / 2) - ((int)(430 * textureScale.X) / 2), screenHeight / 2), "Start Game");
            optionsButton = new Button(new Rectangle(0, 0, (int)(430 * textureScale.X), (int)(190 * textureScale.Y)), new Vector2((screenWidth / 2) - ((int)(430 * textureScale.X) / 2), screenHeight / 2 + 150 * textureScale.Y), "Options");
            scoresButton = new Button(new Rectangle(0, 0, (int)(430 * textureScale.X), (int)(190 * textureScale.Y)), new Vector2((screenWidth / 2) - ((int)(430 * textureScale.X) / 2), screenHeight / 2 + 300 * textureScale.Y), "High Scores");
            stopGameButton = new Button(new Rectangle(0, 0, (int)(430 * textureScale.X), (int)(190 * textureScale.Y)), new Vector2((screenWidth / 2) - ((int)(430 * textureScale.X) / 2), screenHeight / 2 + 450 * textureScale.Y), "Exit");

            pauseGameButton = new Button(new Rectangle(0, 0, 220, 220), new Vector2(screenWidth - 220 * textureScale.X, 0), "");

            musicToggleButton = new Button(new Rectangle(0, 0, (int)(430 * textureScale.X), (int)(190 * textureScale.Y)), new Vector2(screenWidth / 2 + 80, screenHeight / 4 + 150 * textureScale.Y), "Off");
            soundToggleButton = new Button(new Rectangle(0, 0, (int)(430 * textureScale.X), (int)(190 * textureScale.Y)), new Vector2(screenWidth / 2 + 80, screenHeight / 4 + 300 * textureScale.Y), "Off");
            TextureScale = textureScale;
        }

        public void LoadContent(SpriteBatch _spriteBatch, SpriteFont font, Texture2D buttonSprite, Texture2D pauseButtonSprite, Vector2 textureScale)
        {
            startGameButtonRenderer = new ButtonRenderer(_spriteBatch, font, buttonSprite, textureScale);
            stopGameButtonRenderer = new ButtonRenderer(_spriteBatch, font, buttonSprite, textureScale);
            optionsButtonRenderer = new ButtonRenderer(_spriteBatch, font, buttonSprite, textureScale);
            scoresButtonRenderer = new ButtonRenderer(_spriteBatch, font, buttonSprite, textureScale);
            pauseGameButtonRenderer = new ButtonRenderer(_spriteBatch, font, pauseButtonSprite, textureScale);

            musicToggleButtonRenderer = new ButtonRenderer(_spriteBatch, font, buttonSprite, textureScale);
            soundToggleButtonRenderer = new ButtonRenderer(_spriteBatch, font, buttonSprite, textureScale);

            this.startGameButtonController = new ButtonController(this.startGameButton, startGameButtonRenderer);
            this.stopGameButtonController = new ButtonController(this.stopGameButton, stopGameButtonRenderer);
            this.optionsButtonController = new ButtonController(this.optionsButton, optionsButtonRenderer);
            this.scoresButtonController = new ButtonController(this.scoresButton, scoresButtonRenderer);
            this.pauseGameButtonController = new ButtonController(this.pauseGameButton, pauseGameButtonRenderer);

            this.musicToggleButtonController = new ButtonController(this.musicToggleButton, musicToggleButtonRenderer);
            this.soundToggleButtonController = new ButtonController(this.soundToggleButton, soundToggleButtonRenderer);
        }

        public void DrawHighScoresMenu()
        {
            this.stopGameButtonController.setText("Back");
            this.stopGameButtonController.Draw();
        }

        public void DrawOptionsMenu(Settings settings)
        {
            if(settings.MusicEnabled)
            {
                this.musicToggleButtonController.setText("Off");
            }
            else
            {
                this.musicToggleButtonController.setText("On");
            }

            if (settings.SoundEnabled)
            {
                this.soundToggleButtonController.setText("Off");
            }
            else
            {
                this.soundToggleButtonController.setText("On");
            }

            this.musicToggleButtonController.Draw();
            this.soundToggleButtonController.Draw();

            this.stopGameButtonController.setText("Back");
            this.stopGameButtonController.Draw();
        }

        public void DrawStartMenu()
        {
            this.startGameButtonController.setText("Start Game");
            this.startGameButtonController.Draw();
            this.optionsButtonController.Draw();

            this.scoresButtonController.setText("High Scores");
            this.scoresButtonController.Draw();
            this.stopGameButtonController.Draw();
        }

        public void DrawEndMenu(bool checkPointExists)
        {

            this.startGameButtonController.setText("Restart");
            this.startGameButtonController.Draw();
            if(checkPointExists) {
                this.scoresButtonController.setText("Continue");
                this.scoresButtonController.Draw();
            }
            this.optionsButtonController.Draw();
            this.stopGameButtonController.setText("Exit");
            this.stopGameButtonController.Draw();
        }
        public void DrawPauseMenu()
        {
            this.startGameButtonController.setText("Resume");
            this.startGameButtonController.Draw();
            this.optionsButtonController.Draw();
            this.stopGameButtonController.setText("Exit");
            this.stopGameButtonController.Draw();
        }

        public void DrawSettingsMenu()
        {
            this.stopGameButtonController.setText("Back");
            this.stopGameButtonController.Draw();
        }

        public void DrawPauseButton()
        {
            this.pauseGameButtonController.Draw();
        }

        public void DrawTutorialControls()
        {
            this.stopGameButtonController.setText("Continue");
            this.stopGameButtonController.Draw();
        }
    }
}
