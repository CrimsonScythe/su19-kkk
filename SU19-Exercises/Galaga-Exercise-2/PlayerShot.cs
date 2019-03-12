using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
    public class PlayerShot : Entity {
        private Game game;
        public Shape shape;
        
        public PlayerShot(Game game, Shape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
            this.shape = shape;
            this.shape.AsDynamicShape().ChangeDirection(new Vec2F(0.0f,0.01f));
        }
    }
}