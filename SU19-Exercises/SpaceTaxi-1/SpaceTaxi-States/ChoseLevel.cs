using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace SpaceTaxi_1 {
    public class ChoseLevel : IGameState {
        private static ChoseLevel instance = null;
        private Entity backgroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;
        public string filename = "short-n-sweet.txt";
        public float posX = 0.45f;
        public float posY = 0.075f;
        public float extX = 0.1f;
        public float extY = 0.1f;
        public Customer Customer = null;

        public static ChoseLevel GetInstance() {
            return ChoseLevel.instance ?? (ChoseLevel.instance = new ChoseLevel());
        }

        private ChoseLevel() {
            backgroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            menuButtons = new Text[3];
            menuButtons[0] = new Text("Short n Sweet", new Vec2F(0.35f, 0.2f), new Vec2F(0.4f, 0.4f));
            menuButtons[1] = new Text("The Beach", new Vec2F(0.35f, 0.1f), new Vec2F(0.4f, 0.4f));
            menuButtons[2] = new Text("Back", new Vec2F(0.35f, 0.0f), new Vec2F(0.4f, 0.4f));
            menuButtons[0].SetColor(Color.Red);
            menuButtons[1].SetColor(Color.DarkRed);
            menuButtons[2].SetColor(Color.DarkRed);
            activeMenuButton = 0;
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
                                menuButtons[0] = new Text("Short n Sweet", new Vec2F(0.35f, 0.2f), new Vec2F(0.5f,0.4f) );
                                menuButtons[1] = new Text("The Beach", new Vec2F(0.35f, 0.1f), new Vec2F(0.4f,0.4f));
                                menuButtons[0].SetColor(Color.Red);
                                menuButtons[1].SetColor(Color.DarkRed);
                                menuButtons[2].SetColor(Color.DarkRed);
                                activeMenuButton = 0;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[1].RenderText();
                            }
                            if (activeMenuButton == 2) {
                                menuButtons[1] = new Text("The Beach", new Vec2F(0.35f, 0.1f), new Vec2F(0.5f,0.4f));
                                menuButtons[2] = new Text("Back", new Vec2F(0.35f, 0.0f), new Vec2F(0.4f,0.4f) );
                                menuButtons[0].SetColor(Color.DarkRed);
                                menuButtons[1].SetColor(Color.Red);
                                menuButtons[2].SetColor(Color.DarkRed);

                                activeMenuButton = 1;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                            }
                            break;
                        case "KEY_DOWN":
                            if (activeMenuButton == 1)
                            {
                                menuButtons[1] = new Text("The Beach", new Vec2F(0.35f, 0.1f), new Vec2F(0.4f,0.4f));
                                menuButtons[2] = new Text("Back", new Vec2F(0.35f, 0.0f), new Vec2F(0.5f,0.4f) );
                                menuButtons[0].SetColor(Color.DarkRed);
                                menuButtons[1].SetColor(Color.DarkRed);
                                menuButtons[2].SetColor(Color.Red);
                                activeMenuButton = 2;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                            }
                            if (activeMenuButton == 0)
                            {
                                menuButtons[0] = new Text("Short n Sweet", new Vec2F(0.35f, 0.2f), new Vec2F(0.4f,0.4f) );
                                menuButtons[1] = new Text("The Beach", new Vec2F(0.35f, 0.1f), new Vec2F(0.5f,0.4f));
                                menuButtons[0].SetColor(Color.DarkRed);
                                menuButtons[1].SetColor(Color.Red);
                                menuButtons[2].SetColor(Color.DarkRed);
                                activeMenuButton = 1;
                                menuButtons[0].RenderText();
                                menuButtons[1].RenderText();
                                menuButtons[2].RenderText();
                            }

                            break;
                        case "KEY_ENTER":
                            switch (activeMenuButton) {
                                case 0:                                   
                                    // short n sweet chose
                                    GameRunning.instance = null;
                                    filename = "short-n-sweet.txt";
                                    posX = 0.45f;
                                    posY = 0.15f;
                                    extX = 0.1f;
                                    extY = 0.1f;
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "GAME_RUNNING", filename));
                                    break;
                                case 1:
                                    // the beach chose
                                    GameRunning.instance = null;
                                    filename = "the-beach.txt";
                                    posX = 0.25f; 
//                                    posY = 0.162f;
                                    posY = 0.20f;
                                    extX = 0.1f; 
                                    extY = 0.1f;
                                    GameRunning.instance = null;                                   
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "GAME_RUNNING", filename));                                   
                                    break;
                                case 2:
                                    // back chose
                                    GameRunning.instance = null;                                   
                                    SpaceTaxiBus.GetBus().RegisterEvent(
                                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                                            GameEventType.GameStateEvent,
                                            this,
                                            "CHANGE_STATE",
                                            "MAIN_MENU", ""));                                   
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