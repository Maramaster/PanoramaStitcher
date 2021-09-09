using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
    class LensCorrection
    {
        /// <summary>
        /// Additional tool for preprocessing of uploaded photos from lens distortion
        /// </summary>
        /// <param name="inImage"></param>
        /// <param name="strength"></param>
        /// <returns></returns>
        static public Bitmap CorrectLensDistortion(Bitmap inImage, float strength)
        {
            Bitmap outImage = new Bitmap(inImage.Width, inImage.Height);
            int halfWidth = inImage.Width / 2;
            int halfHeight = inImage.Height / 2;
            if (strength == 0)
            {
                strength = 0.00001F;
            }
            //the correction radius is reduced, depending on how small the focal lenght is
            double correctionRadius = Math.Sqrt(Math.Pow(inImage.Width,2) + Math.Pow(inImage.Height,2)) / strength;

            for (int x = 0; x < inImage.Width; x++)
            {
                for (int y = 0; y < inImage.Height; y++)
                {
                    int newX = x - halfWidth;
                    int newY = y - halfHeight;
                    double distance = Math.Sqrt(Math.Pow(newX, 2) + Math.Pow(newY, 2));
                    double r = distance / correctionRadius;
                    double theta;
                    if (r == 0)
                    {
                        theta = 1;
                    }
                    else
                    {
                        theta = Math.Atan(r) / r;
                    }
                    int sourceX = (int)(halfWidth + theta * newX);
                    int sourceY = (int)(halfHeight + theta * newY);
                    outImage.SetPixel(x, y, inImage.GetPixel(sourceX, sourceY));    
                }
            }
            return outImage;
        }
    }
}
