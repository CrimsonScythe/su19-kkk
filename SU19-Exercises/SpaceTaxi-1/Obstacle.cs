using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Obstacle : Entity {
        private Game game;
        private DynamicShape shape;
        private Vec2F vec2F { get; }

        public Obstacle(DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.shape = shape;
            vec2F = shape.Position;
        }
    }
}