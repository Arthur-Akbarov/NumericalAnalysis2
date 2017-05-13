using System;
using System.Globalization;
using static System.Math;
using static System.String;
using static System.Console;
using System.Text;

namespace NumericalAnalysis2
{
	public static class Worker
	{
		public const double eps = 1e-11;

		public static string ToString(this double d, int maxLength)
		{
			char c = (d < 0) ? '-' : ' ';
			d = Abs(d);

			var nfi = new NumberFormatInfo { NumberGroupSeparator = " " };
			string s = d.ToString("N0", nfi);

			int frac = Abs(maxLength) - 2 - s.Length;

			if (frac > 0)
				s = d.ToString("0." + new string('#', frac));

			return Format("{0," + maxLength + "}", c + s);
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