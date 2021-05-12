using System;

namespace ScreenCapture
{
	class Program
	{
		static void Main(string[] args)
		{
			LibScreenCapture.TestCapture test = new LibScreenCapture.TestCapture();
			test.Capture();

			Console.WriteLine("Hello World!");
		}
	}
}
