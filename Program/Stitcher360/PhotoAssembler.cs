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
		/// Main function overseeing whole stitching process
		/// </summary>
		/// <param name="sessionData"></param>
		/// <returns></returns>
		public static Bitmap StitchPhotos(SessionData sessionData)
		{
			CompleteSessionData(sessionData);
			Bitmap output = new Bitmap(sessionData.OutResolutionX, sessionData.OutResolutionY);
			PhotoCenter[] photoCenters = PhotoCenterGenerator.GetPhotocenters(sessionData);

			for (int x = 0; x < sessionData.OutResolutionX; x++)
			{
				for (int y = 0; y < sessionData.OutResolutionY; y++)
				{
					output.SetPixel(GetShiftedX(x, sessionData), y, GetPixelFromSphere(x, y, sessionData, photoCenters));
				}
			}
			return output;
		}

        private static int GetShiftedX(int x, SessionData sessionData)
        {
			//shifting for half size of picture, because points are returned from base which is centered not on the left side
			return ((x + (int)(80* sessionData.ResolutionCorrector)))% sessionData.OutResolutionX;

		}

        private static void CompleteSessionData(SessionData sessionData)
        {
			int tileSize;
			
            switch (sessionData.ImageResolution)
            {
				case ImageResolution.Low:
					tileSize = 160;
					break;
				case ImageResolution.Medium:
					tileSize = 400;
					break;
				case ImageResolution.High:
					tileSize = 800;
					break;
				default:
					throw new InvalidDataException();
			}

			sessionData.ResolutionCorrector = (double)tileSize / 160;

			sessionData.OutResolutionX = tileSize * sessionData.NumberOfPicturesInRow;
			sessionData.OutResolutionY = sessionData.OutResolutionX / 2;

			sessionData.BaseScaleY = 2.225/ sessionData.OutResolutionX * 1000;
			sessionData.BaseScaleX = GetBaseScaleX(sessionData.BaseScaleY, 
				sessionData.NumberOfPicturesInRow, sessionData.ResolutionCorrector);

			sessionData.Radius = sessionData.OutResolutionX / (2 * Math.PI);
		}

		private static Color GetPixelFromSphere(int x, int y, SessionData sessionData, PhotoCenter[] photoCenters)
		{
			SphereVec currentRay = new SphereVec();
			currentRay.GetPointFromCylinder(x, y, sessionData.OutResolutionX, sessionData.OutResolutionY);

			// Figure out which photo is closest to the current view ray
			int selectedImageSegment = PhotoCenter.GetClosestImgCoord(photoCenters, currentRay);

			// Get the exact pixel from the photo we are looking at           
			Color colorFromInput = GetColorFromInput(photoCenters[selectedImageSegment], sessionData.LoadedImages[selectedImageSegment], currentRay,sessionData);
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
		private static Color GetColorFromInput(PhotoCenter photoCenter, Bitmap image, SphereVec currentRay, SessionData sessionData)
		{
			int[] pointOnPlane = PictureAccesser.GetPointOnPicture(photoCenter,currentRay, sessionData);

			int x = (-pointOnPlane[0]) + image.Width / 2;
			int y = pointOnPlane[1] + image.Height / 2;

			if (x >= 0 && x < image.Width && y >= 0 && y < image.Height)
			{
				return image.GetPixel(x, y);
			}
			else
			{
				return Color.Black;
			}
		}
		private static double GetBaseScaleX(double baseScaleY, int rowCount, double corrector)
		{
			//empirically tested scales of vector base in X dimension compared to Y dimension
			//when eqirectangular projection their ratio is same therefore no correction needed

			double diff;
			switch (rowCount)
			{
				case 3:
					diff = 3.8;
					break;
				case 4:
					diff = 2.35;
					break;
				case 5:
					diff = 1.52;
					break;
				case 6:
					diff = 1;
					break;
				case 7:
					diff = 0.64;
					break;
				case 8:
					diff = 0.36;
					break;
				case 9:
					diff = 0.15;
					break;
				default:
					diff = 0;
					break;
			}
			return baseScaleY - diff / corrector;
		}
	}
}
