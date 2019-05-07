using System;

namespace SpaceTaxi_1
{
    public class StateTransformer {
        
        public enum GameStateType { GameRunning, MainMenu, GamePaused, ChoseLevel }

        public static GameStateType TransformStringToState(string state) {
            switch (state) {
                case "CHOSE_LEVEL" :
                    return GameStateType.ChoseLevel;
                case "GAME_RUNNING" :
                    return GameStateType.GameRunning;
                case "GAME_PAUSED":
                    return GameStateType.GamePaused;
                case "MAIN_MENU":
                    return GameStateType.MainMenu;
                default:
                    throw new ArgumentException();
            }
        }

        public static string TransformStateToString(GameStateType state) {
            switch (state) { 
                case GameStateType.ChoseLevel:
                    return "CHOSE_LEVEL";
                case GameStateType.GameRunning:
                    return "GAME_RUNNING";
                case GameStateType.GamePaused:
                    return "GAME_PAUSED";
                case GameStateType.MainMenu:
                    return "MAIN_MENU";
                default:
                    throw new ArgumentException();
            }
        }
    }
}