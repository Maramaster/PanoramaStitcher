using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Stitcher360
{
    public class SessionData
    {
        public Image[] LoadedImages { get; set; }
        public int OutResolutionX { get; set; }
        public int OutResolutionY { get; set; }
        public int ImageResolutionX { get; set; }
        public int ImageResolutionY { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfCols { get; set; }
    }
}
