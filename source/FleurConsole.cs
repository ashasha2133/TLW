using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Photon;
using UnityEngine;
using Utility;

public class FleurConsole : Photon.MonoBehaviour
{
	public enum DebugType
	{
		ANTI,
		LOG,
		STATE
	}

	private static GUIStyle currentStyle;

	private bool IsVisible = true;

	public static FleurConsole instance;

	public static List<string> messages = new List<string>();

	public Stopwatch timeOfFade = new Stopwatch();

	private void Awake()
	{
		instance = this;
	}

	public int Log(string content, DebugType type)
	{
		string text = "<color=";
		string text2 = string.Empty;
		switch (type)
		{
		case DebugType.ANTI:
			text2 = "red><b>「►」:</b></color> ";
			break;
		case DebugType.STATE:
			text2 = "cyan><b>[➤]:</b></color> ";
			break;
		case DebugType.LOG:
			text2 = "white><b>（╱）:</b></color> ";
			break;
		}
		messages.Add(text + text2 + content);
		if (!IsVisible)
		{
			IsVisible = true;
		}
		timeOfFade.Reset();
		timeOfFade.Start();
		return messages.Count - 1;
	}

	private Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	public static void TryHighlightFeed(List<FeedEntry> feedEntries, List<FeedEntry> previousFeedEntries)
	{
		int num = Math.Max(messages.Count - FengGameManagerMKII.instance.ConsoleMessages, 0);
		foreach (FeedEntry previousFeedEntry in previousFeedEntries)
		{
			if (previousFeedEntry.Index >= num && previousFeedEntry.Index < messages.Count)
			{
				string text = messages[previousFeedEntry.Index].Substring(6, messages[previousFeedEntry.Index].Length - 14);
				if (text.Replace("<color=white><b>（╱）:</b></color> ", string.Empty) == previousFeedEntry.ChatElement)
				{
					messages[previousFeedEntry.Index] = text;
				}
			}
		}
		foreach (FeedEntry feedEntry in feedEntries)
		{
			if (feedEntry.Index >= num && feedEntry.Index < messages.Count && messages[feedEntry.Index].Replace("<color=white><b>（╱）:</b></color> ", string.Empty) == feedEntry.ChatElement)
			{
				messages[feedEntry.Index] = "<b><i>" + messages[feedEntry.Index] + "</i></b>";
				UnityEngine.Debug.Log("Added " + messages[feedEntry.Index]);
			}
		}
		if (!instance.IsVisible)
		{
			instance.IsVisible = true;
		}
		instance.timeOfFade.Reset();
		instance.timeOfFade.Start();
	}

	private void Style()
	{
		if (currentStyle == null)
		{
			currentStyle = new GUIStyle(GUI.skin.label);
			currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.2f));
			currentStyle.fontSize = 14;
			currentStyle.margin = new RectOffset(0, 0, 0, 0);
			currentStyle.padding = new RectOffset(0, 0, 0, 0);
			currentStyle.border = new RectOffset(0, 0, 0, 0);
			currentStyle.overflow = new RectOffset(0, 0, 0, 0);
			currentStyle.wordWrap = true;
		}
	}

	public void OnGUI()
	{
		if (!IsVisible || (!PhotonNetwork.inRoom && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE || (int)FengGameManagerMKII.settings[244] != 2)))
		{
			return;
		}
		Style();
		GUILayout.BeginArea(GUIHelpers.AlignRect(370f, 400f, GUIHelpers.Alignment.BOTTOMRIGHT, 0f, -30f));
		GUILayout.FlexibleSpace();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = messages.Count - FengGameManagerMKII.instance.ConsoleMessages; i < messages.Count; i++)
		{
			if (i >= 0)
			{
				stringBuilder.AppendLine(messages[i]);
			}
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
		}
		GUILayout.Label(stringBuilder.ToString(), (GameSettings.ConsoleBackground > 0) ? currentStyle : ((GUIStyle)"Label"), GUILayout.MinWidth(GUIHelpers.AlignRect(370f, 400f, GUIHelpers.Alignment.BOTTOMRIGHT, 0f, -30f).width));
		GUILayout.EndArea();
	}

	private void Start()
	{
		if (FengGameManagerMKII.StartingConsole)
		{
			messages = new List<string> { "<b><color=#9ccfe7>[WELCOME BACK]</color></b>" };
			FengGameManagerMKII.StartingConsole = false;
		}
	}

	private void Update()
	{
		if (FengGameManagerMKII.inputRC.isInputExtras(InputCodeRC.showconsole))
		{
			timeOfFade.Reset();
			if (!IsVisible)
			{
				IsVisible = true;
				timeOfFade.Reset();
				timeOfFade.Start();
			}
			else if (IsVisible)
			{
				IsVisible = false;
			}
		}
		if ((PhotonNetwork.inRoom || (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && (int)FengGameManagerMKII.settings[244] == 2)) && timeOfFade.IsRunning)
		{
			if (timeOfFade.ElapsedMilliseconds >= 1000 * FengGameManagerMKII.instance.ConsoleFadeTime && IsVisible)
			{
				IsVisible = false;
			}
			if (timeOfFade.ElapsedMilliseconds < 1000 * FengGameManagerMKII.instance.ConsoleFadeTime && !IsVisible)
			{
				IsVisible = true;
			}
		}
	}
}
