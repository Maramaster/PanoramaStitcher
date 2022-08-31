using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Stitcher360
{
    /// <summary>
    /// Global holder of all the configuration data which is used throughout the whole solution
    /// </summary>
    public class SessionData
    {
        public Bitmap[] LoadedImages { get; set; }
        public int OutResolutionX { get; set; }
        public int OutResolutionY { get; set; }
        public int NumberOfPicturesInRow { get; set; }
        public int NumberOfPicturesInCol { get; set; }
        public double ResolutionCorrector { get; set; }
        public double Radius { get; set; }
        public int YAngle { get; set; }
        public double BaseScaleX { get; set; }
        public double BaseScaleY { get; set; }
        public ImageResolution ImageResolution { get; set; }
    }
}
