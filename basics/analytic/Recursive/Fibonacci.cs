using System;
using System.Collections.Generic;
using System.Text;

namespace Analytic.Recursive
{
	public class Fibonacci
	{
		public Fibonacci()
		{
			var fibonacciNumber = Basic(9);
			Console.WriteLine(fibonacciNumber);
		}

		/// <summary>
		///                           fib(5)   
		///						 /                \
		///					fib(4)                fib(3)
		///				 /        \              /      \ 
		///			fib(3)      fib(2)         fib(2)   fib(1)
		///			/    \       /    \		   /     \
		///		fib(2)  fib(1) fib(1) fib(0) fib(1) fib(0)
		///		 /     \
		///	 fib(1) fib(0)
		/// </summary>
		/// <TimeComplexity> O(2^n) or exponential </TimeComplexity>
		/// <SpaceComplexity> O(n) if we consider the function call stack size, otherwise O(1) </SpaceComplexity>
		/// <returns></returns>
		public static int Basic(int n)
		{
			if (n <= 1)
				return n;

			return Basic(n - 1) + Basic(n - 2);
		}
	}
}
