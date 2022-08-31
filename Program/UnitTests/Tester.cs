using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
			bool TestValidityOutput = Validator.IsNumber(input);
			//assert
			Assert.AreEqual(true, TestValidityOutput);
		}
         
	}
}
