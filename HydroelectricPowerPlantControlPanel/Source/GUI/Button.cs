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
    public abstract class Button : GUI_Object
    {
        RectangleShape rectangle;
        Vector2f size;
        bool hasButtonBeenPressed;

        Color idleColor, HoverColor, PressedColor;

        protected event EventHandler Click;

        Clock clock;
        bool isTimeCounted;
        int changeValueTime;
        bool haveClockMechanizmBeenActivated;
        private const int clockMechanizmWaitingTime = 1500;
        private const int clockActivatedMechanizmTime = 250;

        public EventHandler click
        {
            set
            {
                if (value != null) Click = value;
            }
        }

        public Button(Vector2f position)
            :base(position)
        {
            this.position = position;
            size = new Vector2f(50, 25);
            rectangle = new RectangleShape(size);
            rectangle.OutlineThickness = -2;

            rectangle.FillColor = new Color(200, 200, 200);
            rectangle.OutlineColor = new Color(70, 70, 70);
            rectangle.Position = this.position;

            idleColor = new Color(200,200,200);
            HoverColor = new Color(170, 170, 170);
            PressedColor = new Color(140, 140, 140);

            hasButtonBeenPressed = false;

            clock = new Clock();
            isTimeCounted = false;
            haveClockMechanizmBeenActivated = false;
            changeValueTime = clockMechanizmWaitingTime;
        }

        public bool IsMouseOverButton(Vector2i mousePosition)
        {
            if (mousePosition.X > position.X && mousePosition.X < position.X+size.X && mousePosition.Y > position.Y && mousePosition.Y < position.Y+size.Y)
            {
                return true;
            }
            else return false;
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

                    if(isTimeCounted)
                    {
                        if (clock.ElapsedTime.AsMilliseconds() > changeValueTime)
                        {
                            clock.Restart();
                            Click.Invoke(this, EventArgs.Empty);
                            changeValueTime = clockActivatedMechanizmTime;
                            haveClockMechanizmBeenActivated  =true;
                        }
                    }
                    else
                    {
                        isTimeCounted = true;
                    }
                    return true;
                }
                else if(hasButtonBeenPressed)
                {
                    if(haveClockMechanizmBeenActivated)
                    {
                        isTimeCounted = false;
                        hasButtonBeenPressed = false;
                        changeValueTime = clockMechanizmWaitingTime;
                        haveClockMechanizmBeenActivated = false;
                        return true;
                    }
                    else
                    {
                        hasButtonBeenPressed = false;
                        if (Click != null) Click.Invoke(this, EventArgs.Empty);
                    }
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
            renderTarget.Draw(rectangle);
        }

        public override void update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {
            buttonUpdate(mousePosition, isLMBPressed);
        }
    }
}
