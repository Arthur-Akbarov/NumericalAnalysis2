using System.Linq;
using static System.Console;
using static System.Math;

namespace NumericalAnalysis2
{
	static class Table
	{
		static int top;
		static int indent;
		static int[] ff;
		static int[] f;

		public static void Reset()
		{
			indent += Max(22, ff.Select(Abs).Sum()) + 5;
			SetCursorPosition(indent, top);
		}
		public static void Init()
		{
			top = CursorTop;
			indent = 0;
		}
		public static void SetCell(params int[] ff)
		{
			Table.ff = ff;
		}
		public static void SetLength(params int[] f)
		{
			Table.f = f;
		}
		public static void PutRow(params string[] ss)
		{
			CursorLeft = indent;
			Write("{0," + ff[0] + "}", ss[0]);

			for (int i = 1; i < ss.Length; i++)
				Write("  {0," + ff[i] + "}", ss[i]);

			WriteLine();
		}
		public static void PutRow(params double[] dd)
		{
			CursorLeft = indent;
			Write("{0," + ff[0] + "}", dd[0].ToString(f[0]));

			for (int i = 1; i < dd.Length; i++)
				Write("  {0," + ff[i] + "}", dd[i].ToString(f[i]));

			WriteLine();
		}
		public static void PutLine(string s = "")
		{
			CursorLeft = indent;
			WriteLine(s);
		}
	}
}
