using DIKUArcade.EventBus;

namespace SpaceTaxi_opgave9 {
    public class SpaceTaxiBus {
        private static GameEventBus<object> eventBus;

        public static GameEventBus<object> GetBus() {
            return eventBus ?? (eventBus = new GameEventBus<object>()); 
        }
    }
}
       