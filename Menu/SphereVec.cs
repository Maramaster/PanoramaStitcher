using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
	class SphereVec
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		public SphereVec(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		public SphereVec() { }

		/// <summary>
		/// setter for class atributes
		/// </summary>
		/// <param name="i"></param>
		/// <param name="j"></param>
		/// <param name="finalResolutionI"></param>
		/// <param name="finalResolutionJ"></param>
		public void getPointFromCylinder(int i, int j, int finalResolutionI, int finalResolutionJ)
		{
			// Normalize the y coordinate so 0 is in the middle
			j -= finalResolutionJ / 2;
			// Now calculate the sphere radius using the circumference following from the geometry of eqirectangular projection
			double sphereRadius = finalResolutionI / (2 * Math.PI);
			// Scale the y coordinate so that it enters the Asin function correctly
			double jScaled = ((double)j / ((double)finalResolutionJ / 2)) * sphereRadius;
			// Flip the y coordinate because the input coordinate system has the y axis flipped as is usual in all computer graphics
			double jFlipped = jScaled * -1;
			double lon = ToDegree(Math.Asin((double)jFlipped / sphereRadius));
			double lat = 360 * ((double)i / (double)finalResolutionI);

			//convert to vector from lat and lon
			int[] coords = getVectorFromLatandLon(lat, lon, sphereRadius);

			this.X = coords[0];
			this.Y = coords[1];
			this.Z = coords[2];
		}

		// converter from angles to vector
		private int[] getVectorFromLatandLon(double lat, double lon, double sphereRadius)
		{
			int[] coords = new int[3];

			// X Y Z
			coords[0] = (int)(sphereRadius * Math.Cos(ToRadians(lat)) * Math.Cos(ToRadians(lon)));
			coords[1] = (int)(sphereRadius * Math.Cos(ToRadians(lat)) * Math.Sin(ToRadians(lon)));
			coords[2] = (int)(sphereRadius * Math.Sin(ToRadians(lat)));

			return coords;
		}

		//euklid. metric distance
		public double DistanceFrom(SphereVec other)
		{
			return Math.Sqrt(
				Math.Pow((double)(this.X - other.X), 2) 
				+ Math.Pow((double)(this.Y - other.Y), 2) 
				+ Math.Pow((double)(this.Z - other.Z), 2)
			);
		}

		//convertor to degrees
		public static double ToDegree(double angle)
		{
			return (180 / Math.PI) * angle;
		}

		//convertor to radians
		public static double ToRadians(double angle)
		{
			return (Math.PI / 180) * angle;
		}
	}
}
