using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    //Author: Arturas Tatariskinas
    public class EnemyFactory : Factory
    {
        #region singleton
        private static EnemyFactory instance;

        public static EnemyFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnemyFactory();
                }
                return instance;
            }
        }
        #endregion
        

        public override GameObject Create(string type)
        {
            Random rng = new Random();
            int number = rng.Next(1, 4);
            GameObject go = new GameObject();
            
            if(number==1)
            {
                go.Transform.Position = new Vector2(1920, GameWorld.Instance.GraphicsDevice.Viewport.Height / 2);//Enemy spawns from the right
            }
            else if (number==2)
            {
                go.Transform.Position = new Vector2(850, 0);//Enemy spawns from the top
            }
            else if (number == 3)
            {
                go.Transform.Position = new Vector2(850, 1080);//Enemy spawns from the bottom
            }


            switch (type)
            {
                case "Enemy": //enemyNameHere
                    SpriteRenderer sr = new SpriteRenderer("Sprites/Temp/Enemy");
                    go.AddComponent(sr);
                    Animator ar = new Animator(19, "Enemy_Walking_Animation/8_enemies_1_walk_");
                    go.AddComponent(ar);
                    go.AddComponent(new Collider());
                    if(number==1)
                    {
                        go.AddComponent(new Enemy(new Vector2(-1, 0)));//Enemy spawns from the right
                    }
                    else if(number==2)
                    {
                        go.AddComponent(new Enemy(new Vector2(-1, 1)));//Enemy spawns from the top
                    }
                    else if (number == 3)
                    {
                        go.AddComponent(new Enemy(new Vector2(-1, -1)));//Enemy spawns from the bottom
                    }
                    break;
            }
            return go;
        }
    }
}
