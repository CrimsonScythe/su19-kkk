using System.Diagnostics;

namespace SpaceTaxi_1 {
    public class SingletonTimer {
//        public static int timer;

        public Stopwatch stopwatch;
        private SingletonTimer(Stopwatch stopwatch) {
            this.stopwatch = stopwatch;
        }

        

        
        
//        public void setCountDown(int timer) {
//            SingletonTimer.timer = timer;
//        }
//
//        public int getTimer() {
//            return SingletonTimer.timer;
//        }


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