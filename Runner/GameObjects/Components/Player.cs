using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{/// <summary>
/// Author: Daniel Lund Justesen
/// 
/// Player character for this game
/// </summary>
    public class Player: Component, IGameListener
    {
        private float speed = 110;

        // Author: Mikkel Emil Nielsen-Man
        // Determines the amount of time (seconds) player is affected by knockback.
        private float knockbackDuration = 0.30f;

        // Author: Mikkel Emil Nielsen-Man
        // Keeps track of how long player has been affected by knockback.

        private float knockbackTimer;

        // Author: Mikkel Emil Nielsen-Man
        // The direction player is pushed when affected by knockback.
        private Vector2 knockbackDirection;

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Determines if the player is being knocked back.
        /// </summary>
        public bool KnockedBack { get; private set; }

        /// <summary>
        /// Author: Daniel Lund Justesen
        /// 
        /// Setup for player character to fx. tell inputhandler who it is giving the commands to
        /// </summary>
        public Player()
        {
            InputHandler.Instance.Entity = this;
        }

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="gameTime">GameTime.</param>
        public override void Update(GameTime gameTime)
        {
            // Contributor: Mikkel Emil Nielsen-Man
            if (KnockedBack)
            {
                if (knockbackTimer < knockbackDuration)
                {
                    Move(knockbackDirection, 200);
                    knockbackTimer += GameWorld.Instance.DeltaTime;
                }
                else
                {
                    knockbackTimer = 0;
                    KnockedBack = false;
                }
            }
            // End contribution: Mikkel Emil Nielsen-Man
        }

        /// <summary>
        /// Author: Daniel Lund Justesen
        /// 
        /// Determins the logic behind player movement capabilitys
        /// </summary>
        /// <param name="velocitybob"></param>
        public void Move(Vector2 velocitybob)
        {
            if(velocitybob != Vector2.Zero)
            {
                velocitybob.Normalize();
            }
            velocitybob *= speed;
            GameObject.Transform.Translate(velocitybob * GameWorld.Instance.DeltaTime);
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Override of Move method that takes a new speed parameter.
        /// </summary>
        /// <param name="velocity">A Vector2 that determnes the direction in which to move.</param>
        /// <param name="speed">A float that determines the speed with which to move.</param>

        public void Move(Vector2 velocity, float speed)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            velocity *= speed;
            GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Returns the name of this component.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Player";
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Code that needs to run when GameObject awakes.
        /// </summary>
        public override void Awake()
        {
            GameObject.Tag = "Player";
            Collider collider = (Collider)GameObject.GetComponent("Collider");
            collider.CollisionEvent.Attach(this);
            collider.CheckCollisionEvents = true;
        }

        public override void Start()
        {
            
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Creates a new tower on Player's location.
        /// </summary>
        /// <param name="towerType">A string specifying the type of tower to build.</param>
        public void BuildTower(string towerType)
        {
            // TODO: Prevent tower stacking, refine positioning, check tower type for buildability (factories).
            if (PlayScreen.Gold < 50) return;
            GameObject go = TowerFactory.Instance.Create(towerType, GameObject.Transform.Position);
            GameWorld.Instance.AddGameObject(go);
            PlayScreen.Gold -= 50;
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
            if (gameEvent.Title == "Collision" && (component.GameObject.Tag == "Enemy") || (component.GameObject.Tag == "TownHall"))
            {
                if (KnockedBack) return;
                knockbackDirection = GameObject.Transform.Position - component.GameObject.Transform.Position;
                KnockedBack = true;
            }
        }
    }
}
