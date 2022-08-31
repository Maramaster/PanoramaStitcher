using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
    class PhotoCenter
    {
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public PhotoCenter(double x, double y, double z, double lon, double lat)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.lon = lon;
			this.lat = lat;
		}

		//euklidean metric distance
		public double DistanceFrom(SphereVec other)
		{
			return Math.Sqrt(
				Math.Pow((double)(this.X - other.X), 2)
				+ Math.Pow((double)(this.Y - other.Y), 2)
				+ Math.Pow((double)(this.Z - other.Z), 2)
			);
		}

		/// <summary>
		/// For the current view requested by the render answer which picture is closest to this angle to be accessed.
		/// </summary>
		/// <param name="photoCenters">Array of centers of all photographs</param>
		/// <param name="currentRay">The current view</param>
		/// <returns></returns>
		public static int GetClosestImgCoord(PhotoCenter[] photoCenters, SphereVec currentRay)
		{
			int minI = 0;
			double minDistance = double.MaxValue;
			for (int i = 0; i < photoCenters.Length; i++)
			{
				if (photoCenters[i].DistanceFrom(currentRay) < minDistance)
				{
					minI = i;
					minDistance = photoCenters[i].DistanceFrom(currentRay);
				}
			}
			return minI;
		}
	}
}
