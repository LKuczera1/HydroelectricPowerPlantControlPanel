using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroelectricPowerPlantControlPanel.Source.Measurments
{
    public class MeasurementCollector
    {
        protected double value;
        protected double min, max;

        protected string units;

        public string Units
        { get { return units; } }

        public double Value
        {
            get { return value; }
        }

        public string ValueWithUnits
        { get { return value.ToString("F1") + " " + units; } }

        public double Min
        { get { return min; } }
        public double Max
        { get { return max; } }

        public MeasurementCollector(double value, double min, double max, string units, Action setValue = null)
        {
            this.value = value;
            this.min = min;
            this.max = max;
            this.units = units;

        }

        //Only to use by simulationManager
        public void setValue(double value)
        {
            this.value = value;
        }
    }
}
