using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DIKUArcade.Entities;

namespace SpaceTaxi_1 {
    public class AsciiLoader {        
        private string fileName;
        private string fileLoaded;
        private List<Tuple<string, string>> legendPairs;
        private string Map;
        private Regex regex;
        private List<Customer> cusList = new List<Customer>();
        private string name;
        private int spawntime;
        private string spawnplatform;
        private string landplatform;
        private int droptime;
        private int droppoints;
        private Customer customer;
        
        // AsciiLoader constructor given a string fileName.
        public AsciiLoader(string fileName) {
            this.fileName = fileName;    
        } 
        
        /*
         Uses regex to create a list of a tuple. The tuple contains two strings.
         The two strings are the ASCII map and the key legends, respectively.                 
        */
        public (List<Tuple<string,string>>, string, List<Customer>) ReadText() {
            fileLoaded = File.ReadAllText(GetLevelFilePath(fileName));
            // regex spilts the file at the string Platforms
            regex = new Regex("\\bPlatforms");            
            var ppp = regex.Split(fileLoaded);
            // the map at ppp element 0
            Map = ppp[0];          
            StringReader stringReader = new StringReader(ppp[1].ToString());
            legendPairs = new List<Tuple<string, string>>();           
            string current = stringReader.ReadLine();
            
            while (current != null) {
              
                if (new Regex("\\bCustomer\\b").IsMatch(current)) {
                    var splitted = new Regex("\\s").Split(current);

                    name = splitted[1];

                    spawntime = Convert.ToInt32(splitted[2]);
                    
                    spawnplatform = splitted[3];

                    landplatform = splitted[4];

                    droptime = Convert.ToInt32(splitted[5]);

                    droppoints = Convert.ToInt32(splitted[6]);
                    cusList.Add(new Customer(name,spawntime,spawnplatform,landplatform,droptime,droppoints));

                }
                
                // checks if the current variable is not empty, a ":" or an empty string
                if (!current.Contains(":") && !current.Equals("")) {
                    /*
                     as long as current is not empty, an empty string or ":" legendPairs will add
                     the current element.
                    */
                    legendPairs.Add(new Tuple<string, string>(new Regex("\\s").Split(current)[0], 
                        new Regex("\\s").Split(current)[1]));
                }
                current = stringReader.ReadLine();
            }
            
            return (legendPairs, Map, cusList);
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