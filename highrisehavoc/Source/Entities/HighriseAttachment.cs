using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace highrisehavoc
{
    public class HighriseAttachment
    {
        public Vector2 PlusSignPosition { get; set; }
        public Rectangle PlusSignSourceRectangle { get; set; }
        public Vector2 SpritePosition { get; set; }
        public Rectangle SourceRectangle { get; set; }

        private Vector2 textureScale;

        public int HitPoints { get; set; }

        public bool isBuilt { get; set; }

        public bool canBeBuilt { get; set; }

        public bool canBeTapped { get; set; }

        public bool hasSoldier { get; set; }

        public Vector2 SoldierPosition { get; set; }

        public HighriseAttachment(Vector2 spritePosition, Vector2 plusSignPosition, bool canBeBuilt, Vector2 textureScale)
        {
            SpritePosition = spritePosition;
            //Soldier position is in middle of sprite position
            SoldierPosition = new Vector2(spritePosition.X + 150 * textureScale.X, spritePosition.Y + 32 * textureScale.Y);
            PlusSignPosition = plusSignPosition;
            SourceRectangle = new Rectangle(2274, 0, 162, 103);
            PlusSignSourceRectangle = new Rectangle(0, 0, 33, 33);
            HitPoints = 25;
            isBuilt = false;
            this.canBeBuilt = canBeBuilt;
            canBeTapped = true;
            hasSoldier = false;
            this.textureScale = textureScale;
        }

        public HighriseAttachment ReturnCopy()
        {
            HighriseAttachment copy = new HighriseAttachment(SpritePosition, PlusSignPosition, canBeBuilt, textureScale);
            copy.HitPoints = HitPoints;
            copy.isBuilt = isBuilt;
            copy.canBeBuilt = canBeBuilt;
            copy.canBeTapped = canBeTapped;
            copy.hasSoldier = hasSoldier;
            return copy;
        }
    }
}