using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroelectricPowerPlantControlPanel.Source.Measurments
{
    public class MeasurementCollector
    {
        protected int value;
        protected int min, max;

        protected string units;

        public string Units
        { get { return units; } }

        public int Value
        {
            get { return value; }
        }

        public string ValueWithUnits
        { get { return value.ToString() + " " + units; } }

        public int Min
        { get { return min; } }
        public int Max
        { get { return max; } }

        public MeasurementCollector(int value, int min, int max, string units)
        {
            this.value = value;
            this.min = min;
            this.max = max;
            this.units = units;
        }

        public void setValue(int value)
        {
            this.value = value;
        }
    }
}
