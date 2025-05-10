using SFML.Graphics;
using SFML.System;
using SFML.Window;

//Klasa bazowa interfejsów

namespace HydroelectricPowerPlantControlPanel.Source.Interfaces
{
    public abstract class Interface
    {
        public HydroelectricPowerPlantControlPanel feedback;
        public Interface(HydroelectricPowerPlantControlPanel feedback)
        {
            this.feedback = feedback;
        }
        public abstract void Render(RenderTexture drawTexture);
        public abstract void Update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys);
    }
}