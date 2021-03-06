using System.Collections.Generic;
using System.IO;
using System.Net;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_2 {
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