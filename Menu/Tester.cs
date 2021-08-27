using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stitcher360;

namespace UnitTests
{
	[TestClass]
	public class Tester
	{
		[TestMethod]
		public void RightIsNumberValidator()
		{
			//arrange
			string input = "5";
			//act
			bool TestValidityOutput = Validator.isNumber(input);
			//assert
			Assert.AreEqual(true, TestValidityOutput);
		}

		//SphereCoords slozitejsi konstruktor (0,0) ma dat 0,+90
		// resX,resY ma dat 360,-90
		[TestMethod]
		public void RightCoordsForLeftTop()
		{
			//arrange
			SphereCoords coords = new SphereCoords(0, 0, 1000, 500);
			//act
			bool TestValidityOutput = (coords.lat == 0 && coords.lon == 90);
			//assert
			Assert.AreEqual(90, coords.lon);
			Assert.AreEqual(0, coords.lat);
		}

		[TestMethod]
		public void RightCoordsForRightBottom()
		{
			int resX = 1000;
			int resY = 500;
			//arrange
			SphereCoords coords = new SphereCoords(resX, resY, resX, resY);
			//act
			bool TestValidityOutput = (coords.lat == 0 && coords.lon == 90);
			//assert
			Assert.AreEqual(-90, coords.lon);
			Assert.AreEqual(360, coords.lat);
		}

		[TestMethod]
		public void RightCoordsForMiddle()
		{
			int resX = 1000;
			int resY = 500;
			//arrange
			SphereCoords coords = new SphereCoords(resX/2, resY/2, resX, resY);
			//act
			bool TestValidityOutput = (coords.lat == 0 && coords.lon == 90);
			//assert
			Assert.AreEqual(0, coords.lon);
			Assert.AreEqual(180, coords.lat);
		}

		[TestMethod]
		public void RightCoordsForLeftBottom()
		{
			int resX = 1000;
			int resY = 500;
			//arrange
			SphereCoords coords = new SphereCoords(0, resY, resX, resY);
			//act
			bool TestValidityOutput = (coords.lat == 0 && coords.lon == 90);
			//assert
			Assert.AreEqual(-90, coords.lon);
			Assert.AreEqual(0, coords.lat);
		}

		[TestMethod]
		public void RightCoordsForRightTop()
		{
			int resX = 1000;
			int resY = 500;
			//arrange
			SphereCoords coords = new SphereCoords(resX, 0, resX, resY);
			//act
			bool TestValidityOutput = (coords.lat == 0 && coords.lon == 90);
			//assert
			Assert.AreEqual(90, coords.lon);
			Assert.AreEqual(360, coords.lat);
		}

		[TestMethod]
		public void RightCoordsForRandom()
		{
			int resX = 1000;
			int resY = 500;
			//arrange
			SphereCoords coords = new SphereCoords(250, 375, resX, resY);
			//act
			bool TestValidityOutput = (coords.lat == 0 && coords.lon == 90);
			//assert
			Assert.AreNotEqual(0, coords.lon);
			Assert.AreEqual(90, coords.lat);
		}
	}
}
