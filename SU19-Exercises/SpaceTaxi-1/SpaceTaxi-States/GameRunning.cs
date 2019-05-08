using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;

namespace SpaceTaxi_1
{
    public class GameRunning : IGameState
    {

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
        
        private GameRunning(Game game)
        {
            this.game = game;
            
            InitializeGameState();
        }

        public static GameRunning GetInstance(Game gm)
        {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning(gm));
        }

        public void GameLoop()
        {
        }
        

        public void InitializeGameState()
        { 
            player = new Player();
            player.SetPosition(ChoseLevel.GetInstance().posX,ChoseLevel.GetInstance().posY);
            player.SetExtent(ChoseLevel.GetInstance().extX, ChoseLevel.GetInstance().extY);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            CreateLevel(ChoseLevel.GetInstance().filename);             
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(5);           
                   
        }

        public void UpdateGameLogic() {

            Console.WriteLine(game.currentVelocity.Y);
            
            foreach (var obstacle in currentLevel.obstacles) {

                var collisionData = CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), obstacle.Shape);
                
                if (collisionData.Collision) {
                    
                    
                    switch (currentLevel.levelName) {
                        case "short-n-sweet.txt":

                            if (obstacle.fileName == "neptune-square.png"
                                || obstacle.fileName == "neptune-lower-left.png"
                                || obstacle.fileName == "neptune-lower-right.png") {

                                if (game.currentVelocity.Y < -0.0001f && game.currentVelocity.Y > -0.0075f) {

                                    isOnPlatform = true;
                                    game.currentVelocity.Y = 0;
                                    game.currentVelocity.X = 0;

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
//                                
                                
                            } 
                            break;
                        case "the-beach.txt":
                            if (obstacle.fileName != "ironstone-square.png") {
                                
                            } else {
                                Console.WriteLine(collisionData.DirectionFactor);
                            }
                            break;
                    }
                    
                } 
                
            }
            
        }

        public void CreateLevel(string fileName)
        {
            AsciiLoader asciiLoader = new AsciiLoader(fileName);
            var txt = asciiLoader.ReadText();
            // currentLevel changes to Item1 and Item2 from the txt variable.      
            currentLevel = new Level(txt.Item1, txt.Item2, fileName);
             
        }

        public void AddExplosion(float posX, float posY,
            float extentX, float extentY)
        {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }

        public void RenderState()
        {                        
            player.RenderPlayer();

            explosions.RenderAnimations();


            if (!isOnPlatform) {
                
            if (game.gameTimer.CapturedUpdates == 0) {
                game.currentVelocity = (game.gravity + player.thrust) * 1 + game.currentVelocity;
            } else {
                game.currentVelocity = (game.gravity + player.thrust) * game.gameTimer.CapturedUpdates + game.currentVelocity;
            }

            }

            if (!isOnPlatform) {
                player.Entity.Shape.Move(game.currentVelocity);
            }
            

            foreach (var obstacle in currentLevel.obstacles) {
                obstacle.RenderEntity(); 
            }          
        }


        public void HandleKeyEvent(string keyValue, string keyAction)
        {
            switch (keyAction)
            {
                case "KEY_PRESS":
                    switch (keyValue)
                    {
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


   