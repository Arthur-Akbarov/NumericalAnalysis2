using System.Linq;

namespace NumericalAnalysis2
{
	static class Worker
	{
		public static bool DiagonallyDominant(this SquareMatrix q)
		{
			return q.X.Select((x, i) => 2 * x[i] - x.N8).All(x => x > 0);
		}
		public static SquareMatrix DiagonalInvert(this SquareMatrix q)
		{
			int n = q.Rows;
			var result = new SquareMatrix(n);

			for (int i = 0; i < n; i++)
			{
				double d = q[i, i];

				if (d != 0)
					result[i, i] = 1 / d;
			}

			return result;
		}
		public static SquareMatrix AboveDiagonal(this SquareMatrix q)
		{
			var result = q.DeepClone;
			int n = q.Rows;

			for (int i = 0; i < n; i++)
				for (int j = 0; j <= i; j++)
					result[i, j] = 0;

			return result;
		}
		public static SquareMatrix Diagonal(this SquareMatrix q)
		{
			int n = q.Rows;
			var result = new SquareMatrix(n);

			for (int i = 0; i < n; i++)
				result[i, i] = q[i, i];

			return result;
		}
		public static SquareMatrix UnderDiagonal(this SquareMatrix q)
		{
			var result = q.DeepClone;
			int n = q.Rows;

			for (int i = 0; i < n; i++)
				for (int j = i; j < n; j++)
					result[i, j] = 0;

			return result;
		}
	}
}
