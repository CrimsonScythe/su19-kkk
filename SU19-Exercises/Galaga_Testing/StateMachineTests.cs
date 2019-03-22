using System.Collections.Generic;
using System.Configuration;
using DIKUArcade.EventBus;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaStates;
using NUnit.Framework;

namespace Galaga_Testing {
    public class StateMachineTests {
        private StateMachine stateMachine;

        [SetUp]
        public void InitiateStateMachine() {
            
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent,
                GameEventType.WindowEvent, 
                GameEventType.GameStateEvent,
                GameEventType.PlayerEvent
            });
            
            
            Game game = new Game();
            stateMachine = new StateMachine(game);

        }

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        [Test]
        public void TestEventGamePaused() {
            
            GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_PAUSED",""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());

        }

        [Test]
        public void TestEventGameRunning() {
            GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_RUNNING",""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }
        
        [Test]
        public void TestEventMainMenu() {
            GalagaBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "MAIN_MENU",""));
            
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }

    }
}