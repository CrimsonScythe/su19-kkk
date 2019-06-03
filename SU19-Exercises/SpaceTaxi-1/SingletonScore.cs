using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class SingletonScore {
        private int score;
        private Text display;

        private SingletonScore(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
        }

        private static SingletonScore instance = null;

        public static SingletonScore Instance {
            get {
                if (SingletonScore.instance==null) {
                    SingletonScore.instance = new SingletonScore(new Vec2F(0.05f, 0.55f),new Vec2F(0.4f,0.4f) );
                }

                return SingletonScore.instance;
            }
        }

        public void PointChanger(string value) {
            switch (value) {         
            case "Add":
                score += 100;
                break;       
            case "Reset":
                score = 0;
                break;
            default:
                score = score + 0;
                break;
            }           
        }

        public void RenderScore() {
            display.SetText(string.Format(score.ToString()));
            display.SetColor(new Vec3I(255,0,0));
            display.RenderText();
        }
    }
}