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
    public class SoldierRenderer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _spriteSheet;
        public bool isBeingDrawn = false;
        public Vector2 TextureScale = new Vector2(0.35f, 0.35f);

        public SoldierRenderer(SpriteBatch spriteBatch, Texture2D spriteSheet, Vector2 textureScale)
        {
            _spriteBatch = spriteBatch;
            _spriteSheet = spriteSheet;
            TextureScale = new Vector2(textureScale.X*0.75f, textureScale.Y * 0.75f);
        }

        public void Draw(Soldier soldier)
        {
            // if sprite is outside of screen, don't draw
            if (soldier.BodySpritePosition.X < 0 || soldier.BodySpritePosition.X > 1300)
            {
                isBeingDrawn = false;
                return;
            }
            isBeingDrawn = true;

            float angle;
            if (soldier is AASoldier aaSoldier)
            {
                // Console.WriteLine("This is the soldiers target: " + aaSoldier.Target);
                // calculate angle between soldier and soldier's target, and rotate soldier's arms and weapon accordingly
                if (aaSoldier.Target.Position.X != 0 && aaSoldier.Target.Position.Y != 0 && (aaSoldier.Target.Enemy != null || aaSoldier.Target.EnemyPlane != null))
                {
                    angle = (float)Math.Atan2(aaSoldier.Target.Position.Y - aaSoldier.BodySpritePosition.Y, aaSoldier.Target.Position.X - aaSoldier.BodySpritePosition.X);
                }
                else angle = 0;

                // Console.WriteLine(angle);
                _spriteBatch.Draw(_spriteSheet, aaSoldier.BackArmsSpritePosition, aaSoldier.BackArmsSourceRectangle, Color.White, angle, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
                _spriteBatch.Draw(_spriteSheet, aaSoldier.WeaponSpritePosition, aaSoldier.WeaponSourceRectangle, Color.White, angle, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
                _spriteBatch.Draw(_spriteSheet, soldier.BodySpritePosition, soldier.BodySourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
                _spriteBatch.Draw(_spriteSheet, soldier.HeadSpritePosition, soldier.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
                _spriteBatch.Draw(_spriteSheet, aaSoldier.ArmsSpritePosition, aaSoldier.ArmsSourceRectangle, Color.White, angle, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
            }
            else
            {
                _spriteBatch.Draw(_spriteSheet, soldier.BodySpritePosition, soldier.BodySourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
                _spriteBatch.Draw(_spriteSheet, soldier.ArmsSpritePosition, soldier.ArmsSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
                _spriteBatch.Draw(_spriteSheet, soldier.HeadSpritePosition, soldier.HeadSourceRectangle, Color.White, 0, new Vector2(0, 0), TextureScale, SpriteEffects.None, 1);
            }

        }

        public SpriteBatch GetSpriteBatch()
        {
            return _spriteBatch;
        }

        public Texture2D GetSpriteSheet()
        {
            return _spriteSheet;
        }

        public SoldierRenderer ReturnCopy()
        {
            return new SoldierRenderer(_spriteBatch, _spriteSheet, new Vector2(TextureScale.X / 0.6f, TextureScale.Y / 0.6f));
        }
    }
}
