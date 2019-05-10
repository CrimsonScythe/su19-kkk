using System;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Test {
    public class SpaceTaxiStatesTests {
  
        [SetUp]
        public void SetUp() {           
        }
        
        [TestCase(StateTransformer.GameStateType.GameRunning, "GAME_RUNNING")]
        [TestCase(StateTransformer.GameStateType.GamePaused, "GAME_PAUSED")]
        [TestCase(StateTransformer.GameStateType.MainMenu, "MAIN_MENU")]
        [TestCase(StateTransformer.GameStateType.ChoseLevel, "CHOSE_LEVEL")]
        [TestCase(StateTransformer.GameStateType.GameOver, "GAME_OVER")]

        public void TestStringToState(StateTransformer.GameStateType type, string input) {
            Assert.AreEqual(type, StateTransformer.TransformStringToState(input));
          
        }
        
        [TestCase("GAME_RUNNING", StateTransformer.GameStateType.GameRunning)]
        [TestCase("GAME_PAUSED", StateTransformer.GameStateType.GamePaused)]
        [TestCase("MAIN_MENU", StateTransformer.GameStateType.MainMenu)]
        [TestCase("CHOSE_LEVEL", StateTransformer.GameStateType.ChoseLevel)]
        [TestCase("GAME_OVER", StateTransformer.GameStateType.GameOver)]

        public void TestStateToString(string str, StateTransformer.GameStateType type) {
            Assert.AreEqual(str, StateTransformer.TransformStateToString(type));
        }

        [Test]
        public void TestNull() {

            Assert.Throws<ArgumentException>((() => StateTransformer.TransformStringToState("STOP")));

        }

    }
}
    