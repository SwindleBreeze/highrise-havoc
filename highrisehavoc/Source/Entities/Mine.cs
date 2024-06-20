using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using highrisehavoc.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace highrisehavoc.Source.Entities
{
    public class Mine
    {
        public Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public bool IsExploded { get; set; }

        public int Damage { get; set; }
        public Mine(Vector2 position, Rectangle hitbox, bool IsExploded)
        {
            Position = position;
            SourceRectangle = hitbox;
            IsExploded = false;
            Damage = 6;
        }

        public Mine ReturnCopy()
        {
            Mine copyMine = new Mine(Position, SourceRectangle, IsExploded);
            return copyMine;
        }

    }
}
