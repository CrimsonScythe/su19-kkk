using System.IO;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Test
{
    [TestFixture]
    public class ASCIIMapTests {
        // sets up a game instance
        private GameRunning gameR;
        private Game game;
        [SetUp]
        public void InitiateGame() {
            game = new Game();
            gameR = new GameRunning(game,new Customer(",",1,",",",",4,5));
        }

        // tests that the levels can be loaded
        [TestCase("short-n-sweet.txt")]
        [TestCase("the-beach.txt")]
        public void LoadFileNameTest(string fileName) {
            gameR.CreateLevel(fileName);
            Assert.That(gameR.currentLevel,Is.InstanceOf<Level>());
        }

        // tests that wrong fileNames can't be loaded.
        [TestCase("")]
        [TestCase("short-beach.txt")]
        [TestCase("the-n-sweet.txt")]
        public void TestNull(string fileName) {
            Assert.Throws<FileNotFoundException>((() =>  gameR.CreateLevel(fileName)));

            Assert.Throws<FileNotFoundException>((() =>  gameR.CreateLevel(fileName)));           
        }      
    }
} 