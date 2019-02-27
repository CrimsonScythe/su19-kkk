using DIKUArcade.Entities;
using DIKUArcade.Graphics;


namespace Galaga_Exercise_1
{
    public class Enemy : Entity
    {
        private Game game;
        private Shape shape;

        public Enemy(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image)
        {
            this.game = game;
            this.shape = shape; 
        }

    }
}

