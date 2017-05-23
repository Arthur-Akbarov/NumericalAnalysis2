using System.Text;
using static NumericalAnalysis2.Printer;
using static System.Console;

namespace NumericalAnalysis2
{
	class Task5
	{
		static void Greeting()
		{
			WriteLine("u(x) = Integrate_0^1 (K(x, y) * u(y) * dy) = f(x), где");
			WriteLine("Ядро K(x, y) =  tg(xy) / 3");
			WriteLine("Правая часть f(x) = (1 + x) / 2;");
			WriteLine();
		}
		static void Main(string[] args)
		{
			f = -10;
			Title = "Task5 Решение интегрального уравнения Фредгольма II рода";
			OutputEncoding = Encoding.GetEncoding(1251);
			Greeting();

			var q1 = Fredholm.MechanicalQuadrature(4, 10);
			var q2 = Fredholm.MechanicalQuadrature(6, 10);

			Line();
			Right("Максимальная разница решений");
			Right((q1 - q2).N1);

			var u1 = Fredholm.DegenerateKernel(3, 10);
			var u2 = Fredholm.DegenerateKernel(4, 10);

			Line();
			Right("Максимальная разница решений");
			Right((u1 - u2).N1);

			ReadKey();
		}
	}
}
