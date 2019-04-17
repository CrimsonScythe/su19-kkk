namespace SpaceTaxi_1 {
    internal class Program {
        public static void Main(string[] args) {
            
            AsciiLoader asciiLoader = new AsciiLoader("short-n-sweet.txt");
            asciiLoader.ReadText();
            
            var game = new Game();
            game.GameLoop();
            
        }
    }
}