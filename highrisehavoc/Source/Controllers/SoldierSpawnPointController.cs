using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace highrisehavoc.Source.Controllers
{
    public class SoldierSpawnPointController
    {
        public SoldierSpawnPoint SoldierSpawnPoint;
        public SoldierSpawnPointRenderer SoldierSpawnPointRenderer;
        public bool HasSoldier = false;
        public SoldierController SoldierController;
        public ObstacleController ObstacleController;
        public MineController MineController;

        public SoldierSpawnPointController(SoldierSpawnPoint soldierSpawnPoint, SoldierSpawnPointRenderer soldierSpawnPointRenderer)
        {
            SoldierSpawnPoint = soldierSpawnPoint;
            SoldierSpawnPointRenderer = soldierSpawnPointRenderer;
        }
        public void AddSoldierController(SoldierController soldierController)
        {
            SoldierController = soldierController;
        }

        public void AddObstacleController(ObstacleController obstacleController)
        {
            ObstacleController = obstacleController;
        }

        public void AddMineController(MineController mineController)
        {
            MineController = mineController;
        }

        public void Draw()
        {
            if(HasSoldier) { return; }
            SoldierSpawnPointRenderer.Draw(SoldierSpawnPoint);
        }

        public SoldierSpawnPointController ReturnCopy()
        {
            SoldierSpawnPointController copyPoint = new SoldierSpawnPointController(SoldierSpawnPoint, SoldierSpawnPointRenderer);
            if (HasSoldier) copyPoint.HasSoldier = true;
            return copyPoint;
        }

        public void Update(GameTime gameTime)
        {
            // to implement
        }
    }
}
