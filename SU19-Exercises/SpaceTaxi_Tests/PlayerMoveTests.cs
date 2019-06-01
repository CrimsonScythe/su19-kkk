using System;
using System.IO;
using DIKUArcade.EventBus;
using NUnit.Framework;
using SpaceTaxi_1;

namespace SpaceTaxi_Test
{
    [TestFixture]
    public class PlayerMoveTests {
        // sets up a game instance
        private GameRunning gameR;
        private Game game;
        
        [SetUp]
        public void InitiateGame() {
            game = new Game();
            gameR = new GameRunning(game,new Customer(",",1,",",",",4,5));
        }

        // tests that the levels can be loaded
        [Test]
        public void PlayerMoveRight () {
            var x1 = gameR.player.shape.Position.X; 
            Console.WriteLine(x1);
            /* SpaceTaxiBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.PlayerEvent, this, gameR.player, "BOOSTER_TO_RIGHT", "", "")); */
            gameR.HandleKeyEvent("KEY_PRESS","KEY_RIGHT"); 
            gameR.HandleKeyEvent("KEY_RELEASE","KEY_RIGHT");
            var x2 = gameR.player.shape.Position.X;      
            Console.WriteLine(x2);
            Assert.That(x1 < x2);
            
        }    
    }
} 