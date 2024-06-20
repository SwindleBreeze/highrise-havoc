using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using highrisehavoc.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace highrisehavoc.Source.Renderers
{
    public class ButtonRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;
        private readonly Texture2D _texture;
        private Vector2 _textureScale;

        public ButtonRenderer(SpriteBatch spriteBatch, SpriteFont font, Texture2D texture, Vector2 textureScale)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _texture = texture;
            _textureScale = textureScale;
        }

        public void Draw(Button button)
        {
            Color color = button.IsPressed ? Color.Gray : Color.White;

            // Draw the button background
            _spriteBatch.Draw(_texture, button.Position, null, color, 0, Vector2.Zero, _textureScale, SpriteEffects.None, 1);
            // _spriteBatch.Draw(_texture, button.Position, color);

            // Draw the text in the center of the button
            Vector2 textSize = _font.MeasureString(button.Text);

            // Center the text in the button rectangle
            int x = (int)(button.Position.X + (button.Bounds.Width / 2) - (textSize.X / 2));
            int y = (int)(button.Position.Y + (button.Bounds.Height / 2) - (textSize.Y / 2));

            Vector2 textPosition = new Vector2(x, y);
            
            _spriteBatch.DrawString(_font, button.Text, textPosition, Color.Black, 0, Vector2.Zero, 2 * _textureScale, SpriteEffects.None, 1);
        }
    }
}
