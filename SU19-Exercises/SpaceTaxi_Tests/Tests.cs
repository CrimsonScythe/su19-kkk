using System;
using System.IO;
using System.Security.Policy;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Test {
    [TestFixture]
    public class Tests {
        private Game game; 
        [SetUp]
        public void InitiateGame() {
            game = new Game();
        }
        
        [TestCase("short-n-sweet.txt")]
        public void LoadFileNameTest(string fileName) {
            game.CreateLevel(fileName);
            Assert.That(game.currentLevel,Is.InstanceOf<Level>());
        }

        [TestCase("")]
        [TestCase("short-beach.txt")]
        [TestCase("the-n-sweet.txt")]
        public void TestNull(string fileName) {
            Assert.Throws<FileNotFoundException>((() =>  game.CreateLevel(fileName)));
            
        }      
    }
}