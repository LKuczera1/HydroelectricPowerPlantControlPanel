using HydroelectricPowerPlantControlPanel.Source.Measurments;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroelectricPowerPlantControlPanel.Source.GUI
{
    public class EnableDisableButton : GUI_Object
    {
        RenderTexture renderTexture;
        Sprite sprite;

        RectangleShape rectangle;
        Vector2f size;
        bool hasButtonBeenPressed;

        Color idleColor, HoverColor, PressedColor;

        RectangleShape informationBox;
        Color informationBoxON, informationBoxOFF;

        bool isEnabled;

        Font font;
        Text text;

        EnableDisableController controller;

        protected event EventHandler Click;

        public EventHandler click
        {
            set
            {
                if (value != null) Click = value;
            }
        }

        public EnableDisableButton(Vector2f position, EnableDisableController controller, string caption)
            : base(position)
        {
            this.position = position;
            size = new Vector2f(250, 75);

            renderTexture = new RenderTexture((uint)size.X, (uint)size.Y);
            sprite = new Sprite();
            sprite.Position = position;

            rectangle = new RectangleShape(size);
            rectangle.OutlineThickness = -2;

            rectangle.FillColor = new Color(200, 200, 200);
            rectangle.OutlineColor = new Color(70, 70, 70);

            idleColor = new Color(200, 200, 200);
            HoverColor = new Color(170, 170, 170);
            PressedColor = new Color(140, 140, 140);

            informationBox = new RectangleShape(new Vector2f(25, 75));

            informationBox.OutlineColor = new Color(70, 70, 70);
            informationBox.OutlineThickness = -2;

            informationBoxOFF = new Color(255, 0, 0);
            informationBoxON = new Color(0, 255, 0);
            informationBox.FillColor = informationBoxOFF;
            isEnabled = false;

            hasButtonBeenPressed = false;

            font = new Font(Directory.GetCurrentDirectory() + "\\resources\\Calibri.ttf");
            text = new Text();
            text.CharacterSize = 22;
            text.Font = font;
            text.DisplayedString = caption;
            text.FillColor = Color.Black;

            text.Origin = new Vector2f((float)Math.Truncate(text.GetGlobalBounds().Width / 2), (float)Math.Truncate(text.GetGlobalBounds().Height / 2));
            text.Position = new Vector2f((float)Math.Truncate(size.X/2)+5, (float)Math.Truncate(size.Y/2)-5);

            this.controller = controller;
        }

        public bool IsMouseOverButton(Vector2i mousePosition)
        {
            if (mousePosition.X > position.X && mousePosition.X < position.X + size.X && mousePosition.Y > position.Y && mousePosition.Y < position.Y + size.Y)
            {
                return true;
            }
            else return false;
        }

        private void disable()
        {
            controller.stepDown();
            informationBox.FillColor = informationBoxOFF;
            isEnabled = false;
        }

        private void enable()
        {
            controller.stepUp();
            informationBox.FillColor = informationBoxON;
            isEnabled = true;
        }

        private bool buttonUpdate(Vector2i mousePosition, bool isLMBPressed)
        {
            if (IsMouseOverButton(mousePosition))
            {
                rectangle.FillColor = HoverColor;
                if (isLMBPressed)
                {
                    rectangle.FillColor = PressedColor;
                    hasButtonBeenPressed = true;

                    return true;
                }
                else if (hasButtonBeenPressed)
                {
                    hasButtonBeenPressed = false;
                    if (!isEnabled)
                    {
                        enable();
                    }
                    else
                    {
                        disable();
                    }
                    if (Click != null) Click.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                rectangle.FillColor = idleColor;
            }
            return false;
        }

        public override void draw(RenderTexture renderTarget)
        {
            renderTexture.Draw(rectangle);
            renderTexture.Draw(informationBox);
            renderTexture.Draw(text);

            renderTexture.Display();
            sprite.Texture = renderTexture.Texture;
            renderTarget.Draw(sprite);
        }

        public override void update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {
            buttonUpdate(mousePosition, isLMBPressed);
        }
    }
}
