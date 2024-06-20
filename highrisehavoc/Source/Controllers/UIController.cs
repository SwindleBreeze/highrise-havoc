using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using highrisehavoc.Source.Helpers;

namespace highrisehavoc.Source.Controllers
{
    public class UIController
    {
        private SpriteFont uiFont;
        private Vector2 _textureScale = new Vector2(0.60f, 0.60f);

        public UIController(SpriteFont font, Vector2 textureScale)
        {
            uiFont = font;
            _textureScale = textureScale;
        }

        public void Draw(SpriteBatch spriteBatch, int waveNumber, int playerGold, int currentHour, int highriseHP, bool gameOver)
        {
            if(gameOver)
            {
                spriteBatch.DrawString(uiFont, "GAME OVER", new Vector2(300, 300), Color.Black, 0, Vector2.Zero, 3, new SpriteEffects(), 1);
            }
            else {
                spriteBatch.DrawString(uiFont, "Day: " + waveNumber, new Vector2(10, 5), Color.Black, 0, Vector2.Zero, 4f * _textureScale, new SpriteEffects(), 1);
                spriteBatch.DrawString(uiFont, "Gold: " + playerGold, new Vector2(10, 55), Color.Black, 0, Vector2.Zero, 4f * _textureScale, new SpriteEffects(), 1);
                spriteBatch.DrawString(uiFont, "Hour: " + currentHour + ":00", new Vector2(140, 5), Color.Black, 0, Vector2.Zero, 4f * _textureScale, new SpriteEffects(), 1);
                spriteBatch.DrawString(uiFont, "Highrise HP: " + highriseHP, new Vector2(200, 55), Color.Black, 0, Vector2.Zero, 4f * _textureScale, new SpriteEffects(), 1);
            }
        }

