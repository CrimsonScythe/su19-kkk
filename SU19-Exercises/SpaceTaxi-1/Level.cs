using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class Level {
        private List<Tuple<string, string>> legendPairs;
        private string Map;
        private float posX = -0.025f;
        private float posY = 0.96f;
        public List<Obstacle> obstacles;

        public string levelName;

        public Customer customer;

        
        
        /*
         The Level constructor makes obstacles all the places where there are key legends.
         This makes it based on the ASCII map and the key legends given from the ASCII loader class.
        */
        public Level(List<Tuple<string, string>> legendPairs, string Map, string levelName, Customer customer) {
            this.levelName = levelName;
            this.legendPairs = legendPairs;
            this.Map = Map;
            this.customer = customer;
            obstacles = new List<Obstacle>();           
            StringReader stringReader = new StringReader(Map);                      
            string currentLine = stringReader.ReadLine();            
            StringReader stringReader2 = new StringReader(currentLine);
            int currentChar = stringReader2.Read();
                     
            // two string readers changes position to get through the hole .txt file
            while (currentLine != null) {

      
                
                while (currentChar != -1) {
                    posX += 0.025f;                    
                    foreach (var pair in legendPairs) {                       
                        if (pair.Item1 == System.Convert.ToChar(currentChar).ToString()+")") {
                            // adds an obstacle with a shape (the position and an Image. 
                            obstacles.Add(new Obstacle
                            (new DynamicShape(new Vec2F(posX,posY), new Vec2F(0.025f, 0.0435f)),
                                new Image(GetAssetsFilePath(pair.Item2)),pair.Item2));
                        }
                    }                        
                    currentChar = stringReader2.Read();
                }            
                currentLine = stringReader.ReadLine();
                
                // if the currentLine is not existing the position will be changed.
                if (currentLine != null) {
                    posX = -0.025f;
                    posY -= 0.0435f;
                    stringReader2 = new StringReader(currentLine);
                    currentChar = stringReader2.Read();
                }                
            }

        }
   
        private string GetAssetsFilePath(string filename) {
            // Find base path.
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location));

            while (dir.Name != "bin") {
                dir = dir.Parent;
            }
            dir = dir.Parent;

            // Find level file.
            string path = Path.Combine(dir.FullName.ToString(), "Assets","Images", filename);

            if (!File.Exists(path)) {
                throw new FileNotFoundException($"Error: The file \"{path}\" does not exist.");
            }
            return path;
        }     
    }
}