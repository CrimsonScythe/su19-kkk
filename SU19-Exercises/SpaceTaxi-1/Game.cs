using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace SpaceTaxi_1 {
    public class Game : IGameEventProcessor<object> {
        private Entity backGroundImage;
        private GameEventBus<object> eventBus;
        private GameTimer gameTimer;
//        private Player player;
        public Window win;
        private List<Obstacle> obstacles;
        public Level currentLevel;

        private Vec2F gravity = new Vec2F(0f, -0.000005f);
        private Vec2F currentVelocity = new Vec2F(0f,0f);
        private StateMachine stateMachine;
        
        public Game() {
            // window
            win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R1X1);

            // event bus
            eventBus = new GameEventBus<object>();
            SpaceTaxiBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,
                GameEventType.WindowEvent, 
                GameEventType.GameStateEvent,
                GameEventType.PlayerEvent
            });
            win.RegisterEventBus(SpaceTaxiBus.GetBus());

            // game timer
            gameTimer = new GameTimer(60); // 60 UPS, no FPS limit

            // game assets
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            backGroundImage.RenderEntity();

            // game entities
//            player = new Player();
//            player.SetPosition(0.45f, 0.6f);
//            player.SetExtent(0.1f, 0.1f);

            // event delegation
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.WindowEvent, this); 
//            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player); 
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);

            
            //make level
            
            stateMachine = new StateMachine(this);
            
            
        }
                   
        
    
        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    SpaceTaxiBus.GetBus().ProcessEvents();                  
                    win.PollEvents();
                    eventBus.ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    backGroundImage.RenderEntity();
                    stateMachine.ActiveState.RenderState();

                    
                    if (gameTimer.CapturedUpdates == 0) {
                        currentVelocity = (gravity + GameRunning.GetInstance(this).player.thrust) * 1 + currentVelocity;
                    } else {
                        currentVelocity = (gravity + GameRunning.GetInstance(this).player.thrust) * gameTimer.CapturedUpdates + currentVelocity;
                    }
                        
                    
                    GameRunning.GetInstance(this).player.Entity.Shape.Move(currentVelocity);
                                    
                    win.SwapBuffers(); 
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps from the timer
                    win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                 gameTimer.CapturedFrames;
                }
            }
        }

        public void KeyPress(string key) {
            switch (key) {
//            case "KEY_F12":
//                Console.WriteLine("Saving screenshot");
//                win.SaveScreenShot();
//                break;
//            case "KEY_UP":
//                eventBus.RegisterEvent(
//                    GameEventFactory<object>.CreateGameEventForAllProcessors(
//                        GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
//                break;
//            case "KEY_LEFT":
//                eventBus.RegisterEvent(
//                    GameEventFactory<object>.CreateGameEventForAllProcessors(
//                        GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
//                break;
//            case "KEY_RIGHT":
//                eventBus.RegisterEvent(
//                    GameEventFactory<object>.CreateGameEventForAllProcessors(
//                        GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
//                break;
            }
        }

        public void KeyRelease(string key) {
            switch (key) {
//            case "KEY_LEFT":
//                eventBus.RegisterEvent(
//                    GameEventFactory<object>.CreateGameEventForAllProcessors(
//                        GameEventType.PlayerEvent, this, "STOP_ACCELERATE_LEFT", "", ""));
//                break;
//            case "KEY_RIGHT":
//                eventBus.RegisterEvent(
//                    GameEventFactory<object>.CreateGameEventForAllProcessors(
//                        GameEventType.PlayerEvent, this, "STOP_ACCELERATE_RIGHT", "", ""));
//                break;
//            case "KEY_UP":
//                eventBus.RegisterEvent(
//                    GameEventFactory<object>.CreateGameEventForAllProcessors(
//                        GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));
//                break;
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }
    }
}