using Runner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Tests
{
    /// <summary>
    /// Author: Mikkel Emil Nielsen-Man
    /// 
    /// This class is for testing functionality associated with the repository and database.
    /// </summary>
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void AfterAddingGameObjects_AllGameObjectCanBeFound()
        {
            // Arrange
            GameMapper mapper = new GameMapper();
            SQLiteDatabaseProvider provider = new SQLiteDatabaseProvider("Data Source=:memory:;Version=3;New=True");
            GameRepository repo = new GameRepository(provider, mapper);

            GameObject go1 = new GameObject();
            GameObject go2 = new GameObject();
            GameObject go3 = new GameObject();
            GameObject go4 = new GameObject();
            GameObject go5 = new GameObject();

            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects.Add(go1);
            gameObjects.Add(go2);
            gameObjects.Add(go3);
            gameObjects.Add(go4);
            gameObjects.Add(go5);

            repo.Open();
            repo.AddGameObjects(gameObjects);

            // Act
            List<GameObject> result = repo.GetGameObjects();

            // Assert
            Assert.AreEqual(5, result.Count);

            repo.Close();
        }

        public void AfterAddingGameObjects_AllComponentsCanBeFound()
        {
            // Arrange
            GameMapper mapper = new GameMapper();
            SQLiteDatabaseProvider provider = new SQLiteDatabaseProvider("Data Source=:memory:;Version=3;New=True");
            GameRepository repo = new GameRepository(provider, mapper);

            GameObject go1 = new GameObject();
            Player p = new Player();
            go1.AddComponent(p);
            SpriteRenderer sr1 = new SpriteRenderer("player_sprite_path");
            go1.AddComponent(sr1);

            GameObject go2 = new GameObject();
            TownHall th = new TownHall(90);
            go2.AddComponent(th);
            SpriteRenderer sr2 = new SpriteRenderer("town_hall_sprite_path");
            go2.AddComponent(sr2);
            Collider c2 = new Collider();
            go2.AddComponent(c2);

            GameObject go3 = new GameObject();
            Enemy e = new Enemy(new Vector2(12, 12));
            go3.AddComponent(e);
            SpriteRenderer sr3 = new SpriteRenderer("enemy_sprite_path");
            go3.AddComponent(sr3);
            Collider c3 = new Collider();
            go3.AddComponent(c3);
            Animator a3 = new Animator(5, "enemy_animation_path");
            go3.AddComponent(a3);

            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects.Add(go1);
            gameObjects.Add(go2);
            gameObjects.Add(go3);

            repo.Open();
            repo.AddGameObjects(gameObjects);

            // Act
            List<GameObject> result = repo.GetGameObjects();

            // Assert
            Assert.AreEqual(2, result[0].GetAllComponents().Count);
            Assert.AreEqual(3, result[1].GetAllComponents().Count);
            Assert.AreEqual(4, result[2].GetAllComponents().Count);

            repo.Close();
        }

        [TestMethod]
        public void AfterGettingGameObject_GameObjectHasCorrectValues()
        {
            // Arrange
            GameMapper mapper = new GameMapper();
            SQLiteDatabaseProvider provider = new SQLiteDatabaseProvider("Data Source=:memory:;Version=3;New=True");
            GameRepository repo = new GameRepository(provider, mapper);

            GameObject go = new GameObject();
            go.Transform.Position = new Vector2(340, 90);

            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects.Add(go);

            repo.Open();
            repo.AddGameObjects(gameObjects);

            // Act
            GameObject result = repo.GetGameObjects().First();

            // Assert
            Assert.AreEqual(result.Transform.Position.X, go.Transform.Position.X);
            Assert.AreEqual(result.Transform.Position.Y, go.Transform.Position.Y);

            repo.Close();
        }

        [TestMethod]
        public void AfterGettingEnemy_EnemyHasCorrectValues()
        {
            // Arrange
            GameMapper mapper = new GameMapper();
            SQLiteDatabaseProvider provider = new SQLiteDatabaseProvider("Data Source=:memory:;Version=3;New=True");
            GameRepository repo = new GameRepository(provider, mapper);

            GameObject go = new GameObject();
            Enemy e = new Enemy(new Vector2(12, 1014));
            go.AddComponent(e);
            SpriteRenderer sr = new SpriteRenderer("enemy_sprite_path");
            go.AddComponent(sr);
            Collider c = new Collider();
            go.AddComponent(c);
            Animator a = new Animator(5, "enemy_animation_path");
            go.AddComponent(a);

            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects.Add(go);

            repo.Open();
            repo.AddGameObjects(gameObjects);

            // Act
            GameObject result = repo.GetGameObjects().First();

            Enemy eResult = (Enemy)result.GetComponent("Enemy");
            SpriteRenderer srResult = (SpriteRenderer)result.GetComponent("SpriteRenderer");
            Collider cResult = (Collider)result.GetComponent("Collider");
            Animator aResult = (Animator)result.GetComponent("Animator");

            // Assert
            Assert.AreEqual(e.Velocity, eResult.Velocity);
            Assert.AreEqual(e.Health, eResult.Health);
            Assert.AreEqual(sr.SpritePath, srResult.SpritePath);
            Assert.AreEqual(c.CollisionBox, cResult.CollisionBox);
            Assert.AreEqual(a.TextureSpriteAmount, aResult.TextureSpriteAmount);
            Assert.AreEqual(a.TextureSpriteName, aResult.TextureSpriteName);

            repo.Close();
        }

        [TestMethod]
        public void AfterGettingPlayer_PlayerHasCorrectValues()
        {
            // Arrange
            GameMapper mapper = new GameMapper();
            SQLiteDatabaseProvider provider = new SQLiteDatabaseProvider("Data Source=:memory:;Version=3;New=True");
            GameRepository repo = new GameRepository(provider, mapper);

            GameObject go = new GameObject();
            Player p = new Player();
            go.AddComponent(p);
            SpriteRenderer sr = new SpriteRenderer("player_sprite_path");
            go.AddComponent(sr);
            Collider c = new Collider();
            go.AddComponent(c);

            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects.Add(go);

            repo.Open();
            repo.AddGameObjects(gameObjects);

            // Act
            GameObject result = repo.GetGameObjects().First();

            Player pResult = (Player)result.GetComponent("Player");
            SpriteRenderer srResult = (SpriteRenderer)result.GetComponent("SpriteRenderer");
            Collider cResult = (Collider)result.GetComponent("Collider");

            // Assert
            Assert.AreEqual(p.KnockedBack, pResult.KnockedBack);
            Assert.AreEqual(sr.SpritePath, srResult.SpritePath);
            Assert.AreEqual(c.CollisionBox, cResult.CollisionBox);

            repo.Close();
        }

        [TestMethod]
        public void AfterGettingProjectile_ProjectileHasCorrectValues()
        {
            // Arrange
            GameMapper mapper = new GameMapper();
            SQLiteDatabaseProvider provider = new SQLiteDatabaseProvider("Data Source=:memory:;Version=3;New=True");
            GameRepository repo = new GameRepository(provider, mapper);

            GameObject go = new GameObject();
            Projectile p = new Projectile(55, 67);
            go.AddComponent(p);
            SpriteRenderer sr = new SpriteRenderer("projectile_sprite_path");
            go.AddComponent(sr);
            Collider c = new Collider();
            go.AddComponent(c);

            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects.Add(go);

            repo.Open();
            repo.AddGameObjects(gameObjects);

            // Act
            GameObject result = repo.GetGameObjects().First();

            Projectile pResult = (Projectile)result.GetComponent("Projectile");
            SpriteRenderer srResult = (SpriteRenderer)result.GetComponent("SpriteRenderer");
            Collider cResult = (Collider)result.GetComponent("Collider");

            // Assert
            Assert.AreEqual(p.Velocity, pResult.Velocity);
            Assert.AreEqual(p.Speed, pResult.Speed);
            Assert.AreEqual(p.Damage, pResult.Damage);
            Assert.AreEqual(sr.SpritePath, srResult.SpritePath);
            Assert.AreEqual(c.CollisionBox, cResult.CollisionBox);

            repo.Close();
        }

        [TestMethod]
        public void AfterGettingTower_TowerHasCorrectValues()
        {
            // Arrange
            GameMapper mapper = new GameMapper();
            SQLiteDatabaseProvider provider = new SQLiteDatabaseProvider("Data Source=:memory:;Version=3;New=True");
            GameRepository repo = new GameRepository(provider, mapper);

            GameObject go = new GameObject();
            Tower t = new Tower(45,3.5f,"projectile_type");
            go.AddComponent(t);
            SpriteRenderer sr = new SpriteRenderer("tower_sprite_path");
            go.AddComponent(sr);

            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects.Add(go);

            repo.Open();
            repo.AddGameObjects(gameObjects);

            // Act
            GameObject result = repo.GetGameObjects().First();

            Tower tResult = (Tower)result.GetComponent("Tower");
            SpriteRenderer srResult = (SpriteRenderer)result.GetComponent("SpriteRenderer");

            // Assert
            Assert.AreEqual(t.Range, tResult.Range);
            Assert.AreEqual(t.ShotInterval, tResult.ShotInterval);
            Assert.AreEqual(t.ProjectileType, tResult.ProjectileType);
            Assert.AreEqual(sr.SpritePath, srResult.SpritePath);

            repo.Close();
        }
    }
}