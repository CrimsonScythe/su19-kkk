using DIKUArcade.Entities;
using Galaga_Exercise_2.GalagaEntities.Enemy;

namespace Galaga_Exercise_2 {
    public class MoveDown : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {
            (enemy).Shape.MoveY(-0.002f);

        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}