using System;
using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace Galaga_Exercise_3.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public  IGameState ActiveState { get; private set; }

        private Game game;

        public StateMachine(Game game) {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            this.game = game;
            
            ActiveState = MainMenu.GetInstance();

        }

        private void SwitchState(StateTransformer.GameStateType stateType) {
            switch (stateType) {
            
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    ActiveState.HandleKeyEvent(gameEvent.Message,gameEvent.Parameter1);
                    break;
                case "KEY_RELEASE":
                    ActiveState.HandleKeyEvent(gameEvent.Message,gameEvent.Parameter1);
                    break;
                }
            }
        }
    }
}