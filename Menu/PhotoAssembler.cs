using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Stitcher360
{
	class PhotoAssembler
	{
		public static Bitmap StitchPhotos(SessionData sessionData)
		{

            if (sessionData.OutResolutionX != sessionData.OutResolutionY * 2)
            {
                // An equirectangular projection requires 2x1 image format
                throw new FormatException();
            }
            Bitmap output = new Bitmap(sessionData.OutResolutionX, sessionData.OutResolutionY);
            SphereCoords[] photoCenters = GetPhotocenters(sessionData);

            Color last = Color.Black;
            for (int x = 0; x < sessionData.OutResolutionX; x++)
            {
                for (int y = 0; y < sessionData.OutResolutionY; y++)
                {
                    output.SetPixel(x, y, GetPixelFromSphere(x, y, sessionData, photoCenters));
                }
            }
            return output;
        }

		//For each picture one center
		private static SphereCoords[] GetPhotocenters(SessionData sessionData)
		{
			//TODO: automaticky dopocitat heightSeparator

			//pouze pro 10x2
			int heightSeparator = 80;
			SphereCoords[] output = new SphereCoords[sessionData.LoadedImages.Length];
			double[] heightCenters = getHeightCenters(sessionData.NumberOfPicturesInCol);
			for (int i = 0; i < sessionData.NumberOfPicturesInCol; i++)
			{
				for (int j = 0; j < sessionData.NumberOfPicturesInRow; j++)
				{
					output[i * sessionData.NumberOfPicturesInRow + j] = new SphereCoords(360 / sessionData.NumberOfPicturesInRow * j, heightCenters[i] * heightSeparator);
				}
			}
			return output;
		}

		private static double[] getHeightCenters(int numberOfPicturesInCol)
		{
			double[] centers = new double[numberOfPicturesInCol];
			if (numberOfPicturesInCol % 2 == 0)
			{
				int[] arr = Enumerable.Range(0, numberOfPicturesInCol / 2).ToArray();
				int[] firstPart = Enumerable.Reverse(arr).ToArray();
				int[] secondPart = Enumerable.Range(0, numberOfPicturesInCol / 2).ToArray();

				for (int i = 0; i < numberOfPicturesInCol; i++)
				{
					if (i < numberOfPicturesInCol / 2) { centers[i] = (double)firstPart[i] + (0.5); }
					else { centers[i] = -1 * ((double)secondPart[i - numberOfPicturesInCol / 2] + (0.5)); }
				}
			}
			else
			{
				int[] arr = Enumerable.Range(1, (numberOfPicturesInCol / 2)).ToArray();
				int[] firstPart = Enumerable.Reverse(arr).ToArray();
				int[] secondPart = Enumerable.Range(1, (numberOfPicturesInCol / 2)).ToArray();

				for (int i = 0; i < numberOfPicturesInCol; i++)
				{
					if (i < numberOfPicturesInCol / 2) { centers[i] = (double)firstPart[i]; }
					else if (i == numberOfPicturesInCol / 2) { centers[i] = 0; }
					else { centers[i] = -1 * ((double)secondPart[i - 1 - numberOfPicturesInCol / 2]); }
				}
			}
			return centers;
		}

		private static Color GetPixelFromSphere(int x, int y, SessionData sessionData, SphereCoords[] photoCenters)
		{
			// We need to modify the x coord slightly to get the correct degree coordinate system
			x -= sessionData.OutResolutionX / photoCenters.Length / 2;
			SphereCoords currentRay = new SphereCoords(x, y, sessionData.OutResolutionX, sessionData.OutResolutionY);

			// Figure out which photo is closest to the current view ray
			int selectedImageSegment = GetClosestImgCoord(photoCenters, currentRay);

            // Get the exact pixel from the photo we are looking at           
            double scaleY = 450;
            double scaleX = scaleY;
            //double scaleX = (scaleY / Math.PI) / 2;
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
		private static Color GetColorFromInput(SphereCoords sphereCoords, Bitmap image, SphereCoords currentRay, double scaleX, double scaleY)
		{
			double deltaLat = currentRay.lat - sphereCoords.lat;
			double deltaLon = currentRay.lon - sphereCoords.lon;

            int x = (int)((Math.Tan(SphereCoords.ToRadians(deltaLat)) * scaleX) + image.Width / 2);
            int y = image.Height - (int)((Math.Tan(SphereCoords.ToRadians(deltaLon)) * scaleY) + image.Height / 2);
            if (x >= 0 && x < image.Width && y >= 0 && y < image.Height)
            {
                return image.GetPixel(x, y);
            }
            else
            {
                return Color.Black;
            }
        }

		/// <summary>
		/// For the current view requested by the render answer which picture is closest to this angle to be accessed.
		/// </summary>
		/// <param name="photoCenters">Array of centers of all photographs</param>
		/// <param name="currentRay">The current view</param>
		/// <returns></returns>
		private static int GetClosestImgCoord(SphereCoords[] photoCenters, SphereCoords currentRay)
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
