using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;

namespace SpaceTaxi_1
{
    public class GameRunning : IGameState
    {

        public static GameRunning instance = null;
        public Player player;
        private List<Image> enemyStrides = new List<Image>();

        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        private string globalMove = "down";
        private Game game;

        private GameRunning(Game game)
        {
            this.game = game;
        }

        public static GameRunning GetInstance(Game gm)
        {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning(gm));
        }

        public void GameLoop()
        {
        }

        public void InitializeGameState()
        {
            throw new NotImplementedException();
        }

        public void UpdateGameLogic()
        {
            throw new NotImplementedException();
        }

        public void RenderState()
        {
            throw new NotImplementedException();
        }


        public void HandleKeyEvent(string keyValue, string keyAction)
        {
            switch (keyAction)
            {
                case "KEY_PRESS":
                    switch (keyValue)
                    {
                        case "KEY_ESCAPE":
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.GameStateEvent,
                                    this,
                                    "CHANGE_STATE",
                                    "GAME_PAUSED", ""));
                            break;
                        case "KEY_A":
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.PlayerEvent, this, player, "move left", "", ""));

                            break;
                        case "KEY_D":
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                                    GameEventType.PlayerEvent, this, player, "move right", "", ""));

                            break;
                    }

                    break;
            }
        }
    }
}
   