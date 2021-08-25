using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
    class SphereCoords
    {
        public SphereCoords(double lat, double lon)
        {
            this.lat = lat;
            this.lon = lon;
        }

        public double lat { get; set; }
        public double lon { get; set; }
    }
}
