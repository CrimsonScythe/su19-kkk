using System.Diagnostics;

namespace SpaceTaxi_1 {
    public class SingletonTimer {
//        public static int timer;

        public Stopwatch stopwatch;
        private SingletonTimer(Stopwatch stopwatch) {
            this.stopwatch = stopwatch;
        }

        private static SingletonTimer instance = null;

        public static SingletonTimer Instance {
            get {
                if (SingletonTimer.instance == null) {
                    SingletonTimer.instance = new SingletonTimer(new Stopwatch());
                }

                return SingletonTimer.instance;
            }
        }
    }
}