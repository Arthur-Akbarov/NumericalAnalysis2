using System;
using System.Linq;
using static System.Math;
using static System.String;

namespace NumericalAnalysis2
{
	public class Vector : IMatrix
	{
		public const double eps = 1e-11;

		public int Rows => (Transpose) ? 1 : N;
		public int Cols => (Transpose) ? N : 1;

		public bool Transpose { get; private set; }
		public int N { get; private set; }
		public double[] X { get; private set; }
		public double this[int i]
		{
			get => X[i];
			set => X[i] = value;
		}

		private Vector(bool t = false)
		{
			Transpose = t;
			X = null;
			N = 0;
		}
		public Vector(int n, bool t = false)
		{
			Transpose = t;
			X = new double[n];
			N = X.Length;
		}
		public Vector(int n, double k, bool t = false)
		{
			Transpose = t;
			X = Enumerable.Repeat(k, n).ToArray();
			N = X.Length;
		}
		public Vector(params double[] a)
		{
			Transpose = false;
			X = a.Clone() as double[];
			N = X.Length;
		}

		public IMatrix DeepAClone
		{
			get => DeepClone;
		}
		public Vector DeepClone
		{
			get => new Vector()
			{
				Transpose = Transpose,
				X = X.Clone() as double[],
				N = N
			};
		}
		public Vector T
		{
			get => new Vector()
				{
					Transpose = !Transpose,
					X = X.Clone() as double[],
					N = N
				};
		}

		internal static bool AreDimensionsEqual(Vector u, Vector v)
		{
			if (u.Transpose != v.Transpose || u.N != v.N)
				throw new RankException("Dimensions are not equal.");

			return true;
		}
		internal static bool DoDimensionsMatch(Vector u, Vector v)
		{
			if (!u.Transpose || v.Transpose || u.N != v.N)
				throw new RankException("Dimensions do not match.");

			return true;
		}

		public static Vector operator -(Vector u)
		{
			return -1 * u;
		}
		public static Vector operator +(Vector u, Vector v)
		{
			AreDimensionsEqual(u, v);

			return new Vector(u.Transpose)
			{
				X = u.X.Zip(v.X, (x, y) => x + y).ToArray(),
				N = u.N
			};
		}
		public static Vector operator -(Vector u, Vector v)
		{
			AreDimensionsEqual(u, v);

			return new Vector(u.Transpose)
			{
				X = u.X.Zip(v.X, (x, y) => x - y).ToArray(),
				N = u.N
			};
		}
		public static Vector operator *(double d, Vector u)
		{
			return new Vector(u.Transpose)
			{
				X = u.X.Select(x => d * x).ToArray(),
				N = u.N
			};
		}
		public static Vector operator *(Vector u, double d)
		{
			return d * u;
		}
		public static Vector operator /(Vector u, double d)
		{
			return new Vector(u.Transpose)
			{
				X = u.X.Select(x => x / d).ToArray(),
				N = u.N
			};
		}
		public static double operator *(Vector u, Vector v)
		{
			DoDimensionsMatch(u, v);

			return u.X.Zip(v.X, (x, y) => x * y).Sum();
		}

		public bool ApproximatelyEquals(Vector u)
		{
			AreDimensionsEqual(this, u);

			return (this - u).N8 < Worker.eps;
		}
		public static bool operator ==(Vector u, Vector v)
		{
			return u.ApproximatelyEquals(v);
		}
		public static bool operator !=(Vector u, Vector v)
		{
			return !(u == v);
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

		/// <summary>
		/// Максимум модулей элементов есть кубическая норма вектора
		/// </summary>
		public double N1 => X.Max(x => Abs(x));
		/// <summary>
		/// Евклидова норма
		/// </summary>
		public double N2 => Sqrt(X.Sum(x => x * x));
		/// <summary>
		/// Сумма модулей всех элементов есть октаэдрическая норма вектора
		/// </summary>
		public double N8 => X.Sum(x => Abs(x));

		public string ToString(int i, int f)
		{
			return X[i].ToString(f);
		}
		public string ToString(int f)
		{
			return Join("  ", X.Select(x => x.ToString(f)));
		}
		public override string ToString()
		{
			return Join(", ", X);
		}
	}
}
