using System;
using System.IO;

namespace SpaceTaxi_1 {
    public class AsciiLoader {
        
        private string fileName;
        private string fileLoaded;
        private string defaultPath = @"C:\su19-kkk\SU19-Exercises\SpaceTaxi-1\Levels\";
        private string legendHolder;
        
        public AsciiLoader(string fileName) {
            this.fileName = defaultPath + fileName;
            
        }

        public void ReadText() {
            fileLoaded = File.ReadAllText(this.fileName);

            
//            Console.WriteLine("df");
//            Console.WriteLine(fileLoaded);
        }
        
    }
}