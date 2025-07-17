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

        double tourbineAngularVelocity;

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

            tourbineAngularVelocity = 0;

            waterLevel = new Measurments.MeasurementCollector(((int)currentWaterVolume), 0, 300000000, "m");

            gateLevelController = new Measurments.MeasurmentsController(25, 0, 100, "%", 1);

            waterFlowOutPerHour = new Measurments.MeasurementCollector(0, 0, int.MaxValue, "m^3/h");

            waterFlowInController = new Measurments.MeasurmentsController(0, 0, 300000, "m^3/h", 100);

            generatorRPM = new Measurments.MeasurementCollector(0, 0, 300000, "RPM/min");

            generatedPower = new Measurments.MeasurementCollector(200, 0, 300000, "kW");

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

            waterLevel.setValue(calculateWaterLevel());

            generatorRPM.setValue(calculateTourbineRPM());

            generatedPower.setValue(calculateElectricalPower()/1000);

            waterFlowOutPerHour.setValue(calculateWaterFlowOut(generatorRPM.Value));

            currentWaterVolume += waterFlowInController.Value - waterFlowOutPerHour.Value;

            if (currentWaterVolume < 0) currentWaterVolume = 0;
        }

        //obliczanie stopnia otwarcia bramy
        private double calculateOpenedAreaOfGate()
        {
            return gateOpeningArea * gateLevelController.Value;
        }

        //Obliczanie ilosci wyplywajacej wody na podstawie wysokosci slupa wody i poziomu otwarcia bramy ()m3/h
        private double calculateWaterFlowOut(double generatorRPM)
        {
            double baseFlow = calculateWaterFallVelocity() * calculateOpenedAreaOfGate();
            double RPMwithNoResistance = 400000;

            double RPMfactor = Math.Clamp(generatorRPM, 0, RPMwithNoResistance) / RPMwithNoResistance;

            return baseFlow * RPMfactor;
        }

        //Obliczanie poziomu tafli wody na podstawie szerokosci, dlugosci i ilosci wody (m3) w zbiorniku
        public double calculateWaterLevel()
        {
            return currentWaterVolume / (tankWidth * tankDepth);
        }

        public double calculateWaterFallVelocity()
        {
            return Math.Sqrt(2 * waterLevel.Value * 9.81);
        }

        public double calculateWaterPressure()
        {
            double waterDensity = 1000;// gestość wody, przeniesc do static

            return 1000 * waterLevel.Value * 9.81;
        }
        public double calculateWaterForce()
        {
            double area = 20; //Powierzchnia wirnika turbiny

            return calculateWaterPressure() * area * calculateOpenedAreaOfGate();
        }

        public double calculateTurbineTorque()
        {
            double r = 5; //Promień wirnika turbiny w m. Przenieść do static

            return calculateWaterForce() * r;
        }

        public double calculateTourbineResistance()
        {
            double resistance = 1100; // wspolczynnik oporu turbiny

            return tourbineAngularVelocity * (-resistance);
        }

        public double calculateTourbineAngularAcceleration()
        {
            double tourbineMomentOfInertia = 100000; //moment bezwładnosci turbiny [kg/m2]

            return (calculateTurbineTorque()+ calculateTourbineResistance()) / tourbineMomentOfInertia;
        }
        public double calculateTourbineKineticForce()
        {
            double tourbineMomentOfInertia = 100000; //moment bezwładnosci turbiny [kg/m2]

            return tourbineMomentOfInertia * tourbineAngularVelocity / 2;
        }

        public void updateAngularVelocity(double deltaTime = 1)
        {
            tourbineAngularVelocity += calculateTourbineAngularAcceleration() * deltaTime;
        }

        public double calculateTourbineRPM()
        {
            updateAngularVelocity();

            double RPM = tourbineAngularVelocity * 60 / (2 * Math.PI);

            if (RPM < 0.01 && RPM > -0.01)
            {
                RPM = 0;
                tourbineAngularVelocity = 0;
            }

            return RPM;
        }

        public double calculateMechanicalPower()
        {
            return calculateTurbineTorque() * tourbineAngularVelocity;
        }

        public double calculateElectricalPower()
        {
            double tourbineEfficiency = 0.85;

            return calculateTourbineKineticForce() * tourbineEfficiency;
        }
    }
}
