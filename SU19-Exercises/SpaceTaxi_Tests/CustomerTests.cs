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
            GameRunning.GetInstance(game, null);

        }

        [Test]
        public void CustomerTimeOut() {

            ChoseLevel.GetInstance().filename = "short-n-sweet.txt";
            GameRunning.instance = null;
            GameRunning.GetInstance(game, null);
            
//            GameRunning.instance.player.thrust = new Vec2F(-0.00001f, 0f);
            
             //sets a convenient start position for player
            GameRunning.instance.player.Entity.Shape.SetPosition(new Vec2F(0.3f
                ,0.25f));

            GameRunning.instance.gravity = new Vec2F(0f, -0.0000003f);
  

            SpaceTaxiBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));
            


            while (GameOver.instance == null) {

                        
                game.gameTimer.MeasureTime();

                while (game.gameTimer.ShouldUpdate()) {
                    SpaceTaxiBus.GetBus().ProcessEvents();                  
                    game.win.PollEvents();
                    game.eventBus.ProcessEvents();
                    game.stateMachine.ActiveState.UpdateGameLogic();
                }

                if (game.gameTimer.ShouldRender()) {
                    game.win.Clear();
                    game.backGroundImage.RenderEntity();
                    game.stateMachine.ActiveState.RenderState();                                    
                    game.win.SwapBuffers(); 
                }

                if (game.gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps from the timer
//                    win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
//                                gameTimer.CapturedFrames;
                }
            }
            
            Assert.That(game.stateMachine.ActiveState, Is.InstanceOf<GameOver>());
        }

        [Test]
        public void CustomerSpawn() {

            
            GameRunning.instance = null;
            GameRunning.GetInstance(game, null);
            
//            GameRunning.instance.player.thrust.Y = 0f;
   
            //sets a convenient start position for player
            GameRunning.instance.player.Entity.Shape.SetPosition(new Vec2F(0.3f
                ,0.25f));

            GameRunning.instance.gravity = new Vec2F(0f, -0.0000003f);
  

            SpaceTaxiBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));
            
//            game.GameLoop();

            while (SingletonScore.Instance.score==0) {


                if (GameRunning.instance != null) {
                    
                
                    if (GameRunning.instance.player.shape.Position.Y < 0.1) {
                        GameRunning.instance.player.SetPosition(0.5f, 0.7f);
                        GameRunning.instance.player.thrust.Y = 0.00001f;
                    }
                }

                if (GameRunning.instance != null) {
                    
                
                    if (GameRunning.instance.currentLevel.levelName.Equals("the-beach.txt") && 
                        GameRunning.instance.player.shape.Position.Y < 0.8f) {
                        GameRunning.instance.player.SetPosition(0.1f, 0.84f);
                        GameRunning.instance.player.thrust.Y = 0.000001f;
                    }
                
                }
                        
                game.gameTimer.MeasureTime();

                while (game.gameTimer.ShouldUpdate()) {
                    SpaceTaxiBus.GetBus().ProcessEvents();                  
                    game.win.PollEvents();
                    game.eventBus.ProcessEvents();
                    game.stateMachine.ActiveState.UpdateGameLogic();
                }

                if (game.gameTimer.ShouldRender()) {
                    game.win.Clear();
                    game.backGroundImage.RenderEntity();
                    game.stateMachine.ActiveState.RenderState();                                    
                    game.win.SwapBuffers(); 
                }

                if (game.gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps from the timer
//                    win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
//                                gameTimer.CapturedFrames;
                }
            }
            
            Assert.AreEqual(100, SingletonScore.Instance.score);
            
        }

        
        [Test]
        public void CustomerCollision() {

            GameRunning.instance = null;
            GameRunning.GetInstance(game, null);
            
//            GameRunning.instance.player.thrust = new Vec2F(-0.00001f, 0f);
            //sets a convenient start position for player
            GameRunning.instance.player.Entity.Shape.SetPosition(new Vec2F(0.3f
                ,GameRunning.instance.player.Entity.Shape.Position.Y-0.1f));
//            
            // changes currentVelocity to "move" player 
            GameRunning.instance.currentVelocity = new Vec2F(0f, -0.002f);
            
            // mock game loop
            while (!GameRunning.instance.currentLevel.cusList[0].entity.IsDeleted()) {

                    GameRunning.instance.UpdateGameLogic();        
                    GameRunning.instance.RenderState();        
                
            }
            
            Assert.IsTrue(GameRunning.instance.currentLevel.cusList[0].entity.IsDeleted());
            
        }
        
//        [Test]
//        public void NoCustomerNextLevel() {
//            
////            GameRunning.instance.player.thrust = new Vec2F(-0.00001f, 0f);
//            //sets a convenient start position for player
//            GameRunning.instance.player.Entity.Shape.SetPosition(new Vec2F(0.5f
//                ,GameRunning.instance.player.Entity.Shape.Position.Y-0.1f));
////            
//            // changes currentVelocity to "move" player 
//            GameRunning.instance.currentVelocity = new Vec2F(0f, 0.002f);
//            
//            // mock game loop
//            while (!GameRunning.instance.currentLevel.cusList[0].entity.IsDeleted()) {
//
//                GameRunning.instance.UpdateGameLogic();        
//                GameRunning.instance.RenderState();        
//                
//            }
//            
////            Assert.IsTrue(GameRunning.instance.currentLevel.cusList[0].entity.IsDeleted());
//            
//        }
        
    }
    
}