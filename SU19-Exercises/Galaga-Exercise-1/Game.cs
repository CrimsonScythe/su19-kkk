using System;
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

        private Player player;

        public Game() {
            
            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f),new Vec2F(0.1f, 0.1f) ),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            win = new Window("test" ,500, 500);
            gameTimer = new GameTimer(60,60);
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    //update game logic here
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    //render gameplay entites here
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
//                player.RenderEntity();
            }
        }

        public void KeyPress(string key) {
            throw new NotImplementedException();
        }

        public void KeyRelease(string key) {
            throw new NotImplementedException();
        }
        

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            throw new NotImplementedException();
        }
    }
}