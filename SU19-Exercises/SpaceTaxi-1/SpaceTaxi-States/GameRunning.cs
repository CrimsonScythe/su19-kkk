using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
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
        private List<Obstacle> obstacles;


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
            obstacles = new List<Obstacle>(); 
            player = new Player();
            player.SetPosition(0.45f, 0.6f);
            player.SetExtent(0.1f, 0.1f);
            
//            entity = new Entity(new DynamicShape(ChoseLevel.GetInstance().position, new Vec2F(0.1f,0.1f)), 
//                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png")));

            SpaceTaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
            CreateLevel(ChoseLevel.GetInstance().filename); 
            
        }

        public void UpdateGameLogic()
        {
        }

        public void CreateLevel(string fileName)
        {
            AsciiLoader asciiLoader = new AsciiLoader(fileName);
            var txt = asciiLoader.ReadText();
            // currentLevel changes to Item1 and Item2 from the txt variable.      
            currentLevel = new Level(txt.Item1, txt.Item2);
        }

        public void RenderState()
        {
            
            player.RenderPlayer();
//            player.Entity.RenderEntity();
                       
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
   