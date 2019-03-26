using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_2.GalagaEntities.Enemy;

namespace Galaga_Exercise_2 {
    public class CreateEnemiesLine : ISquadron {
        public EntityContainer<Enemy> Enemies { get; set; }
        public int MaxEnemies { get; }
        public Game game;
        public List<Enemy> enemies;

        public CreateEnemiesLine(Game game, List<Enemy> enemies) {
            this.game = game;
            this.enemies = enemies;
        }
        public void CreateEnemies(List<Image> enemyStrides) {

            float initValue = 0.0f;
            Enemies = new EntityContainer<Enemy>(8);
            
            for (int i = 0; i < 8; i++) {
                initValue += 0.1f;
                enemies.Add(new Enemy(game, new DynamicShape(new Vec2F(initValue, 0.8f),
                    new Vec2F(0.1f, 0.1f)), new ImageStride(80, enemyStrides) ));
            }
            foreach (var elem in enemies) {
                Enemies.AddStationaryEntity(elem);
            }    
        }   
    }
}
