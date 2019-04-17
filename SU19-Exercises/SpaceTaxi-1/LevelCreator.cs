using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace SpaceTaxi_1 {
    public class LevelCreator {
        private List<Tuple<string, string>> legendPairs;
        private string Map;
        private int posX;
        private int posY;
        
        public LevelCreator(List<Tuple<string, string>> legendPairs, string Map) {
            this.legendPairs = legendPairs;
            this.Map = Map;
        }

        public void CreateLevel() {
            
            StringReader stringReader = new StringReader(Map);
           
            
            string currentLine = stringReader.ReadLine();
            
            StringReader stringReader2 = new StringReader(currentLine);

            int currentChar = stringReader2.Read();
            
            
            while (currentLine != null) {

                while (currentChar != -1) {
                    Console.WriteLine(System.Convert.ToChar(currentChar));
                    currentChar = stringReader2.Read();
                }
                                
                currentLine = stringReader.ReadLine();
            }

        }
    }
}