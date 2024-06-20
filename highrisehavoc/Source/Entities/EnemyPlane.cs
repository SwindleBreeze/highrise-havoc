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
    public class EnemyPlane
    {
        public Rectangle BodySourceRectangle { get; set; }
        public Vector2 BodySpritePosition = new Vector2(0, 0);

        public bool isDead = false;
        public int HitPoints { get; set; }
        public int Damage { get; set; }
        public int Speed { get; set; }

        public EnemyPlane(Rectangle bodySourceRectangle, Vector2 bodySpritePosition, int hitPoints, int damage, int speed)
        {
            BodySourceRectangle = bodySourceRectangle;
            BodySpritePosition = bodySpritePosition;
            HitPoints = hitPoints;
            Damage = damage;
            Speed = speed;
        }
    }
}
