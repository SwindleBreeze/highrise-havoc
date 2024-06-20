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
    public class ObstacleController
    {
        public Obstacle obstacle;
        public ObstacleRenderer obstacleRenderer;
        public CollisionController collisionController;
        public SoundController soundController;

        public ObstacleController(Obstacle obstacle, ObstacleRenderer obstacleRenderer, CollisionController collisionController, SoundController soundController)
        {
            this.obstacle = obstacle;
            this.obstacleRenderer = obstacleRenderer;
            this.collisionController = collisionController;
            this.soundController = soundController;

            collisionController.AddObstacleController(this);

            collisionController.SubscribeToHitEvent(OnHit);
        }

        public void Draw()
        {
            if (!obstacle.IsSolid) { return; }
            obstacleRenderer.Draw(obstacle);
        }

        public void Update()
        {
            if(obstacle.Hitpoint <= 0)
            {
                obstacle.IsSolid = false;
                collisionController.RemoveObstacleController(this);
            }
        }
        public ObstacleController ReturnCopy(CollisionController copiedCollisionController)
        {
            ObstacleController copyObstacle = new ObstacleController(obstacle.ReturnCopy(), obstacleRenderer.ReturnCopy(), copiedCollisionController, soundController);
            return copyObstacle;
        }

        public void OnHit(object sender, CollisionController.HitEventArgs e)
        {
            if (e.Obstacle != null && e.Obstacle == obstacle)
            {
                if (obstacle.Hitpoint <= 0)
                {
                    obstacle.IsSolid = false;
                    collisionController.RemoveObstacleController(this);
                }
            }
        }
    }
}
