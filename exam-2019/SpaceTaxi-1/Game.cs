using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace exam_2019 {
    public class Game : IGameEventProcessor<object> {
        
        public GameTimer gameTimer;
        
        private List<Obstacle> obstacles;
        public Level currentLevel;
        public Vec2F gravity = new Vec2F(0f, -0.000003f);

        public Entity backGroundImage;
        public Window win;
        public StateMachine stateMachine;
        public GameEventBus<object> eventBus;

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

            // event delegation
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.WindowEvent, this); 
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
        }

        public void KeyRelease(string key) {            
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