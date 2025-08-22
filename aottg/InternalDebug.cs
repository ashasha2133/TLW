using System;
using System.Diagnostics;
using UnityEngine;

public static class InternalDebug
{
	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void Break()
	{
		UnityEngine.Debug.Break();
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void ClearDeveloperConsole()
	{
		UnityEngine.Debug.ClearDeveloperConsole();
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0f, bool depthTest = true)
	{
		UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0f, bool depthTest = true)
	{
		UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void Log(object message, bool condition = true)
	{
		if (condition)
		{
			UnityEngine.Debug.Log(message);
		}
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void Log(object message, UnityEngine.Object context, bool condition = true)
	{
		if (condition)
		{
			UnityEngine.Debug.Log(message, context);
		}
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogError(object message, bool condition = true)
	{
		if (condition)
		{
			UnityEngine.Debug.LogError(message);
		}
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogError(object message, UnityEngine.Object context, bool condition = true)
	{
		if (condition)
		{
			UnityEngine.Debug.LogError(message, context);
		}
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogException(Exception exception, bool condition = true)
	{
		if (condition)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogException(Exception exception, UnityEngine.Object context, bool condition = true)
	{
		if (condition)
		{
			UnityEngine.Debug.LogException(exception, context);
		}
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarning(object message, bool condition = true)
	{
		if (condition)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	[Conditional("DEVELOPMENT_BUILD")]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarning(object message, UnityEngine.Object context, bool condition = true)
	{
		if (condition)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}
	}
}
