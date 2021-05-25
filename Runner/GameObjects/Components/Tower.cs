using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This component defines the attributes of towers in the game.
    /// </summary>
    public class Tower : Component
    {
        #region Fields
        // Timer for handling reloading.
        private float reloadTimer;

        // Determines if the tower is reloading.
        private bool reloading;
        #endregion

        #region Properties
        /// <summary>
        /// Range of the tower for shooting enemies
        /// </summary>
        public float Range { get; private set; }

        /// <summary>
        /// The time (in seconds) between shots.
        /// </summary>
        public float ShotInterval { get; private set; }

        /// <summary>
        /// The type of projectile the tower shoots.
        /// </summary>
        public string ProjectileType { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Tower()
        {

        }

        /// <summary>
        /// Constructor that sets range, rate of fire, and projectile type.
        /// </summary>
        /// <param name="range">A float, determining the targeting range of the tower.</param>
        /// <param name="shotInterval">A float, determining the time (in seconds) between shots.</param>
        /// <param name="projectileType">A string, determining the type of projectile the tower shoots.</param>
        public Tower(float range, float shotInterval, string projectileType)
        {
            this.Range = range;
            this.ShotInterval = shotInterval;
            this.ProjectileType = projectileType;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Update method. Runs code that should execute every frame.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            AutoAttack();
        }

        /// <summary>
        /// Code that needs to run when the Component enters the game.
        /// </summary>
        public override void Awake()
        {
            // Fixate tower's position to a node in the grid.
            GameObject.Transform.Position = PathManager.Instance.Grid.FixatePosition(GameObject.Transform.Position);

            // Set the tower's origin to the bottom of the sprite + an offset to appear in the middle of occupied Node.
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.Origin = new Vector2(sr.Sprite.Width / 2, sr.Sprite.Height - 25);

            // Notify SpriteRenderer not to update layer depth continuously.
            sr.CanMoveInGameWorld = false;

            // Set the occupied node to unwalkable.
            Node centerNode = PathManager.Instance.Grid.GetNodeFromPosition(GameObject.Transform.Position);
            centerNode.Walkable = false;
        }

        /// <summary>
        /// Returns the name of this component.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return "Tower";
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Continuously shoots projectiles at the nearest enemy within range, reloading between shots.
        /// </summary>
        private void AutoAttack()
        {
            if (reloading)
            {
                Reload();
            }
            else
            {
                ShootInDirection(GetDirectionToNearestTarget());
            }
        }

        /// <summary>
        /// Enables tower to shoot again after reload time has expired.
        /// </summary>
        private void Reload()
        {
            if (reloadTimer >= ShotInterval)
            {
                reloading = false;
                reloadTimer = 0;
            }
            else
            {
                reloadTimer += GameWorld.Instance.DeltaTime;
            }
        }

        /// <summary>
        /// Shoots a projectile in the given direction.
        /// </summary>
        /// <param name="direction">A normalized Vector2 representing a direction.</param>
        private void ShootInDirection(Vector2 direction)
        {
            if (direction == Vector2.Zero) return;

            GameObject go = ProjectileFactory.Instance.Create(ProjectileType, GameObject.Transform.Position, direction);
            GameWorld.Instance.AddGameObject(go);
            reloading = true;
        }

        /// <summary>
        /// Determines the direction to closest target within range.
        /// </summary>
        /// <returns>A normalized Vector2 representing the direction to target.</returns>
        private Vector2 GetDirectionToNearestTarget()
        {
            Vector2 targetDirection = Vector2.Zero;
            float shortestFoundDistance = float.MaxValue;

            foreach (GameObject gameObject in GameWorld.Instance.GameObjects)
            {
                if (gameObject.Tag != "Enemy") continue;

                Vector2 vectorToEnemy = (gameObject.Transform.Position - GameObject.Transform.Position);
                float distanceSquaredToEnemy = vectorToEnemy.LengthSquared();

                if (distanceSquaredToEnemy > Range * Range) continue;
                if (distanceSquaredToEnemy > shortestFoundDistance) continue;

                shortestFoundDistance = distanceSquaredToEnemy;
                targetDirection = Vector2.Normalize(vectorToEnemy);
            }

            return targetDirection;
        }
        #endregion
    }
}
