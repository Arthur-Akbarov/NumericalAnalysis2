using static NumericalAnalysis2.Printer;
using static NumericalAnalysis2.SquareMatrix;
using static System.Console;
using static System.Math;

namespace NumericalAnalysis2
{
	class Iteration
	{
		public static Vector xGauss;

		const int radiusSteps = 10;
		const int outputModule = 10;
		const int tooBigStep = 1000;
		const int divOneColumnLineLength = 56;

		SquareMatrix H;
		Vector g, x, xPrev;
		int steps, n;
		double e, posterioriEstimate, prioriEstimate;
		double HRN8, HN8, gN8, x0N8;
		bool output;

		Iteration() { }
		Iteration(SquareMatrix q, Vector u)
		{
			n = q.Rows;

			H = q;
			g = u;

			x = new Vector(n);
		}

		Iteration(SquareMatrix q, Vector u, double e)
			: this(q, u)
		{
			this.e = e;
		}
		Iteration(SquareMatrix q, Vector u, int steps)
			: this(q, u)
		{
			this.steps = steps;
		}

		public static bool FixedPoint(SquareMatrix q, Vector u, double e)
		{
			return new Iteration(q, u, e).FixedPointByE();
		}
		public static bool FixedPoint(SquareMatrix q, Vector u, int steps)
		{
			return new Iteration(q, u, steps).FixedPointBySteps();
		}
		public static bool Seidel(SquareMatrix q, Vector u, int steps)
		{
			return new Iteration(q, u, steps).SeidelBySteps();
		}
		public static bool Seidel(SquareMatrix q, Vector u, double e)
		{
			return new Iteration(q, u, e).SeidelByE();
		}
		public static void SuccessiveOverRelaxation(SquareMatrix q, Vector u,
			int steps)
		{
			new Iteration(q, u, steps).SuccessiveOverRelaxationBySteps();
		}

		bool FixedPointBySteps()
		{
			Start("Проделаем ", "метод простой итерации",
				" с заданным кол-ом шагов");
			Right(steps);
			Line();

			FixedPointInit();

			FindPrioriEstimateBySteps();

			for (int i = 0; i < steps; i++)
			{
				xPrev = x;
				x = H * x + g;

				posterioriEstimate = HN8 / (1 - HN8) * (x - xPrev).N8;

				OutputPosterioriEstimate(i + 1);
			}

			return HappyEnd();
		}
		bool FixedPointByE()
		{
			Start("Проделаем ", "метод простой итерации",
				" с заданной погрешностью");
			RightE(e, n);
			Line();

			FixedPointInit();

			if (!FindPrioriEstimationByE())
				return ErrorEnd();

			for (steps = 1; steps < tooBigStep; steps++)
			{
				xPrev = x;
				x = H * x + g;

				posterioriEstimate = HN8 / (1 - HN8) * (x - xPrev).N8;

				if (posterioriEstimate <= e)
					return HappyEnd();
			}

			return ErrorEnd();
		}
		void FixedPointInit()
		{
			HN8 = H.N8;
			gN8 = g.N8;
			x0N8 = x.N8;
		}

		bool SeidelBySteps()
		{
			Start("Проделаем ", "метод Зейделя", " с заданным кол-ом шагов");
			Right(steps);
			Line();

			SeidelInit();
			FindPrioriEstimateBySteps();

			for (int i = 0; i < steps; i++)
			{
				xPrev = x;
				x = H * x + g;

				posterioriEstimate = HRN8 / (1 - HN8) * (x - xPrev).N8;

				OutputPosterioriEstimate(i + 1);
			}

			return HappyEnd();
		}
		bool SeidelByE()
		{
			Start("Проделаем ", "метод Зейделя", " с заданной погрешностью");
			RightE(e, n);
			Line();

			SeidelInit();

			if (!FindPrioriEstimationByE())
				return ErrorEnd();

			for (steps = 1; steps < tooBigStep; steps++)
			{
				xPrev = x;
				x = H * x + g;

				posterioriEstimate = HRN8 / (1 - HN8) * (x - xPrev).N8;

				if (posterioriEstimate <= e)
					return HappyEnd();
			}

			return ErrorEnd();
		}
		void SeidelInit()
		{
			var HL = H.UnderDiagonal();
			var HR = H.AboveDiagonal() + H.Diagonal();

			var h = (Id(n) - HL).Invert;

			H = h * HR;
			g = h * g;

			HN8 = H.N8;
			gN8 = g.N8;
			x0N8 = x.N8;

			HRN8 = HR.N8;
		}

