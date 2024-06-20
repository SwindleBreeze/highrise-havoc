using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace highrisehavoc
{
    public class HighriseAttachmentRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _sprite;
        private readonly Texture2D _plusSignSprite;
        public Vector2 TextureScale = new Vector2(0.60f, 0.60f);
    
        public HighriseAttachmentRenderer(SpriteBatch spriteBatch, Texture2D sprite, Texture2D plusSignSprite, Vector2 textureScale)
        {
            _spriteBatch = spriteBatch;
            _sprite = sprite;
            _plusSignSprite = plusSignSprite;
            TextureScale = textureScale;
        }

        public void Draw(List<HighriseAttachment> attachments, bool canBuildAttachments)
        {
            foreach (var attachment in attachments)
            {
                if(attachment.isBuilt)
                {
                    _spriteBatch.Draw(_sprite, attachment.SpritePosition, attachment.SourceRectangle, Color.White, 0, Vector2.Zero, TextureScale, SpriteEffects.None, 0);
                }
                else
                {
                    if (canBuildAttachments && attachment.canBeBuilt) _spriteBatch.Draw(_plusSignSprite, attachment.PlusSignPosition, Color.White);
                }
                
            }
        }
    }
}