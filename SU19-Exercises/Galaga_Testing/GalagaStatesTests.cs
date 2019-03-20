
using System;
using Galaga_Exercise_3.GalagaStates;
using NUnit.Framework;

namespace Galaga_Testing {
    [TestFixture]
    
    public class GameStateTests {

        
        [SetUp]
        public void SetUp() {
            
        }
        
        [TestCase(StateTransformer.GameStateType.GameRunning, "GAME_RUNNING")]
        [TestCase(StateTransformer.GameStateType.GamePaused, "GAME_PAUSED")]
        [TestCase(StateTransformer.GameStateType.MainMenu, "MAIN_MENU")]
        public void TestStringToState(StateTransformer.GameStateType type, string input) {
            Assert.AreEqual(type, StateTransformer.TransformStringToState(input));
          
        }
        
        [TestCase("GAME_RUNNING", StateTransformer.GameStateType.GameRunning)]
        [TestCase("GAME_PAUSED", StateTransformer.GameStateType.GamePaused)]
        [TestCase("MAIN_MENU", StateTransformer.GameStateType.MainMenu)]
        public void TestStateToString(string str, StateTransformer.GameStateType type) {
            Assert.AreEqual(str, StateTransformer.TransformStateToString(type));
        }

        [Test]
        public void TestNull() {

            Assert.Throws<ArgumentException>((() => StateTransformer.TransformStringToState("STOP")));

        }

    }
}