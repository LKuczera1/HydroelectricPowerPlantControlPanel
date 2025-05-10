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
    public class DownArrowButton : Button
    {
        Texture texture;
        Sprite sprite;
        public DownArrowButton(Vector2f position)
            : base(position)
        {
            texture = new Texture(Directory.GetCurrentDirectory() + "\\resources\\ButtonArrow.png");
            sprite = new Sprite(texture);
            sprite.Origin = new Vector2f(sprite.TextureRect.Width / 2, sprite.TextureRect.Height / 2);
            sprite.Rotation = 180;
            sprite.Position = position+sprite.Origin;
        }

        public override void draw(RenderTexture renderTarget)
        {
            base.draw(renderTarget);
            renderTarget.Draw(sprite);
        }

        public override void update(Vector2i mousePosition, bool isLMBPressed, List<Keyboard.Key> pressedKeys)
        {
            base.update(mousePosition, isLMBPressed, pressedKeys);
        }
    }
}
