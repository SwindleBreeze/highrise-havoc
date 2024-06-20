using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using highrisehavoc.Source.Entities;

namespace highrisehavoc.Source.Entities
{
    public class HighriseLevel
    {
        public Vector2 PlusSignPosition { get; set; }
        public Rectangle PlusSignSourceRectangle { get; set; }
        public Vector2 SpritePosition { get; set; }
        public Rectangle SourceRectangle { get; set; }

        private Vector2 TextureScale = new Vector2(1.0f, 1.0f);

        public HighriseAttachment BalconyAttachment { get; set; }

        public int HitPoints { get; set; }

        public bool IsBuilt { get; set; }

        public bool canBeBuilt { get; set; }

        public bool canBeTapped { get; set; }

        public bool hasSoldier { get; set; }

        public Vector2 SoldierPosition { get; set; }

        public HighriseLevel(Vector2 spritePosition, Vector2 plusSignPosition, Vector2 textureScale)
        {
            SpritePosition = spritePosition;
            PlusSignPosition = plusSignPosition;
            SourceRectangle = new Rectangle(2463, 0, 518, 150);
            SoldierPosition = new Vector2(spritePosition.X + SourceRectangle.Width * textureScale.X / 2.5f, spritePosition.Y );
            PlusSignSourceRectangle = new Rectangle(0, 0, 33, 33);
            BalconyAttachment = new HighriseAttachment(new Vector2(SpritePosition.X + 50, SpritePosition.Y), new Vector2(PlusSignPosition.X + 75, PlusSignPosition.Y + 10), false, textureScale);
            HitPoints = 25;
            IsBuilt = false;
            canBeBuilt = false;
            canBeTapped = true;
            hasSoldier = false;
            TextureScale = textureScale;
        }

        public HighriseLevel ReturnCopy()
        {
            HighriseLevel copy = new HighriseLevel(SpritePosition, PlusSignPosition, TextureScale);
            copy.SourceRectangle = SourceRectangle;
            copy.PlusSignSourceRectangle = PlusSignSourceRectangle;
            copy.BalconyAttachment = BalconyAttachment.ReturnCopy();
            copy.HitPoints = HitPoints;
            copy.IsBuilt = IsBuilt;
            copy.canBeBuilt = canBeBuilt;
            copy.canBeTapped = canBeTapped;
            copy.hasSoldier = hasSoldier;
            copy.SoldierPosition = SoldierPosition;
            return copy;
        }
    }
}
