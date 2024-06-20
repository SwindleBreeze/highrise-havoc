using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace highrisehavoc.Source.Controllers
{
    public class EnemyController
    {
        public Enemy Enemy { get; set; }
        public EnemyRenderer EnemyRenderer { get; set; }
        private Highrise Highrise { get; set; }
        public List<ProjectileController> Projectiles { get; set; }
        private CollisionController CollisionController { get; set; }

        private SoundController _soundController;
        private Vector2 TextureScale;

        public EnemyController(Enemy enemy, EnemyRenderer enemyRenderer, Highrise highrise, CollisionController collisionController, SoundController soundController, Vector2 textureScale)
        {
            Enemy = enemy;
            EnemyRenderer = enemyRenderer;
            Highrise = highrise;
            CollisionController = collisionController;
            TextureScale = textureScale;

            Projectiles = new List<ProjectileController>();

            this._soundController = soundController;

            collisionController.AddEnemyController(this);

            // Subscribe to the collision event
            CollisionController.SubscribeToCollisionEvent(onHighRiseCollision);
            CollisionController.SubscribeToCollisionEvent(onSoldierCollision);
            CollisionController.SubscribeToCollisionEvent(onObstacleCollision);
            CollisionController.SubscribeToHitEvent(onHit);
        }

        private void onHighRiseCollision(object sender, CollisionController.CollisionEventArgs e)
        {
            // Console.WriteLine("on Highrise Collision");
            if(e.Projectile != null && e.Enemy == null && e.Soldier == null && e.EnemyPlane == null && e.Obstacle == null)
            {
                Highrise.HitPoints -= e.Projectile.Damage;
                Projectiles.Remove(Projectiles.Find(p => p._projectile == e.Projectile));
                // Console.WriteLine("Enemy Projectile Hit Highrise");
            }
        }

        private void onEnemyCollision(object sender, CollisionController.CollisionEventArgs e)
        {
            // Console.WriteLine("on Enemy Collision");
            if(e.Projectile != null )
            {
                // Console.WriteLine("Enemy Hit");
                Enemy.HitPoints -= e.Projectile.Damage;
                // remove projectile from list
                Projectiles.Remove(Projectiles.Find(p => p._projectile == e.Projectile));
            }
        }

        private void onObstacleCollision(object sender, CollisionController.CollisionEventArgs e)
        {
            // Console.WriteLine("on Obstacle Collision");
            if(e.Projectile != null && e.Obstacle != null)
            {
                // remove projectile from list
                e.Obstacle.Hitpoint -= e.Projectile.Damage;
                Projectiles.Remove(Projectiles.Find(p => p._projectile == e.Projectile));
            }
        }


        private void onHit(object sender, CollisionController.HitEventArgs e)
        {
            // Console.WriteLine("on Hit");
            if(e.Enemy != null && e.Enemy == Enemy) Enemy.HitPoints -= 1;
            if(e.Mine != null && e.Enemy != null && e.Enemy == Enemy) Enemy.HitPoints -= 6;
        }

        private void onSoldierCollision(object sender, CollisionController.CollisionEventArgs e)
        {
            // Console.WriteLine("on Soldier Collision");
            if(e.Projectile != null && e.Soldier != null)
            {
                // Console.WriteLine("Soldier Hit");
                // remove projectile from list
                Projectiles.Remove(Projectiles.Find(p => p._projectile == e.Projectile));
            }
        }

        private void spawnProjectile()
        {
            Projectile projectile = new(Enemy.BodySpritePosition, new Vector2(-3, 0), 1, new Rectangle(285,10,20,20), new SoldierTarget(Vector2.Zero, null, null));
            ProjectileRenderer projectileRenderer = new(EnemyRenderer.GetSpriteBatch(), EnemyRenderer.GetSpriteSheet());

            ProjectileController projectileController = new(projectile, projectileRenderer, CollisionController);

            // rand number between 0 and 1
            Random rand = new();

            int randNum = rand.Next(0, 2);

            if(randNum == 0)
            {
                _soundController.playSoundEffectLasershot();
            }
            else
            {
                _soundController.playSoundEffectLasershot2();
            }


            CollisionController.AddEnemyProjectile(projectile);
            Projectiles.Add(projectileController);
        }

        public void Update(GameTime gameTime)
        {
        
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float movement = Enemy.Speed * elapsed;

            if (Enemy.HitPoints <= 0)
            {
                // Enemy is dead
                // Console.WriteLine("Enemy is dead");
                
                Enemy.IsAttacking = false;
                Enemy.IsMoving = false;
                
                Enemy.IsDead = true;

                CollisionController.RemoveEnemyController(this);

                for (int i = 0; i < Projectiles.Count; i++)
                {
                    Projectiles.RemoveAt(i);
                }
            }

            if(Enemy.IsDead)
            {
                return;
            }

            // if there is anything within the attack range, stop moving

            // if the enemy is attacking, spawn a projectile every second
            if(Enemy.IsAttacking)
            {
                Enemy.AttackTimer += elapsed;
                if(Enemy.AttackTimer > Enemy.AttackSpeed)
                {
                    spawnProjectile();
                    Enemy.AttackTimer = 0;
                }
            }

            for(int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Update(gameTime);
            }

            if (Enemy.IsMoving)
            {
                Enemy.BodySpritePosition.X -= movement;
                Enemy.BodySpritePosition1.X -= movement;
                Enemy.BodySpritePosition2.X -= movement;
                Enemy.HeadSpritePosition.X -= movement;
                Enemy.ArmsSpritePosition.X -= movement;
            }

        }

        public void Draw()
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Draw(0);
            }

            if (Enemy.IsDead)
            {
                return;
            }

            EnemyRenderer.Draw(Enemy);


        }
    }
}
