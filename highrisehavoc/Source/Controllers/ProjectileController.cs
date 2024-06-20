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

namespace highrisehavoc.Source.Controllers
{
    public class ProjectileController
    {
        public Projectile _projectile;
        public ProjectileRenderer _projectileRenderer;
        public CollisionController _collisionController;
        public bool _outOfBounds = false;

        public ProjectileController(Projectile projectile, ProjectileRenderer projectileRenderer, CollisionController collisionController)
        {
            _projectile = projectile;
            _projectileRenderer = projectileRenderer;
            _collisionController = collisionController;
        }

        public void Update(GameTime gameTime)
        {
            _projectile.Target.UpdatePosition();
            if (_projectile.Target.Enemy == null && _projectile.Target.EnemyPlane == null)
            {
                // projectile should just continue straight
                float elapsedtime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float movement1 = 250 * elapsedtime;
                
                // continue with the same angle as before or use direction
                if(_projectile.Direction == 1)
                {
                    // shoot left
                    _projectile.SpritePosition.X -= (float)Math.Cos(_projectile.prevAngle) * movement1;
                    _projectile.SpritePosition.Y -= (float)Math.Sin(_projectile.prevAngle) * movement1;
                    return;
                }
                else if(_projectile.Direction == -1)
                {
                    // shoot right
                    _projectile.SpritePosition.X += (float)Math.Cos(_projectile.prevAngle) * movement1;
                    _projectile.SpritePosition.Y += (float)Math.Sin(_projectile.prevAngle) * movement1;
                    return;
                }

                return;
            }
            // calculate the angle between the projectile and the target and move the projectile in that direction
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // random speed between 250 and 500

            float movement;
            if(_projectile.Target.EnemyPlane != null)
            {
                movement = (float)new Random().Next(400, 550) * elapsed;
            } else movement = (float)new Random().Next(200, 350) * elapsed;

            // add small margin of error to the angle to make the projectile miss the target

            float angle = (float)Math.Atan2(_projectile.Target.Position.Y - _projectile.SpritePosition.Y, _projectile.Target.Position.X - _projectile.SpritePosition.X);
            _projectile.SpritePosition.X += (float)Math.Cos(angle) * movement;
            _projectile.SpritePosition.Y += (float)Math.Sin(angle) * movement;

            _projectile.prevAngle = angle;
            
            if(_projectile.SpritePosition.X < 0 || _projectile.SpritePosition.X > 3500)
            {
                Console.WriteLine("Projectile out of bounds");
                _outOfBounds = true;
            }
        }

        public void Draw(float? angleOfBullet)
        {
            float angle;
            if (angleOfBullet.HasValue) { angle = angleOfBullet.Value; }
            else
            {
                if(_projectile.Target.Enemy == null && _projectile.Target.EnemyPlane == null)
                {
                    angle = _projectile.prevAngle;
                }
                else angle = (float)Math.Atan2(_projectile.Target.Position.Y - _projectile.SpritePosition.Y, _projectile.Target.Position.X - _projectile.SpritePosition.X);
            }
            _projectileRenderer.Draw(_projectile, angle);
        }
    }
}
