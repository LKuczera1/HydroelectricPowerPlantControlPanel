using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace HydroelectricPowerPlantControlPanel.Source
{
    public class HydroelectricPowerPlantControlPanel
    {
        uint _WindowX, _WindowY;
        public uint WindowXresolution { get { return _WindowX; } }
        public uint WindowYresolution { get { return _WindowY; } }


        public RenderWindow window;
        public RenderTexture RenderedTexture;
        Sprite windowSprite;

        FPSCounter FPS;

        List<Keyboard.Key> pressedKeys;

        Source.Interfaces.Interface currentInterface;
        public HydroelectricPowerPlantControlPanel()
        {
            //Inicjalizacja okna
            _WindowX = 1920;
            _WindowY = 1080;
            window = new RenderWindow(new VideoMode(_WindowX, _WindowY, 32), "Hydroelectric Power Plant", SFML.Window.Styles.Fullscreen);
            window.SetVisible(true);

            //inicjalizacja wydarzeń
            window.Closed += new EventHandler(OnClosed);
            window.KeyPressed += OnKeyPressed;
            window.KeyReleased += onKeyReleased;

            RenderedTexture = new RenderTexture(_WindowX, _WindowY);
            windowSprite = new Sprite(RenderedTexture.Texture);
            pressedKeys = new List<Keyboard.Key>();

            //Licznik klatek na sekunde
            FPS = new FPSCounter(60);

            //Inicjalizacja interfejsu menu głównego
            currentInterface = new Interfaces.ControllPanelInterface(this);

            //Uruchomienie programu
            Run();
        }

        ~HydroelectricPowerPlantControlPanel()
        {
            window = null;
            GC.Collect();
        }

        void OnClosed(object sender, EventArgs events)
        {
            window.Close();
        }

        //Główna pętla programu
        public void Run()
        {
            while (window.IsOpen)
            {
                window.DispatchEvents();
                if (currentInterface == null) window.Close();
                RenderedTexture.Clear(new Color(100, 100, 100));
                FPS.Update();
                Update();
                Render();
                window.Display();
            }
        }

        //Rysowanie interfejsu
        void Render()
        {
            currentInterface.Render(RenderedTexture);
            RenderedTexture.Display();
            window.Draw(windowSprite);
        }
        //Aktualizacja interfejsu
        void Update()
        {
            currentInterface.Update(Mouse.GetPosition(window), Mouse.IsButtonPressed(Mouse.Button.Left), pressedKeys);
            //pressedKeys.Clear();
        }
        //Obsługa klawiatury
        public void OnKeyPressed(object sender, KeyEventArgs Key)
        {
            foreach (Keyboard.Key k in pressedKeys)
            {
                if (k == Key.Code) return;
            }
            pressedKeys.Add(Key.Code);
        }

        public void onKeyReleased(object sender, KeyEventArgs Key)
        {
            pressedKeys.Remove(Key.Code);
        }


        //Inicjalizacja poszczególnych interfejsów
        public void start()
        {
            currentInterface = null;
            GC.Collect();
        }
        public void exit()
        {
            window.Close();
        }
    }
}

