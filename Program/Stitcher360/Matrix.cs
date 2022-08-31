using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stitcher360
{
	class Matrix
	{
		public double[,] body { get; set; }

		public Matrix(double first, double second, double third, double fourth, double fifth, double sixth, double seventh, double eight, double ninth)
		{
			body = new double[3, 3];
			body[0, 0] = first;
			body[0, 1] = second;
			body[0, 2] = third;
			body[1, 0] = fourth;
			body[1, 1] = fifth;
			body[1, 2] = sixth;
			body[2, 0] = seventh;
			body[2, 1] = eight;
			body[2, 2] = ninth;
		}

		public static double[,] Multiply3n3(double[,] matrix1, double[,] matrix2)
		{
			//always fixed 3 dimensions
			int matrix1Rows = 3;
			int matrix1Cols = 3;
			int matrix2Cols = 3;

			// creating the final product matrix  
			double[,] product = new double[3, 3];

			// looping through first matrix rows  
			for (int i = 0; i < matrix1Rows; i++)
			{
				// for each first matrix row, loop through second matrix columns  
				for (int j = 0; j < matrix2Cols; j++)
				{
					// loop through first matrix columns to calculate the dot product  
					for (int k = 0; k < matrix1Cols; k++)
					{
						product[i, j] +=
						  matrix1[i, k] *
						  matrix2[k, j];
					}
				}
			}

			return product;
		}

		public static double[] Multiply3n1(double[,] matrix, double fst, double sec, double trd)
		{
			// creating the final product matrix  
			double[] product = new double[3];
 

			product[0] += matrix[0, 0] * fst;
			product[0] += matrix[0, 1] * sec;
			product[0] += matrix[0, 2] * trd;

			product[1] += matrix[1, 0] * fst;
			product[1] += matrix[1, 1] * sec;
			product[1] += matrix[1, 2] * trd;

			product[2] += matrix[2, 0] * fst;
			product[2] += matrix[2, 1] * sec;
			product[2] += matrix[2, 2] * trd;

			return product;
		}

		public static double[,] ZYRotationMatrix(double lat, double lon)
		{
			//rotation matrix for Z axis
			Matrix Z = new Matrix(Math.Cos(lat), -Math.Sin(lat), 0, Math.Sin(lat), Math.Cos(lat), 0, 0, 0, 1);

			//rotation matrix for Y axis
			Matrix Y = new Matrix(Math.Cos(lon), 0, Math.Sin(lon), 0, 1, 0, -Math.Sin(lon), 0, Math.Cos(lon));

			return Multiply3n3(Y.body,Z.body);
		}

		public static double[,] YZRotationMatrix(double lat, double lon)
		{
			//rotation matrix for Z axis
			Matrix Z = new Matrix(Math.Cos(lat), -Math.Sin(lat), 0, Math.Sin(lat), Math.Cos(lat), 0, 0, 0, 1);

			//rotation matrix for Y axis
			Matrix Y = new Matrix(Math.Cos(lon), 0, Math.Sin(lon), 0, 1, 0, -Math.Sin(lon), 0, Math.Cos(lon));

			return Multiply3n3(Z.body, Y.body);
		}
	}
}
