using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Obstacle : Entity {
        private Game game;
        private DynamicShape shape;
        private Vec2F vec2F { get; }
        /*
         The obstacle reminds a lot like the enemy of the Galaga game.
         The Obstacles constructor is given a location and a picture.
        */
        public Obstacle(DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.shape = shape;
            vec2F = shape.Position;
        }
    }
}