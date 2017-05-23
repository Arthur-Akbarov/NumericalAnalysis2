using System;
using System.Text;
using static NumericalAnalysis2.Worker;
using static System.Console;
using static System.Environment;
using static System.Math;
using static NumericalAnalysis2.Printer;

namespace NumericalAnalysis2
{
	public static class Task1_1
	{
		static bool write;

		static void Init()
		{
			twoColumn = true;
			write = true;

			f = -10;

			indentL = 0;
			alwaysIndentL = 2;

			if (twoColumn)
				indentL = 2;
			if (write)
				indentL = 5;

			columnSize = (write || twoColumn) ? 58 : 48;
		}
		static void Main(string[] args)
		{
			Init();
			OutputEncoding = Encoding.GetEncoding(1251);

			string s = "mod";
			if (args.Length != 0 && args[0] == s)
			{
				Title = "Task1.1 Модифицированный метод Гаусса";
				Start(GaussMod.Do);
				ReadLine();
			}
			else
			{
				System.Diagnostics.Process.Start("Task1.1.exe", s);

				columnSize = (write || twoColumn) ? 58 : 48;
				Title = "Task1.1 Обычный метод Гаусса";
				Start(GaussOrd.Do);
				ReadLine();
			}
		}
		static void Start(Func<SquareMatrix, Vector, bool, Vector> gauss)
		{
			var A = new SquareMatrix(1.00, 0.99, 0.99, 0.98);
			var B = new Vector(1.99, 1.97);
			Left("Рассмотрим исходную систему AX = B");
			Right(A, B);

			var X = gauss(A, B, write);
			Left("Её решением является вектор X");
			Right(X);
			var dB = new Vector(-0.000097, 0.000106);
			Left("Возмутим правую часть системы на вектор dB");
			Right(dB);
			Line();

			Left("Рассмотрим систему A(X + dX) = B + dB");
			Right(A, B + dB);

			var Y = gauss(A, B + dB, write);
			Left("Её решением является вектор Y = X + dX");
			Right(Y);
			var dX = Y - X;
			Left("Откуда находим вектор dX = Y - X");
			Right(dX);
			Line();

			var cond1 = A.Cond1;
			AlwaysLeft("Число обусловленности матрицы A (куб.)");
			AlwaysRightColor(cond1);
			AlwaysLeft("Теоретическая относительная погрешность (куб.)");
			AlwaysRight(cond1 * dB.N1 / B.N1);
			AlwaysLeft("Фактическая относительная погрешность (куб.)");
			AlwaysRight(dX.N1 / X.N1);
			Line();

			var cond8 = A.Cond8;
			AlwaysLeft("Число обусловленности матрицы A (окт.)");
			AlwaysRightColor(cond8);
			AlwaysLeft("Теоретическая относительная погрешность (окт.)");
			AlwaysRight(cond8 * dB.N8 / B.N8);
			AlwaysLeft("Фактическая относительная погрешность (окт.)");
			AlwaysRight(dX.N8 / X.N8);
		}

		//static void Start2()
		//{
		//	var A = new SquareMatrix(-0.636330163831398, 1.12750825408205,
		//		1.13945581868991, 0.42579353469937, -1.23596239973969,
		//		0.00838666140309009, 0.236923671408766, -0.00235524881689462,
		//		1.42163516083347E-05);

		//	WriteLine(A.Invert.ToString(20));
		//	WriteLine();
		//	WriteLine("Кубическое");
		//	WriteLine(A.Cond1);
		//	WriteLine(A.Invert.Cond1);
		//	WriteLine();
		//	WriteLine("Октаэдрическое");
		//	WriteLine(A.Cond8);
		//	WriteLine(A.Invert.Cond8);

		//}
	}
}
