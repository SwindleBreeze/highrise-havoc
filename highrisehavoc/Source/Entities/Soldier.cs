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
    public class SoldierTarget
    {
        public Vector2 Position { get; set; }
        public Enemy Enemy { get; set; }

        public EnemyPlane EnemyPlane { get; set; }

        public SoldierTarget(Vector2 position, Enemy enemyTarget, EnemyPlane enemyPlaneTarget)
        {
            Position = position;
            Enemy = enemyTarget;
            EnemyPlane = enemyPlaneTarget;
        }

        public void UpdatePosition()
        {
            if(EnemyPlane != null && EnemyPlane.isDead)
            {
                EnemyPlane = null;
            }
            if(Enemy != null &&  Enemy.IsDead)
            {
                Enemy = null;
            }

            if (Enemy != null)
            {
                Position = Enemy.BodySpritePosition;
            }
            else if (EnemyPlane != null)
            {
                Position = EnemyPlane.BodySpritePosition;
            }
        }
    }
    public class Soldier
    {
        public Rectangle BodySourceRectangle { get; set; }
        public Vector2 BodySpritePosition = new(0, 0);
        public Rectangle HeadSourceRectangle { get; set; }
        public Vector2 HeadSpritePosition = new(0, 0);
        public Rectangle ArmsSourceRectangle { get; set; }
        public Vector2 ArmsSpritePosition = new(0, 0);

        public Rectangle ProjectileSourceRectangle { get; set; }

        public int HitPoints { get; set; }
        public int Damage { get; set; }
        public float AttackSpeed { get; set; }
        public int Speed { get; set; }
        public bool IsAttacking { get; set; }
        public float AttackTimer { get; set; }
        public float AttackRange { get; set; }
        public bool IsDead { get; set; }
        public int Cost { get; set; }

        public SoldierTarget Target { get; set; }

        public Soldier(Rectangle bodySourceRectangle, Vector2 bodySpritePosition, Rectangle headSourceRectangle, Vector2 headSpritePosition, Rectangle armsSourceRectangle, Vector2 armsSpritePosition, int hitPoints, int damage, int speed, int attackRange, int cost, Rectangle projectileSourceRectangle)
        {
            BodySourceRectangle = bodySourceRectangle;
            BodySpritePosition = bodySpritePosition;
            HeadSourceRectangle = headSourceRectangle;
            HeadSpritePosition = headSpritePosition;
            ArmsSourceRectangle = armsSourceRectangle;
            ArmsSpritePosition = armsSpritePosition;
            HitPoints = hitPoints;
            Damage = damage;
            Speed = speed;
            AttackSpeed = 2.8f;
            IsAttacking = false;
            AttackRange = attackRange;
            IsDead = false;
            Cost = cost;
            ProjectileSourceRectangle = projectileSourceRectangle;
            Target = new SoldierTarget(Vector2.Zero, null, null);
        }

        public Soldier ReturnCopy()
        {
            Soldier copy = new Soldier(BodySourceRectangle, BodySpritePosition, HeadSourceRectangle, HeadSpritePosition, ArmsSourceRectangle, ArmsSpritePosition, HitPoints, Damage, Speed, (int)AttackRange, Cost, ProjectileSourceRectangle);
            copy.AttackSpeed = AttackSpeed;
            copy.IsAttacking = IsAttacking;
            copy.AttackTimer = AttackTimer;
            copy.IsDead = IsDead;
            return copy;
        }

        public void AddTargetPosition(Vector2 CurrentPosition, Enemy enemy, EnemyPlane enemyPlane)
        {
            Target = new SoldierTarget(CurrentPosition, enemy, enemyPlane);
        }

    }

    public class AASoldier : Soldier
    {
        public Rectangle BackArmsSourceRectangle { get; set; }
        public Vector2 BackArmsSpritePosition = new(0, 0);
        public Rectangle WeaponSourceRectangle { get; set; }
        public Vector2 WeaponSpritePosition = new(0, 0);
        
        public AASoldier(Rectangle bodySourceRectangle, Vector2 bodySpritePosition, Rectangle headSourceRectangle, Vector2 headSpritePosition, Rectangle armsSourceRectangle, Vector2 armsSpritePosition, Rectangle backArmsSourceRectangle, Vector2 backArmsSpritePosition, Rectangle weaponSourceRectangle, Vector2 weaponSpritePosition, int hitPoints, int damage, int speed, int attackRange, int cost, Rectangle projectileSourceRectangle) : base(bodySourceRectangle, bodySpritePosition, headSourceRectangle, headSpritePosition, armsSourceRectangle, armsSpritePosition, hitPoints, damage, speed, attackRange, cost, projectileSourceRectangle)
        {
            BackArmsSourceRectangle = backArmsSourceRectangle;
            BackArmsSpritePosition = backArmsSpritePosition;
            WeaponSourceRectangle = weaponSourceRectangle;
            WeaponSpritePosition = weaponSpritePosition;
        }

        public new AASoldier ReturnCopy()
        {
            AASoldier copy = new AASoldier(BodySourceRectangle, BodySpritePosition, HeadSourceRectangle, HeadSpritePosition, ArmsSourceRectangle, ArmsSpritePosition, BackArmsSourceRectangle, BackArmsSpritePosition, WeaponSourceRectangle, WeaponSpritePosition, HitPoints, Damage, Speed, (int)AttackRange, Cost, ProjectileSourceRectangle);
            copy.AttackSpeed = AttackSpeed;
            copy.IsAttacking = IsAttacking;
            copy.AttackTimer = AttackTimer;
            copy.IsDead = IsDead;
            return copy;
        }
    }
}
