using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
	class PictureAccesser
	{
		public static int[] GetPointOnPicture(PhotoCenter photoCenter, SphereVec currentRay, SessionData sessionData)
		{
			// We are finding out where will the ray intersect the picture, which translates to
			// the intersection of a line and a plane
			int[] points = new int[2];

			int a = (int)photoCenter.X;
			int b = (int)photoCenter.Y;
			int c = (int)photoCenter.Z;
			int d = (int)(Math.Pow(a, 2) + Math.Pow(b, 2) + Math.Pow(c, 2));

			int RayX = (int)currentRay.X;
			int RayY = (int)currentRay.Y;
			int RayZ = (int)currentRay.Z;

			// Solve for t
			double t = (double)-d / (double)((a * RayX) + (b * RayY) + (c * RayZ));

			// This is the absolute position of where currentRay (our rendering ray) intersects the plane of the photo
			Vector absolutePosition = new Vector(t * RayX, t * RayY, t * RayZ);

			points = GetLocalImageCoordinate(photoCenter, absolutePosition, sessionData);
			return points;
		}

		public static int[] GetLocalImageCoordinate(PhotoCenter photoCenter, Vector absolutePosition, SessionData sessionData)
		{
			// Generate a rotation matrix from two angles, minus before latitude is needed to flip the coordinate system
			double[,] rotationMatrix = Matrix.ZYRotationMatrix(-photoCenter.lat, photoCenter.lon);
			double[] absPositionInNewBasis = Matrix.Multiply3n1(rotationMatrix, absolutePosition.X, absolutePosition.Y, absolutePosition.Z);

			int[] output = new int[2];

			// After the rotation - which exactly counters the rotation of the plane of the image we are on, we read the Y and Z coordinates of
			// the vector which give us the correct position in the coordinate system of the image plane
			output[0] = (int)Math.Round((absPositionInNewBasis[1] * (sessionData.BaseScaleX)));
			output[1] = (int)Math.Round((absPositionInNewBasis[2] * (sessionData.BaseScaleY)));

			return output;
		}
	}
}
