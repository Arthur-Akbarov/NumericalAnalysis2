using System.Linq;
using System;
using static System.Console;
using static System.Math;
using static System.String;

namespace NumericalAnalysis2
{
	public static class Printer
	{
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
			Write(Format(format, arg));

			if (twoColumn)
				Left();

			WriteLine();
		}
		public static void Left()
		{
			CursorLeft = columnSize;
			Write('|');
		}
		public static void Left(string format, params object[] arg)
		{
			Write(Format(format, arg));

			if (twoColumn)
				Left();
			else
				WriteLine();
		}
		public static void AlwaysLeft(string format = "", params object[] arg)
		{
			Write(format, arg);
			Left();
		}

		public static void RightE(double d)
		{
			WriteLine(Indent + " {0:E2}", d);
		}
		public static void Right(SquareMatrix q, IMatrix t = null)
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
		public static void Right(params double[] d)
		{
			Write(Indent);
			WriteLine(Join("  ", d.Select(x => x.ToString(-Abs(f)))));
		}
		public static void RightArray(int ff, params double[] d)
		{
			Write(Indent);
			WriteLine(Join("  ", d.Select(x => x.ToString(ff, f))));
		}
		public static void RightArray(int ff, params object[] args)
		{
			Write(Indent);
			WriteLine(Join("  ", args.Select(x => x.ToString(ff, f))));
		}
		public static void NextRight(params double[] d)
		{
			if (twoColumn)
				Left();

			Write(Indent);
			WriteLine(Join("  ", d.Select(x => x.ToString(-Abs(f)))));
		}
		public static void Right(string format = "", params object[] arg)
		{
			WriteLine(Indent + Format(format, arg));
		}

		public static void AlwaysRightE(double d, int n = 0)
		{
			if (twoColumn)
				Write(Indent + new string(' ', n * (Abs(f) + 2)));
			else
				Write(AlwaysIndent);

			WriteLine(" {0:E2}", d);
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
		public static void AlwaysRight(string format = "", params object[] arg)
		{
			WriteLine(Indent + Format(format, arg));
		}

		public static void WriteColor(string s)
		{
			ForegroundColor = ConsoleColor.Green;
			Write(s);
			ForegroundColor = ConsoleColor.Gray;
		}
		public static void RightColor(double d)
		{
			ForegroundColor = ConsoleColor.Green;
			Right(d);
			ForegroundColor = ConsoleColor.Gray;
		}
		public static void RightColor(Vector u)
		{
			ForegroundColor = ConsoleColor.Green;
			Right(u);
			ForegroundColor = ConsoleColor.Gray;
		}
		public static void AlwaysRightColor(double d)
		{
			ForegroundColor = ConsoleColor.Green;
			AlwaysRight(d);
			ForegroundColor = ConsoleColor.Gray;
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

		public static void OutputResidual(Vector u)
		{
			string indent = new string(' ', Max(3, Abs(f) - 7));
			string s = " {0:E2}" + indent + "{1:E2}" + indent + "{2:E2}";

			Left("Вектор невязки");
			Right(u);

			Left("Его кубическая, евклидова и октаэдрическая нормы ");
			WriteLine(Indent + s, u.N1, u.N2, u.N8);
		}
	}
}
