﻿namespace SpaceTaxi_1 {
    internal class Program {
        public static void Main(string[] args) {
            
            var game = new Game();
            game.CreateLevel("the-beach.txt");
            game.GameLoop();
            
        }
    }
}