using System;
using System.Text;
using static System.Console;
using static System.Environment;
using static System.Math;
using static System.String;
using static NumericalAnalysis2.Printer;
using static NumericalAnalysis2.Worker;

namespace NumericalAnalysis2
{
	public class GaussOrd : Gauss
	{
		public static T Do<T>(SquareMatrix q, T t, bool write = false)
			where T : class, IMatrix
		{
			return new GaussOrd().Start(q, t, write);
		}
		public static SquareMatrix Invert(SquareMatrix q, bool write = false)
		{
			return new GaussOrd() { invert = true }.
				Start(q, SquareMatrix.Id(q.Rows), write);
		}
		public static bool Invertible(SquareMatrix q)
		{
			var a = new GaussOrd();

			a.Init(q, null, false);

			return a.IsSuccessfully();
		}

		protected override void Init(SquareMatrix q, IMatrix t, bool write)
		{
			if (t.Rows != q.Rows)
				throw new RankException("Row counts do not match.");

			n = q.Rows;
			a = q.DeepClone;
			b = t.DeepAClone();

			int length = Max(51, (n + t.Cols) * (Abs(f) + 2) + 1 + indentL);
			print = (write) ? new GaussPrint(a, b, n, length) : null;
		}

		protected override bool GaussianElimination()
		{
			print?.Start("I Проделаем прямой ход обычного метода Гаусса");

			for (int k = 0; k < n; k++)
			{
				if (!SwapToNonZeroRow(k))
					return false;

				print?.Normalize("Нормализуем {0}-ую строку по {0} элементу",
					k, k);

				double d = a[k, k];

				a.DivideRow(k, d);
				b.DivideRow(k, d);
				// для точности
				a[k, k] = 1;

				if (k != n - 1)
					print?.Subtract("Вычтем из более нижних {0}-ую строку", k);

				for (int i = k + 1; i < n; i++)
				{
					d = a[i, k];

					a.SubtractRow(i, d, k);
					b.SubtractRow(i, d, k);
				}
			}

			return true;
		}
		bool SwapToNonZeroRow(int k)
		{
			if (a[k, k] != 0)
				return true;

			for (int i = k + 1; i < n; i++)
				if (a[i, k] != 0)
				{
					print?.SwapRows("Поменяем {0}-ую и {1}-ую строки", k, i);

					a.SwapRows(k, i);
					b.SwapRows(k, i);

					return true;
				}

			print?.End("Матрица необратима.");

			return false;
		}
		protected override void BackSubstitution()
		{
			print?.NewPart("II Проделаем обратный ход " +
				"обычного метода Гаусса");

			// идём снизу вверх по опорным строкам
			for (int k = n - 1; k > 0; k--)
			{
				print?.Subtract("Вычтем из более верхних {0}-ую строку", k);

				// идём сверху вниз до опорной строки
				for (int i = 0; i < k; i++)
				{
					double d = a[i, k];

					a.SubtractRow(i, d, k);
					b.SubtractRow(i, d, k);
				}
			}

			string s = "Слева получили единичную матрицу, а справа ";
			s += (invert) ? "обратую" : "A^(-1)*B";

			print?.End(s);
		}
	}
}


