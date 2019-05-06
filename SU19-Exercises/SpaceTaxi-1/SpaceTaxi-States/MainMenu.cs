using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1
{
    public class MainMenu : IGameState {

        private static MainMenu instance = null;
        private Entity backgroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;
        private Game game; 

        public MainMenu() {
            backgroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            menuButtons = new Text[3];           
            menuButtons[0] = new Text("New Game", new Vec2F(0.35f, 0.2f), new Vec2F(0.4f,0.4f) );
            menuButtons[1] = new Text("Quit", new Vec2F(0.35f, 0.0f), new Vec2F(0.4f,0.4f));
            menuButtons[2] = new Text("Choose Level", new Vec2F(0.35f, 0.1f), new Vec2F(0.4f,0.4f) );
            menuButtons[0].SetColor(Color.Red);
            menuButtons[1].SetColor(Color.White);
            menuButtons[2].SetColor(Color.White);

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
        }

        public void RenderState() {

            backgroundImage.RenderEntity();
            menuButtons[0].RenderText();
            menuButtons[1].RenderText();
            menuButtons[2].RenderText();
     
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {

            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        
                        case "KEY_UP":
                            if (activeMenuButton == 1) {
                                menuButtons[0].SetColor(Color.Red);
                                menuButtons[1].SetColor(Color.White);
                                menuButtons[2].SetColor(Color.White);

                                activeMenuButton = 0;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                            }
                            break;
                        case "KEY_DOWN":
                            if (activeMenuButton == 0)
                            {
                                menuButtons[0].SetColor(Color.White);
                                menuButtons[1].SetColor(Color.White);
                                menuButtons[2].SetColor(Color.Red);
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();    
                                menuButtons[2].RenderText();
                                activeMenuButton = 2;

                            }
                            if (activeMenuButton == 2) {
                                menuButtons[0].SetColor(Color.White);
                                menuButtons[1].SetColor(Color.Red);
                                menuButtons[2].SetColor(Color.White);
                                activeMenuButton = 1;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                            }

                           
                            break;
                        case "KEY_ENTER":
                            switch (activeMenuButton) {
                                case 0:                             
                                    // new game button selected                                    
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "GAME_RUNNING", "new"));                                
                                    break;
                                case 1:
                                    // quit
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.WindowEvent,
                                            this,
                                            "CLOSE_WINDOW",
                                            "", ""));
                                    break;
                                 case 2:
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.WindowEvent,
                                            this,
                                            "CREATE_LEVEL",
                                            "", "")); 
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
  