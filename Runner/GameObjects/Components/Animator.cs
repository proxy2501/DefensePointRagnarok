using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public class Animator : Component
    {
        ContentManager content = ScreenManager.contentmanager;

        private int fps = 60;
        private Texture2D[] textures;
        private SpriteRenderer spriteRenderer;
        private float timeElasped;

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// The number of sprites in the animation.
        /// </summary>
        public int TextureSpriteAmount { get; private set; }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// The root path name of the animation.
        /// </summary>
        public string TextureSpriteName { get; private set; }

        public Animator(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Constructor that takes a sprite number and a sprite name.
        /// </summary>
        /// <param name="spriteNumber">An Integer denoting the unmber of sprites in the animation.</param>
        /// <param name="animationPath">A string specifying the root path name.</param>
        public Animator(int spriteNumber, string animationPath)
        {
            TextureSpriteAmount = spriteNumber;
            TextureSpriteName = animationPath;
        }

        public override string ToString()
        {
            return "Animator";
        }

        public override void Update(GameTime gameTime)
        {
            PlayAnimation();
        }

        /// <summary>
        /// Author: Mikkel Emil Nielsen-Man
        /// 
        /// Code that needs to run when GameObject starts but after it awakes.
        /// </summary>
        public override void Start()
        {
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            LoadTextureSprites(TextureSpriteAmount, TextureSpriteName);

        }

        public void LoadTextureSprites(int TextureSpriteAmount, string TextureSpriteName)
        {
            textures = new Texture2D[TextureSpriteAmount + 1];

            for (int i = 0; i <= TextureSpriteAmount; i++)
            {
                textures[i] = GameWorld.Instance.Content.Load<Texture2D>(TextureSpriteName + i);
            }

            spriteRenderer.Sprite = textures[0];
            spriteRenderer.Origin = new Vector2(textures[0].Width / 2 ,textures[0].Height / 2);
        }

        private void PlayAnimation()
        {
            //Calculate and set current texture index.
            timeElasped += GameWorld.Instance.DeltaTime;
            int currentIndex = (int)(timeElasped * fps);
            spriteRenderer.Sprite = textures[currentIndex];

            //Reset animation if end of sprites array has been reached.
            if (currentIndex >= textures.Length - 1)
            {
                timeElasped = 0;
                currentIndex = 0;
            }
        }
    }
}
