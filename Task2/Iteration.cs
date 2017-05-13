using System.Linq;
using static NumericalAnalysis2.Printer;
using static NumericalAnalysis2.SquareMatrix;
using static System.Math;

namespace NumericalAnalysis2
{
	static class Worker
	{
		public static bool DiagonallyDominant(this SquareMatrix q)
		{
			return q.X.Select((x, i) => 2 * x[i] - x.N8).All(x => x > 0);
		}
		public static SquareMatrix AboveDiagonal(this SquareMatrix q)
		{
			var result = q.DeepClone;
			int n = q.Rows;

			for (int i = 0; i < n; i++)
				for (int j = 0; j < i; j++)
					result[i, j] = 0;

			return result;
		}
		public static SquareMatrix UnderDiagonal(this SquareMatrix q)
		{
			var result = q.DeepClone;
			int n = q.Rows;

			for (int i = 0; i < n; i++)
				for (int j = i; j < n; j++)
					result[i, j] = 0;

			return result;
		}
	}

	class Interation
	{
		public static Vector x;

		const int module = 1;
		const int tooBigStep = 1000;
		const int length = 56;

		SquareMatrix H;
		Vector g;
		int steps, n;
		double e, posterioriEstimate, prioriEstimate;
		double HRN8, HN8, gN8, x0N8;

		Interation(SquareMatrix q, Vector u)
		{
			n = q.Rows;

			SquareMatrix D = q.DiagonalInvert;
			H = Id(n) - D * q;
			g = D * u;

			x = new Vector(n);
		}

		Interation(SquareMatrix q, Vector u, double e)
			: this(q, u)
		{
			this.e = e;
		}
		Interation(SquareMatrix q, Vector u, int steps)
			: this(q, u)
		{
			this.steps = steps;
		}

		public static bool FixedPoint(SquareMatrix q, Vector u, double e)
		{
			if (!CheckConvergenceCondition(q))
				return false;

			new Interation(q, u, e).FixedPointByE();
			return true;
		}
		public static bool FixedPoint(SquareMatrix q, Vector u, int steps)
		{
			if (!CheckConvergenceCondition(q))
				return false;

			new Interation(q, u, steps).FixedPointBySteps();
			return true;
		}
		public static bool Seidel(SquareMatrix q, Vector u, int steps)
		{
			if (!CheckConvergenceCondition(q))
				return false;

			new Interation(q, u, steps).SeidelBySteps();
			return true;
		}
		public static bool Seidel(SquareMatrix q, Vector u, double e)
		{
			if (!CheckConvergenceCondition(q))
				return false;

			new Interation(q, u, e).SeidelByE();
			return true;
		}
		static bool CheckConvergenceCondition(SquareMatrix q)
		{
			if (q.DiagonallyDominant())
				return true;

			Line("Матрица не диагонально доминирующая");
			return false;
		}

		bool FixedPointBySteps()
		{
			Start("Проделаем метод простой итерации с заданным кол-ом шагов");
			Right(steps);
			Line();

			FixedPointInit();

			FindPrioriEstimateBySteps();

			for (int i = 0; i < steps; i++)
			{
				var xPrev = x;
				x = H * x + g;

				posterioriEstimate = HN8 / (1 - HN8) * (x - xPrev).N8;

				OutputPosterioriEstimate(i);
			}

			return HappyEnd();
		}
		bool FixedPointByE()
		{
			Start("Проделаем метод простой итерации с заданной погрешностью");
			Right(e);
			Line();

			FixedPointInit();

			if (!FindPrioriEstimationByE())
				return ErrorEnd();

			for (steps = 1; steps < tooBigStep; steps++)
			{
				var xPrev = x;
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
			Start("Проделаем метод Зейделя с заданным кол-ом шагов");
			Right(steps);
			Line();

			SeidelInit();
			FindPrioriEstimateBySteps();

			for (int i = 0; i < steps; i++)
			{
				var xPrev = x;
				x = H * x + g;

				posterioriEstimate = HRN8 / (1 - HN8) * (x - xPrev).N8;

				OutputPosterioriEstimate(i);
			}

			return HappyEnd();
		}
		bool SeidelByE()
		{
			Start("Проделаем метод Зейделя с заданной погрешностью");
			Right(e);
			Line();

			SeidelInit();

			if (!FindPrioriEstimationByE())
				return ErrorEnd();

			for (steps = 1; steps < tooBigStep; steps++)
			{
				var xPrev = x;
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
			var HR = H.AboveDiagonal();

			var h = (Id(n) - HL).Invert;

			H = h * HR;
			g = h * g;

			HN8 = H.N8;
			gN8 = g.N8;
			x0N8 = x.N8;

			HRN8 = HR.N8;
		}

		void FindPrioriEstimateBySteps()
		{
			prioriEstimate = Pow(HN8, steps) * x0N8 +
				Pow(HN8, steps) / (1 - HN8) * gN8;

			Left("Априорная оценка погрешности решения на {0}-м шаге", steps);
			Right(prioriEstimate);
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
				Right(" {0:E2} < {0:E2}", prioriEstimate,  e);

				return true;
			}

			Line("Априорная оценка погрешности решения вплоть до ");
			Left("{0} шага не удовлетворяет желаемой точности", tooBigStep);

			return false;
		}
		void OutputPosterioriEstimate(int i)
		{
			if (i % module == 0)
			{
				AlwaysLeft("Апостериорная оценка погрешности решения" +
					" на {0}-м шаге", i);

				AlwaysRight(posterioriEstimate);
			}
		}

		void Start(string s)
		{
			StartLine(length);
			Left(s);
		}
		bool HappyEnd()
		{
			Line();

			Left("На шаге {0} получили решение", steps);
			Right(x);

			Left("c апостериорной оценкой погрешности в");
			Right(posterioriEstimate);

			EndLine(length);

			return true;
		}
		bool ErrorEnd()
		{
			Line();

			Line("Вплоть до {0} шага не получили желаемую", tooBigStep);
			Line("апостериорную оценку погрешности");

			EndLine(length);

			return false;
		}
	}
}