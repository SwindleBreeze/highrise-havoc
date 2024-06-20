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
    public class MineRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spriteSheet;
        private readonly Texture2D _spriteSheet1;
        private Rectangle explosionRectangle = new Rectangle(3100, 0, 186, 168);
        private Vector2 TextureScale = new Vector2(0.35f, 0.35f);
        public int animationFrame = 1;
        public bool animationDirection = true;

        public MineRenderer(SpriteBatch spriteBatch, Texture2D spriteSheet, Texture2D spriteSheet1, Vector2 textureScale)
        {
            _spriteBatch = spriteBatch;
            _spriteSheet = spriteSheet;
            TextureScale = textureScale;
            _spriteSheet1 = spriteSheet1;
        }

        public void Draw(Mine mine)
        {
            if(!mine.IsExploded)
            _spriteBatch.Draw(_spriteSheet, mine.Position, mine.SourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
        }

        public void DrawExplosion(Mine mine)
        {
            if (animationFrame <= 0) return;
            if(animationFrame < 20 && animationDirection) {
                _spriteBatch.Draw(_spriteSheet1, new Vector2(mine.Position.X - (explosionRectangle.Width * TextureScale.X / 2 * (animationFrame / 8)), mine.Position.Y - (explosionRectangle.Height * TextureScale.Y * (animationFrame / 8))), explosionRectangle, Color.White, 0, new Vector2(0, 0), TextureScale * (animationFrame/ 8), SpriteEffects.None, 1);
                animationFrame++;
            }
            else {
                animationDirection = false;
                _spriteBatch.Draw(_spriteSheet1, new Vector2(mine.Position.X - (explosionRectangle.Width * TextureScale.X / 2  * (animationFrame / 8)), mine.Position.Y - (explosionRectangle.Height * TextureScale.Y * (animationFrame / 8))), explosionRectangle, Color.White, 0, new Vector2(0, 0), TextureScale * (animationFrame / 8), SpriteEffects.None, 1);
                animationFrame--;
            }

        }


        public MineRenderer ReturnCopy()
        {
            return new MineRenderer(_spriteBatch, _spriteSheet, _spriteSheet1, TextureScale);
        }
    }
}
