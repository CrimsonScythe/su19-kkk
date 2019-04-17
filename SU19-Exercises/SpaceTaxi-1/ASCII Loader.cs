using System;
using System.IO;

namespace SpaceTaxi_1 {
    public class AsciiLoader {
        
        private string fileName;
        private string fileLoaded;
//        private string defaultPath = @"C:\su19-kkk\SU19-Exercises\SpaceTaxi-1\Levels\";
//        private  legendHolder;
        
        public AsciiLoader(string fileName) {
            this.fileName = fileName;

        }

        public void ReadText() {
            fileLoaded = File.ReadAllText(GetLevelFilePath(fileName));

//            Console.WriteLine(Path.GetDirectoryName());
//            Console.WriteLine("df");
            Console.WriteLine(fileLoaded);
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