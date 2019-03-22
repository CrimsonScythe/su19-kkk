using System.Configuration;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaStates;
using NUnit.Framework;

namespace Galaga_Testing {
    public class StateMachineTests {
        private StateMachine stateMachine;

        [SetUp]
        public void InitiateStateMachine() {
            // ...
            // ...
        }

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        public void TestEventGamePaused() {
            GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.GameStateEvent,
                this, "CHANGE_STATE","GAME_PAUSED",""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());

        }
        
        

    }
}