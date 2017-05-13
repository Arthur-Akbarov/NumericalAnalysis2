using static System.Math;

namespace NumericalAnalysis2
{
	public class GaussMod : Gauss
	{
		bool needSort;
		int[] index;
		protected override void Init(SquareMatrix q, IMatrix t, bool write)
		{
			base.Init(q, t, write);
			index = new int[n];
		}

		public static SquareMatrix Invert(SquareMatrix q, bool write = false)
		{
			return new GaussMod() { invert = true }.
				Start(q, SquareMatrix.Id(q.Rows), write);
		}
		public static bool Invertible(SquareMatrix q)
		{
			var a = new GaussMod();

			a.Init(q, null, false);

			return a.IsSuccessfully();
		}

		public static T Do<T>(SquareMatrix q, T t, bool write = false)
			where T : class, IMatrix
		{
			return new GaussMod().Start(q, t, write);
		}

		protected override bool GaussianElimination()
		{
			print?.Start("I Проделаем прямой ход " +
				"модифицированного метода Гаусса");

			for (int k = 0; k < n; k++)
			{
				if (!FindMaxElementInRow(k))
					return false;

				print?.Normalize("Нормализуем {0}-ую строку", k, index[k]);

				double d = a[k, index[k]];

				a.DivideRow(k, d);
				b.DivideRow(k, d);
				// для точности
				a[k, index[k]] = 1;

				if (k != n - 1)
					print?.Subtract("Вычтем из более нижних " +
						"{0}-ую строку", k);

				for (int i = k + 1; i < n; i++)
				{
					d = a[i, index[k]];

					a.SubtractRow(i, d, k);
					b.SubtractRow(i, d, k);
				}
			}

			return true;
		}
		bool FindMaxElementInRow(int k)
		{
			double max = Abs(a[k, 0]);

			for (int i = 1; i < n; i++)
			{
				double d = Abs(a[k, i]);

				if (max < d)
				{
					max = d;
					index[k] = i;
				}
			}

			if (index[k] != k)
				needSort = true;

			if (max != 0)
				return true;

			print?.End("Матрица необратима.");

			return false;
		}
		protected override void BackSubstitution()
		{
			print?.NewPart("II Проделаем обратный ход " +
				"модифицированного метода Гаусса");

			// идём снизу вверх по опорным строкам
			for (int k = n - 1; k > 0; k--)
			{
				print?.Subtract("Вычтем из более верхних {0}-ую строку", k);

				// идём сверху вниз до опорной строки
				for (int i = 0; i < k; i++)
				{
					double d = a[i, index[k]];

					a.SubtractRow(i, d, k);
					b.SubtractRow(i, d, k);
				}
			}

			if (needSort)
			{
				print?.Put("Отсортируем строки по единицам");

				// убираем лишних с первого места на своё и т.д.
				for (int i = 0; i < n; i++)
					while (index[i] != i)
					{
						a.SwapRows(i, index[i]);
						b.SwapRows(i, index[i]);
						index.SwapInArray(i, index[i]);
					}
			}

			string s = "Слева получили единичную матрицу, а справа ";
			s += (invert) ? "обратую" : "A^(-1)*B";

			print?.End(s);
		}
	}
}
