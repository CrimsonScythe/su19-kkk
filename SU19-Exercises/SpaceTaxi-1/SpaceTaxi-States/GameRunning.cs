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

//            obstacles = new List<Obstacle>(); 
            player.SetPosition(ChoseLevel.GetInstance().posX,ChoseLevel.GetInstance().posY);
            player.SetExtent(ChoseLevel.GetInstance().extX, ChoseLevel.GetInstance().extY);
            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

            
//            entity = new Entity(new DynamicShape(ChoseLevel.GetInstance().position, new Vec2F(0.1f,0.1f)), 
//                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png")));

            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            CreateLevel(ChoseLevel.GetInstance().filename); 
         
            
        }

        public void UpdateGameLogic() 
        {

//            Console.WriteLine(player.Entity.Shape.AsDynamicShape().Direction);
            
            
            foreach (var obstacle in currentLevel.obstacles) {

                if (CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), obstacle.Shape).Collision) {
                    switch (currentLevel.levelName) {
                        case "short-n-sweet.txt":
                            if (obstacle.fileName != "neptune-square.png") {
                                // EXPLOSION HERE AND RUNNING GAME MUST END
                            } else {
                                game.gravity = new Vec2F(0f,0f);
                            }
                            break;
                        case "the-beach.txt":

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

        public void RenderState()
        {
            
            
            player.RenderPlayer();
            
            
            if (game.gameTimer.CapturedUpdates == 0) {
                game.currentVelocity = (game.gravity + player.thrust) * 1 + game.currentVelocity;
            } else {
                game.currentVelocity = (game.gravity + player.thrust) * game.gameTimer.CapturedUpdates + game.currentVelocity;
            }
                        
                    
            player.Entity.Shape.Move(game.currentVelocity);
                       
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
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.PlayerEvent, this, player, "BOOSTER_UPWARDS", "", ""));
//                            Console.WriteLine("pressed key");
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
                    default:
                        Console.WriteLine("realeased");
                        break;
                    
                    }
                    break;
            }
        }
    }
}
   