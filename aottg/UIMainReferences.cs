using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BRM;
using UnityEngine;

public class UIMainReferences : MonoBehaviour
{
	public static string fengVersion;

	public static bool isGAMEFirstLaunch;

	public GameObject panelCredits;

	public GameObject PanelDisconnect;

	public GameObject panelMain;

	public GameObject PanelMultiJoinPrivate;

	public GameObject PanelMultiPWD;

	public GameObject panelMultiROOM;

	public GameObject panelMultiSet;

	public GameObject panelMultiStart;

	public GameObject PanelMultiWait;

	public GameObject panelOption;

	public GameObject panelSingleSet;

	public GameObject PanelSnapShot;

	public static string version;

	public static UIMainReferences UIRefer;

	public Texture2D CursorTexture;

	public CursorMode cursorMode;

	public static Texture2D[] GuiStylesTextures;

	private static bool styleInited;

	private static Texture2D ToggleY;

	private static Texture2D ToggleN;

	private static Texture2D ToggleH;

	private static Texture2D ToggleYH;

	private static string thirdLine;

	private static string fourthLine;

	public static int Width;

	public static int Height;

	public static bool Fullscreen;

	static UIMainReferences()
	{
		isGAMEFirstLaunch = true;
		version = "01042015";
		styleInited = false;
		thirdLine = File.ReadAllLines(Environment.CurrentDirectory + "\\Resolution.txt").ElementAtOrDefault(2);
		fourthLine = File.ReadAllLines(Environment.CurrentDirectory + "\\Resolution.txt").ElementAtOrDefault(3);
		Width = int.MinValue;
		Height = int.MinValue;
		Fullscreen = false;
		string[] source = File.ReadAllLines(Environment.CurrentDirectory + "\\Resolution.txt");
		Width = Convert.ToInt32(source.ElementAtOrDefault(0));
		Height = Convert.ToInt32(source.ElementAtOrDefault(1));
		if (thirdLine.Contains("Fullscreen False"))
		{
			Fullscreen = false;
		}
		else
		{
			Fullscreen = true;
		}
	}

    private void Awake()
    {
        UIRefer = this;
        if (isGAMEFirstLaunch)
        {
            if (Fullscreen)
            {
                Resolution res = Screen.currentResolution;
                Screen.SetResolution(res.width, res.height, false);
                Screen.fullScreen = false;
                BorderlessHelper.MakeBorderlessFullscreen();
            }
            else
            {
                Screen.SetResolution(Width, Height, false);
            }
        }
    }

    private void Destroy()
	{
		UIRefer = null;
	}

