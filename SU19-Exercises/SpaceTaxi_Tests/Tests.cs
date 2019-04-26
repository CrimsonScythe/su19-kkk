using System;
using System.IO;
using System.Security.Policy;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Test {
    [TestFixture]
    public class Tests {
        // sets up a game instance
        private Game game; 
        [SetUp]
        public void InitiateGame() {
            game = new Game();
        }
        
        // tests that the levels can be loaded
        [TestCase("short-n-sweet.txt")]
        [TestCase("the-beach.txt")]
        public void LoadFileNameTest(string fileName) {
            game.CreateLevel(fileName);
            Assert.That(game.currentLevel,Is.InstanceOf<Level>());
        }

        // tests that wrong fileNames can't be loaded.
        [TestCase("")]
        [TestCase("short-beach.txt")]
        [TestCase("the-n-sweet.txt")]
        public void TestNull(string fileName) {
            Assert.Throws<FileNotFoundException>((() =>  game.CreateLevel(fileName)));           
        }      
    }
}