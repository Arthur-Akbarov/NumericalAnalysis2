namespace NumericalAnalysis2
{
	public interface IMatrix
	{
		int Rows { get; }
		int Cols { get; }

		void SwapRows(int i, int j);
		void SubtractRow(int i, double d, int k);
		void DivideRow(int i, double d);

		IMatrix DeepAClone { get; }

		string ToString(int i, int f);
		string ToString(int f);
	}
}
