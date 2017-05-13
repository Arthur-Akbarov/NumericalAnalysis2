//using System.Linq;
//using static System.Math;
//using static System.String;

//namespace NumericalAnalysis2
//{
//	public class Row
//	{
//		public int Cols { get; }
//		public double[] x;
//		public double this[int i]
//		{
//			get => x[i];
//			set => x[i] = value;
//		}

//		public Row(int n)
//		{
//			x = new double[n];
//			Cols = n;
//		}
//		// ShallowCopy
//		public Row(params double[] a)
//		{
//			x = a;
//			Cols = x.Length;
//		}

//		// ShallowCopy
//		public static implicit operator Row(double[] a)
//		{
//			return new Row(a);
//		}
//		public static implicit operator double[] (Row a)
//		{
//			return a.x;
//		}

//		// DeepCopy
//		public Row Clone()
//		{
//			return new Row(x.Clone() as double[]);
//		}

//		// DeepCopy
//		public static Row operator +(Row u, Row v)
//		{
//			return u.x.Zip(v.x, (x, y) => x + y).ToArray();
//		}
//		public static Row operator -(Row u, Row v)
//		{
//			return u.x.Zip(v.x, (x, y) => x - y).ToArray();
//		}
//		public static Row operator *(double d, Row u)
//		{
//			return u.x.Select(x => d * x).ToArray();
//		}
//		public static Row operator *(Row u, double d)
//		{
//			return d * u;
//		}

//		public double N1 => x.Max(x => Abs(x));
//		public double N2 => Sqrt(x.Sum(x => x * x));
//		public double N8 => x.Sum(x => Abs(x));

//		public string ToString(int f)
//		{
//			return Join(" ", x.Select(x => x.ToString(f)));
//		}
//		public override string ToString()
//		{
//			return Join(", ", x);
//		}
//	}
//}