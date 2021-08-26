using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
    public class SphereCoords
    {
        public SphereCoords(double lat, double lon)
        {
            this.lat = lat;
            this.lon = lon;
        }
        /// <summary>
        /// Alternative constructor for constructing out of cartesian coordinates
        /// The following mathematical formulas were borrowed from Wikipedia: https://en.wikipedia.org/wiki/Equirectangular_projection
        /// For clarity:             
        /// Latitude is  - horizontal
        /// Longitude is | vertical
        /// </summary>
        /// <param name="x">X in cartesian coords</param>
        /// <param name="y">Y in cartesian coords</param>
        /// <param name="finalResolutionX">X size of equirectangular canvas</param>
        /// <param name="finalResolutionY">Y size of equirectangular canvas</param>
        public SphereCoords(int x, int y, int finalResolutionX, int finalResolutionY)
        {
            // Normalize the y coordinate so 0 is in the middle
            y -= finalResolutionY / 2;
            this.lon = ToDegree(Math.Atan((double)y / (double)finalResolutionY));
            this.lat = 360 * ((double)x / (double)finalResolutionX);
        }

        public double lat { get; set; }
        public double lon { get; set; }

        public double DistanceFrom(SphereCoords other)
        {
            //TODO: prevest do vektoru, 0 je daleko od 360

            /*double delta_lambda = ToRadians((this.lon - other.lon));
            double delta_phi =  (this.lat - other.lat);
            double a = Math.Pow(Math.Sin(delta_phi / 2.0), 2) + Math.Cos(ToRadians(this.lat)) * Math.Cos(ToRadians(other.lat)) * Math.Pow(Math.Sin(delta_lambda / 2.0), 2);
            return a;*/
            return Math.Sqrt(Math.Pow((double)(this.lat - other.lat), 2) + Math.Pow((double)(this.lon - other.lon), 2));
        }

        public static double ToDegree(double angle)
        {
            return (180 / Math.PI) * angle;
        }
        public static double ToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
