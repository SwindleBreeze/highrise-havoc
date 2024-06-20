using highrisehavoc.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace highrisehavoc.Source.Renderers
{
    public class SoldierSpawnPointRenderer
    {
        public SpriteBatch _spriteBatch;
        public Texture2D spawnPointTexture;

        public SoldierSpawnPointRenderer(SpriteBatch spriteBatch, Texture2D spawnPointTexture)
        {
            _spriteBatch = spriteBatch;
            this.spawnPointTexture = spawnPointTexture;
        }

        public void Draw(SoldierSpawnPoint spawnPoint)
        {
            _spriteBatch.Draw(spawnPointTexture, new Vector2(spawnPoint.Position.X, spawnPoint.Position.Y), new Rectangle(0, 0, 33, 33), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);
        }
    }
}
