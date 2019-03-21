using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;

namespace Galaga_Exercise_3.GalagaStates{
    public class GameRunning : IGameState , ISquadron, IMovementStrategy{

        private static GameRunning instance = null;
        public Player player;
        private List<Image> enemyStrides = new List<Image>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> newEnemies = new List<Enemy>();
//        public List<PlayerShot> newPlayerShots = new List<PlayerShot>();
//        public List<PlayerShot> playerShots { get; private set; }
        public ISquadron Isquadron { get; set; }
        
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        private Score score;
        
        private string globalMove = "down";

        private Game game;
        
        public GameRunning(Game game) {
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
        }

        public void UpdateGameLogic() {
            player.Move();                    
//            IterateShots();
            enemies = newEnemies;
            newEnemies = new List<Enemy>();
//            playerShots = newPlayerShots;
//            newPlayerShots = new List<PlayerShot>();
        }

        public void RenderState() {
//            win.Clear();                   
            player.Entity.RenderEntity();            
            
            foreach (Enemy element in enemies) {
                element.RenderEntity(); 
            }
//            foreach (var elem in playerShots) {
//                elem.RenderEntity();
//            }
                    
            explosions.RenderAnimations();
            score.RenderScore();
//            win.SwapBuffers();
//            score.RenderScore();
                    

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
//            throw new System.NotImplementedException();
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
//                initValue += 0.1f;
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
        
//        private void IterateShots() {
            
//            foreach (var shot in playerShots) {
//                shot.Shape.Move();
//                if (shot.Shape.Position.Y > 1.0f) {
//                    shot.DeleteEntity();
//                }
//                foreach (var enemyIter in enemies) {
//                    var collisionData = CollisionDetection.Aabb(shot.shape.AsDynamicShape(), enemyIter.shape);
//                    if (collisionData.Collision) {
//                        shot.DeleteEntity();
//                        enemyIter.DeleteEntity();
//                        AddExplosion(enemyIter.shape.Position.X,enemyIter.shape.Position.Y,
//                            shot.shape.Extent.X+0.1f,shot.shape.Extent.Y+0.1f);
//                        score.AddPoint(); 
//                    }
//                }
//            }
//            foreach (Enemy enem in enemies) {
//                if (!enem.IsDeleted()) {
//                    newEnemies.Add(enem);
//                }
//            }
//            foreach (PlayerShot elem in playerShots) {
//                if (!elem.IsDeleted()) {
//                    newPlayerShots.Add(elem);
//                }
//            }
//        }
        
        public void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX,posY,extentX,extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));             
        }
        
        public void MoveFunction(string moveFunc) {
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
        
        public void NoMove() {
            MoveEnemy(null);
        }
        
        public void Down(EntityContainer<Enemy> enem) {
        
            MoveEnemies(enem);
        }

        public void ZigZagDown(EntityContainer<Enemy> enemies) {

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