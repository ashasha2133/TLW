using System;
using System.Collections;
using UnityEngine;

public class PanelMultiSet : MonoBehaviour
{
	public GameObject label_ab;

	public GameObject label_BACK;

	public GameObject label_choose_map;

	public GameObject label_difficulty;

	public GameObject label_game_time;

	public GameObject label_hard;

	public GameObject label_max_player;

	public GameObject label_max_time;

	public GameObject label_normal;

	public GameObject label_server_name;

	public GameObject label_START;

	private Vector2 maps = Vector2.zero;

	private string CustomName;

	private int MaxPlayers;

	private static string MaxTime;

	private static string Password;

	private static string[] levelNames = new string[25]
	{
		"The Forest III", "The City", "Custom (No PT)", "Custom-Anarchy (No PT)", "Forest Paths", "Racing - Akina", "[S]Forest Survive(no crawler)", "[S]Forest Survive(no crawler no punk)", "The Forest IV  - LAVA", "The Forest V  - LAVA",
		"The City III", "The Forest", "The Forest II", "Custom", "Outside The Walls", "Annie", "Annie II", "Colossal Titan", "Colossal Titan II", "Trost",
		"Trost II", "Cave Fight", "House Fight", "[S]Tutorial", "[S]Battle training"
	};

	private string[] levelNameShown = new string[25]
	{
		"The Forest III", "The City", "Custom (No PT)", "Custom-Anarchy (No PT)", "Forest Paths", "Racing - Akina", "[S]Forest Survive(no crawler)", "Forest Survive (No Crawler/Punks)", "Forest IV - LAVA (ground)", "Forest V - LAVA (ground and trees)",
		"The City III", "The Forest", "The Forest II", "Custom", "Outside the Walls", "Annie (1 life)", "Annie II (respawn)", "Colossal Titan (1 life)", "Colossal Titan II (respawn)", "Trost (escort Eren)",
		"Trost II (respawn)", "Cave Fight", "House Fight", "[S]Tutorial", "[S]Battle training"
	};

	private static int MapSelected;

	private bool[] numbers = new bool[15];

	private AudioSource room_slide;

	private static string[] difficultyList = new string[3] { "normal", "hard", "abnormal" };

	private string[] difficultyListShown = new string[3] { "Normal", "Hard", "Abnormal" };

	private static int DifficultySelected;

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
	}

	private void Start()
	{
		room_slide = Camera.main.gameObject.AddComponent<AudioSource>();
		room_slide.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("room_slide");
		room_slide.volume = FengGameManagerMKII.InterfaceVolume;
		base.transform.Find("InputServerName").gameObject.SetActive(value: false);
		base.transform.Find("InputMaxPlayer").gameObject.SetActive(value: false);
		base.transform.Find("InputMaxTime").gameObject.SetActive(value: false);
		base.transform.Find("InputStartServerPWD").gameObject.SetActive(value: false);
		base.transform.Find("Labelpwd").gameObject.SetActive(value: false);
		base.transform.Find("CheckboxPublicServer").gameObject.SetActive(value: false);
		label_START.SetActive(value: false);
		label_BACK.SetActive(value: false);
		label_choose_map.SetActive(value: false);
		label_server_name.SetActive(value: false);
		label_max_player.SetActive(value: false);
		label_max_time.SetActive(value: false);
		label_game_time.SetActive(value: false);
		label_difficulty.SetActive(value: false);
		label_normal.SetActive(value: false);
		label_hard.SetActive(value: false);
		label_ab.SetActive(value: false);
		UIButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.SetActive(value: false);
		}
		CustomName = PlayerPrefs.GetString("servername", "Food for puppy");
		MaxPlayers = PlayerPrefs.GetInt("maxplayers", 10);
		MaxTime = "9999";
		Password = string.Empty;
		MapSelected = 0;
		LoadDifficulty();
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

	public void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), FengGameManagerMKII.textureBackgroundBlack);
		GUILayout.BeginArea(GUIHelpers.AlignRect(300f, 350f, GUIHelpers.Alignment.CENTER, -170f, 50f), GUI.skin.box);
		if (numbers[0])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Level:</color></b>");
		}
		if (numbers[1])
		{
			maps = GUILayout.BeginScrollView(maps, GUILayout.Height(180f));
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
		GUILayout.BeginArea(GUIHelpers.AlignRect(300f, 350f, GUIHelpers.Alignment.CENTER, 170f, 50f), GUI.skin.box);
		if (numbers[5])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Custom Server Name:</color></b>");
		}
		if (numbers[6])
		{
			CustomName = GUILayout.TextField(CustomName);
		}
		if (numbers[7])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Password:</color></b>");
			Password = GUILayout.TextField(Password);
			GUILayout.Space(5f);
		}
		if (numbers[8])
		{
			if (GUILayout.Button("Save"))
			{
				PlayerPrefs.SetString("servername", CustomName);
				PlayerPrefs.SetInt("maxplayers", MaxPlayers);
			}
			GUILayout.Space(5f);
		}
		if (numbers[9])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Max Players:</color></b>");
		}
		if (numbers[10])
		{
			MaxPlayers = int.Parse(GUILayout.TextField(MaxPlayers.ToString()));
			if (MaxPlayers > 255 || MaxPlayers < 1)
			{
				MaxPlayers = 10;
			}
		}
		if (numbers[11])
		{
			GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Max Time:</color></b>");
		}
		if (numbers[12])
		{
			MaxTime = GUILayout.TextField(MaxTime);
			if (int.TryParse(MaxTime, out var result))
			{
				if (result < 1)
				{
					MaxTime = "1";
					result = 1;
				}
			}
			else
			{
				MaxTime = "9999";
				result = 9999;
			}
		}
		GUILayout.Space(30f);
		if (numbers[13] && GUILayout.Button("<color=lime><b>Host</b></color>", GUILayout.Height(30f)))
		{
			PlayerPrefs.SetString("difficultyUsed", difficultyList[DifficultySelected]);
			PhotonNetwork.CreateRoom(CustomName + "`" + levelNames[MapSelected] + "`" + difficultyList[DifficultySelected] + "`" + MaxTime + "`" + "day" + "`" + (string.IsNullOrEmpty(Password) ? string.Empty : new SimpleAES().Encrypt(Password)) + "`" + UnityEngine.Random.Range(1200000000, 1300000000), isVisible: true, isOpen: true, Convert.ToInt32(MaxPlayers));
		}
		if (numbers[14] && GUILayout.Button("<color=red><b>Quit</b></color>", GUILayout.Height(30f)))
		{
			GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
			UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
			PhotonNetwork.Disconnect();
		}
		GUILayout.EndArea();
	}
}
