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
    public class ObstacleRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spriteSheet;
        public Vector2 TextureScale = new Vector2(0.60f, 0.60f);

        public ObstacleRenderer(SpriteBatch spriteBatch, Texture2D spriteSheet, Vector2 textureScale)
        {
            _spriteBatch = spriteBatch;
            _spriteSheet = spriteSheet;
            TextureScale = textureScale;
        }

        public void Draw(Obstacle obstacle)
        {   
            _spriteBatch.Draw(_spriteSheet, obstacle.Position, obstacle.SourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
        }

        public ObstacleRenderer ReturnCopy()
        {
            return new ObstacleRenderer(_spriteBatch, _spriteSheet, TextureScale);
        }
    }
}
