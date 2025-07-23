namespace HydroelectricPowerPlantControlPanel.Source.Simulation
{
    public class SimulationManager
    {
        //Pole parametrów obiektów symulacji
        double waterDensity = 1000;// gestość wody

        //Parametry zbiornika wodnego
        //Uproszczony kształt zbiornika wodnego jest prostopadłościanem o wymiarach 1km x 3km x 100m
        static double tankDepth = 3000;
        static double tankWidth = 1000;
        static double tankHeight = 100;
        //Objetosc wody w zbiorniku w m^3
        double currentWaterVolume = (tankDepth * tankWidth * tankHeight) * 0.60; // - Zbiornik wodny wypełniony do połowy

        //Parametry zapory wodnej
        static double waterDamHeight = tankHeight;
        static double gateOpeningArea = 8; //Pole otworu śluzy
        static double tourbineThrottleFactor = 0.2;

        //Parametry turbiny
        static double RPMwithNoResistance = 3000; //Wartość obrotów turbiny nie generujących oporu dla przepływu wody
        static double tourbineArea = 10; //Powierzchnia wirnika turbiny w m^2
        static double tourbineRadius = 1.5; //Promień wirnika turbiny w m.
        static double tourbineCanalRadius = 1.54;
        static double canalFillFactor = tourbineRadius / tourbineCanalRadius; //Współczynnik wypełnienia kanału wirnikiem
        static double tourbineResistance = 9000; // wspolczynnik oporu turbiny
        static double tourbineMomentOfInertia = 340000; //moment bezwładnosci turbiny [kg/m2]
        static double tourbineEfficiency = 0.70; //Wydajność turbiny
        static double tourbineElectricalEfficiency = 0.75; 
        static double generatorLoadFactor = 12000;   // moment oporowy generatora [Nm/(rad/s)]

        double tourbineAngularVelocity = 0; //Początkowa prędkość kątowa turbiny

        static double emergencyGateLevel = 65; //Wysokość śluzy awaryjnej
        static double emergencyGateArea = 60; //Pole powierzchni śluzy awaryjnej

        //Na podstawie realistycznych wartości zapór hydroelektrycznych


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
            waterLevel = new Measurments.MeasurementCollector(currentWaterVolume, 0, double.MaxValue, "m");

            gateLevelController = new Measurments.MeasurmentsController(0, 0, 100, "%", 1);

            waterFlowOutPerHour = new Measurments.MeasurementCollector(0, 0, double.MaxValue, "l/h");

            waterFlowInController = new Measurments.MeasurmentsController(0, 0, double.MaxValue, "l/h", 1000);

            generatorRPM = new Measurments.MeasurementCollector(0, 0, double.MaxValue, "RPM");

            generatedPower = new Measurments.MeasurementCollector(0, 0, double.MaxValue, "kW");

            emergencyGateController = new Measurments.EnableDisableController();
        }

        ~SimulationManager()
        {

        }

        //Parametr delta time, porzucony
        public void update(double deltaTime)
        {
            waterLevel.setValue(calculateWaterLevel());

            generatorRPM.setValue(calculateTourbineRPM());

            generatedPower.setValue(calculateElectricalPower()/1000); // <- Zamiana na kW

            waterFlowOutPerHour.setValue(calculateWaterFlowOut(generatorRPM.Value));

            currentWaterVolume += waterFlowInController.Value - waterFlowOutPerHour.Value;

            currentWaterVolume -= calculateEmergencyGateFlowOut(calculateWaterLevel()) * emergencyGateController.Value;

            if (currentWaterVolume < 0) currentWaterVolume = 0;
        }

        //obliczanie stopnia otwarcia bramy
        private double calculateOpenedAreaOfGate()
        {
            return gateOpeningArea * (gateLevelController.Value/100); //Zamiana na procenty
        }

        //Obliczanie ilosci wody wypływającej z śluzy awaryjnej
        private double calculateEmergencyGateFlowOut(double waterLevel)
        {
            double waterLevelAboveEmergencyGate = waterLevel - emergencyGateLevel;
            if (waterLevelAboveEmergencyGate < 0) return 0;
            else return Math.Sqrt(2 * waterLevelAboveEmergencyGate * 9.81) * emergencyGateArea * 1000;
        }

        //Obliczanie ilosci wyplywajacej wody na podstawie wysokosci slupa wody i poziomu otwarcia bramy ()m3/h
        private double calculateWaterFlowOut(double generatorRPM)
        {
            double baseFlow = calculateWaterFallVelocity() * calculateOpenedAreaOfGate();

            double RPMfactor = 1 - Math.Clamp(generatorRPM, 0, RPMwithNoResistance) / RPMwithNoResistance;
            double throttle = 1.0 - tourbineThrottleFactor * RPMfactor;

            return baseFlow * throttle * 1000;
        }

        //Obliczanie poziomu tafli wody na podstawie szerokosci, dlugosci i ilosci wody (m3) w zbiorniku
        private double calculateWaterLevel()
        {
            return currentWaterVolume / (tankWidth * tankDepth);
        }

        //Prędkość graniczna - Szybkość "Spadania" wody
        private double calculateWaterFallVelocity()
        {
            return Math.Sqrt(2 * waterLevel.Value * 9.81);
        }

        //Cisnienie wody
        private double calculateWaterPressure()
        {
            return waterDensity * waterLevel.Value * 9.81;
        }

        //Siła wywierana przez wodę na turbinę
        private double calculateWaterForce()
        {
            double flow_m3s = calculateWaterFlowOut(generatorRPM.Value) / 3600; //<- zamiana z m^3/h na m^3/s
            double waterVelocity = calculateWaterFallVelocity();
            return waterDensity * flow_m3s * waterVelocity;
        }

        //Moment siły
        private double calculateTurbineTorque()
        {
            return calculateWaterForce() * tourbineRadius* tourbineEfficiency * canalFillFactor;
        }

        //Opór turbiny
        private double calculateTourbineResistance()
        {
            return tourbineAngularVelocity * (-tourbineResistance);
        }

        //Przyśpieszenie kątowe turbiny
        private double calculateTourbineAngularAcceleration()
        {
            return (calculateTurbineTorque()+ calculateTourbineResistance()) / tourbineMomentOfInertia;
        }

        //Wypadkowa momentu sił
        private double calculateResultantTorque()
        {
            return calculateTurbineTorque() + calculateTourbineResistance();
        }

        //Siła kinetyczna turbiny
        private double calculateTourbineKineticForce()
        {
            return tourbineMomentOfInertia * tourbineAngularVelocity / 2;
        }

        //Przyśpieszenie kątowe turbiny - aktualizacja
        private void updateAngularVelocity(double deltaTime = 1)
        {
            tourbineAngularVelocity += calculateTourbineAngularAcceleration() * deltaTime;
        }

        //Obroty turbiny
        private double calculateTourbineRPM()
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

        //Moc generowana przez generator
        private double calculateGeneratorTorque()
        {
            return generatorLoadFactor * tourbineAngularVelocity;
        }

        //Moc mechaniczna generowana przez turbine
        private double calculateMechanicalPower()
        {
            double mechanicalPower = calculateWaterPressure() * calculateWaterFlowOut(generatorRPM.Value)/3600;
            return Math.Max(0, mechanicalPower);
        }

        //Moc generowana przez turbine
        private double calculateElectricalPower()
        {
            return calculateGeneratorTorque() * tourbineElectricalEfficiency;
        }
    }
}
