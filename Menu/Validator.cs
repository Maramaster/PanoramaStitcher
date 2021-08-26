using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
	class Validator
	{
		public static bool isNumber(string testString)
		{
			return int.TryParse(testString, out _);
		}
	}
}
