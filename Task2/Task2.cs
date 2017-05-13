using System.Text;
using static NumericalAnalysis2.Printer;
using static System.Console;

namespace NumericalAnalysis2
{
	class Task2
	{
		const double e = 1e-10;
		const int steps = 4;

		static void Init()
		{
			twoColumn = true;
			f = -10;

			indentL = shortIndentL = 2;
			columnSize = (twoColumn) ? 57 : 55;
		}

		static void Main(string[] args)
		{
			Init();
			Title = "Task2, Итерационные методы решения линейных систем";
			OutputEncoding = Encoding.GetEncoding(1251);

			var A = new SquareMatrix(100, 6, 2, 2, 100, 5, 3, 4, 100);
			//var A = new SquareMatrix(0.082012, 0.059814, -0.03172,
			//						0.05929, 0.00046623, -0.030383,
			//						-0.031204, -0.030532, 0.040177);
			var B = new Vector(60, 70, 80);

			Left("Рассмотрим исходную систему AX = B");
			Right(A, B);

			Left("Решение модифицированным методом Гаусса");
			Right(GaussMod.Do(A, B).ToString(f));

			Line();
			Interation.FixedPoint(A, B, e);

			Line();
			Interation.FixedPoint(A, B, steps);

			Line();
			Interation.Seidel(A, B, steps);

			Line();
			Interation.Seidel(A, B, e);

			ReadKey();
		}
	}
}
//int n = 4;
//var A = new SquareMatrix(n, true);

//for (int i = 0; i < n; i++)
//	for (int j = 0; j < n; j++)
//		if (i != j)
//			A[i, j] = 1 / Pow(2.0, Abs(i - j) + 1);
//		else
//			A[i, j] = 0;

//var B = new Vector(n);
//for (int i = 0; i < n; i++)
//	B[i] = 1;

//var x = new Vector(.6, .7, .8);

//var A = new SquareMatrix(0.082012, 0.059814, -0.03172,
//						0.05929, 0.00046623, -0.030383,
//						-0.031204, -0.030532, 0.040177);

//var B = new Vector(1, 2, 3);

//var A = new SquareMatrix(-1.14896, -0.53716, 0.78959,
//						-0.53716, 0.88917, 0.19536,
//						0.78959, 0.19536, -1.28186);

//var B = new Vector(7.570463, 8.876384, 3.411906);