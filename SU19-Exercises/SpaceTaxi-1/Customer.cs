using System.Drawing;
using System.IO;
using System.Xml;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Customer {

        private string name;
        private int spawntime;
        private string spawnplatform;
        private string landplatform;
        private int droptime;
        private int droppoints;
        private readonly DIKUArcade.Graphics.Image image1;
        private DynamicShape shape;
        private Entity entity;
        
        public Customer(string name, int spawntime, string spawnplatform, string landplatform, int droptime, int droppoints) {
            this.name = name;
            this.spawntime = spawntime;
            this.spawnplatform = spawnplatform;
            this.landplatform = landplatform;
            this.droptime = droptime;
            this.droppoints = droppoints;
            
            shape = new DynamicShape(new Vec2F(), new Vec2F());
            image1 = new DIKUArcade.Graphics.Image(Path.Combine("Assets","Images", "CustomerStandLeft.png"));
            entity = new Entity(shape, image1);

        }
        public void RenderCustomer() {
            entity.RenderEntity();
        }
    } 
}