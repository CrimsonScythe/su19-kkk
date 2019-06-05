using System.Collections.Generic;
using DIKUArcade.EventBus;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Tests
{
    public class StateMachineTests {
        private StateMachine stateMachine;

        [SetUp]
        public void InitiateStateMachine() {
            
            SpaceTaxiBus.GetBus().InitializeEventBus(new List<GameEventType>() {
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
            
            SpaceTaxiBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_PAUSED",""));
            
            SpaceTaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());

        }

        [Test]
        public void TestEventGameRunning() {
            SpaceTaxiBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_RUNNING",""));           
            SpaceTaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }
        
        [Test]
        public void TestEventMainMenu() {
            SpaceTaxiBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "MAIN_MENU",""));
            
            SpaceTaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        [Test]
        public void TestEventChooseLevel() {
            SpaceTaxiBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "CHOSE_LEVEL", ""));

            SpaceTaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<ChoseLevel>());
        }

        [Test]
        public void TestEventGameOver() {
            SpaceTaxiBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_OVER", ""));

            SpaceTaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameOver>());
        }

        [Test]
        public void TestEventGameWon() {

            SpaceTaxiBus.GetBus().RegisterEvent(GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_WON", ""));

            SpaceTaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameWon>());
        }
    }
}
  