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
    public class SoldierController
    {
        public Soldier Soldier { get; set; }
        public SoldierRenderer SoldierRenderer { get; set; }
        private Highrise Highrise { get; set; }
        public List<ProjectileController> Projectiles { get; set; }
        private CollisionController CollisionController { get; set; }

        private SoundController _soundController;

        public SoldierController(Soldier soldier, SoldierRenderer soldierRenderer, Highrise highrise, CollisionController collisionController, SoundController soundController)
        {
            Soldier = soldier;
            SoldierRenderer = soldierRenderer;
            Highrise = highrise;
            CollisionController = collisionController;

            Projectiles = new List<ProjectileController>();

            collisionController.AddSoldierController(this);

            // Subscribe to the collision event
            CollisionController.SubscribeToCollisionEvent(onEnemyCollision);
            CollisionController.SubscribeToHitEvent(onHit);
            _soundController = soundController;
        }

        private void onEnemyCollision(object sender, CollisionController.CollisionEventArgs e)
        {
            if (e.Projectile != null)
            {
                // remove projectile from list
                Projectiles.Remove(Projectiles.Find(p => p._projectile == e.Projectile));
            }
        }

        private void onHit(object sender, CollisionController.HitEventArgs e)
        {
            if (e.Soldier != null && e.Soldier == Soldier)
            {
                Soldier.HitPoints -= 1;
            }
        }

        private void spawnProjectile()
        {

            Projectile projectile = new(new Vector2(Soldier.BodySpritePosition.X + (int)(Soldier.BodySourceRectangle.Width * 0.5), Soldier.BodySpritePosition.Y - 5), new Vector2(-1, 0), -1, Soldier.ProjectileSourceRectangle, Soldier.Target);
            ProjectileRenderer projectileRenderer = new(SoldierRenderer.GetSpriteBatch(), SoldierRenderer.GetSpriteSheet());

            ProjectileController projectileController = new(projectile, projectileRenderer, CollisionController);

            _soundController.playSoundEffectARSHOT();

            CollisionController.AddSoldierProjectile(projectile);
            Projectiles.Add(projectileController);
        }

        public void Update(GameTime gameTime)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(Soldier.HitPoints <= 0)
            {
                Soldier.IsDead = true;

                CollisionController.RemoveSoldierController(this);

                // remove all projectiles
                for (int i = 0; i < Projectiles.Count; i++)
                {
                    Projectiles.RemoveAt(i);
                }
            }

            if(Soldier.IsDead)
            {
                return;
            }

            // if the enemy is attacking, spawn a projectile every second
            if (Soldier.IsAttacking)
            {
                // Console.WriteLine("Soldier is attacking in controller");
                // Console.WriteLine("Soldier Target: " + (Soldier.Target.Enemy != null));
                Soldier.AttackTimer += elapsed;
                if (Soldier.AttackTimer > Soldier.AttackSpeed && (Soldier.Target.Enemy != null || Soldier.Target.EnemyPlane != null))
                {
                    spawnProjectile();
                    Soldier.AttackTimer = 0;
                }

                Soldier.Target.UpdatePosition();
            }

            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Update(gameTime);
                if (Projectiles[i]._outOfBounds)
                {
                    Projectiles.RemoveAt(i);
                }
            }

        }

        public void Draw()
        {

            for (int i = 0; i < Projectiles.Count; i++)
            {
                if (Projectiles[i]._outOfBounds)
                {
                    Projectiles.RemoveAt(i);
                }
                else
                {
                    Projectiles[i].Draw(null);
                }
            }

            if (Soldier.IsDead)
            {
                return;
            }

            SoldierRenderer.Draw(Soldier);

        }

        public SoldierController ReturnCopy(CollisionController copiedCollisionController)
        {
            return new SoldierController(Soldier.ReturnCopy(), SoldierRenderer.ReturnCopy(), Highrise.ReturnCopy(), copiedCollisionController, _soundController);
        }

    }
}
