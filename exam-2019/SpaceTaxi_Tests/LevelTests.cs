using exam_2019;
using NUnit.Framework;

namespace SpaceTaxi_Tests {
    
    public class LevelTests {

        [Test]
        public void LevelTest1() {
            
            Assert.Throws<System.ArgumentNullException>(() =>
                new Level(null, null, null, null));
        }

    }
}