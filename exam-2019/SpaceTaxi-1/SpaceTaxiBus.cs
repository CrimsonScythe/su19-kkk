using DIKUArcade.EventBus;

namespace exam_2019 {
    public class SpaceTaxiBus {
        private static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus() {
            return eventBus ?? (eventBus = new GameEventBus<object>()); 
        }
    }
}
