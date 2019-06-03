using System;
using DIKUArcade.Math;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Tests {
    public class SingletonScoreTests {        
        private SingletonScore singletonScore;
        private Game game;
        private GameRunning gameR;
        
        [SetUp]
        public void InitializeTest() { 
           game = new Game();
           gameR = new GameRunning(game,null);
           singletonScore = new SingletonScore(new Vec2F(0.3f,0.3f), new Vec2F(0.3f,0.3f));         
        }
        
        [Test]
        public void PointAdderTest() {
            gameR.singletonScore.score = 0;
            gameR.singletonScore.PointChanger("Add");
            Assert.That(gameR.singletonScore.score > 0); 
        }

        [Test]
        public void PointResetTest() {
            gameR.singletonScore.PointChanger("Reset");
            Assert.That(gameR.singletonScore.score == 0);
        }

        [TestCase("add")]
        [TestCase("reset")]
        [TestCase("point")]
        public void PointDefaultTest(string value) {
            int scoreStartValue = gameR.singletonScore.score;
            gameR.singletonScore.PointChanger(value);
            int scoreEndValue = gameR.singletonScore.score;
            Assert.That(scoreStartValue == scoreEndValue);
        }
    }
}