		void SuccessiveOverRelaxationBySteps()
		{
			Start("Проделаем ", "метод верхней релаксации", " с кол-ом шагов");
			Right(steps);
			Line();

			double q = FindQ();

			for (int k = 0; k < steps; k++)
				for (int i = 0; i < n; i++)
					x[i] += (H[i] * x - (H[i, i] + 1) * x[i] + g[i]) * q;

			End();
		}
		void SuccessiveOverRelaxationByStepsUsingA()
		{
			Start("Проделаем ", "метод верхней релаксации", " с кол-ом шагов");
			Right(steps);
			Line();

			double q = FindQ();

			for (int k = 0; k < steps; k++)
				for (int i = 0; i < n; i++)
					x[i] += q / H[i, i] * (g[i] - H[i] * x);

			End();
		}

		void FindPrioriEstimateBySteps()
		{
			prioriEstimate = Pow(HN8, steps) * x0N8 +
				Pow(HN8, steps) / (1 - HN8) * gN8;

			Left("Априорная оценка погрешности решения на {0}-м шаге", steps);
			RightE(prioriEstimate, n);
		}
		bool FindPrioriEstimationByE()
		{
			for (int steps = 1; steps < tooBigStep; steps++)
			{
				prioriEstimate = Pow(HN8, steps) * x0N8
					+ Pow(HN8, steps) / (1 - HN8) * gN8;

				if (prioriEstimate > e)
					continue;

				Line("Априорная оценка погрешности решения впервые");
				Left("удовлетворяет желаемой точности на шаге {0}", steps);
				RightE(prioriEstimate, n);

				return true;
			}

			Line("Априорная оценка погрешности решения вплоть до ");
			Left("{0} шага не удовлетворяет желаемой точности", tooBigStep);

			return false;
		}
		void OutputPosterioriEstimate(int i)
		{
			if (i % outputModule == 0)
			{
				if (!output && (output = true))
					Line();

				AlwaysLeft("Апостериорная оценка погрешности решения" +
					" на {0}-м шаге", i);

				AlwaysRightE(posterioriEstimate, n);
			}
		}
		double GetSpectralRadius()
		{
			var xPrev = new Vector(n);

			for (int i = 0; i < radiusSteps - 1; i++)
				xPrev = H * xPrev + g;

			var x = H * xPrev + g;
			var xNext = H * x + g;

			double result = (xNext - x).N8 / (x - xPrev).N8;

			// нашли приближённый модуль с.ч., далее уточним его знак
			var D = result * Id(n);

			return (Abs((H - D).Det) < Abs((H + D).Det)) ? result : -result;
		}
		Vector ClarificationOfLyusternik()
		{
			double pH = GetSpectralRadius();

			return xPrev + 1 / (1 - pH) * (x - xPrev);
		}
		double FindQ()
		{
			double spectralRadius = GetSpectralRadius();

			double result = 2 / (1 + Sqrt(1 - Pow(spectralRadius, 2)));

			Left("Подобрали q методом простой итерации за {0} шага", steps);
			Right(result);

			return result;
		}

		void Start(string s1, string s2, string s3)
		{
			Line();
			StartLine(divOneColumnLineLength);

			Write(s1);
			ColorWrite(s2);
			Left(s3);
		}
		void End()
		{
			Line();

			Left("На шаге {0} получили решение", steps);
			Right(x);
			Line();

			Left("Фактическая погрешность");
			RightE((xGauss - x).N8, n);

			EndLine(divOneColumnLineLength);
		}
		bool HappyEnd()
		{
			Line();

			Left("На шаге {0} получили решение", steps);
			Right(x);
			Line();

			Left("Апостериорная оценка погрешности");
			RightE(posterioriEstimate, n);

			Left("Фактическая погрешность");
			RightE((xGauss - x).N8, n);

			Line();
			var xLyusternik = ClarificationOfLyusternik();
			Left("Уточнение по Люстернику");
			Right(xLyusternik);

			Left("Фактическая погрешность");
			RightE((xGauss - xLyusternik).N8, n);

			EndLine(divOneColumnLineLength);

			return true;
		}
		bool ErrorEnd()
		{
			Line();

			Line("Вплоть до {0} шага не получили желаемую", tooBigStep);
			Line("апостериорную оценку погрешности");

			EndLine(divOneColumnLineLength);

			return false;
		}
	}
}