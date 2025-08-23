using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class AntiCheat : MonoBehaviour
{
	public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

	private List<GameObject> roots;

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int GetWindowTextLength(IntPtr hWnd);

	[DllImport("user32.dll")]
	private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

	private static string GetWindowText(IntPtr hWnd)
	{
		int windowTextLength = GetWindowTextLength(hWnd);
		if (windowTextLength > 0)
		{
			StringBuilder stringBuilder = new StringBuilder(windowTextLength + 1);
			GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}
		return string.Empty;
	}

	private static IEnumerable<IntPtr> FindWindows(EnumWindowsProc filter)
	{
		_ = IntPtr.Zero;
		List<IntPtr> windows = new List<IntPtr>();
		EnumWindows(delegate(IntPtr wnd, IntPtr param)
		{
			if (filter(wnd, param))
			{
				windows.Add(wnd);
			}
			return true;
		}, IntPtr.Zero);
		return windows;
	}

	private static IEnumerable<IntPtr> FindWindowsWithText(string titleText)
	{
		return FindWindows((IntPtr wnd, IntPtr param) => GetWindowText(wnd).Contains(titleText));
	}

	public void IsCheatEngineRunning()
	{
		foreach (IntPtr item in FindWindowsWithText("Cheat Engine"))
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				FengGameManagerMKII.instance.photonView.RPC("ChatPM", PhotonTargets.Others, FengGameManagerMKII.Chatname, "<color=red>Hello! My mod detected " + GetWindowText(item) + " (Cheat Engine) currently working on my computer, window n. " + item + ". Please don't notify</color> " + FengGameManagerMKII.nameField + " <color=red>and screen this and let others know :)</color>");
			}
			FengGameManagerMKII.instance.CheatEngine = true;
		}
		foreach (IntPtr item2 in FindWindowsWithText("Game Fuq"))
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				FengGameManagerMKII.instance.photonView.RPC("ChatPM", PhotonTargets.Others, FengGameManagerMKII.Chatname, "<color=red>Hello! My mod detected " + GetWindowText(item2) + " (Cheat Engine) currently working on my computer, window n. " + item2 + ". Please don't notify</color> " + FengGameManagerMKII.nameField + " <color=red>and screen this and let others know :)</color>");
			}
			FengGameManagerMKII.instance.CheatEngine = true;
		}
		if (Time.timeScale != 1f && FengGameManagerMKII.instance.pauseWaitTime <= 3f)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				FengGameManagerMKII.instance.photonView.RPC("ChatPM", PhotonTargets.Others, FengGameManagerMKII.Chatname, "<color=red>Hello! My mod detected different game speed. Please don't notify</color> " + FengGameManagerMKII.nameField + " <color=red>and screen this and let others know :)</color>");
			}
			FengGameManagerMKII.instance.CheatEngine = true;
			Time.timeScale = 1f;
		}
	}

	private void Start()
	{
		if (roots != null)
		{
			roots.Clear();
		}
		else
		{
			roots = new List<GameObject>();
		}
		DetectBepInEx();
		if (!IsInvoking("IsCheatEngineRunning"))
		{
			InvokeRepeating("IsCheatEngineRunning", 0f, 30f);
		}
	}

	private void DetectBepInEx()
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
		roots.Clear();
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject != null && gameObject.transform.parent == null && !roots.Contains(gameObject))
			{
				roots.Add(gameObject);
			}
		}
		foreach (GameObject root in roots)
		{
			if (root.name.Contains("BepInEx"))
			{
				Application.Quit();
			}
		}
	}
}
