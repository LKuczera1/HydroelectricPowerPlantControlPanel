using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroelectricPowerPlantControlPanel.Source.Simulation
{
    public class ArgsWithValue :EventArgs
    {
        double value;
        double Value
        {
            get {  return value; }
        }
        public ArgsWithValue(double value) 
        { 
            this.value = value;
        }
    }
}
