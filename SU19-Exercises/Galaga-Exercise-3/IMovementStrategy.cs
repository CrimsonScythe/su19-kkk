using DIKUArcade.Entities;
using Galaga_Exercise_3;

namespace Galaga_Exercise_3 {
    public interface IMovementStrategy {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
    }
}
