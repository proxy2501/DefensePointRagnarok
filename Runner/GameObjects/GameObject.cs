using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{/// <summary>
/// Author: Daniel Lund Justesen
/// 
/// Handles the core of all Gameobjects and how they work thogether with components
/// </summary>
    public class GameObject
    {
        private Dictionary<string, Component> components = new Dictionary<string, Component>();
        private Transform transform;
        private string tag;


        public string Tag { get => tag; set => tag = value; }
        public Transform Transform { get => transform; set => transform = value; }


        public GameObject()
        {
            transform = new Transform();
        }

        public void AddComponent(Component component)
        {
            if (component == null) return;

            components.Add(component.ToString(), component);
            component.GameObject = this;
        }

        public Component GetComponent(string component)
        {
            return components[component];
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Gets a list of all components attached to this GameObject.
        /// </summary>
        /// <returns>A List of Components.</returns>
        public List<Component> GetAllComponents()
        {
            List<Component> result = new List<Component>();

            foreach (Component component in components.Values)
            {
                result.Add(component);
            }

            return result;
        }

        public void Awake()
        {
            foreach (Component item in components.Values)
            {
                item.Awake();
            }
        }

        public void Start()
        {
            foreach (Component item in components.Values)
            {
                item.Start();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Component item in components.Values)
            {
                if(item.IsEnabled == true)
                {
                    item.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Component item in components.Values)
            {
                if(item.IsEnabled == true)
                {
                    item.Draw(spriteBatch);
                }
            }
        }

        public void Destroy()
        {
            foreach (Component component in components.Values)
            {
                component.Destroy();
            }

            GameWorld.Instance.RemoveGameObject(this);
        }
    }
}
