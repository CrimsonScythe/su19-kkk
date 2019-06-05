using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace exam_2019 {
    public class SingletonScore {
        public int score;
        private Text display;

        // the constructor of the score with a position and extent
        public SingletonScore(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
        }

        private static SingletonScore instance = null;

        // creates the score at the display always at the same position
        public static SingletonScore Instance {
            get {
                if (SingletonScore.instance==null) {
                    SingletonScore.instance = new SingletonScore(new Vec2F(0.05f, 0.55f),new Vec2F(0.4f,0.4f) );
                }
                return SingletonScore.instance;
            }
        }

        /* Adds point to the score if the message is "Add"
        Resets the score if the message is "Reset" */
        public void PointChanger(string value) {
            switch (value) {         
            case "Add":
                // could had been the information from the player in the .txt (but the information is always 100)
                score += 100;
                break;       
            case "Reset":
                score = 0;
                break;
            // nothing happens if the message is wrong
            default:
                score = score + 0;
                break;
            }           
        }

        // will render the score with a colour on the display
        public void RenderScore() {
            display.SetText(string.Format(score.ToString()));
            display.SetColor(new Vec3I(255,0,0));
            display.RenderText();
        }
    }
}