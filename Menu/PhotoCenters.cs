using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
	/// <summary>
	/// Generates centers of all photos.
	/// </summary>
    class PhotoCenters
    {
		public static SphereVec[] GetPhotocenters(SessionData sessionData)
		{
			//TODO: automaticky dopocitat angle
			int angle = 40;
			//pozor *2 protoze to vychazi na rozdil 40
			int heightSeparatorAngle = (int)(Math.Sin(SphereVec.ToRadians(angle))*sessionData.Radius)*2;
			
			SphereVec[] output = new SphereVec[sessionData.LoadedImages.Length];
			
			double[] heightCenters = getHeightCenters(sessionData.NumberOfPicturesInCol);
			double[] arrayXandY;
			for (int i = 0; i < sessionData.NumberOfPicturesInCol; i++)
			{
				for (int j = 0; j < sessionData.NumberOfPicturesInRow; j++)
				{
					arrayXandY = getXandY(j, sessionData, Math.Asin((heightCenters[i] * heightSeparatorAngle)/sessionData.Radius));
					output[i * sessionData.NumberOfPicturesInRow + j] = new SphereVec(arrayXandY[0], arrayXandY[1] ,heightCenters[i] * heightSeparatorAngle);
				}
			}
			return output;
		}

		/// <summary>
		/// gets coordinates of an X and Y point from circle
		/// </summary>
		/// <param name="i"></param>
		/// <param name="sessionData"></param>
		/// <param name="heightSeparatorAngle"></param>
		/// <returns></returns>
		private static double[] getXandY(int i, SessionData sessionData, double heightSeparatorAngle)
		{
			
			double[] array = new double[2];
			int totalPoints = sessionData.NumberOfPicturesInRow;

			var theta = ((Math.PI * 2) / totalPoints);
			var angle = (theta * i);

			double radiusAdaptation = Math.Cos(heightSeparatorAngle);
			array[0] = ((sessionData.Radius * radiusAdaptation) * Math.Cos(angle));
			array[1] = ((sessionData.Radius * radiusAdaptation) * Math.Sin(angle));

			return array;
		}
		/// <summary>
		/// array of distances between height centers
		/// </summary>
		/// <param name="numberOfPicturesInCol"></param>
		/// <returns></returns>
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
	}
}
