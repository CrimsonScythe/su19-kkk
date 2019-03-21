using System;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaStates
{
    public class GamePaused : IGameState
    {
        private static GamePaused instance = null;
        private Entity backgroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;

        public static GamePaused GetInstance()
        {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }

        public GamePaused()
        {
            backgroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));

            menuButtons = new Text[2];

            menuButtons[0] = new Text("Continue", new Vec2F(0.35f, 0.2f), new Vec2F(0.4f, 0.4f));
            menuButtons[1] = new Text("Main Menu", new Vec2F(0.35f, 0.1f), new Vec2F(0.4f, 0.4f));

            menuButtons[0].SetColor(Color.Red);
            menuButtons[1].SetColor(Color.White);

            activeMenuButton = 0;
        }

        public void RenderState()
        {
            backgroundImage.RenderEntity();
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction)
        {
            switch (keyAction)
            {
                
                case "KEY_PRESS":
                    switch (keyValue)
                    {
                        case "KEY_UP":
                            if (activeMenuButton == 1)
                            {
                                menuButtons[0].SetColor(Color.Red);
                                menuButtons[1].SetColor(Color.White);
                                activeMenuButton = 0;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                            }

                            break;
                        case "KEY_DOWN":
                            if (activeMenuButton == 0)
                            {
                                menuButtons[0].SetColor(Color.White);
                                menuButtons[1].SetColor(Color.Red);
                                activeMenuButton = 1;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                            }

                            break;
                        case "KEY_ENTER":
                            switch (activeMenuButton)
                            {
                                case 0:                                   
                                    // continue selected
                                    GalagaBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "GAME_RUNNING", ""));
                                    break;
                                case 1:
                                    // back to main menu selected
                                    GalagaBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "MAIN_MENU", ""));
                                    GameRunning.instance = null; 

                                    break;
                            }
                            break;
                    }
                    break;
                case "KEY_RELEASE":
                    break;
            }
        }
    





public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
            throw new System.NotImplementedException();
        }

        public void UpdateGameLogic() {
        }
    }
}