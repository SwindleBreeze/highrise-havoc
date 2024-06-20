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
    public class EnemyPlaneController
    {
        public EnemyPlane _enemyPlane;
        public EnemyPlaneRenderer _enemyPlaneRenderer;
        private Highrise highrise;
        private CollisionController collisionController;

        public EnemyPlaneController(EnemyPlane enemyPlane, EnemyPlaneRenderer enemyPlaneRenderer, Highrise highrise, CollisionController collisionController)
        {
            _enemyPlane = enemyPlane;
            _enemyPlaneRenderer = enemyPlaneRenderer;
            this.highrise = highrise;
            this.collisionController = collisionController;

            collisionController.AddEnemyPlaneController(this);

            collisionController.SubscribeToHitEvent(onHit);
        }

        public void Draw()
        {
            if(_enemyPlane.isDead)
            {
                _enemyPlaneRenderer.DrawExplosion(_enemyPlane);
            }
            _enemyPlaneRenderer.Draw(_enemyPlane);
        }

        public void Update(GameTime gameTime)
        {
            if(_enemyPlane.HitPoints <= 0)
            {
                _enemyPlane.isDead = true;
                collisionController.RemoveEnemyPlaneController(this);
            }

            if(_enemyPlane.isDead)
            {
                return;
            }
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float movement = _enemyPlane.Speed * elapsed;

            _enemyPlane.BodySpritePosition.X -= movement;
            if(_enemyPlane.BodySpritePosition.X < 0 - _enemyPlane.BodySourceRectangle.Width)
            {
                _enemyPlane.isDead = true;
            }
        }

        private void onHit(object sender, CollisionController.HitEventArgs e)
        {
            if (e.EnemyPlane != null && e.EnemyPlane == _enemyPlane )
            {
                _enemyPlane.HitPoints -= 10;
            }
        }
    }
}
