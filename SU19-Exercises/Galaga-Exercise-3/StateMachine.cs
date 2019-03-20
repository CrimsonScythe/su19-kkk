using DIKUArcade.EventBus;
using DIKUArcade.State;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3 {
    public class StateMachine : IGameEventProcessor<object> {
        public  IGameState ActiveState { get; private set; }

        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);

            ActiveState = StateTransformer.GameStateType.MainMenu;
        }

        private void SwitchState(StateTransformer.GameStateType stateType) {
            switch (stateType) {
                
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            throw new System.NotImplementedException();
        }
    }
}