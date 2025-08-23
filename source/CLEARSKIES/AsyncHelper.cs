using System;
using System.Threading;

namespace CLEARSKIES
{
	public static class AsyncHelper
	{
		public static void BeginInBackground(Action method)
		{
			Execute(delegate
			{
				method();
			});
		}

		public static void BeginInBackground<T>(Action<T> method, T val)
		{
			Execute(delegate
			{
				method(val);
			});
		}

		public static void BeginInBackground<T, T2>(Action<T, T2> method, T val, T2 val2)
		{
			Execute(delegate
			{
				method(val, val2);
			});
		}

		private static void Execute(Action method)
		{
			Thread thread = new Thread(method.Invoke);
			thread.IsBackground = true;
			thread.Start();
		}
	}
}
