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
		public void GetPointFromCylinder(int i, int j, int finalResolutionI, int finalResolutionJ)
		{
			// normalize the y coordinate so 0 is in the middle
			j -= finalResolutionJ / 2;
			// now calculate the sphere radius using the circumference following from the geometry of eqirectangular projection
			double sphereRadius = finalResolutionI / (2 * Math.PI);
			// scale the y coordinate so that it enters the Asin function correctly
			double jScaled = ((double)j / ((double)finalResolutionJ / 2)) * sphereRadius;
			// set latitude and longtitude 
			double lon = ToDegree(Math.Asin((double)jScaled / sphereRadius));
			double lat = 360 * ((double)i / (double)finalResolutionI);

			//convert to vector from lat and lon
			int[] coords = GetVectorFromLatandLon(lat, lon, sphereRadius);

			this.X = coords[0];
			this.Y = coords[1];
			this.Z = coords[2];
		}

		// converter from angles to vector
		private int[] GetVectorFromLatandLon(double lat, double lon, double sphereRadius)
		{
			double[,] rotationMatrix = Matrix.YZRotationMatrix(ToRadians(lat), ToRadians(lon));
			double[] coordsDouble = Matrix.Multiply3n1(rotationMatrix, sphereRadius, 0, 0);

			return coordsDouble.Select(x => (int)x).ToArray();
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
