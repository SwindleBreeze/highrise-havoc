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
    public class DistanceCheckController
    {
        public Highrise _highrise;
        public List<EnemyController> _enemyControllers;
        public List<EnemyPlaneController> _enemyPlaneControllers;
        public List<SoldierController> _soldierControllers;
        public List<ObstacleController> _obstacleControllers;
        
        public DistanceCheckController(Highrise highrise)
        {
            _highrise = highrise;
            _enemyControllers = new List<EnemyController>();
            _soldierControllers = new List<SoldierController>();
            _enemyPlaneControllers = new List<EnemyPlaneController>();
            _obstacleControllers = new List<ObstacleController>();
        }

        public void addEnemyController(EnemyController enemyController)
        {
            _enemyControllers.Add(enemyController);
        }

        public void addSoldierController(SoldierController soldierController)
        {
            _soldierControllers.Add(soldierController);
        }

        public void addEnemyPlaneController(EnemyPlaneController enemyPlaneController)
        {
            _enemyPlaneControllers.Add(enemyPlaneController);
        }

        public void addObstacleController(ObstacleController obstacleController)
        {
            _obstacleControllers.Add(obstacleController);
        }

        public void removeEnemyController(EnemyController enemyController)
        {
            _enemyControllers.Remove(enemyController);
        }

        public void removeSoldierController(SoldierController soldierController)
        {
            _soldierControllers.Remove(soldierController);
        }

        public void removeEnemyPlaneController(EnemyPlaneController enemyPlaneController)
        {
            _enemyPlaneControllers.Remove(enemyPlaneController);
        }

        public void removeObstacleController(ObstacleController obstacleController)
        {
            _obstacleControllers.Remove(obstacleController);
        }

        public void Update(GameTime gameTime)
        {

            // Console.WriteLine("Amount of Enemies: " + _enemyControllers.Count);
            // Console.WriteLine("Amount of Soldiers: " + _soldierControllers.Count);
            // for each enemy check if within range of highrise or soldier
            foreach (EnemyController enemyController in _enemyControllers)
            {
                bool isInRange = false;

                foreach (SoldierController soldierController in _soldierControllers)
                {

                    bool isSoldierInRange = false;
                    // check if enemy is within range of soldier
                    float soldierDistance = (enemyController.Enemy.BodySpritePosition.X - enemyController.Enemy.BodySourceRectangle.Width) - (soldierController.Soldier.BodySpritePosition.X + soldierController.Soldier.BodySourceRectangle.Width);


                    if (soldierDistance <= enemyController.Enemy.AttackRange)
                    {
                        // Console.WriteLine("Soldier is in range of Enemy");
                        enemyController.Enemy.IsAttacking = true;
                        enemyController.Enemy.IsMoving = false;
                        isInRange = true;
                        break;
                    }
                    else if(soldierDistance > enemyController.Enemy.AttackRange)
                    {
                        // Console.WriteLine("Soldier is not in range of Enemy");
                        enemyController.Enemy.IsAttacking = false;
                        enemyController.Enemy.IsMoving = true;
                    }

                }

                foreach (ObstacleController obstacleController in _obstacleControllers)
                {
                    float obstacleDistance = (enemyController.Enemy.BodySpritePosition.X - enemyController.Enemy.BodySourceRectangle.Width) - (obstacleController.obstacle.Position.X + obstacleController.obstacle.SourceRectangle.Width);
                    if(obstacleDistance <= enemyController.Enemy.AttackRange)
                    {
                        enemyController.Enemy.IsAttacking = true;
                        enemyController.Enemy.IsMoving = false;
                        isInRange = true;
                        break;
                    }
                }

                if(isInRange)
                {
                    continue;
                }

                float distance = (enemyController.Enemy.BodySpritePosition.X - enemyController.Enemy.BodySourceRectangle.Width) - (_highrise.SpritePosition.X + _highrise.SourceRectangle.Width - 100);
                // Console.WriteLine("Distance to Highrise: " + distance);
                if (distance <= enemyController.Enemy.AttackRange)
                {
                    // Console.WriteLine("Highrise is in range of Enemy");
                    enemyController.Enemy.IsAttacking = true;
                    enemyController.Enemy.IsMoving = false;
                }
                else if(distance > enemyController.Enemy.AttackRange)
                {
                    enemyController.Enemy.IsAttacking = false;
                    enemyController.Enemy.IsMoving = true;
                }
            }

            foreach (SoldierController soldierController1 in _soldierControllers)
            {
                if (soldierController1.Soldier.IsDead)
                {
                    continue;
                }

                if(soldierController1.Soldier is AASoldier aaSoldier)
                {
                    Console.WriteLine("Checking for AA Soldier: " + aaSoldier.BodySpritePosition.X);
                    foreach (EnemyPlaneController enemyPlaneController in _enemyPlaneControllers)
                    {
                        float distance = (enemyPlaneController._enemyPlane.BodySpritePosition.X - enemyPlaneController._enemyPlane.BodySourceRectangle.Width) - (aaSoldier.BodySpritePosition.X + aaSoldier.BodySourceRectangle.Width);
                        if(enemyPlaneController._enemyPlane.BodySpritePosition.X < aaSoldier.BodySpritePosition.X + 50 || enemyPlaneController._enemyPlane.isDead)
                        {
                            aaSoldier.IsAttacking = false;
                            aaSoldier.AddTargetPosition(Vector2.Zero, null, null);
                            continue;
                        }   

                        if (distance <= aaSoldier.AttackRange)
                        {
                            aaSoldier.IsAttacking = true;
                            // Console.WriteLine("Current enemy position is: " + aaSoldier.Target.Position.X);
                            aaSoldier.AddTargetPosition(enemyPlaneController._enemyPlane.BodySpritePosition, null, enemyPlaneController._enemyPlane);
                            // Console.WriteLine("New enemy position is: " + enemyPlaneController._enemyPlane.BodySpritePosition.X);
                            continue;
                        }
                        else if(distance > aaSoldier.AttackRange)
                        {
                            aaSoldier.IsAttacking = false;
                        }
                    }
                }else
                {

                    // Console.WriteLine("Checking for Soldier: " + soldierController1.Soldier.BodySpritePosition.X);
                    if (_enemyControllers.Count <= 0)
                    {
                        // Console.WriteLine("No Enemies to attack");
                        soldierController1.Soldier.IsAttacking = false;
                        // Console.WriteLine("No Enemies to attack");
                    }
                    else foreach (EnemyController enemyController in _enemyControllers)
                        {
                            float distance = (enemyController.Enemy.BodySpritePosition.X - enemyController.Enemy.BodySourceRectangle.Width) - (soldierController1.Soldier.BodySpritePosition.X + soldierController1.Soldier.BodySourceRectangle.Width);
                            // Console.WriteLine("Soldiers attack range is: " + soldierController1.Soldier.AttackRange);
                            // Console.WriteLine("Distance to Enemy: " + distance);

                            if(distance > soldierController1.Soldier.AttackRange || enemyController.Enemy.IsDead)
                            {
                                soldierController1.Soldier.IsAttacking = false;
                                soldierController1.Soldier.AddTargetPosition(Vector2.Zero, null, null);
                                continue;
                            }

                            if (distance <= soldierController1.Soldier.AttackRange)
                            {
                                // Console.WriteLine("Soldier is in range of Enemy");
                                soldierController1.Soldier.IsAttacking = true;
                                soldierController1.Soldier.AddTargetPosition(new Vector2(enemyController.Enemy.BodySpritePosition.X, enemyController.Enemy.BodySpritePosition.Y), enemyController.Enemy, null);
                                // Console.WriteLine("Soldier is attacking Enemy");
                                continue;

                            } else if (distance > soldierController1.Soldier.AttackRange)
                            {
                                soldierController1.Soldier.IsAttacking = false;
                                soldierController1.Soldier.AddTargetPosition(Vector2.Zero, null, null);
                            }
                        }
                }
            }
        }
    }
}
