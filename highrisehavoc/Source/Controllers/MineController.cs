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
    public class MineController
    {
        public Mine Mine;
        public MineRenderer MineRenderer;
        public bool HasMine = false;
        public CollisionController collisionController;
        public SoundController soundController;
        public MineController(Mine mine, MineRenderer mineRenderer, CollisionController collisionController, SoundController soundController)
        {
            Mine = mine;
            MineRenderer = mineRenderer;
            this.collisionController = collisionController;
            this.soundController = soundController;

            this.collisionController.AddMineController(this);

            this.collisionController.SubscribeToHitEvent(OnHit);
        }
        public void Draw()
        {
            if(HasMine || Mine.IsExploded) { MineRenderer.DrawExplosion(Mine);  }
            MineRenderer.Draw(Mine);
        }
        public MineController ReturnCopy(CollisionController copiedCollisionController)
        {
            MineController copyMine = new MineController(Mine.ReturnCopy(), MineRenderer.ReturnCopy(), copiedCollisionController, soundController);
            copyMine.HasMine = HasMine;
            return copyMine;
        }

        public void Update(GameTime gameTime)
        {
            if(Mine.IsExploded)
            {
                HasMine = false;
                collisionController.RemoveMineController(this);
            }
        }

        public void OnHit(object sender, CollisionController.HitEventArgs e)
        {
            if (e.Mine != null && e.Mine == Mine)
            {
                /* touch off logic for later
                    if (Mine.IsExploded <= 0)
                    {
                        obstacle.IsSolid = false;
                        collisionController.RemoveObstacleController(this);
                    }
                */
                Mine.IsExploded = true;
                HasMine = false;
                collisionController.RemoveMineController(this);
            }
        }
    }
}
