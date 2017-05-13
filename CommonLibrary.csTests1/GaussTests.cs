using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NumericalAnalysis2.Tests
{
	[TestClass()]
	public class GaussTests
	{
		SquareMatrix a;
		SquareMatrix r;
		Vector b;
		Vector x;

		void Init1()
		{
			a = new SquareMatrix(2, 5, 7, 6, 3, 4, 5, -2, -3);
			r = new SquareMatrix(1, -1, 1, -38, 41, -34, 27, -29, 24);
		}
		void Init2()
		{
			a = new SquareMatrix(1, 2, 3, 4);
			b = new Vector(2, 4);
			x = new Vector(0, 1);

			Assert.IsTrue(GaussMod.Do(a, b) == x);
		}
		void Init3()
		{
			a = new SquareMatrix(1, 2, 3, 4, 5, 6, 9, 7, 7);
			b = new Vector(1, 2, 3);
			x = new Vector(1 / 3d, -2 / 3d, 2 / 3d);
		}

		[TestMethod()]
		public void ModTestMatrix1()
		{
			Init1();
			Assert.IsTrue(r == GaussMod.Invert(a));
		}
		[TestMethod()]
		public void ModTestVector1()
		{
			Init2();
			Assert.IsTrue(GaussMod.Do(a, b) == x);
		}
		[TestMethod()]
		public void ModTestVector2()
		{
			Init3();
			Assert.IsTrue(GaussMod.Do(a, b) == x);
		}

		[TestMethod()]
		public void OrdTestMatrix1()
		{
			Init1();
			Assert.IsTrue(r == GaussOrd.Invert(a));
		}
		[TestMethod()]
		public void OrdTestVector1()
		{
			Init2();
			Assert.IsTrue(GaussOrd.Do(a, b) == x);
		}
		[TestMethod()]
		public void OrdTestVector2()
		{
			Init3();
			Assert.IsTrue(GaussOrd.Do(a, b) == x);
		}

		[TestMethod()]
		public void Invert1()
		{
			Init1();
			Assert.IsTrue(r == a.Invert);
		}
		[TestMethod()]
		public void Invert2()
		{
			Init2();
			Assert.IsTrue(x == a.Invert*b);
		}
		[TestMethod()]
		public void Invert3()
		{
			Init3();
			Assert.IsTrue(x == a.Invert * b);
		}
	}
}
