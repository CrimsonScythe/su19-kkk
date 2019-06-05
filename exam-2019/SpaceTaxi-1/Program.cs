namespace exam_2019 {
    internal class Program {
        public static void Main(string[] args) {
            var game = new Game();
//            game.CreateLevel("short-n-sweet.txt"); // other Level: "the-beach.txt"
            game.GameLoop();
        }
    }
}