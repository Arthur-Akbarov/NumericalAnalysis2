using System;
using System.Linq;
using static System.Environment;
using static System.Math;
using static System.String;

namespace NumericalAnalysis2
{
	public class Matrix : IMatrix
	{
		public int Rows { get; protected set; }
		public int Cols { get; protected set; }

		public Vector[] X { get; protected set; }
		public Vector this[int i]
		{
			get => X[i];
			set => X[i] = value;
		}
		public double this[int i, int j]
		{
			get => X[i][j];
			set => X[i][j] = value;
		}

		protected Matrix() { }
		public Matrix(int rows, int cols, bool init)
		{
			Rows = rows;
			Cols = cols;

			if (!init)
				return;

			X = new Vector[Rows];
			for (int i = 0; i < Rows; i++)
				X[i] = new Vector(Cols, true);
		}
		public Matrix(double[,] a)
			: this(a.GetLength(0), a.GetLength(1), true)
		{
			for (int i = 0; i < Rows; i++)
				for (int j = 0; j < Cols; j++)
					this[i, j] = a[i, j];
		}

		public virtual IMatrix DeepAClone()
		{
			return DeepClone;
		}
		public Matrix DeepClone
		{
			get => new Matrix(Rows, Cols, false)
			{
				X = X.Select(x => x.DeepClone).ToArray()
			};
		}
		public Matrix T
		{
			get
			{
				var result = new Matrix(Cols, Rows, true);

				for (int i = 0; i < result.Rows; i++)
					for (int j = 0; j < result.Cols; j++)
						result[i, j] = this[j, i];

				return result;
			}
		}

		internal static bool AreDimensionsEqual(Matrix q, Matrix t)
		{
			if (q.Rows != t.Rows || q.Cols != t.Cols)
				throw new RankException("Dimensions are not equal.");

			return true;
		}
		internal static bool DoDimensionsMatch(Matrix q, Matrix t)
		{
			if (q.Cols != t.Rows)
				throw new RankException("Dimensions do not match.");

			return true;
		}
		internal static bool DoDimensionsMatch(Matrix q, Vector u)
		{
			if (u.Transpose || q.Cols != u.N)
				throw new RankException("Dimensions do not match.");

			return true;
		}
		internal static bool DoDimensionsMatch(Vector u, Matrix q)
		{
			if (!u.Transpose || q.Rows != u.N)
				throw new RankException("Dimensions do not match.");

			return true;
		}

		public static Matrix operator +(Matrix q, Matrix t)
		{
			AreDimensionsEqual(q, t);

			return new Matrix(q.Rows, q.Cols, false)
			{
				X = q.X.Zip(t.X, (x, y) => x + y).ToArray()
			};
		}
		public static Matrix operator -(Matrix q, Matrix t)
		{
			AreDimensionsEqual(q, t);

			return new Matrix(q.Rows, q.Cols, false)
			{
				X = q.X.Zip(t.X, (x, y) => x - y).ToArray()
			};
		}
		public static Matrix operator *(double d, Matrix q)
		{
			return new Matrix(q.Rows, q.Cols, false)
			{
				X = q.X.Select(x => d * x).ToArray()
			};
		}
		public static Matrix operator *(Matrix q, double d)
		{
			return d * q;
		}
		public static Matrix operator /(Matrix q, double d)
		{
			return new Matrix(q.Rows, q.Cols, false)
			{
				X = q.X.Select(x => x / d).ToArray()
			};
		}
		public static Matrix operator *(Matrix q, Matrix t)
		{
			DoDimensionsMatch(q, t);

			var result = new Matrix(q.Rows, t.Cols, true);

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
		public static Vector operator *(Matrix q, Vector u)
		{
			DoDimensionsMatch(q, u);

			return new Vector(q.X.Select(x => x * u).ToArray());
		}
		public static Vector operator *(Vector u, Matrix q)
		{
			DoDimensionsMatch(u, q);

			var result = new Vector(q.Cols, true);

			for (int i = 0; i < u.N; i++)
				for (int j = 0; j < q.Cols; j++)
					result[i] += u[j] * q[j, i];

			return result;
		}
		public static IMatrix operator *(Matrix q, IMatrix t)
		{
			return (t is Vector)
				? q * (Vector)t as IMatrix
				: q * (Matrix)t as IMatrix;
		}

		public bool ApproximatelyEquals(Matrix q)
		{
			AreDimensionsEqual(this, q);

			return (this - q).N1 < Worker.eps;
		}
		public static bool operator ==(Matrix q, Matrix t)
		{
			return q.ApproximatelyEquals(t);
		}
		public static bool operator !=(Matrix q, Matrix t)
		{
			return !(q == t);
		}

		public void SwapRows(int i, int j)
		{
			X.SwapInArray(i, j);
		}
		public void SubtractRow(int i, double d, int k)
		{
			X[i] -= d * X[k];
		}
		public void DivideRow(int i, double d)
		{
			X[i] /= d;
		}

		public static double Trace(Matrix q)
		{
			double result = 0;

			int n = Min(q.Rows, q.Cols);
			for (int i = 0; i < n; i++)
				result += q[i, i];

			return result;
		}

		/// <summary>
		/// Максимум сумм модулей элементов столбца есть кубическая норма матрицы
		/// </summary>
		public double N1
		{
			get
			{
				double result = 0;
				for (int i = 0; i < Cols; i++)
				{
					double d = 0;
					for (int j = 0; j < Rows; j++)
						d += Abs(this[j, i]);

					result = Max(result, d);
				}

				return result;
			}
		}
		public double N2
		{
			get
			{
				return 1;
			}
		}
		/// <summary>
		/// Максимум сумм модулей элементов строки есть кубическая норма матрицы
		/// </summary>
		public double N8
		{
			get { return X.Max(x => x.N8); }
		}

		public string ToString(int i, int f)
		{
			return X[i].ToString(f);
		}
		public string ToString(int f)
		{
			return Join(NewLine, X.Select(x => x.ToString(f)));
		}
		public override string ToString()
		{
			return Join(NewLine, X.Select(x => Join(", ", x)));
		}
	}
}

//public static implicit operator double[][] (Matrix q)
//{
//	int Rows = q.Rows;
//	int Cols = q.Cols;

//	double[][] result = new double[Rows][];
//	for (int i = 0; i < Rows; i++)
//		result[i] = new double[Cols];

//	for (int i = 0; i < Rows; i++)
//		result[i] = q.x[i].Clone();

//	return result;
//}
// ShallowCopy
//public static implicit operator Matrix(Vector[] a)
//{
//	if (a.Min(x => x.Cols) != a.Max(x => x.Cols) ||
//		a.Min(x => x.Rows) != a.Max(x => x.Rows))
//		throw new System.RankException("Dimensions are not equal.");

//	return new Matrix()
//	{
//		x = a,
//		cols = a.Length,
//		rows = a[0].Cols
//	};
//}
//public double N1
//{
//	get
//	{
//		double result = 0;
//		for (int i = 0; i < Rows; i++)
//		{
//			double d = 0;
//			for (int j = 0; j < Rows; j++)
//				d += Abs(this[i, j]);

//			result = Max(result, d);
//		}

//		return result;
//	}
//}