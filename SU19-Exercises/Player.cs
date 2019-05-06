using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Player : IGameEventProcessor<object> {
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private readonly DynamicShape shape;
        private Orientation taxiOrientation;
        public Vec2F thrust = new Vec2F(0f, 0f);

        public Player() {
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            taxiBoosterOffImageRight =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

            Entity = new Entity(shape, taxiBoosterOffImageLeft);
        }

        public Entity Entity { get; }

        public void SetPosition(float x, float y) {
            shape.Position.X = x;
            shape.Position.Y = y;
        }

        public void SetExtent(float width, float height) {
            shape.Extent.X = width;
            shape.Extent.Y = height;
        }

        public void RenderPlayer() {
            //TODO: Next version needs animation. Skipped for clarity.
            Entity.Image = taxiOrientation == Orientation.Left
                ? taxiBoosterOffImageLeft
                : taxiBoosterOffImageRight;
            Entity.RenderEntity();
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "BOOSTER_UPWARDS":
                    thrust.Y = 0.000005f;
                    break;
                case "STOP_ACCELERATE_UP":
                    thrust.Y = 0f;
                    break;
                case "BOOSTER_TO_LEFT":
                    thrust.X = -0.000005f;
                    break;
                case "STOP_ACCELERATE_LEFT":
                    thrust.X = 0f;
                    break;
                case "BOOSTER_TO_RIGHT":
                    thrust.X = 0.000005f;
                    break;
                case "STOP_ACCELERATE_RIGHT":
                    thrust.X = 0f;
                    break;
                    
                // in the future, we will be handling movement here
                }
            }
        }
    }
}