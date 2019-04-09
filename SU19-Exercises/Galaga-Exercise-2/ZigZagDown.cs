using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Exercise_2.GalagaEntities.Enemy;

namespace Galaga_Exercise_2 {
    public class ZigZagDown : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) {       
                float newY = 0.0f;
                float newX = 0.0f;           
                newY = enemy.shape.Position.Y - 0.0003f;
                newX = (float) (0.8f +
                    0.05f * Math.Sin((2 * Math.PI) * (0.9f - newY) / 0.045f));
                enemy.shape.Position = new Vec2F(newX, newY);          
        }         
        
        public void MoveEnemies(EntityContainer<Enemy> enemies) {
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