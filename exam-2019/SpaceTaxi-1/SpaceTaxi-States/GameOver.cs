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
    public class GameOver : IGameState {

        public static GameOver instance = null;
        private Entity backgroundImage;
        private Text[] menuButtons;
        private int activeMenuButton =0;
        private int maxMenuButtons;
        private Game game;
        private Window win;
        private Customer customer;

        public GameOver() {
            backgroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))); 
            
            menuButtons = new Text[3];           
            menuButtons[0] = new Text("GAME OVER!", new Vec2F(0.035f, 0.4f), new Vec2F(1.1f,0.4f) );
            menuButtons[1] = new Text("Main Menu", new Vec2F(0.35f, 0.1f), new Vec2F(0.5f,0.4f));
            menuButtons[2] = new Text("Quit", new Vec2F(0.35f, 0.0f), new Vec2F(0.4f,0.4f) );                               
            menuButtons[0].SetColor(Color.Yellow);
            menuButtons[1].SetColor(Color.Red);
            menuButtons[2].SetColor(Color.DarkRed);
            activeMenuButton = 1;
        }
        
        public static GameOver GetInstance() {
            return GameOver.instance ?? (GameOver.instance = new GameOver());
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
                            if (activeMenuButton == 2) {
                                menuButtons[1] = new Text("Main Menu", new Vec2F(0.35f, 0.1f), new Vec2F(0.5f,0.4f));
                                menuButtons[2] = new Text("Quit", new Vec2F(0.35f, 0.0f), new Vec2F(0.4f,0.4f) );
                                menuButtons[1].SetColor(Color.Red);
                                menuButtons[2].SetColor(Color.DarkRed);
                                activeMenuButton = 1;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                            }
                            break;
                        case "KEY_DOWN":
                            if (activeMenuButton == 1) {
                                menuButtons[1] = new Text("Main Menu", new Vec2F(0.35f, 0.1f), new Vec2F(0.4f,0.4f));
                                menuButtons[2] = new Text("Quit", new Vec2F(0.35f, 0.0f), new Vec2F(0.5f,0.4f) );
                                menuButtons[1].SetColor(Color.DarkRed);
                                menuButtons[2].SetColor(Color.Red);
                                activeMenuButton = 2;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                            }                                                
                            break;                      
                        case "KEY_ENTER":
                            switch (activeMenuButton) {                                                         
                                case 1:
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "MAIN_MENU", ""));
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
    