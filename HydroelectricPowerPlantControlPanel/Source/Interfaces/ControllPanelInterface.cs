using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace HydroelectricPowerPlantControlPanel.Source.Interfaces
{
    public class ControllPanelInterface : Source.Interfaces.Interface
    {
        List<GUI.GUI_Object> GUI_Objects;
        Measurments.MeasurmentsController controller;
        public ControllPanelInterface(HydroelectricPowerPlantControlPanel feedback)
            :base(feedback)
        {
            GUI_Objects = new List<GUI.GUI_Object>();
            GUI_Objects.Add(new GUI.CPBackground());
            controller = new Measurments.MeasurmentsController(50,0,100, "%",1);

            GUI_Objects.Add(new GUI.ControllDisplayBox(new SFML.System.Vector2f(450, 700), controller));
        }

        public override void Render(RenderTexture drawTexture)
        {
            for (int i = 0; i < GUI_Objects.Count; i++)
            {
                GUI_Objects[i].draw(drawTexture);
            }
        }
        public override void Update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {
            for (int i = 0; i < GUI_Objects.Count; i++)
            {
                GUI_Objects[i].update(mousePosition, isLMBPressed ,pressedKeys);
            }
        }

    }
}
