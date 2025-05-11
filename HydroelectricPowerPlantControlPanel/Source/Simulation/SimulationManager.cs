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

        double waterFlowInPerHour;

        double gateOpeningArea;

        Measurments.MeasurementCollector waterLevel;

        Measurments.MeasurmentsController gateLevelController;

        Measurments.MeasurementCollector waterFlowOutPerHour;

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

            currentWaterVolume = 20000;

            gateOpeningArea = 25;

            waterLevel = new Measurments.MeasurementCollector(((int)currentWaterVolume), 0, 300000000, "m");

            gateLevelController = new Measurments.MeasurmentsController(50, 0, 100, "%", 1);

            waterFlowOutPerHour = new Measurments.MeasurementCollector(0, 0, int.MaxValue, "m^3/h");

            waterFlowInPerHour = 500;

            waterLevel.setValue((int)(currentWaterVolume * 0.001 / tankDepth * tankWidth));
        }

        ~SimulationManager()
        {

        }

        public void update(double deltaTime)
        {
            deltaTime = 0.0002777;

            deltaTime = 0.277;

            double waterFlowOut = Math.Sqrt(2 * waterLevel.Value * 9.81) * 3600 * ((double)gateLevelController.Value * 0.01);

            Console.WriteLine(waterFlowOut);

            currentWaterVolume += (waterFlowInPerHour - waterFlowOut) * deltaTime;

            waterLevel.setValue((int)(currentWaterVolume * 0.001 / tankDepth * tankWidth));
            waterFlowOutPerHour.setValue((int)waterFlowOut);
        }
    }
}
