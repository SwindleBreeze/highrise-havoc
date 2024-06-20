using highrisehavoc.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace highrisehavoc.Source.Renderers
{
    public class HighriseLevelRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _sprite;
        private readonly Texture2D _plusSignSprite;
        private readonly Texture2D _soldierPositionSprite;
        public Vector2 TextureScale = new Vector2(0.65f, 0.65f);


        public HighriseLevelRenderer(SpriteBatch spriteBatch, Texture2D sprite, Texture2D plusSignSprite, Vector2 textureScale, Texture2D soldierPositionSprite)
        {
            _spriteBatch = spriteBatch;
            _sprite = sprite;
            _plusSignSprite = plusSignSprite;
            TextureScale = textureScale;
            _soldierPositionSprite = soldierPositionSprite;
        }

        public void Draw(List<HighriseLevel> levels, bool canBuildLevels)
        {
            foreach (var level in levels)
            {
                if (level.IsBuilt)
                {
                    _spriteBatch.Draw(_sprite, new Vector2(level.SpritePosition.X, level.SpritePosition.Y-((level.SourceRectangle.Height/2.5f)*TextureScale.Y)), level.SourceRectangle, Color.White, 0, Vector2.Zero, TextureScale, SpriteEffects.None, 0);
                    if (!level.hasSoldier) { _spriteBatch.Draw(_soldierPositionSprite, level.SoldierPosition, Color.White); }
                }
                else
                {
                    if(level.canBeBuilt && canBuildLevels) { _spriteBatch.Draw(_plusSignSprite, level.PlusSignPosition, Color.White); }
                }

            }
        }
    }
}
