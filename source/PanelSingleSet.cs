using System.Collections;
using UnityEngine;

public class PanelSingleSet : MonoBehaviour
{
	public GameObject label_ab;

	public GameObject label_BACK;

	public GameObject label_camera;

	public GameObject label_choose_character;

	public GameObject label_choose_map;

	public GameObject label_difficulty;

	public GameObject label_hard;

	public GameObject label_normal;

	public GameObject label_original;

	public GameObject label_START;

	public GameObject label_tps;

	public GameObject label_wow;

	private Texture2D textureBackgroundBlack;

	private static string[] levelNames = new string[27]
	{
		"[S]Forest Survive(no crawler no punk)", "[S]Forest Survive(no crawler)", "[S]Forest", "[S]Racing - Akina", "[S]City", "[S]Tutorial", "[S]Battle training", "The City", "The City III", "The Forest",
		"Forest Paths", "The Forest II", "The Forest III", "The Forest IV  - LAVA", "The Forest V  - LAVA", "Custom", "Custom (No PT)", "Custom-Anarchy (No PT)", "Outside The Walls", "Annie",
		"Annie II", "Colossal Titan", "Colossal Titan II", "Trost", "Trost II", "Cave Fight", "House Fight"
	};

	private string[] levelNameShown = new string[27]
	{
		"Forest Survive (No Crawler/Punks)", "[S]Forest Survive (No Crawler)", "[S]Forest", "[S]Racing - Akina", "[S]City", "[S]Tutorial", "[S]Battle training", "The City", "The City III", "The Forest",
		"Forest Paths", "The Forest II", "The Forest III", "Forest IV - LAVA (ground)", "Forest V  - LAVA (ground and trees)", "Custom", "Custom (No PT)", "Custom-Anarchy (No PT)", "Outside the Walls", "Annie (1 life)",
		"Annie II (respawn)", "Colossal Titan (1 life)", "Colossal Titan II (respawn)", "Trost (escort Eren)", "Trost II (respawn)", "Cave Fight", "House Fight"
	};

	private static int MapSelected;

	private bool[] numbers = new bool[16];

	private Vector2 maps = Vector2.zero;

	private string[] dayLightList = new string[5] { "Day", "<size=12>DayLight</size>", "Dawn", "<size=10>DawnLightt</size>", "Night" };

	private int dayLightSelected;

	private static string[] difficultyList = new string[3] { "normal", "hard", "abnormal" };

	private string[] difficultyListShown = new string[3] { "Normal", "Hard", "Abnormal" };

	private static int DifficultySelected;

	private string[] Cameras = new string[3] { "TPS", "ORIGINAL", "NewTPS" };

	private int CameraSelected;

	public static int CostumeSelected = 0;

	private string[] costumeList = new string[3] { "Cos1", "Cos2", "Cos3" };

	private static int CharSelected;

	private string[] charactersList = new string[11]
	{
		"Set 1", "Set 2", "Set 3", "LEVI", "MIKASA", "PETRA", "SASHA", "ARMIN", "MARCO", "JEAN",
		"EREN"
	};

	private string[] charactersListShown = new string[11]
	{
		"Set 1", "Set 2", "Set 3", "Levi", "Mikasa", "Petra", "Sasha", "Armin", "Marco", "Jean",
		"Eren"
	};

	private Vector2 characters = Vector2.zero;

	private AudioSource room_slide;

