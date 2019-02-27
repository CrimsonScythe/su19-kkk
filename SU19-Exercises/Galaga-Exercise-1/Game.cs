using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace Galaga_Exercise_1 {
    
    public class Game : IGameEventProcessor<object> {

        private Window win;
        private GameTimer gameTimer;
        private GameEventBus<object> eventBus;
        private Player player;

        public Game() {
                
            win = new Window("test" ,500, 500);
            gameTimer = new GameTimer(60,60);
            
            player = new Player(this,
                            new DynamicShape(new Vec2F(0.45f, 0.1f),new Vec2F(0.1f, 0.1f) ),
                            new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);

        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    
                    player.Move();
                    
                    eventBus.ProcessEvents();
                    //update game logic here
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    
                    player.RenderEntity();

                    
                    //render gameplay entites here
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        public void KeyPress(string key) {
            switch (key) {
                case "KEY_ESCAPE":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                    break;
                case "KEY_A":
                    player.Direction(new Vec2F(-0.01f, 0.0f));    
                    break;
                
                case "KEY_D":
                    player.Direction(new Vec2F(0.01f, 0.0f));
                    break;
            }
        }

        public void KeyRelease(string key) {
            switch (key) {
                case "KEY_A":
                    player.Direction(new Vec2F(0.0f, 0.0f));
                    break;
                case "KEY_D":
                    player.Direction(new Vec2F(0.0f, 0.0f));
                    break;
            }
        }
        

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                    case "CLOSE_WINDOW":
                        win.CloseWindow();
                        break;
                    default:
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