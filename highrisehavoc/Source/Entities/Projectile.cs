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
    public class Projectile
    {
        public Vector2 SpritePosition = new Vector2(0, 0);
        public Rectangle SourceRectangle { get; set; }
        public Vector2 Velocity { get; set; }

        public float prevAngle = 0;
        public SoldierTarget Target { get; set; }

        public int Damage { get; set; }

        public int Direction { get; set; }
        public Projectile(Vector2 spritePosition, Vector2 velocity, int direction, Rectangle sourceRectangle, SoldierTarget target)
        {
            SpritePosition = spritePosition;
            SourceRectangle = sourceRectangle;
            Velocity = velocity;
            Damage = 1;
            Direction = direction;
            if(target != null)
            {
                Target = new SoldierTarget(target.Position, target.Enemy, target.EnemyPlane);
            }
        }
    }
}
