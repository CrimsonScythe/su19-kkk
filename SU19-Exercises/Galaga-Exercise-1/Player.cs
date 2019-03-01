using System.IO;
using System.Net;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : Entity {

        private Game game;
        private Shape shape;
        private IBaseImage image;
        
        public Player(Game game, Shape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
            this.shape = shape;
            this.image = image;
        }

        public void Direction(Vec2F vec2F) {

            this.shape.AsDynamicShape().ChangeDirection(vec2F);

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

        public void CreateShot() {
            PlayerShot playerShot = new PlayerShot(game, 
                new DynamicShape(new Vec2F(shape.Position.X + 0.05f, shape.Position.Y+0.05f), 
                    new Vec2F(0.008f, 0.027f) ),
                game.shotImages);
            game.playerShots.Add(playerShot);

        }
        
    }
}