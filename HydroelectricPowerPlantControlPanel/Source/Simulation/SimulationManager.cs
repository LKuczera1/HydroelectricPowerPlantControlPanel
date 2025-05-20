using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HydroelectricPowerPlantControlPanel.Source.Simulation
{
    public class SimulationManager
    {
        //Uproszczony kształt zbiornika wodnego jest prostopadłościanem o wymiarach 1km x 3km x 100m
        double tankWidth, tankHeight, tankDepth;
        double currentWaterVolume;

        double gateOpeningArea;

        Measurments.MeasurementCollector waterLevel;

        Measurments.MeasurmentsController gateLevelController;

        Measurments.MeasurementCollector waterFlowOutPerHour;

        Measurments.MeasurmentsController waterFlowInController;

        Measurments.MeasurementCollector generatorRPM;

        Measurments.MeasurementCollector generatedPower;

        Measurments.EnableDisableController emergencyGateController;

        public Measurments.EnableDisableController EmergencyGateController
        {
            get { return emergencyGateController; }
        }
        public Measurments.MeasurementCollector GeneratedPower
        {
            get { return generatedPower; }
        }
        public Measurments.MeasurementCollector GeneratorRPM
        {
            get { return generatorRPM; }
        }

        public Measurments.MeasurmentsController WaterFlowInController
        {
            get { return waterFlowInController;}
        }

        public Measurments.MeasurementCollector WaterFlowOutPerHour
        {
            get {  return waterFlowOutPerHour; }
        }

        public Measurments.MeasurementCollector WaterLevel
        { 
            get { return waterLevel; } 
        }

        public Measurments.MeasurmentsController GateLevelController
        {
            get { return gateLevelController; }
        }
        public SimulationManager()
        {
            tankDepth = 3000;
            tankWidth = 1000;
            tankHeight = 100;

            currentWaterVolume = 0;

            gateOpeningArea = 8;

            waterLevel = new Measurments.MeasurementCollector(((int)currentWaterVolume), 0, 300000000, "m");

            gateLevelController = new Measurments.MeasurmentsController(25, 0, 100, "%", 1);

            waterFlowOutPerHour = new Measurments.MeasurementCollector(0, 0, int.MaxValue, "m^3/h");

            waterFlowInController = new Measurments.MeasurmentsController(0, 0, 300000, "m^3/h", 100);

            generatorRPM = new Measurments.MeasurementCollector(200, 0, 300000, "RPM/min");

            generatedPower = new Measurments.MeasurementCollector(200, 0, 300000, "m^3/h");

            emergencyGateController = new Measurments.EnableDisableController();

            waterLevel.setValue((int)(currentWaterVolume * 0.001 / tankDepth * tankWidth));
        }

        ~SimulationManager()
        {

        }

        public void update(double deltaTime)
        {
            deltaTime = 0.0002777;

            deltaTime = 0.277;

            //obliczenia w symulacji sa zepsute

            //double waterFlowOut = Math.Sqrt(2 * waterLevel.Value * 9.81) * 3600 * ;


            //currentWaterVolume += (waterFlowInPerHour - waterFlowOut) * deltaTime;


            //waterLevel.setValue((int)(currentWaterVolume * 0.001 / tankDepth * tankWidth));
            //waterFlowOutPerHour.setValue((int)waterFlowOut);

            //Aktualizacja doplywu wody do zbiornika (m3/h)

            waterFlowOutPerHour.setValue(calculateWaterFlowOut());

            currentWaterVolume += waterFlowInController.Value - waterFlowOutPerHour.Value;

            if (currentWaterVolume < 0) currentWaterVolume = 0;

            waterLevel.setValue(calculateWaterLevel());
        }

        //obliczanie stopnia otwarcia bramy
        private double calculateOpenedAreaOfGate()
        {
            return gateOpeningArea * gateLevelController.Value;
        }

        //Obliczanie ilosci wyplywajacej wody na podstawie wysokosci slupa wody i poziomu otwarcia bramy ()m3/h
        private double calculateWaterFlowOut()
        {
            return Math.Sqrt(2 * waterLevel.Value * 9.81) * calculateOpenedAreaOfGate();
        }

        //Obliczanie poziomu tafli wody na podstawie szerokosci, dlugosci i ilosci wody (m3) w zbiorniku
        public double calculateWaterLevel()
        {
            return currentWaterVolume / (tankWidth * tankDepth);
        }
    }
}
