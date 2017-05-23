using static NumericalAnalysis2.Printer;
using static System.Console;
using static System.Math;

namespace NumericalAnalysis2
{
	static class Tridiagonal
	{
		static int oneColumnLength;

		static readonly double alpha1 = 2;
		static readonly double alpha2 = 2;
		static readonly double alpha = 0;
		static readonly double beta1 = 0;
		static readonly double beta2 = 1;
		static readonly double beta = -2 / Sqrt(3);
		static readonly double a = 0;
		static readonly double b = 1;

		static int n;
		static double h;
		static double[] A;
		static double[] B;
		static double[] C;
		static double[] s;
		static double[] t;
		static SquareMatrix M;
		static Vector G;
		static Vector z;
		static bool write;

		public static void Start(int n, bool write)
		{
			Init(n, write);

			if (write)
				Greeting();
			else
				Write($"Размер сетки возьмём за {n,2}");

			var y1 = GetSolution1();
			var y2 = GetSolution2();

			var r1 = y1 - z;
			var r2 = y2 - z;

			if (write)
			{
				int ff = -Max(11, Abs(f));

				WriteLine();
				RightArray(ff, "точка", "точное", "первое", "погрешность",
					"второе", "погрешность");
				RightArray(ff, "", "решение", "решение", "", "решение", "");

				for (int i = 0; i < n + 1; i++)
					RightArray(ff, a + i * h, z[i], y1[i], r1[i], y2[i], r2[i]);

				RightArray(ff, "норма", "", "", r1.N1, "", r2.N1);
				WriteLine();
			}
			else
			{
				Write(" -> норма");
				Right(r1.N1, r2.N1);
			}
		}

		static void Greeting()
		{
			Line("Дано уравнение");
			Line("y'' - 8y / (1 + 2x)^2 = 36 / 2(1 + 2x)^1.5");
			Line();
			Line("Граничные условия");
			Line("2y(0) - 2y'(0) = 0");
			Line("y'(1) = -2 / 3^0.5");
			Line();
			Line("Точное решение имеет вид");
			Line("y = -2(1 + 2x)^0.5");
			Line();
			Line("Найдём численное решение методом прогонки");
			Line($"Размер сетки возьмём за {n}");
		}

		static Vector GetSolution1()
		{
			if (write)
			{
				Line();
				StartLine(oneColumnLength);
				Line("I Аппроксимация дифференциального уравнения разностным");
				Line();
			}

			B[0] = -(alpha1 + alpha2 / h);
			C[0] = -(alpha2 / h);
			G[0] = alpha;

			A[n] = -(beta2 / h);
			B[n] = -(beta1 + beta2 / h);
			G[n] = beta;

			FindST();
			return GetSolution();
		}
		static Vector GetSolution2()
		{
			if (write)
			{
				Line();
				StartLine(oneColumnLength);
				Line("II Аппроксимация граничных условий");
				Line();
			}

			B[0] = -alpha1 + alpha2 * (A[1] / C[1] - 3) / 2 / h;
			C[0] = -alpha2 * (4 - B[1] / C[1]) / 2 / h;
			G[0] = alpha - alpha2 * G[1] / C[1] / 2 / h;

			A[n] = -beta2 * (4 - B[n - 1] / A[n - 1]) / 2 / h;
			B[n] = -beta1 - beta2 * (3 - C[n - 1] / A[n - 1]) / 2 / h;
			G[n] = beta - beta2 * G[n - 1] / A[n - 1] / 2 / h;

			FindST();
			return GetSolution();
		}

		static void Init(int k, bool w)
		{
			n = k;
			write = w;

			h = (b - a) / n;
			oneColumnLength = Max(54, (2 + n) * (2 + Abs(f)) + 1 + indentL);

			A = new double[n + 1];
			B = new double[n + 1];
			C = new double[n + 1];
			s = new double[n + 1];
			t = new double[n + 1];
			M = new SquareMatrix(n + 1);
			G = new Vector(n + 1);
			z = new Vector(n + 1);

			for (int i = 0; i < n + 1; i++)
				z[i] = Precise(a + i * h);

			for (int i = 1; i < n; i++)
			{
				double x = a + i * h;
				A[i] = -P(x) / h / h - Q(x) / 2 / h;
				B[i] = -2 * P(x) / h / h - R(x);
				C[i] = -P(x) / h / h + Q(x) / 2 / h;
				G[i] = F(x);

				M[i, i - 1] = A[i];
				M[i, i] = -B[i];
				M[i, i + 1] = C[i];
			}
		}
		static void FindST()
		{
			M[0, 0] = -B[0];
			M[0, 1] = C[0];

			M[n, n - 1] = A[n];
			M[n, n] = -B[n];

			if (write)
			{
				Left("Трёхдиагональная матрица");
				Right(M, G);
			}

			s[0] = C[0] / B[0];
			t[0] = -G[0] / B[0];

			for (int i = 1; i < n + 1; i++)
			{
				s[i] = C[i] / (B[i] - A[i] * s[i - 1]);
				t[i] = (A[i] * t[i - 1] - G[i]) / (B[i] - A[i] * s[i - 1]);
			}

			if (write)
			{
				Left("Прогоночные коэффициенты s и t");
				Right(s[0], t[0]);

				for (int i = 1; i < n + 1; i++)
					NextRight(s[i], t[i]);
			}
		}
		static Vector GetSolution()
		{
			var result = new Vector(n + 1);

			result[n] = t[n];
			for (int i = n - 1; i >= 0; i--)
				result[i] = s[i] * result[i + 1] + t[i];

			if (write)
			{
				Line();
				Left("Численное решение");
				Right(result);

				OutputResidual(M * result - G);
				EndLine(oneColumnLength);
			}

			return result;
		}

		static double P(double x)
		{
			return -1;
		}
		static double Q(double x)
		{
			return 0;
		}
		static double R(double x)
		{
			return -8 / (1 + 2 * x) / (1 + 2 * x);
		}
		static double F(double x)
		{
			return 36 / 2 / Pow(2 * x + 1, 1.5);
		}
		static double Precise(double x)
		{
			return -2 * Sqrt(2 * x + 1);
		}
	}
}
