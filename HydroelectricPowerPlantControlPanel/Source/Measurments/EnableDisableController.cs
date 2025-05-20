using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroelectricPowerPlantControlPanel.Source.Measurments
{
    public class EnableDisableController : MeasurementCollector
    {
        double step;
        public EnableDisableController()
            : base( 0,  0,  1,  "" )
        {
            this.step = 1;
        }

        public void stepUp()
        {
            if (value + step <= max) value += step;
        }

        public void stepDown()
        {
            if (value + -step >= min) value -= step;
        }
    }
}
