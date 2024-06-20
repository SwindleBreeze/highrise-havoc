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
    public class ProjectileRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spriteSheet;

        public ProjectileRenderer(SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            _spriteBatch = spriteBatch;
            _spriteSheet = spriteSheet;
        }

        public void Draw(Projectile projectile, float angleOfBullet)
        {
            _spriteBatch.Draw(_spriteSheet, projectile.SpritePosition, projectile.SourceRectangle, Color.White, angleOfBullet, new Vector2(0, 0), 0.35f, SpriteEffects.None, 1);
        }
    }
}
