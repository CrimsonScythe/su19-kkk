using System.Drawing;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace exam_2019 {
    public class MainMenu : IGameState {

        private static MainMenu instance = null;
        private Entity backgroundImage;
        private Text[] menuButtons;
        private int activeMenuButton =0;
        private int maxMenuButtons;
        private Game game;
        private Window win;

        public MainMenu() {
            backgroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            menuButtons = new Text[4];           
            menuButtons[0] = new Text("New Game", new Vec2F(0.35f, 0.1f), new Vec2F(0.5f,0.4f) );
            menuButtons[1] = new Text("Choose Level", new Vec2F(0.35f, 0.0f), new Vec2F(0.4f,0.4f));
            menuButtons[2] = new Text("Quit", new Vec2F(0.35f, -0.1f), new Vec2F(0.4f,0.4f) );
            menuButtons[3] = new Text("SPACE TAXI", new Vec2F(0.03f, 0.4f), new Vec2F(1.2f,0.4f) );
            menuButtons[0].SetColor(Color.Red);
            menuButtons[1].SetColor(Color.DarkRed);
            menuButtons[2].SetColor(Color.DarkRed);
            menuButtons[3].SetColor(Color.Yellow);

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
            menuButtons[3].RenderText();  
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {

            switch (keyAction) {
                case "KEY_PRESS":
                    switch (keyValue) {
                        
                        case "KEY_UP":
                            if (activeMenuButton == 1) {
                                menuButtons[0] = new Text("New Game", new Vec2F(0.35f, 0.1f), new Vec2F(0.5f,0.4f) );
                                menuButtons[1] = new Text("Choose Level", new Vec2F(0.35f, 0.0f), new Vec2F(0.4f,0.4f));
                                menuButtons[0].SetColor(Color.Red);
                                menuButtons[1].SetColor(Color.DarkRed);
                                menuButtons[2].SetColor(Color.DarkRed);
                                activeMenuButton = 0;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                                menuButtons[3].RenderText();
                            }
                            if (activeMenuButton == 2) {
                                menuButtons[1] = new Text("Choose Level", new Vec2F(0.35f, 0.0f), new Vec2F(0.5f,0.4f));
                                menuButtons[2] = new Text("Quit", new Vec2F(0.35f, -0.1f), new Vec2F(0.4f,0.4f) );
                                menuButtons[0].SetColor(Color.DarkRed);
                                menuButtons[1].SetColor(Color.Red);
                                menuButtons[2].SetColor(Color.DarkRed);
                                activeMenuButton = 1;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                                menuButtons[3].RenderText();
                            }
                            break;
                        case "KEY_DOWN":
                            if (activeMenuButton == 1) {
                                menuButtons[1] = new Text("Choose Level", new Vec2F(0.35f, 0.0f), new Vec2F(0.4f,0.4f));
                                menuButtons[2] = new Text("Quit", new Vec2F(0.35f, -0.1f), new Vec2F(0.5f,0.4f) );
                                menuButtons[0].SetColor(Color.DarkRed);
                                menuButtons[1].SetColor(Color.DarkRed);
                                menuButtons[2].SetColor(Color.Red);
                                activeMenuButton = 2;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                                menuButtons[3].RenderText();
                            }
                            if (activeMenuButton == 0) {
                                menuButtons[0] = new Text("New Game", new Vec2F(0.35f, 0.1f), new Vec2F(0.4f,0.4f) );
                                menuButtons[1] = new Text("Choose Level", new Vec2F(0.35f, 0.0f), new Vec2F(0.5f,0.4f));
                                menuButtons[0].SetColor(Color.DarkRed);
                                menuButtons[1].SetColor(Color.Red);
                                menuButtons[2].SetColor(Color.DarkRed);
                                activeMenuButton = 1;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                                menuButtons[3].RenderText();
                            }
                                                 
                            break;
                       
                        case "KEY_ENTER":
                            switch (activeMenuButton) {
                          
                                case 0:                             
                                    // new game button selected  
                                    GameRunning.instance = null;
                                    ChoseLevel.GetInstance().filename = "short-n-sweet.txt";
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "GAME_RUNNING", ""));                                
                                    break;
                                case 1:
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "CHOSE_LEVEL", ""));
                                    break;
                                case 2:
                                    // quit
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.WindowEvent,
                                            this,
                                            "CLOSE_WINDOW",
                                            "", ""));
                                    break;                                
                            }
                            break;
                        case "KEY_ESCAPE" : 
                            SpaceTaxiBus.GetBus().RegisterEvent(
                                GameEventFactory<object>.CreateGameEventForAllProcessors(
                                    GameEventType.WindowEvent,
                                    this,
                                    "CLOSE_WINDOW",
                                    "", ""));
                            break;
                    }
                    break;
                case "KEY_RELEASE":                    
                    break;
            }           
        }
    }
}
  