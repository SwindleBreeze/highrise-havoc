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
    public class Obstacle
    {
        public Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public int Hitpoint { get; set; }
        public bool IsSolid { get; set; }

        public Obstacle(Vector2 position, Rectangle hitbox, bool isSolid, int hitpoint)
        {
            Position = position;
            SourceRectangle = hitbox;
            IsSolid = isSolid;
            Hitpoint = hitpoint;
        }

        public Obstacle ReturnCopy()
        {
            Obstacle copyObstacle = new Obstacle(Position, SourceRectangle, IsSolid, Hitpoint);
            return copyObstacle;
        }
    }
}
