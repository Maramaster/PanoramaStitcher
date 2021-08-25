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
            for (int x = 0; x < sessionData.OutResolutionX; x++)
            {
                for (int y = 0; y < sessionData.OutResolutionY; y++)
                {
                    output.SetPixel(x, y, GetPixelFromSphere(sessionData, photoCenters));
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
                output[i] = new SphereCoords(36*i, 130);
                output[i+10] = new SphereCoords(36 * i, 50);
            }
            return output;
        }

        private static Color GetPixelFromSphere(SessionData sessionData, SphereCoords[] photoCenters)
        {
            //double lat = GetLatitude(x, sessionData.OutResolutionX);
            //double lon = GetLongitude(y, sessionData.OutResolutionY);

            return new Color();
        }

        // The following mathematical formulas were borrowed from Wikipedia: https://en.wikipedia.org/wiki/Equirectangular_projection
        // For clarity:             
        // Latitude is  - horizontal
        // Longitude is | vertical
        private static double GetLongitude(int y, int finalResolutionY)
        {
            // arcsin(y / radius)
            return Math.Asin(y / finalResolutionY);
        }

        private static double GetLatitude(int x, int finalResolutionX)
        {
            double whereOnCircle = finalResolutionX / x;
            return whereOnCircle * 360;
        }
    }
}
