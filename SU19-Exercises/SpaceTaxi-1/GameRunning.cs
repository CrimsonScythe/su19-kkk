using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Timers;

namespace SpaceTaxi_1 {
    public class GameRunning : IGameState {

        public static GameRunning instance = null;
        public Player player;
        private List<Image> enemyStrides = new List<Image>();
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        private string globalMove = "down";
        private Game game;
        private Level currentLevel;
        private bool isOnPlatform = false;
        private string platformName;
        private Vec2F currentVelocity;
        public Score score;

        private bool first = true;
        private int time;
        private int elapsedtime;

        private Stopwatch stopwatch;
        
        private GameRunning(Game game) {
            this.game = game;
            
            InitializeGameState();
        }

        public static GameRunning GetInstance(Game gm) {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning(gm));
        }

        public void GameLoop() {
        }
        

        public void InitializeGameState() { 
            player = new Player();
            player.SetPosition(ChoseLevel.GetInstance().posX,ChoseLevel.GetInstance().posY);
            player.SetExtent(ChoseLevel.GetInstance().extX, ChoseLevel.GetInstance().extY);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            CreateLevel(ChoseLevel.GetInstance().filename);             
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(5);
            
            score = new Score(new Vec2F(0.05f,0.55f), new Vec2F(0.4f,0.4f));           
            currentVelocity = new Vec2F(0f, 0f);
        }

        private List<string> GetPlatformName() {
            var list1 = new List<string>();   
            switch (currentLevel.levelName) {
            case "short-n-sweet.txt":
                list1.Add("neptune-square.png");
                break;
            case "the-beach.txt":
                list1.Add("ironstone-square.png");
                list1.Add("studio-square.png");
                list1.Add("white-square.png");
                break;
            }
            return list1;
        }
        

        public void UpdateGameLogic() {       
            var collisiondata =
                CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), currentLevel.customer.entity.Shape);

            if (collisiondata.Collision) {
                currentLevel.customer.entity.DeleteEntity();
            }
            foreach (var obstacle in currentLevel.obstacles) {
                var collisionData = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), obstacle.Shape);                
                if (collisionData.Collision) {
                            if (obstacle.fileName.Equals(GetPlatformName()[0]) || obstacle.fileName.Equals(GetPlatformName()[1])
                               || obstacle.fileName.Equals(GetPlatformName()[2]))  {
                                
                                //if collision from below then gameover and explosion
                                if (collisionData.DirectionFactor.Y < 1) {
                                    AddExplosion(player.shape.Position.X,player.shape.Position.Y,
                                        obstacle.shape.Extent.X+0.1f,obstacle.shape.Extent.Y+0.1f);                            
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "GAME_OVER", ""));   
                                }
                                
                                if (currentVelocity.Y < -0.0001f && currentVelocity.Y > -0.0075f) {
                                    isOnPlatform = true;
                                    currentVelocity.Y = 0;
                                    currentVelocity.X = 0;
                                }
                                
                            } else {
                                AddExplosion(player.shape.Position.X,player.shape.Position.Y,
                                    obstacle.shape.Extent.X+0.1f,obstacle.shape.Extent.Y+0.1f);                                
                                SpaceTaxiBus.GetBus().RegisterEvent(
                                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                                        GameEventType.GameStateEvent,
                                        this,
                                        "CHANGE_STATE",
                                        "GAME_OVER", ""));                                 
                            } 
                } else {
                    if (player.shape.Position.Y > 1) {
                        currentVelocity.Y = 0;
                        currentVelocity.X = 0;
                        isOnPlatform = true;                        
                        GameRunning.instance = null;
                        ChoseLevel.GetInstance().filename = "the-beach.txt";
                        ChoseLevel.GetInstance().posX = 0.25f;
                        ChoseLevel.GetInstance().posY = 0.162f;
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", ""));    
                    }
                }               
            }           
        }

        private void CreateLevel(string fileName) {
            AsciiLoader asciiLoader = new AsciiLoader(fileName);
            var txt = asciiLoader.ReadText();
            // currentLevel changes to Item1 and Item2 from the txt variable.      
            currentLevel = new Level(txt.Item1, txt.Item2,fileName , txt.Item3);             
        }

        private void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }

        public void RenderState() {   
            player.RenderPlayer();
            explosions.RenderAnimations();
            if (!isOnPlatform) {               
            if (game.gameTimer.CapturedUpdates == 0) {
                currentVelocity = (game.gravity + player.thrust) * 1 + currentVelocity;
            } else {
                currentVelocity = (game.gravity + player.thrust) * game.gameTimer.CapturedUpdates + currentVelocity;
                }
            }

            if (!isOnPlatform) {
                player.Entity.Shape.Move(currentVelocity);
            }
            foreach (var obstacle in currentLevel.obstacles) {
                obstacle.RenderEntity(); 
            }   
            score.RenderScore();

            if (first) {
                stopwatch = Stopwatch.StartNew();
                first = false;
            }

//            Console.WriteLine(stopwatch.Elapsed.Seconds);

//            Console.WriteLine(currentLevel.customer.spawntime);

            if (stopwatch.Elapsed.Seconds >= currentLevel.customer.spawntime) {
                currentLevel.customer.RenderCustomer();
            }
            
//            Console.WriteLine(game.gameTimer.CapturedUpdates);
                        
                        
        }


        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        case "KEY_ESCAPE":
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_PAUSED", ""));
                            break;
                        
                        case "KEY_UP":
                            
                            isOnPlatform = false;
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.PlayerEvent, this, player, "BOOSTER_UPWARDS", "", ""));
                            break;
                        case "KEY_LEFT":
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.PlayerEvent, this, player, "BOOSTER_TO_LEFT", "", ""));
                            break;
                        case "KEY_RIGHT":
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.PlayerEvent, this, player, "BOOSTER_TO_RIGHT", "", ""));
                            break;
                        default:
                            Console.WriteLine("pressed");
                            break;
                    }

                    break;
                case "KEY_RELEASE":
                    switch (keyValue) {
                    case "KEY_LEFT":
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                GameEventType.PlayerEvent, this, player, "STOP_ACCELERATE_LEFT", "", ""));
                        break;
                    case "KEY_RIGHT":
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                GameEventType.PlayerEvent, this, player, "STOP_ACCELERATE_RIGHT", "", ""));
                        break;
                    case "KEY_UP":
                        SpaceTaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                GameEventType.PlayerEvent, this,player, "STOP_ACCELERATE_UP", "", ""));
                        break;
                   
                    }
                    break;
            }
        }
    }
}


   