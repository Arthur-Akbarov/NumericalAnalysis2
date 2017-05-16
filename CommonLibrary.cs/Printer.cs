using System;
using static System.Console;
using static System.Math;
using static System.String;

namespace NumericalAnalysis2
{
	public static class Printer
	{
		public static ConsoleColor colorBg = ConsoleColor.Black;
		public static ConsoleColor colorFg = ConsoleColor.Gray;
		public static ConsoleColor colorCfg = ConsoleColor.Green;

		public static int f;
		public static bool twoColumn;
		/// <summary>
		/// отступ перед векторами всегда и перед числами с twoColumn 
		/// </summary>
		public static int indentL;
		/// <summary>
		/// отступ перед числами без twoColumn 
		/// </summary>
		public static int alwaysIndentL;
		/// <summary>
		/// (макс длина предложения c twoColumn) + 1
		/// </summary>
		public static int columnSize;
		public static string Indent => new string(' ', indentL);
		public static string AlwaysIndent => new string(' ', alwaysIndentL);

		public static void Line(string format = "", params object[] arg)
		{
			Left(Format(format, arg));

			if (twoColumn)
				WriteLine();
		}

		public static void Left(string format = "", params object[] arg)
		{
			Write(Format(format, arg));

			if (twoColumn)
			{
				CursorLeft = columnSize;
				Write('|');
			}
			else
				WriteLine();
		}
		public static void AlwaysLeft(string format = "", params object[] arg)
		{
			Write(Format(format, arg).PadRight(columnSize) + '|');
		}

		public static void Right(SquareMatrix q, IMatrix t)
		{
			int n = q.Rows;

			if (t != null && n != t.Rows)
				throw new RankException("Row counts do not match.");

			new GaussPrint(q, t, n, 0).WhitePut(0, n);

			WriteLine();
		}
		public static void Right(Vector u)
		{
			WriteLine(Indent + u.ToString(f));
		}
		public static void Right(double d)
		{
			WriteLine(Indent + d.ToString(-Abs(f)));
		}
		public static void RightE(double d, int n = 0)
		{
			Write(Indent);

			if (twoColumn)
				Write(new string(' ', n * (Abs(f) + 2)));

			WriteLine(" {0:E2}", d);
		}
		public static void Right(string format = "", params object[] arg)
		{
			WriteLine(Indent + Format(format, arg));
		}
		public static void AlwaysRight(Vector u)
		{
			if (twoColumn)
				Write(Indent);
			else
				Write(AlwaysIndent);

			WriteLine(u.ToString(f));
		}
		public static void AlwaysRight(double d)
		{
			if (twoColumn)
				Write(Indent);
			else
				Write(AlwaysIndent);

			WriteLine(d.ToString(-Abs(f)));
		}
		public static void AlwaysRightE(double d, int n = 0)
		{
			if (twoColumn)
				Write(Indent + new string(' ', n * (Abs(f) + 2)));
			else
				Write(AlwaysIndent);

			WriteLine(" {0:E2}", d);
		}
		public static void AlwaysRight(string format = "", params object[] arg)
		{
			WriteLine(Indent + Format(format, arg));
		}

		public static void StartLine(int length)
		{
			DividingLine('*', length);
		}
		public static void EndLine(int length)
		{
			DividingLine('▲', length);
		}
		public static void DividingLine(char c, int length)
		{
			WriteLine(new string(c, (twoColumn) ? columnSize + 1 : length));
		}

		public static void ColorWrite(string s)
		{
			ForegroundColor = colorCfg;
			Write(s);
			ForegroundColor = colorFg;
		}
	}
}
