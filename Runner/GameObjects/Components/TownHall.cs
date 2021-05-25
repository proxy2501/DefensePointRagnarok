using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Runner
{
    //Author: Arturas Tatariskinas
    public class TownHall : Component, IGameListener
    {
        //TODO: If IsAlive=True, change screen to GameOver

        #region Fields
        public Vector2 Position { get => position; set => position = value; }
        private Vector2 position;
        protected Texture2D TownHallTexture;
        #endregion

        public bool IsAlive { get; private set; }

        #region constructor
        public TownHall(int health)
        {
            PlayScreen.tHealth = health;
            IsAlive = true;
        }
        #endregion

        #region public Methods
        
        /// <summary>
        /// Looks after value of health, if lower then 0 sets "IsAlive" to false. Else "Shows" how mutch health is left
        /// </summary>
        public int Health
        {
            get => PlayScreen.tHealth;
            private set
            {
                if (value <= 0)
                {
                    PlayScreen.tHealth = 0;
                    IsAlive = false;
                }
                else
                {
                    PlayScreen.tHealth = value;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public override void Awake()
        {
            //Position for player
            GameObject.Transform.Position = PathManager.Instance.Grid.FixatePosition(new Vector2(90, ScreenManager.ScreenDimensions.Y / 2 - 60));
            /// Contributor: Mikkel Emil Nielsen-Man            // Set tag.            GameObject.Tag = "TownHall";            
            // Set the tower's origin to the bottom of the sprite + an offset to appear in the middle of occupied Node.
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            // Notify SpriteRenderer not to update layer depth continuously.            sr.CanMoveInGameWorld = false;

            // Add collider.
            Collider collider = (Collider)GameObject.GetComponent("Collider");
            collider.CollisionEvent.Attach(this);
            collider.CheckCollisionEvents = true;
            /// End contribution: Mikkel Emil Nielsen-Man        }

        public override void Start()
        {

        }

        public override string ToString()
        {
            return "TownHall";
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
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Enemy")
            {
                Enemy enemyComponent = (Enemy)component.GameObject.GetComponent("Enemy");
                TakeDamage(enemyComponent.Damage);
                component.GameObject.Destroy();
            }
            /// Contributor: Daniel Lund Justesen
            else if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Boss")
            {
                Boss bossComponent = (Boss)component.GameObject.GetComponent("Boss");
                TakeDamage(bossComponent.Damage);
                component.GameObject.Destroy();
            }
            /// End constribution: Daniel Lund Justesen
        }
        #endregion
    }
}