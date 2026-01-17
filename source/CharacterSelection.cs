using System;
using BRM;
using ExitGames.Client.Photon;
using UnityEngine;

internal class CharacterSelection : MonoBehaviour
{
	public static bool characterSelectionIsVisible;

	private Rect charaSelectionRect = GUIHelpers.AlignRect(500f, (GameSettings.bombMode > 0) ? 525 : 385, GUIHelpers.Alignment.CENTER, 0f, 0f);

	private static int CharSelected;

	private static string[] charactersList = new string[11]
	{
		"Set 1", "Set 2", "Set 3", "LEVI", "MIKASA", "PETRA", "SASHA", "ARMIN", "MARCO", "JEAN",
		"EREN"
	};

	private string[] charactersListShown = new string[11]
	{
		"Set 1", "Set 2", "Set 3", "Levi", "Mikasa", "Petra", "Sasha", "Armin", "Marco", "Jean",
		"Eren"
	};

	private static int CostumeSelected;

	private static string[] costumeList = new string[3] { "Cos1", "Cos2", "Cos3" };

	public static int CameraSelected;

	private string[] cameraList = new string[3] { "TPS", "ORIGINAL", "NewTPS" };

	private string[] cameraListShown = new string[3] { "TPS", "Original", "New TPS" };

	private static GUIStyle currentStyle;

	private HeroStat stat;

	private void Start()
	{
	}

	public static void AutoLoadCamera()
	{
		if (!characterSelectionIsVisible)
		{
			return;
		}
		if (PlayerPrefs.HasKey("cameraType"))
		{
			string text = "";
			switch (PlayerPrefs.GetString("cameraType"))
			{
			case "TPS":
				CameraSelected = 0;
				break;
			case "ORIGINAL":
				CameraSelected = 1;
				break;
			case "NewTPS":
				CameraSelected = 2;
				break;
			}
		}
		if (PlayerPrefs.HasKey("costumeUsed"))
		{
			string text2 = "";
			switch (PlayerPrefs.GetString("costumeUsed"))
			{
			case "Cos1":
				CostumeSelected = 0;
				break;
			case "Cos2":
				CostumeSelected = 1;
				break;
			case "Cos3":
				CostumeSelected = 2;
				break;
			}
		}
		if (PlayerPrefs.HasKey("characterUsed"))
		{
			string text3 = "";
			switch (PlayerPrefs.GetString("characterUsed"))
			{
			case "Set 1":
				CharSelected = 0;
				break;
			case "Set 2":
				CharSelected = 1;
				break;
			case "Set 3":
				CharSelected = 2;
				break;
			case "LEVI":
				CharSelected = 3;
				break;
			case "MIKASA":
				CharSelected = 4;
				break;
			case "PETRA":
				CharSelected = 5;
				break;
			case "SASHA":
				CharSelected = 6;
				break;
			case "ARMIN":
				CharSelected = 7;
				break;
			case "MARCO":
				CharSelected = 8;
				break;
			case "JEAN":
				CharSelected = 9;
				break;
			case "EREN":
				CharSelected = 10;
				break;
			default:
				CharSelected = 4;
				break;
			}
		}
	}

	private void OnGUI()
	{
		if (characterSelectionIsVisible)
		{
			charaSelectionRect = GUI.Window(223, GUIHelpers.AlignRect(500f, (GameSettings.bombMode > 0) ? 525 : 385, GUIHelpers.Alignment.CENTER, 0f, 0f), Window, string.Empty);
		}
	}

	public static float GUILAYOUTMyHorizontalSliderOnePoint(float value, float leftValue, float rightValue, GUILayoutOption[] options)
	{
		return GUILayout.HorizontalSlider(Convert.ToSingle(Math.Round(value * 1f, MidpointRounding.AwayFromZero) / 1.0), leftValue, rightValue, options);
	}