	private void Start()
	{
		room_slide = Camera.main.gameObject.AddComponent<AudioSource>();
		room_slide.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("room_slide");
		MapSelected = 0;
		dayLightSelected = 0;
		LoadCamera();
		LoadCharacter();
		LoadCostume();
		LoadDifficulty();
		if (textureBackgroundBlack == null)
		{
			textureBackgroundBlack = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			textureBackgroundBlack.SetPixel(0, 0, Color.black);
			textureBackgroundBlack.Apply();
		}
		label_ab.SetActive(value: false);
		label_BACK.SetActive(value: false);
		label_camera.SetActive(value: false);
		label_choose_character.SetActive(value: false);
		label_choose_map.SetActive(value: false);
		label_difficulty.SetActive(value: false);
		label_hard.SetActive(value: false);
		label_normal.SetActive(value: false);
		label_original.SetActive(value: false);
		label_START.SetActive(value: false);
		label_tps.SetActive(value: false);
		label_wow.SetActive(value: false);
		UIButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.SetActive(value: false);
		}
		StartCoroutine(Giffy());
	}

	private void LoadDifficulty()
	{
		if (PlayerPrefs.HasKey("difficultyUsed"))
		{
			if (PlayerPrefs.GetString("difficultyUsed") == "normal")
			{
				DifficultySelected = 0;
			}
			else if (PlayerPrefs.GetString("difficultyUsed") == "hard")
			{
				DifficultySelected = 1;
			}
			else if (PlayerPrefs.GetString("difficultyUsed") == "abnormal")
			{
				DifficultySelected = 2;
			}
		}
	}

	private void LoadCamera()
	{
		if (PlayerPrefs.HasKey("cameraType"))
		{
			if (PlayerPrefs.GetString("cameraType").ToUpper() == "TPS")
			{
				CameraSelected = 0;
			}
			else if (PlayerPrefs.GetString("cameraType").ToUpper() == "ORIGINAL")
			{
				CameraSelected = 1;
			}
			else if (PlayerPrefs.GetString("cameraType").ToUpper() == "NEWTPS")
			{
				CameraSelected = 2;
			}
		}
	}

	private void LoadCostume()
	{
		if (PlayerPrefs.HasKey("costumeUsed"))
		{
			if (PlayerPrefs.GetString("costumeUsed") == "Cos1")
			{
				CostumeSelected = 0;
			}
			else if (PlayerPrefs.GetString("costumeUsed") == "Cos2")
			{
				CostumeSelected = 1;
			}
			else if (PlayerPrefs.GetString("costumeUsed") == "Cos3")
			{
				CostumeSelected = 2;
			}
		}
	}

	private void LoadCharacter()
	{
		if (PlayerPrefs.HasKey("characterUsed"))
		{
			string text = "";
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

	public void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), FengGameManagerMKII.textureBackgroundBlack);
		GUILayout.BeginArea(GUIHelpers.AlignRect(300f, 415f, GUIHelpers.Alignment.CENTER, -170f, 50f), GUI.skin.box);
		if (numbers[0])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Level:</color></b>");
		}
		if (numbers[1])
		{
			maps = GUILayout.BeginScrollView(maps, GUILayout.Height(220f));
			MapSelected = GUILayout.SelectionGrid(MapSelected, levelNameShown, 1, GUILayout.Height(700f), GUILayout.Width(258f));
			GUILayout.EndScrollView();
		}
		if (numbers[2])
		{
			GUILayout.Space(20f);
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Difficulty:</color></b>");
		}
		if (numbers[3])
		{
			DifficultySelected = GUILayout.SelectionGrid(DifficultySelected, difficultyListShown, 3, GUILayout.Height(30f));
		}
		if (numbers[4])
		{
			GUILayout.Label("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">Selected: <b>" + levelNames[MapSelected] + " - " + difficultyList[DifficultySelected] + "</b></color>");
		}
		GUILayout.EndArea();
		GUILayout.BeginArea(GUIHelpers.AlignRect(300f, 415f, GUIHelpers.Alignment.CENTER, 170f, 50f), GUI.skin.box);
		if (numbers[5])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Character:</color></b>");
		}
		if (numbers[6])
		{
			characters = GUILayout.BeginScrollView(characters, GUILayout.Height(120f));
			CharSelected = GUILayout.SelectionGrid(CharSelected, charactersListShown, 1, GUILayout.Height(300f), GUILayout.Width(258f));
			GUILayout.EndScrollView();
		}
		if (numbers[7])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + "><b>Costume</b></color></b>");
		}
		if (numbers[8])
		{
			CostumeSelected = GUILayout.SelectionGrid(CostumeSelected, costumeList, 3, GUILayout.Height(30f));
		}
		if (numbers[9])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Camera:</color></b>");
		}
		if (numbers[10])
		{
			CameraSelected = GUILayout.SelectionGrid(CameraSelected, Cameras, 3, GUILayout.Height(30f));
			GUILayout.Space(5f);
		}
		if (numbers[11])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">DayLight:</color></b>");
		}
		if (numbers[12])
		{
			dayLightSelected = GUILayout.SelectionGrid(dayLightSelected, dayLightList, 5, GUILayout.Height(30f));
			GUILayout.Space(5f);
		}
		if (numbers[13] && GUILayout.Button("<color=lime><b>Host</b></color>", GUILayout.Height(30f)))
		{
			string obj = levelNames[MapSelected];
			IN_GAME_MAIN_CAMERA.difficulty = DifficultySelected;
			if (obj == "trainning_0")
			{
				IN_GAME_MAIN_CAMERA.difficulty = -1;
			}
			if (dayLightSelected == 0)
			{
				IN_GAME_MAIN_CAMERA.dayLight = DayLight.Day;
			}
			else if (dayLightSelected == 1)
			{
				IN_GAME_MAIN_CAMERA.dayLight = DayLight.DayLight;
			}
			else if (dayLightSelected == 2)
			{
				IN_GAME_MAIN_CAMERA.dayLight = DayLight.Dawn;
			}
			else if (dayLightSelected == 3)
			{
				IN_GAME_MAIN_CAMERA.dayLight = DayLight.DawnLight;
			}
			else if (dayLightSelected == 4)
			{
				IN_GAME_MAIN_CAMERA.dayLight = DayLight.Night;
			}
			if (CameraSelected == 0)
			{
				IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.TPS;
				PlayerPrefs.SetString("cameraType", Cameras[CameraSelected]);
			}
			else if (CameraSelected == 1)
			{
				IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.ORIGINAL;
				PlayerPrefs.SetString("cameraType", Cameras[CameraSelected]);
			}
			else if (CameraSelected == 2)
			{
				IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.NewTPS;
				PlayerPrefs.SetString("cameraType", Cameras[CameraSelected]);
			}
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.SINGLE;
			IN_GAME_MAIN_CAMERA.singleCharacter = charactersList[CharSelected].ToUpper();
			FengGameManagerMKII.costumeSet = CostumeSelected + 1;
			PlayerPrefs.SetString("costumeUsed", costumeList[CostumeSelected]);
			PlayerPrefs.SetString("characterUsed", charactersList[CharSelected]);
			PlayerPrefs.SetString("difficultyUsed", difficultyList[DifficultySelected]);
			if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
			{
				Screen.lockCursor = true;
			}
			Screen.showCursor = false;
			FengGameManagerMKII.level = obj;
			Application.LoadLevel(LevelInfo.getInfo(obj).mapName);
		}
		if (numbers[14] && GUILayout.Button("<color=red><b>Quit</b></color>", GUILayout.Height(30f)))
		{
			GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
			Object.Destroy(GameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
			PhotonNetwork.Disconnect();
		}
		GUILayout.EndArea();
	}

	private IEnumerator Giffy()
	{
		numbers[0] = true;
		if (!room_slide.isPlaying && AudioListener.volume > 0f && FengGameManagerMKII.InterfaceVolume > 0f)
		{
			room_slide.Play();
		}
		yield return new WaitForSeconds(0.04f);
		numbers[1] = true;
		yield return new WaitForSeconds(0.04f);
		numbers[2] = true;
		yield return new WaitForSeconds(0.04f);
		numbers[3] = true;
		yield return new WaitForSeconds(0.04f);
		numbers[4] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[5] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[6] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[7] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[8] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[9] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[10] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[11] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[12] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[13] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[14] = true;
		yield return new WaitForSeconds(0.02f);
		numbers[15] = true;
	}
}
