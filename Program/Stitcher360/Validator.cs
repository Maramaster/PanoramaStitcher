using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
	/// <summary>
	/// This class is used to check whether sessionData is in a correct format before being sent to the main stitching function.
	/// </summary>
	class Validator
	{
		public static bool IsNumber(string testString)
		{
			return int.TryParse(testString, out _);
		}
	}
}
