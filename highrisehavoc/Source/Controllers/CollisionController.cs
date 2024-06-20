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
    public class CollisionController
    {
        // Declare a delegate for handling collision events
        public delegate void CollisionEventHandler(object sender, CollisionEventArgs e);

        public delegate void HitEventHandler(object sender, HitEventArgs e);

        // Declare an event using the delegate
        public event CollisionEventHandler CollisionEvent;

        public event HitEventHandler HitEvent;

        // EventArgs class for passing collision information
        public class CollisionEventArgs : EventArgs
        {
            public Projectile Projectile { get; set; }
            public Enemy Enemy { get; set; }
            public EnemyPlane EnemyPlane { get; set; }
            public Obstacle Obstacle { get; set; }
            public Soldier Soldier { get; set; }
            public Mine Mine { get; set; }
        }

        public class HitEventArgs : EventArgs
        {
            public Enemy Enemy { get; set; }

            public EnemyPlane EnemyPlane { get; set; }

            public Soldier Soldier { get; set; }

            public Obstacle Obstacle { get; set; }

            public Mine Mine { get; set; }
        }

        private List<Projectile> _enemyProjectiles;
        private List<Projectile> _soldierProjectiles;
        private List<SoldierController> _soldierControllers;
        private List<EnemyController> _enemyControllers;
        private List<EnemyPlaneController> _enemyPlaneControllers;
        private List<ObstacleController> _obstacleControllers;
        private List<MineController> _mineControllers;
        private Highrise _highrise;

        public CollisionController()
        {
            _enemyProjectiles = new List<Projectile>();
            _soldierProjectiles = new List<Projectile>();
            _soldierControllers = new List<SoldierController>();
            _enemyControllers = new List<EnemyController>();
            _enemyPlaneControllers = new List<EnemyPlaneController>();
            _obstacleControllers = new List<ObstacleController>();
            _mineControllers = new List<MineController>();
        }

        // Add a method to subscribe to the collision event
        public void SubscribeToCollisionEvent(CollisionEventHandler handler)
        {
            CollisionEvent += handler;
        }

        // Add a method to unsubscribe from the collision event
        public void UnsubscribeFromCollisionEvent(CollisionEventHandler handler)
        {
            CollisionEvent -= handler;
        }

        public void SubscribeToHitEvent(HitEventHandler handler)
        {
            HitEvent += handler;
        }

        public void UnsubscribeFromHitEvent(HitEventHandler handler)
        {
            HitEvent -= handler;
        }

        public void AddSoldierProjectile(Projectile projectile)
        {
            _soldierProjectiles.Add(projectile);
        }

        public void AddEnemyProjectile(Projectile projectile)
        {
            _enemyProjectiles.Add(projectile);
        }

        public void AddHighrise(Highrise highrise)
        {
            _highrise = highrise;
        }

        public void AddSoldierController(SoldierController soldierController)
        {
            _soldierControllers.Add(soldierController);
        }

        public void AddEnemyController(EnemyController enemyController)
        {
            _enemyControllers.Add(enemyController);
        }

        public void AddEnemyPlaneController(EnemyPlaneController enemyPlaneController)
        {
            _enemyPlaneControllers.Add(enemyPlaneController);
        }

        public void AddObstacleController(ObstacleController obstacleController)
        {
            _obstacleControllers.Add(obstacleController);
        }

        public void AddMineController(MineController mineController)
        {
            _mineControllers.Add(mineController);
        }

        public void RemoveMineController(MineController mineController)
        {
            _mineControllers.Remove(mineController);
        }

        public void RemoveEnemyPlaneController(EnemyPlaneController enemyPlaneController)
        {
            _enemyPlaneControllers.Remove(enemyPlaneController);
        }

        public void RemoveSoldierController(SoldierController soldierController)
        {
            _soldierControllers.Remove(soldierController);
        }

        public void RemoveEnemyController(EnemyController enemyController)
        {
            _enemyControllers.Remove(enemyController);
        }

        public void RemoveObstacleController(ObstacleController obstacleController)
        {
            _obstacleControllers.Remove(obstacleController);
        }

        public void ClearProjectiles()
        {
            _enemyProjectiles.Clear();
            _soldierProjectiles.Clear();
        }

        public void Update(GameTime gameTime)
        {   
            // go through all soldiers and enemies and delete any that are dead
            foreach (SoldierController soldierController in _soldierControllers.ToList())
            {
                if (soldierController.Soldier.IsDead)
                {
                    _soldierControllers.Remove(soldierController);
                }
            }

            int eCC = _enemyControllers.Count;
            for (int i = 0; i < eCC; i++)
            {
                if(_enemyControllers.Count == 0)
                {
                    break;
                }
                int mCC = _mineControllers.Count;
                for(int j = 0; j < mCC; j++)
                {
                    Mine mine = _mineControllers[j].Mine;
                    Enemy enemy = _enemyControllers[i].Enemy;

                    float mineX = mine.Position.X;
                    float mineY = mine.Position.Y;

                    float mineWidth = mine.SourceRectangle.Width;
                    float mineHeight = mine.SourceRectangle.Height;

                    float enemyX = enemy.BodySpritePosition.X;
                    float enemyY = enemy.BodySpritePosition.Y;

                    float enemyWidth = enemy.BodySourceRectangle.Width;
                    float enemyHeight = enemy.BodySourceRectangle.Height;

                    if (mineX > enemyX && mineX < enemyX + enemyWidth && mineY <= enemyY + enemyHeight && mineY > enemyY)
                    {
                        // Notify subscribers about the collision
                        OnHit(new HitEventArgs { Enemy = enemy, Mine = mine });
                        // OnCollision(new CollisionEventArgs { Mine = mine, Enemy = enemy });
                    }   
                }
            }

            foreach (EnemyPlaneController enemyPlaneController in _enemyPlaneControllers.ToList())
            {
                if (enemyPlaneController._enemyPlane.isDead)
                {
                    _enemyPlaneControllers.Remove(enemyPlaneController);
                }
            }
            
            foreach (ObstacleController obstacleController in _obstacleControllers.ToList())
            {
                if (!obstacleController.obstacle.IsSolid)
                {
                    _obstacleControllers.Remove(obstacleController);
                }
            }

            foreach (MineController mineController in _mineControllers.ToList())
            {
                if (mineController.Mine.IsExploded)
                {
                    _mineControllers.Remove(mineController);
                }
            }

            foreach(Projectile projectile in _soldierProjectiles.ToList())
            {
                // Console.WriteLine("Position of projectile is: " + projectile.SpritePosition.X + " " + projectile.SpritePosition.Y);
                if(projectile.SpritePosition.X > 3500 || projectile.SpritePosition.X < 0)
                {
                    // Console.WriteLine("Soldier Projectile out of bounds");
                    _soldierProjectiles.Remove(projectile);
                }
                else { 
                    // Console.WriteLine("Soldier Projectile in bounds");
                    // Console.WriteLine("Amount of enemy controllers: " + _enemyControllers.Count);
                    if(projectile.Target.EnemyPlane != null)
                    {
                        float projectileX = projectile.SpritePosition.X;
                        float projectileY = projectile.SpritePosition.Y;

                        float projectileWidth = projectile.SourceRectangle.Width;
                        float projectileHeight = projectile.SourceRectangle.Height;

                        float enemyX = projectile.Target.EnemyPlane.BodySpritePosition.X;
                        float enemyY = projectile.Target.EnemyPlane.BodySpritePosition.Y;

                        float enemyWidth = projectile.Target.EnemyPlane.BodySourceRectangle.Width;
                        float enemyHeight = projectile.Target.EnemyPlane.BodySourceRectangle.Height;


                        if (projectileX > enemyX && projectileX < enemyX + enemyWidth && projectileY > enemyY && projectileY < enemyY + enemyHeight - 120)
                        {
                            Console.WriteLine("Soldier rocket hit enemy plane!");
                            // Notify subscribers about the collision
                            OnHit(new HitEventArgs { EnemyPlane = projectile.Target.EnemyPlane });
                            OnCollision(new CollisionEventArgs { Projectile = projectile, EnemyPlane = projectile.Target.EnemyPlane });

                            // Remove the projectile
                            _soldierProjectiles.Remove(projectile);
                        }


                    } else foreach (Enemy enemy in _enemyControllers.Select(e => e.Enemy))
                    {
                        float projectileX = projectile.SpritePosition.X;
                        float projectileY = projectile.SpritePosition.Y;

                        float enemyX = enemy.BodySpritePosition.X;
                        float enemyY = enemy.BodySpritePosition.Y;

                        float enemyWidth = enemy.BodySourceRectangle.Width;
                        float enemyHeight = enemy.BodySourceRectangle.Height;

                        float enemyCenterX = enemyX + (enemyWidth / 2);


                        // Console.WriteLine("soldier projectile x: " + projectileX);
                        // Console.WriteLine("enemy x: " + enemyX);
                        // Console.WriteLine("enemy width: " + enemyWidth);
                        // Console.WriteLine("projectileX > enemyX: " + (projectileX > enemyX));

                        if (projectileX > enemyX && projectileX < enemyX + enemyWidth)
                        {
                            // Console.WriteLine("Soldier bullet hit enemy!");
                            // Notify subscribers about the collision
                            OnHit(new HitEventArgs { Enemy = enemy });
                            OnCollision(new CollisionEventArgs { Projectile = projectile, Enemy = enemy });

                            // Remove the projectile
                            _soldierProjectiles.Remove(projectile);
                        }
                    }
                }

            }

            // check if enemy projectiles hit the soldier
            foreach(Projectile projectile in _enemyProjectiles.ToList())
            {
                if(projectile.SpritePosition.X < 0 || projectile.SpritePosition.Y < 0)
                {
                    // Console.WriteLine("Enemy Projectile out of bounds");
                    _enemyProjectiles.Remove(projectile);
                }
                else
                {
                    // Console.WriteLine("Enemy Projectile in bounds");
                    // Console.WriteLine("Amount of soldier controllers: " + _soldierControllers.Count);
                    foreach(SoldierController soldierController in _soldierControllers)
                    {
                        Soldier soldier = soldierController.Soldier;

                        float projectileX = projectile.SpritePosition.X;
                        float projectileY = projectile.SpritePosition.Y;

                        float soldierX = soldier.BodySpritePosition.X;
                        float soldierY = soldier.BodySpritePosition.Y;

                        float soldierWidth = soldier.BodySourceRectangle.Width;
                        float soldierHeight = soldier.BodySourceRectangle.Height;

                        // Console.WriteLine("enemy projectile x: " + projectileX);
                        // Console.WriteLine("soldier x: " + soldierX);
                        // Console.WriteLine("soldier width: " + soldierWidth);
                        // Console.WriteLine("projectileX > enemyX: " + (projectileX > soldierX));

                        if (projectileX > soldierX && projectileX < soldierX + soldierWidth/2.4)
                        {
                            // Console.WriteLine("Enemy bullet hit soldier!");
                            // Notify subscribers about the collision
                            OnHit(new HitEventArgs { Soldier = soldier });
                            OnCollision(new CollisionEventArgs { Projectile = projectile, Soldier = soldier });

                            // Remove the projectile
                            _enemyProjectiles.Remove(projectile);
                        }
                    }

                    foreach (ObstacleController obstacleController in _obstacleControllers)
                    {
                        Obstacle obstacle = obstacleController.obstacle;

                        float projectileX = projectile.SpritePosition.X;
                        float projectileY = projectile.SpritePosition.Y;

                        float obstacleX = obstacle.Position.X;
                        float obstacleY = obstacle.Position.Y;

                        float obstacleWidth = obstacle.SourceRectangle.Width;
                        float obstacleHeight = obstacle.SourceRectangle.Height;

                        // Console.WriteLine("enemy projectile x: " + projectileX);
                        // Console.WriteLine("soldier x: " + soldierX);
                        // Console.WriteLine("soldier width: " + soldierWidth);
                        // Console.WriteLine("projectileX > enemyX: " + (projectileX > soldierX));

                        if (projectileX > obstacleX && projectileX < obstacleX + obstacleWidth)
                        {
                            Console.WriteLine("Enemy bullet hit Obstacle!");
                            // Notify subscribers about the collision
                            OnCollision(new CollisionEventArgs { Projectile = projectile, Obstacle = obstacle });

                            // Remove the projectile
                            _enemyProjectiles.Remove(projectile);
                        }
                    }
                }
            }

            // Check if any enemy projectiles hit the highrise
            foreach (Projectile projectile in _enemyProjectiles.ToList()) // Use ToList to avoid modification during iteration
            {

                if (projectile.SpritePosition.X < 0 || projectile.SpritePosition.Y < 0)
                {
                    // Console.WriteLine("Projectile out of bounds");
                    _enemyProjectiles.Remove(projectile);
                }
                else if ((int)(_highrise.SpritePosition.X + (_highrise.SourceRectangle.Width * 0.65)) > projectile.SpritePosition.X)
                {
                    Console.WriteLine("Collision detected with highrise!!!");
                    // Notify subscribers about the collision
                    OnCollision(new CollisionEventArgs { Projectile = projectile, Enemy = null, Soldier=null, Obstacle=null, EnemyPlane= null, Mine = null });

                    // Remove the projectile
                    _enemyProjectiles.Remove(projectile);
                }
            }
        }

        // Helper method to invoke the collision event
        protected virtual void OnCollision(CollisionEventArgs e)
        {
            CollisionEvent?.Invoke(this, e);
        }

        // on hit event

        protected virtual void OnHit(HitEventArgs e)
        {
            HitEvent?.Invoke(this, e);
        }

        public CollisionController ReturnCopy(Highrise copiedHighrise)
        {
            CollisionController copy = new CollisionController();
            copy.AddHighrise(copiedHighrise);
            return copy;
        }
    }
}

