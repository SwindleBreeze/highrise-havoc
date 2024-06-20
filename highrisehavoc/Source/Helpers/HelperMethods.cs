using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace highrisehavoc.Source.Helpers
{
    public static class HelperMethods
    {
        // Helper method to draw a filled rounded rectangle
        public static void DrawRoundedRectangle(SpriteBatch spriteBatch, Rectangle rectangle, int radius, Color color)
        {
            Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { color });

            // Draw the filled rounded rectangle using 4 filled rectangles and 4 filled circles for the corners
            // spriteBatch.Draw(texture, new Rectangle(rectangle.X + radius, rectangle.Y, rectangle.Width - 2 * radius, rectangle.Height), color);
            // spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + radius, rectangle.Width, rectangle.Height - 2 * radius), color);

            // Draw the filled circles at the corners
            DrawCircle(spriteBatch, new Vector2(rectangle.X + radius, rectangle.Y + radius), radius, color);
            DrawCircle(spriteBatch, new Vector2(rectangle.X + rectangle.Width - radius, rectangle.Y + radius), radius, color);
            DrawCircle(spriteBatch, new Vector2(rectangle.X + radius, rectangle.Y + rectangle.Height - radius), radius, color);
            DrawCircle(spriteBatch, new Vector2(rectangle.X + rectangle.Width - radius, rectangle.Y + rectangle.Height - radius), radius, color);
        }

        // Helper method to draw a filled circle
        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 position, int radius, Color color)
        {
            Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { color });

            spriteBatch.Draw(texture, position, null, color, 0f, new Vector2(radius, radius), 1f, SpriteEffects.None, 0);
        }

        // Helper method for word wrapping
        public static string[] WordWrap(string text, SpriteFont font, float maxWidth)
        {
            List<string> lines = new List<string>();
            string[] words = text.Split(' ');
            string currentLine = "";

            foreach (string word in words)
            {
                if (font.MeasureString(currentLine + word).X > maxWidth)
                {
                    lines.Add(currentLine);
                    currentLine = "";
                }
                currentLine += word + " ";
            }

            if (!string.IsNullOrWhiteSpace(currentLine))
                lines.Add(currentLine);

            return lines.ToArray();
        }
    }
}
