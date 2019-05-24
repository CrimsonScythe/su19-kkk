using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace SpaceTaxi_2 {
    public class Customer {

        public string name;
        public int spawntime;
        public string spawnplatform;
        public string landplatform;
        public int droptime;
        public int droppoints;
        private readonly DIKUArcade.Graphics.Image image1;
        private DynamicShape shape;
        public Entity entity;
        

        public Customer(string name, int spawntime, string spawnplatform, string landplatform, int droptime, int droppoints) {
            this.name = name;
            this.spawntime = spawntime;
            this.spawnplatform = spawnplatform;
            this.landplatform = landplatform;
            this.droptime = droptime;
            this.droppoints = droppoints;
            
            shape = new DynamicShape(new Vec2F(0,0), new Vec2F(0.1f,0.1f));
            image1 = new DIKUArcade.Graphics.Image(Path.Combine("Assets","Images", "CustomerStandLeft.png"));
            entity = new Entity(shape, image1);

        }
        public void RenderCustomer() {
            entity.RenderEntity();
        }
    } 
}