using System.Globalization;
using static System.Math;
using static System.String;

namespace NumericalAnalysis2
{
	public static class Worker
	{
		public const double eps = 1e-11;

		public static string ToString(this double obj, int ff, int f)
		{
			return Format("{0," + ff + "}", obj.ToString(f));
		}
		public static string ToString(this object obj, int ff, int f)
		{
			if (obj is double)
				obj = ToString((double)obj, f);

			return Format("{0," + ff + "}", obj);
		}
		public static string ToString(this double d, int maxLength)
		{
			char sign = (d < 0) ? '-' : ' ';
			d = Abs(d);

			var nfi = new NumberFormatInfo
			{
				NumberGroupSeparator = " "
			};

			string module = d.ToString("N0", nfi);

			int frac = Abs(maxLength) - 2 - module.Length;

			if (frac > 0)
			{
				module = d.ToString("N" + frac, nfi);
				module = module.TrimEnd('0');
				module = module.TrimEnd('.');
			}

			return Format("{0," + maxLength + "}", sign + module);
		}
		public static void Swap<T>(ref T a, ref T b)
		{
			T temp;
			temp = a;
			a = b;
			b = temp;
		}
		public static void SwapInArray<T>(this T[] a, int i, int j)
		{
			T temp;
			temp = a[i];
			a[i] = a[j];
			a[j] = temp;
		}
	}
}

//public static string ToString(this double number, int length)
//{
//	string str = Format("{0:N" + Abs(length) + "}", number);

//	if (str.Length > Abs(length))
//		str = str.Substring(0, Abs(length));

//	if (str.Contains("."))
//	{
//		str = str.TrimEnd('0');

//		if (str.EndsWith("."))
//			str.Remove(str.Length - 1);
//	}

//	return (length > 0)
//		? str.TrimEnd('.').PadLeft(length)
//		: str.TrimEnd('.').PadRight(-length);
//}