using static NumericalAnalysis2.Printer;
using System;
using System.Text;
using static NumericalAnalysis2.Worker;
using static System.Console;
using static System.Environment;
using static System.String;
using static System.Math;

namespace NumericalAnalysis2
{
	public class GaussPrint
	{
		readonly SquareMatrix a;
		readonly IMatrix b;
		readonly int n, length;
		StringBuilder sb;
		int step;

		public GaussPrint(SquareMatrix a, IMatrix b, int n, int length)
		{
			this.a = a;
			this.b = b;
			this.n = n;
			this.length = length;
			sb = new StringBuilder();
		}

		internal void Start(string s)
		{
			StartLine(length);
			Line(s);
			Line();
		}
		internal void Normalize(string s, int k, int i)
		{
			Left(++step + ") " + s, k + 1);

			WhitePut(0, k);
			ColorPut(k, i);
			WhitePut(k + 1, n);

			WriteLine();
		}
		internal void Subtract(string s, int k)
		{
			Left(++step + ") " + s, k + 1);

			WhitePut(0, k);
			ColorPut(k);
			WhitePut(k + 1, n);

			WriteLine();
		}
		internal void SwapRows(string s, int k, int i)
		{
			if (k > i)
				Swap(ref k, ref i);

			Left(++step + ") " + s, k + 1, i + 1);

			WhitePut(0, k);
			ColorPut(k, k);
			WhitePut(k + 1, i);
			ColorPut(i, k);
			WhitePut(i + 1, n);

			WriteLine();
		}
		internal void NewPart(string s)
		{
			Line(s);
			Line();
		}
		internal void Put(string s)
		{
			Left(++step + ") " + s);

			WhitePut(0, n);

			WriteLine();
		}
		internal void End(string s, double d)
		{
			Left(s, d);

			WriteLine();
			EndLine(length);
		}
		internal void End(string s)
		{
			Left(s);

			WhitePut(0, n);

			WriteLine();
			EndLine(length);
		}

		public void WhitePut(int start, int end)
		{
			for (int i = start; i < end; i++)
			{
				sb.Append(Indent + a.ToString(i, f));
				AppendBRow(i);
			}

			Write(sb);
			sb.Clear();
		}
		void ColorPut(int k)
		{
			ColorWrite(Format(" [{0}]", k + 1).PadRight(indentL));

			sb.Append(a.ToString(k, f));
			AppendBRow(k);

			Write(sb);
			sb.Clear();
		}
		void ColorPut(int k, int i)
		{
			sb.Append(Indent);

			for (int j = 0; j < i; j++)
				sb.Append(a[k, j].ToString(f) + "  ");

			Write(sb);
			sb.Clear();

			ColorWrite(a[k, i].ToString(f));

			for (int j = i + 1; j < n; j++)
				sb.Append("  " + a[k, j].ToString(f));

			AppendBRow(k);

			Write(sb);
			sb.Clear();
		}

		void ColorWrite(string s)
		{
			ForegroundColor = colorCfg;
			Write(s);
			ForegroundColor = colorFg;
		}
		void AppendBRow(int i)
		{
			if (b != null)
				sb.Append("  |  " + b.ToString(i, f));

			sb.Append(NewLine);

			if (twoColumn)
				sb.Append(new string(' ', columnSize) + '|');
		}
	}
}
