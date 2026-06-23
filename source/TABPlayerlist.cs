using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TABPlayerlist : MonoBehaviour
{
	private int windowId;

	private Rect windowRect;

	private GUIStyle clearStyle;

	private bool bombMode;

	public static bool DetectIsVisible;

	public Rect detectionrect;

	private Vector2 Scroll = Vector2.zero;

	private GUIStyle contentStyle = new GUIStyle();

	private GUIStyle backgroundColor;

	private GUIStyle headerColor;

	private GUIStyle row1Color;

	private GUIStyle row2Color;

	private float scaleFactor = 1f;

	private const float BASE_RESOLUTION_WIDTH = 1920f;

	private List<TableElement> cachedHeaders;

	private List<List<TableElement>> cachedRows;

	private bool cacheValid;

	private int lastPlayerCount;

	private bool lastBombMode;

	private float lastCacheTime;

	private const float CACHE_REFRESH_INTERVAL = 1f;

	private void Awake()
	{
		windowId = 1;
		windowRect = new Rect(0f, 0f, 1000f, 0f);
		UpdateScaleFactor();
		clearStyle = new GUIStyle();
		clearStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0f));
		backgroundColor = new GUIStyle();
		backgroundColor.normal.background = MakeTex(2, 2, new Color(0.15f, 0.15f, 0.18f, 0.7f));
		backgroundColor.padding = new RectOffset(5, 5, 5, 5);
		headerColor = new GUIStyle();
		headerColor.normal.background = MakeTex(2, 2, new Color(0.11f, 0.11f, 0.14f, 0.8f));
		headerColor.normal.textColor = new Color(1f, 1f, 1f, 1f);
		headerColor.fontSize = Mathf.RoundToInt(14f * scaleFactor);
		headerColor.fontStyle = FontStyle.Bold;
		headerColor.padding = new RectOffset(5, 5, 5, 5);
		row1Color = new GUIStyle();
		row1Color.normal.background = MakeTex(2, 2, new Color(0.13f, 0.13f, 0.16f, 0.7f));
		row1Color.padding = new RectOffset(5, 5, 5, 5);
		row2Color = new GUIStyle();
		row2Color.normal.background = MakeTex(2, 2, new Color(0.15f, 0.15f, 0.18f, 0.7f));
		row2Color.padding = new RectOffset(5, 5, 5, 5);
	}

	private void UpdateScaleFactor()
	{
		scaleFactor = Mathf.Clamp((float)Screen.width / 1920f, 0.5f, 2f);
	}

	private bool ShouldInvalidateCache()
	{
		if (!cacheValid)
		{
			return true;
		}
		int num = ((PhotonNetwork.playerList != null) ? PhotonNetwork.playerList.Length : 0);
		if (num != lastPlayerCount)
		{
			lastPlayerCount = num;
			return true;
		}
		bool flag = GameSettings.bombMode != 0;
		if (flag != lastBombMode)
		{
			lastBombMode = flag;
			return true;
		}
		if (Time.time - lastCacheTime > 1f)
		{
			return true;
		}
		return false;
	}

	public void InvalidateCache()
	{
		cacheValid = false;
		cachedHeaders = null;
		cachedRows = null;
	}

	private string GetCSV(List<TableElement> headers, List<List<TableElement>> rows)
	{
		string text = string.Empty;
		foreach (TableElement header in headers)
		{
			text = text + header.content + "\t";
		}
		text = text.TrimEnd('\t');
		text += "\n";
		foreach (List<TableElement> row in rows)
		{
			foreach (TableElement item in row)
			{
				text = text + item.rawContent + "\t";
			}
			text = text.TrimEnd('\t');
			text += "\n";
		}
		return text;
	}

	private void DrawTable(int ID)
	{
		try
		{
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
			gUIStyle.alignment = TextAnchor.MiddleCenter;
			if (PhotonNetwork.inRoom)
			{
				bool flag = GameSettings.bombMode != 0;
				CreatePlayerlist(ID, gUIStyle, flag);
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Error in DrawTable: " + ex.Message + ex.StackTrace);
		}
		finally
		{
			GUI.DragWindow();
		}
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

	private void DrawTableLayout(int ID, GUIStyle contentStyle, List<TableElement> headers, List<List<TableElement>> rows)
	{
		GUIStyle gUIStyle = new GUIStyle(contentStyle);
		gUIStyle.alignment = TextAnchor.MiddleCenter;
		gUIStyle.margin = new RectOffset(0, 0, 0, 0);
		gUIStyle.padding = new RectOffset(2, 2, 2, 2);
		GUIStyle gUIStyle2 = new GUIStyle(contentStyle);
		gUIStyle2.alignment = TextAnchor.MiddleCenter;
		gUIStyle2.margin = new RectOffset(0, 0, 0, 0);
		gUIStyle2.padding = new RectOffset(2, 2, 2, 2);
		GUILayout.BeginVertical(backgroundColor);
		GUILayout.BeginHorizontal(headerColor, GUILayout.ExpandWidth(expand: true));
		foreach (TableElement header in headers)
		{
			if (GUILayout.Button(header.ToString(), gUIStyle, GUILayout.Width(header.width), GUILayout.ExpandHeight(expand: false)) && header.hasAction)
			{
				string cSV = GetCSV(headers, rows);
				TextEditor textEditor = new TextEditor();
				textEditor.content = new GUIContent
				{
					text = cSV
				};
				textEditor.SelectAll();
				textEditor.Copy();
			}
		}
		GUILayout.EndHorizontal();
		int num = 0;
		foreach (List<TableElement> row in rows)
		{
			GUILayout.BeginHorizontal((num++ % 2 == 0) ? row1Color : row2Color, GUILayout.ExpandWidth(expand: true));
			foreach (TableElement item in row)
			{
				if (GUILayout.Button(item.ToString(), gUIStyle2, GUILayout.Width(item.width), GUILayout.ExpandHeight(expand: false)) && item.hasAction)
				{
					string cSV2 = GetCSV(headers, new List<List<TableElement>> { row });
					TextEditor textEditor2 = new TextEditor();
					textEditor2.content = new GUIContent
					{
						text = cSV2
					};
					textEditor2.SelectAll();
					textEditor2.Copy();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	private void CreatePlayerlist(int ID, GUIStyle contentStyle, bool bombMode)
	{
		if (ShouldInvalidateCache())
		{
			BuildPlayerlistCache(bombMode);
		}
		if (cachedHeaders != null && cachedRows != null)
		{
			DrawTableLayout(ID, contentStyle, cachedHeaders, cachedRows);
		}
	}

	private void BuildPlayerlistCache(bool bombMode)
	{
		UpdateScaleFactor();
		List<TableElement> list = new List<TableElement>
		{
			new TableElement("ID", "header", "small", hasAction: true, "", scaleFactor),
			new TableElement("Name", "header", "large", hasAction: false, "", scaleFactor),
			new TableElement("Kills", "header", "small", hasAction: false, "", scaleFactor),
			new TableElement("Deaths", "header", "small", hasAction: false, "", scaleFactor)
		};
		if (bombMode)
		{
			list.Add(new TableElement("New Bomb Stats", "header", "large", hasAction: false, "", scaleFactor));
			list.Add(new TableElement("Old Bomb Stats", "header", "large", hasAction: false, "", scaleFactor));
		}
		else
		{
			list.Add(new TableElement("Max Damage", "header", "", hasAction: false, "", scaleFactor));
			list.Add(new TableElement("Total Damage", "header", "large", hasAction: false, "", scaleFactor));
		}
		list.Add(new TableElement("Ping", "header", "small", hasAction: false, "", scaleFactor));
		list.Add(new TableElement("Mod", "header", "small", hasAction: false, "", scaleFactor));
		List<List<TableElement>> list2 = new List<List<TableElement>>();
		if (PhotonNetwork.playerList != null)
		{
			PhotonPlayer[] array = PhotonNetwork.playerList.OrderBy((PhotonPlayer player) => player.ID).ToArray();
			foreach (PhotonPlayer photonPlayer in array)
			{
				if (photonPlayer.ID > 0 && photonPlayer != null && !photonPlayer.ToString().Contains("Unknown") && !(photonPlayer.ToString() == string.Empty))
				{
					string content = ((!photonPlayer.isLocal) ? ("[" + photonPlayer.ID + "] ") : ("<color=#00CC00>[" + photonPlayer.ID + "]</color> ")) + (photonPlayer.isMasterClient ? "(MC)" : "");
					string text = photonPlayer.uiname.ToUpper().hexColor();
					if (RCextensions.returnBoolFromObject(photonPlayer.customProperties["dead"]))
					{
						text = "<color=red>[DEAD]</color> " + text;
					}
					int num2 = RCextensions.returnIntFromObject(photonPlayer.customProperties["kills"]);
					int num3 = RCextensions.returnIntFromObject(photonPlayer.customProperties["deaths"]);
					string content2 = ((photonPlayer.customProperties["Ping"] != null) ? RCextensions.returnIntFromObject(photonPlayer.customProperties["Ping"]).ToString() : "-");
					List<TableElement> list3 = new List<TableElement>
					{
						new TableElement(content, "cell", "small", hasAction: true, "", scaleFactor),
						new TableElement(text, "cell", "large", hasAction: false, "", scaleFactor),
						new TableElement(num2.ToString(), "cell", "small", hasAction: false, "", scaleFactor),
						new TableElement(num3.ToString(), "cell", "small", hasAction: false, "", scaleFactor)
					};
					if (bombMode)
					{
						list3.Add(new TableElement(NewStatsString(photonPlayer), "cell", "large", hasAction: false, "", scaleFactor));
						list3.Add(new TableElement(OldStatsString(photonPlayer), "cell", "large", hasAction: false, "", scaleFactor));
					}
					else
					{
						int num4 = RCextensions.returnIntFromObject(photonPlayer.customProperties["max_dmg"]);
						int num5 = RCextensions.returnIntFromObject(photonPlayer.customProperties["total_dmg"]);
						list3.Add(new TableElement(num4.ToString(), "cell", "", hasAction: false, "", scaleFactor));
						list3.Add(new TableElement(num5.ToString(), "cell", "large", hasAction: false, "", scaleFactor));
					}
					list3.Add(new TableElement(content2, "cell", "small", hasAction: false, "", scaleFactor));
					list3.Add(new TableElement(Mods(photonPlayer), "cell", "small", hasAction: false, "", scaleFactor));
					list2.Add(list3);
				}
			}
		}
		cachedHeaders = list;
		cachedRows = list2;
		cacheValid = true;
		lastCacheTime = Time.time;
		lastPlayerCount = ((PhotonNetwork.playerList != null) ? PhotonNetwork.playerList.Length : 0);
		lastBombMode = bombMode;
	}

	private string Mods(PhotonPlayer player)
	{
		string result = "";
		if (player.Tlw || player.isLocal)
		{
			result = "TLW";
		}
		if (player.OldTlw)
		{
			result = "OldTLW";
		}
		else if (player.NewTlw)
		{
			result = "NewTLW";
		}
		else if (player.RRC)
		{
			result = "RRC";
		}
		return result;
	}

	private string OldStatsString(PhotonPlayer player)
	{
		string empty = string.Empty;
		string text = "x";
		if (player.customProperties.ContainsKey("RCBombRadius"))
		{
			text = (((float)player.customProperties["RCBombRadius"] - 20f) / 4f).ToString("0.##");
		}
		empty = text + " / x / x / x";
		if (HERO.BombStatsCounter.ContainsKey(player))
		{
			empty = HERO.BombStatsCounter[player];
			int num = empty.IndexOf('+');
			if (num != -1)
			{
				empty = empty.Substring(0, num).Trim();
			}
		}
		else if (player.RRC)
		{
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			if (player.customProperties.ContainsKey("RCBombRange"))
			{
				num2 = RCextensions.returnFloatFromObject(player.customProperties["RCBombRange"]);
				num2 = BombUtil.GetOldRangeCost(BombUtil.GetBombRange(num2, 0f, 4f, 7f));
			}
			if (player.customProperties.ContainsKey("RCBombSpeed"))
			{
				num3 = RCextensions.returnFloatFromObject(player.customProperties["RCBombSpeed"]);
				num3 = BombUtil.GetOldSpeedCost(BombUtil.GetBombSpeed(num3, 3f, 10.5f, 10.5f));
			}
			if (player.customProperties.ContainsKey("RCBombCD"))
			{
				num4 = RCextensions.returnFloatFromObject(player.customProperties["RCBombCD"]);
				num4 = BombUtil.GetOldCooldownCost(BombUtil.GetBombCooldown(num4, 4f, 7f, 7f));
			}
			empty = text + " / " + num2.ToString("0.##") + " / " + num3.ToString("0.##") + " / " + num4.ToString("0.##");
		}
		return empty;
	}

	private string NewStatsString(PhotonPlayer player)
	{
		string text = string.Empty;
		if (player.RRC)
		{
			foreach (string item in new List<string> { "RCBombRadiusCost", "RCBombRange", "RCBombSpeed", "RCBombCD" })
			{
				if (player.customProperties.ContainsKey(item))
				{
					text = text + RCextensions.returnFloatFromObject(player.customProperties[item]) + " ";
				}
			}
		}
		else if (HERO.BombStatsCounter.ContainsKey(player))
		{
			text = HERO.BombStatsCounter[player];
			int num = text.IndexOf('+');
			if (num != -1)
			{
				text = text.Substring(num + 1).Trim();
			}
		}
		return text;
	}

	private void OnGUI()
	{
		if (DetectIsVisible)
		{
			windowRect.y = 120f * scaleFactor;
			float num = ((GameSettings.bombMode != 0) ? 920f : 860f);
			windowRect.width = num * scaleFactor;
			windowRect.x = Mathf.Round(((float)Screen.width - windowRect.width) * 0.5f);
			windowRect = GUILayout.Window(windowId, windowRect, DrawTable, string.Empty, clearStyle);
		}
	}

	private void Update()
	{
		bool detectIsVisible = DetectIsVisible;
		bool detectIsVisible2 = false;
		InputManagerRC inputRC = FengGameManagerMKII.inputRC;
		int secondplayerlist = InputCodeRC.secondplayerlist;
		if (inputRC != null)
		{
			detectIsVisible2 = ((inputRC.extrawheel == null || secondplayerlist < 0 || secondplayerlist >= inputRC.extrawheel.Length || inputRC.extrawheel[secondplayerlist] != 0 || inputRC.extraKey == null || secondplayerlist >= inputRC.extraKey.Length || inputRC.extraKey[secondplayerlist] == KeyCode.None) ? inputRC.isInputExtras(secondplayerlist) : Input.GetKey(inputRC.extraKey[secondplayerlist]));
		}
		DetectIsVisible = detectIsVisible2;
		if (DetectIsVisible && !detectIsVisible)
		{
			InvalidateCache();
		}
		bombMode = GameSettings.bombMode != 0;
	}

	public static void ClearAllCaches()
	{
		TABPlayerlist[] array = UnityEngine.Object.FindObjectsOfType<TABPlayerlist>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].InvalidateCache();
		}
	}
}
