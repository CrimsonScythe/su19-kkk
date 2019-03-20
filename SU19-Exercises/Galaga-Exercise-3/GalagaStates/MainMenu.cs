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
            throw new System.NotImplementedException();
        }

        public void RenderState() {


            backgroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(0.5f, 0.5f)),
                new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
            
            backgroundImage.RenderEntity();

            menuButtons = new Text[2];
            
            menuButtons[0] = new Text("New Game", new Vec2F(0.5f, 0.5f), new Vec2F(0.1f,0.1f) );
            menuButtons[1] = new Text("Quit", new Vec2F(0.5f, 0.5f), new Vec2F(0.05f,0.05f) );
            
            menuButtons[0].SetColor(Color.Red);
            menuButtons[1].SetColor(Color.White);
            
            activeMenuButton = 0;


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
                            }
                            break;
                        case "KEY_DOWN":
                            if (activeMenuButton == 0) {
                                menuButtons[0].SetColor(Color.White);
                                menuButtons[1].SetColor(Color.Red);
                                activeMenuButton = 1;
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