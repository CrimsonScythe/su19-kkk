using System;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    public class Player : IGameEventProcessor<object> {

        public Entity Entity { get; private set; }
        private Game game;
        private Shape shape;
        private IBaseImage image;
    
        
        public Player(Game game, Shape shape, IBaseImage image) {
            this.game = game;
            this.shape = shape;
            this.image = image;       

            Entity = new Entity(shape, image);
        }
        
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            switch (eventType) {
                case GameEventType.PlayerEvent:
                    if (gameEvent.Message.Equals("move left")) {
                        Direction(new Vec2F(-0.01f, 0.0f));
                    } else if (gameEvent.Message.Equals("move right")) {
                        Direction(new Vec2F(0.01f, 0.0f));
                    } else {
                        Direction(new Vec2F(0.0f,0.0f));
                    }
                    
                    break;
            }
        }

        private void Direction(Vec2F vec2F) {

            this.shape.AsDynamicShape().ChangeDirection(vec2F);
            
        }


        public void Move() {
            //moving right

            Console.WriteLine("IT IS: " + shape.AsDynamicShape().Direction.X);
            
            if (this.shape.AsDynamicShape().Direction.X > 0.0f && this.shape.Position.X < 0.90f) {
//                Console.WriteLine("dd");
                this.shape.Move();
            }
            //moving left
            if (this.shape.AsDynamicShape().Direction.X < 0.0f && this.shape.Position.X > 0) {
//                Console.WriteLine("aa");
                this.shape.Move();
            }
        }

        public void CreateShot() {
            PlayerShot playerShot = new PlayerShot(game, 
                new DynamicShape(new Vec2F(this.shape.Position.X + 0.05f, this.shape.Position.Y+0.05f), 
                    new Vec2F(0.008f, 0.027f) ),
                game.shotImages);
            game.playerShots.Add(playerShot);

        }
        
        
    }
}