using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;

namespace Galaga_Exercise_1 {
    
    public class Game : IGameEventProcessor<object> {
        private Window win;
        private GameTimer gameTimer;
        private Score score;
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
        public List<Enemy> newEnemies = new List<Enemy>();
        public List<PlayerShot> newPlayerShots = new List<PlayerShot>();
        public Image shotImages;
        public List<PlayerShot> playerShots { get; private set; }
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        
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
            AddEnemies();            
            shotImages = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
            playerShots = new List<PlayerShot>();           
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,
                GameEventType.WindowEvent, });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(20);
        }

        private void AddEnemies() {
            enemy = new Enemy(this, new DynamicShape(new Vec2F(0.1f, 0.9f),
        new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy);
            enemy2 = new Enemy(this, new DynamicShape(new Vec2F(0.2f, 0.9f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy2);
            enemy3 = new Enemy(this, new DynamicShape(new Vec2F(0.3f, 0.9f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy3);
            enemy4 = new Enemy(this, new DynamicShape(new Vec2F(0.4f, 0.9f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy4); 
            enemy5 = new Enemy(this, new DynamicShape(new Vec2F(0.5f, 0.9f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy5);
            enemy6 = new Enemy(this, new DynamicShape(new Vec2F(0.6f, 0.9f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy6);
            enemy7 = new Enemy(this, new DynamicShape(new Vec2F(0.7f, 0.9f),
                new Vec2F(0.1f, 0.1f)), new ImageStride(80,enemyStrides));
            enemies.Add(enemy7);
            enemy8 = new Enemy(this, new DynamicShape(new Vec2F(0.8f, 0.9f),
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
                    IterateShots();
                    enemies = newEnemies;
                    newEnemies = new List<Enemy>();
                    playerShots = newPlayerShots;
                    newPlayerShots = new List<PlayerShot>();
                }
                if (gameTimer.ShouldRender()) {
                    win.Clear();                   
                    player.RenderEntity();            
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
                }

                if (gameTimer.ShouldReset()) {
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
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
                    player.Direction(new Vec2F(-0.01f, 0.0f));    
                    break;
                
                case "KEY_D":
                    player.Direction(new Vec2F(0.01f, 0.0f));
                    break;
                case "KEY_SPACE":
                    player.CreateShot();
                    break;
            }
        }

        private void KeyRelease(string key) {
            switch (key) {
                case "KEY_A":
                    player.Direction(new Vec2F(0.0f, 0.0f));
                    break;
                case "KEY_D":
                    player.Direction(new Vec2F(0.0f, 0.0f));
                    break;
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