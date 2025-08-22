using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
	private static NGUIDebug mInstance = null;

	private static List<string> mLines = new List<string>();

	public static void DrawBounds(Bounds b)
	{
		_ = b.center;
		_ = b.center - b.extents;
		_ = b.center + b.extents;
	}

	public static void Log(string text)
	{
		if (Application.isPlaying)
		{
			if (mLines.Count > 20)
			{
				mLines.RemoveAt(0);
			}
			mLines.Add(text);
			if (mInstance == null)
			{
				GameObject obj = new GameObject("_NGUI Debug");
				mInstance = obj.AddComponent<NGUIDebug>();
				Object.DontDestroyOnLoad(obj);
			}
		}
	}

	private void OnGUI()
	{
		int i = 0;
		for (int count = mLines.Count; i < count; i++)
		{
			GUILayout.Label(mLines[i]);
		}
	}
}
