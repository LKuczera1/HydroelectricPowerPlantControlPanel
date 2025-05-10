using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroelectricPowerPlantControlPanel.Source.Measurments
{
    public class MeasurmentsController :MeasurementCollector
    {
        int step;
        public MeasurmentsController(int value, int min, int max, int step)
            : base(value, min, max)
        {
            this.step = step;
        }

        public void stepUp()
        {
            if(value + step >= max) value += step;
        }

        public void stepDown()
        {
            if (value +- step >= min) value -= step;
        }
    }
}
