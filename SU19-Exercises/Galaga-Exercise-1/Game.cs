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
        private Enemy enemy; 
        private Enemy enemy2; 
        private Enemy enemy3; 
        private Enemy enemy4;
        private Enemy enemy5; 
        private Enemy enemy6; 
        private Enemy enemy7; 
        private Enemy enemy8; 
        private List<Image> enemyStrides = new List<Image>();
        private List<Enemy> enemies = new List<Enemy>();

        public Game() {
                
            win = new Window("test" ,500, 500);
            gameTimer = new GameTimer(60,60);
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new List<Enemy>(); 
            
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

        public void AddEnemies()
        {
            enemy = new Enemy(this, new DynamicShape(new Vec2F(0.1f, 0.8f),
        new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy);
            enemy2 = new Enemy(this, new DynamicShape(new Vec2F(0.2f, 0.8f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy2);
            enemy3 = new Enemy(this, new DynamicShape(new Vec2F(0.3f, 0.8f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy3);
            enemy4 = new Enemy(this, new DynamicShape(new Vec2F(0.4f, 0.8f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy4); 
            enemy5 = new Enemy(this, new DynamicShape(new Vec2F(0.5f, 0.8f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy5);
            enemy6 = new Enemy(this, new DynamicShape(new Vec2F(0.6f, 0.8f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy6);
            enemy7 = new Enemy(this, new DynamicShape(new Vec2F(0.7f, 0.8f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy7);
            enemy8 = new Enemy(this, new DynamicShape(new Vec2F(0.8f, 0.8f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy8); 
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
                    AddEnemies();
                    foreach (Enemy element in enemies)
                    {
                        element.RenderEntity(); 
                    }

                    
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