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
    //Klasa bazowa obiektów interfejsu
    public class GUI_Object
    {
        protected Vector2f position;
        public GUI_Object(Vector2f position)
        {
            this.position = position;
        }

        ~GUI_Object()
        {

        }
        public virtual void draw(RenderTexture renderTarget)
        {

        }

        public virtual void update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {

        }
    }
}
