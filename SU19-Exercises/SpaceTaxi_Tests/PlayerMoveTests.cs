using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Test {
    [TestFixture]
    public class PlayerMoveTests {
        // sets up a game instance
        private GameRunning gameR;
        private Game game;

        [SetUp]
        public void InitiateGame() {
            game = new Game();           
            gameR = GameRunning.GetInstance(game, null);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]                
        public void PlayerMoveRight (int timerToTest) {
            int i = 0;
            double xStartPostion = gameR.player.shape.Position.X; 
            while (i < timerToTest) {
                gameR.currentVelocity = new Vec2F(0.01f, 0f);
                gameR.UpdateGameLogic();
                gameR.RenderState();              
                i++;
            }
            double xEndPosition = gameR.player.shape.Position.X;      
            Assert.That(xStartPostion < xEndPosition);
            
        }    
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]                
        public void PlayerMoveLeft (int timerToTest) {
            int i = 0;
            double xStartPostion = gameR.player.shape.Position.X; 
            while (i < timerToTest) {
                gameR.currentVelocity = new Vec2F(-0.01f, 0f);
                gameR.UpdateGameLogic();
                gameR.RenderState();              
                i++;
            }
            double xEndPosition = gameR.player.shape.Position.X;      
            Assert.That(xStartPostion > xEndPosition);
            
        }  
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]                
        public void PlayerMoveUp (int timerToTest) {
            int i = 0;
            double yStartPostion = gameR.player.shape.Position.Y; 
            while (i < timerToTest) {
                gameR.currentVelocity = new Vec2F(0.0f, 0.001f);
                gameR.UpdateGameLogic();
                gameR.RenderState();              
                i++;
            }
            double yEndPosition = gameR.player.shape.Position.Y;      
            Assert.That(yStartPostion < yEndPosition);           
        }    
    }
} 