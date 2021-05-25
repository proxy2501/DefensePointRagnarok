using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    /// <summary>
    /// Author: Daniel Lund Justesen
    /// 
    /// Boss entity for this game
    /// </summary>
    public class Boss : Component
    {
        private float speed;
        private Vector2 velocity;
        public bool isAlive = true;
        public int health = 200;
        public int Damage = 30;

        public int Health
        {
            get
            {
                return health;
            }
            private set
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

        public Boss(float speed, Vector2 velocity)
        {
            this.speed = speed;
            this.velocity = velocity;
        }

        public override string ToString()
        {
            return "Boss";
        }

        public override void Awake()
        {
            GameObject.Tag = "Boss";
            GameWorld.Instance.Colliders.Add((Collider)GameObject.GetComponent("Collider"));
        }

        public override void Destroy()
        {
            //PlayScreen.bossEncounter = false;
            //PlayScreen.bossSpawned = false;
            //PlayScreen.enemyKillCounter = 0;
            GameWorld.Instance.Colliders.Remove((Collider)GameObject.GetComponent("Collider"));
            DropLoot();
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isAlive)
            {
                GameObject.Destroy();
            }

            Move();
        }

        public void Move()
        {
            GameObject.Transform.Translate(velocity * speed * GameWorld.Instance.DeltaTime);
        }

        public void DropLoot()
        {
            //To do
            //Making it so boss drops loot when killed
        }
    }
}
