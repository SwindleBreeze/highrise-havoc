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
    public class Enemy
    {
        public Rectangle BodySourceRectangle { get; set; }
        public Vector2 BodySpritePosition = new Vector2(0, 0);

        public Rectangle BodySourceRectangle1 { get; set; }
        public Vector2 BodySpritePosition1 = new Vector2(0, 0);

        public Rectangle BodySourceRectangle2 { get; set; }
        public Vector2 BodySpritePosition2 = new Vector2(0, 0);

        public Rectangle HeadSourceRectangle { get; set; }
        public Vector2 HeadSpritePosition = new Vector2(0, 0);
        public Rectangle ArmsSourceRectangle { get; set; }
        public Vector2 ArmsSpritePosition = new Vector2(0, 0);
        public int HitPoints { get; set; }
        public int Damage { get; set; }
        public float AttackSpeed { get; set; }
        public int Speed { get; set; }
        public bool IsMoving { get; set; }
        public bool IsAttacking { get; set; }
        public float AttackTimer { get; set; }
        public float AttackRange { get; set; }

        public bool IsDead { get; set; }


        public Enemy(Rectangle bodySourceRectangle, Vector2 bodySpritePosition, Rectangle bodySourceRectangle1, Vector2 bodySpritePosition1, Rectangle bodySourceRectangle2, Vector2 bodySpritePosition2, Rectangle headSourceRectangle, Vector2 headSpritePosition, Rectangle armsSourceRectangle, Vector2 armsSpritePosition, int hitPoints, int damage, int speed, int attackRange)
        {
            BodySourceRectangle = bodySourceRectangle;
            BodySpritePosition = bodySpritePosition;
            BodySourceRectangle1 = bodySourceRectangle1;
            BodySpritePosition1 = bodySpritePosition1;
            BodySourceRectangle2 = bodySourceRectangle2;
            BodySpritePosition2 = bodySpritePosition2;
            HeadSourceRectangle = headSourceRectangle;
            HeadSpritePosition = headSpritePosition;
            ArmsSourceRectangle = armsSourceRectangle;
            ArmsSpritePosition = armsSpritePosition;
            HitPoints = hitPoints;
            Damage = damage;
            Speed = speed;
            AttackSpeed = 8f;
            IsMoving = true;
            IsAttacking = false;
            AttackRange = attackRange;
            IsDead = false;

        }

    }
}