	private void Start()
	{
		fengVersion = "01042015";
		NGUITools.SetActive(panelMain, state: true);
		if (version == null || version.StartsWith("error"))
		{
			CacheGameObject.Find("VERSION").GetComponent<UILabel>().text = "Verification failed. Please clear your cache or try another browser";
		}
		if (isGAMEFirstLaunch)
		{
			FengGameManagerMKII.instance.DCPeopleList = new List<string>();
			version = fengVersion;
			isGAMEFirstLaunch = false;
			GameObject obj = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("InputManagerController"));
			obj.name = "InputManagerController";
			UnityEngine.Object.DontDestroyOnLoad(obj);
			FengGameManagerMKII.s = "verified343,hair,character_eye,glass,character_face,character_head,character_hand,character_body,character_arm,character_leg,character_chest,character_cape,character_brand,character_3dmg,r,character_blade_l,character_3dmg_gas_r,character_blade_r,3dmg_smoke,HORSE,hair,body_001,Cube,Plane_031,mikasa_asset,character_cap_,character_gun".Split(',');
			FengGameManagerMKII.loginstate = 0;
			if (fourthLine.Contains("Smoothen True"))
			{
				FengGameManagerMKII.RCassets = WWW.LoadFromCacheOrDownload("File://" + Application.dataPath + "/RCAssets Turbo.unity3d", 934).assetBundle;
			}
			else if (fourthLine.Contains("Smoothen False"))
			{
				FengGameManagerMKII.RCassets = WWW.LoadFromCacheOrDownload("File://" + Application.dataPath + "/RCAssets Normal.unity3d", 804).assetBundle;
			}
			FengGameManagerMKII.someBundle = WWW.LoadFromCacheOrDownload("File://" + Application.dataPath + "/tlw.unity3d", 19497).assetBundle;
			FengGameManagerMKII.Segoe = (Font)FengGameManagerMKII.someBundle.Load("Segoe");
			FengGameManagerMKII.SegoeItalic = (Font)FengGameManagerMKII.someBundle.Load("Segoe Italic");
			FengGameManagerMKII.Shentox = (Font)FengGameManagerMKII.someBundle.Load("Shentox");
			FengGameManagerMKII.ShentoxLight = (Font)FengGameManagerMKII.someBundle.Load("Shentox Light");
			GuiStylesTextures = new Texture2D[11]
			{
				(Texture2D)FengGameManagerMKII.someBundle.Load("textfield"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("textfieldH"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("button"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("buttonH"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("scrollvert"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("scrollhoriz"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("horizslider"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("horizsliderT"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("horizsliderTH"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("buttonRed"),
				(Texture2D)FengGameManagerMKII.someBundle.Load("buttonRedDark")
			};
			ToggleY = (Texture2D)FengGameManagerMKII.someBundle.Load("toggleOn");
			ToggleN = (Texture2D)FengGameManagerMKII.someBundle.Load("toggleOff");
			ToggleH = (Texture2D)FengGameManagerMKII.someBundle.Load("toggleHover");
			ToggleYH = (Texture2D)FengGameManagerMKII.someBundle.Load("toggleOnHover");
			CursorTexture = (Texture2D)FengGameManagerMKII.someBundle.Load("cursor 1");
			Cursor.SetCursor(CursorTexture, new Vector2(CursorTexture.width / 3, 0f), cursorMode);
		}
	}

	public void OnGUI()
	{
		if (!styleInited)
		{
			GUI.skin.label.wordWrap = true;
			GUI.skin.label.font = FengGameManagerMKII.Shentox;
			GUI.skin.label.fontSize = 14;
			GUI.skin.button.font = FengGameManagerMKII.Shentox;
			GUI.skin.button.fontSize = 13;
			GUI.skin.button.normal.background = GuiStylesTextures[3];
			GUI.skin.button.onNormal.background = GuiStylesTextures[9];
			GUI.skin.button.active.background = GuiStylesTextures[3];
			GUI.skin.button.onActive.background = GuiStylesTextures[3];
			GUI.skin.button.hover.background = GuiStylesTextures[10];
			GUI.skin.button.onHover.background = GuiStylesTextures[9];
			GUI.skin.horizontalSlider.normal.background = GuiStylesTextures[6];
			GUI.skin.horizontalSliderThumb.normal.background = GuiStylesTextures[7];
			GUI.skin.horizontalSliderThumb.hover.background = GuiStylesTextures[8];
			GUI.skin.horizontalSliderThumb.active.background = GuiStylesTextures[8];
			GUI.skin.verticalScrollbar.normal.background = GuiStylesTextures[4];
			GUI.skin.verticalScrollbarThumb.normal.background = GuiStylesTextures[4];
			GUI.skin.verticalScrollbarThumb.active.background = GuiStylesTextures[4];
			GUI.skin.horizontalScrollbar.normal.background = GuiStylesTextures[5];
			GUI.skin.horizontalScrollbar.normal.background = GuiStylesTextures[5];
			GUI.skin.horizontalScrollbar.active.background = GuiStylesTextures[5];
			GUI.skin.textField.normal.background = GuiStylesTextures[0];
			GUI.skin.textField.hover.background = GuiStylesTextures[1];
			GUI.skin.textField.active.background = GuiStylesTextures[1];
			GUI.skin.textField.focused.background = GuiStylesTextures[1];
			GUI.skin.window.normal.background = GuiStylesTextures[2];
			GUI.skin.window.onNormal.background = GuiStylesTextures[2];
			GUI.skin.window.active.background = GuiStylesTextures[2];
			GUI.skin.window.onActive.background = GuiStylesTextures[2];
			GUI.skin.toggle.normal.background = ToggleN;
			GUI.skin.toggle.onNormal.background = ToggleY;
			GUI.skin.toggle.active.background = ToggleH;
			GUI.skin.toggle.onActive.background = ToggleYH;
			GUI.skin.toggle.hover.background = ToggleH;
			GUI.skin.toggle.onHover.background = ToggleYH;
			styleInited = true;
		}
	}
}
