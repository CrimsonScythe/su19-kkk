using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Test {
    [TestFixture]
    public class PlayerTests {
        // sets up a game instance
        private GameRunning gameR;
        private Game game;
        private StateMachine stateMachine;

        [SetUp]
        public void InitiateGame() {
            game = new Game();
            gameR = GameRunning.GetInstance(game, null);
            stateMachine = new StateMachine(game);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void PlayerMoveRight(int timerToTest) {
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
        public void PlayerMoveLeft(int timerToTest) {
            int i = 0;
            double xStartPostion = gameR.player.shape.Position.X;
            while (i < timerToTest) {
                gameR.currentVelocity = new Vec2F(-0.1f, 0f);
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
        public void PlayerMoveUp(int timerToTest) {
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

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(10)]
        public void AssumePlayerDoNotMove(int timerToTest) {
            int i = 0;
            double xStartPosition = gameR.player.shape.Position.X;
            double yStartPosition = gameR.player.shape.Position.Y;
            while (i < timerToTest) {
                gameR.currentVelocity = new Vec2F(0.0f, 0.0f);
                gameR.UpdateGameLogic();
                gameR.RenderState();
                i++;
            }
            double yEndPosition = gameR.player.shape.Position.Y;
            double xEndPosition = gameR.player.shape.Position.X;
            Assert.IsTrue(yStartPosition == yEndPosition && xStartPosition == xEndPosition);            
        }

        [TestCase(3,"Left")]
        [TestCase(4,"Left")]
        [TestCase(7,"Right")]
        [TestCase(8,"Right")]
        [TestCase(4,"Down")]
        [TestCase(5,"Down")]
        public void PlayerCollision(int timerToTest, string directionValue) {
            gameR.player.Entity.Shape.SetPosition(new Vec2F(0.3f,0.3f));           
            int i = 0;
            while (i < timerToTest) {
                if (directionValue == "Left") {
                    gameR.currentVelocity = new Vec2F(-0.1f, 0f);
                    gameR.UpdateGameLogic();
                    gameR.RenderState();
                    i++;
                }
                else if (directionValue == "Right") {
                    gameR.currentVelocity = new Vec2F(0.1f, 0.0f);
                    gameR.UpdateGameLogic();
                    gameR.RenderState();
                    i++;
                }
                else {
                    gameR.currentVelocity = new Vec2F(0.0f, -0.1f);
                    gameR.UpdateGameLogic();
                    gameR.RenderState();
                    i++;                    
                }                              
            }
            SpaceTaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameOver>());            
        }
    }
}