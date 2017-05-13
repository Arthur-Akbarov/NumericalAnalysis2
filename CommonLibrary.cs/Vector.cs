using System;
using System.Linq;
using static System.Math;
using static System.String;

namespace NumericalAnalysis2
{
	public class Vector : IMatrix
	{
		public int Rows => (Transpose) ? 1 : N;
		public int Cols => (Transpose) ? N : 1;

		public bool Transpose { get; private set; }
		public int N { get; private set; }
		private double[] x;
		public double this[int i]
		{
			get => x[i];
			set =>x[i] = value;
		}

		private Vector(bool t = false)
		{
			Transpose = t;
			x = null;
			N = 0;
		}
		public Vector(int n, bool t = false)
		{
			Transpose = t;
			x = new double[n];
			N = x.Length;
		}
		public Vector(params double[] a)
		{
			Transpose = false;
			x = a.Clone() as double[];
			N = x.Length;
		}

		public IMatrix DeepAClone()
		{
			return DeepClone;
		}
		public Vector DeepClone
		{
			get => new Vector()
			{
				Transpose = Transpose,
				x = x.Clone() as double[],
				N = N
			};
		}
		public Vector T
		{
			get
			{
				return new Vector()
				{
					Transpose = !Transpose,
					x = x.Clone() as double[],
					N = N
				};
			}
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
				x = u.x.Zip(v.x, (x, y) => x + y).ToArray(),
				N = u.N
			};
		}
		public static Vector operator -(Vector u, Vector v)
		{
			AreDimensionsEqual(u, v);

			return new Vector(u.Transpose)
			{
				x = u.x.Zip(v.x, (x, y) => x - y).ToArray(),
				N = u.N
			};
		}
		public static Vector operator *(double d, Vector u)
		{
			return new Vector(u.Transpose)
			{
				x = u.x.Select(x => d * x).ToArray(),
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
				x = u.x.Select(x => x / d).ToArray(),
				N = u.N
			};
		}
		public static double operator *(Vector u, Vector v)
		{
			DoDimensionsMatch(u, v);

			return u.x.Zip(v.x, (x, y) => x * y).Sum();
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
			x.SwapInArray(i, j);
		}
		public void SubtractRow(int i, double d, int k)
		{
			x[i] -= d * x[k];
		}
		public void DivideRow(int i, double d)
		{
			x[i] /= d;
		}

		/// <summary>
		/// Максимум модулей элементов есть кубическая норма вектора
		/// </summary>
		public double N1 => x.Max(x => Abs(x));
		/// <summary>
		/// Евклидова норма
		/// </summary>
		public double N2 => Sqrt(x.Sum(x => x * x));
		/// <summary>
		/// Сумма модулей всех элементов есть октаэдрическая норма вектора
		/// </summary>
		public double N8 => x.Sum(x => Abs(x));

		public string ToString(int i, int f)
		{
			return x[i].ToString(f);
		}
		public string ToString(int f)
		{
			return Join("  ", x.Select(x => x.ToString(f)));
		}
		public override string ToString()
		{
			return Join(", ", x);
		}
	}
}
// ShallowCopy
//public static implicit operator Vector(double[] a)
//{
//	return new Vector(a);
//}
//public static implicit operator double[] (Vector a)
//{
//	return a.x;
//}
//public static bool AreDimensionsEqual(Vector u, Vector v)
//{
//	if (u.Rows != v.Rows || u.Cols != v.Cols)
//		throw new RankException("Dimensions are not equal.");

//	return true;
//}
//public static bool DoDimensionsMatch(Vector u, Vector v)
//{
//	if (u.Cols != v.Rows)
//		throw new RankException("Dimensions do not match.");

//	return true;
//}
//public static bool AreDimensionsEqual(Vector u, Vector v)
//{
//	if (!u.transpose || v.transpose || u.N != v.N)
//		throw new RankException("Dimensions are not equal.");

//	return true;
//}
//public static bool DoDimensionsMatch(Vector u, Vector v)
//{
//	if (u.transpose != v.transpose || u.N != v.N)
//		throw new RankException("Dimensions do not match.");

//	return true;
//}