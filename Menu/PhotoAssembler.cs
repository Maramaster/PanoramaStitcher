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
			Bitmap output = new Bitmap(sessionData.OutResolutionX, sessionData.OutResolutionY);
			PhotoCenter[] photoCenters = PhotoCenterGenerator.GetPhotocenters(sessionData);

			for (int x = 0; x < sessionData.OutResolutionX; x++)
			{
				for (int y = 0; y < sessionData.OutResolutionY; y++)
				{
					output.SetPixel(x, y, GetPixelFromSphere(x, y, sessionData, photoCenters));
				}
			}
			return output;
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
		
	}
}
