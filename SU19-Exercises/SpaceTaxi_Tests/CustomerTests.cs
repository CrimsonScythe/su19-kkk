using System;
using System.Collections.Generic;
using System.Diagnostics;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Tests {
    
   
    public class CustomerTests {

        private Game game;
        private GameRunning gameRunning;
        
        [SetUp]
        public void Initialize() {

            game = new Game();            
            gameRunning = GameRunning.GetInstance(game, null);

        }
        
        
        [TestCase()]
        public void CustomerSpawn() {
            

        }

        
        [Test]
        public void CustomerCollision() {
            
            //sets a convenient start position for player
            gameRunning.player.Entity.Shape.SetPosition(new Vec2F(0.3f
                ,gameRunning.player.Entity.Shape.Position.Y-0.1f));
//            
            // changes currentVelocity to "move" player 
            gameRunning.currentVelocity = new Vec2F(0f, -0.002f);
            
            // mock game loop
            while (!gameRunning.currentLevel.cusList[0].entity.IsDeleted()) {

                gameRunning.UpdateGameLogic();    
                gameRunning.RenderState();    

            }
            
            Assert.IsTrue(gameRunning.currentLevel.cusList[0].entity.IsDeleted());
          
        }
    }
    
}