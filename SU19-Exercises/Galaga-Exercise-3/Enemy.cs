using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    public class Enemy : Entity {
        private Game game;
        public DynamicShape shape;
        private Vec2F vec2F { get; }

        public Enemy(DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            
            this.shape = shape;
            vec2F = shape.Position;
        }
    }
}
