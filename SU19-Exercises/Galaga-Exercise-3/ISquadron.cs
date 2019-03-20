using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga_Exercise_3 {
    public interface ISquadron {
        EntityContainer<Enemy> Enemies { get; set; }
        int MaxEnemies { get; }

        void CreateEnemies(List<Image> enemyStrides);
        void CreateEnemiesZig(List<Image> enemyStrides);
        void CreateEnemiesSpot(List<Image> enemyStrides);
        
    }
}