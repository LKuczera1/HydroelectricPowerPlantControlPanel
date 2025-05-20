using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroelectricPowerPlantControlPanel.Source.Measurments
{
    public class MeasurmentsController :MeasurementCollector
    {
        double step;
        public MeasurmentsController(double value, double min, double max, string units, double step)
            : base(value, min, max, units)
        {
            this.step = step;
        }

        public void stepUp()
        {
            if(value + step <= max) value += step;
        }

        public void stepDown()
        {
            if (value +- step >= min) value -= step;
        }
    }
}
