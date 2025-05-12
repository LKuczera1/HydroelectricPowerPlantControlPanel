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
        Simulation.SimulationManager simulationManager;

        Measurments.MeasurmentsController controller;
        public ControllPanelInterface(HydroelectricPowerPlantControlPanel feedback)
            :base(feedback)
        {
            GUI_Objects = new List<GUI.GUI_Object>();
            simulationManager = new Simulation.SimulationManager();


            GUI_Objects.Add(new GUI.CPBackground());
            GUI_Objects.Add(new GUI.DataDisplayBox(new Vector2f(25,260), simulationManager.WaterLevel, "Poziom wody"));
            GUI_Objects.Add(new GUI.ControllDisplayBox(new Vector2f(450, 700), simulationManager.GateLevelController, "Stopien otwarcia sluzy"));
            GUI_Objects.Add(new GUI.DataDisplayBox(new Vector2f(450, 800), simulationManager.WaterFlowOutPerHour, "Przepływ"));
            GUI_Objects.Add(new GUI.ControllDisplayBox(new Vector2f(25, 25), simulationManager.WaterFlowInController, "Sym: Kontrola doplywu"));
            GUI_Objects.Add(new GUI.DataDisplayBox(new Vector2f(940, 690), simulationManager.GeneratorRPM, "Obroty generatora"));
            GUI_Objects.Add(new GUI.DataDisplayBox(new Vector2f(940, 790), simulationManager.GeneratedPower, "Generowana moc"));




            controller = new Measurments.MeasurmentsController(50,0,100, "%",1);
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
            simulationManager.update(feedback.getDeltaTime);
            for (int i = 0; i < GUI_Objects.Count; i++)
            {
                GUI_Objects[i].update(mousePosition, isLMBPressed ,pressedKeys);
            }
        }

    }
}
