using HydroelectricPowerPlantControlPanel.Source;

public class Program
{ 
    public static void Main()
    {
        HydroelectricPowerPlantControlPanel.Source.HydroelectricPowerPlantControlPanel HPPControlPanel = new();

        HPPControlPanel.start();
    }
}