using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3 {
    
    public class Game : IGameEventProcessor<object> {
        private Window win;
        private GameTimer gameTimer;
        private List<PlayerShot> newPlayerShots = new List<PlayerShot>();
        public Image shotImages;
        public List<PlayerShot> playerShots { get; private set; }
        private StateMachine stateMachine;
        private string globalMove = "down";
        
        public Game() {               
            win = new Window("test" ,500, 500);          
            gameTimer = new GameTimer(60,60);          
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,
                GameEventType.WindowEvent, 
                GameEventType.GameStateEvent,
                GameEventType.PlayerEvent
            });
            win.RegisterEventBus(GalagaBus.GetBus());
            stateMachine = new StateMachine(this);
            shotImages = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
            playerShots = new List<PlayerShot>();           
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this); 
        }      

        public void GameLoop() {
            
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    GalagaBus.GetBus().ProcessEvents();                  
                    stateMachine.ActiveState.UpdateGameLogic();
                    playerShots = newPlayerShots;
                    newPlayerShots = new List<PlayerShot>();
                }
                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    stateMachine.ActiveState.RenderState();
                    foreach (var elem in playerShots) {
                        elem.RenderEntity();
                    }
                    win.SwapBuffers();
                }
                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + 
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        private void KeyPress(string key) {
            switch (key) {
                case "KEY_SPACE":
                    GameRunning.GetInstance(this).player.CreateShot();
                    break;
            }
        }

        private void KeyRelease(string key) {
            if (key.Equals("KEY_A") || key.Equals("KEY_D")) {
                GalagaBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                        GameEventType.PlayerEvent, this, GameRunning.GetInstance(this).player, "stop move", "", ""));
            }
        }

        public void IterateShots() {
            
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }
                
                foreach (var enemyIter in GameRunning.GetInstance(this).enemies) {
                    var collisionData = CollisionDetection.Aabb(shot.shape.AsDynamicShape(), enemyIter.shape);
                    if (collisionData.Collision) {
                        shot.DeleteEntity();
                        enemyIter.DeleteEntity();
                        GameRunning.GetInstance(this).AddExplosion(enemyIter.shape.Position.X,enemyIter.shape.Position.Y,
                            shot.shape.Extent.X+0.1f,shot.shape.Extent.Y+0.1f);
                        GameRunning.GetInstance(this).score.AddPoint(); 
                    }
                }
            }
            
            foreach (Enemy enem in GameRunning.GetInstance(this).enemies) {
                if (!enem.IsDeleted()) {
                    GameRunning.GetInstance(this).newEnemies.Add(enem);
                }
            }
            foreach (PlayerShot elem in playerShots) {
                if (!elem.IsDeleted()) {
                    newPlayerShots.Add(elem);
                }
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                    case "CLOSE_WINDOW":
                        win.CloseWindow();
                        break;                  
                }
            } 
            else if (eventType == GameEventType.InputEvent) {
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