        public void DrawStartScreen(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // draw buttons for "Start Game" , "Options" , "High Scores" , "Exit"
            spriteBatch.DrawString(uiFont, "Start Game", new Vector2(screenWidth/2 - 125, 510), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
            spriteBatch.DrawString(uiFont, "Options", new Vector2(screenWidth / 2 - 80, 590), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
            spriteBatch.DrawString(uiFont, "High Scores", new Vector2(screenWidth / 2 - 135, 670), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
            spriteBatch.DrawString(uiFont, "Exit", new Vector2(screenWidth / 2 - 35, 750), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
        }

        public void DrawOptionsScreen(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // draw buttons for "Sound" , "Music" , "Back"
            spriteBatch.DrawString(uiFont, "Sound", new Vector2(screenWidth / 2 - 195, screenHeight / 4 + 180 * _textureScale.Y), Color.White, 0, Vector2.Zero, 6 * _textureScale, new SpriteEffects(), 1);
            spriteBatch.DrawString(uiFont, "Music", new Vector2(screenWidth / 2 - 195, screenHeight / 4 + 350 * _textureScale.Y), Color.White, 0, Vector2.Zero, 6 * _textureScale, new SpriteEffects(), 1);
        }

        public void DrawHighScores(HighScoreData highScoreData, SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // draw high scores
            for (int i = 0; i < highScoreData.HighScores.Count; i++)
            {
                spriteBatch.DrawString(uiFont, (i + 1) + ". " + highScoreData.HighScores[i], new Vector2(screenWidth / 2 - 80, 380 + (i * 80)), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
            }
        }

        public void DrawTutorialScreen(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // Draw rounded rectangle with tutorial content
            HelperMethods.DrawRoundedRectangle(spriteBatch, new Rectangle(screenWidth / 2 - 400, 100, 800, 600), 20, Color.Gray);

            // Draw title
            string title = "Highrise Havoc";
            Vector2 titleSize = uiFont.MeasureString(title);
            spriteBatch.DrawString(uiFont, title, new Vector2(screenWidth / 2 - (titleSize.X * 2) / 2, 150), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);

            // Draw explanation of game premise
            string tutorialText = "A company of Green Berets have survived the initial alien invasion in \"The 4 Hour War of Florida\" in 2028. " +
                                   "The majority of the army in Florida has been wiped out. What remains is hiding in a highrise building under your control. " +
                                   "You must survive for 7 days for the US army to return and save you. " +
                                   "You gain score by building up your defenses, recruiting soldiers to your cause and annihilating the enemy. " +
                                   "All resources are bought by a money pool which increases over time and increases with each enemy killed. " +
                                   "Good luck commander, we are counting on you.";
            Vector2 tutorialPosition = new Vector2(screenWidth / 2 - (720 * _textureScale.X), 250);
            string[] lines = HelperMethods.WordWrap(tutorialText, uiFont, 720); // Adjust the width as needed
            float lineHeight = uiFont.MeasureString("A").Y * 2.5f; // Increase line height for readability
            foreach (string line in lines)
            {
                spriteBatch.DrawString(uiFont, line, tutorialPosition, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                tutorialPosition.Y += lineHeight;
            }
        }

        public void DrawUpgradesScreen(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // to be implemented
        }

        public void DrawHighScoresScreen(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // draw buttons for "Back"
            spriteBatch.DrawString(uiFont, "Back", new Vector2(screenWidth / 2 - 80, 510), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
        }

        public void DrawPauseScreen(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // draw buttons for "Resume" , "Options" , "Exit"
            spriteBatch.DrawString(uiFont, "Resume", new Vector2(screenWidth / 2 - 125, 510), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
            spriteBatch.DrawString(uiFont, "Options", new Vector2(screenWidth / 2 - 80, 590), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
            spriteBatch.DrawString(uiFont, "Exit", new Vector2(screenWidth / 2 - 35, 670), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
        }

        public void DrawGameOverScreen(SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            // draw buttons for "Restart" , "Options" , "Exit"
            spriteBatch.DrawString(uiFont, "Restart", new Vector2(screenWidth / 2 - 125, 510), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
            spriteBatch.DrawString(uiFont, "Options", new Vector2(screenWidth / 2 - 80, 590), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
            spriteBatch.DrawString(uiFont, "Exit", new Vector2(screenWidth / 2 - 35, 670), Color.Black, 0, Vector2.Zero, 4, new SpriteEffects(), 1);
        }

        public void DrawCurrentlyBeingDraw(SpriteBatch spriteBatch, int screenWidth, int screenHeight, int total_entities, int drawn_entities)
        {
            spriteBatch.DrawString(uiFont, "Currently drawing: " + drawn_entities + "/" + total_entities, new Vector2(screenWidth - 800, 40), Color.Black, 0, Vector2.Zero, 3, new SpriteEffects(), 1);
        }

        public void DrawHeaderBar(SpriteBatch spriteBatch, int screenWidth, int screenHeight, Texture2D emptyTexture)
        {
            // draw gray header bar on top of screen
            spriteBatch.Draw(emptyTexture, new Rectangle(0, 0, screenWidth, 100), Color.Gray);
        }
        public void DrawDeploymentBar(SpriteBatch spriteBatch, int screenWidth, int screenHeight, Rectangle soldierHeadRectangle, Rectangle aASoldierHeadRectangle, Rectangle obstacleRectangle, Rectangle mineRectangle, Texture2D spritesheet, Texture2D spritesheet1, Texture2D borderSprite)
        {
            // draw border sprite in center top of screen, with the solider head sprite in the middle of it
            spriteBatch.Draw(borderSprite, new Vector2(screenWidth / 2 - 150, 7), null, Color.White, 0, Vector2.Zero, 0.22f, SpriteEffects.None, 1);
            
            // get the sprite from spritesheet with the soldier head rectangle
            spriteBatch.Draw(spritesheet, new Vector2(screenWidth / 2 - 150 + 7, 12), soldierHeadRectangle, Color.White, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);

            spriteBatch.Draw(borderSprite, new Vector2(screenWidth / 2 - 50, 7), null, Color.White, 0, Vector2.Zero, 0.22f, SpriteEffects.None, 1);

            spriteBatch.Draw(spritesheet1, new Vector2(screenWidth / 2 - 50 + 6, 12), aASoldierHeadRectangle, Color.White, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);

            spriteBatch.Draw(borderSprite, new Vector2(screenWidth / 2 + 50, 7), null, Color.White, 0, Vector2.Zero, 0.22f, SpriteEffects.None, 1);

            spriteBatch.Draw(spritesheet1, new Vector2(screenWidth / 2 + 50 + 20, 12), obstacleRectangle, Color.White, 0, Vector2.Zero, 0.69f, SpriteEffects.None, 1);

            spriteBatch.Draw(borderSprite, new Vector2(screenWidth / 2 + 150, 7), null, Color.White, 0, Vector2.Zero, 0.22f, SpriteEffects.None, 1);

            spriteBatch.Draw(spritesheet1, new Vector2(screenWidth / 2 + 150 + 10, mineRectangle.Height+40), mineRectangle, Color.White, 0, Vector2.Zero, 0.69f, SpriteEffects.None, 1);
        }

        public void DrawSoldierSquare(SpriteBatch spriteBatch, Vector2 dragPostion, Rectangle soldierHeadRectangle, Texture2D spritesheet, Texture2D borderSprite)
        {
            spriteBatch.Draw(borderSprite, new Vector2(dragPostion.X - 80, dragPostion.Y - 80), null, Color.White, 0, Vector2.Zero, 0.22f, SpriteEffects.None, 1);

            spriteBatch.Draw(spritesheet, new Vector2(dragPostion.X - 80 + 7, dragPostion.Y - 80 + 5), soldierHeadRectangle, Color.White, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);
        }

        public void DrawAASoldierSquare(SpriteBatch spriteBatch, Vector2 dragPostion, Rectangle aASoldierHeadRectangle, Texture2D spritesheet, Texture2D borderSprite)
        {
            spriteBatch.Draw(borderSprite, new Vector2(dragPostion.X - 80, dragPostion.Y - 80), null, Color.White, 0, Vector2.Zero, 0.22f, SpriteEffects.None, 1);

            spriteBatch.Draw(spritesheet, new Vector2(dragPostion.X - 80 + 7, dragPostion.Y - 80 + 5), aASoldierHeadRectangle, Color.White, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 1);
        }

        public void DrawObstacleSquare(SpriteBatch spriteBatch, Vector2 dragPostion, Rectangle obstacleRectangle, Texture2D spritesheet, Texture2D borderSprite)
        {
            spriteBatch.Draw(borderSprite, new Vector2(dragPostion.X - 80, dragPostion.Y - 80), null, Color.White, 0, Vector2.Zero, 0.22f, SpriteEffects.None, 1);

            spriteBatch.Draw(spritesheet, new Vector2(dragPostion.X - 80 + 15, dragPostion.Y - 80 + 5), obstacleRectangle, Color.White, 0, Vector2.Zero, 0.69f, SpriteEffects.None, 1);
        }

        public void DrawMineSquare(SpriteBatch spriteBatch, Vector2 dragPostion, Rectangle mineRectangle, Texture2D spritesheet, Texture2D borderSprite)
        {
            spriteBatch.Draw(borderSprite, new Vector2(dragPostion.X - 80, dragPostion.Y - 80), null, Color.White, 0, Vector2.Zero, 0.22f, SpriteEffects.None, 1);

            spriteBatch.Draw(spritesheet, new Vector2(dragPostion.X - 80 + 15, dragPostion.Y - 80 + 60), mineRectangle, Color.White, 0, Vector2.Zero, 0.69f, SpriteEffects.None, 1);
        }
    }
}
