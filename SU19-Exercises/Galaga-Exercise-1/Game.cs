using DIKUArcade;
using DIKUArcade.EventBus;

namespace Galaga_Exercise_1 {
    
    public class Game : IGameEventProcessor<object> {
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            throw new System.NotImplementedException();
        }
    }
}