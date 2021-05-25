using Dania.Courses.FørsteÅrseksamen.Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    //Author: Arturas Tatariskinas
    [TestClass]

    class Player_TownHallCollisionTest
    {
        public class PlayerTownHallTest : Component, IGameListener
        {
            private Vector2 knockbackDirection;

            public bool KnockedBack { get; private set; }

            public void OnNotify(GameEvent gameEvent, Component component = null, Node node = null)
            {
                if (gameEvent.Title == "Collision" && (component.GameObject.Tag == "TownHall"))
                {
                    if (KnockedBack) return;
                    knockbackDirection = GameObject.Transform.Position - component.GameObject.Transform.Position;
                    KnockedBack = true;
                }
            }

            [TestMethod]
            public void PlayerTownHallKnockBack() {
                //Arrange
                var TP = new Player();
                bool expected = KnockedBack = true;
                //Act
                bool KnockBack = TP.KnockedBack;
                //Assert
                Assert.AreEqual(TP.KnockedBack, expected);
            }
        }
    }
}
