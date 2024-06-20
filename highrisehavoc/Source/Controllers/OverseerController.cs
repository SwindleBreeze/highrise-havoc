using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using highrisehavoc.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using highrisehavoc.Source.Renderers;
using static highrisehavoc.Source.Controllers.CollisionController;


namespace highrisehavoc.Source.Controllers
{
    public class OverseerController
    {
        public List<EnemyController> enemyControllers;
        public List<EnemyPlaneController> planeControllers;
        public List<SoldierController> soldierControllers;

        public Highrise highrise;
        public CollisionController collisionController;
        public DistanceCheckController distanceCheckController;

        public float SpawnTimer = 4;

        public delegate void SpawnEnemyEventHandler(object sender, SpawnEnemyEventArgs e);

        public event SpawnEnemyEventHandler SpawnEnemyEvent;

        public class SpawnEnemyEventArgs : EventArgs
        {
            public bool SpawnEnemy { get; set; }

            public string Type { get; set; }
        }

        public OverseerController(Highrise highrise, CollisionController collisionController, DistanceCheckController distanceCheckController)
        {
            enemyControllers = new List<EnemyController>();
            soldierControllers = new List<SoldierController>();
            planeControllers = new List<EnemyPlaneController>();
            this.highrise = highrise;
            this.collisionController = collisionController;
            this.distanceCheckController = distanceCheckController;
        }

        public void addEnemyController(EnemyController enemyController)
        {
            enemyControllers.Add(enemyController);
        }

        public void addSoldierController(SoldierController soldierController)
        {
            soldierControllers.Add(soldierController);
        }

        public void removeEnemyController(EnemyController enemyController)
        {
            enemyControllers.Remove(enemyController);
        }

        public void removeSoldierController(SoldierController soldierController)
        {
            soldierControllers.Remove(soldierController);
        }

        public void addEnemyPlaneController(EnemyPlaneController enemyPlaneController)
        {
            planeControllers.Add(enemyPlaneController);
        }

        public void subscribeToSpawnEnemyEvent(SpawnEnemyEventHandler handler)
        {
            SpawnEnemyEvent += handler;
        }

        public void unsubscribeFromSpawnEnemyEvent(SpawnEnemyEventHandler handler)
        {
            SpawnEnemyEvent -= handler;
        }

        protected virtual void Spawn(SpawnEnemyEventArgs e)
        {
            SpawnEnemyEvent?.Invoke(this, e);
        }

        public void Update(GameTime gameTime)
        {
            SpawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(SpawnTimer <= 0)
            {
                SpawnTimer = 4.5f;
                // create random function from 0 to 50
                Random rand = new Random();
                int randNum = rand.Next(0, 50);
                if(randNum > 5 && randNum < 45)
                {
                    Spawn(new SpawnEnemyEventArgs { SpawnEnemy = true, Type = "Soldier" });
                }
                else
                {
                    Spawn(new SpawnEnemyEventArgs { SpawnEnemy = true, Type = "Plane" });
                }

            }
        }
    }
}
