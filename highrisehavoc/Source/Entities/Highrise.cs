using System.Collections.Generic;
using highrisehavoc.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace highrisehavoc
{
    public class Highrise
    {
        public Rectangle SourceRectangle { get; set; }
        public Vector2 SpritePosition { get; set; }
        public int HitPoints { get; set; }

        public List<HighriseAttachment> AttachmentPoints { get; set; }

        public List<HighriseLevel> Levels { get; set; }

        public Highrise(Rectangle sourceRectangle, Vector2 spritePosition)
        {
            SourceRectangle = sourceRectangle;
            SpritePosition = spritePosition;
            HitPoints = 6;
            AttachmentPoints = new List<HighriseAttachment>();
            Levels = new List<HighriseLevel>();
        }

        public Highrise ReturnCopy()
        {
            Highrise copy = new Highrise(SourceRectangle, SpritePosition);
            copy.HitPoints = HitPoints;
            copy.AttachmentPoints = new List<HighriseAttachment>();
            foreach (HighriseAttachment attachment in AttachmentPoints)
            {
                copy.AttachmentPoints.Add(attachment.ReturnCopy());
            }
            copy.Levels = new List<HighriseLevel>();
            foreach (HighriseLevel level in Levels)
            {
                copy.Levels.Add(level.ReturnCopy());
            }

            return copy;
        }
        

    }
}