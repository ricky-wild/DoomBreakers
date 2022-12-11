using System;


namespace wildlogicgames
{
	public static class Utilities
	{
		private static readonly Random _randomSeed = new Random();
		public static int GetRandomNumberInt(int minRange, int maxRange)
		{
			lock (_randomSeed) //Synchronize
			{
				return _randomSeed.Next(minRange, maxRange);
			}
			//return 0;
		}

		//<summary>
		//CrossProduct() method takes two arrays of doubles as arguments, and returns their cross product as a new array of doubles.
		//It first checks that both input arrays have a length of 3 (indicating that they are 3-dimensional vectors), and 
		//throws an ArgumentException if this is not the case. Then, it calculates the cross product using the standard 
		//formula and returns the resulting vector.
		//</summary>
		public static double[] CrossProduct(double[] vector1, double[] vector2)
		{
			// Make sure the vectors are 3-dimensional
			if (vector1.Length != 3 || vector2.Length != 3) throw new ArgumentException("Vectors must be 3-dimensional");

			double[] crossProduct = new double[3];
			crossProduct[0] = vector1[1] * vector2[2] - vector1[2] * vector2[1];
			crossProduct[1] = vector1[2] * vector2[0] - vector1[0] * vector2[2];
			crossProduct[2] = vector1[0] * vector2[1] - vector1[1] * vector2[0];
			return crossProduct;
		}


		//<summary>
		//DotProduct() method takes two arrays of doubles as arguments, and returns their dot product as a double. It first checks
		//that the two arrays have the same number of elements, and throws an ArgumentException if they do not. Then, it 
		//loops through the elements of both arrays and calculates the dot product by summing the products of corresponding elements.
		//</summary>
		public static double DotProduct(double[] vector1, double[] vector2)
		{
			// Make sure the vectors have the same number of elements
			if (vector1.Length != vector2.Length) throw new ArgumentException("Vectors must have the same number of elements");

			double dotProduct = 0;
			for (int i = 0; i < vector1.Length; i++)
			{
				dotProduct += vector1[i] * vector2[i];
			}
			return dotProduct;
		}

	}
}
