using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
	class PointDebugger
	{
		public static void WritePointArrayToTxt(SphereVec[] sphereVecs, string debugName)
        {
			// The output is formatted to allow easy reading from a python script which I use
			// to visualize the given point cloud in Blender
			//[(0.0, 0.0, 0.0), (1.0, 0, 0)]
			using (StreamWriter SW = new StreamWriter(debugName))
			{
				for (int i = 0; i < sphereVecs.Length; i++)
				{
					SW.Write(((int)sphereVecs[i].X).ToString() + "," + ((int)sphereVecs[i].Y).ToString() + "," + ((int)sphereVecs[i].Z).ToString() + ";");
				}
			}
		}
		public static void WritePointToTxt(SphereVec sphereVec, string debugName)
		{
			using (StreamWriter SW = File.AppendText(debugName))
			{
				SW.Write(sphereVec.X.ToString() + "," + sphereVec.Y.ToString() + "," + sphereVec.Z.ToString() + ";");
			}
		}
	}
}
