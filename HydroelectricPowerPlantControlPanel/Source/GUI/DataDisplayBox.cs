using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace HydroelectricPowerPlantControlPanel.Source.GUI
{
    public class DataDisplayBox : GUI_Object
    {
        RectangleShape rectangle;
        Font font;
        Text text;

        private Measurments.MeasurementCollector collector;
        public DataDisplayBox(Vector2f position, Measurments.MeasurementCollector collector)
            :base(position)
        {
            this.position = position;
            rectangle = new RectangleShape(new Vector2f(200, 50));
            rectangle.OutlineThickness = -2;

            rectangle.FillColor = new Color(200, 200, 200);
            rectangle.OutlineColor = new Color(70, 70, 70);
            rectangle.Position = this.position;

            font = new Font(Directory.GetCurrentDirectory() + "\\resources\\Calibri.ttf");
            text = new Text();
            text.Font = font;
            text.FillColor = Color.Black;
            text.Position = new Vector2f(2, 5);

            this.collector = collector;
        }

        public override void draw(RenderTexture renderTarget)
        {
            renderTarget.Draw(rectangle);
            renderTarget.Draw(text);
        }

        public override void update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {
            text.DisplayedString = collector.Value.ToString();
        }
    }
}
