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
using Galaga_Exercise_2.GalagaEntities.Enemy;

namespace Galaga_Exercise_2 {
    
    public class Game : IGameEventProcessor<object> {

        public ISquadron Isquadron { get; set; }       
        private Window win;
        private GameTimer gameTimer;
        private Score score;
        private GameEventBus<object> eventBus;
        private Player player;
        private List<Image> enemyStrides = new List<Image>();
        private List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> newEnemies = new List<Enemy>();
        public List<PlayerShot> newPlayerShots = new List<PlayerShot>();
        public Image shotImages;
        public List<PlayerShot> playerShots { get; private set; }
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        private string globalMove = "down";
        private CreateEnemiesZig createEnemiesZig;
        private CreateEnemiesLine createEnemiesLine;
        private CreateEnemiesSpot createEnemiesSpot;
        private NoMove noMove;
        private ZigZagDown zigZagDown;
        private MoveDown moveDown;
        
        public Game() {               
            win = new Window("test" ,500, 500);          
            score = new Score(new Vec2F(0.0f,0.0f), new Vec2F(0.2f,0.2f));           
            gameTimer = new GameTimer(60,60);
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new List<Enemy>();             
            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f),new Vec2F(0.1f, 0.1f) ),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            moveDown = new MoveDown();
            noMove = new NoMove();
            zigZagDown = new ZigZagDown();
            createEnemiesLine = new CreateEnemiesLine(this, enemies);
            createEnemiesLine.CreateEnemies(enemyStrides);
            shotImages = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
            playerShots = new List<PlayerShot>();            
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
                GameEventType.PlayerEvent
            });           
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, player);           
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(20);          
        }
        
        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();                    
                    player.Move();                    
                    eventBus.ProcessEvents();                    
                    IterateShots();
                    enemies = newEnemies;
                    newEnemies = new List<Enemy>();
                    playerShots = newPlayerShots;
                    newPlayerShots = new List<PlayerShot>();
                }
                if (gameTimer.ShouldRender()) {
                    win.Clear();                   
                    player.Entity.RenderEntity();            
                    foreach (Enemy element in enemies) {
                        element.RenderEntity(); 
                    }
                    foreach (var elem in playerShots) {
                        elem.RenderEntity();
                    }
                    
                    explosions.RenderAnimations();
                    score.RenderScore();
                    win.SwapBuffers();
                    score.RenderScore();
                    bool allDead = true;
                    bool belowScreen = true;
                    
                    foreach (var iter in enemies) {
                        if (!iter.IsDeleted()) {
                            allDead = false;
                        }

                        if (iter.shape.Position.Y > -0.2f) {
                            belowScreen = false;
                        }
                    }

                    if (allDead || belowScreen)  {                       
                        if (globalMove.Equals("down")) {
                            createEnemiesSpot = new CreateEnemiesSpot(this, enemies);
                            createEnemiesSpot.CreateEnemies(enemyStrides);
                            globalMove = "zigzag";
                        } else if (globalMove.Equals("zigzag")) {
                            createEnemiesZig = new CreateEnemiesZig(this, enemies);
                            createEnemiesZig.CreateEnemies(enemyStrides);
                            globalMove = "nomove";
                        }                        

                    } 
                    else {
                        MoveFunction(globalMove);                        
                    }                   
                }

                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        public void MoveFunction(string moveFunc) {
            switch (moveFunc) {
                case "down":
                    moveDown.MoveEnemies(createEnemiesLine.Enemies);
                    break;
                case "zigzag":
                    zigZagDown.MoveEnemies(createEnemiesSpot.Enemies);
                    break;
                case "nomove":
                    noMove.MoveEnemies(createEnemiesZig.Enemies);
                    break;
            }
        }

        private void KeyPress(string key) {
            switch (key) {
                case "KEY_ESCAPE":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                    break;
                case "KEY_A":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                            GameEventType.PlayerEvent, this, player, "move left", "", ""));
                    break;
                case "KEY_D":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                            GameEventType.PlayerEvent, this, player, "move right", "", ""));
                    break;
                case "KEY_SPACE":
                    player.CreateShot();
                    break;
            }
        } 

        private void KeyRelease(string key) {
            if (key.Equals("KEY_A") || key.Equals("KEY_D")) {
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                        GameEventType.PlayerEvent, this, player, "stop move", "", ""));
            }
        }

        private void IterateShots() {
            
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }
                foreach (var enemyIter in enemies) {
                    var collisionData = CollisionDetection.Aabb(shot.shape.AsDynamicShape(), enemyIter.shape);
                    if (collisionData.Collision) {
                        shot.DeleteEntity();
                        enemyIter.DeleteEntity();
                        AddExplosion(enemyIter.shape.Position.X,enemyIter.shape.Position.Y,
                            shot.shape.Extent.X+0.1f,shot.shape.Extent.Y+0.1f);
                        score.AddPoint(); 
                    }
                }
            }
            foreach (Enemy enem in enemies) {
                if (!enem.IsDeleted()) {
                    newEnemies.Add(enem);
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

        private void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX,posY,extentX,extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));             
        }
    }
}