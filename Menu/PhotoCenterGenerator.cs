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
    class PhotoCenterGenerator
    {
		public static PhotoCenter[] GetPhotocenters(SessionData sessionData)
		{
			//set angle from the horizontal line
			int heightSeparatorAngle = (int)(Math.Sin(SphereVec.ToRadians(sessionData.YAngle))*sessionData.Radius)*2;

			PhotoCenter[] output = new PhotoCenter[sessionData.LoadedImages.Length];
			
			double[] heightCenters = GetHeightCenters(sessionData.NumberOfPicturesInCol);
			for (int i = 0; i < sessionData.NumberOfPicturesInCol; i++)
			{
				for (int j = 0; j < sessionData.NumberOfPicturesInRow; j++)
				{
					double separator = Math.Asin((heightCenters[i] * heightSeparatorAngle) / sessionData.Radius);
					output[i * sessionData.NumberOfPicturesInRow + j] = GetXandY(j, sessionData, separator , heightCenters[i] * heightSeparatorAngle);
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
		private static PhotoCenter GetXandY(int i, SessionData sessionData, double heightSeparatorAngle, double Z)
		{
			double[] array = new double[2];
			int totalPoints = sessionData.NumberOfPicturesInRow;

			var theta = ((Math.PI * 2) / totalPoints);
			var angle = (theta * i);

			//gets X and Y from mathematical equation of points on circle
			double radiusAdaptation = Math.Cos(heightSeparatorAngle);
			array[0] = ((sessionData.Radius * radiusAdaptation) * Math.Cos(angle));
			array[1] = ((sessionData.Radius * radiusAdaptation) * Math.Sin(angle));

			return new PhotoCenter(array[0], array[1], Z, heightSeparatorAngle, angle);
		}

		/// <summary>
		/// array of distances between height centers
		/// </summary>
		/// <param name="numberOfPicturesInCol"></param>
		/// <returns></returns>
		private static double[] GetHeightCenters(int numberOfPicturesInCol)
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
