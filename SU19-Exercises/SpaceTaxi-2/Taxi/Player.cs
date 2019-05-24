using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_2.Taxi {
    public class Player : IGameEventProcessor<object> {
        private readonly Image taxiBoosterOffImageLeft;
        private readonly Image taxiBoosterOffImageRight;
        private readonly ImageStride taxiBoosterOnImageUp;
        private readonly ImageStride taxiBoosterOnImageLeft;
        private readonly ImageStride taxiBoosterOnImageRight;
        private readonly ImageStride taxiBoosterOnImageUpRight;
        private readonly ImageStride taxiBoosterOnImageUpLeft;
        public DynamicShape shape;
        private Orientation taxiOrientation;
        public Vec2F thrust = new Vec2F(0f, 0f);
        private bool isUp = false;

        public Player() {
           
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            taxiBoosterOffImageLeft =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
            taxiBoosterOffImageRight =
                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));
            taxiBoosterOnImageLeft =
                new ImageStride(80,
                    ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png")));
            taxiBoosterOnImageRight =
                new ImageStride(80,
                    ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png")));
            taxiBoosterOnImageUp =
                new ImageStride(80,
                    ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png")));
            taxiBoosterOnImageUpRight =
                new ImageStride(80,
                    ImageStride.CreateStrides(2,
                        Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png")));
            taxiBoosterOnImageUpLeft =
                new ImageStride(80,
                    ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png")));
            Entity = new Entity(shape, taxiBoosterOffImageLeft);
            Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.01f, 0.01f));
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
            switch (taxiOrientation) {
                case Orientation.Up:
                    Entity.Image = taxiBoosterOnImageUp;
                    break;
                case Orientation.None:
                    Entity.Image = taxiBoosterOffImageLeft;
                    break;
                case Orientation.Right:
                    Entity.Image = taxiBoosterOnImageRight;
                    break;
                case Orientation.Left:
                    Entity.Image = taxiBoosterOnImageLeft;
                    break;
                case Orientation.UpLeft:
                    Entity.Image = taxiBoosterOnImageUpLeft;
                    break;
                case Orientation.UpRight:
                    Entity.Image = taxiBoosterOnImageUpRight;
                    break;
            }
            Entity.RenderEntity();
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                    case "BOOSTER_UPWARDS":
                        if (taxiOrientation == Orientation.Left) {
                            taxiOrientation = Orientation.UpLeft;
//                            Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(-0.01f, 0.01f));

                        }
                        else if (taxiOrientation == Orientation.Right) {
                            taxiOrientation = Orientation.UpRight;
//                            Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.01f, 0.01f));

                        }
                        else {
//                            Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.01f, 0.01f));

                            taxiOrientation = Orientation.Up;
                        }
                        
//                        Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(Entity.Shape.AsDynamicShape().Direction.X,1));

                        thrust.Y = 0.00001f;
                        break;
                    case "STOP_ACCELERATE_UP":
                        thrust.Y = 0f;
                        taxiOrientation = Orientation.None;
                        
//                        Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(Entity.Shape.AsDynamicShape().Direction.X, -1f));

//                        Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(Entity.Shape.AsDynamicShape().Direction.X,-1));

                        
                        break;
                    case "BOOSTER_TO_LEFT":
                        if (taxiOrientation == Orientation.Up) {
                            taxiOrientation = Orientation.UpLeft;
//                            Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(-0.01f, 0.01f));

                        }
                        else {
//                            Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(-0.01f, Entity.Shape.AsDynamicShape().Direction.Y));

                            taxiOrientation = Orientation.Left;
                        }

//                        Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(-1,0));
                        
                        thrust.X = -0.000005f;
                        break;
                    case "STOP_ACCELERATE_LEFT":
                        taxiOrientation = Orientation.None;
                        thrust.X = 0f;
                        break;
                    case "BOOSTER_TO_RIGHT":
                        if (taxiOrientation == Orientation.Up) {
                            taxiOrientation = Orientation.UpRight;
                        }
                        else {
                            taxiOrientation = Orientation.Right;
                        }
                        
//                        Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0.01f, Entity.Shape.AsDynamicShape().Direction.Y));

//                        Entity.Shape.AsDynamicShape().ChangeDirection(new Vec2F(1,0));

                        thrust.X = 0.000005f;
                        break;
                    case "STOP_ACCELERATE_RIGHT":
                        taxiOrientation = Orientation.None;
                        thrust.X = 0f;
                        break;
                }
            }
        }
    }
}