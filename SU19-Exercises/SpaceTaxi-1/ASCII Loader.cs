using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SpaceTaxi_1 {
    public class AsciiLoader {
        
        private string fileName;
        private string fileLoaded;
        private List<Tuple<string, string>> legendPairs;
        private string Map;
        private Regex regex;
        
        public AsciiLoader(string fileName) {
            this.fileName = fileName;
            
        }

        
        public void ReadText() {
            fileLoaded = File.ReadAllText(GetLevelFilePath(fileName));

            regex = new Regex("\\bPlatforms");
            
            var ppp = regex.Split(fileLoaded);

            Map = ppp[0];
            
            StringReader stringReader = new StringReader(ppp[1].ToString());

            legendPairs = new List<Tuple<string, string>>();
            
            string current = stringReader.ReadLine();
            
            while (current != null) {
//                Console.WriteLine(current);
                if (!current.Contains(":") && !current.Equals("")) {
                    legendPairs.Add(new Tuple<string, string>(new Regex("\\s").Split(current)[0], new Regex("\\s").Split(current)[1]));
                }
                current = stringReader.ReadLine();
            }

//            foreach (var pair in legendPairs) {
//                Console.WriteLine(pair);
//            }
            
            LevelCreator levelCreator = new LevelCreator(legendPairs, Map);
            levelCreator.CreateLevel();
        }
        
        private string GetLevelFilePath(string filename) {
            // Find base path.
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location));

            while (dir.Name != "bin") {
                dir = dir.Parent;
            }
            dir = dir.Parent;

            // Find level file.
            string path = Path.Combine(dir.FullName.ToString(), "Levels", filename);

            if (!File.Exists(path)) {
                throw new FileNotFoundException($"Error: The file \"{path}\" does not exist.");
            }

            return path;
        }
        
    }
}