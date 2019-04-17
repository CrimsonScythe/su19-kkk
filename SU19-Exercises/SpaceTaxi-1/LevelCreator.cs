using System;
using System.Collections.Generic;
using System.Data;

namespace SpaceTaxi_1 {
    public class LevelCreator {
        private List<Tuple<string, string>> legendPairs;
        private string Map;
        
        public LevelCreator(List<Tuple<string, string>> legendPairs, string Map) {
            this.legendPairs = legendPairs;
            this.Map = Map;
        }

        public void CreateLevel() {
            
        }
    }
}