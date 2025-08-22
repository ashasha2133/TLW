using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace CLEARSKIES
{
	internal class Yield : MonoBehaviour
	{
		internal delegate void Create();

		internal delegate T Create<out T>();

		internal delegate void CreateParams(params object[] parameters);

		internal delegate T CreateParams<out T>(params object[] parameters);

		internal static Yield yield = new GameObject("yield").AddComponent<Yield>();

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		internal static void Begin(Action method)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(method);
		}

		internal static void Begin<T>(Action<T> method, T var)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(method, var);
		}

		internal static void Begin<T, T2>(Action<T, T2> method, T var, T2 var2)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(method, var, var2);
		}

		internal static void Begin<T, T2, T3>(Action<T, T2, T3> method, T var, T2 var2, T3 var3)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(method, var, var2, var3);
		}

		internal static void Begin<T, T2, T3, T4>(Action<T, T2, T3, T4> method, T var, T2 var2, T3 var3, T4 var4)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(method, var, var2, var3, var4);
		}

		internal static void Begin(YieldInstruction yieldInstruction, Action method)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(yieldInstruction, method);
		}

		internal static void Begin<T>(YieldInstruction yieldInstruction, Action<T> method, T var)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(yieldInstruction, method, var);
		}

		internal static void Begin<T, T2>(YieldInstruction yieldInstruction, Action<T, T2> method, T var, T2 var2)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(yieldInstruction, method, var, var2);
		}

		internal static void Begin<T, T2, T3>(YieldInstruction yieldInstruction, Action<T, T2, T3> method, T var, T2 var2, T3 var3)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(yieldInstruction, method, var, var2, var3);
		}

		internal static void Begin<T, T2, T3, T4>(YieldInstruction yieldInstruction, Action<T, T2, T3, T4> method, T var, T2 var2, T3 var3, T4 var4)
		{
			if (yield == null)
			{
				yield = new GameObject("yield").AddComponent<Yield>();
			}
			yield.Invoke(yieldInstruction, method, var, var2, var3, var4);
		}

		internal void Invoke(Action method)
		{
			StartCoroutine(Coroutine(method));
		}

		private static IEnumerator Coroutine(Action method)
		{
			yield return new WaitForEndOfFrame();
			method();
		}

		internal void Invoke<T>(Action<T> method, T var)
		{
			StartCoroutine(Coroutine(method, var));
		}

		private static IEnumerator Coroutine<T>(Action<T> method, T var)
		{
			yield return new WaitForEndOfFrame();
			method(var);
		}

		internal void Invoke<T, T2>(Action<T, T2> method, T var, T2 var2)
		{
			StartCoroutine(Coroutine(method, var, var2));
		}

		private static IEnumerator Coroutine<T, T2>(Action<T, T2> method, T var, T2 var2)
		{
			yield return new WaitForEndOfFrame();
			method(var, var2);
		}

		internal void Invoke<T, T2, T3>(Action<T, T2, T3> method, T var, T2 var2, T3 var3)
		{
			StartCoroutine(Coroutine(method, var, var2, var3));
		}

		private static IEnumerator Coroutine<T, T2, T3>(Action<T, T2, T3> method, T var, T2 var2, T3 var3)
		{
			yield return new WaitForEndOfFrame();
			method(var, var2, var3);
		}

		internal void Invoke<T, T2, T3, T4>(Action<T, T2, T3, T4> method, T var, T2 var2, T3 var3, T4 var4)
		{
			StartCoroutine(Coroutine(method, var, var2, var3, var4));
		}

		private static IEnumerator Coroutine<T, T2, T3, T4>(Action<T, T2, T3, T4> method, T var, T2 var2, T3 var3, T4 var4)
		{
			yield return new WaitForEndOfFrame();
			method(var, var2, var3, var4);
		}

		internal void Invoke(YieldInstruction yieldInstruction, Action method)
		{
			StartCoroutine(Coroutine(yieldInstruction, method));
		}

		private static IEnumerator Coroutine(YieldInstruction yieldInstruction, Action method)
		{
			yield return yieldInstruction;
			method();
		}

		internal void Invoke<T>(YieldInstruction yieldInstruction, Action<T> method, T var)
		{
			StartCoroutine(Coroutine(yieldInstruction, method, var));
		}

		private static IEnumerator Coroutine<T>(YieldInstruction yieldInstruction, Action<T> method, T var)
		{
			yield return yieldInstruction;
			method(var);
		}

		internal void Invoke<T, T2>(YieldInstruction yieldInstruction, Action<T, T2> method, T var, T2 var2)
		{
			StartCoroutine(Coroutine(yieldInstruction, method, var, var2));
		}

		private static IEnumerator Coroutine<T, T2>(YieldInstruction yieldInstruction, Action<T, T2> method, T var, T2 var2)
		{
			yield return yieldInstruction;
			method(var, var2);
		}

		internal void Invoke<T, T2, T3>(YieldInstruction yieldInstruction, Action<T, T2, T3> method, T var, T2 var2, T3 var3)
		{
			StartCoroutine(Coroutine(yieldInstruction, method, var, var2, var3));
		}

		private static IEnumerator Coroutine<T, T2, T3>(YieldInstruction yieldInstruction, Action<T, T2, T3> method, T var, T2 var2, T3 var3)
		{
			yield return yieldInstruction;
			method(var, var2, var3);
		}

		internal void Invoke<T, T2, T3, T4>(YieldInstruction yieldInstruction, Action<T, T2, T3, T4> method, T var, T2 var2, T3 var3, T4 var4)
		{
			StartCoroutine(Coroutine(yieldInstruction, method, var, var2, var3, var4));
		}

		private static IEnumerator Coroutine<T, T2, T3, T4>(YieldInstruction yieldInstruction, Action<T, T2, T3, T4> method, T var, T2 var2, T3 var3, T4 var4)
		{
			yield return yieldInstruction;
			method(var, var2, var3, var4);
		}

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
