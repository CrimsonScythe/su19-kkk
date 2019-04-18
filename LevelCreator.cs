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
            Console.WriteLine("ok");
            StringReader stringReader = new StringReader(Map);
           
            
            string currentLine = stringReader.ReadLine();
            
            StringReader stringReader2 = new StringReader(currentLine);

            int currentChar = stringReader2.Read();
            
            
            while (currentLine != null) {

                while (currentChar != -1) {
                    Console.WriteLine(System.Convert.ToChar(currentChar));
                   
                    //if (legendPairs.IndexOf(new Tuple<string, string>(System.Convert.ToString(currentChar, "")))
                    Console.WriteLine(legendPairs[0]);
                    if(legendPairs.Contains(new Tuple<string, string>(System.Convert.ToString(currentChar)+")", "white-square.png")))
                        Console.WriteLine("currentChar works");
                    
                        
                        
                        
                    currentChar = stringReader2.Read();
                }
                                
                currentLine = stringReader.ReadLine();
            }

        }
        
    }
}