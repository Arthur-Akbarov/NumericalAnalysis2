//#define var
using NumericalAnalysis;
using static NumericalAnalysis.Worker;
using static NumericalAnalysis2.Printer;
using static NumericalAnalysis2.Table;
using static System.Console;
using static System.Math;

namespace NumericalAnalysis2
{
	class Fredholm
	{
		const double a = 0;
		const double b = 1;

		static int n, m;
		static double h;
		static double[] A, node, x;
		static Vector u, un;

		const int f0 = -5;
		static int ff = -Max(8, Abs(f));
		static bool mech, degenerate;

		static void Init(int n, int m)
		{
			Fredholm.n = n;
			Fredholm.m = m;

			h = (b - a) / m;

			u = new Vector(m + 1);
			x = new double[m + 1];

			for (int i = 0; i < x.Length; i++)
				x[i] = a + i * h;
		}

		public static Vector MechanicalQuadrature(int n, int m)
		{
			Init(n, m);
			MechanicalInit();

			PutLine($"КФ Гаусса с {n} узлами");
			PutRow("точка", "решение");

			for (int i = 0; i <= m; i++)
			{
				u[i] = F(x[i]);

				for (int j = 0; j < n; j++)
					u[i] += A[j] * K(x[i], node[j]) * un[j];

				PutRow(x[i], u[i]);
			}

			//MechanicalResidual();
			return u;
		}
		static void MechanicalGreeting()
		{
			WriteLine("I Метод механических квадратур");
			WriteLine();
		}
		static void MechanicalResidual()
		{
			PutLine();
			PutRow("точка", "невязка");

			for (int i = 0; i <= m; i++)
			{
				double x = a + i * h;
				double y = u[i] - F(x);

				for (int j = 1; j <= m; j++)
					y -= h * K(x, a + (j - 0.5) * h) / 2 * (u[j - 1] + u[j]);

				PutRow(x, y);
			}
		}
		static void MechanicalInit()
		{
			if (!mech)
			{
				mech = true;
				MechanicalGreeting();
				Table.Init();
				SetCell(f0, ff);
				SetLength(f0, Printer.f);
			}
			else
				Reset();

			FindGaussQuadrature();

			var f = new Vector(n);
			for (int i = 0; i < n; i++)
				f[i] = F(node[i]);

			var G = new SquareMatrix(n);
			for (int i = 0; i < n; i++)
				for (int j = 0; j < n; j++)
					G[i, j] = -A[j] * K(node[i], node[j]);

			for (int i = 0; i < n; i++)
				G[i, i] += 1;

			un = GaussMod.Do(G, f, false);
		}
		static void FindGaussQuadrature()
		{
			var p = new Polynomial(-1, 0, 1) ^ n;
			p = p.GetDer(n);
			p /= Factorial(n);
			p /= Pow(2, n);

			var t = p.GetRoots(-1, 1);
			p = p.GetDer();

			A = new double[n];
			for (int i = 0; i < n; i++)
				A[i] = (b - a) / (1 - t[i] * t[i]) / Pow(p.Eval(t[i]), 2);

			node = new double[n];
			for (int i = 0; i < n; i++)
				node[i] = (b - a) / 2 * t[i] + (b + a) / 2;
		}

		public static Vector DegenerateKernel(int n, int m)
		{
			Init(n, m);
			DegenerateInit();

			PutLine($"С ядром ранга {n}");
			PutRow("точка", "решение");

			for (int i = 0; i <= m; i++)
			{
				u[i] = F(x[i]);

				for (int j = 0; j < n; j++)
					u[i] += A[j] * Alpha(j, x[i]);

				PutRow(x[i], u[i]);
			}

			return u;
		}
		static void DegenerateGreeting()
		{
			WriteLine();
			WriteLine();
			WriteLine("II Метод замены ядра на вырожденное");
			WriteLine();
		}
		static void DegenerateInit()
		{
			if (!degenerate)
			{
				degenerate = true;
				DegenerateGreeting();
				Table.Init();
				SetCell(f0, ff);
				SetLength(f0, Printer.f);
			}
			else
				Reset();

			var H = SquareMatrix.Id(n);
			var B = new Vector(n);
			double d = h / m;

			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
					for (int l = 0; l < m * m; l++)
						H[i, j] -= d * Beta(i, a + (0.5 + l) * d)
							* Alpha(j, a + (0.5 + l) * d);

				for (int l = 0; l < m * m; l++)
					B[i] += d * Beta(i, a + (0.5 + l) * d)
						* F(a + (0.5 + l) * d);
			}

			A = GaussMod.Do(H, B, false).X;
		}

		static double K(double x, double y)
		{
#if var
			return Cos(x * y) / 2;
#else
			return Tan(x * y) / 3;
#endif
		}
		static double F(double x)
		{
#if var
			return 1 + x - x * x;
#else
			return (1 + x) / 2;
#endif
		}
		static double Alpha(int i, double x)
		{
#if var
			return Pow(-1, i) * Pow(x, 2 * i);
#else
			return Pow(x, 2 * i + 1);
#endif

		}
		static double Beta(int i, double y)
		{
#if var
			return Pow(x, 2 * i) / 2 / Factorial(2 * i);
#else
			double result = Pow(y, 2 * i + 1);

			switch (i)
			{
				case 0:
					return result / 3;
				case 1:
					return result / 9;
				case 2:
					return result * 2 / 45;
				default:
					return result * 17 / 945;
			}
#endif
		}
		//static double P6der(double x)
		//{
		//	return 1 / 16 * (105 * 2 * x - 315 * 4 * x * x * x +
		//		231 * 6 * x * x * x * x * x);
		//}
	}
}
