using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Threading;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;

namespace Galaga_Exercise_3.GalagaStates{
    public class GameRunning : IGameState , ISquadron, IMovementStrategy {

        public static GameRunning instance = null;
        public Player player;
        private List<Image> enemyStrides = new List<Image>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> newEnemies = new List<Enemy>();
        public ISquadron Isquadron { get; set; }
        
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        public Score score;      
        private string globalMove = "down";
        private Game game;
        
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
                player = new Player(game, 
                new DynamicShape(new Vec2F(0.45f, 0.1f),new Vec2F(0.1f, 0.1f) ),
                new Image(Path.Combine("Assets", "Images", "Player.png")));            
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));          
            enemies = new List<Enemy>();             
            score = new Score(new Vec2F(0.0f,0.0f), new Vec2F(0.2f,0.2f));           
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(20);           
            CreateEnemies(enemyStrides);          
            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);
        }

        public void UpdateGameLogic() {
            player.Move();                    
            game.IterateShots();
            enemies = newEnemies;
            newEnemies = new List<Enemy>();
        }

        public void RenderState() {
            player.Entity.RenderEntity();            
            
            foreach (Enemy element in enemies) {
                element.RenderEntity(); 
            }
            
            explosions.RenderAnimations();
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
                Enemies.ClearContainer();

                if (globalMove.Equals("down")) {
                    CreateEnemiesSpot(enemyStrides);
                    globalMove = "zigzag";
                } else if (globalMove.Equals("zigzag")) {
                    CreateEnemiesZig(enemyStrides);
                    globalMove = "nomove";
                }
                        

            } else {
                MoveFunction(globalMove);                        
            } 
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        case "KEY_ESCAPE":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_PAUSED", ""));
                            break;
                        case "KEY_A":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.PlayerEvent, this, player, "move left", "", ""));

                            break;
                        case "KEY_D":
                            GalagaBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.PlayerEvent, this, player, "move right", "", ""));

                            break;
                    }
                    break;
            }
        }

        public EntityContainer<Enemy> Enemies { get; set; }
        public int MaxEnemies { get; }
        public void CreateEnemies(List<Image> enemyStrides) {

            
            float initValue = 0.0f;
            Enemies = new EntityContainer<Enemy>(8);
            
            for (int i = 0; i < 8; i++) {
                initValue += 0.1f;
                enemies.Add(new Enemy(new DynamicShape(new Vec2F(initValue, 0.8f),
                    new Vec2F(0.1f, 0.1f)), new ImageStride(80, enemyStrides) ));
                
            }

            foreach (var elem in enemies) {
                Enemies.AddStationaryEntity(elem);
            }        
        }

        public void CreateEnemiesZig(List<Image> enemyStrides) {
            float initValueX = 0.0f;
            float initValueY = 0.7f;

            Enemies = new EntityContainer<Enemy>(8);
            
            for (int i = 0; i < 8; i++) {
                initValueX += 0.1f;
                initValueY += 0.02f;
                enemies.Add(new Enemy(new DynamicShape(new Vec2F(initValueX, initValueY),
                    new Vec2F(0.1f, 0.1f)), new ImageStride(80, enemyStrides) ));    
            }

            foreach (var elem in enemies) {
                Enemies.AddStationaryEntity(elem);
            }
        }

        public void CreateEnemiesSpot(List<Image> enemyStrides) {
            float initValue = 0.8f;
            Enemies = new EntityContainer<Enemy>(8);
            
            for (int i = 0; i < 8; i++) {
                enemies.Add(new Enemy(new DynamicShape(new Vec2F(initValue, 0.9f),
                    new Vec2F(0.1f, 0.1f)), new ImageStride(80, enemyStrides) ));    
            }

            foreach (var elem in enemies) {
                Enemies.AddStationaryEntity(elem);
            }
        }

        public void MoveEnemy(Enemy enemy) {
            try { 
            
                float newY = 0.0f;
                float newX = 0.0f;
            
                newY = enemy.shape.Position.Y - 0.0003f;
                newX = (float) (0.8f +
                                0.05f * Math.Sin((2 * Math.PI) * (0.9f - newY) / 0.045f));

                enemy.shape.Position = new Vec2F(newX, newY);
                
            } catch (NullReferenceException e) {
            
            }
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (var enem in enemies) {
                ((Enemy) enem).Shape.MoveY(-0.002f);
            }
        }
        
        public void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX,posY,extentX,extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));             
        }
        
        private void MoveFunction(string moveFunc) {
            switch (moveFunc) {
            case "down":
                Down(Enemies);
                break;
            case "zigzag":
                ZigZagDown(Enemies);
                break;
            case "nomove":
                NoMove();
                break;
            }
        }
        
        private void NoMove() {
            MoveEnemy(null);
        }
        
        private void Down(EntityContainer<Enemy> enem) {
        
            MoveEnemies(enem);
        }

        private void ZigZagDown(EntityContainer<Enemy> enemies) {

            float prevPosY = 0.0f;
            
            foreach (var enem in enemies) {
                if (((Enemy) enem).shape.Position.Y - prevPosY > 0.1f) {
                    MoveEnemy((Enemy) enem);
                    prevPosY = ((Enemy) enem).shape.Position.Y;
                }
            }
        }
    }
}