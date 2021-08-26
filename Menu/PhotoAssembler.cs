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

        private static SphereCoords[] GetPhotocenters(SessionData sessionData)
        {
            // TODO: A simple algorithm that takes in number of rows and cols, resolutions and focal length and
            // returns an array of coordinates of the centroids of the photos in 3D space

            // For testing purposes, a constant array of centers is returned:
            SphereCoords[] output = new SphereCoords[20];
            for (int i = 0; i < 10; i++)
            {
                // Puvodne 130 a 50, coz je spatne
                output[i + 10] = new SphereCoords(36 * i, 40);
                output[i] = new SphereCoords(36 * i, -40);
            }
            return output;
        }

        private static Color GetPixelFromSphere(int x, int y, SessionData sessionData, SphereCoords[] photoCenters)
        {
            // We need to modify the x coord slightly to get the correct degree coordinate system
            x -= sessionData.OutResolutionX / photoCenters.Length / 2;
            SphereCoords currentRay = new SphereCoords(x, y, sessionData.OutResolutionX, sessionData.OutResolutionY);

            // Figure out which photo is closest to the current view ray
            int selectedImageSegment = GetClosestImgCoord(photoCenters, currentRay);

            // Get the exact pixel from the photo we are looking at           
            double scaleY = 302;
            double scaleX = (scaleY / Math.PI) * 2;
            Color colorFromInput = GetColorFromInput(photoCenters[selectedImageSegment], sessionData.LoadedImages[selectedImageSegment], currentRay, scaleX, scaleY);
            return colorFromInput;
        }

        private static Color GetColorFromInput(SphereCoords sphereCoords, Bitmap image, SphereCoords currentRay, double scaleX, double scaleY)
        {
            double deltaLat = currentRay.lat - sphereCoords.lat;
            double deltaLon = currentRay.lon - sphereCoords.lon;

            int x = (int)((Math.Tan(SphereCoords.ToRadians(deltaLat)) * scaleX) + image.Width / 2);
            int y = (int)((Math.Tan(SphereCoords.ToRadians(deltaLon)) * scaleY) + image.Height / 2);
            if (x >= 0 && x < image.Width && y >= 0 && y < image.Height)
            {
                return image.GetPixel(x, y);
            }
            else
            {
                return Color.Black;
            }
        }

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
