using System;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Tests {
    [TestFixture]
    public class Tests {
        private Game game; 
        [SetUp]
        public void InitiateGame() {
            game = new Game();
        }
        
        [TestCase("short-n-sweet.txt")]
        [TestCase("the-beach.txt")]
        public void FirstTest(string fileName) {
            game.CreateLevel(fileName);
            Assert.That(game.currentLevel,Is.InstanceOf<Level>());
        }
    }
}