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
    public class EnemyPlaneRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spriteSheet;
        private readonly int _screenWidth;
        public bool isBeingDrawn = false;
        public int animationFrame = 1;
        private bool animationDirection = true;
        private Vector2 TextureScale = new Vector2(0.7f, 0.7f);
        private Rectangle explosionRectangle = new Rectangle(3100, 0, 186, 168);

        public EnemyPlaneRenderer(SpriteBatch spriteBatch, Texture2D spriteSheet, int screenWidth, Vector2 textureScale)
        {
            _spriteBatch = spriteBatch;
            _spriteSheet = spriteSheet;
            _screenWidth = screenWidth;
            TextureScale = textureScale;
        }

        public void Draw(EnemyPlane enemyPlane)
        {
            if(enemyPlane.isDead)
            {
                isBeingDrawn = false;
                return;
            }
            // if sprite is outside of screen, don't draw
            if (enemyPlane.BodySpritePosition.X < 0 - enemyPlane.BodySourceRectangle.Width || enemyPlane.BodySpritePosition.X > _screenWidth) 
            {
                isBeingDrawn = false;
                return;
            }
            isBeingDrawn = true;
            _spriteBatch.Draw(_spriteSheet, enemyPlane.BodySpritePosition, enemyPlane.BodySourceRectangle, Color.White, -0.6f, new Vector2(0, 0), 0.7f, SpriteEffects.FlipHorizontally, 1);
        }

        public void DrawExplosion(EnemyPlane enemyPlane)
        {
            if (animationFrame <= 0) return;
            if (animationFrame < 20 && animationDirection)
            {
                _spriteBatch.Draw(_spriteSheet, new Vector2(enemyPlane.BodySpritePosition.X - (explosionRectangle.Width * TextureScale.X / 2 * (animationFrame / 8)), enemyPlane.BodySpritePosition.Y - (explosionRectangle.Height * TextureScale.Y * (animationFrame / 8))), explosionRectangle, Color.White, 0, new Vector2(0, 0), TextureScale * (animationFrame / 8), SpriteEffects.None, 1);
                animationFrame++;
            }
            else
            {
                animationDirection = false;
                _spriteBatch.Draw(_spriteSheet, new Vector2(enemyPlane.BodySpritePosition.X - (explosionRectangle.Width * TextureScale.X / 2 * (animationFrame / 8)), enemyPlane.BodySpritePosition.Y - (explosionRectangle.Height * TextureScale.Y * (animationFrame / 8))), explosionRectangle, Color.White, 0, new Vector2(0, 0), TextureScale * (animationFrame / 8), SpriteEffects.None, 1);
                animationFrame--;
            }

        }


    }
}
