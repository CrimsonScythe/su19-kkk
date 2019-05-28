using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using SpaceTaxi_opgave9;

namespace SpaceTaxi_opgave9 {
    public class Obstacle : Entity {
        private Game game;
        public DynamicShape shape;
        public string fileName;
        private Vec2F vec2F { get; }
        /*
         The obstacle reminds a lot like the enemy of the Galaga game.
         The Obstacles constructor is given a location and a picture.
        */
        public Obstacle(DynamicShape shape, IBaseImage image, string fileName)
            : base(shape, image) {
            this.shape = shape;
            vec2F = shape.Position;
            this.fileName = fileName;
        }
    }
}