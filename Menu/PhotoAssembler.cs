using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Stitcher360
{
	class PhotoAssembler
	{
		/// <summary>
		/// Main function overseeing whole stiting process
		/// </summary>
		/// <param name="sessionData"></param>
		/// <returns></returns>
		public static Bitmap StitchPhotos(SessionData sessionData)
		{

			if (sessionData.OutResolutionX != sessionData.OutResolutionY * 2)
			{
				// An equirectangular projection requires 2x1 image format
				throw new FormatException();
			}
			Bitmap output = new Bitmap(sessionData.OutResolutionX, sessionData.OutResolutionY);
			SphereVec[] photoCenters = PhotoCenters.GetPhotocenters(sessionData);

			for (int x = 0; x < sessionData.OutResolutionX; x++)
			{
				for (int y = 0; y < sessionData.OutResolutionY; y++)
				{
					output.SetPixel(x, y, GetPixelFromSphere(x, y, sessionData, photoCenters));
				}
			}
			return output;
		}

		private static Color GetPixelFromSphere(int x, int y, SessionData sessionData, SphereVec[] photoCenters)
		{
			// We need to modify the x coord slightly to get the correct degree coordinate system
			x -= sessionData.OutResolutionX / photoCenters.Length / 2;
			SphereVec currentRay = new SphereVec();
			currentRay.getPointFromCylinder(x, y, sessionData.OutResolutionX, sessionData.OutResolutionY);

			// Figure out which photo is closest to the current view ray
			int selectedImageSegment = GetClosestImgCoord(photoCenters, currentRay);

			// Get the exact pixel from the photo we are looking at           
			double scaleY = 300;
			double scaleX = scaleY;
			Color colorFromInput = GetColorFromInput(photoCenters[selectedImageSegment], sessionData.LoadedImages[selectedImageSegment], currentRay, scaleX, scaleY);
			return colorFromInput;
		}

		/// <summary>
		/// Handles accessing the correct pixel from the input data 
		/// </summary>
		/// <param name="sphereCoords"></param>
		/// <param name="image"></param>
		/// <param name="currentRay"></param>
		/// <param name="scaleX"></param>
		/// <param name="scaleY"></param>
		/// <returns></returns>
		private static Color GetColorFromInput(SphereVec photoCenter, Bitmap image, SphereVec currentRay, double scaleX, double scaleY)
		{
			int[] pointOnPlane = GetPointOnPicture(photoCenter,currentRay);

			int x = pointOnPlane[0];
			int y = pointOnPlane[1];
			if (x >= 0 && x < image.Width * 2 && y >= 0 && y < image.Height * 2)
			{
				return image.GetPixel(x, y);
			}
			else
			{
				return Color.Black;
			}
		}

        private static int[] GetPointOnPicture(SphereVec photoCenter, SphereVec currentRay)
        {
			// We are finding out where will the ray intersect the picture, which translates to
			// the intersection of a line and a plane
			int[] points = new int[2];

			int a = (int)photoCenter.X;
			int b = (int)photoCenter.Y;
			int c = (int)photoCenter.Z;
			int d = (int)(Math.Pow(a,2) + Math.Pow(b, 2) + Math.Pow(c, 2));

			int RayX = (int)currentRay.X;
			int RayY = (int)currentRay.Y;
			int RayZ = (int)currentRay.Z;

			// Solve for t

			double t = -d / ((a * RayX) + (b * RayY) + (c * RayZ));

			// This is the absolute position of where currentRay (our rendering ray) intersects the plane of the photo
			Vector absolutePosition = new Vector(t*RayX,t*RayY,t*RayZ);
			PointDebugger.WritePointToTxt(new SphereVec(absolutePosition.X, absolutePosition.Y, absolutePosition.Z), "Polyhedron.txt");
			return points;
        }


        /// <summary>
        /// For the current view requested by the render answer which picture is closest to this angle to be accessed.
        /// </summary>
        /// <param name="photoCenters">Array of centers of all photographs</param>
        /// <param name="currentRay">The current view</param>
        /// <returns></returns>
        private static int GetClosestImgCoord(SphereVec[] photoCenters, SphereVec currentRay)
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
