using System.Linq;
using static NumericalAnalysis2.Printer;
using static NumericalAnalysis2.SquareMatrix;
using static System.Math;

namespace NumericalAnalysis2
{
	static class Eigenvalue
	{
		const int oneColumnLength = 56;

		static SquareMatrix A;
		static Vector y;
		static double max = 0;
		static int steps;

		static void Start(string s1, string s2, string s3, double e)
		{
			StartLine(oneColumnLength);

			Line(s1);

			WriteColor(s2);
			Left(s3);
			RightE(e);

			Line();
		}
		static void End(bool posterioriEstimate = false)
		{
			Left("Необходимая точность была достигнута на шаге");
			Right(steps);

			Line();
			Left("Максимальное по модулю собственное число");
			RightColor(max);

			if (posterioriEstimate)
			{
				Left("Апостериорная оценка погрешности вычисленного с.ч.");
				RightE((A * y - max * y).N2 / y.N2);
				Line();
			}

			Left("Соответствующий ему собственный вектор");
			Right(y / y.N1);

			OutputResidual(A * y - max * y);

			EndLine(oneColumnLength);
			Line();
		}

		public static double PowerIteration(SquareMatrix A, double e, bool write)
		{
			Eigenvalue.A = A;

			if (write)
				Start("Найдём наибольшее по модулю собственное число",
					"степенным методом", " с заданной погрешностью", e);

			y = new Vector(A.Rows, 1);
			var yNext = A * y;

			for (steps = 1; ; steps++)
			{
				var ratio = yNext.X.Zip(y.X, (x, y) => x / y);

				max = ratio.Max();
				double min = ratio.Min();

				y = yNext;
				yNext = A * y;

				if (max - min > e)
					continue;

				if (write)
					End();

				return max;
			}
		}
		public static double DotProduct(SquareMatrix A, double e, bool write)
		{
			Eigenvalue.A = A;

			if (write)
				Start("Найдём наибольшее по модулю собственное число",
					"методом скалярных произведений",
					" с заданной погрешностью", e);

			var yNext = new Vector(A.Rows, 1);

			for (steps = 1; ; steps++)
			{
				y = yNext / yNext.N1;
				yNext = A * y;

				double maxPrev = max;
				max = yNext.T * y / (y.T * y);

				if (steps == 1 || Abs(max - maxPrev) > e)
					continue;

				if (write)
					End(true);

				return max;
			}
		}
		public static double OppositeBoundary(double e, double lambda1)
		{
			Start("Найдём противоположную границу спектра",
				"степенным методом", " с заданной погрешностью", e);

			PowerIteration(A - max * Id(A.Rows), e, false);
			var result = max - lambda1 * Sign(max) * Sign(lambda1);

			Line();

			Left("Необходимая точность была достигнута на шаге");
			Right(steps);

			Left("Собственное число на противопложной границе спектра");
			Right(result);

			EndLine(oneColumnLength);
			Line();

			return result;
		}

		public static void Jacobi(SquareMatrix q, double e, bool write)
		{
			if (write)
				Start("Найдём все собственные числа и вектора",
					"методом Якоби", " с заданной погрешностью", e);

			A = q.DeepClone;

			int n = A.Rows;
			SquareMatrix X = Id(n);

			while (true)
			{
				double max = 0;
				int mi = 0, mj = 0;

				for (int i = 0; i < n; i++)
					for (int j = 0; j < n; j++)
						if ((i != j) & (Abs(A[i, j]) > max))
						{
							max = Abs(A[i, j]);
							mi = i;
							mj = j;
						}

				if (max < e)
				{
					JacobiEnd(q, X.T);
					return;
				}

				double d = Sqrt((A[mi, mi] - A[mj, mj]) *
					(A[mi, mi] - A[mj, mj]) + 4 * A[mi, mj] * A[mi, mj]);
				double ds = Sqrt(0.5 * (1 + Abs(A[mi, mi] - A[mj, mj]) / d));
				double dc = Sign(A[mi, mj] * (A[mi, mi] - A[mj, mj])) *
					Sqrt(0.5 * (1 - Abs(A[mi, mi] - A[mj, mj]) / d));

				var L = new SquareMatrix(n);

				for (int i = 0; i < n; i++)
					for (int j = 0; j < n; j++)
						if ((i == j) & (j != mi) & (j != mj))
							L[i, j] = 1;
						else
						if ((i != mj) & (i != mi) & (j != mj) & (j != mi))
							L[i, j] = 0;
						else
						if ((i == mi) & (j == mi))
							L[i, j] = dc;
						else
						if ((i == mj) & (j == mj))
							L[i, j] = dc;
						else
						if ((i == mi) & (j == mj))
							L[i, j] = ds;
						else
						if ((i == mj) & (j == mi))
							L[i, j] = -ds;

				X = X * L;
				SquareMatrix B = A.DeepClone;

				for (int i = 0; i < n; i++)
					for (int j = 0; j < n; j++)
						if ((i == mi) & (j == mj))
							B[i, j] = 0;
						else
						if ((i == mj) & (j == mi))
							B[i, j] = 0;
						else
						if ((i == mi) & (j != mj) & (j != mi))
							B[i, j] = dc * A[j, mi] - ds * A[j, mj];
						else
						if ((i != mj) & (i != mi) & (j == mi))
							B[i, j] = dc * A[i, mi] - ds * A[i, mj];
						else
						if ((i == mj) & (j != mi) & (j != mj))
							B[i, j] = ds * A[j, mi] + dc * A[j, mj];
						else
						if ((i != mi) & (i != mj) & (j == mj))
							B[i, j] = ds * A[i, mi] + dc * A[i, mj];
						else
						if ((i == mi) & (j == mi))
							B[i, j] = dc * dc * A[mi, mi] -
								2 * ds * dc * A[mi, mj] + ds * ds * A[mj, mj];
						else
						if ((i == mj) & (j == mj))
							B[i, j] = ds * ds * A[mi, mi] +
								2 * ds * dc * A[mi, mj] + dc * dc * A[mj, mj];
						else
							B[i, j] = A[i, j];

				A = B;
			}
		}
		static void JacobiEnd(SquareMatrix A0, SquareMatrix X)
		{
			Left("Необходимая точность была достигнута на шаге");
			Right(steps);

			Line();
			Left("Матрица A");
			Right(A);

			for (int j = 0; j < X.Rows; j++)
			{
				Line();

				double lambda = A[j, j];
				Left($"{j + 1}-е собственное число ");
				RightColor(lambda);

				var x = X[j].T;
				x /= x.N1;
				Left("Соответствующий ему собственный вектор");
				Right(x);

				OutputResidual(A0 * x - lambda * x);
			}

			EndLine(oneColumnLength);
		}
	}
}
