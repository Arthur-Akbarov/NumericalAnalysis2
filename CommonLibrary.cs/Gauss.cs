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
	public abstract class Gauss
	{
		protected GaussPrint print;
		protected SquareMatrix a;
		protected IMatrix b;
		protected int n;
		protected bool invert;

		protected T Start<T>(SquareMatrix q, T t, bool write)
			where T : class, IMatrix
		{
			Init(q, t, write);

			IsSuccessfully();

			return b as T;
		}

		protected virtual void Init(SquareMatrix q, IMatrix t, bool write)
		{
			if (t.Rows != q.Rows)
				throw new RankException("Row counts do not match.");

			n = q.Rows;
			a = q.DeepClone;
			b = t.DeepAClone();

			int length = Max(57, (n + t.Cols) * (Abs(f) + 2) + 1 + indentL);
			print = (write) ? new GaussPrint(a, b, n, length) : null;
		}
		protected bool IsSuccessfully()
		{
			if (!GaussianElimination())
				return false;

			BackSubstitution();

			return true;
		}

		protected abstract bool GaussianElimination();
		protected abstract void BackSubstitution();
	}
}
