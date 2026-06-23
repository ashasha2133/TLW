using ExitGames.Client.Photon;
using UnityEngine;

internal class DetectionPanel : MonoBehaviour
{
	private static GUIStyle FontStyle;

	private int settings;

	private PhotonPlayer choosedPlayer;

	public static bool DetectIsVisible;

	private Rect detectionrect = new Rect(25f, 25f, 720f, 550f);

	private Vector2 Scroll = Vector2.zero;

	public bool statsOn;

	private static Texture2D[] DetectionTextures;

	private void CheckerWindow(int ID)
	{
		int fontSize = GUI.skin.button.fontSize;
		Font font = GUI.skin.button.font;
		GUI.skin.button.font = FengGameManagerMKII.Segoe;
		GUI.skin.button.fontSize = 12;
		FontStyle = new GUIStyle(GUI.skin.label);
		FontStyle.fontSize = 12;
		FontStyle.font = FengGameManagerMKII.Segoe;
		GUI.DrawTexture(new Rect(0f, 0f, 500f, 550f), DetectionTextures[0]);
		GUI.DrawTexture(new Rect(500f, 0f, 230f, 550f), DetectionTextures[1]);
		if (PhotonNetwork.inRoom)
		{
			GUILayout.BeginArea(new Rect(0f, 30f, 190f, 440f));
			Scroll = GUILayout.BeginScrollView(Scroll, FontStyle);
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer in playerList)
			{
				if (photonPlayer.ID > 0 && photonPlayer != null && !photonPlayer.ToString().Contains("Unknown") && photonPlayer.ToString() != string.Empty && GUILayout.Button("[" + photonPlayer.ID + "] " + photonPlayer.uiname.hexColor(), FontStyle))
				{
					choosedPlayer = photonPlayer;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(200f, 30f, 280f, 405f));
			string text = ((choosedPlayer.customProperties["name"] == null) ? "Unknown" : choosedPlayer.customProperties["name"].ToString().hexColor());
			if (text == null)
			{
				text = "Uknown";
			}
			if (text != null && text != "Uknown" && choosedPlayer.ID > 0 && text != string.Empty && !choosedPlayer.ToString().Contains("Unknown"))
			{
				if (settings == 0)
				{
					Scroll = GUILayout.BeginScrollView(Scroll, FontStyle);
					GUILayout.Label("ID: " + choosedPlayer.ID, FontStyle);
					GUILayout.Label("Name: " + text, FontStyle);
					GUILayout.Label("Guild: " + choosedPlayer.customProperties["guildName"], FontStyle);
					GUILayout.Label("Property: " + CheckProps(choosedPlayer), FontStyle);
					GUILayout.Label("Character: " + RCextensions.returnStringFromObject(choosedPlayer.customProperties["character"]), FontStyle);
					GUILayout.Label("SPD: " + RCextensions.returnIntFromObject(choosedPlayer.customProperties["statSPD"]) + "                BLA: " + RCextensions.returnIntFromObject(choosedPlayer.customProperties["statBLA"]), FontStyle);
					GUILayout.Label("GAS: " + RCextensions.returnIntFromObject(choosedPlayer.customProperties["statGAS"]) + "                ACL: " + RCextensions.returnIntFromObject(choosedPlayer.customProperties["statACL"]), FontStyle);
					if (choosedPlayer.customProperties["RCBombRadius"] != null)
					{
						GUILayout.Label("Radius: " + ((float)choosedPlayer.customProperties["RCBombRadius"] - 20f) / 4f, FontStyle);
					}
					GUILayout.Label("Bomb A:" + RCextensions.returnFloatFromObject(choosedPlayer.customProperties["RCBombA"]), FontStyle);
					GUILayout.Label("Bomb B: " + RCextensions.returnFloatFromObject(choosedPlayer.customProperties["RCBombB"]), FontStyle);
					GUILayout.Label("Bomb G: " + RCextensions.returnFloatFromObject(choosedPlayer.customProperties["RCBombG"]), FontStyle);
					GUILayout.Label("Bomb R: " + RCextensions.returnFloatFromObject(choosedPlayer.customProperties["RCBombR"]), FontStyle);
					GUILayout.Label("Hair1: " + RCextensions.returnFloatFromObject(choosedPlayer.customProperties["hair_color1"]), FontStyle);
					GUILayout.Label("Hair2: " + RCextensions.returnFloatFromObject(choosedPlayer.customProperties["hair_color2"]), FontStyle);
					GUILayout.Label("Hair3: " + RCextensions.returnFloatFromObject(choosedPlayer.customProperties["hair_color3"]), FontStyle);
					GUILayout.Label("FriendName: " + ((choosedPlayer.FriendName != null) ? choosedPlayer.FriendName : ""));
					GUILayout.EndScrollView();
				}
				else if (settings == 1)
				{
					Scroll = GUILayout.BeginScrollView(Scroll, FontStyle);
					GUILayout.Label("ID: " + choosedPlayer.ID, FontStyle);
					GUILayout.Label("Name: " + text, FontStyle);
					GUILayout.Label(choosedPlayer.allProps, FontStyle);
					GUILayout.EndScrollView();
				}
				else if (settings == 2)
				{
					Scroll = GUILayout.BeginScrollView(Scroll, FontStyle);
					GUILayout.Label("ID: " + choosedPlayer.ID, FontStyle);
					GUILayout.Label("Name: " + text, FontStyle);
					GUILayout.Label(choosedPlayer.RPCs, FontStyle);
					GUILayout.EndScrollView();
				}
				else if (settings == 3)
				{
					Scroll = GUILayout.BeginScrollView(Scroll, FontStyle);
					GUILayout.Label("ID: " + choosedPlayer.ID, FontStyle);
					GUILayout.Label("Name: " + choosedPlayer.uiname.hexColor(), FontStyle);
					GUILayout.Label(choosedPlayer.Events, FontStyle);
					GUILayout.EndScrollView();
				}
				else if (settings == 4)
				{
					Scroll = GUILayout.BeginScrollView(Scroll, FontStyle);
					GUILayout.Label("ID: " + choosedPlayer.ID, FontStyle);
					GUILayout.Label("Name: " + text, FontStyle);
					GUILayout.Label("Bytes: " + (float)choosedPlayer.BytesReceived, FontStyle);
					GUILayout.Label("KB: " + ((float)choosedPlayer.BytesReceived / 1024f).RoundTo(2), FontStyle);
					if (GUILayout.Button("Clean", GUILayout.Width(45f)))
					{
						playerList = PhotonNetwork.playerList;
						for (int i = 0; i < playerList.Length; i++)
						{
							playerList[i].BytesReceived = 0L;
						}
					}
					GUILayout.EndScrollView();
				}
			}
			GUILayout.BeginHorizontal(FontStyle);
			if (GUILayout.Button("Info", "Button"))
			{
				settings = 0;
			}
			if (GUILayout.Button("Properties", "Button"))
			{
				settings = 1;
			}
			if (GUILayout.Button("RPCs", "Button"))
			{
				settings = 2;
			}
			if (GUILayout.Button("Events", "Button"))
			{
				settings = 3;
			}
			if (GUILayout.Button("Bytes", "Button"))
			{
				settings = 4;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		GUILayout.BeginArea(new Rect(505f, 0f, 210f, 550f));
		bool flag = false;
		TrafficStatsGameLevel trafficStatsGameLevel = PhotonNetwork.networkingPeer.TrafficStatsGameLevel;
		long num = PhotonNetwork.networkingPeer.TrafficStatsElapsedMs / 1000;
		if (num == 0L)
		{
			num = 1L;
		}
		string text2 = $"<color=#00FF00>Out {trafficStatsGameLevel.TotalOutgoingMessageCount,4}</color> | <color=#FF0000>In {trafficStatsGameLevel.TotalIncomingMessageCount,4}</color> | <color=#ADD8E6>Sum {trafficStatsGameLevel.TotalMessageCount,4}</color>";
		string text3 = $"{num}sec average:";
		string text4 = $"<color=#00FF00>Out {trafficStatsGameLevel.TotalOutgoingMessageCount / num,4}</color> |  <color=#FF0000>In {trafficStatsGameLevel.TotalIncomingMessageCount / num,4}</color> | <color=#ADD8E6>Sum {trafficStatsGameLevel.TotalMessageCount / num,4}</color>";
		GUILayout.Label(text2, FontStyle);
		GUILayout.Label(text3, FontStyle);
		GUILayout.Label(text4, FontStyle);
		GUILayout.BeginHorizontal();
		statsOn = GUILayout.Toggle(statsOn, "stats on");
		if (GUILayout.Button("Reset"))
		{
			PhotonNetwork.networkingPeer.TrafficStatsReset();
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = true;
		}
		flag = GUILayout.Button("To Log");
		GUILayout.EndHorizontal();
		string empty = string.Empty;
		string empty2 = string.Empty;
		GUILayout.Box("Traffic Stats");
		empty = "<color=#FF0000>Incoming:</color> \n" + PhotonNetwork.networkingPeer.TrafficStatsIncoming.ToString();
		GUILayout.Label(empty, FontStyle);
		GUILayout.Label("<color=#0000FF>Server:</color>:" + FengGameManagerMKII.instance.EventListServer);
		empty2 = "<color=#00FF00>Outgoing:</color> \n" + PhotonNetwork.networkingPeer.TrafficStatsOutgoing.ToString();
		GUILayout.Label(empty2, FontStyle);
		string empty3 = string.Empty;
		GUILayout.Box("Health Stats");
		empty3 = string.Format("ping: {6}[+/-{7}]ms resent:{8} \n\nmax ms between\nsend: {0,4} \ndispatch: {1,4} \n\nlongest dispatch for: \nev({3}):{2,3}ms \nop({5}):{4,3}ms", trafficStatsGameLevel.LongestDeltaBetweenSending, trafficStatsGameLevel.LongestDeltaBetweenDispatching, trafficStatsGameLevel.LongestEventCallback, trafficStatsGameLevel.LongestEventCallbackCode, trafficStatsGameLevel.LongestOpResponseCallback, trafficStatsGameLevel.LongestOpResponseCallbackOpCode, PhotonNetwork.networkingPeer.RoundTripTime, PhotonNetwork.networkingPeer.RoundTripTimeVariance, PhotonNetwork.networkingPeer.ResentReliableCommands);
		GUILayout.Label(empty3);
		if (flag)
		{
			_ = $"{text2}\n{text3}\n{text4}\n{empty}\n{empty2}\n{empty3}";
		}
		GUILayout.EndArea();
		GUI.DragWindow();
		GUI.skin.toggle.font = font;
		GUI.skin.toggle.fontSize = fontSize;
		GUI.skin.button.font = font;
		GUI.skin.button.fontSize = fontSize;
	}

	private string CheckProps(PhotonPlayer player)
	{
		string text = string.Empty;
		foreach (object key in player.allProperties.Keys)
		{
			if (!player.allProperties.ContainsKey(key))
			{
				text = ((!((string)key == string.Empty)) ? string.Concat(text, key, " ") : (text + "string.Empty "));
			}
		}
		if (text == string.Empty)
		{
			return "No unusual properties";
		}
		return text;
	}

	private void OnGUI()
	{
		if (DetectIsVisible)
		{
			detectionrect = GUI.Window(641, detectionrect, CheckerWindow, string.Empty);
		}
	}

	private void Start()
	{
		DetectionTextures = new Texture2D[2]
		{
			(Texture2D)FengGameManagerMKII.someBundle.Load("Detection Left"),
			(Texture2D)FengGameManagerMKII.someBundle.Load("Detection Right")
		};
		choosedPlayer = PhotonNetwork.player;
	}

	private void Update()
	{
		if (FengGameManagerMKII.inputRC.isInputExtras(InputCodeRC.trafficpanel))
		{
			DetectIsVisible = !DetectIsVisible;
			if (DetectIsVisible)
			{
				Screen.showCursor = true;
				Screen.lockCursor = false;
			}
			else if (!DetectIsVisible && PhotonNetwork.inRoom)
			{
				Screen.showCursor = false;
				Screen.lockCursor = true;
			}
			else if (!DetectIsVisible && !PhotonNetwork.inRoom)
			{
				Screen.showCursor = true;
				Screen.lockCursor = false;
			}
		}
	}
}
