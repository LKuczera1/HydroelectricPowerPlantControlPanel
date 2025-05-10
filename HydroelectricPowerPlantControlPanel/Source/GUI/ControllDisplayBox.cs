using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HydroelectricPowerPlantControlPanel.Source.Measurments;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace HydroelectricPowerPlantControlPanel.Source.GUI
{
    internal class ControllDisplayBox :GUI_Object
    {
        RenderTexture renderTexture;
        Sprite sprite;
        GUI.DataDisplayBox dataDisplayBox;
        GUI.UpArrowButton upArrow;
        GUI.DownArrowButton downArrow;
        Vector2i size;
        MeasurmentsController controller;
        public ControllDisplayBox(Vector2f position, Measurments.MeasurmentsController controller)
            :base(position)
        {
            size = new Vector2i(200, 50);
            renderTexture = new RenderTexture((uint)(size.X),(uint)(size.Y));
            sprite = new Sprite();

            dataDisplayBox = new DataDisplayBox(new Vector2f(0,0), controller);
            upArrow = new UpArrowButton(new Vector2f(150, 0));
            downArrow = new DownArrowButton(new Vector2f(150, 25));

            upArrow.click = this.increaseValue;
            downArrow.click = this.decreseValue;

            sprite.Position = position;
            this.controller = controller;
        }

        public void increaseValue(object sender, EventArgs args)
        {
            this.controller.stepUp();
        }

        public void decreseValue(object sender, EventArgs args)
        {
            this.controller.stepDown();
        }

        ~ControllDisplayBox()
        {

        }
        public bool IsMouseOver(Vector2i mousePosition)
        {
            if (mousePosition.X > position.X && mousePosition.X < position.X + size.X && mousePosition.Y > position.Y && mousePosition.Y < position.Y + size.Y)
            {
                return true;
            }
            else return false;
        }

        public override void draw(RenderTexture renderTarget)
        {
            dataDisplayBox.draw(renderTexture);

            upArrow.draw(renderTexture);
            downArrow.draw(renderTexture);

            renderTexture.Display();
            sprite.Texture = renderTexture.Texture;
            renderTarget.Draw(sprite);
        }

        public override void update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {
            dataDisplayBox.update(mousePosition - (Vector2i)position, isLMBPressed, pressedKeys);
            upArrow.update(mousePosition - (Vector2i)position, isLMBPressed, pressedKeys);
            downArrow.update(mousePosition - (Vector2i)position, isLMBPressed, pressedKeys);
        }
    }
}