	private void Window(int ID)
	{
		Style();
		GUI.DrawTexture(new Rect(0f, 0f, 500f, (GameSettings.bombMode > 0) ? 525 : 385), FengGameManagerMKII.textureBackgroundBlack);
		GUILayout.BeginArea(new Rect(270f, 0f, 360f, 500f));
		GUILayout.Space(102f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(80f);
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>Stats</b></color></b>");
		GUILayout.EndHorizontal();
		GUILayout.Label(HeroStats(charactersList[CharSelected]) ?? "", "Button", GUILayout.Width(200f));
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(75f);
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>Costume</b></color></b>");
		GUILayout.EndHorizontal();
		CostumeSelected = GUILayout.SelectionGrid(CostumeSelected, costumeList, 3, GUILayout.Width(200f), GUILayout.Height(30f));
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(25f, 0f, 360f, 500f));
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(80f);
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>Camera</b></color></b>");
		GUILayout.EndHorizontal();
		CameraSelected = GUILayout.SelectionGrid(CameraSelected, cameraListShown, 3, GUILayout.Width(200f), GUILayout.Height(30f));
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(75f);
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>Character</b></color></b>");
		GUILayout.EndHorizontal();
		CharSelected = GUILayout.SelectionGrid(CharSelected, charactersListShown, 3, GUILayout.Width(200f), GUILayout.Height(150f));
		if (GameSettings.bombMode > 0)
		{
			GUILayout.Space(25f);
			float num = Convert.ToSingle(FengGameManagerMKII.settings[250]);
			float num2 = Convert.ToSingle(FengGameManagerMKII.settings[251]);
			float num3 = Convert.ToSingle(FengGameManagerMKII.settings[252]);
			float num4 = Convert.ToSingle(FengGameManagerMKII.settings[253]);
			float bombRadius = BombUtil.GetBombRadius(num, 5.4f, 7.4f, 7f);
			float bombRange = BombUtil.GetBombRange(num2, 0f, 4f, 7f);
			float bombSpeed = BombUtil.GetBombSpeed(num3, 3f, 10.5f, 10.5f);
			float bombCooldown = BombUtil.GetBombCooldown(num4, 4f, 7f, 7f);
			float oldRadiusCost = BombUtil.GetOldRadiusCost(bombRadius);
			float oldRangeCost = BombUtil.GetOldRangeCost(bombRange);
			float oldSpeedCost = BombUtil.GetOldSpeedCost(bombSpeed);
			float oldCooldownCost = BombUtil.GetOldCooldownCost(bombCooldown);
			string text = bombRadius.ToString("0.##");
			string text2 = bombRange.ToString("0.##");
			string text3 = (bombSpeed / 100f).ToString("0.##");
			string text4 = bombCooldown.ToString("0.##");
			text = string.Format("<color=#{0}>{1} </color>({2}) ({3}u)", FengGameManagerMKII.finalhex(), num, oldRadiusCost.ToString("0.##"), text);
			text2 = string.Format("<color=#{0}>{1} </color>({2}) ({3}u)", FengGameManagerMKII.finalhex(), num2, oldRangeCost.ToString("0.##"), text2);
			text3 = string.Format("<color=#{0}>{1} </color>({2}) ({3}k)", FengGameManagerMKII.finalhex(), num3, oldSpeedCost.ToString("0.##"), text3);
			text4 = string.Format("<color=#{0}>{1} </color>({2}) ({3}s)", FengGameManagerMKII.finalhex(), num4, oldCooldownCost.ToString("0.##"), text4);
			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#" + FengGameManagerMKII.finalhex() + ">Bomb Radius:</color> " + text, GUILayout.Width(170f), GUILayout.Height(22f));
			GUILayout.Space(5f);
			FengGameManagerMKII.settings[250] = GUILAYOUTMyHorizontalSliderOnePoint((float)FengGameManagerMKII.settings[250], 0f, 10f, new GUILayoutOption[2]
			{
				GUILayout.Width(115f),
				GUILayout.Height(20f)
			});
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#" + FengGameManagerMKII.finalhex() + ">Bomb Range:</color> " + text2, GUILayout.Width(170f), GUILayout.Height(22f));
			GUILayout.Space(5f);
			FengGameManagerMKII.settings[251] = GUILAYOUTMyHorizontalSliderOnePoint((float)FengGameManagerMKII.settings[251], 0f, 10f, new GUILayoutOption[2]
			{
				GUILayout.Width(115f),
				GUILayout.Height(20f)
			});
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#" + FengGameManagerMKII.finalhex() + ">Bomb Speed:</color> " + text3, GUILayout.Width(170f), GUILayout.Height(22f));
			GUILayout.Space(5f);
			FengGameManagerMKII.settings[252] = GUILAYOUTMyHorizontalSliderOnePoint((float)FengGameManagerMKII.settings[252], 0f, 10f, new GUILayoutOption[2]
			{
				GUILayout.Width(115f),
				GUILayout.Height(20f)
			});
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#" + FengGameManagerMKII.finalhex() + ">Bomb CD:</color> " + text4, GUILayout.Width(170f), GUILayout.Height(22f));
			GUILayout.Space(5f);
			FengGameManagerMKII.settings[253] = GUILAYOUTMyHorizontalSliderOnePoint((float)FengGameManagerMKII.settings[253], 0f, 10f, new GUILayoutOption[2]
			{
				GUILayout.Width(115f),
				GUILayout.Height(20f)
			});
			GUILayout.EndHorizontal();
			float num5 = 20f - (float)FengGameManagerMKII.settings[250] - (float)FengGameManagerMKII.settings[251] - (float)FengGameManagerMKII.settings[252] - (float)FengGameManagerMKII.settings[253];
			GUILayout.Label("<color=#" + FengGameManagerMKII.finalhex() + ">Unused Points:</color> " + num5);
		}
		GUILayout.EndArea();
		GUILayout.Space(25f);
		GUILayout.BeginArea(new Rect(25f, (GameSettings.bombMode > 0) ? 450 : 310, 450f, 70f));
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("<color=red><b>Human</b></color>", GUILayout.Width(200f), GUILayout.Height(60f)))
        {
            int num = 610, num2 = 290, num3 = 25;
            if (CharSelected == 0)
			{
                FengGameManagerMKII.settings[133] = 1;
                FengGameManagerMKII.settings[134] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 0f, 230f, 20f), (string)FengGameManagerMKII.settings[134]);
                FengGameManagerMKII.settings[135] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 1f, 230f, 20f), (string)FengGameManagerMKII.settings[135]);
                FengGameManagerMKII.settings[136] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 2f, 230f, 20f), (string)FengGameManagerMKII.settings[136]);
                FengGameManagerMKII.settings[137] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 3f, 230f, 20f), (string)FengGameManagerMKII.settings[137]);
                FengGameManagerMKII.settings[138] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 4f, 230f, 20f), (string)FengGameManagerMKII.settings[138]);
                FengGameManagerMKII.settings[139] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 5f, 230f, 20f), (string)FengGameManagerMKII.settings[139]);
                FengGameManagerMKII.settings[145] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 6f, 230f, 20f), (string)FengGameManagerMKII.settings[145]);
                FengGameManagerMKII.settings[140] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 7f, 230f, 20f), (string)FengGameManagerMKII.settings[140]);
                FengGameManagerMKII.settings[141] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 8f, 230f, 20f), (string)FengGameManagerMKII.settings[141]);
                FengGameManagerMKII.settings[142] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 9f, 230f, 20f), (string)FengGameManagerMKII.settings[142]);
                FengGameManagerMKII.settings[143] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 10f, 230f, 20f), (string)FengGameManagerMKII.settings[143]);
                FengGameManagerMKII.settings[144] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 11f, 230f, 20f), (string)FengGameManagerMKII.settings[144]);
                FengGameManagerMKII.settings[146] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 12f, 230f, 20f), (string)FengGameManagerMKII.settings[146]);
                FengGameManagerMKII.settings[318] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 13f, 230f, 20f), (string)FengGameManagerMKII.settings[318]);
                FengGameManagerMKII.settings[319] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 14f, 230f, 20f), (string)FengGameManagerMKII.settings[319]);
            }
			else if (CharSelected == 3)
			{
                FengGameManagerMKII.settings[133] = 0;
                FengGameManagerMKII.settings[3] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 0f, 230f, 20f), (string)FengGameManagerMKII.settings[3]);
                FengGameManagerMKII.settings[4] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 1f, 230f, 20f), (string)FengGameManagerMKII.settings[4]);
                FengGameManagerMKII.settings[5] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 2f, 230f, 20f), (string)FengGameManagerMKII.settings[5]);
                FengGameManagerMKII.settings[6] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 3f, 230f, 20f), (string)FengGameManagerMKII.settings[6]);
                FengGameManagerMKII.settings[7] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 4f, 230f, 20f), (string)FengGameManagerMKII.settings[7]);
                FengGameManagerMKII.settings[8] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 5f, 230f, 20f), (string)FengGameManagerMKII.settings[8]);
                FengGameManagerMKII.settings[14] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 6f, 230f, 20f), (string)FengGameManagerMKII.settings[14]);
                FengGameManagerMKII.settings[9] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 7f, 230f, 20f), (string)FengGameManagerMKII.settings[9]);
                FengGameManagerMKII.settings[10] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 8f, 230f, 20f), (string)FengGameManagerMKII.settings[10]);
                FengGameManagerMKII.settings[11] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 9f, 230f, 20f), (string)FengGameManagerMKII.settings[11]);
                FengGameManagerMKII.settings[12] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 10f, 230f, 20f), (string)FengGameManagerMKII.settings[12]);
                FengGameManagerMKII.settings[13] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 11f, 230f, 20f), (string)FengGameManagerMKII.settings[13]);
                FengGameManagerMKII.settings[94] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 12f, 230f, 20f), (string)FengGameManagerMKII.settings[94]);
                FengGameManagerMKII.settings[316] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 13f, 230f, 20f), (string)FengGameManagerMKII.settings[316]);
                FengGameManagerMKII.settings[317] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 14f, 230f, 20f), (string)FengGameManagerMKII.settings[317]);
            }
				FengGameManagerMKII.costumeSet = CostumeSelected + 1;
			SelectCamera();
			ChoosePlayer(charactersList[CharSelected]);
        }
		GUILayout.Space(40f);
		if (GUILayout.Button("<color=red><b>Titan</b></color>", GUILayout.Width(200f), GUILayout.Height(60f)))
		{
			if (GameSettings.bombMode == 0)
			{
				ChooseTitan();
			}
			else
			{
				FleurConsole.instance.Log("Can't spawn as titan in bomb", FleurConsole.DebugType.LOG);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
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

	private void Style()
	{
		if (currentStyle == null)
		{
			currentStyle = new GUIStyle(GUI.skin.label);
			currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.2f));
		}
	}

	private string HeroStats(string selection)
	{
		switch (selection)
		{
		case "Set 1":
		case "Set 2":
		case "Set 3":
		{
			HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(selection.ToUpper());
			if (heroCostume == null)
			{
				stat = new HeroStat();
			}
			else
			{
				stat = heroCostume.stat;
			}
			break;
		}
		default:
			stat = HeroStat.getInfo(charactersList[CharSelected]);
			break;
		}
		return "<color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>SPD:</b> </color>" + stat.SPD + "     <color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>ACL:</b> </color>" + stat.ACL + "\n<color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>GAS:</b> </color>" + stat.GAS + "     <color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>BLA:</b> </color>" + stat.BLA;
	}

	private void ChooseTitan()
	{
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			string text = "AHSS";
			NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], state: true);
			FengGameManagerMKII.instance.needChooseSide = false;
			if (!PhotonNetwork.isMasterClient && FengGameManagerMKII.instance.roundTime > 60f)
			{
				FengGameManagerMKII.instance.NOTSpawnPlayer(text);
				FengGameManagerMKII.instance.photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
			}
			else
			{
				FengGameManagerMKII.instance.SpawnPlayer(text, "playerRespawn2");
			}
			NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
			NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
			characterSelectionIsVisible = false;
			IN_GAME_MAIN_CAMERA.usingTitan = false;
			IN_GAME_MAIN_CAMERA.mainCamera.setHUDposition();
			Hashtable customProperties = new Hashtable { { "character", text } };
			PhotonNetwork.player.SetCustomProperties(customProperties);
		}
		else
		{
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
			{
				FengGameManagerMKII.instance.checkpoint = CacheGameObject.Find("PVPchkPtT");
			}
			string id = "RANDOM";
			NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], state: true);
			if ((!PhotonNetwork.isMasterClient && FengGameManagerMKII.instance.roundTime > 60f) || FengGameManagerMKII.justSuicide)
			{
				FengGameManagerMKII.justSuicide = false;
				FengGameManagerMKII.instance.NOTSpawnNonAITitan(id);
			}
			else
			{
				FengGameManagerMKII.instance.SpawnNonAITitan2(id);
			}
			FengGameManagerMKII.instance.needChooseSide = false;
			NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
			NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
			characterSelectionIsVisible = false;
			IN_GAME_MAIN_CAMERA.usingTitan = true;
			IN_GAME_MAIN_CAMERA.mainCamera.setHUDposition();
		}
	}

	private void SelectCamera()
	{
		if (CameraSelected == 0 || cameraList[CameraSelected] == "TPS")
		{
			IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.TPS;
		}
		if (CameraSelected == 1 || cameraList[CameraSelected] == "ORIGINAL")
		{
			IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.ORIGINAL;
		}
		if (CameraSelected == 2 || cameraList[CameraSelected] == "NewTPS")
		{
			IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.NewTPS;
		}
		PlayerPrefs.SetString("cameraType", cameraList[CameraSelected]);
	}

	public static bool isPlayerAllDead2()
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (RCextensions.returnIntFromObject(photonPlayer.customProperties["isTitan"]) == 1)
			{
				num++;
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties["dead"]))
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public static void ChoosePlayer(string selection)
	{
		NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], state: true);
		FengGameManagerMKII.instance.needChooseSide = false;
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			FengGameManagerMKII.instance.checkpoint = CacheGameObject.Find("PVPchkPtH");
		}
		if (!PhotonNetwork.isMasterClient && FengGameManagerMKII.instance.roundTime > 60f)
		{
			if (!isPlayerAllDead2())
			{
				FengGameManagerMKII.instance.SpawnPlayer(selection);
			}
			else
			{
				FengGameManagerMKII.instance.SpawnPlayer(selection);
				FengGameManagerMKII.instance.photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.TROST || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			if (isPlayerAllDead2())
			{
				FengGameManagerMKII.instance.SpawnPlayer(selection);
				FengGameManagerMKII.instance.photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
			}
			else
			{
				FengGameManagerMKII.instance.SpawnPlayer(selection);
			}
		}
		else
		{
			FengGameManagerMKII.instance.SpawnPlayer(selection);
		}
		UIReferArray uIReferArray = CacheGameObject.Find<UIReferArray>("UI_IN_GAME");
		NGUITools.SetActive(uIReferArray.panels[1], state: false);
		NGUITools.SetActive(uIReferArray.panels[2], state: false);
		characterSelectionIsVisible = false;
		IN_GAME_MAIN_CAMERA.usingTitan = false;
		IN_GAME_MAIN_CAMERA.mainCamera.setHUDposition();
		Hashtable customProperties = new Hashtable { { "character", selection } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		PlayerPrefs.SetString("costumeUsed", costumeList[CostumeSelected]);
		PlayerPrefs.SetString("characterUsed", charactersList[CharSelected]);
	}
}
