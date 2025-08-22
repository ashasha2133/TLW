using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacterManager : MonoBehaviour
{
	private int capeId;

	private int[] capeOption;

	public GameObject character;

	private int costumeId = 1;

	private HeroCostume[] costumeOption;

	private HeroCostume currentCostume;

	private string currentSlot;

	private int divisionId;

	private DIVISION[] divisionOption;

	private int eyeId;

	private int[] eyeOption;

	private int faceId;

	private int[] faceOption;

	private int glassId;

	private int[] glassOption;

	public GameObject hairR;

	public GameObject hairB;

	public GameObject hairG;

	private int hairId;

	private int[] hairOption;

	public GameObject labelACL;

	public GameObject labelBLA;

	public GameObject labelCape;

	public GameObject labelCostume;

	public GameObject labelDivision;

	public GameObject labelEye;

	public GameObject labelFace;

	public GameObject labelGAS;

	public GameObject labelGlass;

	public GameObject labelHair;

	public GameObject labelPOINT;

	public GameObject labelPreset;

	public GameObject labelSex;

	public GameObject labelSKILL;

	public GameObject labelSkin;

	public GameObject labelSPD;

	private int presetId;

	private HERO_SETUP setup;

	private int sexId;

	private SEX[] sexOption;

	private int skillId;

	private string[] skillOption;

	private int skinId;

	private int[] skinOption;

	internal Vector2 SkinScroll;

	internal Vector2 SkinSetScroll;

	private Vector2 AnimationScroll = Vector2.zero;

	private Vector2 CharactersScroll = Vector2.zero;

	public Rect SkinsRect = GUIHelpers.AlignRect(343f, 300f, GUIHelpers.Alignment.BOTTOMRIGHT, 0f, 0f);

	private Rect StatsRect = GUIHelpers.AlignRect(220f, 340f, GUIHelpers.Alignment.TOPRIGHT, 0f, 0f);

	private Rect ComponentsRect = GUIHelpers.AlignRect(220f, 480f, GUIHelpers.Alignment.TOPLEFT, 0f, 0f);

	private bool BrightColors = true;

	private string BrightColorsStr = "Normal Colors";

	private static int CharacterSetOption;

	public static string[] AnimationsList = new string[9] { "stand", "supply", "dodge", "salute", "sit", "ride", "dance", "jump", "eat" };

	private void Awake()
	{
		List<string> list = new List<string> { "someshit" };
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			foreach (string item in list)
			{
				if (gameObject.name.Contains(item) || gameObject.name.Contains("Save") || gameObject.name.Contains("Load") || gameObject.name.Contains("Quit") || gameObject.name == "Button" || gameObject.name == "Label" || gameObject.name.Contains("opup"))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
		hairR.SetActive(value: false);
		hairG.SetActive(value: false);
		hairB.SetActive(value: false);
	}

	public void SkinsList(int id)
	{
		SkinScroll = GUILayout.BeginScrollView(SkinScroll);
		if ((int)FengGameManagerMKII.settings[133] == 0)
		{
			GUILayout.Label("<color=#FFFFFF>Hair:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[4] = GUILayout.TextField((string)FengGameManagerMKII.settings[4], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Glass:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[6] = GUILayout.TextField((string)FengGameManagerMKII.settings[6], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Face:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[7] = GUILayout.TextField((string)FengGameManagerMKII.settings[7], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Skin:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[8] = GUILayout.TextField((string)FengGameManagerMKII.settings[8], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Costume:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[9] = GUILayout.TextField((string)FengGameManagerMKII.settings[9], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Cape:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[10] = GUILayout.TextField((string)FengGameManagerMKII.settings[10], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Hoodie:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[14] = GUILayout.TextField((string)FengGameManagerMKII.settings[14], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Left Blade:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[11] = GUILayout.TextField((string)FengGameManagerMKII.settings[11], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Right Blade:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[12] = GUILayout.TextField((string)FengGameManagerMKII.settings[12], GUILayout.Width(290f));
		}
		else if ((int)FengGameManagerMKII.settings[133] == 1)
		{
			GUILayout.Label("<color=#FFFFFF>Hair:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[135] = GUILayout.TextField((string)FengGameManagerMKII.settings[135], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Glass:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[137] = GUILayout.TextField((string)FengGameManagerMKII.settings[137], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Face:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[138] = GUILayout.TextField((string)FengGameManagerMKII.settings[138], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Skin:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[139] = GUILayout.TextField((string)FengGameManagerMKII.settings[139], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Costume:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[140] = GUILayout.TextField((string)FengGameManagerMKII.settings[140], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Cape:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[141] = GUILayout.TextField((string)FengGameManagerMKII.settings[141], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Hoodie:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[145] = GUILayout.TextField((string)FengGameManagerMKII.settings[145], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Left Blade:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[142] = GUILayout.TextField((string)FengGameManagerMKII.settings[142], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Right Blade:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[143] = GUILayout.TextField((string)FengGameManagerMKII.settings[143], GUILayout.Width(290f));
		}
		else if ((int)FengGameManagerMKII.settings[133] == 2)
		{
			GUILayout.Label("<color=#FFFFFF>Hair:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[148] = GUILayout.TextField((string)FengGameManagerMKII.settings[148], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Glass:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[150] = GUILayout.TextField((string)FengGameManagerMKII.settings[150], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Face:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[151] = GUILayout.TextField((string)FengGameManagerMKII.settings[151], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Skin:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[152] = GUILayout.TextField((string)FengGameManagerMKII.settings[152], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Costume:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[153] = GUILayout.TextField((string)FengGameManagerMKII.settings[153], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Cape:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[154] = GUILayout.TextField((string)FengGameManagerMKII.settings[154], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Hoodie:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[158] = GUILayout.TextField((string)FengGameManagerMKII.settings[158], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Left Blade:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[155] = GUILayout.TextField((string)FengGameManagerMKII.settings[155], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Right Blade:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[156] = GUILayout.TextField((string)FengGameManagerMKII.settings[156], GUILayout.Width(290f));
		}
		else if ((int)FengGameManagerMKII.settings[133] == 3)
		{
			GUILayout.Label("<color=#FFFFFF>Hair:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[304] = GUILayout.TextField((string)FengGameManagerMKII.settings[304], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Glass:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[306] = GUILayout.TextField((string)FengGameManagerMKII.settings[306], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Face:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[307] = GUILayout.TextField((string)FengGameManagerMKII.settings[307], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Skin:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[308] = GUILayout.TextField((string)FengGameManagerMKII.settings[308], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Costume:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[309] = GUILayout.TextField((string)FengGameManagerMKII.settings[309], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Cape:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[310] = GUILayout.TextField((string)FengGameManagerMKII.settings[310], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Hoodie:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[314] = GUILayout.TextField((string)FengGameManagerMKII.settings[314], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Left Blade:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[311] = GUILayout.TextField((string)FengGameManagerMKII.settings[311], GUILayout.Width(290f));
			GUILayout.Label("<color=#FFFFFF>Right Blade:</color>", GUILayout.Width(290f));
			FengGameManagerMKII.settings[312] = GUILayout.TextField((string)FengGameManagerMKII.settings[312], GUILayout.Width(290f));
		}
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("save", GUILayout.Width(143f)))
		{
			if ((int)FengGameManagerMKII.settings[133] == 0)
			{
				PlayerPrefs.SetString("hair", (string)FengGameManagerMKII.settings[4]);
				PlayerPrefs.SetString("eye", (string)FengGameManagerMKII.settings[5]);
				PlayerPrefs.SetString("glass", (string)FengGameManagerMKII.settings[6]);
				PlayerPrefs.SetString("face", (string)FengGameManagerMKII.settings[7]);
				PlayerPrefs.SetString("skin", (string)FengGameManagerMKII.settings[8]);
				PlayerPrefs.SetString("costume", (string)FengGameManagerMKII.settings[9]);
				PlayerPrefs.SetString("logo", (string)FengGameManagerMKII.settings[10]);
				PlayerPrefs.SetString("bladel", (string)FengGameManagerMKII.settings[11]);
				PlayerPrefs.SetString("blader", (string)FengGameManagerMKII.settings[12]);
				PlayerPrefs.SetString("hoodie", (string)FengGameManagerMKII.settings[14]);
			}
			else if ((int)FengGameManagerMKII.settings[133] == 1)
			{
				PlayerPrefs.SetString("hair2", (string)FengGameManagerMKII.settings[135]);
				PlayerPrefs.SetString("eye2", (string)FengGameManagerMKII.settings[136]);
				PlayerPrefs.SetString("glass2", (string)FengGameManagerMKII.settings[137]);
				PlayerPrefs.SetString("face2", (string)FengGameManagerMKII.settings[138]);
				PlayerPrefs.SetString("skin2", (string)FengGameManagerMKII.settings[139]);
				PlayerPrefs.SetString("costume2", (string)FengGameManagerMKII.settings[140]);
				PlayerPrefs.SetString("logo2", (string)FengGameManagerMKII.settings[141]);
				PlayerPrefs.SetString("bladel2", (string)FengGameManagerMKII.settings[142]);
				PlayerPrefs.SetString("blader2", (string)FengGameManagerMKII.settings[143]);
				PlayerPrefs.SetString("hoodie2", (string)FengGameManagerMKII.settings[145]);
			}
			else if ((int)FengGameManagerMKII.settings[133] == 2)
			{
				PlayerPrefs.SetString("hair3", (string)FengGameManagerMKII.settings[148]);
				PlayerPrefs.SetString("eye3", (string)FengGameManagerMKII.settings[149]);
				PlayerPrefs.SetString("glass3", (string)FengGameManagerMKII.settings[150]);
				PlayerPrefs.SetString("face3", (string)FengGameManagerMKII.settings[151]);
				PlayerPrefs.SetString("skin3", (string)FengGameManagerMKII.settings[152]);
				PlayerPrefs.SetString("costume3", (string)FengGameManagerMKII.settings[153]);
				PlayerPrefs.SetString("logo3", (string)FengGameManagerMKII.settings[154]);
				PlayerPrefs.SetString("bladel3", (string)FengGameManagerMKII.settings[155]);
				PlayerPrefs.SetString("blader3", (string)FengGameManagerMKII.settings[156]);
				PlayerPrefs.SetString("hoodie3", (string)FengGameManagerMKII.settings[158]);
			}
			else if ((int)FengGameManagerMKII.settings[133] == 3)
			{
				PlayerPrefs.SetString("hair4", (string)FengGameManagerMKII.settings[304]);
				PlayerPrefs.SetString("eye4", (string)FengGameManagerMKII.settings[305]);
				PlayerPrefs.SetString("glass4", (string)FengGameManagerMKII.settings[306]);
				PlayerPrefs.SetString("face4", (string)FengGameManagerMKII.settings[307]);
				PlayerPrefs.SetString("skin4", (string)FengGameManagerMKII.settings[308]);
				PlayerPrefs.SetString("costume4", (string)FengGameManagerMKII.settings[309]);
				PlayerPrefs.SetString("logo4", (string)FengGameManagerMKII.settings[310]);
				PlayerPrefs.SetString("bladel4", (string)FengGameManagerMKII.settings[311]);
				PlayerPrefs.SetString("blader4", (string)FengGameManagerMKII.settings[312]);
				PlayerPrefs.SetString("hoodie4", (string)FengGameManagerMKII.settings[314]);
			}
		}
		if (GUILayout.Button("load", GUILayout.Width(143f)))
		{
			if ((int)FengGameManagerMKII.settings[133] == 0)
			{
				FengGameManagerMKII.settings[4] = PlayerPrefs.GetString("hair", string.Empty);
				FengGameManagerMKII.settings[5] = PlayerPrefs.GetString("eye", string.Empty);
				FengGameManagerMKII.settings[6] = PlayerPrefs.GetString("glass", string.Empty);
				FengGameManagerMKII.settings[7] = PlayerPrefs.GetString("face", string.Empty);
				FengGameManagerMKII.settings[8] = PlayerPrefs.GetString("skin", string.Empty);
				FengGameManagerMKII.settings[9] = PlayerPrefs.GetString("costume", string.Empty);
				FengGameManagerMKII.settings[10] = PlayerPrefs.GetString("logo", string.Empty);
				FengGameManagerMKII.settings[11] = PlayerPrefs.GetString("bladel", string.Empty);
				FengGameManagerMKII.settings[12] = PlayerPrefs.GetString("blader", string.Empty);
				FengGameManagerMKII.settings[14] = PlayerPrefs.GetString("hoodie", string.Empty);
			}
			else if ((int)FengGameManagerMKII.settings[133] == 1)
			{
				FengGameManagerMKII.settings[135] = PlayerPrefs.GetString("hair2", string.Empty);
				FengGameManagerMKII.settings[136] = PlayerPrefs.GetString("eye2", string.Empty);
				FengGameManagerMKII.settings[137] = PlayerPrefs.GetString("glass2", string.Empty);
				FengGameManagerMKII.settings[138] = PlayerPrefs.GetString("face2", string.Empty);
				FengGameManagerMKII.settings[139] = PlayerPrefs.GetString("skin2", string.Empty);
				FengGameManagerMKII.settings[140] = PlayerPrefs.GetString("costume2", string.Empty);
				FengGameManagerMKII.settings[141] = PlayerPrefs.GetString("logo2", string.Empty);
				FengGameManagerMKII.settings[142] = PlayerPrefs.GetString("bladel2", string.Empty);
				FengGameManagerMKII.settings[143] = PlayerPrefs.GetString("blader2", string.Empty);
				FengGameManagerMKII.settings[145] = PlayerPrefs.GetString("hoodie2", string.Empty);
			}
			else if ((int)FengGameManagerMKII.settings[133] == 2)
			{
				FengGameManagerMKII.settings[148] = PlayerPrefs.GetString("hair3", string.Empty);
				FengGameManagerMKII.settings[149] = PlayerPrefs.GetString("eye3", string.Empty);
				FengGameManagerMKII.settings[150] = PlayerPrefs.GetString("glass3", string.Empty);
				FengGameManagerMKII.settings[151] = PlayerPrefs.GetString("face3", string.Empty);
				FengGameManagerMKII.settings[152] = PlayerPrefs.GetString("skin3", string.Empty);
				FengGameManagerMKII.settings[153] = PlayerPrefs.GetString("costume3", string.Empty);
				FengGameManagerMKII.settings[154] = PlayerPrefs.GetString("logo3", string.Empty);
				FengGameManagerMKII.settings[155] = PlayerPrefs.GetString("bladel3", string.Empty);
				FengGameManagerMKII.settings[156] = PlayerPrefs.GetString("blader3", string.Empty);
				FengGameManagerMKII.settings[158] = PlayerPrefs.GetString("hoodie3", string.Empty);
			}
			else if ((int)FengGameManagerMKII.settings[133] == 3)
			{
				FengGameManagerMKII.settings[304] = PlayerPrefs.GetString("hair4", string.Empty);
				FengGameManagerMKII.settings[305] = PlayerPrefs.GetString("eye4", string.Empty);
				FengGameManagerMKII.settings[306] = PlayerPrefs.GetString("glass4", string.Empty);
				FengGameManagerMKII.settings[307] = PlayerPrefs.GetString("face4", string.Empty);
				FengGameManagerMKII.settings[308] = PlayerPrefs.GetString("skin4", string.Empty);
				FengGameManagerMKII.settings[309] = PlayerPrefs.GetString("costume4", string.Empty);
				FengGameManagerMKII.settings[310] = PlayerPrefs.GetString("logo4", string.Empty);
				FengGameManagerMKII.settings[311] = PlayerPrefs.GetString("bladel4", string.Empty);
				FengGameManagerMKII.settings[312] = PlayerPrefs.GetString("blader4", string.Empty);
				FengGameManagerMKII.settings[314] = PlayerPrefs.GetString("hoodie4", string.Empty);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		string[] texts = new string[4] { "Set 1", "Set 2", "Set 3", "Set 4" };
		FengGameManagerMKII.settings[133] = GUILayout.SelectionGrid((int)FengGameManagerMKII.settings[133], texts, 5, GUILayout.MinHeight(30f));
		if (GUILayout.Button("Preview"))
		{
			StartCoroutine(SkinLoad());
		}
	}

	private void MyStats(int id)
	{
		GUILayout.BeginArea(new Rect(5f, 30f, StatsRect.width - 10f, StatsRect.height - 55f));
		GUILayout.BeginVertical();
		string empty = string.Empty;
		bool flag = false;
		if (skinId == 2 || costumeId == 26)
		{
			if (currentSlot == "Set 1" && CharacterSetOption == 0)
			{
				FengGameManagerMKII.settings[283] = 0;
			}
			else if (currentSlot == "Set 2" && CharacterSetOption == 1)
			{
				FengGameManagerMKII.settings[295] = 0;
			}
			else if (currentSlot == "Set 3" && CharacterSetOption == 2)
			{
				FengGameManagerMKII.settings[296] = 0;
			}
			flag = true;
		}
		if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 0) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 0) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 0))
		{
			empty = ((calTotalPoints() <= 400) ? string.Concat(calTotalPoints()) : ("<color=red>" + calTotalPoints() + "</color>"));
			GUILayout.Label("Total Points: " + empty + "/400");
			GUILayout.BeginHorizontal();
			GUILayout.Label("SPD: " + setup.myCostume.stat.SPD, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			setup.myCostume.stat.SPD = Convert.ToInt32(GUILayout.HorizontalSlider(setup.myCostume.stat.SPD, 75f, 125f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("GAS: " + setup.myCostume.stat.GAS, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			setup.myCostume.stat.GAS = Convert.ToInt32(GUILayout.HorizontalSlider(setup.myCostume.stat.GAS, 75f, 125f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("BLA: " + setup.myCostume.stat.BLA, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			setup.myCostume.stat.BLA = Convert.ToInt32(GUILayout.HorizontalSlider(setup.myCostume.stat.BLA, 75f, 125f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("ACL: " + setup.myCostume.stat.ACL, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			setup.myCostume.stat.ACL = Convert.ToInt32(GUILayout.HorizontalSlider(setup.myCostume.stat.ACL, 75f, 125f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Skill: ", GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			if (GUILayout.Button("<", GUILayout.MaxWidth(20f), GUILayout.MinHeight(20f)))
			{
				skillId = toPrev(skillId, skillOption.Length);
				setup.myCostume.stat.skillId = skillOption[skillId];
				character.GetComponent<CharacterCreateAnimationControl>().playAttack(setup.myCostume.stat.skillId);
			}
			GUILayout.Button(setup.myCostume.stat.skillId, GUILayout.MinHeight(20f));
			if (GUILayout.Button(">", GUILayout.MaxWidth(20f), GUILayout.MinHeight(20f)))
			{
				skillId = toNext(skillId, skillOption.Length);
				setup.myCostume.stat.skillId = skillOption[skillId];
				character.GetComponent<CharacterCreateAnimationControl>().playAttack(setup.myCostume.stat.skillId);
			}
			GUILayout.EndHorizontal();
		}
		else
		{
			if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 1) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 1) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 1))
			{
				setup.myCostume.stat.skillId = "mikasa";
				setup.myCostume.stat.SPD = 125;
				setup.myCostume.stat.GAS = 75;
				setup.myCostume.stat.BLA = 75;
				setup.myCostume.stat.ACL = 135;
			}
			if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 2) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 2) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 2))
			{
				setup.myCostume.stat.skillId = "levi";
				setup.myCostume.stat.SPD = 95;
				setup.myCostume.stat.GAS = 100;
				setup.myCostume.stat.BLA = 100;
				setup.myCostume.stat.ACL = 150;
			}
			if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 3) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 3) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 3))
			{
				setup.myCostume.stat.skillId = "armin";
				setup.myCostume.stat.SPD = 75;
				setup.myCostume.stat.GAS = 150;
				setup.myCostume.stat.BLA = 125;
				setup.myCostume.stat.ACL = 85;
			}
			if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 4) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 4) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 4))
			{
				setup.myCostume.stat.skillId = "marco";
				setup.myCostume.stat.SPD = 110;
				setup.myCostume.stat.GAS = 100;
				setup.myCostume.stat.BLA = 115;
				setup.myCostume.stat.ACL = 95;
			}
			if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 5) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 5) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 5))
			{
				setup.myCostume.stat.skillId = "jean";
				setup.myCostume.stat.SPD = 100;
				setup.myCostume.stat.GAS = 150;
				setup.myCostume.stat.BLA = 80;
				setup.myCostume.stat.ACL = 100;
			}
			if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 6) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 6) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 6))
			{
				setup.myCostume.stat.skillId = "eren";
				setup.myCostume.stat.SPD = 100;
				setup.myCostume.stat.GAS = 90;
				setup.myCostume.stat.BLA = 90;
				setup.myCostume.stat.ACL = 100;
			}
			if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 7) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 7) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 7))
			{
				setup.myCostume.stat.skillId = "petra";
				setup.myCostume.stat.SPD = 80;
				setup.myCostume.stat.GAS = 110;
				setup.myCostume.stat.BLA = 100;
				setup.myCostume.stat.ACL = 140;
			}
			if ((currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 8) || (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 8) || (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 8))
			{
				setup.myCostume.stat.skillId = "sasha";
				setup.myCostume.stat.SPD = 140;
				setup.myCostume.stat.GAS = 100;
				setup.myCostume.stat.BLA = 100;
				setup.myCostume.stat.ACL = 115;
			}
			empty = string.Concat(calTotalPoints());
			GUILayout.Label("Total Points: " + empty);
			GUILayout.BeginHorizontal();
			GUILayout.Label("SPD: " + setup.myCostume.stat.SPD, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("GAS: " + setup.myCostume.stat.GAS, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("BLA: " + setup.myCostume.stat.BLA, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("ACL: " + setup.myCostume.stat.ACL, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Skill: ", GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			GUILayout.Button(setup.myCostume.stat.skillId, GUILayout.MinHeight(20f));
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(5f);
		CharacterSetOption = GUILayout.SelectionGrid(CharacterSetOption, new string[3] { "Set 1", "Set 2", "Set 3" }, 3, GUILayout.MinHeight(20f));
		if (CharacterSetOption == 0 && currentSlot != "Set 1")
		{
			currentSlot = "Set 1";
			LoadData();
		}
		if (CharacterSetOption == 1 && currentSlot != "Set 2")
		{
			currentSlot = "Set 2";
			LoadData();
		}
		if (CharacterSetOption == 2 && currentSlot != "Set 3")
		{
			currentSlot = "Set 3";
			LoadData();
		}
		GUILayout.Space(5f);
		CharactersScroll = GUILayout.BeginScrollView(CharactersScroll);
		GUILayout.BeginHorizontal();
		string[] texts = new string[9] { "Custom", "Mikasa", "Levi", "Armin", "Marco", "Jean", "Eren", "Petra", "Sasha" };
		if (!flag)
		{
			if (currentSlot == "Set 1" && CharacterSetOption == 0)
			{
				FengGameManagerMKII.settings[283] = GUILayout.SelectionGrid((int)FengGameManagerMKII.settings[283], texts, 9, GUILayout.MinHeight(25f));
			}
			else if (currentSlot == "Set 2" && CharacterSetOption == 1)
			{
				FengGameManagerMKII.settings[295] = GUILayout.SelectionGrid((int)FengGameManagerMKII.settings[295], texts, 9, GUILayout.MinHeight(25f));
			}
			else if (currentSlot == "Set 3" && CharacterSetOption == 2)
			{
				FengGameManagerMKII.settings[296] = GUILayout.SelectionGrid((int)FengGameManagerMKII.settings[296], texts, 9, GUILayout.MinHeight(25f));
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save", GUILayout.Height(40f)))
		{
			if (calTotalPoints() > 400 || setup.myCostume.stat.SPD > 125 || setup.myCostume.stat.ACL > 125 || setup.myCostume.stat.BLA > 125 || setup.myCostume.stat.GAS > 125)
			{
				if (currentSlot == "Set 1" && CharacterSetOption == 0 && (int)FengGameManagerMKII.settings[283] == 0)
				{
					setup.myCostume.stat.SPD = 100;
					setup.myCostume.stat.ACL = 100;
					setup.myCostume.stat.BLA = 100;
					setup.myCostume.stat.GAS = 100;
				}
				else if (currentSlot == "Set 2" && CharacterSetOption == 1 && (int)FengGameManagerMKII.settings[295] == 0)
				{
					setup.myCostume.stat.SPD = 100;
					setup.myCostume.stat.ACL = 100;
					setup.myCostume.stat.BLA = 100;
					setup.myCostume.stat.GAS = 100;
				}
				else if (currentSlot == "Set 3" && CharacterSetOption == 2 && (int)FengGameManagerMKII.settings[296] == 0)
				{
					setup.myCostume.stat.SPD = 100;
					setup.myCostume.stat.ACL = 100;
					setup.myCostume.stat.BLA = 100;
					setup.myCostume.stat.GAS = 100;
				}
			}
			SaveData();
			PlayerPrefs.SetInt("PremadeCharacter", (int)FengGameManagerMKII.settings[283]);
			PlayerPrefs.SetInt("SecondPremadeCharacter", (int)FengGameManagerMKII.settings[295]);
			PlayerPrefs.SetInt("ThirdPremadeCharacter", (int)FengGameManagerMKII.settings[296]);
		}
		if (GUILayout.Button("Quit", GUILayout.Height(40f)))
		{
			UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
			Application.LoadLevel(0);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	public void ExecuteAnimations(string ani)
	{
		if (ani.StartsWith("stand"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("stand");
		}
		if (ani.StartsWith("supply"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("supply");
		}
		if (ani.StartsWith("dodge"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("dodge");
		}
		if (ani.StartsWith("salute"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("salute");
		}
		if (ani.StartsWith("dance"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("armin");
		}
		if (ani.StartsWith("sit"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("sit");
		}
		if (ani.StartsWith("ride"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("ride");
		}
		if (ani.StartsWith("jump"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("jump");
		}
		if (ani.StartsWith("eat"))
		{
			character.GetComponent<CharacterCreateAnimationControl>().playAttack("sasha");
		}
	}

	private void MyComponents(int id)
	{
		GUILayout.BeginArea(new Rect(5f, 30f, ComponentsRect.width - 10f, ComponentsRect.height - 50f));
		GUILayout.Label("", GUILayout.Height(1.5f));
		GUILayout.BeginVertical();
		GUIOption("Preset: ", CreatePart.Preset, HeroCostume.costume[presetId].name);
		GUIOption("Sex: ", CreatePart.Sex, setup.myCostume.sex.ToString());
		GUIOption("Eye: ", CreatePart.Eye, "eye_" + eyeId);
		GUIOption("Face: ", CreatePart.Face, "face_" + faceId);
		GUIOption("Glass: ", CreatePart.Glass, "glass_" + glassId);
		GUIOption("Hair: ", CreatePart.Hair, "hair_" + hairId);
		if (GUILayout.Button(BrightColorsStr))
		{
			if (!BrightColors)
			{
				BrightColors = true;
				BrightColorsStr = "Normal Colors";
			}
			else
			{
				BrightColors = false;
				BrightColorsStr = "Bright Colors";
			}
		}
		RedColorSlider("Red: ", setup.myCostume.hair_color.r);
		GreenColorSlider("Green: ", setup.myCostume.hair_color.g);
		BlueColorSlider("Blue: ", setup.myCostume.hair_color.b);
		GUIOption("Skin: ", CreatePart.Skin, "skin_" + skinId);
		GUIOption("Costume: ", CreatePart.Costume, "costume_" + costumeId);
		GUIOption("Cape: ", CreatePart.Cape, "cape_" + capeId);
		GUIOption("Division: ", CreatePart.Division, "<size=10>" + setup.myCostume.division.ToString() + "</size>");
		GUILayout.EndVertical();
		AnimationScroll = GUILayout.BeginScrollView(AnimationScroll);
		GUILayout.BeginHorizontal();
		for (int i = 0; i < AnimationsList.Length; i++)
		{
			if (GUILayout.Button(AnimationsList[i], GUILayout.MaxHeight(50f), GUILayout.MaxWidth(500f)))
			{
				ExecuteAnimations(AnimationsList[i]);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	private void GUIOption(string Labeltext, CreatePart option, string Buttontext)
	{
		GUILayout.BeginHorizontal();
		if (option != CreatePart.Preset)
		{
			GUILayout.Label(Labeltext, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			if (GUILayout.Button("<", GUILayout.MaxWidth(20f), GUILayout.MinHeight(20f)))
			{
				toOption(option, next: false);
			}
			GUI.backgroundColor = new Color(1f, 1f, 1f, 0f);
			GUILayout.Button(Buttontext, GUILayout.MinHeight(20f));
			GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);
			if (GUILayout.Button(">", GUILayout.MaxWidth(20f), GUILayout.MinHeight(20f)))
			{
				toOption(option, next: true);
			}
		}
		else
		{
			GUILayout.Label(Labeltext, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
			if (GUILayout.Button("<", GUILayout.MaxWidth(20f), GUILayout.MinHeight(20f)))
			{
				prevOption(option);
			}
			GUI.backgroundColor = new Color(1f, 1f, 1f, 0f);
			GUILayout.Button(Buttontext, GUILayout.MinHeight(20f));
			GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);
			if (GUILayout.Button(">", GUILayout.MaxWidth(20f), GUILayout.MinHeight(20f)))
			{
				nextOption(option);
			}
		}
		GUILayout.EndHorizontal();
	}

	private void RedColorSlider(string text, float color)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(text, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
		color = ((!BrightColors) ? GUILayout.HorizontalSlider(color, 0f, 3f) : GUILayout.HorizontalSlider(color, 0f, 1f));
		OnHairRChange(color);
		GUILayout.EndHorizontal();
	}

	private void GreenColorSlider(string text, float color)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(text, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
		color = ((!BrightColors) ? GUILayout.HorizontalSlider(color, 0f, 3f) : GUILayout.HorizontalSlider(color, 0f, 1f));
		OnHairGChange(color);
		GUILayout.EndHorizontal();
	}

	private void BlueColorSlider(string text, float color)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(text, GUILayout.MaxWidth(60f), GUILayout.MinHeight(20f));
		color = ((!BrightColors) ? GUILayout.HorizontalSlider(color, 0f, 3f) : GUILayout.HorizontalSlider(color, 0f, 1f));
		OnHairBChange(color);
		GUILayout.EndHorizontal();
	}

	public void OnGUI()
	{
		GUI.backgroundColor = new Color(0f, 0f, 0f, 1.5f);
		SkinsRect = GUI.Window(553, SkinsRect, SkinsList, "<b><size=15><color=#1999ff>Preview your skins</color></size></b>");
		StatsRect = GUI.Window(2340, StatsRect, MyStats, "<b><size=15><color=#1999ff>Stats</color></size></b>");
		ComponentsRect = GUI.Window(902, ComponentsRect, MyComponents, "<b><size=15><color=#1999ff>Components</color></size></b>");
	}

	private IEnumerator SkinLoad()
	{
		if ((int)FengGameManagerMKII.settings[133] == 0)
		{
			if ((string)FengGameManagerMKII.settings[4] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[4] == "transparent")
				{
					setup.part_hair.renderer.enabled = false;
				}
				else
				{
					setup.part_hair.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[4]);
					yield return var35;
					Texture2D texture = var35.texture;
					var35.Dispose();
					setup.part_hair.renderer.material.SetTexture("_MainTex", texture);
				}
			}
			if ((string)FengGameManagerMKII.settings[6] != string.Empty && glassId != -1)
			{
				if ((string)FengGameManagerMKII.settings[6] == "transparent")
				{
					setup.part_glass.renderer.enabled = false;
				}
				else
				{
					setup.part_glass.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[6]);
					yield return var35;
					Texture2D texture2 = var35.texture;
					var35.Dispose();
					setup.part_glass.renderer.material.mainTexture = texture2;
				}
			}
			if ((string)FengGameManagerMKII.settings[7] != string.Empty && faceId != -1)
			{
				if ((string)FengGameManagerMKII.settings[7] == "transparent")
				{
					setup.part_face.renderer.enabled = false;
				}
				else
				{
					setup.part_face.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[7]);
					yield return var35;
					Texture2D texture3 = var35.texture;
					var35.Dispose();
					setup.part_face.renderer.material.mainTexture = texture3;
				}
			}
			if ((string)FengGameManagerMKII.settings[8] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[8] == "transparent")
				{
					setup.part_head.renderer.enabled = false;
					setup.part_chest.renderer.enabled = false;
					setup.part_hand_l.renderer.enabled = false;
					setup.part_hand_r.renderer.enabled = false;
				}
				else
				{
					setup.part_head.renderer.enabled = true;
					setup.part_chest.renderer.enabled = true;
					setup.part_hand_l.renderer.enabled = true;
					setup.part_hand_r.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[8]);
					yield return var35;
					Texture2D texture4 = var35.texture;
					var35.Dispose();
					setup.part_head.renderer.material.SetTexture("_MainTex", texture4);
					setup.part_chest.renderer.material.SetTexture("_MainTex", texture4);
					setup.part_hand_l.renderer.material.SetTexture("_MainTex", texture4);
					setup.part_hand_r.renderer.material.SetTexture("_MainTex", texture4);
				}
			}
			if ((string)FengGameManagerMKII.settings[9] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[9] == "transparent")
				{
					setup.part_arm_l.renderer.enabled = false;
					setup.part_arm_r.renderer.enabled = false;
					setup.part_leg.renderer.enabled = false;
					setup.part_upper_body.renderer.enabled = false;
					setup.part_chest_2.renderer.enabled = false;
				}
				else
				{
					setup.part_arm_l.renderer.enabled = true;
					setup.part_arm_r.renderer.enabled = true;
					setup.part_leg.renderer.enabled = true;
					setup.part_upper_body.renderer.enabled = true;
					setup.part_chest_2.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[9]);
					yield return var35;
					Texture2D texture5 = var35.texture;
					var35.Dispose();
					setup.part_arm_l.renderer.material.SetTexture("_MainTex", texture5);
					setup.part_arm_r.renderer.material.SetTexture("_MainTex", texture5);
					setup.part_leg.renderer.material.SetTexture("_MainTex", texture5);
					setup.part_upper_body.renderer.material.SetTexture("_MainTex", texture5);
					setup.part_chest_2.renderer.material.SetTexture("_MainTex", texture5);
				}
			}
			if ((string)FengGameManagerMKII.settings[10] != string.Empty && capeId == 1)
			{
				if ((string)FengGameManagerMKII.settings[10] == "transparent")
				{
					setup.part_cape.renderer.enabled = false;
				}
				else
				{
					setup.part_cape.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[10]);
					yield return var35;
					Texture2D texture6 = var35.texture;
					var35.Dispose();
					setup.part_cape.renderer.material.SetTexture("_MainTex", texture6);
				}
			}
			if ((string)FengGameManagerMKII.settings[14] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[14] == "transparent")
				{
					setup.part_chest_1.renderer.enabled = false;
				}
				else
				{
					setup.part_chest_1.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[14]);
					yield return var35;
					Texture2D texture7 = var35.texture;
					var35.Dispose();
					setup.part_chest_1.renderer.material.SetTexture("_MainTex", texture7);
				}
			}
			if ((string)FengGameManagerMKII.settings[11] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[11] == "transparent")
				{
					setup.part_blade_l.renderer.enabled = false;
				}
				else
				{
					setup.part_blade_l.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[11]);
					yield return var35;
					Texture2D texture8 = var35.texture;
					var35.Dispose();
					setup.part_3dmg_gas_l.renderer.material.SetTexture("_MainTex", texture8);
					setup.part_blade_l.renderer.material.SetTexture("_MainTex", texture8);
					setup.part_3dmg.renderer.material.SetTexture("_MainTex", texture8);
					setup.part_3dmg_belt.renderer.material.SetTexture("_MainTex", texture8);
				}
			}
			if ((string)FengGameManagerMKII.settings[12] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[12] == "transparent")
				{
					setup.part_blade_r.renderer.enabled = false;
				}
				else
				{
					setup.part_blade_r.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[12]);
					yield return var35;
					Texture2D texture9 = var35.texture;
					var35.Dispose();
					setup.part_blade_r.renderer.material.SetTexture("_MainTex", texture9);
					setup.part_3dmg_gas_r.renderer.material.SetTexture("_MainTex", texture9);
					setup.part_3dmg.renderer.material.SetTexture("_MainTex", texture9);
					setup.part_3dmg_belt.renderer.material.SetTexture("_MainTex", texture9);
				}
			}
		}
		else if ((int)FengGameManagerMKII.settings[133] == 1)
		{
			if ((string)FengGameManagerMKII.settings[135] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[135] == "transparent")
				{
					setup.part_hair.renderer.enabled = false;
				}
				else
				{
					setup.part_hair.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[135]);
					yield return var35;
					Texture2D texture10 = var35.texture;
					var35.Dispose();
					setup.part_hair.renderer.material.SetTexture("_MainTex", texture10);
				}
			}
			if ((string)FengGameManagerMKII.settings[137] != string.Empty && glassId != -1)
			{
				if ((string)FengGameManagerMKII.settings[137] == "transparent")
				{
					setup.part_glass.renderer.enabled = false;
				}
				else
				{
					setup.part_glass.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[137]);
					yield return var35;
					Texture2D texture11 = var35.texture;
					var35.Dispose();
					setup.part_glass.renderer.material.mainTexture = texture11;
				}
			}
			if ((string)FengGameManagerMKII.settings[138] != string.Empty && faceId != -1)
			{
				if ((string)FengGameManagerMKII.settings[138] == "transparent")
				{
					setup.part_face.renderer.enabled = false;
				}
				else
				{
					setup.part_face.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[138]);
					yield return var35;
					Texture2D texture12 = var35.texture;
					var35.Dispose();
					setup.part_face.renderer.material.mainTexture = texture12;
				}
			}
			if ((string)FengGameManagerMKII.settings[139] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[139] == "transparent")
				{
					setup.part_head.renderer.enabled = false;
					setup.part_chest.renderer.enabled = false;
					setup.part_hand_l.renderer.enabled = false;
					setup.part_hand_r.renderer.enabled = false;
				}
				else
				{
					setup.part_head.renderer.enabled = true;
					setup.part_chest.renderer.enabled = true;
					setup.part_hand_l.renderer.enabled = true;
					setup.part_hand_r.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[139]);
					yield return var35;
					Texture2D texture13 = var35.texture;
					var35.Dispose();
					setup.part_head.renderer.material.SetTexture("_MainTex", texture13);
					setup.part_chest.renderer.material.SetTexture("_MainTex", texture13);
					setup.part_hand_l.renderer.material.SetTexture("_MainTex", texture13);
					setup.part_hand_r.renderer.material.SetTexture("_MainTex", texture13);
				}
			}
			if ((string)FengGameManagerMKII.settings[140] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[140] == "transparent")
				{
					setup.part_arm_l.renderer.enabled = false;
					setup.part_arm_r.renderer.enabled = false;
					setup.part_leg.renderer.enabled = false;
					setup.part_upper_body.renderer.enabled = false;
					setup.part_chest_2.renderer.enabled = false;
				}
				else
				{
					setup.part_arm_l.renderer.enabled = true;
					setup.part_arm_r.renderer.enabled = true;
					setup.part_leg.renderer.enabled = true;
					setup.part_upper_body.renderer.enabled = true;
					setup.part_chest_2.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[140]);
					yield return var35;
					Texture2D texture14 = var35.texture;
					var35.Dispose();
					setup.part_arm_l.renderer.material.SetTexture("_MainTex", texture14);
					setup.part_arm_r.renderer.material.SetTexture("_MainTex", texture14);
					setup.part_leg.renderer.material.SetTexture("_MainTex", texture14);
					setup.part_upper_body.renderer.material.SetTexture("_MainTex", texture14);
					setup.part_chest_2.renderer.material.SetTexture("_MainTex", texture14);
				}
			}
			if ((string)FengGameManagerMKII.settings[141] != string.Empty && capeId == 1)
			{
				if ((string)FengGameManagerMKII.settings[141] == "transparent")
				{
					setup.part_cape.renderer.enabled = false;
				}
				else
				{
					setup.part_cape.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[141]);
					yield return var35;
					Texture2D texture15 = var35.texture;
					var35.Dispose();
					setup.part_cape.renderer.material.SetTexture("_MainTex", texture15);
				}
			}
			if ((string)FengGameManagerMKII.settings[145] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[145] == "transparent")
				{
					setup.part_chest_1.renderer.enabled = false;
				}
				else
				{
					setup.part_chest_1.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[145]);
					yield return var35;
					Texture2D texture16 = var35.texture;
					var35.Dispose();
					setup.part_chest_1.renderer.material.SetTexture("_MainTex", texture16);
				}
			}
			if ((string)FengGameManagerMKII.settings[142] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[142] == "transparent")
				{
					setup.part_blade_l.renderer.enabled = false;
				}
				else
				{
					setup.part_blade_l.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[142]);
					yield return var35;
					Texture2D texture17 = var35.texture;
					var35.Dispose();
					setup.part_3dmg_gas_l.renderer.material.SetTexture("_MainTex", texture17);
					setup.part_blade_l.renderer.material.SetTexture("_MainTex", texture17);
					setup.part_3dmg.renderer.material.SetTexture("_MainTex", texture17);
					setup.part_3dmg_belt.renderer.material.SetTexture("_MainTex", texture17);
				}
			}
			if ((string)FengGameManagerMKII.settings[143] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[143] == "transparent")
				{
					setup.part_blade_r.renderer.enabled = false;
				}
				else
				{
					setup.part_blade_r.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[143]);
					yield return var35;
					Texture2D texture18 = var35.texture;
					var35.Dispose();
					setup.part_blade_r.renderer.material.SetTexture("_MainTex", texture18);
					setup.part_3dmg_gas_r.renderer.material.SetTexture("_MainTex", texture18);
					setup.part_3dmg.renderer.material.SetTexture("_MainTex", texture18);
					setup.part_3dmg_belt.renderer.material.SetTexture("_MainTex", texture18);
				}
			}
		}
		else if ((int)FengGameManagerMKII.settings[133] == 2)
		{
			if ((string)FengGameManagerMKII.settings[148] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[148] == "transparent")
				{
					setup.part_hair.renderer.enabled = false;
				}
				else
				{
					setup.part_hair.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[148]);
					yield return var35;
					Texture2D texture19 = var35.texture;
					var35.Dispose();
					setup.part_hair.renderer.material.SetTexture("_MainTex", texture19);
				}
			}
			if ((string)FengGameManagerMKII.settings[150] != string.Empty && glassId != -1)
			{
				if ((string)FengGameManagerMKII.settings[150] == "transparent")
				{
					setup.part_glass.renderer.enabled = false;
				}
				else
				{
					setup.part_glass.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[150]);
					yield return var35;
					Texture2D texture20 = var35.texture;
					var35.Dispose();
					setup.part_glass.renderer.material.mainTexture = texture20;
				}
			}
			if ((string)FengGameManagerMKII.settings[151] != string.Empty && faceId != -1)
			{
				if ((string)FengGameManagerMKII.settings[151] == "transparent")
				{
					setup.part_face.renderer.enabled = false;
				}
				else
				{
					setup.part_face.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[151]);
					yield return var35;
					Texture2D texture21 = var35.texture;
					var35.Dispose();
					setup.part_face.renderer.material.mainTexture = texture21;
				}
			}
			if ((string)FengGameManagerMKII.settings[152] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[152] == "transparent")
				{
					setup.part_head.renderer.enabled = false;
					setup.part_chest.renderer.enabled = false;
					setup.part_hand_l.renderer.enabled = false;
					setup.part_hand_r.renderer.enabled = false;
				}
				else
				{
					setup.part_head.renderer.enabled = true;
					setup.part_chest.renderer.enabled = true;
					setup.part_hand_l.renderer.enabled = true;
					setup.part_hand_r.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[152]);
					yield return var35;
					Texture2D texture22 = var35.texture;
					var35.Dispose();
					setup.part_head.renderer.material.SetTexture("_MainTex", texture22);
					setup.part_chest.renderer.material.SetTexture("_MainTex", texture22);
					setup.part_hand_l.renderer.material.SetTexture("_MainTex", texture22);
					setup.part_hand_r.renderer.material.SetTexture("_MainTex", texture22);
				}
			}
			if ((string)FengGameManagerMKII.settings[153] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[153] == "transparent")
				{
					setup.part_arm_l.renderer.enabled = false;
					setup.part_arm_r.renderer.enabled = false;
					setup.part_leg.renderer.enabled = false;
					setup.part_upper_body.renderer.enabled = false;
					setup.part_chest_2.renderer.enabled = false;
				}
				else
				{
					setup.part_arm_l.renderer.enabled = true;
					setup.part_arm_r.renderer.enabled = true;
					setup.part_leg.renderer.enabled = true;
					setup.part_upper_body.renderer.enabled = true;
					setup.part_chest_2.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[153]);
					yield return var35;
					Texture2D texture23 = var35.texture;
					var35.Dispose();
					setup.part_arm_l.renderer.material.SetTexture("_MainTex", texture23);
					setup.part_arm_r.renderer.material.SetTexture("_MainTex", texture23);
					setup.part_leg.renderer.material.SetTexture("_MainTex", texture23);
					setup.part_upper_body.renderer.material.SetTexture("_MainTex", texture23);
					setup.part_chest_2.renderer.material.SetTexture("_MainTex", texture23);
				}
			}
			if ((string)FengGameManagerMKII.settings[154] != string.Empty && capeId == 1)
			{
				if ((string)FengGameManagerMKII.settings[154] == "transparent")
				{
					setup.part_cape.renderer.enabled = false;
				}
				else
				{
					setup.part_cape.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[154]);
					yield return var35;
					Texture2D texture24 = var35.texture;
					var35.Dispose();
					setup.part_cape.renderer.material.SetTexture("_MainTex", texture24);
				}
			}
			if ((string)FengGameManagerMKII.settings[158] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[158] == "transparent")
				{
					setup.part_chest_1.renderer.enabled = false;
				}
				else
				{
					setup.part_chest_1.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[158]);
					yield return var35;
					Texture2D texture25 = var35.texture;
					var35.Dispose();
					setup.part_chest_1.renderer.material.SetTexture("_MainTex", texture25);
				}
			}
			if ((string)FengGameManagerMKII.settings[155] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[155] == "transparent")
				{
					setup.part_blade_l.renderer.enabled = false;
				}
				else
				{
					setup.part_blade_l.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[155]);
					yield return var35;
					Texture2D texture26 = var35.texture;
					var35.Dispose();
					setup.part_3dmg_gas_l.renderer.material.SetTexture("_MainTex", texture26);
					setup.part_blade_l.renderer.material.SetTexture("_MainTex", texture26);
					setup.part_3dmg.renderer.material.SetTexture("_MainTex", texture26);
					setup.part_3dmg_belt.renderer.material.SetTexture("_MainTex", texture26);
				}
			}
			if ((string)FengGameManagerMKII.settings[156] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[156] == "transparent")
				{
					setup.part_blade_r.renderer.enabled = false;
				}
				else
				{
					setup.part_blade_r.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[156]);
					yield return var35;
					Texture2D texture27 = var35.texture;
					var35.Dispose();
					setup.part_blade_r.renderer.material.SetTexture("_MainTex", texture27);
					setup.part_3dmg_gas_r.renderer.material.SetTexture("_MainTex", texture27);
					setup.part_3dmg.renderer.material.SetTexture("_MainTex", texture27);
					setup.part_3dmg_belt.renderer.material.SetTexture("_MainTex", texture27);
				}
			}
		}
		else if ((int)FengGameManagerMKII.settings[133] == 3)
		{
			if ((string)FengGameManagerMKII.settings[304] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[304] == "transparent")
				{
					setup.part_hair.renderer.enabled = false;
				}
				else
				{
					setup.part_hair.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[304]);
					yield return var35;
					Texture2D texture28 = var35.texture;
					var35.Dispose();
					setup.part_hair.renderer.material.SetTexture("_MainTex", texture28);
				}
			}
			if ((string)FengGameManagerMKII.settings[306] != string.Empty && glassId != -1)
			{
				if ((string)FengGameManagerMKII.settings[306] == "transparent")
				{
					setup.part_glass.renderer.enabled = false;
				}
				else
				{
					setup.part_glass.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[306]);
					yield return var35;
					Texture2D texture29 = var35.texture;
					var35.Dispose();
					setup.part_glass.renderer.material.mainTexture = texture29;
				}
			}
			if ((string)FengGameManagerMKII.settings[307] != string.Empty && faceId != -1)
			{
				if ((string)FengGameManagerMKII.settings[307] == "transparent")
				{
					setup.part_face.renderer.enabled = false;
				}
				else
				{
					setup.part_face.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[307]);
					yield return var35;
					Texture2D texture30 = var35.texture;
					var35.Dispose();
					setup.part_face.renderer.material.mainTexture = texture30;
				}
			}
			if ((string)FengGameManagerMKII.settings[308] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[308] == "transparent")
				{
					setup.part_head.renderer.enabled = false;
					setup.part_chest.renderer.enabled = false;
					setup.part_hand_l.renderer.enabled = false;
					setup.part_hand_r.renderer.enabled = false;
				}
				else
				{
					setup.part_head.renderer.enabled = true;
					setup.part_chest.renderer.enabled = true;
					setup.part_hand_l.renderer.enabled = true;
					setup.part_hand_r.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[308]);
					yield return var35;
					Texture2D texture31 = var35.texture;
					var35.Dispose();
					setup.part_head.renderer.material.SetTexture("_MainTex", texture31);
					setup.part_chest.renderer.material.SetTexture("_MainTex", texture31);
					setup.part_hand_l.renderer.material.SetTexture("_MainTex", texture31);
					setup.part_hand_r.renderer.material.SetTexture("_MainTex", texture31);
				}
			}
			if ((string)FengGameManagerMKII.settings[309] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[309] == "transparent")
				{
					setup.part_arm_l.renderer.enabled = false;
					setup.part_arm_r.renderer.enabled = false;
					setup.part_leg.renderer.enabled = false;
					setup.part_upper_body.renderer.enabled = false;
					setup.part_chest_2.renderer.enabled = false;
				}
				else
				{
					setup.part_arm_l.renderer.enabled = true;
					setup.part_arm_r.renderer.enabled = true;
					setup.part_leg.renderer.enabled = true;
					setup.part_upper_body.renderer.enabled = true;
					setup.part_chest_2.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[309]);
					yield return var35;
					Texture2D texture32 = var35.texture;
					var35.Dispose();
					setup.part_arm_l.renderer.material.SetTexture("_MainTex", texture32);
					setup.part_arm_r.renderer.material.SetTexture("_MainTex", texture32);
					setup.part_leg.renderer.material.SetTexture("_MainTex", texture32);
					setup.part_upper_body.renderer.material.SetTexture("_MainTex", texture32);
					setup.part_chest_2.renderer.material.SetTexture("_MainTex", texture32);
				}
			}
			if ((string)FengGameManagerMKII.settings[310] != string.Empty && capeId == 1)
			{
				if ((string)FengGameManagerMKII.settings[310] == "transparent")
				{
					setup.part_cape.renderer.enabled = false;
				}
				else
				{
					setup.part_cape.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[310]);
					yield return var35;
					Texture2D texture33 = var35.texture;
					var35.Dispose();
					setup.part_cape.renderer.material.SetTexture("_MainTex", texture33);
				}
			}
			if ((string)FengGameManagerMKII.settings[314] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[314] == "transparent")
				{
					setup.part_chest_1.renderer.enabled = false;
				}
				else
				{
					setup.part_chest_1.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[314]);
					yield return var35;
					Texture2D texture34 = var35.texture;
					var35.Dispose();
					setup.part_chest_1.renderer.material.SetTexture("_MainTex", texture34);
				}
			}
			if ((string)FengGameManagerMKII.settings[311] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[311] == "transparent")
				{
					setup.part_blade_l.renderer.enabled = false;
				}
				else
				{
					setup.part_blade_l.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[311]);
					yield return var35;
					Texture2D texture35 = var35.texture;
					var35.Dispose();
					setup.part_3dmg_gas_l.renderer.material.SetTexture("_MainTex", texture35);
					setup.part_blade_l.renderer.material.SetTexture("_MainTex", texture35);
					setup.part_3dmg.renderer.material.SetTexture("_MainTex", texture35);
					setup.part_3dmg_belt.renderer.material.SetTexture("_MainTex", texture35);
				}
			}
			if ((string)FengGameManagerMKII.settings[312] != string.Empty)
			{
				if ((string)FengGameManagerMKII.settings[312] == "transparent")
				{
					setup.part_blade_r.renderer.enabled = false;
				}
				else
				{
					setup.part_blade_r.renderer.enabled = true;
					WWW var35 = new WWW((string)FengGameManagerMKII.settings[312]);
					yield return var35;
					Texture2D texture36 = var35.texture;
					var35.Dispose();
					setup.part_blade_r.renderer.material.SetTexture("_MainTex", texture36);
					setup.part_3dmg_gas_r.renderer.material.SetTexture("_MainTex", texture36);
					setup.part_3dmg.renderer.material.SetTexture("_MainTex", texture36);
					setup.part_3dmg_belt.renderer.material.SetTexture("_MainTex", texture36);
				}
			}
		}
		yield return null;
	}

	private int calTotalPoints()
	{
		if (setup.myCostume != null)
		{
			return 0 + setup.myCostume.stat.SPD + setup.myCostume.stat.GAS + setup.myCostume.stat.BLA + setup.myCostume.stat.ACL;
		}
		return 460;
	}

	private void copyBodyCostume(HeroCostume from, HeroCostume to)
	{
		to.arm_l_mesh = from.arm_l_mesh;
		to.arm_r_mesh = from.arm_r_mesh;
		to.body_mesh = from.body_mesh;
		to.body_texture = from.body_texture;
		to.uniform_type = from.uniform_type;
		to.part_chest_1_object_mesh = from.part_chest_1_object_mesh;
		to.part_chest_1_object_texture = from.part_chest_1_object_texture;
		to.part_chest_object_mesh = from.part_chest_object_mesh;
		to.part_chest_object_texture = from.part_chest_object_texture;
		to.part_chest_skinned_cloth_mesh = from.part_chest_skinned_cloth_mesh;
		to.part_chest_skinned_cloth_texture = from.part_chest_skinned_cloth_texture;
		to.division = from.division;
		to.id = from.id;
		to.costumeId = from.costumeId;
	}

	private void copyCostume(HeroCostume from, HeroCostume to, bool init = false)
	{
		copyBodyCostume(from, to);
		to.sex = from.sex;
		to.hair_mesh = from.hair_mesh;
		to.hair_1_mesh = from.hair_1_mesh;
		to.hair_color = new Color(from.hair_color.r, from.hair_color.g, from.hair_color.b);
		to.hairInfo = from.hairInfo;
		to.cape = from.cape;
		to.cape_mesh = from.cape_mesh;
		to.cape_texture = from.cape_texture;
		to.brand1_mesh = from.brand1_mesh;
		to.brand2_mesh = from.brand2_mesh;
		to.brand3_mesh = from.brand3_mesh;
		to.brand4_mesh = from.brand4_mesh;
		to.brand_texture = from.brand_texture;
		to._3dmg_texture = from._3dmg_texture;
		to.face_texture = from.face_texture;
		to.eye_mesh = from.eye_mesh;
		to.glass_mesh = from.glass_mesh;
		to.beard_mesh = from.beard_mesh;
		to.eye_texture_id = from.eye_texture_id;
		to.beard_texture_id = from.beard_texture_id;
		to.glass_texture_id = from.glass_texture_id;
		to.skin_color = from.skin_color;
		to.skin_texture = from.skin_texture;
		to.beard_texture_id = from.beard_texture_id;
		to.hand_l_mesh = from.hand_l_mesh;
		to.hand_r_mesh = from.hand_r_mesh;
		to.mesh_3dmg = from.mesh_3dmg;
		to.mesh_3dmg_gas_l = from.mesh_3dmg_gas_l;
		to.mesh_3dmg_gas_r = from.mesh_3dmg_gas_r;
		to.mesh_3dmg_belt = from.mesh_3dmg_belt;
		to.weapon_l_mesh = from.weapon_l_mesh;
		to.weapon_r_mesh = from.weapon_r_mesh;
		if (init)
		{
			to.stat = new HeroStat();
			to.stat.ACL = 100;
			to.stat.SPD = 100;
			to.stat.GAS = 100;
			to.stat.BLA = 100;
			to.stat.skillId = "mikasa";
		}
		else
		{
			to.stat = new HeroStat();
			to.stat.ACL = from.stat.ACL;
			to.stat.SPD = from.stat.SPD;
			to.stat.GAS = from.stat.GAS;
			to.stat.BLA = from.stat.BLA;
			to.stat.skillId = from.stat.skillId;
		}
	}

	private void CostumeDataToMyID()
	{
		int num = 0;
		for (num = 0; num < sexOption.Length; num++)
		{
			if (sexOption[num] == setup.myCostume.sex)
			{
				sexId = num;
				break;
			}
		}
		for (num = 0; num < eyeOption.Length; num++)
		{
			if (eyeOption[num] == setup.myCostume.eye_texture_id)
			{
				eyeId = num;
				break;
			}
		}
		faceId = -1;
		for (num = 0; num < faceOption.Length; num++)
		{
			if (faceOption[num] == setup.myCostume.beard_texture_id)
			{
				faceId = num;
				break;
			}
		}
		glassId = -1;
		for (num = 0; num < glassOption.Length; num++)
		{
			if (glassOption[num] == setup.myCostume.glass_texture_id)
			{
				glassId = num;
				break;
			}
		}
		for (num = 0; num < hairOption.Length; num++)
		{
			if (hairOption[num] == setup.myCostume.hairInfo.id)
			{
				hairId = num;
				break;
			}
		}
		for (num = 0; num < skinOption.Length; num++)
		{
			if (skinOption[num] == setup.myCostume.skin_color)
			{
				skinId = num;
				break;
			}
		}
		if (setup.myCostume.cape)
		{
			capeId = 1;
		}
		else
		{
			capeId = 0;
		}
		for (num = 0; num < divisionOption.Length; num++)
		{
			if (divisionOption[num] == setup.myCostume.division)
			{
				divisionId = num;
				break;
			}
		}
		costumeId = setup.myCostume.costumeId;
		float r = setup.myCostume.hair_color.r;
		float g = setup.myCostume.hair_color.g;
		float b = setup.myCostume.hair_color.b;
		hairR.GetComponent<UISlider>().sliderValue = r;
		hairG.GetComponent<UISlider>().sliderValue = g;
		hairB.GetComponent<UISlider>().sliderValue = b;
		for (num = 0; num < skillOption.Length; num++)
		{
			if (skillOption[num] == setup.myCostume.stat.skillId)
			{
				skillId = num;
				break;
			}
		}
	}

	private void freshLabel()
	{
	}

	public void LoadData()
	{
		HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(currentSlot);
		if (heroCostume != null)
		{
			copyCostume(heroCostume, setup.myCostume);
			setup.deleteCharacterComponent();
			setup.setCharacterComponent();
		}
		CostumeDataToMyID();
		freshLabel();
	}

	public void nextOption(CreatePart part)
	{
		if (part == CreatePart.Preset)
		{
			presetId = toNext(presetId, HeroCostume.costume.Length);
			copyCostume(HeroCostume.costume[presetId], setup.myCostume, init: true);
			CostumeDataToMyID();
			setup.deleteCharacterComponent();
			setup.setCharacterComponent();
			labelPreset.GetComponent<UILabel>().text = HeroCostume.costume[presetId].name;
			freshLabel();
		}
		else
		{
			toOption(part, next: true);
		}
	}

	public void nextStatOption(CreateStat type)
	{
		if (type == CreateStat.Skill)
		{
			skillId = toNext(skillId, skillOption.Length);
			setup.myCostume.stat.skillId = skillOption[skillId];
			character.GetComponent<CharacterCreateAnimationControl>().playAttack(setup.myCostume.stat.skillId);
			freshLabel();
		}
		else if (calTotalPoints() < 460)
		{
			setStatPoint(type, 1);
		}
	}

	public void OnHairBChange(float value)
	{
		if (setup != null && setup.myCostume != null && setup.part_hair != null)
		{
			setup.myCostume.hair_color = new Color(setup.part_hair.renderer.material.color.r, setup.part_hair.renderer.material.color.g, value);
			setHairColor();
		}
	}

	public void OnHairGChange(float value)
	{
		if (setup.myCostume != null && setup.part_hair != null)
		{
			setup.myCostume.hair_color = new Color(setup.part_hair.renderer.material.color.r, value, setup.part_hair.renderer.material.color.b);
			setHairColor();
		}
	}

	public void OnHairRChange(float value)
	{
		if (setup.myCostume != null && setup.part_hair != null)
		{
			setup.myCostume.hair_color = new Color(value, setup.part_hair.renderer.material.color.g, setup.part_hair.renderer.material.color.b);
			setHairColor();
		}
	}

	public void OnSoltChange(string id)
	{
		currentSlot = id;
	}

	public void prevOption(CreatePart part)
	{
		if (part == CreatePart.Preset)
		{
			presetId = toPrev(presetId, HeroCostume.costume.Length);
			copyCostume(HeroCostume.costume[presetId], setup.myCostume, init: true);
			CostumeDataToMyID();
			setup.deleteCharacterComponent();
			setup.setCharacterComponent();
			labelPreset.GetComponent<UILabel>().text = HeroCostume.costume[presetId].name;
			freshLabel();
		}
		else
		{
			toOption(part, next: false);
		}
	}

	public void prevStatOption(CreateStat type)
	{
		if (type == CreateStat.Skill)
		{
			skillId = toPrev(skillId, skillOption.Length);
			setup.myCostume.stat.skillId = skillOption[skillId];
			character.GetComponent<CharacterCreateAnimationControl>().playAttack(setup.myCostume.stat.skillId);
			freshLabel();
		}
		else
		{
			setStatPoint(type, -1);
		}
	}

	public void SaveData()
	{
		CostumeConeveter.HeroCostumeToLocalData(setup.myCostume, currentSlot);
	}

	private void setHairColor()
	{
		if (setup.part_hair != null)
		{
			setup.part_hair.renderer.material.color = setup.myCostume.hair_color;
		}
		if (setup.part_hair_1 != null)
		{
			setup.part_hair_1.renderer.material.color = setup.myCostume.hair_color;
		}
	}

	private void setStatPoint(CreateStat type, int pt)
	{
		switch (type)
		{
		case CreateStat.SPD:
			setup.myCostume.stat.SPD += pt;
			break;
		case CreateStat.GAS:
			setup.myCostume.stat.GAS += pt;
			break;
		case CreateStat.BLA:
			setup.myCostume.stat.BLA += pt;
			break;
		case CreateStat.ACL:
			setup.myCostume.stat.ACL += pt;
			break;
		}
		setup.myCostume.stat.SPD = Mathf.Clamp(setup.myCostume.stat.SPD, 15, 140);
		setup.myCostume.stat.GAS = Mathf.Clamp(setup.myCostume.stat.GAS, 15, 150);
		setup.myCostume.stat.BLA = Mathf.Clamp(setup.myCostume.stat.BLA, 15, 125);
		setup.myCostume.stat.ACL = Mathf.Clamp(setup.myCostume.stat.ACL, 15, 150);
		freshLabel();
	}

	private void Start()
	{
		QualitySettings.SetQualityLevel(5, applyExpensiveChanges: true);
		costumeOption = HeroCostume.costumeOption;
		setup = character.GetComponent<HERO_SETUP>();
		setup.init();
		setup.myCostume = new HeroCostume();
		copyCostume(HeroCostume.costume[2], setup.myCostume);
		setup.myCostume.setMesh();
		setup.setCharacterComponent();
		sexOption = new SEX[2]
		{
			SEX.MALE,
			SEX.FEMALE
		};
		eyeOption = new int[28];
		for (int i = 0; i < 28; i++)
		{
			eyeOption[i] = i;
		}
		faceOption = new int[14];
		for (int i = 0; i < 14; i++)
		{
			faceOption[i] = i + 32;
		}
		glassOption = new int[10];
		for (int i = 0; i < 10; i++)
		{
			glassOption[i] = i + 48;
		}
		hairOption = new int[11];
		for (int i = 0; i < 11; i++)
		{
			hairOption[i] = i;
		}
		skinOption = new int[3];
		for (int i = 0; i < 3; i++)
		{
			skinOption[i] = i + 1;
		}
		capeOption = new int[2];
		for (int i = 0; i < 2; i++)
		{
			capeOption[i] = i;
		}
		divisionOption = new DIVISION[4]
		{
			DIVISION.TraineesSquad,
			DIVISION.TheGarrison,
			DIVISION.TheMilitaryPolice,
			DIVISION.TheSurveryCorps
		};
		skillOption = new string[7] { "mikasa", "levi", "sasha", "jean", "marco", "armin", "petra" };
		CostumeDataToMyID();
		LoadData();
	}

	private int toNext(int id, int Count, int start = 0)
	{
		id++;
		if (id >= Count)
		{
			id = start;
		}
		id = Mathf.Clamp(id, start, start + Count - 1);
		return id;
	}

	public void toOption(CreatePart part, bool next)
	{
		switch (part)
		{
		case CreatePart.Sex:
			sexId = ((!next) ? toPrev(sexId, sexOption.Length) : toNext(sexId, sexOption.Length));
			if (sexId == 0)
			{
				costumeId = 11;
			}
			else
			{
				costumeId = 0;
			}
			copyCostume(costumeOption[costumeId], setup.myCostume, init: true);
			setup.myCostume.sex = sexOption[sexId];
			character.GetComponent<CharacterCreateAnimationControl>().toStand();
			CostumeDataToMyID();
			setup.deleteCharacterComponent();
			setup.setCharacterComponent();
			break;
		case CreatePart.Eye:
			eyeId = ((!next) ? toPrev(eyeId, eyeOption.Length) : toNext(eyeId, eyeOption.Length));
			setup.myCostume.eye_texture_id = eyeId;
			setup.setFacialTexture(setup.part_eye, eyeOption[eyeId]);
			break;
		case CreatePart.Face:
			faceId = ((!next) ? toPrev(faceId, faceOption.Length) : toNext(faceId, faceOption.Length));
			setup.myCostume.beard_texture_id = faceOption[faceId];
			if (setup.part_face == null)
			{
				setup.createFace();
			}
			setup.setFacialTexture(setup.part_face, faceOption[faceId]);
			break;
		case CreatePart.Glass:
			glassId = ((!next) ? toPrev(glassId, glassOption.Length) : toNext(glassId, glassOption.Length));
			setup.myCostume.glass_texture_id = glassOption[glassId];
			if (setup.part_glass == null)
			{
				setup.createGlass();
			}
			setup.setFacialTexture(setup.part_glass, glassOption[glassId]);
			break;
		case CreatePart.Hair:
			hairId = ((!next) ? toPrev(hairId, hairOption.Length) : toNext(hairId, hairOption.Length));
			if (sexId == 0)
			{
				setup.myCostume.hair_mesh = CostumeHair.hairsM[hairOption[hairId]].hair;
				setup.myCostume.hair_1_mesh = CostumeHair.hairsM[hairOption[hairId]].hair_1;
				setup.myCostume.hairInfo = CostumeHair.hairsM[hairOption[hairId]];
			}
			else
			{
				setup.myCostume.hair_mesh = CostumeHair.hairsF[hairOption[hairId]].hair;
				setup.myCostume.hair_1_mesh = CostumeHair.hairsF[hairOption[hairId]].hair_1;
				setup.myCostume.hairInfo = CostumeHair.hairsF[hairOption[hairId]];
			}
			setup.createHair();
			setHairColor();
			break;
		case CreatePart.Skin:
			if (setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
			{
				skinId = 2;
			}
			else
			{
				skinId = ((!next) ? toPrev(skinId, 2) : toNext(skinId, 2));
			}
			setup.myCostume.skin_color = skinOption[skinId];
			setup.myCostume.setTexture();
			setup.setSkin();
			break;
		case CreatePart.Costume:
			if (setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
			{
				if (setup.myCostume.sex == SEX.FEMALE)
				{
					costumeId = 26;
				}
				else if (setup.myCostume.sex == SEX.MALE)
				{
					costumeId = 25;
				}
			}
			else if (sexId != 0)
			{
				costumeId = ((!next) ? toPrev(costumeId, 24) : toNext(costumeId, 24));
			}
			else
			{
				costumeId = ((!next) ? toPrev(costumeId, 24) : toNext(costumeId, 24));
			}
			copyBodyCostume(costumeOption[costumeId], setup.myCostume);
			setup.myCostume.setMesh();
			setup.myCostume.setTexture();
			setup.createUpperBody();
			setup.createLeftArm();
			setup.createRightArm();
			setup.createLowerBody();
			break;
		case CreatePart.Cape:
			capeId = ((!next) ? toPrev(capeId, capeOption.Length) : toNext(capeId, capeOption.Length));
			setup.myCostume.cape = capeId == 1;
			setup.myCostume.setCape();
			setup.myCostume.setTexture();
			setup.createCape();
			break;
		case CreatePart.Division:
			divisionId = ((!next) ? toPrev(divisionId, divisionOption.Length) : toNext(divisionId, divisionOption.Length));
			setup.myCostume.division = divisionOption[divisionId];
			setup.myCostume.setTexture();
			setup.createUpperBody();
			break;
		}
		freshLabel();
	}

	private int toPrev(int id, int Count, int start = 0)
	{
		id--;
		if (id < start)
		{
			id = Count - 1;
		}
		id = Mathf.Clamp(id, start, start + Count - 1);
		return id;
	}
}
