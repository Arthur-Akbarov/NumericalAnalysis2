using System;
using System.Linq;
using static System.Math;
using static NumericalAnalysis2.Printer;

namespace NumericalAnalysis2
{
	public class SquareMatrix : Matrix
	{
		static GaussPrint print;

		public SquareMatrix(int n, bool init)
			: base(n, n, init) { }
		public SquareMatrix(params double[] a)
		{
			double d = Sqrt(a.Length);
			int n = (int)d;

			if (d != n)
				throw new RankException("Dimension is not perfect square.");

			Rows = Cols = n;

			X = new Vector[n];
			for (int i = 0; i < n; i++)
				X[i] = new Vector(n, true);

			int k = 0;

			for (int i = 0; i < n; i++)
				for (int j = 0; j < n; j++)
					this[i, j] = a[k++];
		}

		public static SquareMatrix Id(int n)
		{
			var result = new SquareMatrix(n, true);

			for (int i = 0; i < n; i++)
				result[i, i] = 1;

			return result;
		}
		public static SquareMatrix CastToSquare(Matrix q)
		{
			if (q.Rows != q.Cols)
				throw new RankException("Matrix are not square.");

			var result = new SquareMatrix(q.Rows, false)
			{
				X = q.X
			};

			return result;
		}

		public override IMatrix DeepAClone()
		{
			return DeepClone;
		}
		public new SquareMatrix DeepClone
		{
			get => new SquareMatrix(Rows, false)
			{
				X = X.Select(x => x.DeepClone).ToArray()
			};
		}

		public SquareMatrix Diagonal
		{
			get
			{
				int n = Rows;
				var result = new SquareMatrix(n, true);

				for (int i = 0; i < n; i++)
					result[i, i] = this[i, i];

				return result;
			}
		}
		public SquareMatrix DiagonalInvert
		{
			get
			{
				int n = Rows;
				var result = new SquareMatrix(n, true);

				for (int i = 0; i < n; i++)
				{
					double d = this[i, i];

					if (d != 0)
						result[i, i] = 1 / d;
				}

				return result;
			}
		}

		public static double Determinant(SquareMatrix q, bool write = false)
		{
			int n = q.Rows;
			double result = 1;
			var a = q.DeepClone;

			int length = Max(51, n * Abs(f) + (n - 1)*  2 + indentL);
			print = (write) ? new GaussPrint(a, null, n, length) : null;

			print?.Start("Посчитаем определитель матрицы A методом Гаусса");

			for (int k = 0; k < n; k++)
			{
				if (!SwapToMaxRow(a, n, k))
					return 0;

				if (k != n - 1)
					print?.Subtract("Вычтем из более нижних {0}-ую строку", k);

				for (int i = k + 1; i < n; i++)
				{
					double d = a[i, k] / a[k, k];

					a[i, k] = 0;

					for (int j = k + 1; j < n; j++)
						a[i, j] -= d * a[k, j];
				}

				result *= a[k, k];
			}

			print?.Put("Перемножим диагональные элементы и получим ответ");
			print?.End("|A| = {0}", result);

			return result;
		}
		static bool SwapToMaxRow(SquareMatrix a, int n, int k)
		{
			int index = k;
			double max = Abs(a[k, k]);

			for (int j = k + 1; j < n; j++)
			{
				double d = Abs(a[j, k]);

				if (d > max)
				{
					max = Abs(a[j, k]);
					index = j;
				}
			}

			if (max == 0)
			{
				print?.End("Матрица A вырождена.");
				return false;
			}

			if (index != k)
			{
				print?.SwapRows("Поменяем {0}-ую и {1}-ую строки, " +
					"домножив {0}-ую на -1", k, index);

				a.DivideRow(k, -1);
				a.SwapRows(k, index);
			}

			return true;
		}

		public SquareMatrix Invert
		{
			get
			{
				int n = Rows;
				var t = new SquareMatrix(n, true);

				for (int i = 0; i < n; i++)
					for (int j = 0; j < n; j++)
						t[j, i] = (i % 2 == j % 2)
							? GetMinor(i, j)
							: -GetMinor(i, j);

				return CastToSquare(t / Det);
			}
		}
		double GetMinor(int x, int y)
		{
			int n = Rows;
			var m = new SquareMatrix(n - 1, true);

			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
					m[i, j] = this[i, j];

				for (int j = y + 1; j < n; j++)
					m[i, j - 1] = this[i, j];
			}

			for (int i = x + 1; i < n; i++)
			{
				for (int j = 0; j < y; j++)
					m[i - 1, j] = this[i, j];

				for (int j = y + 1; j < n; j++)
					m[i - 1, j - 1] = this[i, j];
			}

			return m.Det;
		}
		public bool Invertible
		{
			get => GaussMod.Invertible(this);
		}
		public double Det
		{
			get => Determinant(this);
		}

		internal static bool AreDimensionsEqual(SquareMatrix q, SquareMatrix t)
		{
			if (q.Rows != t.Rows)
				throw new RankException("Dimensions are not equal.");

			return true;
		}
		internal static bool DoDimensionsMatch(SquareMatrix q, SquareMatrix t)
		{
			return AreDimensionsEqual(q, t);
		}

		public static SquareMatrix operator +(SquareMatrix q, SquareMatrix t)
		{
			AreDimensionsEqual(q, t);

			return new SquareMatrix(q.Rows, false)
			{
				X = q.X.Zip(t.X, (x, y) => x + y).ToArray()
			};
		}
		public static SquareMatrix operator /(SquareMatrix q, double d)
		{
			return new SquareMatrix(q.Rows, false)
			{
				X = q.X.Select(x => x / d).ToArray()
			};
		}
		public static SquareMatrix operator *(SquareMatrix q, SquareMatrix t)
		{
			DoDimensionsMatch(q, t);

			var result = new SquareMatrix(q.Rows, true);

			for (int i = 0; i < result.Rows; i++)
				for (int j = 0; j < result.Cols; j++)
				{
					double sum = 0;

					for (int k = 0; k < q.Cols; k++)
						sum += q[i, k] * t[k, j];

					result[i, j] = sum;
				}

			return result;
		}
		public static SquareMatrix operator -(SquareMatrix q, SquareMatrix t)
		{
			AreDimensionsEqual(q, t);

			return new SquareMatrix(q.Rows, false)
			{
				X = q.X.Zip(t.X, (x, y) => x - y).ToArray()
			};
		}

		/// <summary>
		/// Кубическое число обусловленности квадратной матрицы
		/// </summary>
		public double Cond1
		{
			get => N1 * Invert.N1;
		}
		public double Cond2
		{
			get => N2 * Invert.N2;
		}
		/// <summary>
		/// Октаэдрическое число обусловленности квадратной матрицы
		/// </summary>
		public double Cond8
		{
			get => N8 * Invert.N8;
		}
	}
}