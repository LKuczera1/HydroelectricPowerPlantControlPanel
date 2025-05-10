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
    public class CPBackground :GUI_Object
    {
        Texture texture;
        Sprite sprite;
        public CPBackground()
            :base(new Vector2f(0,0))
        {
            texture = new Texture(Directory.GetCurrentDirectory() + "\\resources\\ControlPanelBackground.png");
            sprite = new Sprite(texture);
        }

        public override void draw(RenderTexture renderTarget)
        {
            renderTarget.Draw(sprite);
        }

        public override void update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {

        }
    }
}
