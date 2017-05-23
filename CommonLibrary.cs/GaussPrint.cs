using System.Text;
using static NumericalAnalysis2.Printer;
using static NumericalAnalysis2.Worker;
using static System.Console;
using System.Linq;
using static System.Environment;
using static System.String;

namespace NumericalAnalysis2
{
	public class GaussPrint
	{
		readonly SquareMatrix a0, a;
		readonly IMatrix b0, b;
		readonly int n, oneColumnLength;
		readonly StringBuilder sb;
		int step;

		public GaussPrint(SquareMatrix a, IMatrix b, int n, int length)
		{
			a0 = a.DeepClone;
			b0 = b?.DeepAClone;
			this.a = a;
			this.b = b;
			this.n = n;
			this.oneColumnLength = length;
			sb = new StringBuilder();
		}

		internal void Start(string s1, string s2 = null)
		{
			StartLine(oneColumnLength);

			Write(s1);
			WriteColor(s2);
			Line();

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
			Left(s);
			Right(d);

			Line();
			EndLine(oneColumnLength);
		}
		internal void End(string s)
		{
			Left(s);

			WhitePut(0, n);

			WriteLine();

			if (b0?.Cols == 1)
			{
				OutputResidual(a0 * (Vector)b  - (Vector)b0);
				Line();
			}

			EndLine(oneColumnLength);
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
			WriteColor($" [{k + 1}]".PadRight(indentL));

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

			WriteColor(a[k, i].ToString(f));

			for (int j = i + 1; j < n; j++)
				sb.Append("  " + a[k, j].ToString(f));

			AppendBRow(k);

			Write(sb);
			sb.Clear();
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
