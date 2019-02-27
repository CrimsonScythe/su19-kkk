using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : Entity {

        private Game game;
        private Shape shape;
        
        
        public Player(Game game, Shape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
            this.shape = shape;
        }

        public void Direction(Vec2F vec2F) {
            //code below makes it more generic?
            this.shape.AsDynamicShape().ChangeDirection(vec2F);
            // OR
//            this.shape.ChangeDirection(vec2F);
        }

        public void Move() {
            //moving right
            if (shape.AsDynamicShape().Direction.X > 0.0f && shape.Position.X < 0.90f) {
                shape.Move();
            }
            //moving left
            if (shape.AsDynamicShape().Direction.X < 0.0f && shape.Position.X > 0) {
                shape.Move();
            }
        }
        
    }
}