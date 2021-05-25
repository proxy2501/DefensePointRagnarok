using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Runner
{
    //Author: Magnus E. Poulsen
    public class Enemy : Component, IGameListener
    {
        #region Fields
        private float speed = 100;
        private Vector2 velocity;
        public bool isAlive = true;
        public int health = 100;
        private int damage = 10;

        /// Contributor: Mikkel Emil Nielsen-Man
        /// 
        // A pathfinder that finds paths using the A* algorithm.
        private Pathfinder pathfinder;

        // The target that enemy moves towards.
        private Transform pathingTarget;

        // A path of waypoint positions for enemy to follow.
        private Vector2[] path;

        // Index for tracking pathing waypoints.
        private int pathIndex;

        // Indicates that a path is currently being calculated.
        private bool threadCalculatingPath;

        // A thread for handling pathfinding tasks after initial spawn.
        private Thread pathingThread;

        // Tells the pathfinding thread to calculate a new path.
        private bool threadGetNewPath;
        ///
        /// End contribution: Mikkel Emil Nielsen-Man
        #endregion

        #region Properties
        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Public property for field: 'damage'.
        /// </summary>
        public int Damage
        { 
            get
            {
                return damage;
            }
            private set
            {
                if (value >= 0)
                {
                    damage = value;
                }
                else
                {
                    damage = 0;
                }
            }
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Public property for field: 'velocity'.
        /// </summary>
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        #endregion

        public Enemy(Vector2 velocity)
        {
            this.velocity = velocity;

            pathfinder = new Pathfinder(PathManager.Instance.Grid);
        }

        public override void Update(GameTime gameTime)
        {
            /// Contributor: Mikkel Emil Nielsen-Man
            if (!isAlive)
            {
                GameObject.Destroy();
            }

            FollowPath();
            /// End contribution: Mikkel Emil Nielsen-Man
        }

        private void Move()
        {
            GameObject.Transform.Translate(velocity * speed * GameWorld.Instance.DeltaTime);
        }

        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                if (value <= 0)
                {
                    health = 0;
                    isAlive = false;
                    PlayScreen.Gold += 10;
                }
                else
                {
                    health = value;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Returns the name of this component.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Enemy";
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Code that needs to run when GameObject awakes.
        /// </summary>
        public override void Awake()
        {
            // Set tag.
            GameObject.Tag = "Enemy";

            // Subscribe to pathfinding events.
            PathManager.Instance.NodeBlockedEvent.Attach(this);
            PathManager.Instance.NodeClearedEvent.Attach(this);

            pathingTarget = GameWorld.Instance.FindGameObjectWithTag("TownHall").Transform;
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Code that needs to run when GameObject starts but after it awakes.
        /// </summary>
        public override void Start()
        {
            // Initialize thread for pathing.
            pathingThread = new Thread(UpdatePath);
            pathingThread.IsBackground = true;
            pathingThread.Start();

            // Get initial path.
            threadGetNewPath = true;
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Destroys this component.
        /// </summary>
        public override void Destroy()
        {
            // Unsubscribe from pathfinding events.
            PathManager.Instance.NodeBlockedEvent.Detatch(this);
            PathManager.Instance.NodeClearedEvent.Detatch(this);
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Makes enemy move sequentially through the waypoints contained within its path.
        /// </summary>
        private void FollowPath()
        {
            if (path == null) return;

            if (Vector2.DistanceSquared(path[pathIndex], GameObject.Transform.Position) <= 4)
            {
                pathIndex++;
            }

            velocity = Vector2.Normalize(path[pathIndex] - GameObject.Transform.Position); ;
            Move();
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Defines how this IGameListener handles events.
        /// </summary>
        /// <param name="gameEvent">The notifying GameEvent.</param>
        /// <param name="component">A Component that is involved in the event (if applicable).</param>
        /// <param name="node">A Node that triggered the event (if applicable).</param>
        public void OnNotify(GameEvent gameEvent, Component component = null, Node node = null)
        {
            if (gameEvent.Title == "NodeBlocked")
            {
                if (threadCalculatingPath) return;
                if (path == null) return;
                if (!path.Contains(node.Position)) return;

                threadGetNewPath = true;
            }
            else if (gameEvent.Title == "NodeCleared")
            {
                if (threadCalculatingPath) return;

                threadGetNewPath = true;
            }
        }

        private void UpdatePath()
        {
            while (isAlive)
            {
                if (threadGetNewPath)
                {
                    threadCalculatingPath = true;

                    lock (PathManager.Instance.GridLock)
                    {
                        path = pathfinder.FindPath(GameObject.Transform.Position, pathingTarget.Position);
                    }

                    pathIndex = 0;
                    threadGetNewPath = false;
                    threadCalculatingPath = false;

                }

                // Limit thread cycle to 60 fps.
                Thread.Sleep(17);
            }
        }
    }
}
