using System;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaStates {
    public class MainMenu : IGameState {

        private static MainMenu instance = null;

        private Entity backgroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;


        public MainMenu() {
            backgroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));

            menuButtons = new Text[2];
            
            menuButtons[0] = new Text("New Game", new Vec2F(0.5f, 0.5f), new Vec2F(0.3f,0.3f) );
            menuButtons[1] = new Text("Quit", new Vec2F(0.5f, 0.4f), new Vec2F(0.3f,0.3f) );

            menuButtons[0].SetColor(Color.Red);
            menuButtons[1].SetColor(Color.White);
            
            activeMenuButton = 0;

        }
        
        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }
        
        public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
            throw new System.NotImplementedException();
        }

        public void UpdateGameLogic() {
//            throw new System.NotImplementedException();
        }

        public void RenderState() {

            backgroundImage.RenderEntity();
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
            
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {

            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        case "KEY_UP":
                            if (activeMenuButton == 1) {
                                menuButtons[0].SetColor(Color.Red);
                                menuButtons[1].SetColor(Color.White);
                                activeMenuButton = 0;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                            }
                            break;
                        case "KEY_DOWN":
                            if (activeMenuButton == 0) {
                                menuButtons[0].SetColor(Color.White);
                                menuButtons[1].SetColor(Color.Red);
                                activeMenuButton = 1;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                            }
                            break;
                        case "KEY_ENTER":
                            switch (activeMenuButton) {
                                case 0:
                                    // new game button selected
                                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                                        GameEventType.GameStateEvent,
                                        this,
                                        "CHANGE_STATE",
                                        "GAME_RUNNING", "");
                                    break;
                                case 1:
                                    // quit
                                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                                        GameEventType.WindowEvent,
                                        this,
                                        "CLOSE_WINDOW",
                                        "", "");
                                    break;
                            }
                            break;
                    }
                    break;
                case "KEY_RELEASE":
                    
                    break;
            }
            
        }
    }
}