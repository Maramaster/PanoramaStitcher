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
            Color last= Color.Black;
            for (int x = 0; x < sessionData.OutResolutionX; x++)
            {
                for (int y = 0; y < sessionData.OutResolutionY; y++)
                {
                    Color col = GetPixelFromSphere(x, y, sessionData, photoCenters);
                    
                    output.SetPixel(x, y, col);

                    //only for debugging 
                    if(last != GetPixelFromSphere(x, y, sessionData, photoCenters))
                    {
                        int x53 = 10;
                    }
                    last = GetPixelFromSphere(x, y, sessionData, photoCenters);
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
                output[i] = new SphereCoords(36*i, 40);
                output[i+10] = new SphereCoords(36 * i, -40);
            }
            return output;
        }

        private static Color GetPixelFromSphere(int x, int y, SessionData sessionData, SphereCoords[] photoCenters)
        {
            SphereCoords currentRay = new SphereCoords(x, y, sessionData.OutResolutionX, sessionData.OutResolutionY);

            int selectedImageSegment = GetClosestImgCoord(photoCenters, currentRay);

            Color[] rainbowColors = {Color.Red, Color.Green, Color.Yellow, Color.AliceBlue, Color.Teal, Color.Aqua, Color.DarkCyan, Color.SaddleBrown, Color.Pink, Color.MediumVioletRed,
                Color.Salmon, Color.SeaShell, Color.Tomato, Color.DodgerBlue, Color.LimeGreen, Color.Indigo, Color.PeachPuff, Color.Coral, Color.Gold, Color.Cyan};
            return rainbowColors[selectedImageSegment];
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
