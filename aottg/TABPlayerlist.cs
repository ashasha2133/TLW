using System;
using System.Linq;
using Boo.Lang;
using UnityEngine;

public class TABPlayerlist : MonoBehaviour
{
	public static bool DetectIsVisible;

	public Rect detectionrect = GUIHelpers.AlignRect((float)Screen.width / 1.5f, (float)Screen.height / 1.5f, GUIHelpers.Alignment.CENTER, 0f, 0f);

	private Vector2 Scroll = Vector2.zero;

	private GUIStyle contentStyle = new GUIStyle();

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

	private void DrawTable(int ID, GUIStyle contentStyle, List<TableElement> headers, List<List<TableElement>> rows)
	{
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.background = MakeTex(1, 1, new Color(0.15f, 0.15f, 0.18f, 0.7f));
		GUIStyle style = new GUIStyle
		{
			normal = 
			{
				background = MakeTex(1, 1, new Color(0.11f, 0.11f, 0.14f, 0.8f))
			}
		};
		GUIStyle gUIStyle2 = new GUIStyle
		{
			normal = 
			{
				background = MakeTex(1, 1, new Color(0.13f, 0.13f, 0.16f, 0.7f))
			}
		};
		GUIStyle gUIStyle3 = new GUIStyle
		{
			normal = 
			{
				background = MakeTex(1, 1, new Color(0.15f, 0.15f, 0.18f, 0.7f))
			}
		};
		Scroll = GUILayout.BeginScrollView(Scroll, gUIStyle);
		GUILayout.BeginHorizontal(style);
		GUILayout.FlexibleSpace();
		foreach (TableElement header in headers)
		{
			if (GUILayout.Button(header.ToString(), contentStyle, GUILayout.Width(header.width)) && header.hasAction)
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
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();
		int num = 0;
		foreach (List<TableElement> row in rows)
		{
			GUILayout.BeginHorizontal((num++ % 2 == 0) ? gUIStyle2 : gUIStyle3);
			GUILayout.FlexibleSpace();
			foreach (TableElement item in row)
			{
				if (GUILayout.Button(item.ToString(), contentStyle, GUILayout.Width(item.width)) && item.hasAction)
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
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		GUI.DragWindow();
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

	private void CreatePlayerlist(int ID, GUIStyle contentStyle)
	{
		List<TableElement> headers = new List<TableElement>
		{
			new TableElement("ID", "header", "small", hasAction: true),
			new TableElement("Name", "header", "large"),
			new TableElement("Kills", "header", "small"),
			new TableElement("Deaths", "header", "small"),
			new TableElement("Max Damage", "header"),
			new TableElement("Total Damage", "header", "large"),
			new TableElement("Balance", "header"),
			new TableElement("Ping", "header", "small"),
			new TableElement("Mod", "header", "small")
		};
		List<List<TableElement>> list = new List<List<TableElement>>();
		PhotonPlayer[] array = PhotonNetwork.playerList.OrderBy((PhotonPlayer player) => player.ID).ToArray();
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer.ID > 0 && photonPlayer != null && !photonPlayer.ToString().Contains("Unknown") && !(photonPlayer.ToString() == string.Empty))
			{
				string content = ((!photonPlayer.isLocal) ? ("[" + photonPlayer.ID + "] ") : ("<color=#00CC00>[" + photonPlayer.ID + "]</color> ")) + (photonPlayer.isMasterClient ? "(MC)" : "");
				string content2 = (RCextensions.returnBoolFromObject(photonPlayer.customProperties["dead"]) ? ("<color=red>*dead*</color> " + photonPlayer.uiname.hexColor()) : photonPlayer.uiname.hexColor());
				int num = RCextensions.returnIntFromObject(photonPlayer.customProperties["kills"]);
				int num2 = RCextensions.returnIntFromObject(photonPlayer.customProperties["deaths"]);
				int num3 = RCextensions.returnIntFromObject(photonPlayer.customProperties["max_dmg"]);
				int num4 = RCextensions.returnIntFromObject(photonPlayer.customProperties["total_dmg"]);
				double num5 = (double)(1200 * RCextensions.returnIntFromObject(photonPlayer.customProperties["kills"]) + RCextensions.returnIntFromObject(photonPlayer.customProperties["total_dmg"])) * (Math.Pow(10.0, 4.0) + (double)RCextensions.returnIntFromObject(photonPlayer.customProperties["max_dmg"])) / (5.0 * Math.Pow(10.0, 5.0) * (double)(20 + RCextensions.returnIntFromObject(photonPlayer.customProperties["deaths"])));
				string content3 = ((photonPlayer.customProperties["Ping"] != null) ? RCextensions.returnIntFromObject(photonPlayer.customProperties["Ping"]).ToString() : "-");
				list.Add(new List<TableElement>
				{
					new TableElement(content, "cell", "small", hasAction: true),
					new TableElement(content2, "cell", "large"),
					new TableElement(num.ToString(), "cell", "small"),
					new TableElement(num2.ToString(), "cell", "small"),
					new TableElement(num3.ToString(), "cell"),
					new TableElement(num4.ToString(), "cell", "large"),
					new TableElement(num5.ToString("N2"), "cell", "", hasAction: false, num5.ToString()),
					new TableElement(content3, "cell", "small"),
					new TableElement(Mods(photonPlayer), "cell", "small")
				});
			}
		}
		DrawTable(ID, contentStyle, headers, list);
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

	private void CreateBombPlayerlist(int ID, GUIStyle contentStyle)
	{
		List<TableElement> headers = new List<TableElement>
		{
			new TableElement("ID", "header", "small", hasAction: true),
			new TableElement("Name", "header", "large"),
			new TableElement("Kills", "header", "small"),
			new TableElement("Deaths", "header", "small"),
			new TableElement("New Bomb Stats", "header", "large"),
			new TableElement("Old Bomb Stats", "header", "large"),
			new TableElement("Ping", "header", "small"),
			new TableElement("Mod", "header", "small")
		};
		List<List<TableElement>> list = new List<List<TableElement>>();
		PhotonPlayer[] array = PhotonNetwork.playerList.OrderBy((PhotonPlayer player) => player.ID).ToArray();
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer.ID > 0 && photonPlayer != null && !photonPlayer.ToString().Contains("Unknown") && !(photonPlayer.ToString() == string.Empty))
			{
				string content = ((!photonPlayer.isLocal) ? ("[" + photonPlayer.ID + "] ") : ("<color=#00CC00>[" + photonPlayer.ID + "]</color> ")) + (photonPlayer.isMasterClient ? "(MC)" : "");
				string content2 = (RCextensions.returnBoolFromObject(photonPlayer.customProperties["dead"]) ? ("<color=red>*dead*</color> " + photonPlayer.uiname.hexColor()) : photonPlayer.uiname.hexColor());
				int num = RCextensions.returnIntFromObject(photonPlayer.customProperties["kills"]);
				int num2 = RCextensions.returnIntFromObject(photonPlayer.customProperties["deaths"]);
				string content3 = ((photonPlayer.customProperties["Ping"] != null) ? RCextensions.returnIntFromObject(photonPlayer.customProperties["Ping"]).ToString() : "-");
				list.Add(new List<TableElement>
				{
					new TableElement(content, "cell", "small", hasAction: true),
					new TableElement(content2, "cell", "large"),
					new TableElement(num.ToString(), "cell", "small"),
					new TableElement(num2.ToString(), "cell", "small"),
					new TableElement(NewStatsString(photonPlayer), "cell", "large"),
					new TableElement(OldStatsString(photonPlayer), "cell", "large"),
					new TableElement(content3, "cell", "small"),
					new TableElement(Mods(photonPlayer), "cell", "small")
				});
			}
		}
		DrawTable(ID, contentStyle, headers, list);
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
			return text;
		}
		if (HERO.BombStatsCounter.ContainsKey(player))
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

	private void CheckerWindow(int ID)
	{
		contentStyle = new GUIStyle(GUI.skin.label);
		contentStyle.alignment = TextAnchor.MiddleCenter;
		if (PhotonNetwork.inRoom)
		{
			if (GameSettings.bombMode == 0)
			{
				CreatePlayerlist(ID, contentStyle);
			}
			else
			{
				CreateBombPlayerlist(ID, contentStyle);
			}
		}
	}

	private void OnGUI()
	{
		if (DetectIsVisible)
		{
			detectionrect = GUI.Window(600, detectionrect, CheckerWindow, "<color=white><b>Playerlist</b></color>");
		}
	}

	private void Update()
	{
		if (FengGameManagerMKII.inputRC.isInputExtras(InputCodeRC.secondplayerlist))
		{
			DetectIsVisible = true;
		}
		else if (FengGameManagerMKII.inputRC.isInputExtrasUp(InputCodeRC.secondplayerlist))
		{
			DetectIsVisible = false;
		}
	}
}
