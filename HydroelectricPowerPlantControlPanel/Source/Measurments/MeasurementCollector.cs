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

        public int Value
        {
            get { return value; }
        }

        public int Min
        { get { return min; } }
        public int Max
        { get { return max; } }

        public MeasurementCollector(int value, int min, int max)
        {
            this.value = value;
            this.min = min;
            this.max = max;
        }
    }
}
