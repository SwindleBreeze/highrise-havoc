using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace highrisehavoc.Source.Extentions
{
    public static class SpriteBatchExtensions
    {
        public static void DrawRoundedRectangle(this SpriteBatch spriteBatch, Texture2D texture, Rectangle bounds, int borderRadius, Color color)
        {
            spriteBatch.Draw(texture, bounds, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0);

            // Draw rounded corners
            spriteBatch.Draw(texture, new Rectangle(bounds.Left, bounds.Top, borderRadius, borderRadius), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0); // Top-left
            spriteBatch.Draw(texture, new Rectangle(bounds.Right - borderRadius, bounds.Top, borderRadius, borderRadius), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0); // Top-right
            spriteBatch.Draw(texture, new Rectangle(bounds.Left, bounds.Bottom - borderRadius, borderRadius, borderRadius), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0); // Bottom-left
            spriteBatch.Draw(texture, new Rectangle(bounds.Right - borderRadius, bounds.Bottom - borderRadius, borderRadius, borderRadius), null, color, 0f, Vector2.Zero, SpriteEffects.None, 0); // Bottom-right
        }
    }
}


