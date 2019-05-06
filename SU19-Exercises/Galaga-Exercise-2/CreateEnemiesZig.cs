using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_2.GalagaEntities.Enemy;

namespace Galaga_Exercise_2 {
    public class CreateEnemiesZig : ISquadron {
        public EntityContainer<Enemy> Enemies { get; set; }
        public int MaxEnemies { get; }
        public Game game;
        public List<Enemy> enemies;

        public CreateEnemiesZig(Game game, List<Enemy> enemies) {
            this.game = game;
            this.enemies = enemies;
        }

        public void CreateEnemies(List<Image> enemyStrides) {
            
             float initValueX = 0.0f;
             float initValueY = 0.7f;

             Enemies = new EntityContainer<Enemy>(8);
             for (int i = 0; i < 8; i++) {
                 initValueX += 0.1f;
                 initValueY += 0.02f;
                 enemies.Add(new Enemy(game, new DynamicShape(new Vec2F(initValueX, initValueY),
                        new Vec2F(0.1f, 0.1f)), new ImageStride(80, enemyStrides)));
                 
                }

                foreach (var elem in enemies) {
                    Enemies.AddStationaryEntity(elem);
                }
            
        }
    }
}