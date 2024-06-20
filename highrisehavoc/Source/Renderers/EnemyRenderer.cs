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
    public class EnemyRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spriteSheet;
        private readonly int _screenWidth;
        public bool isBeingDrawn = false;
        private int animationCounter = 0;
        private Vector2 TextureScale = new Vector2(0.35f, 0.35f);
        public EnemyRenderer(SpriteBatch spriteBatch, Texture2D spriteSheet, int screenWidth, Vector2 textureScale)
        {
            _spriteBatch = spriteBatch;
            _spriteSheet = spriteSheet;
            _screenWidth = screenWidth;
            TextureScale = new Vector2(textureScale.X * 0.75f, textureScale.Y * 0.75f);
        }

        public void Draw(Enemy enemy)
        {
            // if sprite is outside of screen, don't draw
            if (enemy.BodySpritePosition.X < 0 || enemy.BodySpritePosition.X > _screenWidth)
            {
                isBeingDrawn = false;
                return;
            }
            isBeingDrawn = true;
            if(!enemy.IsMoving)
            {
                _spriteBatch.Draw(_spriteSheet, enemy.BodySpritePosition, enemy.BodySourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
                _spriteBatch.Draw(_spriteSheet, enemy.HeadSpritePosition, enemy.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
            }
            else if(animationCounter < 30)
            {
                _spriteBatch.Draw(_spriteSheet, enemy.BodySpritePosition, enemy.BodySourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
                _spriteBatch.Draw(_spriteSheet, enemy.HeadSpritePosition, enemy.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
            }
            else if(animationCounter < 60)
            {
                _spriteBatch.Draw(_spriteSheet, enemy.BodySpritePosition1, enemy.BodySourceRectangle1, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
                _spriteBatch.Draw(_spriteSheet, new Vector2(enemy.HeadSpritePosition.X, enemy.HeadSpritePosition.Y+2), enemy.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
            }
            else if(animationCounter < 90)
            {
                _spriteBatch.Draw(_spriteSheet, enemy.BodySpritePosition2, enemy.BodySourceRectangle2, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
                _spriteBatch.Draw(_spriteSheet, enemy.HeadSpritePosition, enemy.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
            }
            else if(animationCounter < 120)
            {
                _spriteBatch.Draw(_spriteSheet, enemy.BodySpritePosition1, enemy.BodySourceRectangle1, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
                _spriteBatch.Draw(_spriteSheet, new Vector2(enemy.HeadSpritePosition.X, enemy.HeadSpritePosition.Y + 2), enemy.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
            }
            else
            {
                _spriteBatch.Draw(_spriteSheet, enemy.BodySpritePosition, enemy.BodySourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
                _spriteBatch.Draw(_spriteSheet, enemy.HeadSpritePosition, enemy.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
                animationCounter = 0;
            }
            animationCounter++;
            // _spriteBatch.Draw(_spriteSheet, enemy.HeadSpritePosition, enemy.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
            _spriteBatch.Draw(_spriteSheet, enemy.ArmsSpritePosition, enemy.ArmsSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.FlipHorizontally, 1);
        }

        public SpriteBatch GetSpriteBatch()
        {
            return _spriteBatch;
        }

        public Texture2D GetSpriteSheet()
        {
            return _spriteSheet;
        }
    }
}
