using System.Text;
using static NumericalAnalysis2.Printer;
using static System.Console;
using static System.Math;

namespace NumericalAnalysis2
{
	class Task1_2
	{
		static void Init()
		{
			twoColumn = true;

			f = (twoColumn) ? -8 : -8;

			shortIndentL = 2;
			indentL = 5;

			columnSize = (twoColumn) ? 58
				: Max(41, 3 * (Abs(f) + 2) + indentL);
		}

		static void Main(string[] args)
		{
			Init();
			Title = "Task1.2";
			OutputEncoding = Encoding.GetEncoding(1251);

			Left("Рассмотрим исходную систему AX = B");

			var A = new SquareMatrix(6.5176E-06, -8.0648E-03, 4.23528,
									 5.9176E-03, -0.80648, 1.46528,
									 0.87176, 0.79352, 0.91528);

			var B = new Vector(3.61628, 1.52097, 1.81150);

			Right(A, B);

			var ord = GaussOrd.Do(A, B, true);
			var mod = GaussMod.Do(A, B, true);
			var det = SquareMatrix.Determinant(A, true);
			var inv = GaussMod.Invert(A, true);

			Line();
			AlwaysLeft("X = A^(-1)*B");
			AlwaysRight(inv * B);
			AlwaysLeft("Число обусловленности матрицы A (окт.)");
			AlwaysRight(A.Cond1);
			AlwaysLeft("Число обусловленности матрицы A (куб.)");
			AlwaysRight(A.Cond8);

			ReadLine();
		}
	}
}
