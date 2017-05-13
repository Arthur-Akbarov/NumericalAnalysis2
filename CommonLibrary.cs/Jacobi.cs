using static System.Math;

namespace NumericalAnalysis2
{
	class Jacobi
	{
		void Start(Matrix a, double[] l, Vector[] vv)
		{
			int n = a.Rows;

			if (n == 0)
				return;

			double[] b = new double[n];
			double[] z = new double[n];

			for (int i = 0; i < n; ++i)
			{
				z[i] = 0;
				b[i] = l[i] = a[i][i];

				for (int j = 0; j < n; j++)
					vv[i][j] = i == j ? 1 : 0;
			}
			for (int i = 0; i < 50; ++i)
			{
				double sm = 0;

				for (int p = 0; p < n - 1; p++)
					for (int q = p + 1; q < n; q++)
						sm += Abs(a[p][q]);

				if (sm == 0)
					break;

				double tresh = i < 3
					? 02 * sm / (n * n)
					: 0;

				for (int p = 0; p < n - 1; p++)
					for (int q = p + 1; q < n; q++)
					{
						double g = 1e12 * Abs(a[p][q]);

						if (i >= 3 && Abs(l[p]) > g && Abs(l[q]) > g)
							a[p, q] = 0;
						else
						if (Abs(a[p][q]) > tresh)
						{
							double theta = 05 * (l[q] - l[p]) / a[p][q];
							double t = 1 / (Abs(theta) + Sqrt(1 + theta * theta));
							if (theta < 0) t = -t;
							double c = 1 / Sqrt(1 + t * t);
							double s = t * c;
							double tau = s / (1 + c);
							double h = t * a[p][q];
							z[p] -= h;
							z[q] += h;
							l[p] -= h;
							l[q] += h;
							a[p, q] = 0;

							for (int j = 0; j < p; j++)
							{
								g = a[j][p];
								h = a[j][q];

								a[j, p] = g - s * (h + g * tau);
								a[j, q] = h + s * (g - h * tau);
							}
							for (int j = p + 1; j < q; j++)
							{
								g = a[p][j];
								h = a[j][q];

								a[p, j] = g - s * (h + g * tau);
								a[j, q] = h + s * (g - h * tau);
							}
							for (int j = q + 1; j < n; j++)
							{
								g = a[p][j];
								h = a[q][j];

								a[p, j] = g - s * (h + g * tau);
								a[q, j] = h + s * (g - h * tau);
							}
							for (int j = 0; j < n; j++)
							{
								g = vv[j][p];
								h = vv[j][q];

								vv[j][p] = g - s * (h + g * tau);
								vv[j][q] = h + s * (g - h * tau);
							}
						}
					}

				for (int p = 0; p < n; p++)
				{
					l[p] = (b[p] += z[p]);
					z[p] = 0;
				}
			}
		}
	}
}
