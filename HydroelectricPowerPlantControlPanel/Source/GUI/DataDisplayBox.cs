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
        RenderTexture texture;
        Sprite sprite;
        RectangleShape rectangle;
        Font font;
        Text text;

        CaptionBox captionBox;

        string caption;

        private Measurments.MeasurementCollector collector;
        public DataDisplayBox(Vector2f position, Measurments.MeasurementCollector collector, string caption)
            :base(position)
        {
            this.position = position;
            texture = new RenderTexture(200, 75);
            sprite = new Sprite();
            sprite.Position = this.position;
            rectangle = new RectangleShape(new Vector2f(200, 50));
            rectangle.OutlineThickness = -2;
            rectangle.Position = new Vector2f(0, 25);

            rectangle.FillColor = new Color(200, 200, 200);
            rectangle.OutlineColor = new Color(70, 70, 70);

            font = new Font(Directory.GetCurrentDirectory() + "\\resources\\Calibri.ttf");
            text = new Text();
            text.Font = font;
            text.FillColor = Color.Black;
            text.Position = new Vector2f(2, 5+25);
            text.CharacterSize = 28;

            this.collector = collector;

            this.caption = caption;
            captionBox = new CaptionBox(new Vector2f(0,0), caption);
        }

        public override void draw(RenderTexture renderTarget)
        {
            captionBox.draw(texture);


            texture.Draw(rectangle);
            texture.Draw(text);
            texture.Display();

            sprite.Texture = texture.Texture;

            renderTarget.Draw(sprite);
        }

        public override void update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {
            text.DisplayedString = collector.ValueWithUnits;
        }
    }
}
