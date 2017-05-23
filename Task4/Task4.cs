using System.Text;
using static NumericalAnalysis2.Printer;
using static System.Console;

namespace NumericalAnalysis2
{
	class Task4
	{
		static void Init()
		{
			//twoColumn = true;
			f = -5;

			columnSize = 55;
			indentL = alwaysIndentL = 2;
		}
		static void Main(string[] args)
		{
			Init();
			Title = "Task4 Метод прогонки";
			OutputEncoding = Encoding.GetEncoding(1251);

			Tridiagonal.Start(4, true);

			for (int i = 2; i < 20; i++)
				Tridiagonal.Start(i, false);

			ReadKey();
		}
	}
}
