using System.Text;
using static NumericalAnalysis2.Printer;
using static System.Console;

namespace NumericalAnalysis2
{
	class Task3
	{
		const double e = 1e-10;
		const int steps = 4;

		static void Init()
		{
			twoColumn = true;
			f = -10;

			indentL = alwaysIndentL = 2;
			columnSize = (twoColumn) ? 55 : 53;
		}
		static void Main(string[] args)
		{
			Init();
			Title = "Task3 Проблема собственных значений";
			OutputEncoding = Encoding.GetEncoding(1251);

			var A = new SquareMatrix(-0.930161, -0.257696, 0.452554,
									 -0.257696, 0.650218, 0.071929,
									  0.452554, 0.071929, -0.971119);

			Left("Рассмотрим матрицу A");
			Right(A);

			double lambda1 = Eigenvalue.PowerIteration(A, e, true);
			Eigenvalue.DotProduct(A, e, true);
			double lambda2 = Eigenvalue.OppositeBoundary(e, lambda1);

			double trace = A.Trace;
			Left("След матрицы A");
			Right(trace);

			Left("Третье собственное число");
			RightColor(trace - lambda1 - lambda2);
			Line();

			Eigenvalue.Jacobi(A, e, true);

			ReadKey();
		}
	}
}
