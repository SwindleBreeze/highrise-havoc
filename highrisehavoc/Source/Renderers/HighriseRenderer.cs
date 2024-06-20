using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace highrisehavoc
{
    public class HighriseRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spritesheet;
        public Vector2 TextureScale = new Vector2(0.60f, 0.60f);

        public HighriseRenderer(SpriteBatch spriteBatch, Texture2D spritesheet, Highrise highrise, Vector2 textureScale)
        {
            _spriteBatch = spriteBatch;
            _spritesheet = spritesheet;
            TextureScale = textureScale;

        }

        public void Draw(Highrise highrise)
        {
            _spriteBatch.Draw(_spritesheet, highrise.SpritePosition, highrise.SourceRectangle, Color.White, 0, new Vector2(0,0), TextureScale, new SpriteEffects(), 0.0f);
        }


    }
}