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
        public Level currentLevel;
        private bool isOnPlatform = false;
        private string platformName;
        public Vec2F currentVelocity;        
        private CollisionData[] collisiondatas;
        private bool first = true;
        private int time;
        private int elapsedtime;
        private Stopwatch stopwatch;
        public Customer customer { get; private set; }
        private Obstacle spawnPlatform;
        private bool startup = false;
        private SingletonTimer singletonTimer;
        public SingletonScore singletonScore;

//        public Vec2F gravity = new Vec2F(0f, 0f);

        public Vec2F gravity = new Vec2F(0f, -0.000005f);
        
        public GameRunning(Game game, Customer customer) {
            this.game = game;
            if (customer!=null) {
                this.customer = customer;
            }
            InitializeGameState();
        }

        public static GameRunning GetInstance(Game gm, Customer cust) {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning(gm, cust));
        }

        public void GameLoop() {
        }


        public void InitializeGameState() {
            
            CreateLevel(ChoseLevel.GetInstance().filename);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(5);
//            currentVelocity = new Vec2F(-0.000001f, 0f);
            currentVelocity = new Vec2F(-0.000001f, 0f);
            foreach (var customer in currentLevel.cusList) {
                foreach (var obstacle in currentLevel.obstacles) {
                    if (obstacle.symbol.ToString().Equals(customer.spawnplatform)) {
                        spawnPlatform = obstacle;
                        customer.entity.Shape.Position = new Vec2F(obstacle.shape.Position.X,
                            obstacle.shape.Position.Y+0.05f);
                        break;
                    }
                }
            }
            
            player = new Player();
            player.SetPosition(currentLevel.spawnPos.X, currentLevel.spawnPos.Y);
            
            player.SetExtent(ChoseLevel.GetInstance().extX, ChoseLevel.GetInstance().extY);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

            Console.WriteLine(currentLevel.cusList.Count);
            singletonTimer = SingletonTimer.Instance;
            singletonScore = SingletonScore.Instance;           
            collisiondatas = new CollisionData[currentLevel.cusList.Count];

            for (int i = 0; i < currentLevel.cusList.Count; i++) {
                collisiondatas[i] = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                    currentLevel.cusList[i].entity.Shape);
            }
        }

        private List<string> GetPlatformName() {
            var list1 = new List<string>();
            var list2 = new List<string>();
                list2.Add("neptune-square.png");
                list2.Add("ironstone-square.png");
                list2.Add("studio-square.png");
                list2.Add("white-square.png");            
            return list2;
        }
        

        public void UpdateGameLogic() {       
            for (int i = 0; i < currentLevel.cusList.Count; i++) {
                collisiondatas[i] = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                    currentLevel.cusList[i].entity.Shape);
            }

            for (int i = 0; i < currentLevel.cusList.Count; i++) {
                if (collisiondatas[i].Collision) {

                    //if no customer on board then delete (pickup) customer
                    if (customer == null) {
//                        Console.WriteLine("customer picked up");
                        currentLevel.cusList[i].entity.DeleteEntity();
                        customer = currentLevel.cusList[i];
                        singletonTimer.stopwatch.Start();
                    }                   
                }
            }
            foreach (var obstacle in currentLevel.obstacles) {
                var collisionData = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), obstacle.Shape);                
                if (collisionData.Collision) {
                            if (obstacle.fileName.Equals(GetPlatformName()[0]) || obstacle.fileName.Equals(GetPlatformName()[1])
                               || obstacle.fileName.Equals(GetPlatformName()[2]) || obstacle.fileName.Equals(GetPlatformName()[3]))  {
                                 // lands                                
                                if (customer != null) {
                                    if (obstacle.symbol.ToString().Equals(customer.landplatform) ||
                                        (customer.landplatform == "^" && currentLevel.levelName=="short-n-sweet.txt")) {
                                        Console.WriteLine("ADDPOINT");
                                        singletonScore.PointChanger("Add");
                                        singletonTimer.stopwatch.Reset();
                                        customer = null;  
                                        Console.WriteLine(singletonScore.score.ToString());
                                        if (singletonScore.score == 300)
                                        {
                                            SpaceTaxiBus.GetBus().RegisterEvent(
                                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                                    GameEventType.GameStateEvent,
                                                    this,
                                                    "CHANGE_STATE",
                                                    "GAME_WON", ""));                                     
                                        }
                                    }
                                }
     
                                if (currentVelocity.Y < -0.0001f && currentVelocity.Y > -0.0075f) {
                                    isOnPlatform = true;
                                    currentVelocity.Y = 0;
                                    currentVelocity.X = 0;
                                }                               
                                
                            } else {
                                singletonTimer.stopwatch.Reset();
                                ChoseLevel.GetInstance().Customer = null;
                                singletonScore.PointChanger("Reset");
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

                        //if customer has been picked up and has to be dropped off at next level
//                        if (customer!=null) {

                        if (customer==null) {
                            singletonTimer.stopwatch.Reset();
                            ChoseLevel.GetInstance().Customer = null;
                            singletonScore.PointChanger("Reset");
                            //END GAME
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_OVER", "")); 
                        } else {
                        
                        if (customer.entity.IsDeleted()) {
                            //change customer.platformname to remove ^
                            if (customer.landplatform.Contains("^")) {
                                if (customer.landplatform.Length > 1) {
                                    customer.landplatform =
                                        customer.landplatform.Substring(1, 1);    
                                }   
                            }                                                        
                            ChoseLevel.GetInstance().Customer = customer;                            
                        }       
//                        } else {
//                            END GAME
//                            SpaceTaxiBus.GetBus().RegisterEvent(
//                                GameEventFactory<object>.CreateGameEventForAllProcessors(
//                                    GameEventType.GameStateEvent,
//                                    this,
//                                    "CHANGE_STATE",
//                                    "GAME_OVER", ""));
//                        }

                        currentVelocity.Y = 0;
                        currentVelocity.X = 0;
                        isOnPlatform = true;                        
                        GameRunning.instance = null;
                        if (currentLevel.levelName.Equals("short-n-sweet.txt")) {
                            ChoseLevel.GetInstance().filename = "the-beach.txt";
                            ChoseLevel.GetInstance().posX = 0.25f;
                            ChoseLevel.GetInstance().posY = 0.20f;    
                        } else {
                            ChoseLevel.GetInstance().filename = "short-n-sweet.txt";
                            ChoseLevel.GetInstance().posX = 0.45f;
                            ChoseLevel.GetInstance().posY = 0.15f;       
                        }
                        
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
        }

        public void CreateLevel(string fileName) {
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
//            if (game.gameTimer.CapturedUpdates == 0) {
//                currentVelocity = (game.gravity + player.thrust) * 1 + currentVelocity;
//            } else {
                currentVelocity = (gravity + player.thrust) * game.gameTimer.CapturedUpdates + currentVelocity;
                player.Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(currentVelocity.X, currentVelocity.Y));
//                }
//                Console.WriteLine(currentVelocity);
            }

            if (!isOnPlatform) {
                player.Entity.Shape.Move(currentVelocity);
            }
            foreach (var obstacle in currentLevel.obstacles) {
                obstacle.RenderEntity(); 
            }   
            singletonScore.RenderScore();

            if (first) {
                stopwatch = Stopwatch.StartNew();
                first = false;
            }


            if (customer!=null) {
                if (singletonTimer.stopwatch.Elapsed.Seconds > customer.droptime && customer!=null) {
                    singletonTimer.stopwatch.Reset();
                    ChoseLevel.GetInstance().Customer = null;
                    Console.WriteLine("END GAME");
                    //END GAME
                    SpaceTaxiBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,
                            this,
                            "CHANGE_STATE",
                            "GAME_OVER", ""));
                }     
            }

            
            //renders customers in current level
            foreach (var cus in currentLevel.cusList) {
                if (stopwatch.Elapsed.Seconds + (stopwatch.Elapsed.Minutes * 60) >= cus.spawntime) {
                    if (!cus.entity.IsDeleted()) {
                        cus.RenderCustomer();
                    }
                }
            }
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
                            startup = true;
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

