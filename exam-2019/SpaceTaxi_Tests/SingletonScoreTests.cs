using System;
using DIKUArcade.EventBus;
using DIKUArcade.Math;
using exam_2019;
using NUnit.Framework;

namespace SpaceTaxi_Tests {
    public class SingletonScoreTests {        
        private Game game;
        private GameRunning gameR;
        private StateMachine stateMachine;
        
        [SetUp]
        public void InitializeTest() { 
           game = new Game();
           gameR = new GameRunning(game,null);
           gameR.singletonScore = new SingletonScore(new Vec2F(0.3f,0.3f), new Vec2F(0.3f,0.3f));   
           stateMachine = new StateMachine(game);
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

        [TestCase(300)]
        [TestCase(500)]
        [TestCase(700)]
        public void PointToWinGameTest(int pointsToWin) {
            SpaceTaxiBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));
            while (gameR.singletonScore.score != pointsToWin) {
                gameR.singletonScore.PointChanger("Add");
                gameR.UpdateGameLogic();
                gameR.RenderState();
                
            }           
            SpaceTaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameWon>());                        
        }
    }
}