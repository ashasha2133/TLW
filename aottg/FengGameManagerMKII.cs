using Anarchy.Custom.Interfaces;
using Anarchy.Custom.Level;
using BRM;
using CLEARSKIES;
using CustomAudio;
using Events;
using ExitGames.Client.Photon;
using MapCeilingNS;
using Photon;
using SimplerGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using Utility;

public class FengGameManagerMKII : Photon.MonoBehaviour, IGameManager
{
	public class OnPlayerLeaveArgs
	{
		public int ViewId { get; set; }

		public PhotonPlayer Player { get; set; }
	}

	public delegate void OnPlayerLeaveHandler(OnPlayerLeaveArgs e);

	public enum BuildType : byte
	{
		Dev = 1,
		Beta = 2,
		Relase = 3,
		Stable = 4,
		Other = byte.MaxValue
	}

	public enum LANGUAGE
	{
		en,
		es,
		pt,
		ru,
		tl,
		th,
		fr,
		it,
		tr
	}

	public static bool am = false;

	public static Dictionary<string, List<string>> amMsgs = new Dictionary<string, List<string>>();

	public static List<string> amBlackList = new List<string>();

	public static int costumeSet;

	public static string race_timer = "10";

	public static string UpdateStatus = "";

	public bool CheatEngine;

	public static object[] shallRejoin = new object[8];

	public static Stopwatch timeofAutorejoin = new Stopwatch();

	public bool isPlayerTTL;

	public static KeyCode BombSpecialKey = KeyCode.F;

	private int changingKeys;

	public string ConsoleFade = "4";

	public string ChatFade = "4";

	public static bool StartingConsole = true;

	public static AssetBundle someBundle;

	public static Texture2D[] PMenuTexturesRC;

	public static Font Shentox;

	public static Font ShentoxLight;

	public static Font Segoe;

	public static Font SegoeItalic;

	public static int LanOrAppID = 0;

	public static int ServerType = 0;

	public Texture2D progressColor;

	public static string FriendName;

	private object thread_locker = new object();

	public Dictionary<int, CannonValues> allowedToCannon;

	public static string applicationId = "e3341a7b-1cb9-4b6c-90e4-39b796e52876";

	public Dictionary<string, Texture2D> assetCacheTextures;

	public static ExitGames.Client.Photon.Hashtable banHash;

	public static ExitGames.Client.Photon.Hashtable boolVariables;

	public static Dictionary<string, GameObject> CachedPrefabs;

	private ArrayList chatContent;

	public GameObject checkpoint;

	private ArrayList cT;

	public static string currentLevel;

	public static string currentScript;

	public static string currentScriptLogic;

	private float currentSpeed;

	public static bool customLevelLoaded;

	public int cyanKills;

	public int cyanWins;

	public int magentaKills;

	public int magentaWins;

	public int difficulty;

	public float distanceSlider;

	public float tiltSlider;

	private bool endRacing;

	private ArrayList eT;

	public static ExitGames.Client.Photon.Hashtable floatVariables;

	private ArrayList fT;

	private float gameEndCD;

	private float gameEndTotalCDtime = 3f;

	public static bool gameStart = false;

	private bool gameTimesUp;

	public static ExitGames.Client.Photon.Hashtable globalVariables;

	public List<GameObject> groundList;

	public static ExitGames.Client.Photon.Hashtable heroHash;

	private int highestwave = 1;

	private ArrayList hooks;

	private int humanScore;

	public static List<int> ignoreList;

	public static ExitGames.Client.Photon.Hashtable imatitan;

	public static InputManagerRC inputRC;

	public static FengGameManagerMKII instance;

	public static ExitGames.Client.Photon.Hashtable intVariables;

	public static bool isAssetLoaded;

	public bool isFirstLoad;

	private bool isLosing;

	public bool isRecompiling;

	public bool isRestarting;

	public bool isSpawning;

	public bool isUnloading;

	private bool isWinning;

	public bool justSuicide;

	private ArrayList kicklist;

	private ArrayList killInfoGO = new ArrayList();

	public static bool LAN;

	public List<string[]> levelCache;

	public static ExitGames.Client.Photon.Hashtable[] linkHash;

	public string localRacingResult;

	public static bool logicLoaded;

	public static int loginstate;

	public static bool masterRC;

	public int maxPlayers;

	private float maxSpeed;

	public float mouseSlider;

	private string myLastHero;

	private string myLastRespawnTag = "playerRespawn";

	public float myRespawnTime;

	public new string name;

	public bool needChooseSide;

	public static bool noRestart;

	public static string oldScript;

	public static string oldScriptLogic;

	public static bool OnPrivateServer;

	public static string passwordField;

	public float pauseWaitTime;

	public string playerList;

	public List<Vector3> playerSpawnsC;

	public List<Vector3> playerSpawnsM;

	public List<PhotonPlayer> playersRPC;

	public static ExitGames.Client.Photon.Hashtable playerVariables;

	public Dictionary<string, int[]> PreservedPlayerKDR;

	public static string PrivateServerAuthPass;

	public static string privateServerField;

	public static string privateAppIDField;

	public static string LANField;

	public int PVPhumanScore;

	private int PVPhumanScoreMax = 200;

	public int PVPtitanScore;

	private int PVPtitanScoreMax = 200;

	public float qualitySlider;

	public List<GameObject> racingDoors;

	private ArrayList racingResult;

	public Vector3 racingSpawnPoint;

	public bool racingSpawnPointSet;

	public static AssetBundle RCassets;

	public static ExitGames.Client.Photon.Hashtable RCEvents;

	public static ExitGames.Client.Photon.Hashtable RCRegions;

	public static ExitGames.Client.Photon.Hashtable RCRegionTriggers;

	public static ExitGames.Client.Photon.Hashtable RCVariableNames;

	public List<float> restartCount;

	public bool restartingBomb;

	public bool restartingEren;

	public bool restartingHorse;

	public bool restartingMC;

	public float retryTime;

	public float roundTime;

	public static string[] s;

	public Vector2 allScrolls;

	public GameObject selectedObj;

	public static object[] settings;

	private int single_kills;

	private int single_maxDamage;

	private int single_totalDamage;

	public static Material skyMaterial;

	public List<GameObject> spectateSprites;

	public bool startRacing;

	public static ExitGames.Client.Photon.Hashtable stringVariables;

	private int[] teamScores;

	private int teamWinner;

	public static Texture2D textureBackgroundBlack;

	public Texture2D textureBackgroundBlue;

	public int time = 600;

	private float timeElapse;

	private float timeTotalServer;

	private int titanScore;

	public List<TitanSpawner> titanSpawners;

	public List<Vector3> titanSpawns;

	public static ExitGames.Client.Photon.Hashtable titanVariables;

	public float updateTime;

	public static string usernameField;

	public int wave = 1;

	public int EventListServer;

	private static float FogDensity;

	public static bool is_skin_gui = false;

	private static string save_skin_text_field = string.Empty;

	public static string script_text_field = string.Empty;

	public static bool customMapScripts = false;

	public static string mapScript = string.Empty;

	public string CursorW = "27";

	public string CursorH = "27";

	public string MinCharL = "3";

	public string MaxCharL = "12";

	public string GLMinCharL = "3";

	public string GLMaxCharL = "12";

	public string ChatMess = "10";

	public string ConsoleMess = "5";

	public string RenderDist = "1500";

	public string FadeTime = "5";

	private static Texture2D HookTexture;

	public static float HookColorFloats0 = 0f;

	public static float HookColorFloats1 = 0f;

	public static float HookColorFloats2 = 0f;

	private static Texture2D NamesTexture;

	public static float NamesColorFloats0 = 0f;

	public static float NamesColorFloats1 = 0f;

	public static float NamesColorFloats2 = 0f;

	private static Texture2D HexTexture;

	private static float HexColorFloats0;

	private static float HexColorFloats1;

	private static float HexColorFloats2;

	private static Texture2D AHexTexture;

	private static float AHexColorFloats0;

	private static float AHexColorFloats1;

	private static float AHexColorFloats2;

	private static Texture2D TeamHexTexture;

	public static float TeamColorFloats0;

	public static float TeamColorFloats1;

	public static float TeamColorFloats2;

	private static Texture2D ETeamHexTexture;

	public static float ETeamColorFloats0;

	public static float ETeamColorFloats1;

	public static float ETeamColorFloats2;

	public static float YellowDamageSize = 0.5f;

	public static float InterfaceVolume;

	public static float AssistantVolume;

	public static string level = string.Empty;

	private static LevelInfo current;

	public UIReferArray uirefer;

	public static PhotonView PView;

	public static GameObject LabelScore;

	public static UILabel LabelScoreUI;

	public static Transform LabelScoreT;

	private UILabel LabelInfoTopRight;

	private string scriptcache = string.Empty;

	public static List<GameObject> allheroes = new List<GameObject>();

	public static List<TITAN> titans = new List<TITAN>();

	public static List<HERO> heroes = new List<HERO>();

	public static List<GameObject> alltitans = new List<GameObject>();

	public static Texture2D ImageFront;

	internal static Queue<GameObject> customObjects = new Queue<GameObject>();

	public static int SelectMode;

	private static Pair<Dictionary<int, string>, bool> PhotonObjectsText = new Pair<Dictionary<int, string>, bool>(new Dictionary<int, string> { { 0, "[000000][[cfcfcf]LIST OF OBJECTS[-]][-]\nOwnerID:Name:Tag:ViewID\n" } }, second: false);

	public static object[] customkey = new object[0];

	private GameObject Door;

	public List<GameObject> batchableObjects = new List<GameObject>();

	public List<GameObject> UIObjects;

	public bool InsideSpecmode;

	public List<string> DCPeopleList;

	public static List<PhotonPlayer> oneLifeList = new List<PhotonPlayer>();

	private string cause = "";

	private static readonly string[] OnOff = new string[2] { "Off", "On" };

	public static string[] ActDisact = new string[54];

	private int PagesNumGame;

	private int SkinsType;

	private int PagesNumVideo;

	private int PagesNumColors;

	private bool autoScroll;

	private bool selectable;

	private bool save;

	private string SearchThisShit = "";

	private string customMapPath = Directory.GetCurrentDirectory() + "/Utils/Maps/Custom Maps/";

	private Rect mapRect = GUIHelpers.AlignRect(500f, 500f, GUIHelpers.Alignment.CENTER);

	private bool resetCache;

	public int tlwv = 6012004;

	public List<int> muteList;

	public List<int> whiteNameList;

	public List<int> markList;

	private AudioClip BotPersonallyContacted;

	private AudioClip BotQuoted;

	private AudioClip BotNoConnection;

	private AudioClip BotTimeFinish;

	private static AudioClip BotWarning;

	private AudioClip ToClick;

	public bool disableEventAntispam;

	public static string nameField;

	public static string guildField;

	public static string Chatname;

	public static string Chatcolor;

	public static string Prefix;

	public static string Suffix;

	public static string nameField2;

	public static string guildField2;

	public static string Chatname2;

	public static string Chatcolor2;

	public static string Prefix2;

	public static string Suffix2;

	private static Stopwatch restartCD = new Stopwatch();

	private float accum;

	private float frequency;

	private int frames;

	private int nbDecimal;

	private string sFPS;

	private GUIStyle MiscStyle;

	private int feed_number;

	private int feed_count;

	private int highest_feed_count;

	private int highest_feed;

	public FeedManager feedManager = new FeedManager();

	public int ConsoleFadeTime => Convert.ToInt32(ConsoleFade);

	public int ChatFadeTime => Convert.ToInt32(ChatFade);

	public int CursorWidth => Convert.ToInt32(CursorW);

	public int CursorHeight => Convert.ToInt32(CursorH);

	public int MinCharLength => Convert.ToInt32(MinCharL);

	public int MaxCharLength => Convert.ToInt32(MaxCharL);

	public int GLMinCharLength => Convert.ToInt32(GLMinCharL);

	public int GLMaxCharLength => Convert.ToInt32(GLMaxCharL);

	public int ChatMessages => Convert.ToInt32(ChatMess);

	public int ConsoleMessages => Convert.ToInt32(ConsoleMess);

	public int RenderDistance => Convert.ToInt32(RenderDist);

	public float SomeFadeTime => Convert.ToSingle(FadeTime);

	public static LevelInfo levelinfo
	{
		get
		{
			if (Application.loadedLevel != 0)
			{
				return current;
			}
			return new LevelInfo();
		}
		set
		{
			current = value;
		}
	}

	public static event OnPlayerLeaveHandler OnPlayerLeave;

	[DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
	private static extern int SetWindowLong(int hwnd, int nIndex, long dwNewLong);

	[DllImport("user32.dll")]
	private static extern int SetLayeredWindowAttributes(int hwnd, int crKey, byte bAlpha, int dwFlags);

	[DllImport("user32.dll")]
	private static extern int GetActiveWindow();

	[DllImport("user32.dll")]
	private static extern long GetWindowLong(int hwnd, int nIndex);

	[DllImport("user32.dll")]
	private static extern int SetWindowPos(int hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);

	private void Awake()
	{
		isBackground = true;
		(PView = base.photonView).isBackground = true;
		instance = this;
		base.gameObject.AddComponent<CustomAnarchyLevel>().GameManager = this;
	}

	public float GetRoundTime()
	{
		return roundTime;
	}

	public string GetLevelName()
	{
		return level;
	}

	public bool IsCustomMapLoaded()
	{
		return customLevelLoaded;
	}

	public void addCT(COLOSSAL_TITAN titan)
	{
		cT.Add(titan);
	}

	public void addET(TITAN_EREN hero)
	{
		eT.Add(hero);
	}

	public void addFT(FEMALE_TITAN titan)
	{
		fT.Add(titan);
	}

	public void addHero(HERO hero, GameObject GO)
	{
		heroes.Add(hero);
		allheroes.Add(GO);
	}

	public void addHook(Bullet h)
	{
		hooks.Add(h);
	}

	public void addTime(float time)
	{
		timeTotalServer -= time;
	}

	public static void addTitan(TITAN titan, GameObject GO)
	{
		titans.Add(titan);
		alltitans.Add(GO);
	}

	private void cache()
	{
		PView = base.photonView;
		IN_GAME_MAIN_CAMERA.mainC = Camera.main;
		IN_GAME_MAIN_CAMERA.mainT = Camera.main.transform;
		IN_GAME_MAIN_CAMERA.mainG = Camera.main.gameObject;
		IN_GAME_MAIN_CAMERA.skybox = Camera.main.GetComponent<Skybox>();
		IN_GAME_MAIN_CAMERA.mainCamera = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
		IN_GAME_MAIN_CAMERA.shake = Camera.main.GetComponent<CameraShake>();
		IN_GAME_MAIN_CAMERA.tiltshift = Camera.main.GetComponent<TiltShift>();
		IN_GAME_MAIN_CAMERA.mouselook = Camera.main.GetComponent<MouseLook>();
		IN_GAME_MAIN_CAMERA.spectate = Camera.main.GetComponent<SpectatorMovement>();
		ClothFactory.ClearClothCache();
		playersRPC.Clear();
		titanSpawners.Clear();
		groundList.Clear();
		PreservedPlayerKDR = new Dictionary<string, int[]>();
		noRestart = false;
		skyMaterial = null;
		isSpawning = false;
		retryTime = 0f;
		logicLoaded = false;
		customLevelLoaded = true;
		isUnloading = false;
		isRecompiling = false;
		FengCustomInputs.Inputs = CacheGameObject.Find<FengCustomInputs>("InputManagerController");
		LabelScoreT = (LabelScoreUI = (LabelScore = CacheGameObject.Find("LabelScore")).GetComponent<UILabel>()).transform;
		Time.timeScale = 1f;
		Camera.main.farClipPlane = RenderDistance;
		pauseWaitTime = 0f;
		spectateSprites = new List<GameObject>();
		isRestarting = false;
		if (PhotonNetwork.isMasterClient)
		{
			StartCoroutine(WaitAndResetRestarts());
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			roundTime = 0f;
			if (level.StartsWith("Custom"))
			{
				customLevelLoaded = false;
			}
			if (PhotonNetwork.isMasterClient)
			{
				if (isFirstLoad)
				{
					setGameSettings(checkGameGUI());
				}
				if (GameSettings.endlessMode > 0)
				{
                    StartCoroutine(respawnE(GameSettings.endlessMode));
				}
			}
		}
		if ((int)settings[244] != 0)
		{
			if ((int)settings[244] == 1)
			{
				InRoomChat.Write("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round Start.");
			}
			else
			{
				FleurConsole.instance.Log("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round Start.", FleurConsole.DebugType.LOG);
			}
		}
		isFirstLoad = false;
		if (instance == null || (instance != this && this != null))
		{
			instance = this;
		}
	}

	private void TranslateIncomingMessages(string text, bool PM, string sender, int ID)
	{
		int num = (int)settings[326];
		LANGUAGE lANGUAGE = (LANGUAGE)num;
		string toLang = Enum.GetName(typeof(LANGUAGE), lANGUAGE);
		Action<string, string> callback = delegate(string translation, string errors)
		{
			if (!errors.IsNullOrEmpty())
			{
				InRoomChat.Write(errors);
			}
			PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
			if (photonPlayer != null)
			{
				if (!PM)
				{
					if (sender != string.Empty)
					{
						translation = sender + " (<color=red>->" + toLang + "</color>):" + translation;
					}
					translation = "<color=#FFC000>[" + Convert.ToString(ID) + "]</color> " + translation;
					if (!ignoreList.Contains(ID) && (translation.Length > 4000 || translation.StripColor().StripHex().Length > 1000) && !photonPlayer.isLocal)
					{
						ThrowJusticeDefensive(photonPlayer, "long text spam (" + translation.Length + ")");
					}
					InRoomChat.Write(translation);
				}
				else
				{
					if (sender != string.Empty)
					{
						translation = sender + " (<color=red>->" + toLang + "</color>):" + translation;
					}
					translation = "<color=#FFC000>FROM [" + Convert.ToString(ID) + "]</color> " + translation;
					if (!ignoreList.Contains(ID) && (translation.Length > 4000 || translation.StripColor().StripHex().Length > 1000) && !photonPlayer.isLocal)
					{
						ThrowJusticeDefensive(photonPlayer, "long text spam (" + translation.Length + ")");
					}
					InRoomChat.Write(translation);
				}
			}
		};
		StartCoroutine(ThymesUtils.TranslateText(text, num, callback));
	}

    IEnumerator WaitAndTurnOffVolume()
    {
        yield return new WaitForSeconds(0.5f);
        AudioListener.volume = 0;
    }

    IEnumerator WaitAndPlayNotification()
    {
        yield return new WaitForSeconds(0.5f);
        Camera.main.gameObject.AddComponent<AudioSource>().PlayOneShot(ToClick, 1);
        StartCoroutine(WaitAndTurnOffVolume());
    }

    IEnumerator WaitAndSendAM(string msg, string name = "")
    {
        yield return new WaitForSeconds(1f);
        InRoomChat.Chat("<color=#FFFFFF> Automated Message: " + msg.Replace("{name}", name) + "</color>", RCextensions.hexColor(PhotonNetwork.player.uiname));
    }

    [RPC]
	private void Chat(string content, string sender, PhotonMessageInfo info)
	{
		string name = "";

        if (content == string.Empty || muteList.Contains(info.sender.ID))
		{
			return;
		}
		if ((int)settings[285] == 1 && !info.sender.isLocal)
		{
			if ((int)settings[282] == 1)
			{
				InRoomChat.Write("<color=red>[original]:</color> " + content);
			}
			TranslateIncomingMessages(content.StripColor() ?? "", PM: false, sender, info.sender.ID);
		}
		else
		{
			name = sender == "" ? info.sender.isLocal || info.sender.isMasterClient ? "" : content.StripColor().Split(' ')[0].Replace(":", "") : sender.StripColor();
			
			//if(!(content.StartsWith("<color=#A8FF24>Wave : ") && content.EndsWith("</color>")) && !(content.StartsWith("<color=#FFCC00>") && (content.EndsWith(" wins!</color>") || content.EndsWith(" wins.</color>"))))
			if(name != "")
			{
				AudioListener.volume = 1;
                StartCoroutine(WaitAndPlayNotification());
            }

            if (sender != string.Empty)
			{
				content = sender + ":" + content;
			}
			content = DateTime.Now.ToString("h:mm:ss") + " <color=#FFC000>[" + Convert.ToString(info.sender.ID) + "]</color> " + content;
			if (!info.sender.isLocal)
			{
				if (!ignoreList.Contains(info.sender.ID) && (content.Length > 4000 || content.StripColor().StripHex().Length > 1000))
				{
					ThrowJusticeDefensive(info.sender, "long text spam (" + content.Length + ")");
					return;
				}
				if (content.Contains("quad material") || content.Contains("quad size"))
				{
					ThrowJusticeDefensive(info.sender, "quad material");
					return;
				}
			}
			InRoomChat.Write(content);

			File.AppendAllText(Environment.CurrentDirectory + "\\Chats\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", "\n " + content.StripColor() + Environment.NewLine);

            if (content.Contains("-"))
			{
                string botTxt = Regex.Replace(content, "<color=#.*?>", "");
                botTxt = Regex.Replace(botTxt, "</color>", "");
				botTxt = botTxt.Replace(":", ": ");
				if (botTxt.Contains("-avg") || botTxt.Contains("-average") || botTxt.Contains("-kd"))
					botTxt += " " + info.sender.ID.ToString();
                InRoomChat.BotCommandInvoke(botTxt);
            }
		}



		if (am && name != "" && !info.sender.isLocal)
		{
            if (amMsgs.TryGetValue(name.ToLower(), out var msgs))
			{
				amMsgs.Remove(name.ToLower());
				amBlackList.Add(name);
				foreach (string msg in msgs)
				{
                    StartCoroutine(WaitAndSendAM(msg, name));
                }
            }
			else if (!amBlackList.Contains(name))
			{
                if (amMsgs.TryGetValue("all", out msgs))
				{
					amBlackList.Add(name);
					foreach (string msg in msgs)
					{
                        StartCoroutine(WaitAndSendAM(msg, name));
                    }
				}
			}
        }

        if (!InRoomChat.ChatInstanse.IsVisible)
		{
			InRoomChat.ChatInstanse.IsVisible = true;
		}
		InRoomChat.ChatInstanse.timeOfFade.Reset();
		InRoomChat.ChatInstanse.timeOfFade.Start();
	}

	[RPC]
	public void ChatPM(string sender, string content, PhotonMessageInfo info)
	{
		if (content == string.Empty || muteList.Contains(info.sender.ID))
		{
			return;
		}
		if ((int)settings[285] == 1 && !info.sender.isLocal)
		{
			if ((int)settings[282] == 1)
			{
				InRoomChat.Write("<color=red>[original]:</color> " + content);
			}
			TranslateIncomingMessages(content.StripColor() ?? "", PM: true, sender, info.sender.ID);
		}
		else
		{
			if (sender != string.Empty)
			{
				content = sender + ":" + content;
			}
			content = "<color=#FFC000>FROM [" + Convert.ToString(info.sender.ID) + "]</color> " + content;
			if (!ignoreList.Contains(info.sender.ID) && (content.Length > 4000 || content.StripColor().StripHex().Length > 1000) && !info.sender.isLocal)
			{
				if (!ignoreList.Contains(info.sender.ID) && (content.Length > 4000 || content.StripColor().StripHex().Length > 1000))
				{
					ThrowJusticeDefensive(info.sender, "long text spam (" + content.Length + ")");
					return;
				}
				if (content.Contains("quad material") || content.Contains("quad size"))
				{
					ThrowJusticeDefensive(info.sender, "quad material");
					return;
				}
			}
			if (GameSettings.BotPersonallyContacted > 0)
			{
				//AudioSource.PlayClipAtPoint(BotPersonallyContacted, Camera.main.transform.position, AssistantVolume);
			}
			InRoomChat.Write(content);
		}
		if (!InRoomChat.ChatInstanse.IsVisible)
		{
			InRoomChat.ChatInstanse.IsVisible = true;
		}
		InRoomChat.ChatInstanse.timeOfFade.Reset();
		InRoomChat.ChatInstanse.timeOfFade.Start();
	}

	private ExitGames.Client.Photon.Hashtable checkGameGUI()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if ((int)settings[192] > 0)
		{
			hashtable.Add("bomb", (int)settings[192]);
		}
		if ((int)settings[235] > 0)
		{
			hashtable.Add("globalDisableMinimap", (int)settings[235]);
		}
		if ((int)settings[193] > 0)
		{
			hashtable.Add("team", (int)settings[193]);
			if (GameSettings.teamMode != (int)settings[193])
			{
				int num = 1;
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					PhotonPlayer targetPlayer = PhotonNetwork.playerList[i];
					switch (num)
					{
					case 1:
						base.photonView.RPC("setTeamRPC", targetPlayer, 1);
						num = 2;
						break;
					case 2:
						base.photonView.RPC("setTeamRPC", targetPlayer, 2);
						num = 1;
						break;
					}
				}
			}
		}
		if ((int)settings[226] > 0)
		{
			int result = 50;
			if (!int.TryParse((string)settings[227], out result) || result > 1000 || result < 0)
			{
				settings[227] = "50";
			}
			hashtable.Add("point", result);
		}
		if ((int)settings[194] > 0)
		{
			hashtable.Add("rock", (int)settings[194]);
		}
		if ((int)settings[195] > 0)
		{
			int result = 30;
			if (!int.TryParse((string)settings[196], out result) || result > 100 || result < 0)
			{
				settings[196] = "30";
			}
			hashtable.Add("explode", result);
		}
		if ((int)settings[197] > 0)
		{
			int result2 = 100;
			int result3 = 200;
			if (!int.TryParse((string)settings[198], out result2) || result2 > 100000 || result2 < 0)
			{
				settings[198] = "100";
			}
			if (!int.TryParse((string)settings[199], out result3) || result3 > 100000 || result3 < 0)
			{
				settings[199] = "200";
			}
			hashtable.Add("healthMode", (int)settings[197]);
			hashtable.Add("healthLower", result2);
			hashtable.Add("healthUpper", result3);
		}
		if ((int)settings[202] > 0)
		{
			hashtable.Add("eren", (int)settings[202]);
		}
		if ((int)settings[203] > 0)
		{
			int result = 1;
			if (!int.TryParse((string)settings[204], out result) || result > 80 || result < 0)
			{
				settings[204] = "1";
			}
			hashtable.Add("titanc", result);
		}
		if ((int)settings[205] > 0)
		{
			int result = 1000;
			if (!int.TryParse((string)settings[206], out result) || result > 100000 || result < 0)
			{
				settings[206] = "1000";
			}
			hashtable.Add("damage", result);
		}
		if ((int)settings[207] > 0)
		{
			float result4 = 1f;
			float result5 = 3f;
			if (!float.TryParse((string)settings[208], out result4) || result4 > 100f || result4 < 0f)
			{
				settings[208] = "1.0";
			}
			if (!float.TryParse((string)settings[209], out result5) || result5 > 100f || result5 < 0f)
			{
				settings[209] = "3.0";
			}
			hashtable.Add("sizeMode", (int)settings[207]);
			hashtable.Add("sizeLower", result4);
			hashtable.Add("sizeUpper", result5);
		}
		if ((int)settings[210] > 0)
		{
			float result4 = 20f;
			float result5 = 20f;
			float result6 = 20f;
			float result7 = 20f;
			float result8 = 20f;
			if (!float.TryParse((string)settings[211], out result4) || !(result4 >= 0f))
			{
				settings[211] = "20.0";
			}
			if (!float.TryParse((string)settings[212], out result5) || !(result5 >= 0f))
			{
				settings[212] = "20.0";
			}
			if (!float.TryParse((string)settings[213], out result6) || !(result6 >= 0f))
			{
				settings[213] = "20.0";
			}
			if (!float.TryParse((string)settings[214], out result7) || !(result7 >= 0f))
			{
				settings[214] = "20.0";
			}
			if (!float.TryParse((string)settings[215], out result8) || !(result8 >= 0f))
			{
				settings[215] = "20.0";
			}
			if (result4 + result5 + result6 + result7 + result8 > 100f)
			{
				settings[211] = "20.0";
				settings[212] = "20.0";
				settings[213] = "20.0";
				settings[214] = "20.0";
				settings[215] = "20.0";
				result4 = 20f;
				result5 = 20f;
				result6 = 20f;
				result7 = 20f;
				result8 = 20f;
			}
			hashtable.Add("spawnMode", (int)settings[210]);
			hashtable.Add("nRate", result4);
			hashtable.Add("aRate", result5);
			hashtable.Add("jRate", result6);
			hashtable.Add("cRate", result7);
			hashtable.Add("pRate", result8);
		}
		if ((int)settings[216] > 0)
		{
			hashtable.Add("horse", (int)settings[216]);
		}
		if ((int)settings[217] > 0)
		{
			int result = 1;
			if (!int.TryParse((string)settings[218], out result) || result > 50)
			{
				settings[218] = "1";
			}
			hashtable.Add("waveModeOn", (int)settings[217]);
			hashtable.Add("waveModeNum", result);
		}
		if ((int)settings[219] > 0)
		{
			hashtable.Add("friendly", (int)settings[219]);
		}
		if ((int)settings[220] > 0)
		{
			hashtable.Add("pvp", (int)settings[220]);
		}
		if ((int)settings[221] > 0)
		{
			int result = 20;
			if (!int.TryParse((string)settings[222], out result) || result > 1000000 || result < 0)
			{
				settings[222] = "20";
			}
			hashtable.Add("maxwave", result);
		}
		if ((int)settings[223] > 0)
		{
			int result = 5;
			if (!int.TryParse((string)settings[224], out result) || result > 1000000 || result < 1)
			{
				settings[224] = "5";
			}
			hashtable.Add("endless", result);
		}
		if ((string)settings[225] != string.Empty)
		{
			hashtable.Add("motd", (string)settings[225]);
		}
		if ((int)settings[228] > 0)
		{
			hashtable.Add("ahssReload", (int)settings[228]);
		}
		if ((int)settings[229] > 0)
		{
			hashtable.Add("punkWaves", (int)settings[229]);
		}
		if ((int)settings[261] > 0)
		{
			hashtable.Add("deadlycannons", (int)settings[261]);
		}
		if (GameSettings.racingStatic > 0)
		{
			hashtable.Add("asoracing", 1);
		}
		float num2 = Convert.ToSingle(race_timer);
		hashtable.Add("racingStartTime", num2);
		if ((int)settings[302] > 0)
		{
			hashtable.Add("DrunkTitans", (int)settings[302]);
		}
		return hashtable;
	}

	private bool checkIsTitanAllDie()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<TITAN>() != null && !gameObject.GetComponent<TITAN>().hasDie)
			{
				return false;
			}
			if (gameObject.GetComponent<FEMALE_TITAN>() != null)
			{
				return false;
			}
		}
		return true;
	}

	public void checkPVPpts()
	{
		if (PVPtitanScore >= PVPtitanScoreMax)
		{
			PVPtitanScore = PVPtitanScoreMax;
			gameLose2();
		}
		else if (PVPhumanScore >= PVPhumanScoreMax)
		{
			PVPhumanScore = PVPhumanScoreMax;
			gameWin2();
		}
	}

	[RPC]
	private void clearlevel(string[] link, int gametype, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			switch (gametype)
			{
			case 0:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.KILL_TITAN;
				break;
			case 1:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.SURVIVE_MODE;
				break;
			case 2:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.PVP_AHSS;
				break;
			case 3:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.RACING;
				break;
			case 4:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.None;
				break;
			}
			if (info.sender.isMasterClient && link.Length > 6)
			{
				StartCoroutine(clearlevelE(link));
			}
			else
			{
				ThrowJusticeDefensive(info.sender, "invalid clearLevelE");
			}
		}
		else if (!info.sender.isMasterClient && !isPlayerTTL)
		{
			ThrowJusticeDefensive(info.sender, "invalid clearLevel");
		}
	}

	private IEnumerator clearlevelE(string[] skybox)
	{
		string key = skybox[6];
		bool mipmap = true;
		bool iteratorVariable11 = false;
		if ((int)settings[63] == 0)
		{
			mipmap = false;
		}
		if (skybox[0] != string.Empty || skybox[1] != string.Empty || skybox[2] != string.Empty || skybox[3] != string.Empty || skybox[4] != string.Empty || skybox[5] != string.Empty)
		{
			string iteratorVariable13 = string.Join(",", skybox);
			if (!linkHash[1].ContainsKey(iteratorVariable13))
			{
				iteratorVariable11 = true;
				Material material = IN_GAME_MAIN_CAMERA.skybox.material;
				string text = skybox[0];
				string iteratorVariable14 = skybox[1];
				string iteratorVariable15 = skybox[2];
				string iteratorVariable16 = skybox[3];
				string iteratorVariable17 = skybox[4];
				string iteratorVariable10 = skybox[5];
				if ((text.EndsWith(".jpg") || text.EndsWith(".png") || text.EndsWith(".jpeg")) && RCextensions.CheckIP(text))
				{
					WWW link7 = new WWW(text);
					yield return link7;
					Texture2D texture = RCextensions.loadimage(link7, mipmap, 500000);
					link7.Dispose();
					material.SetTexture("_FrontTex", texture);
				}
				if ((iteratorVariable14.EndsWith(".jpg") || iteratorVariable14.EndsWith(".png") || iteratorVariable14.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable14))
				{
					WWW link7 = new WWW(iteratorVariable14);
					yield return link7;
					Texture2D texture2 = RCextensions.loadimage(link7, mipmap, 500000);
					link7.Dispose();
					material.SetTexture("_BackTex", texture2);
				}
				if ((iteratorVariable15.EndsWith(".jpg") || iteratorVariable15.EndsWith(".png") || iteratorVariable15.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable15))
				{
					WWW link7 = new WWW(iteratorVariable15);
					yield return link7;
					Texture2D texture3 = RCextensions.loadimage(link7, mipmap, 500000);
					link7.Dispose();
					material.SetTexture("_LeftTex", texture3);
				}
				if ((iteratorVariable16.EndsWith(".jpg") || iteratorVariable16.EndsWith(".png") || iteratorVariable16.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable16))
				{
					WWW link7 = new WWW(iteratorVariable16);
					yield return link7;
					Texture2D texture4 = RCextensions.loadimage(link7, mipmap, 500000);
					link7.Dispose();
					material.SetTexture("_RightTex", texture4);
				}
				if ((iteratorVariable17.EndsWith(".jpg") || iteratorVariable17.EndsWith(".png") || iteratorVariable17.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable17))
				{
					WWW link7 = new WWW(iteratorVariable17);
					yield return link7;
					Texture2D texture5 = RCextensions.loadimage(link7, mipmap, 500000);
					link7.Dispose();
					material.SetTexture("_UpTex", texture5);
				}
				if ((iteratorVariable10.EndsWith(".jpg") || iteratorVariable10.EndsWith(".png") || iteratorVariable10.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable10))
				{
					WWW link7 = new WWW(iteratorVariable10);
					yield return link7;
					Texture2D texture6 = RCextensions.loadimage(link7, mipmap, 500000);
					link7.Dispose();
					material.SetTexture("_DownTex", texture6);
				}
				IN_GAME_MAIN_CAMERA.skybox.material = material;
				linkHash[1].Add(iteratorVariable13, material);
				skyMaterial = material;
			}
			else
			{
				IN_GAME_MAIN_CAMERA.skybox.material = (Material)linkHash[1][iteratorVariable13];
				skyMaterial = (Material)linkHash[1][iteratorVariable13];
			}
		}
		if (key.EndsWith(".jpg") || key.EndsWith(".png") || key.EndsWith(".jpeg"))
		{
			if (RCextensions.CheckIP(key))
			{
				foreach (GameObject ground in groundList)
				{
					if (!(ground != null) || !(ground.renderer != null))
					{
						continue;
					}
					Renderer[] componentsInChildren = ground.GetComponentsInChildren<Renderer>();
					foreach (Renderer iteratorVariable12 in componentsInChildren)
					{
						if (!linkHash[0].ContainsKey(key))
						{
							WWW link7 = new WWW(key);
							yield return link7;
							Texture2D mainTexture = RCextensions.loadimage(link7, mipmap, 200000);
							link7.Dispose();
							if (!linkHash[0].ContainsKey(key))
							{
								iteratorVariable11 = true;
								iteratorVariable12.material.mainTexture = mainTexture;
								linkHash[0].Add(key, iteratorVariable12.material);
								iteratorVariable12.material = (Material)linkHash[0][key];
							}
							else
							{
								iteratorVariable12.material = (Material)linkHash[0][key];
							}
						}
						else
						{
							iteratorVariable12.material = (Material)linkHash[0][key];
						}
					}
				}
			}
		}
		else if (key.ToLower() == "transparent")
		{
			foreach (GameObject ground2 in groundList)
			{
				if (ground2 != null && ground2.renderer != null)
				{
					Renderer[] componentsInChildren2 = ground2.GetComponentsInChildren<Renderer>();
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						componentsInChildren2[j].enabled = false;
					}
				}
			}
		}
		if (iteratorVariable11)
		{
			unloadAssets();
		}
	}

	public void compileScript(string str)
	{
		string[] array = str.Replace(" ", string.Empty).Split(new string[2] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		int num = 0;
		int num2 = 0;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == "{")
			{
				num++;
				continue;
			}
			if (array[i] == "}")
			{
				num2++;
				continue;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			string text = array[i];
			for (int j = 0; j < text.Length; j++)
			{
				switch (text[j])
				{
				case '(':
					num3++;
					break;
				case ')':
					num4++;
					break;
				case '"':
					num5++;
					break;
				}
			}
			if (num3 != num4)
			{
				InRoomChat.ChatInstanse.addLINE("Script Error: Parentheses not equal! (line " + (i + 1) + ")");
				flag = true;
			}
			if (num5 % 2 != 0)
			{
				InRoomChat.ChatInstanse.addLINE("Script Error: Quotations not equal! (line " + (i + 1) + ")");
				flag = true;
			}
		}
		if (num != num2)
		{
			InRoomChat.ChatInstanse.addLINE("Script Error: Bracket count not equivalent!");
			flag = true;
		}
		if (flag)
		{
			return;
		}
		try
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].StartsWith("On") || !(array[i + 1] == "{"))
				{
					continue;
				}
				int num6 = i;
				int num7 = i + 2;
				int num8 = 0;
				for (int k = i + 2; k < array.Length; k++)
				{
					if (array[k] == "{")
					{
						num8++;
					}
					if (array[k] == "}")
					{
						if (num8 > 0)
						{
							num8--;
							continue;
						}
						num7 = k - 1;
						k = array.Length;
					}
				}
				hashtable.Add(num6, num7);
				i = num7;
			}
			foreach (int key in hashtable.Keys)
			{
				string text2 = array[key];
				int num7 = (int)hashtable[key];
				string[] array2 = new string[num7 - key + 1];
				int num10 = 0;
				for (int i = key; i <= num7; i++)
				{
					array2[num10] = array[i];
					num10++;
				}
				RCEvent rCEvent = parseBlock(array2, 0, 0, null);
				if (text2.StartsWith("OnPlayerEnterRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
						regionTrigger.playerEventEnter = rCEvent;
						regionTrigger.myName = text3;
						RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							playerEventEnter = rCEvent,
							myName = text3
						};
						RCRegionTriggers.Add(text3, regionTrigger);
					}
					RCVariableNames.Add("OnPlayerEnterRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnPlayerLeaveRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
						regionTrigger.playerEventExit = rCEvent;
						regionTrigger.myName = text3;
						RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							playerEventExit = rCEvent,
							myName = text3
						};
						RCRegionTriggers.Add(text3, regionTrigger);
					}
					RCVariableNames.Add("OnPlayerExitRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnTitanEnterRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
						regionTrigger.titanEventEnter = rCEvent;
						regionTrigger.myName = text3;
						RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							titanEventEnter = rCEvent,
							myName = text3
						};
						RCRegionTriggers.Add(text3, regionTrigger);
					}
					RCVariableNames.Add("OnTitanEnterRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnTitanLeaveRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
						regionTrigger.titanEventExit = rCEvent;
						regionTrigger.myName = text3;
						RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							titanEventExit = rCEvent,
							myName = text3
						};
						RCRegionTriggers.Add(text3, regionTrigger);
					}
					RCVariableNames.Add("OnTitanExitRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnFirstLoad()"))
				{
					RCEvents.Add("OnFirstLoad", rCEvent);
				}
				else if (text2.StartsWith("OnRoundStart()"))
				{
					RCEvents.Add("OnRoundStart", rCEvent);
				}
				else if (text2.StartsWith("OnUpdate()"))
				{
					RCEvents.Add("OnUpdate", rCEvent);
				}
				else if (text2.StartsWith("OnTitanDie"))
				{
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					RCVariableNames.Add("OnTitanDie", array3);
					RCEvents.Add("OnTitanDie", rCEvent);
				}
				else if (text2.StartsWith("OnPlayerDieByTitan"))
				{
					RCEvents.Add("OnPlayerDieByTitan", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					RCVariableNames.Add("OnPlayerDieByTitan", array3);
				}
				else if (text2.StartsWith("OnPlayerDieByPlayer"))
				{
					RCEvents.Add("OnPlayerDieByPlayer", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					RCVariableNames.Add("OnPlayerDieByPlayer", array3);
				}
				else if (text2.StartsWith("OnChatInput"))
				{
					RCEvents.Add("OnChatInput", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string value = text2.Substring(num11 + 1, num12 - num11 - 1);
					RCVariableNames.Add("OnChatInput", value.Substring(1, value.Length - 2));
				}
			}
		}
		catch (UnityException ex)
		{
			InRoomChat.ChatInstanse.addLINE(ex.Message);
		}
	}

	public int conditionType(string str)
	{
		if (!str.StartsWith("Int"))
		{
			if (str.StartsWith("Bool"))
			{
				return 1;
			}
			if (str.StartsWith("String"))
			{
				return 2;
			}
			if (str.StartsWith("Float"))
			{
				return 3;
			}
			if (str.StartsWith("Titan"))
			{
				return 5;
			}
			if (str.StartsWith("Player"))
			{
				return 4;
			}
		}
		return 0;
	}

	private void UGH()
	{
		UpdateGameHuds(IN_GAME_MAIN_CAMERA.gamemode);
	}

	private string Pace()
	{
		string result = "";
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			if (GameSettings.Pace == 0)
			{
				float num = (float)(int)PhotonNetwork.player.customProperties["total_dmg"] / roundTime * 600f;
				result = ((num >= 1000f) ? ((num / 1000f).ToString("F0") + "k") : num.ToString("F0")) + ", " + roundTime.ToString("F0") + "s";
			}
			else if (GameSettings.Pace == 1)
			{
				result = ((float)(int)PhotonNetwork.player.customProperties["kills"] / roundTime * 600f).ToString("F0") + ", " + roundTime.ToString("F0") + "s";
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (GameSettings.Pace == 0)
			{
				float num2 = (float)single_totalDamage / roundTime * 600f;
				result = ((num2 >= 1000f) ? ((num2 / 1000f).ToString("F0") + "k") : num2.ToString("F0")) + ", " + roundTime.ToString("F0") + "s";
			}
			else if (GameSettings.Pace == 1)
			{
				result = ((float)single_kills / roundTime * 600f).ToString("F0") + ", " + roundTime.ToString("F0") + "s";
			}
		}
		return result;
	}

	public void UpdateGameHuds(GAMEMODE mode)
	{
		string text = string.Empty;
		string text2 = DateTime.Now.ToString("h:mm:ss") + "\n";
		text2 += "Time : " + ((int)timeTotalServer).ToString() + "\n";
        string empty = string.Empty;
		_ = string.Empty;
		if (!(timeElapse > 1f))
		{
			return;
		}
		timeElapse -= 1f;
		if (GameSettings.TopRightHUD > 0)
		{
			switch (mode)
			{
			case GAMEMODE.ENDLESS_TITAN:
				text = text + "Time : " + (time - (int)timeTotalServer);
				text2 = "Humanity " + humanScore + " : Titan " + titanScore + " ";
				break;
			case GAMEMODE.SURVIVE_MODE:
				text = "Titan Left: ";
				text = text + GameObject.FindGameObjectsWithTag("titan").Length.ToString() + " Wave : " + wave;
				break;
			case GAMEMODE.BOSS_FIGHT_CT:
				text = "Time : ";
				text = text + (time - (int)timeTotalServer) + "\nDefeat the Colossal Titan.\nPrevent abnormal titan from running to the north gate";
				break;
			case GAMEMODE.PVP_CAPTURE:
			{
				string text3 = "| ";
				for (int i = 0; i < PVPcheckPoint.chkPts.Count; i++)
				{
					text3 = text3 + (PVPcheckPoint.chkPts[i] as PVPcheckPoint).getStateString() + " ";
				}
				text3 += "|";
				text = string.Concat(PVPtitanScoreMax - PVPtitanScore + "  " + text3 + "  " + (PVPhumanScoreMax - PVPhumanScore) + "\n", "Time : ", (time - (int)timeTotalServer).ToString());
				break;
			}
			default:
				text = "Titan Left: ";
				text = text + GameObject.FindGameObjectsWithTag("titan").Length + "  Time : ";
				text = ((IN_GAME_MAIN_CAMERA.gametype != 0) ? (text + (time - (int)timeTotalServer)) : (text + (int)timeTotalServer));
				break;
			}
			if (GameSettings.teamMode > 0 && GameSettings.bombMode > 0)
			{
				text = text + "\nFIRST:" + Convert.ToString(cyanKills) + "       SECOND:" + Convert.ToString(magentaKills) + "[ffffff]";
			}
		}
		ShowHUDInfoTopCenter(text);
		ShowHUDInfoTopRight(text2 ?? "");
		if (GameSettings.TopRightHUD > 0 || CheatEngine)
		{
			empty = ((IN_GAME_MAIN_CAMERA.difficulty < 0) ? "Training" : ((IN_GAME_MAIN_CAMERA.difficulty == 0) ? "Normal" : ((IN_GAME_MAIN_CAMERA.difficulty != 1) ? "Abnormal" : "Hard")));
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.CAGE_FIGHT)
			{
				ShowHUDInfoTopRightMAPNAME((int)roundTime + "s\n" + level + (CheatEngine ? " - " : ": ") + empty);
			}
			else
			{
				ShowHUDInfoTopRightMAPNAME(level + (CheatEngine ? " - " : ": ") + empty);
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			if (GameSettings.TopRightHUD > 0)
			{
				string text4 = "";
				if (GameSettings.SettingsHUD == 1)
				{
					if (GameSettings.sizeMode != 0)
					{
						text4 = text4 + "\nSize [ffc700](" + GameSettings.sizeLower.ToString("F2") + "," + GameSettings.sizeUpper.ToString("F2") + ")[-]";
					}
					if (GameSettings.spawnMode != 0)
					{
						text4 = text4 + "\nRate [ffc700](" + ((GameSettings.nRate != 0f) ? (GameSettings.nRate.ToString("F0") + "N ") : "") + ((GameSettings.aRate != 0f) ? (GameSettings.aRate.ToString("F0") + "A ") : "") + ((GameSettings.jRate != 0f) ? (GameSettings.jRate.ToString("F0") + "J ") : "") + ((GameSettings.cRate != 0f) ? (GameSettings.cRate.ToString("F0") + "C ") : "") + ((GameSettings.pRate != 0f) ? (GameSettings.pRate.ToString("F0") + "P") : "") + ")[-]";
					}
					if (GameSettings.moreTitans != 0)
					{
						text4 = text4 + "\n Titans [ffc700](" + Convert.ToString(GameSettings.moreTitans) + ")[-]";
					}
					if (GameSettings.damageMode != 0)
					{
						text4 = text4 + "\n Nape [ffc700](" + Convert.ToString(GameSettings.damageMode) + ")[-]";
					}
				}
				char[] separator = new char[1] { "`"[0] };
				empty = PhotonNetwork.room.name.Split(separator)[0];
				if (empty.Length > 100)
				{
					empty = empty.Remove(99) + "...";
				}
				string region = TLWWebSocket.Region;
				ShowHUDInfoTopRightMAPNAME(text4 + "\n" + empty + "[F7D358](" + Convert.ToString(PhotonNetwork.room.playerCount) + "/" + Convert.ToString(PhotonNetwork.room.maxPlayers) + ")" + (PhotonNetwork.offlineMode ? "" : (PhotonNetwork.room.open ? "" : " Closed") + (" (" + region + ")")));
			}
			if (needChooseSide)
			{
				ShowHUDInfoTopCenterADD("\n\nPRESS 1 TO ENTER GAME");
			}
			if ((int)settings[281] > 0 && GameSettings.bombMode == 0)
			{
				ShowHUDInfoTopCenterADD(string.Format("\nDamage Feed: {0}x {1}  Highest Feed: {2}x {3}", feed_count, feed_number.ToString("#,##0"), highest_feed_count, highest_feed.ToString("#,##0")));
			}
		}
		if (GameSettings.bombMode == 0 && GameSettings.Pace != 2)
		{
			ShowHUDInfoTopCenterADD("\n" + Pace());
		}
	}

	public void ShowGameResult()
	{
		IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
		gameStart = false;
		Screen.lockCursor = false;
		Screen.showCursor = true;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = string.Empty;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			ExitGames.Client.Photon.Hashtable customProperties;
			if (photonPlayer != null && (customProperties = photonPlayer.customProperties) != null)
			{
				text = string.Concat(text, customProperties["name"], "\n");
				text2 = string.Concat(text2, customProperties["kills"], "\n");
				text3 = string.Concat(text3, customProperties["deaths"], "\n");
				text4 = string.Concat(text4, customProperties["max_dmg"], "\n");
				text5 = string.Concat(text5, customProperties["total_dmg"], "\n");
			}
		}
		string text6;
		if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_AHSS)
		{
			text6 = ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.SURVIVE_MODE) ? ("Humanity " + humanScore + " : Titan " + titanScore) : ("Highest Wave : " + highestwave));
		}
		else
		{
			text6 = string.Empty;
			for (int j = 0; j < teamScores.Length; j++)
			{
				text6 += ((j == 0) ? ("Team" + (j + 1) + " " + teamScores[j] + " ") : " : ");
			}
		}
		_ = new object[6] { text, text2, text3, text4, text5, text6 };
	}

	public void Core(GAMETYPE game_type)
	{
		if ((int)settings[64] >= 100)
		{
			coreeditor();
			return;
		}
		timeElapse += Time.deltaTime;
		roundTime += Time.deltaTime;
		switch (game_type)
		{
		case GAMETYPE.SINGLE:
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
			{
				if (!isLosing)
				{
					currentSpeed = IN_GAME_MAIN_CAMERA.main_objectR.velocity.magnitude;
					maxSpeed = Mathf.Max(maxSpeed, currentSpeed);
					ShowHUDInfoTopLeft("Current Speed : " + (int)currentSpeed + "\nMax Speed:" + maxSpeed);
				}
			}
			else
			{
				ShowHUDInfoTopLeft("Kills:" + single_kills + "\nMax Damage:" + single_maxDamage + "\nTotal Damage:" + single_totalDamage);
			}
			if (isLosing && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					ShowHUDInfoCenter("Survive " + wave + " Waves!\n Press " + FengCustomInputs.Inputs.inputString[InputCode.restart] + " to Restart.\n\n\n");
				}
				else
				{
					ShowHUDInfoCenter("Humanity Fail!\n Press " + FengCustomInputs.Inputs.inputString[InputCode.restart] + " to Restart.\n\n\n");
				}
			}
			if (isWinning)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
				{
					ShowHUDInfoCenter((timeTotalServer * 10f * 0.1f - Convert.ToSingle(race_timer)).ToString("N2") + "s !\n Press " + FengCustomInputs.Inputs.inputString[InputCode.restart] + " to Restart.\n\n\n");
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					ShowHUDInfoCenter("Survive All Waves!\n Press " + FengCustomInputs.Inputs.inputString[InputCode.restart] + " to Restart.\n\n\n");
				}
				else
				{
					ShowHUDInfoCenter("Humanity Win!\n Press " + FengCustomInputs.Inputs.inputString[InputCode.restart] + " to Restart.\n\n\n");
				}
			}
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
			{
				if (!isWinning)
				{
					timeTotalServer += Time.deltaTime;
				}
				if (roundTime < Convert.ToSingle(race_timer))
				{
					ShowHUDInfoCenter("RACE STARTS IN " + (Convert.ToSingle(race_timer) - roundTime).ToString("N2"));
				}
				else
				{
					ShowHUDInfoTopCenter("Time : " + (roundTime - Convert.ToSingle(race_timer)).ToString("N2"));
					if (!startRacing)
					{
						ShowHUDInfoCenter(string.Empty);
						startRacing = true;
						endRacing = false;
						Door = CacheGameObject.Find("door");
						Door.SetActive(value: false);
					}
				}
				if (!Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver || justSuicide || !customLevelLoaded)
				{
					break;
				}
				myRespawnTime += Time.deltaTime;
				if (myRespawnTime > 1.5f)
				{
					myRespawnTime = 0f;
					IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
					if (checkpoint != null)
					{
						StartCoroutine(WaitAndRespawn2(0.1f, checkpoint));
					}
					else
					{
						StartCoroutine(WaitAndRespawn1(0.1f, myLastRespawnTag));
					}
					IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
					ShowHUDInfoCenter(string.Empty);
				}
			}
			else if (!isLosing && !isWinning)
			{
				timeTotalServer += Time.deltaTime;
			}
			break;
		case GAMETYPE.MULTIPLAYER:
			timeTotalServer += Time.deltaTime;
			if (needChooseSide)
			{
				if (FengCustomInputs.Inputs.isInputDown[InputCode.flare1])
				{
					if (CharacterSelection.characterSelectionIsVisible)
					{
						Screen.lockCursor = true;
						Screen.showCursor = true;
						NGUITools.SetActive(uirefer.panels[0], state: true);
						NGUITools.SetActive(uirefer.panels[1], state: false);
						NGUITools.SetActive(uirefer.panels[2], state: false);
						CharacterSelection.characterSelectionIsVisible = false;
						Camera.main.GetComponent<SpectatorMovement>().disable = false;
						Camera.main.GetComponent<MouseLook>().disable = false;
					}
					else
					{
						Screen.lockCursor = false;
						Screen.showCursor = true;
						NGUITools.SetActive(uirefer.panels[0], state: false);
						NGUITools.SetActive(uirefer.panels[1], state: false);
						NGUITools.SetActive(uirefer.panels[2], state: false);
						CharacterSelection.characterSelectionIsVisible = true;
						Camera.main.GetComponent<SpectatorMovement>().disable = true;
						Camera.main.GetComponent<MouseLook>().disable = true;
						CharacterSelection.AutoLoadCamera();
					}
				}
				if (FengCustomInputs.Inputs.isInputDown[15] && !FengCustomInputs.Inputs.menuOn)
				{
					Screen.showCursor = true;
					Screen.lockCursor = false;
					Camera.main.GetComponent<SpectatorMovement>().disable = true;
					Camera.main.GetComponent<MouseLook>().disable = true;
					FengCustomInputs.Inputs.menuOn = true;
				}
			}
			coreadd();
			if (InsideSpecmode && GameObject.FindGameObjectsWithTag("Player").Length != 0 && IN_GAME_MAIN_CAMERA.main_objectR != null)
			{
				currentSpeed = IN_GAME_MAIN_CAMERA.main_objectR.velocity.magnitude.RoundTo(0);
				string content = (((int)settings[189] == 1) ? (currentSpeed + " u/s \n") : ((currentSpeed / 100f).ToString("0.00") + "K \n"));
				ShowHUDInfoCenter(content);
			}
			if (Camera.main != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING && IN_GAME_MAIN_CAMERA.mainCamera.gameOver && (int)settings[245] == 0)
			{
				if (GameObject.FindGameObjectsWithTag("Player").Length != 0 && IN_GAME_MAIN_CAMERA.main_objectR != null)
				{
					currentSpeed = IN_GAME_MAIN_CAMERA.main_objectR.velocity.magnitude.RoundTo(0);
					string content2 = (((int)settings[189] == 1) ? (currentSpeed + " u/s \n") : ((currentSpeed / 100f).ToString("0.00") + "K \n"));
					ShowHUDInfoCenter(content2);
				}
				if ((LevelInfo.getInfo(level).respawnMode == RespawnMode.DEATHMATCH || GameSettings.endlessMode > 0 || ((GameSettings.bombMode == 1 || GameSettings.pvpMode > 0) && GameSettings.pointMode > 0)) && !justSuicide)
				{
					ShowHUDInfoCenter("");
					myRespawnTime += Time.deltaTime;
					int num = 5;
					if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) == 2)
					{
						num = 10;
					}
					if (GameSettings.endlessMode > 0)
					{
						num = GameSettings.endlessMode;
					}
					ShowHUDInfoCenterADD("Respawn in " + (num - (int)myRespawnTime) + "s.");
					if (myRespawnTime > (float)num)
					{
						myRespawnTime = 0f;
						IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
						if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) == 2)
						{
							SpawnNonAITitan2(myLastHero);
						}
						else
						{
                            needChooseSide = false;
                            justSuicide = false;
                            StartCoroutine(WaitAndRespawn1(0.1f, myLastRespawnTag));
						}
						IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
						ShowHUDInfoCenter(string.Empty);
					}
				}
			}
			if (isLosing && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					ShowHUDInfoCenter("Survive " + wave + " Waves!\nGame Restart in " + (int)gameEndCD + "s\n\n");
                }
				else
				{
					ShowHUDInfoCenter("Humanity Fail!\nAgain!\nGame Restart in " + (int)gameEndCD + "s\n\n");
                }
				if (gameEndCD <= 0f)
				{
					gameEndCD = 0f;
					if (PhotonNetwork.isMasterClient)
					{
						restartRC();
					}
					ShowHUDInfoCenter(string.Empty);
				}
				else
				{
					gameEndCD -= Time.deltaTime;
				}
			}
			if (isWinning)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
				{
					ShowHUDInfoCenter(localRacingResult + "\n\nGame Restart in " + (int)gameEndCD + "s");
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					ShowHUDInfoCenter("Survive All Waves!\nGame Restart in " + (int)gameEndCD + "s\n\n");
                }
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
				{
					if (GameSettings.pvpMode == 0 && GameSettings.bombMode == 0)
					{
						ShowHUDInfoCenter("Team " + teamWinner + " Win!\nGame Restart in " + (int)gameEndCD + "s\n\n");
					}
					else
					{
						ShowHUDInfoCenter(string.Concat(new object[3]
						{
							"Round Ended!\nGame Restart in ",
							(int)gameEndCD,
							"s\n\n"
						}));
					}
				}
				else
				{
					ShowHUDInfoCenter("Humanity Win!\nGame Restart in " + (int)gameEndCD + "s\n\n");
				}
				if (gameEndCD <= 0f)
				{
					gameEndCD = 0f;
					if (PhotonNetwork.isMasterClient)
					{
						restartRC();
					}
					ShowHUDInfoCenter(string.Empty);
				}
				else
				{
					gameEndCD -= Time.deltaTime;
				}
			}
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
			{
				if (roundTime > Convert.ToSingle(race_timer))
				{
					ShowHUDInfoTopCenter("Time : " + (roundTime - Convert.ToSingle(race_timer)).ToString("N2"));
					if (!startRacing)
					{
						ShowHUDInfoCenter(string.Empty);
						startRacing = true;
						endRacing = false;
						Door = CacheGameObject.Find("door");
						Door.SetActive(value: false);
						if (racingDoors != null && customLevelLoaded)
						{
							foreach (GameObject racingDoor in racingDoors)
							{
								racingDoor.SetActive(value: false);
							}
							racingDoors = null;
						}
					}
					else if (racingDoors != null && customLevelLoaded)
					{
						foreach (GameObject racingDoor2 in racingDoors)
						{
							racingDoor2.SetActive(value: false);
						}
						racingDoors = null;
					}
				}
				else
				{
					ShowHUDInfoCenter("RACE STARTS IN " + (Convert.ToSingle(race_timer) - roundTime).ToString("N2") + ((!(localRacingResult == string.Empty)) ? ("\nLast Round\n" + localRacingResult) : "\n\n"));
				}
				if (Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !justSuicide && customLevelLoaded)
				{
					myRespawnTime += Time.deltaTime;
					if (myRespawnTime > 1.5f)
					{
						myRespawnTime = 0f;
						IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
						if (checkpoint != null)
						{
							StartCoroutine(WaitAndRespawn2(0.1f, checkpoint));
						}
						else
						{
							StartCoroutine(WaitAndRespawn1(0.1f, myLastRespawnTag));
						}
						IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
						ShowHUDInfoCenter(string.Empty);
					}
				}
			}
			if (killInfoGO.Count > 0 && killInfoGO[0] == null)
			{
				killInfoGO.RemoveAt(0);
			}
			if (PhotonNetwork.isMasterClient && timeTotalServer > (float)time)
			{
				ShowGameResult();
			}
			break;
		}
	}

	private string TOPRIGHTSETTINGS()
	{
		if (GameSettings.TopRightHUD == 0)
		{
			return "";
		}
		string text = ((IN_GAME_MAIN_CAMERA.difficulty < 0) ? "Training" : ((IN_GAME_MAIN_CAMERA.difficulty == 0) ? "Normal" : ((IN_GAME_MAIN_CAMERA.difficulty != 1) ? "Abnormal" : "Hard")));
		string text2 = string.Empty;
		int waveModeNum = GameSettings.waveModeNum;
		if (GameSettings.waveModeOn > 0)
		{
			object obj = text2;
			text2 = string.Concat(obj, "\n", waveModeNum, " WAVES");
		}
		int pointMode = GameSettings.pointMode;
		if (GameSettings.pointMode > 0)
		{
			object obj2 = text2;
			text2 = string.Concat(obj2, "\nPoint Mode [ffc700](", pointMode, ")[-]");
		}
		char[] separator = new char[1] { "`"[0] };
		string text3 = "\n" + PhotonNetwork.room.name.Split(separator)[0];
		if (text3.Length > 20)
		{
			text3 = text3.Remove(19) + "...";
		}
		string text4 = "[ffc700](" + PhotonNetwork.room.playerCount + "/" + PhotonNetwork.room.maxPlayers + ")[-][-]";
		return string.Concat(new string[4] { text, text3, text4, text2 });
	}

	private void coreadd()
	{
		if (PhotonNetwork.isMasterClient)
		{
			OnUpdate();
			if (customLevelLoaded)
			{
				for (int i = 0; i < titanSpawners.Count; i++)
				{
					TitanSpawner titanSpawner = titanSpawners[i];
					titanSpawner.time -= Time.deltaTime;
					if (!(titanSpawner.time <= 0f) || titans.Count + fT.Count >= Math.Min(GameSettings.titanCap, 80))
					{
						continue;
					}
					string text = titanSpawner.name;
					if (text == "spawnAnnie")
					{
						PhotonNetwork.Instantiate("FEMALE_TITAN", titanSpawner.location, new Quaternion(0f, 0f, 0f, 1f), 0);
					}
					else
					{
						GameObject gameObject = PhotonNetwork.Instantiate("TITAN_VER3.1", titanSpawner.location, new Quaternion(0f, 0f, 0f, 1f), 0);
						switch (text)
						{
						case "spawnAbnormal":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
							break;
						case "spawnJumper":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
							break;
						case "spawnCrawler":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
							break;
						case "spawnPunk":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
							break;
						}
					}
					if (titanSpawner.endless)
					{
						titanSpawner.time = titanSpawner.delay;
					}
					else
					{
						titanSpawners.Remove(titanSpawner);
					}
				}
			}
		}
		if (Time.timeScale <= 0.1f && pauseWaitTime <= 3f)
		{
			pauseWaitTime -= Time.deltaTime * 1000000f;
			if (pauseWaitTime <= 1f)
			{
				Camera.main.farClipPlane = RenderDistance;
			}
			if (pauseWaitTime <= 0f)
			{
				pauseWaitTime = 0f;
				Time.timeScale = 1f;
			}
		}
	}

	private void coreeditor()
	{
		if (Input.GetKey(KeyCode.Tab))
		{
			GUI.FocusControl(null);
		}
		if (selectedObj != null)
		{
			float num = 0.2f;
			if (inputRC.isInputLevel(InputCodeRC.levelSlow))
			{
				num = 0.04f;
			}
			else if (inputRC.isInputLevel(InputCodeRC.levelFast))
			{
				num = 0.6f;
			}
			if (inputRC.isInputLevel(InputCodeRC.levelForward))
			{
				selectedObj.transform.position += num * new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
			}
			else if (inputRC.isInputLevel(InputCodeRC.levelBack))
			{
				selectedObj.transform.position -= num * new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
			}
			if (inputRC.isInputLevel(InputCodeRC.levelLeft))
			{
				selectedObj.transform.position -= num * new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
			}
			else if (inputRC.isInputLevel(InputCodeRC.levelRight))
			{
				selectedObj.transform.position += num * new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
			}
			if (inputRC.isInputLevel(InputCodeRC.levelDown))
			{
				selectedObj.transform.position -= Vector3.up * num;
			}
			else if (inputRC.isInputLevel(InputCodeRC.levelUp))
			{
				selectedObj.transform.position += Vector3.up * num;
			}
			if (!selectedObj.name.StartsWith("misc,region"))
			{
				if (inputRC.isInputLevel(InputCodeRC.levelRRight))
				{
					selectedObj.transform.Rotate(Vector3.up * num);
				}
				else if (inputRC.isInputLevel(InputCodeRC.levelRLeft))
				{
					selectedObj.transform.Rotate(Vector3.down * num);
				}
				if (inputRC.isInputLevel(InputCodeRC.levelRCCW))
				{
					selectedObj.transform.Rotate(Vector3.forward * num);
				}
				else if (inputRC.isInputLevel(InputCodeRC.levelRCW))
				{
					selectedObj.transform.Rotate(Vector3.back * num);
				}
				if (inputRC.isInputLevel(InputCodeRC.levelRBack))
				{
					selectedObj.transform.Rotate(Vector3.left * num);
				}
				else if (inputRC.isInputLevel(InputCodeRC.levelRForward))
				{
					selectedObj.transform.Rotate(Vector3.right * num);
				}
			}
			if (inputRC.isInputLevel(InputCodeRC.levelPlace))
			{
				linkHash[3].Add(selectedObj.GetInstanceID(), selectedObj.name + "," + Convert.ToString(selectedObj.transform.position.x) + "," + Convert.ToString(selectedObj.transform.position.y) + "," + Convert.ToString(selectedObj.transform.position.z) + "," + Convert.ToString(selectedObj.transform.rotation.x) + "," + Convert.ToString(selectedObj.transform.rotation.y) + "," + Convert.ToString(selectedObj.transform.rotation.z) + "," + Convert.ToString(selectedObj.transform.rotation.w));
				selectedObj = null;
				Camera.main.GetComponent<MouseLook>().enabled = true;
				Screen.lockCursor = true;
			}
			if (inputRC.isInputLevel(InputCodeRC.levelDelete))
			{
				UnityEngine.Object.Destroy(selectedObj);
				selectedObj = null;
				Camera.main.GetComponent<MouseLook>().enabled = true;
				Screen.lockCursor = true;
				linkHash[3].Remove(selectedObj.GetInstanceID());
			}
			return;
		}
		if (Screen.lockCursor)
		{
			float num2 = 100f;
			if (inputRC.isInputLevel(InputCodeRC.levelSlow))
			{
				num2 = 20f;
			}
			else if (inputRC.isInputLevel(InputCodeRC.levelFast))
			{
				num2 = 400f;
			}
			Transform mainT = IN_GAME_MAIN_CAMERA.mainT;
			if (inputRC.isInputLevel(InputCodeRC.levelForward))
			{
				mainT.position += mainT.forward * num2 * Time.deltaTime;
			}
			else if (inputRC.isInputLevel(InputCodeRC.levelBack))
			{
				mainT.position -= mainT.forward * num2 * Time.deltaTime;
			}
			if (inputRC.isInputLevel(InputCodeRC.levelLeft))
			{
				mainT.position -= mainT.right * num2 * Time.deltaTime;
			}
			else if (inputRC.isInputLevel(InputCodeRC.levelRight))
			{
				mainT.position += mainT.right * num2 * Time.deltaTime;
			}
			if (inputRC.isInputLevel(InputCodeRC.levelUp))
			{
				mainT.position += mainT.up * num2 * Time.deltaTime;
			}
			else if (inputRC.isInputLevel(InputCodeRC.levelDown))
			{
				mainT.position -= mainT.up * num2 * Time.deltaTime;
			}
		}
		if (inputRC.isInputLevelDown(InputCodeRC.levelCursor))
		{
			if (Screen.lockCursor)
			{
				IN_GAME_MAIN_CAMERA.mouselook.enabled = false;
				Screen.lockCursor = false;
			}
			else
			{
				IN_GAME_MAIN_CAMERA.mouselook.enabled = true;
				Screen.lockCursor = true;
			}
		}
		if (!Input.GetKeyDown(KeyCode.Mouse0) || Screen.lockCursor || GUIUtility.hotControl != 0 || ((!(Input.mousePosition.x > 300f) || !(Input.mousePosition.x < (float)Screen.width - 300f)) && !((float)Screen.height - Input.mousePosition.y > 600f)))
		{
			return;
		}
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
		{
			Transform transform = hitInfo.transform;
			if (transform.gameObject.name.StartsWith("custom") || transform.gameObject.name.StartsWith("base") || transform.gameObject.name.StartsWith("racing") || transform.gameObject.name.StartsWith("photon") || transform.gameObject.name.StartsWith("spawnpoint") || transform.gameObject.name.StartsWith("misc"))
			{
				selectedObj = transform.gameObject;
				IN_GAME_MAIN_CAMERA.mouselook.enabled = false;
				Screen.lockCursor = true;
				linkHash[3].Remove(selectedObj.GetInstanceID());
			}
			else if (transform.parent.gameObject.name.StartsWith("custom") || transform.parent.gameObject.name.StartsWith("base") || transform.parent.gameObject.name.StartsWith("racing") || transform.parent.gameObject.name.StartsWith("photon"))
			{
				selectedObj = transform.parent.gameObject;
				IN_GAME_MAIN_CAMERA.mouselook.enabled = false;
				Screen.lockCursor = true;
				linkHash[3].Remove(selectedObj.GetInstanceID());
			}
		}
	}

	private IEnumerator customlevelcache()
	{
		for (int i = 0; i < levelCache.Count; i++)
		{
			customlevelclientE(levelCache[i], renewHash: false);
			yield return new WaitForEndOfFrame();
		}
		batchViaUnity();
	}

	private void batchViaUnity()
	{
		if (batchableObjects.Count > 0)
		{
			GameObject gameObject = new GameObject("BatchedItems");
			foreach (GameObject batchableObject in batchableObjects)
			{
				batchableObject.isStatic = true;
				batchableObject.transform.parent = gameObject.transform;
			}
			StaticBatchingUtility.Combine(gameObject);
		}
		batchableObjects.Clear();
	}

	public bool hasAnarchyScript(string[] data)
	{
		if (!CustomAnarchyLevel.Instance.GameManager.GetLevelName().StartsWith("Custom-Anarchy"))
		{
			return false;
		}
		int num = ((data[0] == "misc" || data[0] == "racing") ? 12 : 19);
		if (data.Length <= num)
		{
			return false;
		}
		string text = string.Join(",", data, num, data.Length - num);
		if (text.StartsWith("rmo"))
		{
			return true;
		}
		if (text.StartsWith("umv"))
		{
			return true;
		}
		if (text.StartsWith("rts"))
		{
			return true;
		}
		if (text.Contains("clip"))
		{
			return true;
		}
		if (text.Contains("nokill") && data[0] == "racing")
		{
			return true;
		}
		if (text.Contains("trap"))
		{
			return true;
		}
		if (text.Contains("parent"))
		{
			return true;
		}
		return false;
	}

	private void customlevelclientE(string[] content, bool renewHash)
	{
		bool flag = false;
		bool flag2 = false;
		if (content[content.Length - 1].StartsWith("a"))
		{
			flag = true;
		}
		else if (content[content.Length - 1].StartsWith("z"))
		{
			flag2 = true;
			customLevelLoaded = true;
			spawnPlayerCustomMap();
			unloadAssets();
			Camera.main.GetComponent<TiltShift>().enabled = false;
		}
		if (renewHash)
		{
			if (flag)
			{
				currentLevel = string.Empty;
				levelCache.Clear();
				titanSpawns.Clear();
				playerSpawnsC.Clear();
				playerSpawnsM.Clear();
				for (int i = 0; i < content.Length; i++)
				{
					string[] array = content[i].Split(',');
					if (array[0] == "titan")
					{
						titanSpawns.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
					else if (array[0] == "playerC")
					{
						playerSpawnsC.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
					else if (array[0] == "playerM")
					{
						playerSpawnsM.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
				}
				spawnPlayerCustomMap();
			}
			currentLevel += content[content.Length - 1];
			levelCache.Add(content);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("currentLevel", currentLevel);
			PhotonNetwork.player.SetCustomProperties(hashtable);
		}
		if (flag || flag2)
		{
			return;
		}
		for (int i = 0; i < content.Length; i++)
		{
			GameObject gameObject = null;
			string[] array = content[i].Split(',');
			float result;
			if (array[0].StartsWith("custom"))
			{
				float a = 1f;
				GameObject gameObject2 = null;
				gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])));
				if (array[2] != "default")
				{
					if (array[2].StartsWith("transparent"))
					{
						if (float.TryParse(array[2].Substring(11), out result))
						{
							a = result;
						}
						Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer in componentsInChildren)
						{
							renderer.material = (Material)RCassets.Load("transparent");
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array[11]));
							}
						}
					}
					else
					{
						Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer2 in componentsInChildren)
						{
							renderer2.material = (Material)RCassets.Load(array[2]);
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer2.material.mainTextureScale.y * Convert.ToSingle(array[11]));
							}
						}
					}
				}
				float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[3]);
				num -= 0.001f;
				float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[4]);
				float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[5]);
				gameObject2.transform.localScale = new Vector3(num, y, z);
				if (array[6] != "0")
				{
					Color color = new Color(Convert.ToSingle(array[7]), Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), a);
					MeshFilter[] componentsInChildren2 = gameObject2.GetComponentsInChildren<MeshFilter>();
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						Mesh mesh = componentsInChildren2[j].mesh;
						Color[] array2 = new Color[mesh.vertexCount];
						for (int k = 0; k < mesh.vertexCount; k++)
						{
							array2[k] = color;
						}
						mesh.colors = array2;
					}
				}
				gameObject = gameObject2;
			}
			else if (array[0].StartsWith("base"))
			{
				if (array.Length < 15)
				{
					gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(array[1]), new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])));
				}
				else
				{
					float a = 1f;
					GameObject gameObject2 = null;
					gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array[1]), new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])));
					if (array[2] != "default")
					{
						if (array[2].StartsWith("transparent"))
						{
							if (float.TryParse(array[2].Substring(11), out result))
							{
								a = result;
							}
							Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
							foreach (Renderer renderer3 in componentsInChildren)
							{
								renderer3.material = (Material)RCassets.Load("transparent");
								if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
								{
									renderer3.material.mainTextureScale = new Vector2(renderer3.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer3.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								}
							}
						}
						else
						{
							Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
							foreach (Renderer renderer4 in componentsInChildren)
							{
								if (!renderer4.name.Contains("Particle System") || !gameObject2.name.Contains("aot_supply"))
								{
									renderer4.material = (Material)RCassets.Load(array[2]);
									if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
									{
										renderer4.material.mainTextureScale = new Vector2(renderer4.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer4.material.mainTextureScale.y * Convert.ToSingle(array[11]));
									}
								}
							}
						}
					}
					float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[3]);
					num -= 0.001f;
					float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[4]);
					float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[5]);
					gameObject2.transform.localScale = new Vector3(num, y, z);
					if (array[6] != "0")
					{
						Color color = new Color(Convert.ToSingle(array[7]), Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), a);
						MeshFilter[] componentsInChildren2 = gameObject2.GetComponentsInChildren<MeshFilter>();
						for (int j = 0; j < componentsInChildren2.Length; j++)
						{
							Mesh mesh = componentsInChildren2[j].mesh;
							Color[] array2 = new Color[mesh.vertexCount];
							for (int k = 0; k < mesh.vertexCount; k++)
							{
								array2[k] = color;
							}
							mesh.colors = array2;
						}
					}
					gameObject = gameObject2;
				}
			}
			else if (array[0].StartsWith("misc"))
			{
				if (array[1].StartsWith("barrier"))
				{
					GameObject gameObject2 = null;
					gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject2.transform.localScale = new Vector3(num, y, z);
					gameObject = gameObject2;
				}
				else if (array[1].StartsWith("racingStart"))
				{
					GameObject gameObject2 = null;
					gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject2.transform.localScale = new Vector3(num, y, z);
					if (racingDoors != null)
					{
						racingDoors.Add(gameObject2);
					}
					gameObject = gameObject2;
				}
				else if (array[1].StartsWith("racingEnd"))
				{
					GameObject gameObject2 = null;
					gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject2.transform.localScale = new Vector3(num, y, z);
					gameObject2.AddComponent<LevelTriggerRacingEnd>();
					gameObject = gameObject2;
				}
				else if (array[1].StartsWith("region") && PhotonNetwork.isMasterClient)
				{
					Vector3 vector = new Vector3(Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8]));
					RCRegion rCRegion = new RCRegion(vector, Convert.ToSingle(array[3]), Convert.ToSingle(array[4]), Convert.ToSingle(array[5]));
					string key = array[2];
					if (RCRegionTriggers.ContainsKey(key))
					{
						GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load("region"));
						gameObject3.transform.position = vector;
						gameObject3.AddComponent<RegionTrigger>();
						gameObject3.GetComponent<RegionTrigger>().CopyTrigger((RegionTrigger)RCRegionTriggers[key]);
						float num = gameObject3.transform.localScale.x * Convert.ToSingle(array[3]);
						num -= 0.001f;
						float y = gameObject3.transform.localScale.y * Convert.ToSingle(array[4]);
						float z = gameObject3.transform.localScale.z * Convert.ToSingle(array[5]);
						gameObject3.transform.localScale = new Vector3(num, y, z);
						rCRegion.myBox = gameObject3;
					}
					RCRegions.Add(key, rCRegion);
				}
			}
			else if (array[0].StartsWith("racing"))
			{
				if (array[1].StartsWith("start"))
				{
					GameObject gameObject2 = null;
					gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject2.transform.localScale = new Vector3(num, y, z);
					if (racingDoors != null)
					{
						racingDoors.Add(gameObject2);
					}
					gameObject = gameObject2;
				}
				else if (array[1].StartsWith("end"))
				{
					GameObject gameObject2 = null;
					gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject2.transform.localScale = new Vector3(num, y, z);
					gameObject2.GetComponentInChildren<Collider>().gameObject.AddComponent<LevelTriggerRacingEnd>();
					gameObject = gameObject2;
				}
				else if (array[1].StartsWith("kill"))
				{
					GameObject gameObject2 = null;
					gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject2.transform.localScale = new Vector3(num, y, z);
					gameObject2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingKillTrigger>();
					gameObject = gameObject2;
				}
				else if (array[1].StartsWith("checkpoint"))
				{
					GameObject gameObject2 = null;
					gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject2.transform.localScale = new Vector3(num, y, z);
					gameObject2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingCheckpointTrigger>();
					gameObject = gameObject2;
				}
			}
			else if (array[0].StartsWith("map"))
			{
				if (array[1].StartsWith("disablebounds"))
				{
					UnityEngine.Object.Destroy(GameObject.Find("gameobjectOutSide"));
					UnityEngine.Object.Instantiate(RCassets.Load("outside"));
				}
			}
			else if (PhotonNetwork.isMasterClient && array[0].StartsWith("photon"))
			{
				if (array[1].StartsWith("Cannon"))
				{
					if (array.Length > 15)
					{
						GameObject obj = PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])), 0);
						obj.GetComponent<CannonPropRegion>().settings = content[i];
						obj.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, content[i]);
					}
					else
					{
						PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])), 0).GetComponent<CannonPropRegion>().settings = content[i];
					}
				}
				else
				{
					TitanSpawner titanSpawner = new TitanSpawner();
					float num = 30f;
					if (float.TryParse(array[2], out result))
					{
						num = Mathf.Max(Convert.ToSingle(array[2]), 1f);
					}
					titanSpawner.time = num;
					titanSpawner.delay = num;
					titanSpawner.name = array[1];
					if (array[3] == "1")
					{
						titanSpawner.endless = true;
					}
					else
					{
						titanSpawner.endless = false;
					}
					titanSpawner.location = new Vector3(Convert.ToSingle(array[4]), Convert.ToSingle(array[5]), Convert.ToSingle(array[6]));
					titanSpawners.Add(titanSpawner);
				}
			}
			if (gameObject != null)
			{
				if (hasAnarchyScript(array))
				{
					CustomAnarchyLevel.Instance.TryAddAnarchyScripts(gameObject, array);
				}
				else
				{
					batchableObjects.Add(gameObject);
				}
			}
		}
	}

	private IEnumerator customlevelE(List<PhotonPlayer> players)
	{
		string[] array;
		if (!(currentLevel == string.Empty))
		{
			for (int i = 0; i < levelCache.Count; i++)
			{
				foreach (PhotonPlayer player in players)
				{
					if (player.customProperties["currentLevel"] != null && currentLevel != string.Empty && RCextensions.returnStringFromObject(player.customProperties["currentLevel"]) == currentLevel)
					{
						if (i == 0)
						{
							array = new string[1] { "loadcached" };
							base.photonView.RPC("customlevelRPC", player, new object[1] { array });
						}
					}
					else
					{
						base.photonView.RPC("customlevelRPC", player, new object[1] { levelCache[i] });
					}
				}
				if (i > 0)
				{
					yield return new WaitForSeconds(0.75f);
				}
				else
				{
					yield return new WaitForSeconds(0.25f);
				}
			}
			yield break;
		}
		array = new string[1] { "loadempty" };
		foreach (PhotonPlayer player2 in players)
		{
			base.photonView.RPC("customlevelRPC", player2, new object[1] { array });
		}
		customLevelLoaded = true;
	}

	[RPC]
	private void customlevelRPC(string[] content, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			if (content.Length == 1 && content[0] == "loadcached")
			{
				StartCoroutine(customlevelcache());
			}
			else if (content.Length == 1 && content[0] == "loadempty")
			{
				currentLevel = string.Empty;
				levelCache.Clear();
				titanSpawns.Clear();
				playerSpawnsC.Clear();
				playerSpawnsM.Clear();
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add("currentLevel", currentLevel);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				customLevelLoaded = true;
				spawnPlayerCustomMap();
			}
			else
			{
				customlevelclientE(content, renewHash: true);
			}
		}
	}

	public void DestroyAllExistingCloths()
	{
		Cloth[] array = UnityEngine.Object.FindObjectsOfType<Cloth>();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				ClothFactory.DisposeObject(array[i].gameObject);
			}
		}
	}

	private void endGameRC()
	{
		if (GameSettings.pointMode > 0)
		{
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonNetwork.playerList[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable
				{
					{ "kills", 0 },
					{ "deaths", 0 },
					{ "max_dmg", 0 },
					{ "total_dmg", 0 }
				});
			}
		}
		gameEndCD = 0f;
		restartGame2(masterclientSwitched: false);
	}

	public void BottomHUD(bool enabled)
	{
		if (!enabled)
		{
			string[] array = new string[2] { "Flare", "LabelInfoBottomRight" };
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = CacheGameObject.Find(array[i]);
				if (gameObject != null)
				{
					if (!UIObjects.Contains(gameObject))
					{
						UIObjects.Add(gameObject);
					}
					gameObject.SetActive(value: false);
				}
			}
			UnityEngine.Object[] array2 = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject2 = (GameObject)array2[i];
				if (!(gameObject2.GetComponent<UISprite>() != null) || !gameObject2.activeInHierarchy)
				{
					continue;
				}
				string text = gameObject2.name;
				if (text.Contains("blade") || text.Contains("bullet") || text.Contains("gas"))
				{
					if (!UIObjects.Contains(gameObject2))
					{
						UIObjects.Add(gameObject2);
					}
					gameObject2.SetActive(value: false);
				}
			}
		}
		else
		{
			if (UIObjects == null)
			{
				return;
			}
			foreach (GameObject uIObject in UIObjects)
			{
				if (uIObject != null)
				{
					uIObject.SetActive(value: true);
				}
			}
		}
	}

	public void EnterSpecMode(bool enter)
	{
		InsideSpecmode = enter;
		if (enter)
		{
			spectateSprites = new List<GameObject>();
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				if (!(gameObject.GetComponent<UISprite>() != null) || !gameObject.activeInHierarchy)
				{
					continue;
				}
				string text = gameObject.name;
				if (text.Contains("blade") || text.Contains("bullet") || text.Contains("gas") || text.Contains("flare") || text.Contains("skill_cd"))
				{
					if (!spectateSprites.Contains(gameObject))
					{
						spectateSprites.Add(gameObject);
					}
					gameObject.SetActive(value: false);
				}
			}
			string[] array2 = new string[2] { "Flare", "LabelInfoBottomRight" };
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject2 = CacheGameObject.Find(array2[i]);
				if (gameObject2 != null)
				{
					if (!spectateSprites.Contains(gameObject2))
					{
						spectateSprites.Add(gameObject2);
					}
					gameObject2.SetActive(value: false);
				}
			}
			foreach (HERO hero in heroes)
			{
				if (hero.photonView.isMine)
				{
					PhotonNetwork.Destroy(hero.photonView);
				}
			}
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) == 2 && !RCextensions.returnBoolFromObject(PhotonNetwork.player.customProperties["dead"]))
			{
				foreach (TITAN titan in titans)
				{
					if (titan.photonView.isMine)
					{
						PhotonNetwork.Destroy(titan.photonView);
					}
				}
			}
			NGUITools.SetActive(uirefer.panels[1], state: false);
			NGUITools.SetActive(uirefer.panels[2], state: false);
			CharacterSelection.characterSelectionIsVisible = false;
			instance.needChooseSide = false;
			IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
			if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.ORIGINAL)
			{
				Screen.lockCursor = false;
				Screen.showCursor = false;
			}
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("Player");
			if (gameObject3 != null && gameObject3.GetComponent<HERO>() != null)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(gameObject3);
			}
			else
			{
				IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
			}
			IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: false);
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
			StartCoroutine(reloadSky());
			return;
		}
		if (CacheGameObject.Find("cross1") != null)
		{
			CacheGameObject.Find("cross1").transform.localPosition = Vector3.up * 5000f;
		}
		if (spectateSprites != null)
		{
			foreach (GameObject spectateSprite in spectateSprites)
			{
				if (spectateSprite != null)
				{
					spectateSprite.SetActive(value: true);
				}
			}
		}
		spectateSprites = new List<GameObject>();
		NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
		NGUITools.SetActive(CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
		CharacterSelection.characterSelectionIsVisible = false;
		instance.needChooseSide = true;
		IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
		IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
		IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
	}

	public void gameLose2()
	{
		if (isWinning || isLosing)
		{
			return;
		}
		isLosing = true;
		titanScore++;
		gameEndCD = gameEndTotalCDtime;
		if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
		{
			return;
		}
		object[] parameters = new object[1] { titanScore };
		PView.RPC("netGameLose", PhotonTargets.Others, parameters);
		if ((int)settings[244] != 0)
		{
			if ((int)settings[244] == 1)
			{
				InRoomChat.Write("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game lost).");
			}
			else
			{
				FleurConsole.instance.Log("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game lost).", FleurConsole.DebugType.LOG);
			}
		}
	}

	public void gameWin2()
	{
		if (isLosing || isWinning)
		{
			return;
		}
		isWinning = true;
		humanScore++;
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			if (GameSettings.racingStatic == 1)
			{
				gameEndCD = 1000f;
			}
			else
			{
				gameEndCD = 20f;
			}
			if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
			{
				return;
			}
			object[] parameters = new object[1] { 0 };
			PView.RPC("netGameWin", PhotonTargets.Others, parameters);
			if ((int)settings[244] != 0)
			{
				if ((int)settings[244] == 1)
				{
					InRoomChat.Write("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).");
				}
				else
				{
					FleurConsole.instance.Log("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).", FleurConsole.DebugType.LOG);
				}
			}
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			gameEndCD = gameEndTotalCDtime;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters2 = new object[1] { teamWinner };
				PView.RPC("netGameWin", PhotonTargets.Others, parameters2);
				if ((int)settings[244] != 0)
				{
					if ((int)settings[244] == 1)
					{
						InRoomChat.Write("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).");
					}
					else
					{
						FleurConsole.instance.Log("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).", FleurConsole.DebugType.LOG);
					}
				}
			}
			teamScores[teamWinner - 1]++;
			return;
		}
		gameEndCD = gameEndTotalCDtime;
		if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
		{
			return;
		}
		object[] parameters3 = new object[1] { humanScore };
		PView.RPC("netGameWin", PhotonTargets.Others, parameters3);
		if ((int)settings[244] != 0)
		{
			if ((int)settings[244] == 1)
			{
				InRoomChat.Write("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).");
			}
			else
			{
				FleurConsole.instance.Log("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).", FleurConsole.DebugType.LOG);
			}
		}
	}

	[RPC]
	private void getRacingResult(string player, float time, PhotonMessageInfo info)
	{
		if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING || !PhotonNetwork.isMasterClient)
		{
			ThrowJusticeDefensive(info.sender, "GRR");
			return;
		}
		RacingResult value = new RacingResult
		{
			name = player,
			time = time
		};
		racingResult.Add(value);
		refreshRacingResult2();
	}

	private string hairtype(int lol)
	{
		if (lol < 0)
		{
			return "Random";
		}
		return "Male " + lol;
	}

	[RPC]
	private void ignorePlayer(int ID, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient)
		{
			ThrowJusticeDefensive(info.sender, "invalid ignorePlayer");
			return;
		}
		PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
		if (photonPlayer != null && !ignoreList.Contains(ID))
		{
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				if (PhotonNetwork.playerList[i] == photonPlayer)
				{
					ignoreList.Add(ID);
				}
			}
		}
		RecompilePlayerList(0.1f);
	}

	[RPC]
	private void ignorePlayerArray(int[] IDS, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient)
		{
			ThrowJusticeDefensive(info.sender, "invalid ignorePlayArray");
			return;
		}
		foreach (int num in IDS)
		{
			PhotonPlayer photonPlayer = PhotonPlayer.Find(num);
			if (photonPlayer == null || ignoreList.Contains(num))
			{
				continue;
			}
			for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
			{
				if (PhotonNetwork.playerList[j] == photonPlayer)
				{
					ignoreList.Add(num);
					RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
					raiseEventOptions.TargetActors = new int[1] { num };
					RaiseEventOptions options = raiseEventOptions;
					PhotonNetwork.RaiseEvent(254, null, sendReliable: true, options);
				}
			}
		}
		RecompilePlayerList(0.1f);
	}

	public static GameObject InstantiateCustomAsset(string key)
	{
		key = key.Substring(8);
		return (GameObject)RCassets.Load(key);
	}

	public bool isPlayerAllDead2()
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
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

	public bool isTeamAllDead2(int team)
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer.customProperties["isTitan"] != null && photonPlayer.customProperties["team"] != null && RCextensions.returnIntFromObject(photonPlayer.customProperties["isTitan"]) == 1 && RCextensions.returnIntFromObject(photonPlayer.customProperties["team"]) == team)
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

	public void kickPlayerRC(PhotonPlayer player, bool ban, string reason)
	{
		string empty = string.Empty;
		empty = RCextensions.returnStringFromObject(player.customProperties["name"]);
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.DestroyPlayerObjects(player);
			if (OnPrivateServer)
			{
				ServerCloseConnection(player, ban, empty);
			}
			else
			{
				PhotonNetwork.CloseConnection(player);
			}
			PView.RPC("ignorePlayer", PhotonTargets.Others, player.ID);
		}
		if (!ignoreList.Contains(player.ID))
		{
			ignoreList.Add(player.ID);
		}
		if (ban && !banHash.ContainsKey(player.ID) && !banHash.ContainsValue(RCextensions.returnStringFromObject(player.customProperties["name"])))
		{
			banHash.Add(player.ID, empty);
		}
		if (reason != string.Empty)
		{
			FleurConsole.instance.Log("<color=#" + finalhex() + ">Player:</color> " + player.uiname.hexColor() + " <color=#" + finalhex() + ">{" + player.ID + "}</color> <color=#" + alternativefinalhex() + ">was autobanned.</color> <color=#" + finalhex() + ">Reason:</color>\n <color=#" + alternativefinalhex() + ">" + reason + ".</color>", FleurConsole.DebugType.LOG);
		}
		RecompilePlayerList(0.1f);
	}

	public static void ThrowJusticeDefensive(PhotonPlayer Putin, string Reason)
	{
		if (Putin == null)
		{
			return;
		}
		string text = ((Putin.customProperties["name"] == null) ? "「Unknown」" : ("「" + Putin.customProperties["name"].ToString().StripColor().StripHex() + "」"));
		string text2 = RCextensions.returnStringFromObject(Putin.customProperties["name"]);
		if (text.Length >= 200 || text.ToString().StripColor().Length >= 20)
		{
			text = "「big name」";
		}
		if (!ignoreList.Contains(Putin.ID) && !Putin.isLocal)
		{
			if (PhotonNetwork.isMasterClient)
			{
				instance.kickPlayerRC(Putin, ban: true, "");
			}
			ignoreList.Add(Putin.ID);
			if (!instance.DCPeopleList.Contains(text2))
			{
				instance.DCPeopleList.Add(text2);
			}
			if (!banHash.ContainsKey(Putin.ID) && !banHash.ContainsValue(text2))
			{
				banHash.Add(Putin.ID, text2);
			}
			instance.RecompilePlayerList(0.1f);
			FleurConsole.instance.Log("<color=red><b>[" + (PhotonNetwork.isMasterClient ? "Ban" : "Ignore") + "]</b></color> " + text + " <color=#" + alternativefinalhex() + ">{" + Putin.ID + "}</color> <color=#" + finalhex() + ">Reason:</color> " + Reason, FleurConsole.DebugType.ANTI);
			if (GameSettings.BotWarning > 0)
			{
				//AudioSource.PlayClipAtPoint(BotWarning, Camera.main.transform.position, AssistantVolume);
			}
			File.AppendAllText(Environment.CurrentDirectory + "\\Antis Log.txt", "[" + DateTime.Now.ToLongDateString() + " | " + DateTime.Now.ToLongTimeString() + "] " + (PhotonNetwork.isMasterClient ? "Ban " : "Ignore ") + ((Putin.uiname.StripHex().Length > 100) ? "big name" : Putin.uiname.StripHex()) + " {" + Putin.ID + "} " + Reason + Environment.NewLine + "------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine);
		}
	}

	[RPC]
	private void labelRPC(int setting, PhotonMessageInfo info)
	{
		if (!(PhotonView.Find(setting) != null) || !(base.photonView != null))
		{
			return;
		}
		PhotonPlayer owner = base.photonView.owner;
		if (owner == info.sender)
		{
			string guildname = owner.guildname;
			HERO component = base.photonView.GetComponent<HERO>();
			if (guildname != string.Empty)
			{
				component.myNetWorkName.GetComponent<UILabel>().text = "[FFFF00]" + guildname + "\n[FFFFFF]" + owner.name;
			}
			else
			{
				component.myNetWorkName.GetComponent<UILabel>().text = owner.name;
			}
		}
	}

	private void LateUpdate()
	{
		if (!gameStart)
		{
			return;
		}
		foreach (HERO hero in heroes)
		{
			hero.lateUpdate();
		}
		foreach (TITAN_EREN item in eT)
		{
			item.lateUpdate();
		}
		foreach (TITAN titan in titans)
		{
			titan.lateUpdate();
		}
		foreach (FEMALE_TITAN item2 in fT)
		{
			item2.lateUpdate();
		}
		Core(IN_GAME_MAIN_CAMERA.gametype);
	}

	public void loadconfig()
	{
		race_timer = PlayerPrefs.GetString("racetimer", "10");
		GameSettings.OneLifeRacing = PlayerPrefs.GetInt("OneLifeRacing", 0);
		GameSettings.DeleteRacing = PlayerPrefs.GetInt("DeleteRacing", 1);
		GameSettings.whiteNames = PlayerPrefs.GetInt("whiteNames", 0);
		MinCharL = PlayerPrefs.GetString("MinCharL", "3");
		MaxCharL = PlayerPrefs.GetString("MaxCharL", "12");
		GLMinCharL = PlayerPrefs.GetString("GLMinCharL", "3");
		GLMaxCharL = PlayerPrefs.GetString("GLMaxCharL", "12");
		GameSettings.AntiGuest = PlayerPrefs.GetInt("AntiGuest", 0);
		GameSettings.BotNoConnection = PlayerPrefs.GetInt("BotNoConnection", 1);
		GameSettings.BotPersonallyContacted = PlayerPrefs.GetInt("BotPersonallyContacted", 1);
		GameSettings.BotQuoted = PlayerPrefs.GetInt("BotQuoted", 1);
		GameSettings.BotTimeFinish = PlayerPrefs.GetInt("BotTimeFinish", 1);
		GameSettings.BotWarning = PlayerPrefs.GetInt("BotWarning", 1);
		GameSettings.Typing = PlayerPrefs.GetInt("Typing", 1);
		GameSettings.Clicking = PlayerPrefs.GetInt("Clicking", 1);
		GameSettings.GasSound = PlayerPrefs.GetInt("GasSound", 1);
		GameSettings.GasBurst = PlayerPrefs.GetInt("GasBurst", 1);
		GameSettings.BladesBroken = PlayerPrefs.GetInt("BladesBroken", 1);
		GameSettings.BladesReload = PlayerPrefs.GetInt("BladesReload", 1);
		GameSettings.GunsReload = PlayerPrefs.GetInt("GunsReload", 1);
		GameSettings.GunsShot = PlayerPrefs.GetInt("GunsShot", 1);
		GameSettings.BombFlying = PlayerPrefs.GetInt("BombFlying", 1);
		GameSettings.BombExplosion = PlayerPrefs.GetInt("BombExplosion", 1);
		GameSettings.BombKill = PlayerPrefs.GetInt("BombKill", 1);
		GameSettings.HookObject = PlayerPrefs.GetInt("HookObject", 1);
		GameSettings.HookTitan = PlayerPrefs.GetInt("HookTitan", 1);
		GameSettings.Flare = PlayerPrefs.GetInt("Flare", 1);
		GameSettings.Refill = PlayerPrefs.GetInt("Refill", 1);
		GameSettings.Slide = PlayerPrefs.GetInt("Slide", 1);
		GameSettings.Walk = PlayerPrefs.GetInt("Walk", 1);
		HookColorFloats0 = PlayerPrefs.GetFloat("HookColorFloats0", 0f);
		HookColorFloats1 = PlayerPrefs.GetFloat("HookColorFloats1", 0f);
		HookColorFloats2 = PlayerPrefs.GetFloat("HookColorFloats2", 0f);
		NamesColorFloats0 = PlayerPrefs.GetFloat("NamesColorFloats0", 0.8f);
		NamesColorFloats1 = PlayerPrefs.GetFloat("NamesColorFloats1", 0.3f);
		NamesColorFloats2 = PlayerPrefs.GetFloat("NamesColorFloats2", 0.4f);
		HexColorFloats0 = PlayerPrefs.GetFloat("HexColorFloats0", 0.612f);
		HexColorFloats1 = PlayerPrefs.GetFloat("HexColorFloats1", 0.812f);
		HexColorFloats2 = PlayerPrefs.GetFloat("HexColorFloats2", 0.906f);
		AHexColorFloats0 = PlayerPrefs.GetFloat("AHexColorFloats0", 0.953f);
		AHexColorFloats1 = PlayerPrefs.GetFloat("AHexColorFloats1", 0.302f);
		AHexColorFloats2 = PlayerPrefs.GetFloat("AHexColorFloats2", 0.302f);
		TeamColorFloats0 = PlayerPrefs.GetFloat("TeamColorFloats0", 0.9f);
		TeamColorFloats1 = PlayerPrefs.GetFloat("TeamColorFloats1", 0.2f);
		TeamColorFloats2 = PlayerPrefs.GetFloat("TeamColorFloats2", 0.2f);
		ETeamColorFloats0 = PlayerPrefs.GetFloat("ETeamColorFloats0", 0.5f);
		ETeamColorFloats1 = PlayerPrefs.GetFloat("ETeamColorFloats1", 0.7f);
		ETeamColorFloats2 = PlayerPrefs.GetFloat("ETeamColorFloats2", 0.8f);
		YellowDamageSize = PlayerPrefs.GetFloat("YellowDamageSize", 0.5f);
		FogDensity = PlayerPrefs.GetFloat("FogDensity", 0f);
		FadeTime = PlayerPrefs.GetString("FadeTime", "5");
		GameSettings.CMDHelper = PlayerPrefs.GetInt("CMDHelper", 0);
		LANField = PlayerPrefs.GetString("lastIP", "put ipt");
		CursorW = PlayerPrefs.GetString("CursorW", "27");
		CursorH = PlayerPrefs.GetString("CursorH", "27");
		ConsoleFade = PlayerPrefs.GetString("ConsoleFade", "4");
		ChatFade = PlayerPrefs.GetString("ChatFade", "4");
		ChatMess = PlayerPrefs.GetString("ChatMess", "5");
		ConsoleMess = PlayerPrefs.GetString("ConsoleMess", "5");
		RenderDist = PlayerPrefs.GetString("RenderDist", "1500");
		object[] array = new object[370]
		{
			PlayerPrefs.GetInt("humanskins", 2),
			PlayerPrefs.GetInt("titanskins", 2),
			PlayerPrefs.GetInt("levelskins", 2),
			PlayerPrefs.GetString("horse", string.Empty),
			PlayerPrefs.GetString("hair", string.Empty),
			PlayerPrefs.GetString("eye", string.Empty),
			PlayerPrefs.GetString("glass", string.Empty),
			PlayerPrefs.GetString("face", string.Empty),
			PlayerPrefs.GetString("skin", string.Empty),
			PlayerPrefs.GetString("costume", string.Empty),
			PlayerPrefs.GetString("logo", string.Empty),
			PlayerPrefs.GetString("bladel", string.Empty),
			PlayerPrefs.GetString("blader", string.Empty),
			PlayerPrefs.GetString("gas", string.Empty),
			PlayerPrefs.GetString("hoodie", string.Empty),
			PlayerPrefs.GetInt("EnableGasSkin", 1),
			PlayerPrefs.GetInt("titantype1", -1),
			PlayerPrefs.GetInt("titantype2", -1),
			PlayerPrefs.GetInt("titantype3", -1),
			PlayerPrefs.GetInt("titantype4", -1),
			PlayerPrefs.GetInt("titantype5", -1),
			PlayerPrefs.GetString("titanhair1", string.Empty),
			PlayerPrefs.GetString("titanhair2", string.Empty),
			PlayerPrefs.GetString("titanhair3", string.Empty),
			PlayerPrefs.GetString("titanhair4", string.Empty),
			PlayerPrefs.GetString("titanhair5", string.Empty),
			PlayerPrefs.GetString("titaneye1", string.Empty),
			PlayerPrefs.GetString("titaneye2", string.Empty),
			PlayerPrefs.GetString("titaneye3", string.Empty),
			PlayerPrefs.GetString("titaneye4", string.Empty),
			PlayerPrefs.GetString("titaneye5", string.Empty),
			0,
			PlayerPrefs.GetInt("titanR", 0),
			PlayerPrefs.GetString("tree1", ""),
			PlayerPrefs.GetString("tree2", ""),
			PlayerPrefs.GetString("tree3", ""),
			PlayerPrefs.GetString("tree4", ""),
			PlayerPrefs.GetString("tree5", ""),
			PlayerPrefs.GetString("tree6", ""),
			PlayerPrefs.GetString("tree7", ""),
			PlayerPrefs.GetString("tree8", ""),
			PlayerPrefs.GetString("leaf1", ""),
			PlayerPrefs.GetString("leaf2", ""),
			PlayerPrefs.GetString("leaf3", ""),
			PlayerPrefs.GetString("leaf4", ""),
			PlayerPrefs.GetString("leaf5", ""),
			PlayerPrefs.GetString("leaf6", ""),
			PlayerPrefs.GetString("leaf7", ""),
			PlayerPrefs.GetString("leaf8", ""),
			PlayerPrefs.GetString("forestG", ""),
			PlayerPrefs.GetInt("forestR", 0),
			PlayerPrefs.GetString("house1", ""),
			PlayerPrefs.GetString("house2", ""),
			PlayerPrefs.GetString("house3", ""),
			PlayerPrefs.GetString("house4", ""),
			PlayerPrefs.GetString("house5", ""),
			PlayerPrefs.GetString("house6", ""),
			PlayerPrefs.GetString("house7", ""),
			PlayerPrefs.GetString("house8", ""),
			PlayerPrefs.GetString("cityG", ""),
			PlayerPrefs.GetString("cityW", "https://i.imgur.com/F7ivDsN.png"),
			PlayerPrefs.GetString("cityH", ""),
			PlayerPrefs.GetInt("skinQ", 0),
			PlayerPrefs.GetInt("Mipmapping", 0),
			0,
			PlayerPrefs.GetString("eren", string.Empty),
			PlayerPrefs.GetString("annie", string.Empty),
			PlayerPrefs.GetString("colossal", string.Empty),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			PlayerPrefs.GetString("cmax", "20"),
			PlayerPrefs.GetString("titanbody1", string.Empty),
			PlayerPrefs.GetString("titanbody2", string.Empty),
			PlayerPrefs.GetString("titanbody3", string.Empty),
			PlayerPrefs.GetString("titanbody4", string.Empty),
			PlayerPrefs.GetString("titanbody5", string.Empty),
			0,
			PlayerPrefs.GetInt("EnableWeaponTrail", 0),
			PlayerPrefs.GetInt("windeffect", 0),
			PlayerPrefs.GetString("trailskin", string.Empty),
			PlayerPrefs.GetString("snapshot", "0"),
			PlayerPrefs.GetString("trailskin2", string.Empty),
			null,
			PlayerPrefs.GetString("reelin", "Space"),
			PlayerPrefs.GetString("reelout", "LeftAlt"),
			0,
			PlayerPrefs.GetString("tforward", "W"),
			PlayerPrefs.GetString("tback", "S"),
			PlayerPrefs.GetString("tleft", "A"),
			PlayerPrefs.GetString("tright", "D"),
			PlayerPrefs.GetString("twalk", "LeftShift"),
			PlayerPrefs.GetString("tjump", "Alpha0"),
			PlayerPrefs.GetString("tpunch", "Q"),
			PlayerPrefs.GetString("tslam", "E"),
			PlayerPrefs.GetString("tgrabfront", "Alpha1"),
			PlayerPrefs.GetString("tgrabback", "Alpha3"),
			PlayerPrefs.GetString("tgrabnape", "Mouse1"),
			PlayerPrefs.GetString("tantiae", "Mouse0"),
			PlayerPrefs.GetString("tbite", "Alpha2"),
			PlayerPrefs.GetString("tcover", "Z"),
			PlayerPrefs.GetString("tsit", "X"),
			null,
			PlayerPrefs.GetString("lforward", "W"),
			PlayerPrefs.GetString("lback", "S"),
			PlayerPrefs.GetString("lleft", "A"),
			PlayerPrefs.GetString("lright", "D"),
			PlayerPrefs.GetString("lup", "Mouse1"),
			PlayerPrefs.GetString("ldown", "Mouse0"),
			PlayerPrefs.GetString("lcursor", "X"),
			PlayerPrefs.GetString("lplace", "Backlash"),
			PlayerPrefs.GetString("ldel", "Backspace"),
			PlayerPrefs.GetString("lslow", "LeftShift"),
			PlayerPrefs.GetString("lrforward", "R"),
			PlayerPrefs.GetString("lrback", "F"),
			PlayerPrefs.GetString("lrleft", "Q"),
			PlayerPrefs.GetString("lrright", "E"),
			PlayerPrefs.GetString("lrccw", "Z"),
			PlayerPrefs.GetString("lrcw", "C"),
			PlayerPrefs.GetInt("humangui", 0),
			PlayerPrefs.GetString("horse2", string.Empty),
			PlayerPrefs.GetString("hair2", string.Empty),
			PlayerPrefs.GetString("eye2", string.Empty),
			PlayerPrefs.GetString("glass2", string.Empty),
			PlayerPrefs.GetString("face2", string.Empty),
			PlayerPrefs.GetString("skin2", string.Empty),
			PlayerPrefs.GetString("costume2", string.Empty),
			PlayerPrefs.GetString("logo2", string.Empty),
			PlayerPrefs.GetString("bladel2", string.Empty),
			PlayerPrefs.GetString("blader2", string.Empty),
			PlayerPrefs.GetString("gas2", string.Empty),
			PlayerPrefs.GetString("hoodie2", string.Empty),
			PlayerPrefs.GetString("trail2", string.Empty),
			PlayerPrefs.GetString("horse3", string.Empty),
			PlayerPrefs.GetString("hair3", string.Empty),
			PlayerPrefs.GetString("eye3", string.Empty),
			PlayerPrefs.GetString("glass3", string.Empty),
			PlayerPrefs.GetString("face3", string.Empty),
			PlayerPrefs.GetString("skin3", string.Empty),
			PlayerPrefs.GetString("costume3", string.Empty),
			PlayerPrefs.GetString("logo3", string.Empty),
			PlayerPrefs.GetString("bladel3", string.Empty),
			PlayerPrefs.GetString("blader3", string.Empty),
			PlayerPrefs.GetString("gas3", string.Empty),
			PlayerPrefs.GetString("hoodie3", string.Empty),
			PlayerPrefs.GetString("trail3", string.Empty),
			null,
			PlayerPrefs.GetString("lfast", "LeftControl"),
			PlayerPrefs.GetString("customGround", string.Empty),
			PlayerPrefs.GetString("forestskyfront", string.Empty),
			PlayerPrefs.GetString("forestskyback", string.Empty),
			PlayerPrefs.GetString("forestskyleft", string.Empty),
			PlayerPrefs.GetString("forestskyright", string.Empty),
			PlayerPrefs.GetString("forestskyup", string.Empty),
			PlayerPrefs.GetString("forestskydown", string.Empty),
			PlayerPrefs.GetString("cityskyfront", string.Empty),
			PlayerPrefs.GetString("cityskyback", string.Empty),
			PlayerPrefs.GetString("cityskyleft", string.Empty),
			PlayerPrefs.GetString("cityskyright", string.Empty),
			PlayerPrefs.GetString("cityskyup", string.Empty),
			PlayerPrefs.GetString("cityskydown", string.Empty),
			PlayerPrefs.GetString("customskyfront", string.Empty),
			PlayerPrefs.GetString("customskyback", string.Empty),
			PlayerPrefs.GetString("customskyleft", string.Empty),
			PlayerPrefs.GetString("customskyright", string.Empty),
			PlayerPrefs.GetString("customskyup", string.Empty),
			PlayerPrefs.GetString("customskydown", string.Empty),
			PlayerPrefs.GetInt("dashenable", 0),
			PlayerPrefs.GetString("dashkey", "Scroll Up"),
			PlayerPrefs.GetInt("vsync", 0),
			PlayerPrefs.GetString("fpscap", "0"),
			PlayerPrefs.GetInt("sendskins", 1),
			null,
			null,
			null,
			PlayerPrefs.GetInt("speedometer", 2),
			0,
			string.Empty,
			PlayerPrefs.GetInt("bombMode", 0),
			PlayerPrefs.GetInt("teamMode", 0),
			PlayerPrefs.GetInt("rockThrow", 0),
			PlayerPrefs.GetInt("explodeModeOn", 0),
			PlayerPrefs.GetString("explodeModeNum", "30"),
			PlayerPrefs.GetInt("healthMode", 0),
			PlayerPrefs.GetString("healthLower", "100"),
			PlayerPrefs.GetString("healthUpper", "200"),
			null,
			null,
			PlayerPrefs.GetInt("banEren", 0),
			PlayerPrefs.GetInt("moreTitanOn", 0),
			PlayerPrefs.GetString("moreTitanNum", "1"),
			PlayerPrefs.GetInt("damageModeOn", 0),
			PlayerPrefs.GetString("damageModeNum", "1000"),
			PlayerPrefs.GetInt("sizeMode", 0),
			PlayerPrefs.GetString("sizeLower", "1.0"),
			PlayerPrefs.GetString("sizeUpper", "3.0"),
			PlayerPrefs.GetInt("spawnModeOn", 0),
			PlayerPrefs.GetString("nRate", "20.0"),
			PlayerPrefs.GetString("aRate", "20.0"),
			PlayerPrefs.GetString("jRate", "20.0"),
			PlayerPrefs.GetString("cRate", "20.0"),
			PlayerPrefs.GetString("pRate", "20.0"),
			PlayerPrefs.GetInt("horseMode", 0),
			PlayerPrefs.GetInt("waveModeOn", 0),
			PlayerPrefs.GetString("waveModeNum", "1"),
			PlayerPrefs.GetInt("friendlyMode", 0),
			PlayerPrefs.GetInt("pvpMode", 0),
			PlayerPrefs.GetInt("maxWaveOn", 0),
			PlayerPrefs.GetString("maxWaveNum", "20"),
			PlayerPrefs.GetInt("endlessModeOn", 0),
			PlayerPrefs.GetString("endlessModeNum", "10"),
			PlayerPrefs.GetString("motd", string.Empty),
			PlayerPrefs.GetInt("pointModeOn", 0),
			PlayerPrefs.GetString("pointModeNum", "50"),
			PlayerPrefs.GetInt("ahssReload", 0),
			PlayerPrefs.GetInt("punkWaves", 0),
			0,
			null,
			PlayerPrefs.GetString("mapMaximize", "Tab"),
			PlayerPrefs.GetString("mapToggle", "M"),
			PlayerPrefs.GetString("mapReset", "K"),
			PlayerPrefs.GetInt("globalDisableMinimap", 0),
			PlayerPrefs.GetString("chatRebind", "None"),
			PlayerPrefs.GetString("hforward", "W"),
			PlayerPrefs.GetString("hback", "S"),
			PlayerPrefs.GetString("hleft", "A"),
			PlayerPrefs.GetString("hright", "D"),
			PlayerPrefs.GetString("hwalk", "LeftShift"),
			PlayerPrefs.GetString("hjump", "Q"),
			PlayerPrefs.GetString("hmount", "LeftControl"),
			PlayerPrefs.GetInt("chatfeed", 0),
			0,
			PlayerPrefs.GetFloat("bombR", 1f),
			PlayerPrefs.GetFloat("bombG", 1f),
			PlayerPrefs.GetFloat("bombB", 1f),
			PlayerPrefs.GetFloat("bombA", 1f),
			PlayerPrefs.GetFloat("bombRadius", 6f),
			PlayerPrefs.GetFloat("bombRange", 2f),
			PlayerPrefs.GetFloat("bombSpeed", 6f),
			PlayerPrefs.GetFloat("bombCD", 6f),
			PlayerPrefs.GetString("cannonUp", "W"),
			PlayerPrefs.GetString("cannonDown", "S"),
			PlayerPrefs.GetString("cannonLeft", "A"),
			PlayerPrefs.GetString("cannonRight", "D"),
			PlayerPrefs.GetString("cannonFire", "Q"),
			PlayerPrefs.GetString("cannonMount", "G"),
			PlayerPrefs.GetString("cannonSlow", "LeftShift"),
			PlayerPrefs.GetInt("deadlyCannon", 0),
			PlayerPrefs.GetString("liveCam", "Y"),
			1,
			null,
			null,
			null,
			PlayerPrefs.GetFloat("bombR2", 1f),
			PlayerPrefs.GetFloat("bombG2", 1f),
			PlayerPrefs.GetFloat("bombB2", 1f),
			PlayerPrefs.GetFloat("bombA2", 1f),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			PlayerPrefs.GetInt("ReelType", 0),
			PlayerPrefs.GetInt("FeedCounter", 0),
			PlayerPrefs.GetInt("ShowOriginalMess", 0),
			PlayerPrefs.GetInt("PremadeCharacter", 0),
			PlayerPrefs.GetInt("BurstType", 2),
			PlayerPrefs.GetInt("TranslateIncomingMess", 0),
			null,
			null,
			null,
			PlayerPrefs.GetInt("MovementType", 0),
			null,
			PlayerPrefs.GetInt("DetonateSticky", 0),
			PlayerPrefs.GetInt("HookColors", 0),
			PlayerPrefs.GetInt("Region", 0),
			PlayerPrefs.GetInt("Interpolation", 0),
			PlayerPrefs.GetInt("SecondPremadeCharacter", 0),
			PlayerPrefs.GetInt("ThirdPremadeCharacter", 0),
			PlayerPrefs.GetInt("AntiAliasing", 0),
			null,
			PlayerPrefs.GetInt("BrightForest", 1),
			PlayerPrefs.GetInt("AxisType", 1),
			PlayerPrefs.GetInt("HDTrails", 0),
			PlayerPrefs.GetInt("DrunkTitans", 0),
			PlayerPrefs.GetString("horse4", string.Empty),
			PlayerPrefs.GetString("hair4", string.Empty),
			PlayerPrefs.GetString("eye4", string.Empty),
			PlayerPrefs.GetString("glass4", string.Empty),
			PlayerPrefs.GetString("face4", string.Empty),
			PlayerPrefs.GetString("skin4", string.Empty),
			PlayerPrefs.GetString("costume4", string.Empty),
			PlayerPrefs.GetString("logo4", string.Empty),
			PlayerPrefs.GetString("bladel4", string.Empty),
			PlayerPrefs.GetString("blader4", string.Empty),
			PlayerPrefs.GetString("gas4", string.Empty),
			PlayerPrefs.GetString("hoodie4", string.Empty),
			PlayerPrefs.GetString("trail4", string.Empty),
			PlayerPrefs.GetString("ThunderspearL1", string.Empty),
			PlayerPrefs.GetString("ThunderspearR1", string.Empty),
			PlayerPrefs.GetString("ThunderspearL2", string.Empty),
			PlayerPrefs.GetString("ThunderspearR2", string.Empty),
			PlayerPrefs.GetString("ThunderspearL3", string.Empty),
			PlayerPrefs.GetString("ThunderspearR3", string.Empty),
			PlayerPrefs.GetString("ThunderspearL4", string.Empty),
			PlayerPrefs.GetString("ThunderspearR4", string.Empty),
			PlayerPrefs.GetInt("TranslateOutgoingMess", 0),
			PlayerPrefs.GetInt("LevelLight", 0),
			PlayerPrefs.GetInt("TranslateToInc", 0),
			PlayerPrefs.GetInt("FogColors", 0),
			PlayerPrefs.GetString("BombName", "bomb"),
			PlayerPrefs.GetInt("TranslateToOut", 0),
			PlayerPrefs.GetInt("anisotropicFiltering", 0),
			PlayerPrefs.GetString("NormalTitan", "Titan"),
			PlayerPrefs.GetString("AberrantTitan", "Aberrant"),
			PlayerPrefs.GetString("JumperTitan", "Jumper"),
			PlayerPrefs.GetString("CrawlerTitan", "Crawler"),
			PlayerPrefs.GetString("PunkTitan", "Punk"),
			PlayerPrefs.GetInt("rejoinType", 0),
			PlayerPrefs.GetString("BombName2", "bomb"),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			PlayerPrefs.GetInt("NameSets", 0),
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			PlayerPrefs.GetInt("ProtocolType", 1)
		};
		GameSettings.hideGuilds = PlayerPrefs.GetInt("hideGuilds", 0);
		GameSettings.hideYourName = PlayerPrefs.GetInt("hideYourName", 0);
		GameSettings.napeMelons = PlayerPrefs.GetInt("napeMelons", 0);
		GameSettings.yourGas = PlayerPrefs.GetInt("yourGas", 0);
		GameSettings.theirGas = PlayerPrefs.GetInt("theirGas", 1);
		GameSettings.chatStyle = PlayerPrefs.GetInt("chatStyle", 0);
		GameSettings.Pace = PlayerPrefs.GetInt("Pace", 2);
		GameSettings.chatNameStyle = PlayerPrefs.GetInt("chatNameStyle", 0);
		GameSettings.BackflipBurst = PlayerPrefs.GetInt("BackflipBurst", 0);
		GameSettings.Drunk = PlayerPrefs.GetInt("Drunk", 0);
		GameSettings.UpsideDown = PlayerPrefs.GetInt("UpsideDown", 0);
		GameSettings.ChatBackground = PlayerPrefs.GetInt("ChatBackground", 0);
		GameSettings.ConsoleBackground = PlayerPrefs.GetInt("ConsoleBackground", 0);
		GameSettings.ShowFPS = PlayerPrefs.GetInt("ShowFPS", 1);
		GameSettings.ShowDistance = PlayerPrefs.GetInt("ShowDistance", 0);
		GameSettings.ShowCD = PlayerPrefs.GetInt("ShowCD", 0);
		GameSettings.SettingsHUD = PlayerPrefs.GetInt("titan_settings", 0);
		GameSettings.OldPlayerlist = PlayerPrefs.GetInt("OldPlayerlist", 0);
		GameSettings.NoArrowsCursor = PlayerPrefs.GetInt("NoArrowsCursor", 1);
		GameSettings.TopRightHUD = PlayerPrefs.GetInt("TopRightHUD", 0);
		GameSettings.BottomHUD = PlayerPrefs.GetInt("BottomHUD", 1);
		GameSettings.UseOldBombEffect = PlayerPrefs.GetInt("UseOldBombEffect", 1);
		GameSettings.ShowBombColor = PlayerPrefs.GetInt("ShowBombColor", 1);
		GameSettings.HandsHolding = PlayerPrefs.GetInt("HandsHolding", 0);
		GameSettings.NoCorners = PlayerPrefs.GetInt("NoCorners", 0);
		GameSettings.HideTeam = PlayerPrefs.GetInt("HideTeam", 0);
		inputRC = new InputManagerRC();
		inputRC.setInputHuman(InputCodeRC.reelin, (string)array[98]);
		inputRC.setInputHuman(InputCodeRC.reelout, (string)array[99]);
		inputRC.setInputHuman(InputCodeRC.dash, (string)array[182]);
		inputRC.setInputHuman(InputCodeRC.mapMaximize, (string)array[232]);
		inputRC.setInputHuman(InputCodeRC.mapToggle, (string)array[233]);
		inputRC.setInputHuman(InputCodeRC.mapReset, (string)array[234]);
		inputRC.setInputHuman(InputCodeRC.chat, (string)array[236]);
		inputRC.setInputHuman(InputCodeRC.liveCam, (string)array[262]);
		if (!Enum.IsDefined(typeof(KeyCode), (string)array[232]))
		{
			array[232] = "None";
		}
		if (!Enum.IsDefined(typeof(KeyCode), (string)array[233]))
		{
			array[233] = "None";
		}
		if (!Enum.IsDefined(typeof(KeyCode), (string)array[234]))
		{
			array[234] = "None";
		}
		for (int i = 0; i < 15; i++)
		{
			inputRC.setInputTitan(i, (string)array[101 + i]);
		}
		for (int i = 0; i < 16; i++)
		{
			inputRC.setInputLevel(i, (string)array[117 + i]);
		}
		for (int i = 0; i < 7; i++)
		{
			inputRC.setInputHorse(i, (string)array[237 + i]);
		}
		for (int i = 0; i < 7; i++)
		{
			inputRC.setInputCannon(i, (string)array[254 + i]);
		}
		object[] array2 = new object[6]
		{
			0,
			PlayerPrefs.GetString("leavegame", "F1"),
			PlayerPrefs.GetString("showconsole", "F2"),
			PlayerPrefs.GetString("trafficpanel", "F3"),
			PlayerPrefs.GetString("2ndplayerlist", "Tab"),
			PlayerPrefs.GetString("rest", "F9")
		};
		for (int i = 0; i < 5; i++)
		{
			inputRC.setInputExtras(i, (string)array2[1 + i]);
		}
		customkey = array2;
		inputRC.setInputLevel(InputCodeRC.levelFast, (string)array[161]);
		Application.targetFrameRate = -1;
		if (int.TryParse((string)array[184], out var result) && result > 0)
		{
			Application.targetFrameRate = result;
		}
		QualitySettings.vSyncCount = 0;
		if ((int)array[183] == 1)
		{
			QualitySettings.vSyncCount = 1;
		}
		AudioListener.volume = PlayerPrefs.GetFloat("vol", 1f);
		InterfaceVolume = PlayerPrefs.GetFloat("interfacevol", 1f);
		AssistantVolume = PlayerPrefs.GetFloat("assistantvol", 1f);
		QualitySettings.masterTextureLimit = PlayerPrefs.GetInt("skinQ", 0);
		linkHash = new ExitGames.Client.Photon.Hashtable[5]
		{
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable()
		};
		settings = array;
		allScrolls = Vector2.zero;
		tiltSlider = PlayerPrefs.GetFloat("tiltSlider", 5f);
		distanceSlider = PlayerPrefs.GetFloat("cameraDistance", 1f);
		mouseSlider = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
		qualitySlider = PlayerPrefs.GetFloat("GameQuality", 0f);
	}

	private void CreateBarrier()
	{
		if (GameSettings.bombMode <= 0)
		{
			return;
		}
		if (level.StartsWith("The Forest"))
		{
			GameObject[] array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			foreach (GameObject gameObject in array)
			{
				if (gameObject.name.Contains("gameobjectOutSide"))
				{
					gameObject.transform.position = new Vector3(-19.1f, 0f, 182.9f);
					gameObject.transform.localScale = new Vector3(1.6f, 0.9f, 1.6f);
				}
			}
		}
		else
		{
			if (!level.StartsWith("The City"))
			{
				return;
			}
			GameObject[] array2 = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			foreach (GameObject gameObject2 in array2)
			{
				if (gameObject2.name.Contains("gameobjectOutSide"))
				{
					gameObject2.transform.position = new Vector3(-19.1f, 0f, 182.9f);
					gameObject2.transform.localScale = new Vector3(1.6f, 0.48f, 1.6f);
				}
			}
		}
	}

	private void SuicideBarrier()
	{
		foreach (HERO hero in heroes)
		{
			if (hero != null && GameSettings.bombMode > 0)
			{
				PhotonView photonView = hero.photonView;
				if (hero.transform.position.y > (float)(level.StartsWith("The Forest") ? 280 : (level.StartsWith("The City") ? 210 : 500)))
				{
					PhotonPlayer owner = photonView.owner;
					base.photonView.RPC("netDie2", owner, -1, "filthy Skybomber");
				}
			}
		}
	}

	private void teleport(PhotonPlayer player)
	{
		foreach (HERO hero in heroes)
		{
			hero.photonView.RPC("blowAway", player, new Vector3(-20000f, -20000f, -20000f));
		}
	}

	private void OneLifeGameMode()
	{
		if (GameSettings.OneLifeRacing != 1 || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
		{
			return;
		}
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (oneLifeList.Contains(photonPlayer))
			{
				teleport(photonPlayer);
			}
		}
	}

	private void loadskin()
	{
		CreateBarrier();
		if (PhotonNetwork.isMasterClient)
		{
			if (GameSettings.bombMode > 0 && !IsInvoking("SuicideBarrier"))
			{
				InvokeRepeating("SuicideBarrier", 0f, 2f);
			}
			if (GameSettings.OneLifeRacing == 1 && !IsInvoking("OneLifeGameMode") && level.StartsWith("Custom"))
			{
				InvokeRepeating("OneLifeGameMode", 0f, 6f);
			}
		}
		InstantiateTracker.instance.Dispose();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			updateTime = 1f;
			if (oldScriptLogic != currentScriptLogic)
			{
				intVariables.Clear();
				boolVariables.Clear();
				stringVariables.Clear();
				floatVariables.Clear();
				globalVariables.Clear();
				RCEvents.Clear();
				RCVariableNames.Clear();
				playerVariables.Clear();
				titanVariables.Clear();
				RCRegionTriggers.Clear();
				oldScriptLogic = currentScriptLogic;
				compileScript(currentScriptLogic);
				if (RCEvents.ContainsKey("OnFirstLoad"))
				{
					((RCEvent)RCEvents["OnFirstLoad"]).checkEvent();
				}
			}
			if (RCEvents.ContainsKey("OnRoundStart"))
			{
				((RCEvent)RCEvents["OnRoundStart"]).checkEvent();
			}
			base.photonView.RPC("setMasterRC", PhotonTargets.All);
		}
		logicLoaded = true;
		racingSpawnPoint = new Vector3(0f, 0f, 0f);
		racingSpawnPointSet = false;
		racingDoors = new List<GameObject>();
		allowedToCannon = new Dictionary<int, CannonValues>();
		if (!level.StartsWith("Custom"))
		{
			if (PhotonNetwork.isMasterClient && level.Contains("Forest") && GameSettings.bombMode > 0 && GameSettings.NoCorners > 0)
			{
				currentScript = "custom,barrier,transparent0.3,125,27,2,1,0.25,0,0.6,0,0,0,50,585,0,0,0,1; custom,barrier,transparent0.3,125,27,2,1,0.25,0,0.6,0,0,0,50,-575,0,0,0,1; custom,barrier,transparent0.3,2,27,125,1,0.25,0,0.6,0,0,585,50,0,0,0,0,1; custom,barrier,transparent0.3,2,27,125,1,0.25,0,0.6,0,0,-605,50,0,0,0,0,1; custom,barrier,transparent0.3,125,2,125,1,0.25,0,0.6,0,0,0,175,0,0,0,0,1; custom,barrier,transparent0.3,40,27,2,1,0.25,0,0.6,0,0,-455,50,-455,0,0.38268343236509,0,0.923879532511287; custom,barrier,transparent0.3,40,27,2,1,0.25,0,0.6,0,0,455,50,455,0,0.38268343236509,0,0.923879532511287; custom,barrier,transparent0.3,47,27,2,1,0.25,0,0.6,0,0,-430,50,430,0,-0.38268343236509,0,0.923879532511287; custom,barrier,transparent0.3,46,27,2,1,0.25,0,0.6,0,0,415,50,-415,0,-0.38268343236509,0,0.923879532511287; custom,killcuboid,transparent0.3,125,27,2,1,0.25,0,0.6,0,0,0,50,585,0,0,0,1; custom,killcuboid,transparent0.3,125,27,2,1,0.25,0,0.6,0,0,0,50,-575,0,0,0,1; custom,killcuboid,transparent0.3,2,27,125,1,0.25,0,0.6,0,0,585,50,0,0,0,0,1; custom,killcuboid,transparent0.3,2,27,125,1,0.25,0,0.6,0,0,-605,50,0,0,0,0,1; custom,killcuboid,transparent0.3,125,2,125,1,0.25,0,0.6,0,0,0,175,0,0,0,0,1; custom,killcuboid,transparent0.3,40,27,2,1,0.25,0,0.6,0,0,-455,50,-455,0,0.38268343236509,0,0.923879532511287; custom,killcuboid,transparent0.3,40,27,2,1,0.25,0,0.6,0,0,455,50,455,0,0.38268343236509,0,0.923879532511287; custom,killcuboid,transparent0.3,47,27,2,1,0.25,0,0.6,0,0,-430,50,430,0,-0.38268343236509,0,0.923879532511287; custom,killcuboid,transparent0.3,46,27,2,1,0.25,0,0.6,0,0,415,50,-415,0,-0.38268343236509,0,0.923879532511287;";
				if (oldScript != currentScript)
				{
					levelCache.Clear();
					titanSpawns.Clear();
					playerSpawnsC.Clear();
					playerSpawnsM.Clear();
					titanSpawners.Clear();
					currentLevel = string.Empty;
					if (currentScript == string.Empty)
					{
						ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
						hashtable.Add("currentLevel", currentLevel);
						PhotonNetwork.player.SetCustomProperties(hashtable);
						oldScript = currentScript;
					}
					else
					{
						string[] array = Regex.Replace(currentScript, "\\s+", "").Replace("\r\n", "").Replace("\n", "")
							.Replace("\r", "")
							.Split(';');
						for (int i = 0; i < Mathf.FloorToInt((array.Length - 1) / 100) + 1; i++)
						{
							string[] array2;
							int num;
							if (i < Mathf.FloorToInt(array.Length / 100))
							{
								array2 = new string[101];
								num = 0;
								for (int j = 100 * i; j < 100 * i + 100; j++)
								{
									if (array[j].StartsWith("spawnpoint"))
									{
										string[] array3 = array[j].Split(',');
										if (array3[1] == "titan")
										{
											titanSpawns.Add(new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])));
										}
										else if (array3[1] == "playerC")
										{
											playerSpawnsC.Add(new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])));
										}
										else if (array3[1] == "playerM")
										{
											playerSpawnsM.Add(new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])));
										}
									}
									array2[num] = array[j];
									num++;
								}
								currentLevel += (array2[100] = UnityEngine.Random.Range(10000, 99999).ToString());
								levelCache.Add(array2);
								continue;
							}
							array2 = new string[array.Length % 100 + 1];
							num = 0;
							for (int j = 100 * i; j < 100 * i + array.Length % 100; j++)
							{
								if (array[j].StartsWith("spawnpoint"))
								{
									string[] array3 = array[j].Split(',');
									if (array3[1] == "titan")
									{
										titanSpawns.Add(new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])));
									}
									else if (array3[1] == "playerC")
									{
										playerSpawnsC.Add(new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])));
									}
									else if (array3[1] == "playerM")
									{
										playerSpawnsM.Add(new Vector3(Convert.ToSingle(array3[2]), Convert.ToSingle(array3[3]), Convert.ToSingle(array3[4])));
									}
								}
								array2[num] = array[j];
								num++;
							}
							string text = UnityEngine.Random.Range(10000, 99999).ToString();
							array2[array.Length % 100] = text;
							currentLevel += text;
							levelCache.Add(array2);
						}
						List<string> list = new List<string>();
						foreach (Vector3 titanSpawn in titanSpawns)
						{
							string[] obj = new string[6] { "titan,", null, null, null, null, null };
							float x = titanSpawn.x;
							obj[1] = x.ToString();
							obj[2] = ",";
							x = titanSpawn.y;
							obj[3] = x.ToString();
							obj[4] = ",";
							x = titanSpawn.z;
							obj[5] = x.ToString();
							list.Add(string.Concat(obj));
						}
						foreach (Vector3 item in playerSpawnsC)
						{
							string[] obj2 = new string[6] { "playerC,", null, null, null, null, null };
							float x = item.x;
							obj2[1] = x.ToString();
							obj2[2] = ",";
							x = item.y;
							obj2[3] = x.ToString();
							obj2[4] = ",";
							x = item.z;
							obj2[5] = x.ToString();
							list.Add(string.Concat(obj2));
						}
						foreach (Vector3 item2 in playerSpawnsM)
						{
							string[] obj3 = new string[6] { "playerM,", null, null, null, null, null };
							float x = item2.x;
							obj3[1] = x.ToString();
							obj3[2] = ",";
							x = item2.y;
							obj3[3] = x.ToString();
							obj3[4] = ",";
							x = item2.z;
							obj3[5] = x.ToString();
							list.Add(string.Concat(obj3));
						}
						string text2 = "a" + UnityEngine.Random.Range(10000, 99999);
						list.Add(text2);
						currentLevel = text2 + currentLevel;
						levelCache.Insert(0, list.ToArray());
						string text3 = "z" + UnityEngine.Random.Range(10000, 99999);
						levelCache.Add(new string[1] { text3 });
						currentLevel += text3;
						ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
						hashtable.Add("currentLevel", currentLevel);
						PhotonNetwork.player.SetCustomProperties(hashtable);
						oldScript = currentScript;
					}
				}
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
					if (!photonPlayer.isMasterClient)
					{
						playersRPC.Add(photonPlayer);
					}
				}
				StartCoroutine(customlevelE(playersRPC));
				StartCoroutine(customlevelcache());
			}
			if ((int)settings[2] <= 0)
			{
				return;
			}
			GameObject.Find("aot_supply");
			string text4 = string.Empty;
			string text5 = string.Empty;
			string text6 = string.Empty;
			string[] array4 = new string[6]
			{
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty
			};
			if (LevelInfo.getInfo(level).mapName.Contains("City"))
			{
				for (int i = 51; i < 59; i++)
				{
					text4 = text4 + (string)settings[i] + ",";
				}
				text4.TrimEnd(',');
				for (int j = 0; j < 250; j++)
				{
					text6 += Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
				}
				text5 = (string)settings[59] + "," + (string)settings[60] + "," + (string)settings[61];
				for (int i = 0; i < 6; i++)
				{
					array4[i] = (string)settings[i + 169];
				}
			}
			else if (LevelInfo.getInfo(level).mapName.Contains("Forest"))
			{
				for (int k = 33; k < 41; k++)
				{
					text4 = text4 + (string)settings[k] + ",";
				}
				text4.TrimEnd(',');
				for (int l = 41; l < 49; l++)
				{
					text5 = text5 + (string)settings[l] + ",";
				}
				text5 += (string)settings[49];
				for (int m = 0; m < 150; m++)
				{
					string text7 = Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
					text6 += text7;
					text6 = (((int)settings[50] != 0) ? (text6 + Convert.ToString((int)UnityEngine.Random.Range(0f, 8f))) : (text6 + text7));
				}
				for (int i = 0; i < 6; i++)
				{
					array4[i] = (string)settings[i + 163];
				}
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				StartCoroutine(loadskinE(text6, text4, text5, array4));
			}
			else if ((int)settings[2] == 2)
			{
				if (LevelInfo.getInfo(level).mapName.Contains("Forest"))
				{
					text4 = "https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,";
					text5 = "https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif";
				}
				else if (LevelInfo.getInfo(level).mapName.Contains("City"))
				{
					text4 = "https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,";
					text5 = "https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif,https://i.imgur.com/nmpD1oo.gif";
				}
				StartCoroutine(loadskinE(text6, text4, text5, array4));
			}
			else if (PhotonNetwork.isMasterClient)
			{
				base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, text6, text4, text5, array4);
			}
			else if (LevelInfo.getInfo(level).mapName.Contains("City"))
			{
				StartCoroutine(loadskinE(text6, text4, text5, array4));
			}
		}
		else
		{
			if (!level.StartsWith("Custom") || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				return;
			}
			GameObject[] array5 = GameObject.FindGameObjectsWithTag("playerRespawn");
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].transform.position = new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, UnityEngine.Random.Range(-5f, 5f));
			}
			GameObject[] array6 = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			foreach (GameObject gameObject in array6)
			{
				if (gameObject.name.Contains("TREE") || gameObject.name.Contains("aot_supply"))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				else if (gameObject.name == "Cube_001" && gameObject.transform.parent.gameObject.tag != "player" && gameObject.renderer != null)
				{
					groundList.Add(gameObject);
					gameObject.renderer.material.mainTexture = ((Material)RCassets.Load("grass")).mainTexture;
				}
			}
			if (!PhotonNetwork.isMasterClient)
			{
				return;
			}
			string[] array4 = new string[7]
			{
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty
			};
			for (int i = 0; i < 6; i++)
			{
				array4[i] = (string)settings[i + 175];
			}
			array4[6] = (string)settings[162];
			if (int.TryParse((string)settings[85], out var result))
			{
				GameSettings.titanCap = result;
			}
			else
			{
				GameSettings.titanCap = 0;
				settings[85] = "0";
			}
			GameSettings.titanCap = Math.Min(50, GameSettings.titanCap);
			base.photonView.RPC("clearlevel", PhotonTargets.AllBuffered, array4, GameSettings.gameType);
			RCRegions.Clear();
			if (oldScript != currentScript)
			{
				levelCache.Clear();
				titanSpawns.Clear();
				playerSpawnsC.Clear();
				playerSpawnsM.Clear();
				titanSpawners.Clear();
				currentLevel = string.Empty;
				if (currentScript == string.Empty)
				{
					ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
					hashtable2.Add("currentLevel", currentLevel);
					PhotonNetwork.player.SetCustomProperties(hashtable2);
					oldScript = currentScript;
				}
				else
				{
					string[] array7 = Regex.Replace(currentScript, "\\s+", "").Replace("\r\n", "").Replace("\n", "")
						.Replace("\r", "")
						.Split(';');
					for (int i = 0; i < Mathf.FloorToInt((array7.Length - 1) / 100) + 1; i++)
					{
						string[] array8;
						int num2;
						if (i < Mathf.FloorToInt(array7.Length / 100))
						{
							array8 = new string[101];
							num2 = 0;
							for (int j = 100 * i; j < 100 * i + 100; j++)
							{
								if (array7[j].StartsWith("spawnpoint"))
								{
									string[] array9 = array7[j].Split(',');
									if (array9[1] == "titan")
									{
										titanSpawns.Add(new Vector3(Convert.ToSingle(array9[2]), Convert.ToSingle(array9[3]), Convert.ToSingle(array9[4])));
									}
									else if (array9[1] == "playerC")
									{
										playerSpawnsC.Add(new Vector3(Convert.ToSingle(array9[2]), Convert.ToSingle(array9[3]), Convert.ToSingle(array9[4])));
									}
									else if (array9[1] == "playerM")
									{
										playerSpawnsM.Add(new Vector3(Convert.ToSingle(array9[2]), Convert.ToSingle(array9[3]), Convert.ToSingle(array9[4])));
									}
								}
								array8[num2] = array7[j];
								num2++;
							}
							currentLevel += (array8[100] = UnityEngine.Random.Range(10000, 99999).ToString());
							levelCache.Add(array8);
							continue;
						}
						array8 = new string[array7.Length % 100 + 1];
						num2 = 0;
						for (int j = 100 * i; j < 100 * i + array7.Length % 100; j++)
						{
							if (array7[j].StartsWith("spawnpoint"))
							{
								string[] array9 = array7[j].Split(',');
								if (array9[1] == "titan")
								{
									titanSpawns.Add(new Vector3(Convert.ToSingle(array9[2]), Convert.ToSingle(array9[3]), Convert.ToSingle(array9[4])));
								}
								else if (array9[1] == "playerC")
								{
									playerSpawnsC.Add(new Vector3(Convert.ToSingle(array9[2]), Convert.ToSingle(array9[3]), Convert.ToSingle(array9[4])));
								}
								else if (array9[1] == "playerM")
								{
									playerSpawnsM.Add(new Vector3(Convert.ToSingle(array9[2]), Convert.ToSingle(array9[3]), Convert.ToSingle(array9[4])));
								}
							}
							array8[num2] = array7[j];
							num2++;
						}
						string text8 = UnityEngine.Random.Range(10000, 99999).ToString();
						array8[array7.Length % 100] = text8;
						currentLevel += text8;
						levelCache.Add(array8);
					}
					List<string> list2 = new List<string>();
					foreach (Vector3 titanSpawn2 in titanSpawns)
					{
						string[] obj4 = new string[6] { "titan,", null, null, null, null, null };
						float x = titanSpawn2.x;
						obj4[1] = x.ToString();
						obj4[2] = ",";
						x = titanSpawn2.y;
						obj4[3] = x.ToString();
						obj4[4] = ",";
						x = titanSpawn2.z;
						obj4[5] = x.ToString();
						list2.Add(string.Concat(obj4));
					}
					foreach (Vector3 item3 in playerSpawnsC)
					{
						string[] obj5 = new string[6] { "playerC,", null, null, null, null, null };
						float x = item3.x;
						obj5[1] = x.ToString();
						obj5[2] = ",";
						x = item3.y;
						obj5[3] = x.ToString();
						obj5[4] = ",";
						x = item3.z;
						obj5[5] = x.ToString();
						list2.Add(string.Concat(obj5));
					}
					foreach (Vector3 item4 in playerSpawnsM)
					{
						string[] obj6 = new string[6] { "playerM,", null, null, null, null, null };
						float x = item4.x;
						obj6[1] = x.ToString();
						obj6[2] = ",";
						x = item4.y;
						obj6[3] = x.ToString();
						obj6[4] = ",";
						x = item4.z;
						obj6[5] = x.ToString();
						list2.Add(string.Concat(obj6));
					}
					string text9 = "a" + UnityEngine.Random.Range(10000, 99999);
					list2.Add(text9);
					currentLevel = text9 + currentLevel;
					levelCache.Insert(0, list2.ToArray());
					string text10 = "z" + UnityEngine.Random.Range(10000, 99999);
					levelCache.Add(new string[1] { text10 });
					currentLevel += text10;
					ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
					hashtable2.Add("currentLevel", currentLevel);
					PhotonNetwork.player.SetCustomProperties(hashtable2);
					oldScript = currentScript;
				}
			}
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonPlayer photonPlayer2 = PhotonNetwork.playerList[i];
				if (!photonPlayer2.isMasterClient)
				{
					playersRPC.Add(photonPlayer2);
				}
			}
			StartCoroutine(customlevelE(playersRPC));
			StartCoroutine(customlevelcache());
		}
	}

	private IEnumerator loadskinE(string n, string url, string url2, string[] skybox)
	{
		bool mipmap = true;
		bool iteratorVariable1 = false;
		if ((int)settings[63] == 0)
		{
			mipmap = false;
		}
		if (skybox.Length > 5 && (skybox[0] != string.Empty || skybox[1] != string.Empty || skybox[2] != string.Empty || skybox[3] != string.Empty || skybox[4] != string.Empty || skybox[5] != string.Empty))
		{
			string key = string.Join(",", skybox);
			if (!linkHash[1].ContainsKey(key))
			{
				iteratorVariable1 = true;
				Material material = Camera.main.GetComponent<Skybox>().material;
				string text = skybox[0];
				string iteratorVariable12 = skybox[1];
				string iteratorVariable13 = skybox[2];
				string iteratorVariable14 = skybox[3];
				string iteratorVariable15 = skybox[4];
				string iteratorVariable22 = skybox[5];
				if ((text.EndsWith(".jpg") || text.EndsWith(".png") || text.EndsWith(".jpeg")) && RCextensions.CheckIP(text))
				{
					WWW link13 = new WWW(text);
					yield return link13;
					Texture2D texture2D = RCextensions.loadimage(link13, mipmap, 500000);
					link13.Dispose();
					texture2D.wrapMode = TextureWrapMode.Clamp;
					material.SetTexture("_FrontTex", texture2D);
				}
				if ((iteratorVariable12.EndsWith(".jpg") || iteratorVariable12.EndsWith(".png") || iteratorVariable12.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable12))
				{
					WWW link13 = new WWW(iteratorVariable12);
					yield return link13;
					Texture2D texture2D2 = RCextensions.loadimage(link13, mipmap, 500000);
					link13.Dispose();
					texture2D2.wrapMode = TextureWrapMode.Clamp;
					material.SetTexture("_BackTex", texture2D2);
				}
				if ((iteratorVariable13.EndsWith(".jpg") || iteratorVariable13.EndsWith(".png") || iteratorVariable13.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable13))
				{
					WWW link13 = new WWW(iteratorVariable13);
					yield return link13;
					Texture2D texture2D3 = RCextensions.loadimage(link13, mipmap, 500000);
					link13.Dispose();
					texture2D3.wrapMode = TextureWrapMode.Clamp;
					material.SetTexture("_LeftTex", texture2D3);
				}
				if ((iteratorVariable14.EndsWith(".jpg") || iteratorVariable14.EndsWith(".png") || iteratorVariable14.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable14))
				{
					WWW link13 = new WWW(iteratorVariable14);
					yield return link13;
					Texture2D texture2D4 = RCextensions.loadimage(link13, mipmap, 500000);
					link13.Dispose();
					texture2D4.wrapMode = TextureWrapMode.Clamp;
					material.SetTexture("_RightTex", texture2D4);
				}
				if ((iteratorVariable15.EndsWith(".jpg") || iteratorVariable15.EndsWith(".png") || iteratorVariable15.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable15))
				{
					WWW link13 = new WWW(iteratorVariable15);
					yield return link13;
					Texture2D texture2D5 = RCextensions.loadimage(link13, mipmap, 500000);
					link13.Dispose();
					texture2D5.wrapMode = TextureWrapMode.Clamp;
					material.SetTexture("_UpTex", texture2D5);
				}
				if ((iteratorVariable22.EndsWith(".jpg") || iteratorVariable22.EndsWith(".png") || iteratorVariable22.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable22))
				{
					WWW link13 = new WWW(iteratorVariable22);
					yield return link13;
					Texture2D texture2D6 = RCextensions.loadimage(link13, mipmap, 500000);
					link13.Dispose();
					texture2D6.wrapMode = TextureWrapMode.Clamp;
					material.SetTexture("_DownTex", texture2D6);
				}
				Camera.main.GetComponent<Skybox>().material = material;
				skyMaterial = material;
				linkHash[1].Add(key, material);
			}
			else if (RCextensions.CheckIP(key))
			{
				Camera.main.GetComponent<Skybox>().material = (Material)linkHash[1][key];
				skyMaterial = (Material)linkHash[1][key];
			}
		}
		if (LevelInfo.getInfo(level).mapName.Contains("Forest"))
		{
			string[] iteratorVariable3 = url.Split(',');
			string[] iteratorVariable5 = url2.Split(',');
			int startIndex2 = 0;
			object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			object[] array2 = array;
			object[] array3 = array2;
			for (int i = 0; i < array3.Length; i++)
			{
				GameObject gameObject = (GameObject)array3[i];
				if (!(gameObject != null))
				{
					continue;
				}
				if (gameObject.name.Contains("TREE") && n.Length > startIndex2 + 1)
				{
					string text2 = n.Substring(startIndex2, 1);
					string text3 = n.Substring(startIndex2 + 1, 1);
					if (int.TryParse(text2, out var result) && int.TryParse(text3, out var result2) && result >= 0 && result < 8 && result2 >= 0 && result2 < 8 && iteratorVariable3.Length >= 8 && iteratorVariable5.Length >= 8 && iteratorVariable3[result] != null && iteratorVariable5[result2] != null)
					{
						string key = iteratorVariable3[result];
						string iteratorVariable22 = iteratorVariable5[result2];
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer iteratorVariable11 in componentsInChildren)
						{
							if (iteratorVariable11.name.Contains(s[22]))
							{
								if ((!key.EndsWith(".jpg") && !key.EndsWith(".png") && !key.EndsWith(".jpeg")) || !RCextensions.CheckIP(key))
								{
									continue;
								}
								if (!linkHash[2].ContainsKey(key))
								{
									WWW link13 = new WWW(key);
									yield return link13;
									Texture2D mainTexture = RCextensions.loadimage(link13, mipmap, 1000000);
									link13.Dispose();
									if (!linkHash[2].ContainsKey(key))
									{
										iteratorVariable1 = true;
										iteratorVariable11.material.mainTexture = mainTexture;
										linkHash[2].Add(key, iteratorVariable11.material);
										iteratorVariable11.material = (Material)linkHash[2][key];
									}
									else
									{
										iteratorVariable11.material = (Material)linkHash[2][key];
									}
								}
								else
								{
									iteratorVariable11.material = (Material)linkHash[2][key];
								}
							}
							else
							{
								if (!iteratorVariable11.name.Contains(s[23]))
								{
									continue;
								}
								if (iteratorVariable22.EndsWith(".jpg") || iteratorVariable22.EndsWith(".png") || iteratorVariable22.EndsWith(".jpeg"))
								{
									if (!RCextensions.CheckIP(iteratorVariable22))
									{
										continue;
									}
									if (!linkHash[0].ContainsKey(iteratorVariable22))
									{
										WWW link13 = new WWW(iteratorVariable22);
										yield return link13;
										Texture2D mainTexture2 = RCextensions.loadimage(link13, mipmap, 200000);
										link13.Dispose();
										if (!linkHash[0].ContainsKey(iteratorVariable22))
										{
											iteratorVariable1 = true;
											iteratorVariable11.material.mainTexture = mainTexture2;
											linkHash[0].Add(iteratorVariable22, iteratorVariable11.material);
											iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
										}
										else
										{
											iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
										}
									}
									else
									{
										iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
									}
								}
								else if (iteratorVariable22.ToLower() == "transparent")
								{
									iteratorVariable11.enabled = false;
								}
							}
						}
					}
					startIndex2 += 2;
				}
				else
				{
					if (!gameObject.name.Contains("Cube_001") || !(gameObject.transform.parent.gameObject.tag != "Player") || iteratorVariable5.Length <= 8 || iteratorVariable5[8] == null)
					{
						continue;
					}
					string iteratorVariable22 = iteratorVariable5[8];
					if (iteratorVariable22.EndsWith(".jpg") || iteratorVariable22.EndsWith(".png") || iteratorVariable22.EndsWith(".jpeg"))
					{
						if (!RCextensions.CheckIP(iteratorVariable22))
						{
							continue;
						}
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer iteratorVariable11 in componentsInChildren)
						{
							if (!linkHash[0].ContainsKey(iteratorVariable22))
							{
								WWW link13 = new WWW(iteratorVariable22);
								yield return link13;
								Texture2D mainTexture3 = RCextensions.loadimage(link13, mipmap, 200000);
								link13.Dispose();
								if (!linkHash[0].ContainsKey(iteratorVariable22))
								{
									iteratorVariable1 = true;
									iteratorVariable11.material.mainTexture = mainTexture3;
									linkHash[0].Add(iteratorVariable22, iteratorVariable11.material);
									iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
								}
								else
								{
									iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
								}
							}
							else
							{
								iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
							}
						}
					}
					else if (iteratorVariable22.ToLower() == "transparent")
					{
						Renderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<Renderer>();
						for (int k = 0; k < componentsInChildren2.Length; k++)
						{
							componentsInChildren2[k].enabled = false;
						}
					}
				}
			}
		}
		else if (LevelInfo.getInfo(level).mapName.Contains("City"))
		{
			string[] iteratorVariable5 = url.Split(',');
			string[] iteratorVariable3 = url2.Split(',');
			_ = iteratorVariable3[2];
			int startIndex2 = 0;
			object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			object[] array4 = array;
			object[] array3 = array4;
			for (int i = 0; i < array3.Length; i++)
			{
				GameObject gameObject2 = (GameObject)array3[i];
				if (!(gameObject2 != null) || !gameObject2.name.Contains("Cube_") || !(gameObject2.transform.parent.gameObject.tag != "Player"))
				{
					continue;
				}
				if (gameObject2.name.EndsWith("001"))
				{
					if (iteratorVariable3.Length == 0 || iteratorVariable3[0] == null)
					{
						continue;
					}
					string iteratorVariable22 = iteratorVariable3[0];
					if (iteratorVariable22.EndsWith(".jpg") || iteratorVariable22.EndsWith(".png") || iteratorVariable22.EndsWith(".jpeg"))
					{
						if (!RCextensions.CheckIP(iteratorVariable22))
						{
							continue;
						}
						Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
						foreach (Renderer iteratorVariable11 in componentsInChildren)
						{
							if (!linkHash[0].ContainsKey(iteratorVariable22))
							{
								WWW link13 = new WWW(iteratorVariable22);
								yield return link13;
								Texture2D mainTexture4 = RCextensions.loadimage(link13, mipmap, 200000);
								link13.Dispose();
								if (!linkHash[0].ContainsKey(iteratorVariable22))
								{
									iteratorVariable1 = true;
									iteratorVariable11.material.mainTexture = mainTexture4;
									linkHash[0].Add(iteratorVariable22, iteratorVariable11.material);
									iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
								}
								else
								{
									iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
								}
							}
							else
							{
								iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
							}
						}
					}
					else if (iteratorVariable22.ToLower() == "transparent")
					{
						Renderer[] componentsInChildren2 = gameObject2.GetComponentsInChildren<Renderer>();
						for (int k = 0; k < componentsInChildren2.Length; k++)
						{
							componentsInChildren2[k].enabled = false;
						}
					}
				}
				else if (gameObject2.name.EndsWith("006") || gameObject2.name.EndsWith("007") || gameObject2.name.EndsWith("015") || gameObject2.name.EndsWith("000") || (gameObject2.name.EndsWith("002") && gameObject2.transform.position.x == 0f && gameObject2.transform.position.y == 0f && gameObject2.transform.position.z == 0f))
				{
					if (iteratorVariable3.Length == 0 || iteratorVariable3[1] == null)
					{
						continue;
					}
					string iteratorVariable22 = iteratorVariable3[1];
					if ((!iteratorVariable22.EndsWith(".jpg") && !iteratorVariable22.EndsWith(".png") && !iteratorVariable22.EndsWith(".jpeg")) || !RCextensions.CheckIP(iteratorVariable22))
					{
						continue;
					}
					Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
					foreach (Renderer iteratorVariable11 in componentsInChildren)
					{
						if (!linkHash[0].ContainsKey(iteratorVariable22))
						{
							WWW link13 = new WWW(iteratorVariable22);
							yield return link13;
							Texture2D mainTexture5 = RCextensions.loadimage(link13, mipmap, 200000);
							link13.Dispose();
							if (!linkHash[0].ContainsKey(iteratorVariable22))
							{
								iteratorVariable1 = true;
								iteratorVariable11.material.mainTexture = mainTexture5;
								linkHash[0].Add(iteratorVariable22, iteratorVariable11.material);
								iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
							}
							else
							{
								iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
							}
						}
						else
						{
							iteratorVariable11.material = (Material)linkHash[0][iteratorVariable22];
						}
					}
				}
				else if (gameObject2.name.EndsWith("005") || gameObject2.name.EndsWith("003") || (gameObject2.name.EndsWith("002") && (gameObject2.transform.position.x != 0f || gameObject2.transform.position.y != 0f || gameObject2.transform.position.z != 0f) && n.Length > startIndex2))
				{
					if (int.TryParse(n.Substring(startIndex2, 1), out var result3) && result3 >= 0 && result3 < 8 && iteratorVariable5.Length >= 8 && iteratorVariable5[result3] != null)
					{
						string iteratorVariable22 = iteratorVariable5[result3];
						if ((iteratorVariable22.EndsWith(".jpg") || iteratorVariable22.EndsWith(".png") || iteratorVariable22.EndsWith(".jpeg")) && RCextensions.CheckIP(iteratorVariable22))
						{
							Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
							foreach (Renderer iteratorVariable11 in componentsInChildren)
							{
								if (!linkHash[2].ContainsKey(iteratorVariable22))
								{
									WWW link13 = new WWW(iteratorVariable22);
									yield return link13;
									Texture2D mainTexture6 = RCextensions.loadimage(link13, mipmap, 1000000);
									link13.Dispose();
									if (!linkHash[2].ContainsKey(iteratorVariable22))
									{
										iteratorVariable1 = true;
										iteratorVariable11.material.mainTexture = mainTexture6;
										linkHash[2].Add(iteratorVariable22, iteratorVariable11.material);
										iteratorVariable11.material = (Material)linkHash[2][iteratorVariable22];
									}
									else
									{
										iteratorVariable11.material = (Material)linkHash[2][iteratorVariable22];
									}
								}
								else
								{
									iteratorVariable11.material = (Material)linkHash[2][iteratorVariable22];
								}
							}
						}
					}
					startIndex2++;
				}
				else
				{
					if ((!gameObject2.name.EndsWith("019") && !gameObject2.name.EndsWith("020")) || iteratorVariable3.Length <= 2 || iteratorVariable3[2] == null)
					{
						continue;
					}
					string iteratorVariable22 = iteratorVariable3[2];
					if ((!iteratorVariable22.EndsWith(".jpg") && !iteratorVariable22.EndsWith(".png") && !iteratorVariable22.EndsWith(".jpeg")) || !RCextensions.CheckIP(iteratorVariable22))
					{
						continue;
					}
					Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
					foreach (Renderer iteratorVariable11 in componentsInChildren)
					{
						if (!linkHash[2].ContainsKey(iteratorVariable22))
						{
							WWW link13 = new WWW(iteratorVariable22);
							yield return link13;
							Texture2D mainTexture7 = RCextensions.loadimage(link13, mipmap, 1000000);
							link13.Dispose();
							if (!linkHash[2].ContainsKey(iteratorVariable22))
							{
								iteratorVariable1 = true;
								iteratorVariable11.material.mainTexture = mainTexture7;
								linkHash[2].Add(iteratorVariable22, iteratorVariable11.material);
								iteratorVariable11.material = (Material)linkHash[2][iteratorVariable22];
							}
							else
							{
								iteratorVariable11.material = (Material)linkHash[2][iteratorVariable22];
							}
						}
						else
						{
							iteratorVariable11.material = (Material)linkHash[2][iteratorVariable22];
						}
					}
				}
			}
		}
		if (iteratorVariable1)
		{
			unloadAssets();
		}
	}

	[RPC]
	private void loadskinRPC(string n, string url, string url2, string[] skybox, PhotonMessageInfo info)
	{
		if ((info != null) & !info.sender.isMasterClient)
		{
			ThrowJusticeDefensive(info.sender, "invalid loadSkinRPC");
		}
		else if ((int)settings[2] == 1 && info.sender.isMasterClient)
		{
			if (!LevelInfo.getInfo(level).mapName.Contains("City"))
			{
				StartCoroutine(loadskinE(n, url, url2, skybox));
			}
			else if (PhotonNetwork.isMasterClient || GameSettings.bombMode == 0)
			{
				StartCoroutine(loadskinE(n, url, url2, skybox));
			}
		}
	}

	public string mastertexturetype(int lol)
	{
		return lol switch
		{
			0 => "High", 
			1 => "Med", 
			2 => "Low", 
			3 => "Very Low", 
			_ => "Ultra Low", 
		};
	}

	public void multiplayerRacingFinsih()
	{
		float num = roundTime - Convert.ToSingle(race_timer);
		if (PhotonNetwork.isMasterClient)
		{
			getRacingResult(LoginFengKAI.player.name, num, new PhotonMessageInfo(PhotonNetwork.player, PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds, null));
		}
		else
		{
			object[] parameters = new object[2]
			{
				LoginFengKAI.player.name,
				num
			};
			PView.RPC("getRacingResult", PhotonTargets.MasterClient, parameters);
		}
		gameWin2();
	}

	[RPC]
	private void netGameLose(int score, PhotonMessageInfo info)
	{
		isLosing = true;
		titanScore = score;
		gameEndCD = gameEndTotalCDtime;
		if ((int)settings[244] != 0)
		{
			if ((int)settings[244] == 1)
			{
				InRoomChat.Write("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game lost).");
			}
			else
			{
				FleurConsole.instance.Log("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game lost).", FleurConsole.DebugType.LOG);
			}
		}
		if (info.sender != PhotonNetwork.masterClient && !info.sender.isLocal && PhotonNetwork.isMasterClient)
		{
			InRoomChat.ChatInstanse.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID + "</color>");
		}
	}

	[RPC]
	private void netGameWin(int score, PhotonMessageInfo info)
	{
		humanScore = score;
		isWinning = true;
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			teamWinner = score;
			teamScores[teamWinner - 1]++;
			gameEndCD = gameEndTotalCDtime;
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			if (GameSettings.racingStatic == 1)
			{
				gameEndCD = 1000f;
			}
			else
			{
				gameEndCD = 20f;
			}
		}
		else
		{
			gameEndCD = gameEndTotalCDtime;
		}
		if ((int)settings[244] != 0)
		{
			if ((int)settings[244] == 1)
			{
				InRoomChat.Write("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).");
			}
			else
			{
				FleurConsole.instance.Log("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).", FleurConsole.DebugType.LOG);
			}
		}
		if (info.sender != PhotonNetwork.masterClient && !info.sender.isLocal)
		{
			InRoomChat.ChatInstanse.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID + "</color>");
		}
	}

	[RPC]
	private void netRefreshRacingResult(string tmp)
	{
		localRacingResult = tmp;
	}

	[RPC]
	public void netShowDamage(int speed)
	{
		StylishComponent.stylebar.Style(speed);
		if (LabelScore != null)
		{
			LabelScoreUI.text = speed.ToString();
			LabelScoreT.localScale = Vector3.zero;
			speed = (int)((float)speed * 0.1f);
			speed = Mathf.Max(40, speed);
			speed = Mathf.Min(150, speed);
			speed = (int)((float)speed * YellowDamageSize);
			iTween.Stop(LabelScore);
			object[] args = new object[10]
			{
				"x",
				speed,
				"y",
				speed,
				"z",
				speed,
				"easetype",
				iTween.EaseType.easeOutElastic,
				"time",
				1f
			};
			iTween.ScaleTo(LabelScore, iTween.Hash(args));
			object[] args2 = new object[12]
			{
				"x",
				0,
				"y",
				0,
				"z",
				0,
				"easetype",
				iTween.EaseType.easeInBounce,
				"time",
				0.5f,
				"delay",
				2f
			};
			iTween.ScaleTo(LabelScore, iTween.Hash(args2));
		}
	}

	public void NOTSpawnNonAITitan(string id)
	{
		myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("dead", true);
		hashtable.Add("isTitan", 2);
		PhotonNetwork.player.SetCustomProperties(hashtable);
		if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
		{
			Screen.lockCursor = true;
		}
		else
		{
			Screen.lockCursor = false;
		}
		Screen.showCursor = true;
		ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
		IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
		IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
		IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
		IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
	}

	public void NOTSpawnNonAITitanRC(string id)
	{
		myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("dead", true);
		hashtable.Add("isTitan", 2);
		PhotonNetwork.player.SetCustomProperties(hashtable);
		if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
		{
			Screen.lockCursor = true;
		}
		else
		{
			Screen.lockCursor = false;
		}
		Screen.showCursor = true;
		ShowHUDInfoCenter("Syncing spawn locations...");
		IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
		IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
		IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
		IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
	}

	public void NOTSpawnPlayer(string id)
	{
		myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("dead", true);
		hashtable.Add("isTitan", 1);
		PhotonNetwork.player.SetCustomProperties(hashtable);
		if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
		{
			Screen.lockCursor = true;
		}
		else
		{
			Screen.lockCursor = false;
		}
		Screen.showCursor = false;
		ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
		IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
		IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
		IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
		IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
	}

	public void NOTSpawnPlayerRC(string id)
	{
		myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("dead", true);
		hashtable.Add("isTitan", 1);
		PhotonNetwork.player.SetCustomProperties(hashtable);
		if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
		{
			Screen.lockCursor = true;
		}
		else
		{
			Screen.lockCursor = false;
		}
		Screen.showCursor = false;
		IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
		IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
		IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
		IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
	}

	private static bool ObjectListCheck(string key)
	{
		key = key.ToLower();
		if (key.EndsWith("(clone)"))
		{
			key = key.Remove(key.Length - 7);
		}
		switch (key)
		{
		case "hook":
		case "fx/justSmoke":
		case "fx/boost_smoke":
		case "aottg_hero 1":
		case "hitmeat":
		case "hitmeat2":
		case "redcross":
		case "redcross1":
		case "shotgun":
		case "shotgun 1":
		case "fx/fxtitandie":
		case "fx/fxtitandie1":
		case "fx/fxtitanspawn":
		case "fx/bite":
		case "fxtitandie":
		case "fxtitandie1":
		case "fxtitanspawn":
		case "bite":
		case "justSmoke":
		case "boost_smoke":
			return false;
		default:
			return true;
		}
	}

	public void OnConnectionFail(DisconnectCause cause)
	{
		this.cause = cause.ToString();
		FleurConsole.instance.Log("Disconnected by " + cause, FleurConsole.DebugType.LOG);
		alltitans.Clear();
		if (PhotonNetwork.networkingPeer.UsedProtocol != ConnectionProtocol.WebSocket)
		{
			if (cause == DisconnectCause.DisconnectByServerLogic || cause == DisconnectCause.DisconnectByServer || cause == DisconnectCause.Exception)
			{
				shallRejoin[0] = true;
				timeofAutorejoin.Start();
				ignoreList.Clear();
			}
			if (cause == DisconnectCause.DisconnectByClientTimeout)
			{
				if (GameSettings.BotNoConnection > 0)
				{
					//AudioSource.PlayClipAtPoint(BotNoConnection, Camera.main.transform.position, AssistantVolume);
				}
				FleurConsole.instance.Log("Net Vanish ", FleurConsole.DebugType.STATE);
				shallRejoin[0] = true;
				timeofAutorejoin.Start();
				ignoreList.Clear();
			}
		}
		Screen.lockCursor = false;
		Screen.showCursor = true;
		IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
		gameStart = false;
		if (cause == DisconnectCause.MaxCcuReached)
		{
			GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
			UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
			PhotonNetwork.Disconnect();
			UpdateStatus = "Max limit of people";
		}
		if (cause == DisconnectCause.DisconnectByClientTimeout)
		{
			NGUITools.SetActive(uirefer.panels[0], state: false);
			NGUITools.SetActive(uirefer.panels[1], state: false);
			NGUITools.SetActive(uirefer.panels[2], state: false);
			CharacterSelection.characterSelectionIsVisible = false;
			NGUITools.SetActive(uirefer.panels[4], state: true);
			CacheGameObject.Find("LabelDisconnectInfo").GetComponent<UILabel>().text = "OnConnectionFail : " + cause;
		}
	}

	public void OnCreatedRoom()
	{
		mapScript = string.Empty;
		script_text_field = string.Empty;
		customMapScripts = false;
		is_skin_gui = false;
		kicklist = new ArrayList();
		racingResult = new ArrayList();
		teamScores = new int[2];
	}

	public void OnCustomAuthenticationFailed()
	{
		GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
		UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
		Application.LoadLevel("menu");
		PhotonNetwork.Disconnect();
		UpdateStatus = "Invalid AppID";
	}

	public void OnDisconnectedFromPhoton()
	{
		Screen.lockCursor = false;
		Screen.showCursor = true;
	}

	[RPC]
	public void oneTitanDown(string name1, bool onPlayerLeave)
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !PhotonNetwork.isMasterClient)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			if (name1 != string.Empty)
			{
				switch (name1)
				{
				case "Titan":
					PVPhumanScore++;
					break;
				case "Aberrant":
					PVPhumanScore += 2;
					break;
				case "Jumper":
					PVPhumanScore += 3;
					break;
				case "Crawler":
					PVPhumanScore += 4;
					break;
				case "Female Titan":
					PVPhumanScore += 10;
					break;
				default:
					PVPhumanScore += 3;
					break;
				}
			}
			checkPVPpts();
			object[] parameters = new object[2] { PVPhumanScore, PVPtitanScore };
			PView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.CAGE_FIGHT)
		{
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN)
			{
				if (checkIsTitanAllDie() && GameSettings.bombMode == 0)
				{
					gameWin2();
					IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
			{
				if (checkIsTitanAllDie() && GameSettings.bombMode == 0)
				{
					wave++;
					if ((LevelInfo.getInfo(level).respawnMode == RespawnMode.NEWROUND || (level.StartsWith("Custom") && GameSettings.gameType == 1)) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						PhotonPlayer[] array = PhotonNetwork.playerList;
						foreach (PhotonPlayer photonPlayer in array)
						{
							if (RCextensions.returnIntFromObject(photonPlayer.customProperties["isTitan"]) != 2)
							{
								PView.RPC("respawnHeroInNewRound", photonPlayer);
							}
						}
					}
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						sendChatContentInfo("<color=#A8FF24>Wave : " + wave + "</color>");
					}
					if (wave > highestwave)
					{
						highestwave = wave;
					}
					if (PhotonNetwork.isMasterClient)
					{
						RequireStatus();
					}
					if ((GameSettings.maxWave == 0 && wave > 20) || (GameSettings.maxWave > 0 && wave > GameSettings.maxWave))
					{
						gameWin2();
					}
					else
					{
						int abnormal = 90;
						if (difficulty == 1)
						{
							abnormal = 70;
						}
						if (!LevelInfo.getInfo(level).punk)
						{
							spawnTitanCustom("titanRespawn", abnormal, wave + 2, punk: false);
						}
						else if (wave == 5)
						{
							spawnTitanCustom("titanRespawn", abnormal, 1, punk: true);
						}
						else if (wave == 10)
						{
							spawnTitanCustom("titanRespawn", abnormal, 2, punk: true);
						}
						else if (wave == 15)
						{
							spawnTitanCustom("titanRespawn", abnormal, 3, punk: true);
						}
						else if (wave == 20)
						{
							spawnTitanCustom("titanRespawn", abnormal, 4, punk: true);
						}
						else
						{
							spawnTitanCustom("titanRespawn", abnormal, wave + 2, punk: false);
						}
					}
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
			{
				if (!onPlayerLeave)
				{
					humanScore++;
					int abnormal2 = 90;
					if (difficulty == 1)
					{
						abnormal2 = 70;
					}
					spawnTitanCustom("titanRespawn", abnormal2, 1, punk: false);
				}
			}
			else
			{
				_ = LevelInfo.getInfo(level).enemyNumber;
				_ = -1;
			}
		}
		if (CheatEngine)
		{
			setDrunkPunks();
		}
	}

	private void setDrunkPunks()
	{
		if (wave == 5 || wave == 10 || wave == 15)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<TITAN>().photonView.RPC("netSetLevel", PhotonTargets.Local, 50, GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().difficulty, UnityEngine.Random.Range(0, 4));
			}
			array = GameObject.FindGameObjectsWithTag("titan");
			foreach (GameObject obj in array)
			{
				obj.rigidbody.freezeRotation = false;
				obj.rigidbody.useGravity = true;
			}
		}
	}

	public void OnFailedToConnectToPhoton()
	{
		GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
		UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
		Application.LoadLevel("menu");
		PhotonNetwork.Disconnect();
	}

	public static Vector2 BoundsOfText(string text)
	{
		return new GUIStyle(GUI.skin.label)
		{
			fontSize = 12
		}.CalcSize(new GUIContent(text));
	}

	private void MyFpsInGUI()
	{
		MiscStyle = new GUIStyle(GUI.skin.label);
		MiscStyle.fontSize = 12;
		if (sFPS != string.Empty)
		{
			MiscStyle.normal.textColor = new Color(AHexColorFloats0, AHexColorFloats1, AHexColorFloats2, 1f);
			Vector2 vector = BoundsOfText(sFPS);
			GUI.Label(new Rect((float)Screen.width / 2f + 210f, 1f, vector.x, vector.y), sFPS, MiscStyle);
		}
	}

	private void GameSettingsMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		_ = (float)Screen.height / 2f;
		float num2 = (float)Screen.width / 2f - 350f;
		float num3 = (float)Screen.height / 2f - 250f;
		Rect screenRect = new Rect(num2 + 30f, num3 + 55f, 355f, 445f);
		Rect screenRect2 = new Rect(num2 + 400f, num3 + 55f, 305f, 430f);
		string[] texts = new string[2] { "Page 1", "Page 2" };
		PagesNumGame = GUI.SelectionGrid(new Rect(num + 520f, num3 + 70f, 160f, 20f), PagesNumGame, texts, 2);
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginVertical();
		GUILayout.Space(20f);
		Rendering.SubHeader("Mouse");
		GUILayout.Space(5f);
		Rendering.Slider("Mouse Sensivity", round: true, 100f, ref mouseSlider, 0.01f, 1.2f, "0", 130f);
		PlayerPrefs.SetFloat("MouseSensitivity", mouseSlider);
		IN_GAME_MAIN_CAMERA.sensitivityMulti = PlayerPrefs.GetFloat("MouseSensitivity");
		GUILayout.Space(40f);
		Rendering.SubHeader("Camera");
		GUILayout.Space(5f);
		Rendering.Slider("Camera Distance", round: true, 100f, ref distanceSlider, 0f, 1f, "0", 130f);
		PlayerPrefs.SetFloat("cameraDistance", distanceSlider);
		IN_GAME_MAIN_CAMERA.cameraDistance = 0.3f + distanceSlider;
		GUILayout.BeginHorizontal();
		Rendering.Label("Tps Movement", 0, 80f, 20f);
		GUILayout.Space(50f);
		string[] texts2 = new string[2] { "Smoothing", "Raw" };
		settings[300] = GUILayout.SelectionGrid((int)settings[300], texts2, 2, GUILayout.Width(160f), GUILayout.Height(20f));
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Camera Tilt", 0, 80f, 20f);
		GUILayout.Space(50f);
		if (PlayerPrefs.HasKey("cameraTilt"))
		{
			if (PlayerPrefs.GetInt("cameraTilt") == 1)
			{
				ActDisact[1] = "On";
			}
			else
			{
				ActDisact[1] = "Off";
			}
		}
		else
		{
			PlayerPrefs.SetInt("cameraTilt", 1);
		}
		if (GUILayout.Button(ActDisact[1], GUILayout.Width(40f), GUILayout.Height(17f)))
		{
			if (PlayerPrefs.GetInt("cameraTilt") == 0)
			{
				PlayerPrefs.SetInt("cameraTilt", 1);
			}
			else
			{
				PlayerPrefs.SetInt("cameraTilt", 0);
			}
		}
		IN_GAME_MAIN_CAMERA.cameraTilt = PlayerPrefs.GetInt("cameraTilt");
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		Rendering.Slider("Tilt Rotation", round: true, 10f, ref tiltSlider, 1f, 9f, "0", 130f);
		PlayerPrefs.SetFloat("tiltSlider", tiltSlider);
		GUILayout.Space(30f);
		Rendering.SubHeader("Snapshots");
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Enable Snapshots", 0, 80f, 20f);
		GUILayout.Space(50f);
		if (PlayerPrefs.GetInt("EnableSS", 0) == 1)
		{
			ActDisact[3] = "On";
		}
		else
		{
			ActDisact[3] = "Off";
		}
		if (GUILayout.Button(ActDisact[3], GUILayout.Width(65f), GUILayout.Height(17f)))
		{
			if (PlayerPrefs.GetInt("EnableSS", 0) == 0)
			{
				PlayerPrefs.SetInt("EnableSS", 1);
			}
			else
			{
				PlayerPrefs.SetInt("EnableSS", 0);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Show in game", 0, 80f, 20f);
		GUILayout.Space(50f);
		if (PlayerPrefs.GetInt("showSSInGame", 0) == 1)
		{
			ActDisact[4] = "On";
		}
		else
		{
			ActDisact[4] = "Off";
		}
		if (GUILayout.Button(ActDisact[4], GUILayout.Width(65f), GUILayout.Height(17f)))
		{
			if (PlayerPrefs.GetInt("showSSInGame", 0) == 0)
			{
				PlayerPrefs.SetInt("showSSInGame", 1);
			}
			else
			{
				PlayerPrefs.SetInt("showSSInGame", 0);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Minimum damage", 0, 80f, 20f);
		GUILayout.Space(50f);
		settings[95] = GUILayout.TextField((string)settings[95], GUILayout.Width(65f), GUILayout.Height(20f));
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
		GUILayout.BeginArea(screenRect2);
		GUILayout.BeginVertical();
		GUILayout.Space(20f);
		if (PagesNumGame == 0)
		{
			Rendering.SubHeader("Local");
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Speedometer", 0, 80f, 20f);
			GUILayout.Space(15f);
			string[] texts3 = new string[3] { "Off", "Speed", "Damage" };
			settings[189] = GUILayout.SelectionGrid((int)settings[189], texts3, 3, GUILayout.Width(185f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Game Feed", 0, 80f, 20f);
			GUILayout.Space(15f);
			string[] texts4 = new string[3] { "Off", "Chat", "Console" };
			settings[244] = GUILayout.SelectionGrid((int)settings[244], texts4, 3, GUILayout.Width(185f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Feed Counter", 0, 80f, 20f);
			GUILayout.Space(15f);
			string[] texts5 = new string[2] { "Off", "On Kill" };
			settings[281] = GUILayout.SelectionGrid((int)settings[281], texts5, 2, GUILayout.Width(185f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Pace Counter", 0, 80f, 20f);
			GUILayout.Space(15f);
			string[] texts6 = new string[3] { "DMG", "Kills", "None" };
			GameSettings.Pace = GUILayout.SelectionGrid(GameSettings.Pace, texts6, 3, GUILayout.Width(185f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Reel in", 0, 80f, 20f);
			GUILayout.Space(15f);
			string[] texts7 = new string[2] { "Once", "Multiple" };
			settings[280] = GUILayout.SelectionGrid((int)settings[280], texts7, 2, GUILayout.Width(185f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Race Timer", 0, 80f, 20f);
			GUILayout.Space(55f);
			if (PhotonNetwork.isMasterClient)
			{
				race_timer = GUILayout.TextField(race_timer, GUILayout.Width(145f), GUILayout.Height(20f));
			}
			else
			{
				GUILayout.Label(race_timer, GUILayout.Width(145f), GUILayout.Height(20f));
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("1 Life Racing:", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.OneLifeRacing = GUILayout.SelectionGrid(GameSettings.OneLifeRacing, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Smoothen (no custom):", 0, 80f, 20f);
			GUILayout.Space(55f);
			bool flag = false;
			if (GameSettings.DeleteRacing == 1)
			{
				flag = true;
			}
			bool flag2 = GUILayout.Toggle(flag, "On", GUILayout.Width(40f), GUILayout.Height(20f));
			if (flag != flag2)
			{
				if (flag2)
				{
					GameSettings.DeleteRacing = 1;
					PlayerPrefs.SetInt("DeleteRacing", GameSettings.DeleteRacing);
					string path = Environment.CurrentDirectory + "\\Resolution.txt";
					if (File.ReadAllLines(path).ElementAtOrDefault(3).Contains("Smoothen False"))
					{
						string text = File.ReadAllText(path);
						text = text.Replace("Smoothen False", "Smoothen True");
						File.WriteAllText(path, text);
						InRoomChat.Write("Custom maps are fucked but the used memory is less. Reopen the mod.");
					}
				}
				else
				{
					GameSettings.DeleteRacing = 0;
					PlayerPrefs.SetInt("DeleteRacing", GameSettings.DeleteRacing);
					string path2 = Environment.CurrentDirectory + "\\Resolution.txt";
					if (File.ReadAllLines(path2).ElementAtOrDefault(3).Contains("Smoothen True"))
					{
						string text2 = File.ReadAllText(path2);
						text2 = text2.Replace("Smoothen True", "Smoothen False");
						File.WriteAllText(path2, text2);
						InRoomChat.Write("All custom elements work again. Reopen the mod.");
					}
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(15f);
			Rendering.SubHeader("Networking");
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("On Disconnect", 0, 80f, 20f);
			GUILayout.Space(15f);
			string[] texts8 = new string[2] { "Rejoin", "Rehost" };
			settings[336] = GUILayout.SelectionGrid((int)settings[336], texts8, 2, GUILayout.Width(185f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			string[] texts9 = new string[2] { "<size=14>UDP</size>", "<size=14>TCP</size>" };
			Rendering.Label("Protocol", 0, 80f, 20f);
			GUILayout.Space(15f);
			settings[369] = GUILayout.SelectionGrid((int)settings[369], texts9, 2, GUILayout.Width(185f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("AntiGuest", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.AntiGuest = GUILayout.SelectionGrid(GameSettings.AntiGuest, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
		}
		else if (PagesNumGame == 1)
		{
			Rendering.SubHeader("Translator");
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Show original message", 0, 80f, 20f);
			GUILayout.Space(90f);
			if ((int)settings[282] == 1)
			{
				ActDisact[49] = "On";
			}
			else
			{
				ActDisact[49] = "Off";
			}
			if (GUILayout.Button(ActDisact[49], GUILayout.Width(75f), GUILayout.Height(17f)))
			{
				if ((int)settings[282] == 0)
				{
					settings[282] = 1;
				}
				else
				{
					settings[282] = 0;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			Rendering.Label("Translate incoming mess.", 0, 80f, 20f);
			GUILayout.Space(90f);
			if ((int)settings[285] == 1)
			{
				ActDisact[5] = "On";
			}
			else
			{
				ActDisact[5] = "Off";
			}
			if (GUILayout.Button(ActDisact[5], GUILayout.Width(75f), GUILayout.Height(17f)))
			{
				if ((int)settings[285] == 0)
				{
					settings[285] = 1;
				}
				else
				{
					settings[285] = 0;
				}
			}
			GUILayout.EndHorizontal();
			if ((int)settings[285] == 1)
			{
				GUILayout.BeginHorizontal();
				Rendering.Label("To:", 0, 20f, 20f);
				GUILayout.Space(15f);
				string[] texts10 = new string[9] { "English", "Spanish", "Portuguese", "Russian", "Philippine", "Thai", "French", "Italian", "Turkish" };
				settings[326] = GUILayout.SelectionGrid((int)settings[326], texts10, 3, GUILayout.Width(240f), GUILayout.Height(65f));
				GUILayout.EndHorizontal();
				GUILayout.Space(25f);
			}
			GUILayout.BeginHorizontal();
			Rendering.Label("Translate outgoing mess.", 0, 80f, 20f);
			GUILayout.Space(90f);
			if ((int)settings[324] == 1)
			{
				ActDisact[6] = "On";
			}
			else
			{
				ActDisact[6] = "Off";
			}
			if (GUILayout.Button(ActDisact[6], GUILayout.Width(75f), GUILayout.Height(17f)))
			{
				if ((int)settings[324] == 0)
				{
					settings[324] = 1;
				}
				else
				{
					settings[324] = 0;
				}
			}
			GUILayout.EndHorizontal();
			if ((int)settings[324] == 1)
			{
				GUILayout.BeginHorizontal();
				Rendering.Label("To:", 0, 20f, 20f);
				GUILayout.Space(15f);
				string[] texts11 = new string[9] { "English", "Spanish", "Portuguese", "Russian", "Philippine", "Thai", "French", "Italian", "Turkish" };
				settings[329] = GUILayout.SelectionGrid((int)settings[329], texts11, 3, GUILayout.Width(240f), GUILayout.Height(65f));
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(30f);
			Rendering.Label("<size=12>Translator might take a second slower to\n translate, be sure to turn if off if you don't use it</size>", 0, 130f, 60f);
		}
		GUILayout.Space(40f);
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void RebindsMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		if (GUI.Button(new Rect(num + 233f, num2 + 66f, 55f, 25f), "Human"))
		{
			settings[190] = 0;
		}
		else if (GUI.Button(new Rect(num + 293f, num2 + 66f, 52f, 25f), "Titan"))
		{
			settings[190] = 1;
		}
		else if (GUI.Button(new Rect(num + 350f, num2 + 66f, 53f, 25f), "Horse"))
		{
			settings[190] = 2;
		}
		else if (GUI.Button(new Rect(num + 408f, num2 + 66f, 59f, 25f), "Cannon"))
		{
			settings[190] = 3;
		}
		else if (GUI.Button(new Rect(num + 472f, num2 + 66f, 59f, 25f), "Extras"))
		{
			settings[190] = 4;
		}
		if ((int)settings[190] == 0)
		{
			GUI.Label(new Rect(num + 80f, num2 + 475f, 100f, 22f), "<color=#" + finalhex() + "><size=14>Burst:</size></color>", "Label");
			if (GUI.Button(new Rect(num + 190f, num2 + 475f, 120f, 20f), (string)settings[182]))
			{
				settings[182] = "waiting...";
				settings[100] = 182;
			}
			GUI.Label(new Rect(num + 390f, num2 + 450f, 100f, 22f), "<color=#" + finalhex() + ">Burst Type:</color>", "Label");
			string[] texts = new string[3] { "<size=10>Double Tap</size>", "<size=13>Rebind</size>", "<size=14>Both</size>" };
			settings[284] = GUI.SelectionGrid(new Rect(num + 500f, num2 + 450f, 195f, 25f), (int)settings[284], texts, 3);
			List<string> list = new List<string>
			{
				"<color=#" + finalhex() + ">Forward:</color>",
				"<color=#" + finalhex() + ">Backward:</color>",
				"<color=#" + finalhex() + ">Left:</color>",
				"<color=#" + finalhex() + ">Right:</color>",
				"<color=#" + finalhex() + ">Jump:</color>",
				"<color=#" + finalhex() + ">Dodge:</color>",
				"<color=#" + finalhex() + ">Left Hook:</color>",
				"<color=#" + finalhex() + ">Right Hook:</color>",
				"<color=#" + finalhex() + ">Both Hooks:</color>",
				"<color=#" + finalhex() + ">Lock:</color>",
				"<color=#" + finalhex() + ">Attack:</color>",
				"<color=#" + finalhex() + ">Special:</color>",
				"<color=#" + finalhex() + ">Salute:</color>",
				"<color=#" + finalhex() + ">Change Camera:</color>",
				"<color=#" + finalhex() + ">Reset:</color>",
				"<color=#" + finalhex() + ">Pause:</color>",
				"<color=#" + finalhex() + ">Show/Hide Cursor:</color>",
				"<color=#" + finalhex() + ">Fullscreen:</color>",
				"<color=#" + finalhex() + ">Change Blade:</color>",
				"<color=#" + finalhex() + ">Flare Green:</color>",
				"<color=#" + finalhex() + ">Flare Red:</color>",
				"<color=#" + finalhex() + ">Flare Black:</color>",
				"<color=#" + finalhex() + ">Reel in:</color>",
				"<color=#" + finalhex() + ">Reel out:</color>",
				"<color=#" + finalhex() + ">Minimap Max:</color>",
				"<color=#" + finalhex() + ">Minimap Toggle:</color>",
				"<color=#" + finalhex() + ">Minimap Reset:</color>",
				"<color=#" + finalhex() + ">Open Chat:</color>",
				"<color=#" + finalhex() + ">Live Spectate:</color>"
			};
			for (int i = 0; i < list.Count; i++)
			{
				int num3 = i;
				float num4 = 80f;
				if (num3 > 14)
				{
					num4 = 390f;
					num3 -= 15;
				}
				GUI.Label(new Rect(num + num4, num2 + 101f + (float)num3 * 25f, 145f, 22f), list[i], "Label");
			}
			for (int i = 0; i < 22; i++)
			{
				int num3 = i;
				float num4 = 190f;
				if (num3 > 14)
				{
					num4 = 500f;
					num3 -= 15;
				}
				if (GUI.Button(new Rect(num + num4, num2 + 101f + (float)num3 * 25f, 120f, 20f), FengCustomInputs.Inputs.getKeyRC(i)))
				{
					settings[100] = i + 1;
					FengCustomInputs.Inputs.setNameRC(i, "waiting...");
				}
			}
			if (GUI.Button(new Rect(num + 500f, num2 + 276f, 120f, 20f), (string)settings[98]))
			{
				settings[98] = "waiting...";
				settings[100] = 98;
			}
			else if (GUI.Button(new Rect(num + 500f, num2 + 301f, 120f, 20f), (string)settings[99]))
			{
				settings[99] = "waiting...";
				settings[100] = 99;
			}
			else if (GUI.Button(new Rect(num + 500f, num2 + 326f, 120f, 20f), (string)settings[232]))
			{
				settings[232] = "waiting...";
				settings[100] = 232;
			}
			else if (GUI.Button(new Rect(num + 500f, num2 + 351f, 120f, 20f), (string)settings[233]))
			{
				settings[233] = "waiting...";
				settings[100] = 233;
			}
			else if (GUI.Button(new Rect(num + 500f, num2 + 376f, 120f, 20f), (string)settings[234]))
			{
				settings[234] = "waiting...";
				settings[100] = 234;
			}
			else if (GUI.Button(new Rect(num + 500f, num2 + 401f, 120f, 20f), (string)settings[236]))
			{
				settings[236] = "waiting...";
				settings[100] = 236;
			}
			else if (GUI.Button(new Rect(num + 500f, num2 + 426f, 120f, 20f), (string)settings[262]))
			{
				settings[262] = "waiting...";
				settings[100] = 262;
			}
			if ((int)settings[100] == 0)
			{
				return;
			}
			Event @event = Event.current;
			bool flag = false;
			string text = "waiting...";
			if (@event.type == EventType.KeyDown && @event.keyCode != 0)
			{
				flag = true;
				text = @event.keyCode.ToString();
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				flag = true;
				text = KeyCode.LeftShift.ToString();
			}
			else if (Input.GetKey(KeyCode.RightShift))
			{
				flag = true;
				text = KeyCode.RightShift.ToString();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					flag = true;
					text = "Scroll Up";
				}
				else
				{
					flag = true;
					text = "Scroll Down";
				}
			}
			else
			{
				for (int i = 0; i < 7; i++)
				{
					if (Input.GetKeyDown((KeyCode)(323 + i)))
					{
						flag = true;
						text = "Mouse" + Convert.ToString(i);
					}
				}
			}
			if (!flag)
			{
				return;
			}
			if ((int)settings[100] == 98)
			{
				settings[98] = text;
				settings[100] = 0;
				inputRC.setInputHuman(InputCodeRC.reelin, text);
				return;
			}
			if ((int)settings[100] == 99)
			{
				settings[99] = text;
				settings[100] = 0;
				inputRC.setInputHuman(InputCodeRC.reelout, text);
				return;
			}
			if ((int)settings[100] == 182)
			{
				settings[182] = text;
				settings[100] = 0;
				inputRC.setInputHuman(InputCodeRC.dash, text);
				return;
			}
			if ((int)settings[100] == 232)
			{
				settings[232] = text;
				settings[100] = 0;
				inputRC.setInputHuman(InputCodeRC.mapMaximize, text);
				return;
			}
			if ((int)settings[100] == 233)
			{
				settings[233] = text;
				settings[100] = 0;
				inputRC.setInputHuman(InputCodeRC.mapToggle, text);
				return;
			}
			if ((int)settings[100] == 234)
			{
				settings[234] = text;
				settings[100] = 0;
				inputRC.setInputHuman(InputCodeRC.mapReset, text);
				return;
			}
			if ((int)settings[100] == 236)
			{
				settings[236] = text;
				settings[100] = 0;
				inputRC.setInputHuman(InputCodeRC.chat, text);
				return;
			}
			if ((int)settings[100] == 262)
			{
				settings[262] = text;
				settings[100] = 0;
				inputRC.setInputHuman(InputCodeRC.liveCam, text);
				return;
			}
			for (int i = 0; i < 22; i++)
			{
				int num5 = i + 1;
				if ((int)settings[100] == num5)
				{
					FengCustomInputs.Inputs.setKeyRC(i, text);
					settings[100] = 0;
				}
			}
		}
		else if ((int)settings[190] == 1)
		{
			List<string> list = new List<string>
			{
				"<color=#" + finalhex() + ">Forward:</color>",
				"<color=#" + finalhex() + ">Back:</color>",
				"<color=#" + finalhex() + ">Left:</color>",
				"<color=#" + finalhex() + ">Right:</color>",
				"<color=#" + finalhex() + ">Walk:</color>",
				"<color=#" + finalhex() + ">Jump:</color>",
				"<color=#" + finalhex() + ">Punch:</color>",
				"<color=#" + finalhex() + ">Slam:</color>",
				"<color=#" + finalhex() + ">Grab (front):</color>",
				"<color=#" + finalhex() + ">Grab (back):</color>",
				"<color=#" + finalhex() + ">Grab (nape):</color>",
				"<color=#" + finalhex() + ">Slap:</color>",
				"<color=#" + finalhex() + ">Bite:</color>",
				"<color=#" + finalhex() + ">Cover Nape:</color>"
			};
			for (int i = 0; i < list.Count; i++)
			{
				int num3 = i;
				float num4 = 80f;
				if (num3 > 6)
				{
					num4 = 390f;
					num3 -= 7;
				}
				GUI.Label(new Rect(num + num4, num2 + 101f + (float)num3 * 25f, 145f, 22f), list[i], "Label");
			}
			for (int i = 0; i < 14; i++)
			{
				int num5 = 101 + i;
				int num3 = i;
				float num4 = 190f;
				if (num3 > 6)
				{
					num4 = 500f;
					num3 -= 7;
				}
				if (GUI.Button(new Rect(num + num4, num2 + 101f + (float)num3 * 25f, 120f, 20f), (string)settings[num5]))
				{
					settings[num5] = "waiting...";
					settings[100] = num5;
				}
			}
			if ((int)settings[100] == 0)
			{
				return;
			}
			Event @event = Event.current;
			bool flag = false;
			string text = "waiting...";
			if (@event.type == EventType.KeyDown && @event.keyCode != 0)
			{
				flag = true;
				text = @event.keyCode.ToString();
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				flag = true;
				text = KeyCode.LeftShift.ToString();
			}
			else if (Input.GetKey(KeyCode.RightShift))
			{
				flag = true;
				text = KeyCode.RightShift.ToString();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					flag = true;
					text = "Scroll Up";
				}
				else
				{
					flag = true;
					text = "Scroll Down";
				}
			}
			else
			{
				for (int i = 0; i < 7; i++)
				{
					if (Input.GetKeyDown((KeyCode)(323 + i)))
					{
						flag = true;
						text = "Mouse" + Convert.ToString(i);
					}
				}
			}
			if (!flag)
			{
				return;
			}
			for (int i = 0; i < 14; i++)
			{
				int num5 = 101 + i;
				if ((int)settings[100] == num5)
				{
					settings[num5] = text;
					settings[100] = 0;
					inputRC.setInputTitan(i, text);
				}
			}
		}
		else if ((int)settings[190] == 2)
		{
			List<string> list = new List<string>
			{
				"<color=#" + finalhex() + ">Forward:</color>",
				"<color=#" + finalhex() + ">Back:</color>",
				"<color=#" + finalhex() + ">Left:</color>",
				"<color=#" + finalhex() + ">Right:</color>",
				"<color=#" + finalhex() + ">Walk:</color>",
				"<color=#" + finalhex() + ">Jump:</color>",
				"<color=#" + finalhex() + ">Mount:</color>"
			};
			for (int i = 0; i < list.Count; i++)
			{
				int num3 = i;
				float num4 = 80f;
				if (num3 > 3)
				{
					num4 = 390f;
					num3 -= 4;
				}
				GUI.Label(new Rect(num + num4, num2 + 101f + (float)num3 * 25f, 145f, 22f), list[i], "Label");
			}
			for (int i = 0; i < 7; i++)
			{
				int num5 = 237 + i;
				int num3 = i;
				float num4 = 190f;
				if (num3 > 3)
				{
					num4 = 500f;
					num3 -= 4;
				}
				if (GUI.Button(new Rect(num + num4, num2 + 101f + (float)num3 * 25f, 120f, 20f), (string)settings[num5]))
				{
					settings[num5] = "waiting...";
					settings[100] = num5;
				}
			}
			if ((int)settings[100] == 0)
			{
				return;
			}
			Event @event = Event.current;
			bool flag = false;
			string text = "waiting...";
			if (@event.type == EventType.KeyDown && @event.keyCode != 0)
			{
				flag = true;
				text = @event.keyCode.ToString();
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				flag = true;
				text = KeyCode.LeftShift.ToString();
			}
			else if (Input.GetKey(KeyCode.RightShift))
			{
				flag = true;
				text = KeyCode.RightShift.ToString();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					flag = true;
					text = "Scroll Up";
				}
				else
				{
					flag = true;
					text = "Scroll Down";
				}
			}
			else
			{
				for (int i = 0; i < 7; i++)
				{
					if (Input.GetKeyDown((KeyCode)(323 + i)))
					{
						flag = true;
						text = "Mouse" + Convert.ToString(i);
					}
				}
			}
			if (!flag)
			{
				return;
			}
			for (int i = 0; i < 7; i++)
			{
				int num5 = 237 + i;
				if ((int)settings[100] == num5)
				{
					settings[num5] = text;
					settings[100] = 0;
					inputRC.setInputHorse(i, text);
				}
			}
		}
		else if ((int)settings[190] == 3)
		{
			List<string> list = new List<string>
			{
				"<color=#" + finalhex() + ">Rotate Up:</color>",
				"<color=#" + finalhex() + ">Rotate Down:</color>",
				"<color=#" + finalhex() + ">Rotate Left:</color>",
				"<color=#" + finalhex() + ">Rotate Right:</color>",
				"<color=#" + finalhex() + ">Fire:</color>",
				"<color=#" + finalhex() + ">Mount:</color>",
				"<color=#" + finalhex() + ">Slow Rotate:</color>"
			};
			for (int i = 0; i < list.Count; i++)
			{
				int num3 = i;
				float num4 = 80f;
				if (num3 > 3)
				{
					num4 = 390f;
					num3 -= 4;
				}
				GUI.Label(new Rect(num + num4, num2 + 101f + (float)num3 * 25f, 145f, 22f), list[i], "Label");
			}
			for (int i = 0; i < 7; i++)
			{
				int num5 = 254 + i;
				int num3 = i;
				float num4 = 190f;
				if (num3 > 3)
				{
					num4 = 500f;
					num3 -= 4;
				}
				if (GUI.Button(new Rect(num + num4, num2 + 101f + (float)num3 * 25f, 120f, 20f), (string)settings[num5]))
				{
					settings[num5] = "waiting...";
					settings[100] = num5;
				}
			}
			if ((int)settings[100] == 0)
			{
				return;
			}
			Event @event = Event.current;
			bool flag = false;
			string text = "waiting...";
			if (@event.type == EventType.KeyDown && @event.keyCode != 0)
			{
				flag = true;
				text = @event.keyCode.ToString();
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				flag = true;
				text = KeyCode.LeftShift.ToString();
			}
			else if (Input.GetKey(KeyCode.RightShift))
			{
				flag = true;
				text = KeyCode.RightShift.ToString();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					flag = true;
					text = "Scroll Up";
				}
				else
				{
					flag = true;
					text = "Scroll Down";
				}
			}
			else
			{
				for (int i = 0; i < 6; i++)
				{
					if (Input.GetKeyDown((KeyCode)(323 + i)))
					{
						flag = true;
						text = "Mouse" + Convert.ToString(i);
					}
				}
			}
			if (!flag)
			{
				return;
			}
			for (int i = 0; i < 7; i++)
			{
				int num5 = 254 + i;
				if ((int)settings[100] == num5)
				{
					settings[num5] = text;
					settings[100] = 0;
					inputRC.setInputCannon(i, text);
				}
			}
		}
		else
		{
			if ((int)settings[190] != 4)
			{
				return;
			}
			List<string> list2 = new List<string>
			{
				"<color=#" + finalhex() + ">Quit Game:</color>",
				"<color=#" + finalhex() + ">Show console:</color>",
				"<color=#" + finalhex() + ">Traffic Panel:</color>",
				"<color=#" + finalhex() + ">TAB Playerlist:</color>",
				"<color=#" + finalhex() + ">Restart:</color>"
			};
			for (int j = 0; j < list2.Count; j++)
			{
				int num6 = j;
				float num7 = 80f;
				if (num6 > 4)
				{
					num7 = 390f;
					num6 -= 5;
				}
				GUI.Label(new Rect(num + num7, num2 + 101f + (float)num6 * 25f, 145f, 22f), list2[j], "Label");
			}
			for (int k = 0; k < 5; k++)
			{
				int num8 = 1 + k;
				int num9 = k;
				float num10 = 190f;
				if (num9 > 4)
				{
					num10 = 500f;
					num9 -= 5;
				}
				if (GUI.Button(new Rect(num + num10, num2 + 101f + (float)num9 * 25f, 120f, 20f), (string)customkey[num8]))
				{
					customkey[num8] = "waiting...";
					settings[100] = num8;
				}
			}
			if ((int)settings[100] == 0)
			{
				return;
			}
			Event event2 = Event.current;
			bool flag2 = false;
			string text2 = "waiting...";
			if (event2.type == EventType.KeyDown && event2.keyCode > KeyCode.None)
			{
				flag2 = true;
				text2 = event2.keyCode.ToString();
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				flag2 = true;
				text2 = KeyCode.LeftShift.ToString();
			}
			else if (Input.GetKey(KeyCode.RightShift))
			{
				flag2 = true;
				text2 = KeyCode.RightShift.ToString();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					flag2 = true;
					text2 = "Scroll Up";
				}
				else
				{
					flag2 = true;
					text2 = "Scroll Down";
				}
			}
			else
			{
				for (int l = 0; l < 5; l++)
				{
					if (Input.GetKeyDown((KeyCode)(323 + l)))
					{
						flag2 = true;
						text2 = "Mouse" + Convert.ToString(l);
					}
				}
			}
			if (!flag2)
			{
				return;
			}
			for (int m = 0; m < 5; m++)
			{
				int num11 = 1 + m;
				if ((int)settings[100] == num11)
				{
					customkey[num11] = text2;
					settings[100] = 0;
					inputRC.setInputExtras(m, text2);
				}
			}
		}
	}

	private void SkinsMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		string[] texts = new string[4] { "Humans", "Titans", "City", "Forest" };
		SkinsType = GUI.SelectionGrid(new Rect(num + 47f, num2 + 77f, 265f, 25f), SkinsType, texts, 4);
		if (SkinsType == 0)
		{
			HumanSkins();
		}
		else if (SkinsType == 1)
		{
			TitanSkins();
		}
		else if (SkinsType == 2)
		{
			CitySkins();
		}
		else if (SkinsType == 3)
		{
			ForestSkins();
		}
	}

	private void HumanSkins()
	{
		float num = (float)Screen.width - (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		GUI.Label(new Rect(num + 52f, num2 + 112f, 120f, 30f), "<color=#" + finalhex() + ">See:</color>", "Label");
		string[] texts = new string[3] { "None", "Yours", "All" };
		settings[0] = GUI.SelectionGrid(new Rect(num + 120f, num2 + 112f, 180f, 20f), (int)settings[0], texts, 3);
		GUI.Label(new Rect(num + 52f, num2 + 137f, 185f, 22f), "<color=#" + finalhex() + ">Send skin:</color>", "Label");
		string[] texts2 = new string[2] { "No", "Yes" };
		settings[185] = GUI.SelectionGrid(new Rect(num + 170f, num2 + 137f, 130f, 20f), (int)settings[185], texts2, 2);
		GUI.Label(new Rect(num + 52f, num2 + 163f, 185f, 22f), "<color=#" + finalhex() + ">Gas skin:</color>", "Label");
		settings[15] = GUI.SelectionGrid(new Rect(num + 170f, num2 + 163f, 130f, 20f), (int)settings[15], OnOff, 2);
		GUI.Label(new Rect(num + 52f, num2 + 189f, 185f, 22f), "<color=#" + finalhex() + ">Weapon trail:</color>", "Label");
		settings[92] = GUI.SelectionGrid(new Rect(num + 170f, num2 + 189f, 130f, 20f), (int)settings[92], OnOff, 2);
		GUI.Label(new Rect(num + 52f, num2 + 215f, 185f, 22f), "<color=#" + finalhex() + ">HD Trails:</color>", "Label");
		settings[301] = GUI.SelectionGrid(new Rect(num + 170f, num2 + 215f, 130f, 20f), (int)settings[301], OnOff, 2);
		GUI.Label(new Rect(num + 52f, num2 + 241f, 185f, 22f), "<color=#" + finalhex() + ">Fade time:</color>", "Label");
		instance.FadeTime = GUI.TextField(new Rect(num + 170f, num2 + 241f, 60f, 20f), instance.FadeTime);
		if (GUI.Button(new Rect(num + 237f, num2 + 240f, 60f, 22f), "Default"))
		{
			instance.FadeTime = "1";
		}
		GUI.Label(new Rect(num + 52f, num2 + 275f, 185f, 22f), "<color=#" + finalhex() + ">Skin name:</color>", "Label");
		save_skin_text_field = GUI.TextField(new Rect(num + 160f, num2 + 275f, 100f, 20f), save_skin_text_field);
		if (GUI.Button(new Rect(num + 52f, num2 + 300f, 100f, 20f), "Save Skin", "button"))
		{
			File.WriteAllText(Directory.GetCurrentDirectory() + "/Utils/Skins/" + save_skin_text_field + ".txt", save_skin());
			save_skin_text_field = string.Empty;
		}
		if (GUI.Button(new Rect(num + 160f, num2 + 300f, 100f, 20f), "Load Skin", "button"))
		{
			is_skin_gui = true;
		}
		float num3 = 25f;
		if ((int)settings[133] == 0)
		{
			if (GUI.Button(new Rect(num + 415f, num2 + 77f, 120f, 22f), "Human Set 1"))
			{
				settings[133] = 1;
			}
			settings[3] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 0f, 230f, 20f), (string)settings[3]);
			settings[4] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 1f, 230f, 20f), (string)settings[4]);
			settings[5] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 2f, 230f, 20f), (string)settings[5]);
			settings[6] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 3f, 230f, 20f), (string)settings[6]);
			settings[7] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 4f, 230f, 20f), (string)settings[7]);
			settings[8] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 5f, 230f, 20f), (string)settings[8]);
			settings[14] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 6f, 230f, 20f), (string)settings[14]);
			settings[9] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 7f, 230f, 20f), (string)settings[9]);
			settings[10] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 8f, 230f, 20f), (string)settings[10]);
			settings[11] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 9f, 230f, 20f), (string)settings[11]);
			settings[12] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 10f, 230f, 20f), (string)settings[12]);
			settings[13] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 11f, 230f, 20f), (string)settings[13]);
			settings[94] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 12f, 230f, 20f), (string)settings[94]);
			settings[316] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 13f, 230f, 20f), (string)settings[316]);
			settings[317] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 14f, 230f, 20f), (string)settings[317]);
		}
		else if ((int)settings[133] == 1)
		{
			if (GUI.Button(new Rect(num + 415f, num2 + 77f, 120f, 22f), "Human Set 2"))
			{
				settings[133] = 2;
			}
			settings[134] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 0f, 230f, 20f), (string)settings[134]);
			settings[135] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 1f, 230f, 20f), (string)settings[135]);
			settings[136] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 2f, 230f, 20f), (string)settings[136]);
			settings[137] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 3f, 230f, 20f), (string)settings[137]);
			settings[138] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 4f, 230f, 20f), (string)settings[138]);
			settings[139] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 5f, 230f, 20f), (string)settings[139]);
			settings[145] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 6f, 230f, 20f), (string)settings[145]);
			settings[140] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 7f, 230f, 20f), (string)settings[140]);
			settings[141] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 8f, 230f, 20f), (string)settings[141]);
			settings[142] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 9f, 230f, 20f), (string)settings[142]);
			settings[143] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 10f, 230f, 20f), (string)settings[143]);
			settings[144] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 11f, 230f, 20f), (string)settings[144]);
			settings[146] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 12f, 230f, 20f), (string)settings[146]);
			settings[318] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 13f, 230f, 20f), (string)settings[318]);
			settings[319] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 14f, 230f, 20f), (string)settings[319]);
		}
		else if ((int)settings[133] == 2)
		{
			if (GUI.Button(new Rect(num + 415f, num2 + 77f, 120f, 22f), "Human Set 3"))
			{
				settings[133] = 3;
			}
			settings[147] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 0f, 230f, 20f), (string)settings[147]);
			settings[148] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 1f, 230f, 20f), (string)settings[148]);
			settings[149] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 2f, 230f, 20f), (string)settings[149]);
			settings[150] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 3f, 230f, 20f), (string)settings[150]);
			settings[151] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 4f, 230f, 20f), (string)settings[151]);
			settings[152] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 5f, 230f, 20f), (string)settings[152]);
			settings[158] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 6f, 230f, 20f), (string)settings[158]);
			settings[153] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 7f, 230f, 20f), (string)settings[153]);
			settings[154] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 8f, 230f, 20f), (string)settings[154]);
			settings[155] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 9f, 230f, 20f), (string)settings[155]);
			settings[156] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 10f, 230f, 20f), (string)settings[156]);
			settings[157] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 11f, 230f, 20f), (string)settings[157]);
			settings[159] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 12f, 230f, 20f), (string)settings[159]);
			settings[320] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 13f, 230f, 20f), (string)settings[320]);
			settings[321] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 14f, 230f, 20f), (string)settings[321]);
		}
		else if ((int)settings[133] == 3)
		{
			if (GUI.Button(new Rect(num + 415f, num2 + 77f, 120f, 22f), "Human Set 4"))
			{
				settings[133] = 0;
			}
			settings[303] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 0f, 230f, 20f), (string)settings[303]);
			settings[304] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 1f, 230f, 20f), (string)settings[304]);
			settings[305] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 2f, 230f, 20f), (string)settings[305]);
			settings[306] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 3f, 230f, 20f), (string)settings[306]);
			settings[307] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 4f, 230f, 20f), (string)settings[307]);
			settings[308] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 5f, 230f, 20f), (string)settings[308]);
			settings[314] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 6f, 230f, 20f), (string)settings[314]);
			settings[309] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 7f, 230f, 20f), (string)settings[309]);
			settings[310] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 8f, 230f, 20f), (string)settings[310]);
			settings[311] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 9f, 230f, 20f), (string)settings[311]);
			settings[312] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 10f, 230f, 20f), (string)settings[312]);
			settings[313] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 11f, 230f, 20f), (string)settings[313]);
			settings[315] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 12f, 230f, 20f), (string)settings[315]);
			settings[322] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 13f, 230f, 20f), (string)settings[322]);
			settings[323] = GUI.TextField(new Rect(num + 415f, num2 + 112f + num3 * 14f, 230f, 20f), (string)settings[323]);
		}
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 0f, 150f, 20f), "<color=#" + finalhex() + ">Horse:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 1f, 227f, 20f), "<color=#" + finalhex() + ">Hair:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 2f, 150f, 20f), "<color=#" + finalhex() + ">Eyes:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 3f, 240f, 20f), "<color=#" + finalhex() + ">Glass:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 4f, 150f, 20f), "<color=#" + finalhex() + ">Face:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 5f, 150f, 20f), "<color=#" + finalhex() + ">Skin:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 6f, 240f, 20f), "<color=#" + finalhex() + ">Hoodie:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 7f, 240f, 20f), "<color=#" + finalhex() + ">Costume:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 8f, 150f, 20f), "<color=#" + finalhex() + ">Logo & Cape:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 9f, 240f, 20f), "<color=#" + finalhex() + ">Left 3DMG:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 10f, 227f, 20f), "<color=#" + finalhex() + ">Right 3DMG:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 11f, 150f, 20f), "<color=#" + finalhex() + ">Gas:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 12f, 150f, 20f), "<color=#" + finalhex() + ">Weapon Trail:</color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 13f, 150f, 20f), "<color=#" + finalhex() + "><size=12>ThunderspearL:</size></color>", "Label");
		GUI.Label(new Rect(num + 330f, num2 + 112f + num3 * 14f, 150f, 20f), "<color=#" + finalhex() + "><size=12>ThunderspearR:</size></color>", "Label");
	}

	private void TitanSkins()
	{
		float num = (float)Screen.width - (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		GUI.Label(new Rect(num + 115f, num2 + 127f, 120f, 30f), "<color=#" + finalhex() + ">Titan Skin Mode:</color>", "Label");
		string[] texts = new string[3] { "Off", "On", "Off.Opt" };
		settings[1] = GUI.SelectionGrid(new Rect(num + 225f, num2 + 127f, 150f, 20f), (int)settings[1], texts, 3);
		GUI.Label(new Rect(num + 425f, num2 + 127f, 120f, 30f), "<color=#" + finalhex() + ">Randomized Pairs:</color>", "Label");
		if ((int)settings[32] == 1)
		{
			ActDisact[8] = "On";
		}
		else
		{
			ActDisact[8] = "Off";
		}
		if (GUI.Button(new Rect(num + 545f, num2 + 127f, 40f, 20f), ActDisact[8]))
		{
			if ((int)settings[32] == 0)
			{
				settings[32] = 1;
			}
			else
			{
				settings[32] = 0;
			}
		}
		GUI.Label(new Rect(num + 158f, num2 + 162f, 150f, 20f), "<color=#" + finalhex() + ">Titan Hair:</color>", "Label");
		settings[21] = GUI.TextField(new Rect(num + 80f, num2 + 184f, 165f, 20f), (string)settings[21]);
		settings[22] = GUI.TextField(new Rect(num + 80f, num2 + 206f, 165f, 20f), (string)settings[22]);
		settings[23] = GUI.TextField(new Rect(num + 80f, num2 + 228f, 165f, 20f), (string)settings[23]);
		settings[24] = GUI.TextField(new Rect(num + 80f, num2 + 250f, 165f, 20f), (string)settings[24]);
		settings[25] = GUI.TextField(new Rect(num + 80f, num2 + 272f, 165f, 20f), (string)settings[25]);
		if (GUI.Button(new Rect(num + 250f, num2 + 184f, 60f, 20f), hairtype((int)settings[16])))
		{
			int num3 = 16;
			int num4 = (int)settings[num3];
			num4 = ((num4 < 9) ? (num4 + 1) : (-1));
			settings[num3] = num4;
		}
		else if (GUI.Button(new Rect(num + 250f, num2 + 206f, 60f, 20f), hairtype((int)settings[17])))
		{
			int num3 = 17;
			int num4 = (int)settings[num3];
			num4 = ((num4 < 9) ? (num4 + 1) : (-1));
			settings[num3] = num4;
		}
		else if (GUI.Button(new Rect(num + 250f, num2 + 228f, 60f, 20f), hairtype((int)settings[18])))
		{
			int num3 = 18;
			int num4 = (int)settings[num3];
			num4 = ((num4 < 9) ? (num4 + 1) : (-1));
			settings[num3] = num4;
		}
		else if (GUI.Button(new Rect(num + 250f, num2 + 250f, 60f, 20f), hairtype((int)settings[19])))
		{
			int num3 = 19;
			int num4 = (int)settings[num3];
			num4 = ((num4 < 9) ? (num4 + 1) : (-1));
			settings[num3] = num4;
		}
		else if (GUI.Button(new Rect(num + 250f, num2 + 272f, 60f, 20f), hairtype((int)settings[20])))
		{
			int num3 = 20;
			int num4 = (int)settings[num3];
			num4 = ((num4 < 9) ? (num4 + 1) : (-1));
			settings[num3] = num4;
		}
		GUI.Label(new Rect(num + 158f, num2 + 302f, 150f, 20f), "<color=#" + finalhex() + ">Titan Eye:</color>", "Label");
		settings[26] = GUI.TextField(new Rect(num + 80f, num2 + 324f, 230f, 20f), (string)settings[26]);
		settings[27] = GUI.TextField(new Rect(num + 80f, num2 + 346f, 230f, 20f), (string)settings[27]);
		settings[28] = GUI.TextField(new Rect(num + 80f, num2 + 368f, 230f, 20f), (string)settings[28]);
		settings[29] = GUI.TextField(new Rect(num + 80f, num2 + 390f, 230f, 20f), (string)settings[29]);
		settings[30] = GUI.TextField(new Rect(num + 80f, num2 + 412f, 230f, 20f), (string)settings[30]);
		GUI.Label(new Rect(num + 455f, num2 + 162f, 150f, 20f), "<color=#" + finalhex() + ">Titan Body:</color>", "Label");
		settings[86] = GUI.TextField(new Rect(num + 390f, num2 + 184f, 230f, 20f), (string)settings[86]);
		settings[87] = GUI.TextField(new Rect(num + 390f, num2 + 206f, 230f, 20f), (string)settings[87]);
		settings[88] = GUI.TextField(new Rect(num + 390f, num2 + 228f, 230f, 20f), (string)settings[88]);
		settings[89] = GUI.TextField(new Rect(num + 390f, num2 + 250f, 230f, 20f), (string)settings[89]);
		settings[90] = GUI.TextField(new Rect(num + 390f, num2 + 272f, 230f, 20f), (string)settings[90]);
		GUI.Label(new Rect(num + 472f, num2 + 302f, 150f, 20f), "<color=#" + finalhex() + ">Eren:</color>", "Label");
		settings[65] = GUI.TextField(new Rect(num + 390f, num2 + 324f, 230f, 20f), (string)settings[65]);
		GUI.Label(new Rect(num + 470f, num2 + 346f, 150f, 20f), "<color=#" + finalhex() + ">Annie:</color>", "Label");
		settings[66] = GUI.TextField(new Rect(num + 390f, num2 + 368f, 230f, 20f), (string)settings[66]);
		GUI.Label(new Rect(num + 465f, num2 + 390f, 150f, 20f), "<color=#" + finalhex() + ">Colossal:</color>", "Label");
		settings[67] = GUI.TextField(new Rect(num + 390f, num2 + 412f, 230f, 20f), (string)settings[67]);
	}

	public void ForestSkins()
	{
		float num = (float)Screen.width - (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		float num3 = 22f;
		GUI.Label(new Rect(num + 390f, num2 + 91f, 145f, 30f), "<color=#" + finalhex() + ">Level Skin Mode:</color>", "Label");
		string[] texts = new string[3] { "Off", "On", "Off.Opt" };
		settings[2] = GUI.SelectionGrid(new Rect(num + 500f, num2 + 91f, 150f, 20f), (int)settings[2], texts, 3);
		allScrolls = GUI.BeginScrollView(new Rect(num, num2 + 115f, 712f, 340f), allScrolls, new Rect(num, num2 + 115f, 700f, 475f), alwaysShowHorizontal: true, alwaysShowVertical: true);
		GUI.Label(new Rect(num + 80f, num2 + 112f + num3 * 0f, 150f, 20f), "<color=#" + finalhex() + ">Ground:</color>", "Label");
		settings[49] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 1f, 230f, 20f), (string)settings[49]);
		GUI.Label(new Rect(num + 80f, num2 + 112f + num3 * 2f, 150f, 20f), "<color=#" + finalhex() + ">Forest Trunks</color>", "Label");
		settings[33] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 3f, 230f, 20f), (string)settings[33]);
		settings[34] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 4f, 230f, 20f), (string)settings[34]);
		settings[35] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 5f, 230f, 20f), (string)settings[35]);
		settings[36] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 6f, 230f, 20f), (string)settings[36]);
		settings[37] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 7f, 230f, 20f), (string)settings[37]);
		settings[38] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 8f, 230f, 20f), (string)settings[38]);
		settings[39] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 9f, 230f, 20f), (string)settings[39]);
		settings[40] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 10f, 230f, 20f), (string)settings[40]);
		GUI.Label(new Rect(num + 80f, num2 + 112f + num3 * 11f, 150f, 20f), "<color=#" + finalhex() + ">Forest Leaves:</color>", "Label");
		settings[41] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 12f, 230f, 20f), (string)settings[41]);
		settings[42] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 13f, 230f, 20f), (string)settings[42]);
		settings[43] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 14f, 230f, 20f), (string)settings[43]);
		settings[44] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 15f, 230f, 20f), (string)settings[44]);
		settings[45] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 16f, 230f, 20f), (string)settings[45]);
		settings[46] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 17f, 230f, 20f), (string)settings[46]);
		settings[47] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 18f, 230f, 20f), (string)settings[47]);
		settings[48] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 19f, 230f, 20f), (string)settings[48]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 0f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Front:</color>", "Label");
		settings[163] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 1f, 230f, 20f), (string)settings[163]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 2f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Back:</color>", "Label");
		settings[164] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 3f, 230f, 20f), (string)settings[164]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 4f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Left:</color>", "Label");
		settings[165] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 5f, 230f, 20f), (string)settings[165]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 6f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Right:</color>", "Label");
		settings[166] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 7f, 230f, 20f), (string)settings[166]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 8f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Up:</color>", "Label");
		settings[167] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 9f, 230f, 20f), (string)settings[167]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 10f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Down:</color>", "Label");
		settings[168] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 11f, 230f, 20f), (string)settings[168]);
		GUI.Label(new Rect(num + 390f, num2 + 379f, 145f, 30f), "<color=#" + finalhex() + ">Randomized Pairs:</color>", "Label");
		if ((int)settings[50] == 1)
		{
			ActDisact[10] = "On";
		}
		else
		{
			ActDisact[10] = "Off";
		}
		if (GUI.Button(new Rect(num + 510f, num2 + 379f, 40f, 20f), ActDisact[10]))
		{
			if ((int)settings[50] == 0)
			{
				settings[50] = 1;
			}
			else
			{
				settings[50] = 0;
			}
		}
		GUI.EndScrollView();
	}

	public static void CitySkins()
	{
		float num = (float)Screen.width - (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		float num3 = 22f;
		GUI.Label(new Rect(num + 390f, num2 + 91f, 145f, 30f), "<color=#" + finalhex() + ">Level Skin Mode:</color>", "Label");
		string[] texts = new string[3] { "Off", "On", "Off.Opt" };
		settings[2] = GUI.SelectionGrid(new Rect(num + 500f, num2 + 91f, 150f, 20f), (int)settings[2], texts, 3);
		GUI.Label(new Rect(num + 80f, num2 + 112f + num3 * 0f, 150f, 20f), "<color=#" + finalhex() + ">Ground:</color>", "Label");
		settings[59] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 1f, 230f, 20f), (string)settings[59]);
		GUI.Label(new Rect(num + 80f, num2 + 112f + num3 * 2f, 150f, 20f), "<color=#" + finalhex() + ">Wall:</color>", "Label");
		settings[60] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 3f, 230f, 20f), (string)settings[60]);
		GUI.Label(new Rect(num + 80f, num2 + 112f + num3 * 4f, 150f, 20f), "<color=#" + finalhex() + ">Gate:</color>", "Label");
		settings[61] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 5f, 230f, 20f), (string)settings[61]);
		GUI.Label(new Rect(num + 80f, num2 + 112f + num3 * 6f, 150f, 20f), "<color=#" + finalhex() + ">Houses:</color>", "Label");
		settings[51] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 7f, 230f, 20f), (string)settings[51]);
		settings[52] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 8f, 230f, 20f), (string)settings[52]);
		settings[53] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 9f, 230f, 20f), (string)settings[53]);
		settings[54] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 10f, 230f, 20f), (string)settings[54]);
		settings[55] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 11f, 230f, 20f), (string)settings[55]);
		settings[56] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 12f, 230f, 20f), (string)settings[56]);
		settings[57] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 13f, 230f, 20f), (string)settings[57]);
		settings[58] = GUI.TextField(new Rect(num + 80f, num2 + 112f + num3 * 14f, 230f, 20f), (string)settings[58]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 0f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Front:</color>", "Label");
		settings[169] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 1f, 230f, 20f), (string)settings[169]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 2f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Back:</color>", "Label");
		settings[170] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 3f, 230f, 20f), (string)settings[170]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 4f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Left:</color>", "Label");
		settings[171] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 5f, 230f, 20f), (string)settings[171]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 6f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Right:</color>", "Label");
		settings[172] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 7f, 230f, 20f), (string)settings[172]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 8f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Up:</color>", "Label");
		settings[173] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 9f, 230f, 20f), (string)settings[173]);
		GUI.Label(new Rect(num + 390f, num2 + 112f + num3 * 10f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Down:</color>", "Label");
		settings[174] = GUI.TextField(new Rect(num + 390f, num2 + 112f + num3 * 11f, 230f, 20f), (string)settings[174]);
	}

	private void VideoMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		float num3 = (float)Screen.width / 2f - 350f;
		float num4 = (float)Screen.height / 2f - 250f;
		Rect screenRect = new Rect(num3 + 30f, num4 + 55f, 355f, 445f);
		Rect screenRect2 = new Rect(num3 + 30f, num2 + 250f, 295f, 495f);
		Rect screenRect3 = new Rect(num3 + 390f, num4 + 55f, 310f, 430f);
		new Rect(num3 + 390f, num2 + 340f, 310f, 425f);
		string[] texts = new string[2] { "Page 1", "Page 2" };
		PagesNumVideo = GUI.SelectionGrid(new Rect(num + 520f, num4 + 70f, 160f, 20f), PagesNumVideo, texts, 2);
		if (PagesNumVideo == 1)
		{
			GUILayout.BeginArea(screenRect);
			GUILayout.BeginVertical();
			GUILayout.Space(20f);
			Rendering.SubHeader("Quality");
			GUILayout.Space(5f);
			Rendering.Label("Smoothen camera by..(restart):", 0, 80f, 20f);
			string[] texts2 = new string[3] { "Interpolation", "Extrapolation", "Normal" };
			settings[294] = GUILayout.SelectionGrid((int)settings[294], texts2, 3, GUILayout.Width(280f), GUILayout.Height(20f));
			GUILayout.EndVertical();
			GUILayout.EndArea();
			qualitySlider = GUI.HorizontalSlider(new Rect(num + 157f, num2 + 165f, 100f, 20f), qualitySlider, 0f, 1f);
			PlayerPrefs.SetFloat("GameQuality", qualitySlider);
			if (qualitySlider < 0.167f)
			{
				QualitySettings.SetQualityLevel(0, applyExpensiveChanges: true);
				GUI.Label(new Rect(num + 30f, num2 + 160f, 150f, 22f), "<color=#" + finalhex() + ">Fastest Quality:</color>", "Label");
			}
			else if (qualitySlider < 0.33f)
			{
				QualitySettings.SetQualityLevel(1, applyExpensiveChanges: true);
				GUI.Label(new Rect(num + 30f, num2 + 160f, 150f, 22f), "<color=#" + finalhex() + ">Fast Quality:</color>", "Label");
			}
			else if (qualitySlider < 0.5f)
			{
				QualitySettings.SetQualityLevel(2, applyExpensiveChanges: true);
				GUI.Label(new Rect(num + 30f, num2 + 160f, 150f, 22f), "<color=#" + finalhex() + ">Simple Quality:</color>", "Label");
			}
			else if (qualitySlider < 0.67f)
			{
				QualitySettings.SetQualityLevel(3, applyExpensiveChanges: true);
				GUI.Label(new Rect(num + 30f, num2 + 160f, 150f, 22f), "<color=#" + finalhex() + ">Good Quality:</color>", "Label");
			}
			else if (qualitySlider < 0.83f)
			{
				QualitySettings.SetQualityLevel(4, applyExpensiveChanges: true);
				GUI.Label(new Rect(num + 30f, num2 + 160f, 150f, 22f), "<color=#" + finalhex() + ">Beautiful Quality:</color>", "Label");
			}
			else if (qualitySlider <= 1f)
			{
				QualitySettings.SetQualityLevel(5, applyExpensiveChanges: true);
				GUI.Label(new Rect(num + 30f, num2 + 160f, 150f, 22f), "<color=#" + finalhex() + ">Fantastic Quality:</color>", "Label");
			}
			if (!(qualitySlider < 0.9f) && !level.StartsWith("Custom"))
			{
				IN_GAME_MAIN_CAMERA.tiltshift.enabled = true;
			}
			else
			{
				IN_GAME_MAIN_CAMERA.tiltshift.enabled = false;
			}
			GUI.Label(new Rect(num + 30f, num2 + 185f, 150f, 22f), "<color=#" + finalhex() + ">Texture Quality:</color>", "Label");
			if (GUI.Button(new Rect(num + 177f, num2 + 185f, 65f, 18f), mastertexturetype(QualitySettings.masterTextureLimit)))
			{
				if (QualitySettings.masterTextureLimit <= 0)
				{
					QualitySettings.masterTextureLimit = 4;
				}
				else
				{
					QualitySettings.masterTextureLimit--;
				}
				linkHash[0].Clear();
				linkHash[1].Clear();
				linkHash[2].Clear();
			}
			GUI.Label(new Rect(num + 30f, num2 + 210f, 227f, 20f), "<color=#" + finalhex() + ">FPS Cap (0 = max):</color>", "Label");
			settings[184] = GUI.TextField(new Rect(num + 177f, num2 + 210f, 60f, 20f), (string)settings[184]);
			Application.targetFrameRate = -1;
			if (int.TryParse((string)settings[184], out var result) && result > 0)
			{
				Application.targetFrameRate = result;
			}
			if ((int)settings[183] == 1)
			{
				ActDisact[12] = "On";
			}
			else
			{
				ActDisact[12] = "Off";
			}
			GUI.Label(new Rect(num + 30f, num2 + 235f, 185f, 22f), "<color=#" + finalhex() + ">Vertical Sync:</color>", "Label");
			if (GUI.Button(new Rect(num + 177f, num2 + 235f, 65f, 18f), ActDisact[12]))
			{
				if ((int)settings[183] == 0)
				{
					settings[183] = 1;
					QualitySettings.vSyncCount = 1;
				}
				else
				{
					settings[183] = 0;
					QualitySettings.vSyncCount = 0;
				}
			}
			GUILayout.BeginArea(screenRect2);
			GUILayout.BeginVertical();
			GUILayout.Space(20f);
			Rendering.SubHeader("Graphics");
			allScrolls = GUILayout.BeginScrollView(allScrolls, GUILayout.Height(180f));
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			if ((int)settings[63] == 1)
			{
				ActDisact[13] = "On";
			}
			else
			{
				ActDisact[13] = "Off";
			}
			Rendering.Label("Mipmapping (skins)", 0, 80f, 20f);
			GUILayout.Space(60f);
			if (GUILayout.Button(ActDisact[13], GUILayout.Width(65f), GUILayout.Height(18f)))
			{
				if ((int)settings[63] == 0)
				{
					settings[63] = 1;
				}
				else
				{
					settings[63] = 0;
				}
				linkHash[0].Clear();
				linkHash[1].Clear();
				linkHash[2].Clear();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			if ((int)settings[93] == 1)
			{
				ActDisact[14] = "On";
			}
			else
			{
				ActDisact[14] = "Off";
			}
			Rendering.Label("Wind effect", 0, 80f, 20f);
			GUILayout.Space(60f);
			if (GUILayout.Button(ActDisact[14], GUILayout.Width(65f), GUILayout.Height(18f)))
			{
				if ((int)settings[93] == 0)
				{
					settings[93] = 1;
				}
				else
				{
					settings[93] = 0;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Anisotropic Filtering", 0, 80f, 20f);
			GUILayout.Space(60f);
			if ((int)settings[330] == 1)
			{
				ActDisact[15] = "On";
			}
			else
			{
				ActDisact[15] = "Off";
			}
			if (GUILayout.Button(ActDisact[15], GUILayout.Width(65f), GUILayout.Height(18f)))
			{
				if ((int)settings[330] == 0)
				{
					settings[330] = 1;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
				}
				else
				{
					settings[330] = 0;
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Anti-aliasing", 0, 80f, 20f);
			GUILayout.Space(55f);
			string[] texts3 = new string[4] { "0", "2", "4", "8" };
			settings[297] = GUILayout.SelectionGrid((int)settings[297], texts3, 4, GUILayout.Width(100f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Rendering Distance", 0, 80f, 20f);
			GUILayout.Space(60f);
			RenderDist = GUILayout.TextField(RenderDist, GUILayout.Width(65f), GUILayout.Height(22f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Level Light", 0, 80f, 20f);
			GUILayout.Space(60f);
			switch ((int)settings[325])
			{
			case 0:
				if (GUILayout.Button("None", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 1;
				}
				break;
			case 1:
				if (GUILayout.Button("Red", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 2;
				}
				break;
			case 2:
				if (GUILayout.Button("Yellow", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 3;
				}
				break;
			case 3:
				if (GUILayout.Button("Green", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 4;
				}
				break;
			case 4:
				if (GUILayout.Button("Blue", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 5;
				}
				break;
			case 5:
				if (GUILayout.Button("Cyan", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 6;
				}
				break;
			case 6:
				if (GUILayout.Button("Magenta", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 7;
				}
				break;
			case 7:
				if (GUILayout.Button("Gray", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 8;
				}
				break;
			case 8:
				if (GUILayout.Button("Dark", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[325] = 0;
				}
				break;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Fog Colors", 0, 80f, 20f);
			GUILayout.Space(60f);
			switch ((int)settings[327])
			{
			case 0:
				if (GUILayout.Button("None", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 1;
				}
				break;
			case 1:
				if (GUILayout.Button("Grey", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 2;
				}
				break;
			case 2:
				if (GUILayout.Button("Yellow", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 3;
				}
				break;
			case 3:
				if (GUILayout.Button("Green", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 4;
				}
				break;
			case 4:
				if (GUILayout.Button("Blue", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 5;
				}
				break;
			case 5:
				if (GUILayout.Button("Cyan", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 6;
				}
				break;
			case 6:
				if (GUILayout.Button("Magenta", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 7;
				}
				break;
			case 7:
				if (GUILayout.Button("White", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 8;
				}
				break;
			case 8:
				if (GUILayout.Button("Red", GUILayout.Width(65f), GUILayout.Height(20f)))
				{
					settings[327] = 0;
				}
				break;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			if ((int)settings[299] == 1)
			{
				ActDisact[47] = "On";
			}
			else
			{
				ActDisact[47] = "Off";
			}
			Rendering.Label("Bright Forest", 0, 80f, 20f);
			GUILayout.Space(60f);
			if (GUILayout.Button(ActDisact[47], GUILayout.Width(65f), GUILayout.Height(18f)))
			{
				if ((int)settings[299] == 0)
				{
					settings[299] = 1;
				}
				else
				{
					settings[299] = 0;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			Rendering.Slider("Fog Density (no forest)", round: true, 10000f, ref FogDensity, 0f, 0.01f, "0", 110f);
			PlayerPrefs.SetFloat("FogDensity", FogDensity);
			RenderSettings.fogDensity = 0f + FogDensity;
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
		else
		{
			GUILayout.BeginArea(screenRect);
			GUILayout.Space(20f);
			Rendering.SubHeader("Names");
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Hide Local Name", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.hideYourName = GUILayout.SelectionGrid(GameSettings.hideYourName, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Hide Guilds", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.hideGuilds = GUILayout.SelectionGrid(GameSettings.hideGuilds, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("White Names", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.whiteNames = GUILayout.SelectionGrid(GameSettings.whiteNames, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Hide Team", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.HideTeam = GUILayout.SelectionGrid(GameSettings.HideTeam, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Visible Name Length", 0, 80f, 20f);
			GUILayout.Space(55f);
			MinCharL = GUILayout.TextField(MinCharL, GUILayout.Width(40f), GUILayout.Height(18f));
			Rendering.Label("Min", 0, 30f, 20f);
			MaxCharL = GUILayout.TextField(MaxCharL, GUILayout.Width(40f), GUILayout.Height(18f));
			Rendering.Label("Max", 0, 30f, 20f);
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Visible Guild Length", 0, 80f, 20f);
			GUILayout.Space(55f);
			GLMinCharL = GUILayout.TextField(GLMinCharL, GUILayout.Width(40f), GUILayout.Height(18f));
			Rendering.Label("Min", 0, 30f, 20f);
			GLMaxCharL = GUILayout.TextField(GLMaxCharL, GUILayout.Width(40f), GUILayout.Height(18f));
			Rendering.Label("Max", 0, 30f, 20f);
			GUILayout.EndHorizontal();
			GUILayout.Space(15f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Normal Titan", 0, 80f, 20f);
			settings[331] = GUILayout.TextField((string)settings[331], GUILayout.Width(60f), GUILayout.Height(20f));
			GUILayout.Space(5f);
			Rendering.Label("Aberrant", 0, 50f, 20f);
			settings[332] = GUILayout.TextField((string)settings[332], GUILayout.Width(60f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Jumper", 0, 80f, 20f);
			settings[333] = GUILayout.TextField((string)settings[333], GUILayout.Width(60f), GUILayout.Height(20f));
			GUILayout.Space(5f);
			Rendering.Label("Crawler", 0, 50f, 20f);
			settings[334] = GUILayout.TextField((string)settings[334], GUILayout.Width(60f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Punk", 0, 80f, 20f);
			settings[335] = GUILayout.TextField((string)settings[335], GUILayout.Width(60f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(25f);
			Rendering.SubHeader("Colors");
			GUILayout.Space(5f);
			GUILayout.BeginVertical();
			string[] texts4 = new string[6] { "HUD1", "HUD2", "/name ID", "Team", "E.Team", "Hooks" };
			PagesNumColors = GUILayout.SelectionGrid(PagesNumColors, texts4, 3, GUILayout.Width(200f), GUILayout.Height(40f));
			if (PagesNumColors == 0)
			{
				GUILayout.Space(3f);
				GUILayout.BeginHorizontal();
				Rendering.Label("R:", 0, 15f, 20f);
				AHexColorFloats0 = GUILayout.HorizontalSlider(AHexColorFloats0, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("G:", 0, 15f, 20f);
				AHexColorFloats1 = GUILayout.HorizontalSlider(AHexColorFloats1, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("B:", 0, 15f, 20f);
				AHexColorFloats2 = GUILayout.HorizontalSlider(AHexColorFloats2, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
			}
			else if (PagesNumColors == 1)
			{
				GUILayout.Space(3f);
				GUILayout.BeginHorizontal();
				Rendering.Label("R:", 0, 15f, 20f);
				HexColorFloats0 = GUILayout.HorizontalSlider(HexColorFloats0, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("G:", 0, 15f, 20f);
				HexColorFloats1 = GUILayout.HorizontalSlider(HexColorFloats1, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("B:", 0, 15f, 20f);
				HexColorFloats2 = GUILayout.HorizontalSlider(HexColorFloats2, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
			}
			else if (PagesNumColors == 2)
			{
				GUILayout.Space(3f);
				GUILayout.BeginHorizontal();
				Rendering.Label("R:", 0, 15f, 20f);
				NamesColorFloats0 = GUILayout.HorizontalSlider(NamesColorFloats0, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("G:", 0, 15f, 20f);
				NamesColorFloats1 = GUILayout.HorizontalSlider(NamesColorFloats1, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("B:", 0, 15f, 20f);
				NamesColorFloats2 = GUILayout.HorizontalSlider(NamesColorFloats2, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
			}
			else if (PagesNumColors == 3)
			{
				GUILayout.Space(3f);
				GUILayout.BeginHorizontal();
				Rendering.Label("R:", 0, 15f, 20f);
				TeamColorFloats0 = GUILayout.HorizontalSlider(TeamColorFloats0, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("G:", 0, 15f, 20f);
				TeamColorFloats1 = GUILayout.HorizontalSlider(TeamColorFloats1, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("B:", 0, 15f, 20f);
				TeamColorFloats2 = GUILayout.HorizontalSlider(TeamColorFloats2, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
			}
			else if (PagesNumColors == 4)
			{
				GUILayout.Space(3f);
				GUILayout.BeginHorizontal();
				Rendering.Label("R:", 0, 15f, 20f);
				ETeamColorFloats0 = GUILayout.HorizontalSlider(ETeamColorFloats0, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("G:", 0, 15f, 20f);
				ETeamColorFloats1 = GUILayout.HorizontalSlider(ETeamColorFloats1, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("B:", 0, 15f, 20f);
				ETeamColorFloats2 = GUILayout.HorizontalSlider(ETeamColorFloats2, 0f, 1f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
			}
			else if (PagesNumColors == 5)
			{
				GUILayout.Space(3f);
				GUILayout.BeginHorizontal();
				Rendering.Label("R:", 0, 15f, 20f);
				HookColorFloats0 = GUILayout.HorizontalSlider(HookColorFloats0, 0f, 2f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("G:", 0, 15f, 20f);
				HookColorFloats1 = GUILayout.HorizontalSlider(HookColorFloats1, 0f, 2f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				Rendering.Label("B:", 0, 15f, 20f);
				HookColorFloats2 = GUILayout.HorizontalSlider(HookColorFloats2, 0f, 2f, GUILayout.Width(90f), GUILayout.Height(12f));
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
			if (PagesNumColors == 0)
			{
				AHexTexture.SetPixel(1, 1, new Color(AHexColorFloats0, AHexColorFloats1, AHexColorFloats2, 1f));
				AHexTexture.Apply();
				GUI.DrawTexture(new Rect(num + 155f, num2 + 445f, 50f, 50f), AHexTexture);
			}
			else if (PagesNumColors == 1)
			{
				HexTexture.SetPixel(1, 1, new Color(HexColorFloats0, HexColorFloats1, HexColorFloats2, 1f));
				HexTexture.Apply();
				GUI.DrawTexture(new Rect(num + 155f, num2 + 445f, 50f, 50f), HexTexture);
			}
			else if (PagesNumColors == 2)
			{
				NamesTexture.SetPixel(1, 1, new Color(NamesColorFloats0, NamesColorFloats1, NamesColorFloats2, 1f));
				NamesTexture.Apply();
				GUI.DrawTexture(new Rect(num + 155f, num2 + 445f, 50f, 50f), NamesTexture);
			}
			else if (PagesNumColors == 3)
			{
				TeamHexTexture.SetPixel(1, 1, new Color(TeamColorFloats0, TeamColorFloats1, TeamColorFloats2, 1f));
				TeamHexTexture.Apply();
				GUI.DrawTexture(new Rect(num + 155f, num2 + 445f, 50f, 50f), TeamHexTexture);
			}
			else if (PagesNumColors == 4)
			{
				ETeamHexTexture.SetPixel(1, 1, new Color(ETeamColorFloats0, ETeamColorFloats1, ETeamColorFloats2, 1f));
				ETeamHexTexture.Apply();
				GUI.DrawTexture(new Rect(num + 155f, num2 + 445f, 50f, 50f), ETeamHexTexture);
			}
			else if (PagesNumColors == 5)
			{
				bool value = false;
				if ((int)settings[292] == 1)
				{
					value = true;
				}
				if (GUI.Toggle(new Rect(num + 220f, num2 + 445f, 460f, 20f), value, "On"))
				{
					settings[292] = 1;
				}
				else
				{
					settings[292] = 0;
				}
				HookTexture.SetPixel(1, 1, new Color(HookColorFloats0, HookColorFloats1, HookColorFloats2, 1f));
				HookTexture.Apply();
				GUI.DrawTexture(new Rect(num + 155f, num2 + 445f, 50f, 50f), HookTexture);
			}
		}
		if (PagesNumVideo == 0)
		{
			GUILayout.BeginArea(screenRect3);
			GUILayout.BeginVertical();
			GUILayout.Space(20f);
			Rendering.SubHeader("Chat");
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Name Style", 0, 80f, 20f);
			string[] texts5 = new string[4] { "<size=12>Normal</size>", "Bold", "Italics", "Both" };
			GameSettings.chatNameStyle = GUILayout.SelectionGrid(GameSettings.chatNameStyle, texts5, 4, GUILayout.Width(220f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Chat Style", 0, 80f, 20f);
			string[] texts6 = new string[4] { "<size=12>Normal</size>", "Bold", "Italics", "Both" };
			GameSettings.chatStyle = GUILayout.SelectionGrid(GameSettings.chatStyle, texts6, 4, GUILayout.Width(220f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Dark Chat", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.ChatBackground == 1)
			{
				ActDisact[42] = "On";
			}
			else
			{
				ActDisact[42] = "Off";
			}
			if (GUILayout.Button(ActDisact[42], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.ChatBackground == 0)
				{
					GameSettings.ChatBackground = 1;
				}
				else
				{
					GameSettings.ChatBackground = 0;
				}
			}
			GUILayout.Space(10f);
			Rendering.Label("Dark Console", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.ConsoleBackground == 1)
			{
				ActDisact[43] = "On";
			}
			else
			{
				ActDisact[43] = "Off";
			}
			if (GUILayout.Button(ActDisact[43], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.ConsoleBackground == 0)
				{
					GameSettings.ConsoleBackground = 1;
				}
				else
				{
					GameSettings.ConsoleBackground = 0;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Chat Lines", 0, 80f, 20f);
			GUILayout.Space(10f);
			ChatMess = GUILayout.TextField(ChatMess, GUILayout.Width(40f), GUILayout.Height(18f));
			GUILayout.Space(10f);
			Rendering.Label("Console Lines", 0, 80f, 20f);
			GUILayout.Space(10f);
			ConsoleMess = GUILayout.TextField(ConsoleMess, GUILayout.Width(40f), GUILayout.Height(18f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Chat Fade", 0, 80f, 20f);
			GUILayout.Space(10f);
			ChatFade = GUILayout.TextField(ChatFade, GUILayout.Width(40f), GUILayout.Height(18f));
			GUILayout.Space(10f);
			Rendering.Label("Console Fade", 0, 80f, 20f);
			GUILayout.Space(10f);
			ConsoleFade = GUILayout.TextField(ConsoleFade, GUILayout.Width(40f), GUILayout.Height(18f));
			GUILayout.EndHorizontal();
			GUILayout.Space(20f);
			Rendering.SubHeader("Interface");
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Top HUD", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.TopRightHUD == 1)
			{
				ActDisact[16] = "On";
			}
			else
			{
				ActDisact[16] = "Off";
			}
			if (GUILayout.Button(ActDisact[16], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.TopRightHUD == 0)
				{
					GameSettings.TopRightHUD = 1;
				}
				else
				{
					GameSettings.TopRightHUD = 0;
				}
			}
			GUILayout.Space(10f);
			Rendering.Label("Bottom HUD", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.BottomHUD == 1)
			{
				ActDisact[46] = "On";
			}
			else
			{
				ActDisact[46] = "Off";
			}
			if (GUILayout.Button(ActDisact[46], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.BottomHUD == 0)
				{
					GameSettings.BottomHUD = 1;
					BottomHUD(enabled: true);
				}
				else
				{
					GameSettings.BottomHUD = 0;
					BottomHUD(enabled: false);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Old Playerlist", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.OldPlayerlist == 1)
			{
				ActDisact[18] = "On";
			}
			else
			{
				ActDisact[18] = "Off";
			}
			if (GUILayout.Button(ActDisact[18], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.OldPlayerlist == 0)
				{
					GameSettings.OldPlayerlist = 1;
				}
				else
				{
					GameSettings.OldPlayerlist = 0;
				}
			}
			GUILayout.Space(10f);
			Rendering.Label("CMD Helper", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.CMDHelper == 1)
			{
				ActDisact[53] = "On";
			}
			else
			{
				ActDisact[53] = "Off";
			}
			if (GUILayout.Button(ActDisact[53], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.CMDHelper == 0)
				{
					GameSettings.CMDHelper = 1;
				}
				else
				{
					GameSettings.CMDHelper = 0;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("<size=12>No arrows\n  cursor</size>", 0, 80f);
			GUILayout.Space(10f);
			if (GameSettings.NoArrowsCursor == 1)
			{
				ActDisact[17] = "On";
			}
			else
			{
				ActDisact[17] = "Off";
			}
			if (GUILayout.Button(ActDisact[17], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.NoArrowsCursor == 0)
				{
					GameSettings.NoArrowsCursor = 1;
				}
				else
				{
					GameSettings.NoArrowsCursor = 0;
				}
			}
			GUILayout.Space(10f);
			Rendering.Label("Cursor Size", 0, 80f, 20f);
			CursorW = GUILayout.TextField(CursorW, GUILayout.Width(25f), GUILayout.Height(18f));
			Rendering.Label("x", 0, 5f, 20f);
			CursorH = GUILayout.TextField(CursorH, GUILayout.Width(25f), GUILayout.Height(18f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Show FPS", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.ShowFPS == 1)
			{
				ActDisact[45] = "On";
			}
			else
			{
				ActDisact[45] = "Off";
			}
			if (GUILayout.Button(ActDisact[45], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.ShowFPS == 0)
				{
					GameSettings.ShowFPS = 1;
				}
				else
				{
					GameSettings.ShowFPS = 0;
				}
			}
			GUILayout.Space(10f);
			Rendering.Label("Show Distance", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.ShowDistance == 1)
			{
				ActDisact[48] = "On";
			}
			else
			{
				ActDisact[48] = "Off";
			}
			if (GUILayout.Button(ActDisact[48], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.ShowDistance == 0)
				{
					GameSettings.ShowDistance = 1;
				}
				else
				{
					GameSettings.ShowDistance = 0;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Show Rules", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.SettingsHUD == 1)
			{
				ActDisact[50] = "On";
			}
			else
			{
				ActDisact[50] = "Off";
			}
			if (GUILayout.Button(ActDisact[50], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.SettingsHUD == 0)
				{
					GameSettings.SettingsHUD = 1;
				}
				else
				{
					GameSettings.SettingsHUD = 0;
				}
			}
			GUILayout.Space(10f);
			Rendering.Label("Show CD", 0, 80f, 20f);
			GUILayout.Space(10f);
			if (GameSettings.ShowCD == 1)
			{
				ActDisact[7] = "On";
			}
			else
			{
				ActDisact[7] = "Off";
			}
			if (GUILayout.Button(ActDisact[7], GUILayout.Width(40f), GUILayout.Height(18f)))
			{
				if (GameSettings.ShowCD == 0)
				{
					GameSettings.ShowCD = 1;
				}
				else
				{
					GameSettings.ShowCD = 0;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
		else
		{
			GUILayout.BeginArea(screenRect3);
			GUILayout.BeginVertical();
			GUILayout.Space(20f);
			Rendering.SubHeader("Animations");
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Animation (bomb)", 0, 80f, 20f);
			GUILayout.Space(55f);
			string[] texts7 = new string[2] { "BLADE", "AHSS" };
			settings[289] = GUILayout.SelectionGrid((int)settings[289], texts7, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Backflip Burst", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.BackflipBurst = GUILayout.SelectionGrid(GameSettings.BackflipBurst, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Drunk", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.Drunk = GUILayout.SelectionGrid(GameSettings.Drunk, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Drunk Titans [MC]", 0, 80f, 20f);
			GUILayout.Space(55f);
			settings[302] = GUILayout.SelectionGrid((int)settings[302], OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Upside Down (LCtrl)", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.UpsideDown = GUILayout.SelectionGrid(GameSettings.UpsideDown, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(30f);
			Rendering.SubHeader("Effects");
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Nape Cut", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.napeMelons = GUILayout.SelectionGrid(GameSettings.napeMelons, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Your Gas", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.yourGas = GUILayout.SelectionGrid(GameSettings.yourGas, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Their Gas", 0, 80f, 20f);
			GUILayout.Space(55f);
			GameSettings.theirGas = GUILayout.SelectionGrid(GameSettings.theirGas, OnOff, 2, GUILayout.Width(145f), GUILayout.Height(20f));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			Rendering.Label("Damage Size", 0, 80f, 20f);
			GUILayout.Space(57f);
			YellowDamageSize = GUILayout.HorizontalSlider(YellowDamageSize, 0f, 2f, GUILayout.Width(120f), GUILayout.Height(12f));
			Rendering.Label("x" + YellowDamageSize.RoundTo(1), 0, 10f, 20f);
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}

	private void AudioMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		float num3 = (float)Screen.width / 2f - 350f;
		float num4 = (float)Screen.height / 2f - 250f;
		new Rect(num3 + 30f, num4 + 55f, 355f, 445f);
		new Rect(num3 + 30f, num2 + 340f, 345f, 445f);
		Rect screenRect = new Rect(num3 + 390f, num4 + 55f, 315f, 405f);
		Rect screenRect2 = new Rect(num3 + 30f, num4 + 55f, 355f, 445f);
		GUI.Label(new Rect(num + 390f, num2 + 225f, 160f, 20f), "<color=#" + finalhex() + ">Overall Volume:</color>", "Label");
		AudioListener.volume = GUI.HorizontalSlider(new Rect(num + 510f, num2 + 233f, 130f, 20f), AudioListener.volume, 0f, 1f);
		GUI.Label(new Rect(num + 650f, num2 + 225f, 20f, 20f), "<color=#" + finalhex() + ">" + AudioListener.volume + "</color>");
		GUI.Label(new Rect(num + 390f, num2 + 250f, 160f, 20f), "<color=#" + finalhex() + ">Interface Volume:</color>", "Label");
		InterfaceVolume = GUI.HorizontalSlider(new Rect(num + 510f, num2 + 258f, 130f, 20f), InterfaceVolume, 0f, 1f);
		GUI.Label(new Rect(num + 650f, num2 + 250f, 20f, 20f), "<color=#" + finalhex() + ">" + InterfaceVolume + "</color>");
		GUI.Label(new Rect(num + 390f, num2 + 275f, 160f, 20f), "<color=#" + finalhex() + ">Assistant Volume:</color>", "Label");
		AssistantVolume = GUI.HorizontalSlider(new Rect(num + 510f, num2 + 283f, 130f, 20f), AssistantVolume, 0f, 1f);
		GUI.Label(new Rect(num + 650f, num2 + 275f, 20f, 20f), "<color=#" + finalhex() + ">" + AssistantVolume + "</color>");
		GUILayout.BeginArea(screenRect2);
		GUILayout.BeginVertical();
		GUILayout.Space(20f);
		Rendering.SubHeader("Game Sounds");
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Chat Typing", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.Typing == 1)
		{
			ActDisact[25] = "On";
		}
		else
		{
			ActDisact[25] = "Off";
		}
		if (GUILayout.Button(ActDisact[25], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.Typing == 0)
			{
				GameSettings.Typing = 1;
			}
			else
			{
				GameSettings.Typing = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Menu Clicking", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.Clicking == 1)
		{
			ActDisact[26] = "On";
		}
		else
		{
			ActDisact[26] = "Off";
		}
		if (GUILayout.Button(ActDisact[26], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.Clicking == 0)
			{
				GameSettings.Clicking = 1;
			}
			else
			{
				GameSettings.Clicking = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Gas Burst", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.GasBurst == 1)
		{
			ActDisact[27] = "On";
		}
		else
		{
			ActDisact[27] = "Off";
		}
		if (GUILayout.Button(ActDisact[27], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.GasBurst == 0)
			{
				GameSettings.GasBurst = 1;
			}
			else
			{
				GameSettings.GasBurst = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Gas Emission", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.GasSound == 1)
		{
			ActDisact[28] = "On";
		}
		else
		{
			ActDisact[28] = "Off";
		}
		if (GUILayout.Button(ActDisact[28], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.GasSound == 0)
			{
				GameSettings.GasSound = 1;
			}
			else
			{
				GameSettings.GasSound = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Blades reload", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.BladesReload == 1)
		{
			ActDisact[29] = "On";
		}
		else
		{
			ActDisact[29] = "Off";
		}
		if (GUILayout.Button(ActDisact[29], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BladesReload == 0)
			{
				GameSettings.BladesReload = 1;
			}
			else
			{
				GameSettings.BladesReload = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Blades broken", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.BladesBroken == 1)
		{
			ActDisact[30] = "On";
		}
		else
		{
			ActDisact[30] = "Off";
		}
		if (GUILayout.Button(ActDisact[30], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BladesBroken == 0)
			{
				GameSettings.BladesBroken = 1;
			}
			else
			{
				GameSettings.BladesBroken = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Guns reload", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.GunsReload == 1)
		{
			ActDisact[31] = "On";
		}
		else
		{
			ActDisact[31] = "Off";
		}
		if (GUILayout.Button(ActDisact[31], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.GunsReload == 0)
			{
				GameSettings.GunsReload = 1;
			}
			else
			{
				GameSettings.GunsReload = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Guns shot", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.GunsShot == 1)
		{
			ActDisact[32] = "On";
		}
		else
		{
			ActDisact[32] = "Off";
		}
		if (GUILayout.Button(ActDisact[32], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.GunsShot == 0)
			{
				GameSettings.GunsShot = 1;
			}
			else
			{
				GameSettings.GunsShot = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Bomb Flying", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.BombFlying == 1)
		{
			ActDisact[33] = "On";
		}
		else
		{
			ActDisact[33] = "Off";
		}
		if (GUILayout.Button(ActDisact[33], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BombFlying == 0)
			{
				GameSettings.BombFlying = 1;
			}
			else
			{
				GameSettings.BombFlying = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Bomb Explode", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.BombExplosion == 1)
		{
			ActDisact[34] = "On";
		}
		else
		{
			ActDisact[34] = "Off";
		}
		if (GUILayout.Button(ActDisact[34], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BombExplosion == 0)
			{
				GameSettings.BombExplosion = 1;
			}
			else
			{
				GameSettings.BombExplosion = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Hook Object", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.HookObject == 1)
		{
			ActDisact[35] = "On";
		}
		else
		{
			ActDisact[35] = "Off";
		}
		if (GUILayout.Button(ActDisact[35], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.HookObject == 0)
			{
				GameSettings.HookObject = 1;
			}
			else
			{
				GameSettings.HookObject = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Hook Enemy", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.HookTitan == 1)
		{
			ActDisact[36] = "On";
		}
		else
		{
			ActDisact[36] = "Off";
		}
		if (GUILayout.Button(ActDisact[36], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.HookTitan == 0)
			{
				GameSettings.HookTitan = 1;
			}
			else
			{
				GameSettings.HookTitan = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Flare", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.Flare == 1)
		{
			ActDisact[37] = "On";
		}
		else
		{
			ActDisact[37] = "Off";
		}
		if (GUILayout.Button(ActDisact[37], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.Flare == 0)
			{
				GameSettings.Flare = 1;
			}
			else
			{
				GameSettings.Flare = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Refill", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.Refill == 1)
		{
			ActDisact[38] = "On";
		}
		else
		{
			ActDisact[38] = "Off";
		}
		if (GUILayout.Button(ActDisact[38], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.Refill == 0)
			{
				GameSettings.Refill = 1;
			}
			else
			{
				GameSettings.Refill = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Slide", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.Slide == 1)
		{
			ActDisact[39] = "On";
		}
		else
		{
			ActDisact[39] = "Off";
		}
		if (GUILayout.Button(ActDisact[39], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.Slide == 0)
			{
				GameSettings.Slide = 1;
			}
			else
			{
				GameSettings.Slide = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Walk", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.Walk == 1)
		{
			ActDisact[40] = "On";
		}
		else
		{
			ActDisact[40] = "Off";
		}
		if (GUILayout.Button(ActDisact[40], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.Walk == 0)
			{
				GameSettings.Walk = 1;
			}
			else
			{
				GameSettings.Walk = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Bomb Kill", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.BombKill == 1)
		{
			ActDisact[41] = "On";
		}
		else
		{
			ActDisact[41] = "Off";
		}
		if (GUILayout.Button(ActDisact[41], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BombKill == 0)
			{
				GameSettings.BombKill = 1;
			}
			else
			{
				GameSettings.BombKill = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginVertical();
		GUILayout.Space(20f);
		Rendering.SubHeader("Personal Assistant");
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("PMed", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.BotPersonallyContacted == 1)
		{
			ActDisact[20] = "On";
		}
		else
		{
			ActDisact[20] = "Off";
		}
		if (GUILayout.Button(ActDisact[20], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BotPersonallyContacted == 0)
			{
				GameSettings.BotPersonallyContacted = 1;
			}
			else
			{
				GameSettings.BotPersonallyContacted = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("Quoted", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.BotQuoted == 1)
		{
			ActDisact[21] = "On";
		}
		else
		{
			ActDisact[21] = "Off";
		}
		if (GUILayout.Button(ActDisact[21], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BotQuoted == 0)
			{
				GameSettings.BotQuoted = 1;
			}
			else
			{
				GameSettings.BotQuoted = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Time Finished", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.BotTimeFinish == 1)
		{
			ActDisact[22] = "On";
		}
		else
		{
			ActDisact[22] = "Off";
		}
		if (GUILayout.Button(ActDisact[22], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BotTimeFinish == 0)
			{
				GameSettings.BotTimeFinish = 1;
			}
			else
			{
				GameSettings.BotTimeFinish = 0;
			}
		}
		GUILayout.Space(10f);
		Rendering.Label("No Connection", 0, 80f, 20f);
		GUILayout.Space(10f);
		if (GameSettings.BotNoConnection == 1)
		{
			ActDisact[23] = "On";
		}
		else
		{
			ActDisact[23] = "Off";
		}
		if (GUILayout.Button(ActDisact[23], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BotNoConnection == 0)
			{
				GameSettings.BotNoConnection = 1;
			}
			else
			{
				GameSettings.BotNoConnection = 0;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		Rendering.Label("Antis triggered", 0, 80f);
		GUILayout.Space(10f);
		if (GameSettings.BotWarning == 1)
		{
			ActDisact[24] = "On";
		}
		else
		{
			ActDisact[24] = "Off";
		}
		if (GUILayout.Button(ActDisact[24], GUILayout.Width(40f), GUILayout.Height(18f)))
		{
			if (GameSettings.BotWarning == 0)
			{
				GameSettings.BotWarning = 1;
			}
			else
			{
				GameSettings.BotWarning = 0;
			}
		}
		GUILayout.Space(10f);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void ChatMenu()
	{
		float num = (float)Screen.width - (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		float left = num + 7f;
		Rect screenRect = new Rect(left, num2 + 70f, 700f, 350f);
		if (InRoomChat.messages == null)
		{
			return;
		}
		string[] array = InRoomChat.messages.ToArray();
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginHorizontal();
		autoScroll = GUILayout.Toggle(autoScroll, "", GUILayout.Width(18f), GUILayout.Height(18f));
		GUILayout.Label("<color=#" + finalhex() + ">Auto Scroll</color>", GUILayout.Width(80f));
		selectable = GUILayout.Toggle(selectable, "", GUILayout.Width(18f), GUILayout.Height(18f));
		GUILayout.Label("<color=#" + finalhex() + ">Selectable</color>", GUILayout.Width(80f));
		save = GUILayout.Toggle(save, "", GUILayout.Width(18f), GUILayout.Height(18f));
		GUILayout.Label("<color=#" + finalhex() + ">Save in Chat.txt</color>", GUILayout.Width(130f));
		GUILayout.Label(" <color=#" + finalhex() + ">Search:</color> ", GUILayout.Width(60f));
		SearchThisShit = GUILayout.TextField(SearchThisShit, GUILayout.Width(110f));
		if (GUILayout.Button("Clear chat", GUILayout.Width(100f)))
		{
			Array.Clear(array, 0, array.Length);
			InRoomChat.messages.Clear();
		}
		GUILayout.EndHorizontal();
		if (save)
		{
			File.WriteAllText(Environment.CurrentDirectory + "\\Chat.txt", "\n ");
			FleurConsole.instance.Log("<color=#" + finalhex() + ">Chat saved</color>", FleurConsole.DebugType.LOG);
			int num3 = 0;
			while (num3 < array.Length)
			{
				int num4 = num3;
				File.AppendAllText(Environment.CurrentDirectory + "\\Chat.txt", "\n " + array[num3].StripColor() + Environment.NewLine);
				num3 = num4 + 1;
			}
		}
		save = false;
		allScrolls = GUILayout.BeginScrollView(allScrolls);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Contains(SearchThisShit))
			{
				if (selectable)
				{
					GUILayout.TextArea(array[i].StripColor(), "Label");
				}
				else
				{
					GUILayout.Label(array[i]);
				}
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		if (autoScroll)
		{
			allScrolls.y = float.PositiveInfinity;
		}
	}

	private void ExtraMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		GUI.Label(new Rect(num + 530f, num2 + 70f, 185f, 25f), "<size=18>Community</size>", "Label");
		GUILayout.BeginArea(new Rect(num + 480f, num2 + 110f, 200f, 160f));
		allScrolls = GUILayout.BeginScrollView(allScrolls);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Ranking RC Server", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/TZSaH7d");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Racing Server", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/9jhUB4bPym");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("PvP Bomb Server", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/rJyazCYrqN");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("PvP Blade Server", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/vBstmP4D8k");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("RC Server", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/wQhmCT2EkT");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Anarchy Server", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/8wy5Pkm");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Expedition Server", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/muUaZQQ");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Modding Chart", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://aottgmoddingcommunity.carrd.co/");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("AoTTG Reddit", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://old.reddit.com/r/Attackontitangame/");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("AoTTG Skins", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://aotskins.com/");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("AoTTG Scoreboard", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/wkBzXWQ");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Aottg 2 Server", "Label");
		if (GUILayout.Button("Open", GUILayout.Width(50f)))
		{
			Application.OpenURL("https://discord.gg/WYnAKTq");
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void CustomMapMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		GUI.Label(new Rect(num + 150f, num2 + 71f, 120f, 22f), "<color=#" + finalhex() + ">Map Settings</color>", "Label");
		GUI.Label(new Rect(num + 50f, num2 + 101f, 140f, 20f), "<color=#" + finalhex() + ">Titan Spawn Cap:</color>", "Label");
		settings[85] = GUI.TextField(new Rect(num + 155f, num2 + 101f, 30f, 20f), (string)settings[85]);
		string[] texts = new string[5] { "1 Round", "Waves", "PVP", "Racing", "Custom" };
		GameSettings.gameType = GUI.SelectionGrid(new Rect(num + 190f, num2 + 100f, 140f, 60f), GameSettings.gameType, texts, 2, GUI.skin.toggle);
		GUI.Label(new Rect(num + 150f, num2 + 175f, 150f, 20f), "<color=#" + finalhex() + ">Level Script:</color>", "Label");
		currentScript = GUI.TextField(new Rect(num + 50f, num2 + 200f, 275f, 220f), currentScript);
		if (GUI.Button(new Rect(num + 115f, num2 + 430f, 50f, 25f), "Maps"))
		{
			if (customMapScripts)
			{
				customMapScripts = false;
			}
			else
			{
				customMapScripts = true;
			}
		}
		else if (GUI.Button(new Rect(num + 170f, num2 + 430f, 50f, 25f), "Copy"))
		{
			TextEditor textEditor = new TextEditor();
			textEditor.content = new GUIContent(currentScript);
			textEditor.SelectAll();
			textEditor.Copy();
		}
		else if (GUI.Button(new Rect(num + 225f, num2 + 430f, 50f, 25f), "Clear"))
		{
			currentScript = string.Empty;
		}
		GUI.Label(new Rect(num + 455f, num2 + 71f, 180f, 20f), "<color=#" + finalhex() + ">Custom Textures</color>", "Label");
		GUI.Label(new Rect(num + 375f, num2 + 101f, 180f, 20f), "<color=#" + finalhex() + ">Ground Skin:</color>", "Label");
		settings[162] = GUI.TextField(new Rect(num + 375f, num2 + 123f, 275f, 20f), (string)settings[162]);
		GUI.Label(new Rect(num + 375f, num2 + 145f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Front:</color>", "Label");
		settings[175] = GUI.TextField(new Rect(num + 375f, num2 + 167f, 275f, 20f), (string)settings[175]);
		GUI.Label(new Rect(num + 375f, num2 + 189f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Back:</color>", "Label");
		settings[176] = GUI.TextField(new Rect(num + 375f, num2 + 211f, 275f, 20f), (string)settings[176]);
		GUI.Label(new Rect(num + 375f, num2 + 233f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Left:</color>", "Label");
		settings[177] = GUI.TextField(new Rect(num + 375f, num2 + 255f, 275f, 20f), (string)settings[177]);
		GUI.Label(new Rect(num + 375f, num2 + 277f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Right:</color>", "Label");
		settings[178] = GUI.TextField(new Rect(num + 375f, num2 + 299f, 275f, 20f), (string)settings[178]);
		GUI.Label(new Rect(num + 375f, num2 + 321f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Up:</color>", "Label");
		settings[179] = GUI.TextField(new Rect(num + 375f, num2 + 343f, 275f, 20f), (string)settings[179]);
		GUI.Label(new Rect(num + 375f, num2 + 365f, 150f, 20f), "<color=#" + finalhex() + ">Skybox Down:</color>", "Label");
		settings[180] = GUI.TextField(new Rect(num + 375f, num2 + 387f, 275f, 20f), (string)settings[180]);
	}

	private void CustomLogicMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		currentScriptLogic = GUI.TextField(new Rect(num + 50f, num2 + 82f, 600f, 270f), currentScriptLogic);
		if (GUI.Button(new Rect(num + 250f, num2 + 365f, 50f, 20f), "Copy"))
		{
			TextEditor textEditor = new TextEditor();
			textEditor.content = new GUIContent(currentScriptLogic);
			textEditor.SelectAll();
			textEditor.Copy();
		}
		else if (GUI.Button(new Rect(num + 400f, num2 + 365f, 50f, 20f), "Clear"))
		{
			currentScriptLogic = string.Empty;
		}
	}

	private void ServerSettingsMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		GUI.Label(new Rect(num + 200f, num2 + 435f, 400f, 22f), "Master Client only. Changes will take effect upon restart.");
		if (GUI.Button(new Rect(num + 239f, num2 + 77f, 60f, 25f), "Titans"))
		{
			settings[230] = 0;
		}
		else if (GUI.Button(new Rect(num + 305f, num2 + 77f, 40f, 25f), "PVP"))
		{
			settings[230] = 1;
		}
		else if (GUI.Button(new Rect(num + 350f, num2 + 77f, 50f, 25f), "Misc"))
		{
			settings[230] = 2;
		}
		else if (GUI.Button(new Rect(num + 404f, num2 + 77f, 60f, 25f), "Presets"))
		{
			settings[230] = 3;
		}
		else if (GUI.Button(new Rect(num + 320f, num2 + 460f, 60f, 30f), "Reset"))
		{
			settings[192] = 0;
			settings[193] = 0;
			settings[194] = 0;
			settings[195] = 0;
			settings[196] = "30";
			settings[197] = 0;
			settings[198] = "100";
			settings[199] = "200";
			settings[202] = 0;
			settings[203] = 0;
			settings[204] = "1";
			settings[205] = 0;
			settings[206] = "1000";
			settings[207] = 0;
			settings[208] = "1.0";
			settings[209] = "3.0";
			settings[210] = 0;
			settings[211] = "20.0";
			settings[212] = "20.0";
			settings[213] = "20.0";
			settings[214] = "20.0";
			settings[215] = "20.0";
			settings[216] = 0;
			settings[217] = 0;
			settings[218] = "1";
			settings[219] = 0;
			settings[220] = 0;
			settings[221] = 0;
			settings[222] = "20";
			settings[223] = 0;
			settings[224] = "10";
			settings[225] = string.Empty;
			settings[226] = 0;
			settings[227] = "50";
			settings[228] = 0;
			settings[229] = 0;
			settings[235] = 0;
		}
		if ((int)settings[230] == 0)
		{
			GUI.Label(new Rect(num + 100f, num2 + 115f, 160f, 22f), "<color=#" + finalhex() + ">Custom Titan Number:</color>", "Label");
			bool flag = false;
			if ((int)settings[203] == 1)
			{
				flag = true;
			}
			bool flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 115f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[203] = 1;
				}
				else
				{
					settings[203] = 0;
				}
			}
			GUI.Label(new Rect(num + 100f, num2 + 140f, 200f, 22f), "<color=#" + finalhex() + ">Amount (Integer):</color>", "Label");
			settings[204] = GUI.TextField(new Rect(num + 250f, num2 + 140f, 50f, 22f), (string)settings[204]);
			GUI.Label(new Rect(num + 100f, num2 + 180f, 160f, 22f), "<color=#" + finalhex() + ">Custom Titan Spawns:</color>", "Label");
			flag = false;
			if ((int)settings[210] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 180f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[210] = 1;
				}
				else
				{
					settings[210] = 0;
				}
			}
			GUI.Label(new Rect(num + 100f, num2 + 205f, 150f, 22f), "<color=#" + finalhex() + ">Normal (Decimal):</color>", "Label");
			settings[211] = GUI.TextField(new Rect(num + 250f, num2 + 205f, 50f, 22f), (string)settings[211]);
			GUI.Label(new Rect(num + 100f, num2 + 230f, 150f, 22f), "<color=#" + finalhex() + ">Aberrant (Decimal):</color>", "Label");
			settings[212] = GUI.TextField(new Rect(num + 250f, num2 + 230f, 50f, 22f), (string)settings[212]);
			GUI.Label(new Rect(num + 100f, num2 + 255f, 150f, 22f), "<color=#" + finalhex() + ">Jumper (Decimal):</color>", "Label");
			settings[213] = GUI.TextField(new Rect(num + 250f, num2 + 255f, 50f, 22f), (string)settings[213]);
			GUI.Label(new Rect(num + 100f, num2 + 280f, 150f, 22f), "<color=#" + finalhex() + ">Crawler (Decimal):</color>", "Label");
			settings[214] = GUI.TextField(new Rect(num + 250f, num2 + 280f, 50f, 22f), (string)settings[214]);
			GUI.Label(new Rect(num + 100f, num2 + 305f, 150f, 22f), "<color=#" + finalhex() + ">Punk (Decimal):</color>", "Label");
			settings[215] = GUI.TextField(new Rect(num + 250f, num2 + 305f, 50f, 22f), (string)settings[215]);
			GUI.Label(new Rect(num + 100f, num2 + 345f, 160f, 22f), "<color=#" + finalhex() + ">Titan Size Mode:</color>", "Label");
			flag = false;
			if ((int)settings[207] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 345f, 40f, 20f), flag, "On");
			if (flag2 != flag)
			{
				if (flag2)
				{
					settings[207] = 1;
				}
				else
				{
					settings[207] = 0;
				}
			}
			GUI.Label(new Rect(num + 100f, num2 + 370f, 150f, 22f), "<color=#" + finalhex() + ">Minimum (Decimal):</color>", "Label");
			settings[208] = GUI.TextField(new Rect(num + 250f, num2 + 370f, 50f, 22f), (string)settings[208]);
			GUI.Label(new Rect(num + 100f, num2 + 395f, 150f, 22f), "<color=#" + finalhex() + ">Maximum (Decimal):</color>", "Label");
			settings[209] = GUI.TextField(new Rect(num + 250f, num2 + 395f, 50f, 22f), (string)settings[209]);
			GUI.Label(new Rect(num + 400f, num2 + 115f, 160f, 22f), "<color=#" + finalhex() + ">Titan Health Mode:</color>", "Label");
			string[] texts = new string[3] { "Off", "Fixed", "Scaled" };
			settings[197] = GUI.SelectionGrid(new Rect(num + 550f, num2 + 115f, 100f, 66f), (int)settings[197], texts, 1, GUI.skin.toggle);
			GUI.Label(new Rect(num + 400f, num2 + 185f, 150f, 22f), "<color=#" + finalhex() + ">Minimum (Integer):</color>", "Label");
			settings[198] = GUI.TextField(new Rect(num + 550f, num2 + 185f, 50f, 22f), (string)settings[198]);
			GUI.Label(new Rect(num + 400f, num2 + 210f, 150f, 22f), "<color=#" + finalhex() + ">Maximum (Integer):</color>", "Label");
			settings[199] = GUI.TextField(new Rect(num + 550f, num2 + 210f, 50f, 22f), (string)settings[199]);
			GUI.Label(new Rect(num + 400f, num2 + 250f, 160f, 22f), "<color=#" + finalhex() + ">Titan Damage Mode:</color>", "Label");
			flag = false;
			if ((int)settings[205] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 550f, num2 + 250f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[205] = 1;
				}
				else
				{
					settings[205] = 0;
				}
			}
			GUI.Label(new Rect(num + 400f, num2 + 275f, 150f, 22f), "<color=#" + finalhex() + ">Damage (Integer):</color>", "Label");
			settings[206] = GUI.TextField(new Rect(num + 550f, num2 + 275f, 50f, 22f), (string)settings[206]);
			GUI.Label(new Rect(num + 400f, num2 + 315f, 160f, 22f), "<color=#" + finalhex() + ">Titan Explode Mode:</color>", "Label");
			flag = false;
			if ((int)settings[195] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 550f, num2 + 315f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[195] = 1;
				}
				else
				{
					settings[195] = 0;
				}
			}
			GUI.Label(new Rect(num + 400f, num2 + 340f, 160f, 22f), "<color=#" + finalhex() + ">Radius (Integer)</color>:", "Label");
			settings[196] = GUI.TextField(new Rect(num + 550f, num2 + 340f, 50f, 22f), (string)settings[196]);
			GUI.Label(new Rect(num + 400f, num2 + 365f, 160f, 22f), "<color=#" + finalhex() + ">Disable Rock Throwing:</color>", "Label");
			flag = false;
			if ((int)settings[194] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 550f, num2 + 365f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[194] = 1;
				}
				else
				{
					settings[194] = 0;
				}
			}
		}
		else if ((int)settings[230] == 1)
		{
			GUI.Label(new Rect(num + 100f, num2 + 115f, 160f, 22f), "<color=#" + finalhex() + ">Point Mode:</color>", "Label");
			bool flag = false;
			if ((int)settings[226] == 1)
			{
				flag = true;
			}
			bool flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 115f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[226] = 1;
				}
				else
				{
					settings[226] = 0;
				}
			}
			GUI.Label(new Rect(num + 100f, num2 + 140f, 160f, 22f), "<color=#" + finalhex() + ">Max Points (Integer):</color>", "Label");
			settings[227] = GUI.TextField(new Rect(num + 250f, num2 + 140f, 50f, 22f), (string)settings[227]);
			GUI.Label(new Rect(num + 100f, num2 + 215f, 160f, 22f), "<color=#" + finalhex() + ">Friendly Mode:</color>", "Label");
			flag = false;
			if ((int)settings[219] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 215f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[219] = 1;
				}
				else
				{
					settings[219] = 0;
				}
			}
			GUI.Label(new Rect(num + 400f, num2 + 115f, 160f, 22f), "<color=#" + finalhex() + ">Sword/AHSS PVP:</color>", "Label");
			string[] texts = new string[3] { "Off", "Teams", "FFA" };
			settings[220] = GUI.SelectionGrid(new Rect(num + 550f, num2 + 115f, 100f, 66f), (int)settings[220], texts, 1, GUI.skin.toggle);
			GUI.Label(new Rect(num + 400f, num2 + 190f, 160f, 22f), "<color=#" + finalhex() + ">No AHSS Air-Reloading:</color>", "Label");
			flag = false;
			if ((int)settings[228] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 550f, num2 + 190f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[228] = 1;
				}
				else
				{
					settings[228] = 0;
				}
			}
			GUI.Label(new Rect(num + 400f, num2 + 215f, 160f, 22f), "<color=#" + finalhex() + ">Cannons kill humans:</color>", "Label");
			flag = false;
			if ((int)settings[261] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 550f, num2 + 215f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[261] = 1;
				}
				else
				{
					settings[261] = 0;
				}
			}
		}
		else if ((int)settings[230] == 2)
		{
			GUI.Label(new Rect(num + 100f, num2 + 115f, 160f, 22f), "<color=#" + finalhex() + ">Custom Titans/Wave:</color>", "Label");
			bool flag = false;
			if ((int)settings[217] == 1)
			{
				flag = true;
			}
			bool flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 115f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[217] = 1;
				}
				else
				{
					settings[217] = 0;
				}
			}
			GUI.Label(new Rect(num + 100f, num2 + 140f, 160f, 22f), "<color=#" + finalhex() + ">Amount (Integer):</color>", "Label");
			settings[218] = GUI.TextField(new Rect(num + 250f, num2 + 140f, 50f, 22f), (string)settings[218]);
			GUI.Label(new Rect(num + 100f, num2 + 180f, 160f, 22f), "<color=#" + finalhex() + ">Maximum Waves:</color>", "Label");
			flag = false;
			if ((int)settings[221] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 180f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[221] = 1;
				}
				else
				{
					settings[221] = 0;
				}
			}
			GUI.Label(new Rect(num + 100f, num2 + 205f, 160f, 22f), "<color=#" + finalhex() + ">Amount (Integer):</color>", "Label");
			settings[222] = GUI.TextField(new Rect(num + 250f, num2 + 205f, 50f, 22f), (string)settings[222]);
			GUI.Label(new Rect(num + 100f, num2 + 245f, 160f, 22f), "<color=#" + finalhex() + ">Punks every 5 waves:</color>", "Label");
			flag = false;
			if ((int)settings[229] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 245f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[229] = 1;
				}
				else
				{
					settings[229] = 0;
				}
			}
			GUI.Label(new Rect(num + 100f, num2 + 275f, 160f, 22f), "<color=#" + finalhex() + ">Global Minimap Disable:</color>", "Label");
			flag = false;
			if ((int)settings[235] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 250f, num2 + 275f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[235] = 1;
				}
				else
				{
					settings[235] = 0;
				}
			}
			GUI.Label(new Rect(num + 400f, num2 + 115f, 160f, 22f), "<color=#" + finalhex() + ">Endless Respawn:</color>", "Label");
			flag = false;
			if ((int)settings[223] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 550f, num2 + 115f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[223] = 1;
				}
				else
				{
					settings[223] = 0;
				}
			}
			GUI.Label(new Rect(num + 400f, num2 + 140f, 160f, 22f), "<color=#" + finalhex() + ">Respawn Time (Integer):</color>", "Label");
			settings[224] = GUI.TextField(new Rect(num + 550f, num2 + 140f, 50f, 22f), (string)settings[224]);
			GUI.Label(new Rect(num + 400f, num2 + 180f, 160f, 22f), "<color=#" + finalhex() + ">Kick Eren Titan:</color>", "Label");
			flag = false;
			if ((int)settings[202] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 550f, num2 + 180f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[202] = 1;
				}
				else
				{
					settings[202] = 0;
				}
			}
			GUI.Label(new Rect(num + 400f, num2 + 210f, 160f, 22f), "<color=#" + finalhex() + ">Allow Horses:</color>", "Label");
			flag = false;
			if ((int)settings[216] == 1)
			{
				flag = true;
			}
			flag2 = GUI.Toggle(new Rect(num + 550f, num2 + 210f, 40f, 20f), flag, "On");
			if (flag != flag2)
			{
				if (flag2)
				{
					settings[216] = 1;
				}
				else
				{
					settings[216] = 0;
				}
			}
			GUI.Label(new Rect(num + 400f, num2 + 240f, 180f, 22f), "<color=#" + finalhex() + ">Message of the day:</color>", "Label");
			settings[225] = GUI.TextField(new Rect(num + 400f, num2 + 262f, 200f, 22f), (string)settings[225]);
		}
		else if ((int)settings[230] == 3)
		{
			GUILayout.BeginArea(new Rect(num + 70f, num2 + 120f, 170f, 300f));
			GUILayout.BeginHorizontal();
			GUILayout.Space(45f);
			GUILayout.Label("<color=#" + finalhex() + ">Speed Killing:</color>");
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Vanilla", "Button"))
			{
				speed_killing_presets("0", 0, "0", "0", 0);
				InRoomChat.Write("Vanilla Speed Killing preset loaded after restart.");
			}
			if (GUILayout.Button("One Minute", "Button"))
			{
				speed_killing_presets("24", 1, "1", "3", 0);
				InRoomChat.Write("One Minute Speed Killing preset loaded after restart.");
			}
			if (GUILayout.Button("Stylish Bar Run", "Button"))
			{
				speed_killing_presets("20", 1, "1.0", "3.0", 1);
				InRoomChat.Write("Stylish Bar Run Speed Killing preset loaded after restart.");
			}
			if (GUILayout.Button("One Minute AHSS", "Button"))
			{
				speed_killing_presets("16", 1, "1", "3", 0);
				InRoomChat.Write("One Minute AHSS Speed Killing preset loaded after restart.");
			}
			if (GUILayout.Button("Special Settings Run", "Button"))
			{
				IN_GAME_MAIN_CAMERA.difficulty = 1;
				difficulty = 1;
				speed_damage_preset("100", "0", "0", "15", 1, "2.0", "2.0", "0", 0);
				InRoomChat.Write("Special Settings Run preset loaded after restart.");
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(num + 250f, num2 + 120f, 170f, 300f));
			GUILayout.BeginHorizontal();
			GUILayout.Space(35f);
			GUILayout.Label("<color=#" + finalhex() + ">Speed Damage:</color>");
			GUILayout.EndHorizontal();
			if (GUILayout.Button("ASO Damage Run", "Button"))
			{
				IN_GAME_MAIN_CAMERA.difficulty = 0;
				difficulty = 0;
				speed_damage_preset("100", "0", "0", "0", 0, "3.0", "2.5", "1000", 1);
				InRoomChat.Write("ASO Damage Run preset loaded after restart.");
			}
			if (GUILayout.Button("ASO One Minute", "Button"))
			{
				IN_GAME_MAIN_CAMERA.difficulty = 0;
				difficulty = 0;
				speed_damage_preset("100", "0", "0", "20", 1, "3.0", "2.5", "1000", 1);
				InRoomChat.Write("ASO One Minute Run preset loaded after restart.");
			}
			if (GUILayout.Button("AHSS One Minute", "Button"))
			{
				IN_GAME_MAIN_CAMERA.difficulty = 0;
				difficulty = 0;
				speed_damage_preset("100", "0", "0", "16", 1, "3.0", "2.0", "500", 1);
				InRoomChat.Write("ASO One Minute Run preset loaded after restart.");
			}
			if (GUILayout.Button("Aby Damage Run", "Button"))
			{
				IN_GAME_MAIN_CAMERA.difficulty = 0;
				difficulty = 0;
				speed_damage_preset("0", "50", "50", "0", 0, "3.0", "2.5", "1000", 1);
				InRoomChat.Write("Aby Damage Run preset loaded after restart.");
			}
			if (GUILayout.Button("2k+ Damage Run", "Button"))
			{
				IN_GAME_MAIN_CAMERA.difficulty = 0;
				difficulty = 0;
				speed_damage_preset("100", "0", "0", "0", 0, "3.0", "3.0", "2000", 1);
				InRoomChat.Write("2k+ Damage Run preset loaded after restart.");
			}
			if (GUILayout.Button("3k+ Damage Run", "Button"))
			{
				IN_GAME_MAIN_CAMERA.difficulty = 0;
				difficulty = 0;
				speed_damage_preset("100", "0", "0", "0", 0, "3.0", "3.0", "3000", 1);
				InRoomChat.Write("3k+ Damage Run preset loaded after restart.");
			}
			if (GUILayout.Button("AHSS Damage Run", "Button"))
			{
				IN_GAME_MAIN_CAMERA.difficulty = 0;
				difficulty = 0;
				speed_damage_preset("100", "0", "0", "0", 0, "3.0", "2.0", "500", 1);
				InRoomChat.Write("AHSS Damage Run preset loaded after restart.");
			}
			GUILayout.EndArea();
		}
	}

	private void BombMenu()
	{
		float num = (float)Screen.width / 2f - 350f;
		float num2 = (float)Screen.height / 2f - 250f;
		string text = (((int)settings[359] == 0) ? "1" : "2");
		GUI.Label(new Rect(num + 50f, num2 + 80f, 80f, 22f), "<color=#" + finalhex() + ">Color " + text + ": </color>", "Label");
		bool value = false;
		if (GameSettings.ShowBombColor > 0)
		{
			value = true;
		}
		if (GUI.Toggle(new Rect(num + 155f, num2 + 80f, 40f, 20f), value, "On"))
		{
			GameSettings.ShowBombColor = 1;
		}
		else
		{
			GameSettings.ShowBombColor = 0;
		}
		if ((int)settings[359] == 0)
		{
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			texture2D.SetPixel(0, 0, new Color((float)settings[246], (float)settings[247], (float)settings[248], (float)settings[249]));
			texture2D.Apply();
			GUI.DrawTexture(new Rect(num + 105f, num2 + 83f, 40f, 15f), texture2D, ScaleMode.StretchToFill);
			UnityEngine.Object.Destroy(texture2D);
		}
		else if ((int)settings[359] == 1)
		{
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			texture2D.SetPixel(0, 0, new Color((float)settings[267], (float)settings[268], (float)settings[269], (float)settings[270]));
			texture2D.Apply();
			GUI.DrawTexture(new Rect(num + 105f, num2 + 83f, 40f, 15f), texture2D, ScaleMode.StretchToFill);
			UnityEngine.Object.Destroy(texture2D);
		}
		GUI.Label(new Rect(num + 42f, num2 + 105f, 20f, 22f), "<color=#" + finalhex() + ">R:</color>", "Label");
		GUI.Label(new Rect(num + 42f, num2 + 130f, 20f, 22f), "<color=#" + finalhex() + ">G:</color>", "Label");
		GUI.Label(new Rect(num + 42f, num2 + 155f, 20f, 22f), "<color=#" + finalhex() + ">B:</color>", "Label");
		GUI.Label(new Rect(num + 42f, num2 + 180f, 20f, 22f), "<color=#" + finalhex() + ">A:</color>", "Label");
		if ((int)settings[359] == 0)
		{
			settings[246] = GUI.HorizontalSlider(new Rect(num + 62f, num2 + 108f, 100f, 20f), (float)settings[246], 0f, 1f);
			settings[247] = GUI.HorizontalSlider(new Rect(num + 62f, num2 + 133f, 100f, 20f), (float)settings[247], 0f, 1f);
			settings[248] = GUI.HorizontalSlider(new Rect(num + 62f, num2 + 158f, 100f, 20f), (float)settings[248], 0f, 1f);
			settings[249] = GUI.HorizontalSlider(new Rect(num + 62f, num2 + 183f, 100f, 20f), (float)settings[249], 0.5f, 1f);
		}
		else if ((int)settings[359] == 1)
		{
			settings[267] = GUI.HorizontalSlider(new Rect(num + 62f, num2 + 108f, 100f, 20f), (float)settings[267], 0f, 1f);
			settings[268] = GUI.HorizontalSlider(new Rect(num + 62f, num2 + 133f, 100f, 20f), (float)settings[268], 0f, 1f);
			settings[269] = GUI.HorizontalSlider(new Rect(num + 62f, num2 + 158f, 100f, 20f), (float)settings[269], 0f, 1f);
			settings[270] = GUI.HorizontalSlider(new Rect(num + 62f, num2 + 183f, 100f, 20f), (float)settings[270], 0.5f, 1f);
		}
		float num3 = Convert.ToSingle(settings[250]);
		float num4 = Convert.ToSingle(settings[251]);
		float num5 = Convert.ToSingle(settings[252]);
		float num6 = Convert.ToSingle(settings[253]);
		float bombRadius = BombUtil.GetBombRadius(num3, 5.4f, 7.4f, 7f);
		float bombRange = BombUtil.GetBombRange(num4, 0f, 4f, 7f);
		float bombSpeed = BombUtil.GetBombSpeed(num5, 3f, 10.5f, 10.5f);
		float bombCooldown = BombUtil.GetBombCooldown(num6, 4f, 7f, 7f);
		float oldRadiusCost = BombUtil.GetOldRadiusCost(bombRadius);
		float oldRangeCost = BombUtil.GetOldRangeCost(bombRange);
		float oldSpeedCost = BombUtil.GetOldSpeedCost(bombSpeed);
		float oldCooldownCost = BombUtil.GetOldCooldownCost(bombCooldown);
		string text2 = bombRadius.ToString("0.##");
		string text3 = bombRange.ToString("0.##");
		string text4 = (bombSpeed / 100f).ToString("0.##");
		string text5 = bombCooldown.ToString("0.##");
		text2 = string.Format("<color=#{0}>{1} </color>({2}) ({3}u)", finalhex(), num3, oldRadiusCost.ToString("0.##"), text2);
		text3 = string.Format("<color=#{0}>{1} </color>({2}) ({3}u)", finalhex(), num4, oldRangeCost.ToString("0.##"), text3);
		text4 = string.Format("<color=#{0}>{1} </color>({2}) ({3}k)", finalhex(), num5, oldSpeedCost.ToString("0.##"), text4);
		text5 = string.Format("<color=#{0}>{1} </color>({2}) ({3}s)", finalhex(), num6, oldCooldownCost.ToString("0.##"), text5);
		GUI.Label(new Rect(num + 42f, num2 + 225f, 95f, 22f), "<color=#" + finalhex() + ">Bomb Radius:</color>", "Label");
		GUI.Label(new Rect(num + 138f, num2 + 225f, 120f, 22f), text2, "Label");
		GUI.Label(new Rect(num + 42f, num2 + 250f, 95f, 22f), "<color=#" + finalhex() + ">Bomb Range:</color>", "Label");
		GUI.Label(new Rect(num + 138f, num2 + 250f, 120f, 22f), text3, "Label");
		GUI.Label(new Rect(num + 42f, num2 + 275f, 95f, 22f), "<color=#" + finalhex() + ">Bomb Speed:</color>", "Label");
		GUI.Label(new Rect(num + 138f, num2 + 275f, 120f, 22f), text4, "Label");
		GUI.Label(new Rect(num + 42f, num2 + 300f, 95f, 22f), "<color=#" + finalhex() + ">Bomb CD:</color>", "Label");
		GUI.Label(new Rect(num + 138f, num2 + 300f, 120f, 22f), text5, "Label");
		GUI.Label(new Rect(num + 42f, num2 + 325f, 200f, 40f), "<color=#" + finalhex() + ">Unused Points:</color>", "Label");
		settings[250] = MyHorizontalSliderOnePoint(new Rect(num + 255f, num2 + 230f, 115f, 20f), num3, 0f, 10f);
		settings[251] = MyHorizontalSliderOnePoint(new Rect(num + 255f, num2 + 255f, 115f, 20f), num4, 0f, 10f);
		settings[252] = MyHorizontalSliderOnePoint(new Rect(num + 255f, num2 + 280f, 115f, 20f), num5, 0f, 10f);
		settings[253] = MyHorizontalSliderOnePoint(new Rect(num + 255f, num2 + 305f, 115f, 20f), num6, 0f, 10f);
		GUI.Label(text: (20f - (float)settings[250] - (float)settings[251] - (float)settings[252] - (float)settings[253]).ToString(), position: new Rect(num + 138f, num2 + 325f, 100f, 22f), style: "Label");
		GUI.Label(new Rect(num + 42f, num2 + 375f, 160f, 20f), "<color=#" + finalhex() + ">Old Bomb:</color>", "Label");
		GameSettings.UseOldBombEffect = GUI.SelectionGrid(new Rect(num + 140f, num2 + 375f, 190f, 20f), GameSettings.UseOldBombEffect, OnOff, 2);
		GUI.Label(new Rect(num + 42f, num2 + 400f, 160f, 20f), "<color=#" + finalhex() + ">Hands carry:</color>", "Label");
		string[] texts = new string[3] { "Nothing", "Blades", "TSpears" };
		GameSettings.HandsHolding = GUI.SelectionGrid(new Rect(num + 140f, num2 + 400f, 190f, 20f), GameSettings.HandsHolding, texts, 3);
		GUI.Label(new Rect(num + 42f, num2 + 425f, 160f, 20f), "<color=#" + finalhex() + ">Forest Arena:</color>", "Label");
		GameSettings.NoCorners = GUI.SelectionGrid(new Rect(num + 140f, num2 + 425f, 190f, 20f), GameSettings.NoCorners, OnOff, 2);
		GUI.Label(new Rect(num + 400f, num2 + 80f, 160f, 20f), "<color=#" + finalhex() + ">Bomb Mode:</color>", "Label");
		bool value2 = false;
		if ((int)settings[192] == 1)
		{
			value2 = true;
		}
		if (GUI.Toggle(new Rect(num + 520f, num2 + 80f, 40f, 20f), value2, "On"))
		{
			settings[192] = 1;
			settings[226] = 0;
		}
		else
		{
			settings[192] = 0;
		}
		GUI.Label(new Rect(num + 400f, num2 + 105f, 100f, 66f), "<color=#" + finalhex() + ">Team Mode:</color>", "Label");
		string[] texts2 = new string[2] { "Off", "On" };
		settings[193] = GUI.SelectionGrid(new Rect(num + 520f, num2 + 105f, 80f, 20f), (int)settings[193], texts2, 2, GUI.skin.toggle);
		GUI.Label(new Rect(num + 400f, num2 + 235f, 185f, 22f), "<color=#" + finalhex() + ">Bomb Name " + text + ": </color>", "Label");
		if ((int)settings[359] == 0)
		{
			settings[328] = GUI.TextField(new Rect(num + 500f, num2 + 235f, 150f, 22f), (string)settings[328]);
		}
		else if ((int)settings[359] == 1)
		{
			settings[337] = GUI.TextField(new Rect(num + 500f, num2 + 235f, 150f, 22f), (string)settings[337]);
		}
		GUI.Label(new Rect(num + 400f, num2 + 265f, 160f, 20f), "<color=#" + finalhex() + ">Bomb Type:</color>", "Label");
		string[] texts3 = new string[2] { "Impact", "Bouncy" };
		settings[291] = GUI.SelectionGrid(new Rect(num + 500f, num2 + 265f, 150f, 20f), (int)settings[291], texts3, 2);
		GUI.Label(new Rect(num + 400f, num2 + 295f, 160f, 20f), "<color=#" + finalhex() + ">Switch Bomb:</color>", "Label");
		string text7 = BombSpecialKey.ToString();
		if (changingKeys == 0)
		{
			text7 = "Waiting...";
			for (int i = 1; i <= 429; i++)
			{
				KeyCode keyCode = (KeyCode)i;
				if (Input.GetKeyDown(keyCode))
				{
					BombSpecialKey = keyCode;
					changingKeys = -1;
					PlayerPrefs.SetInt("BombSpecialKey", (int)keyCode);
				}
			}
		}
		if (GUI.Button(new Rect(num + 505f, num2 + 295f, 100f, 20f), text7) && changingKeys == -1)
		{
			changingKeys = 0;
		}
	}

	private void speed_killing_presets(string titanN, int titan, string min, string max, int size)
	{
		settings[192] = 0;
		settings[193] = 0;
		settings[194] = 0;
		settings[195] = 0;
		settings[197] = 0;
		settings[202] = 1;
		settings[203] = titan;
		settings[204] = titanN;
		settings[205] = 0;
		settings[207] = size;
		settings[208] = min;
		settings[209] = max;
		settings[210] = 0;
		settings[216] = 0;
		settings[217] = 0;
		settings[219] = 0;
		settings[220] = 0;
		settings[221] = 0;
		settings[223] = 0;
		settings[226] = 1;
		settings[227] = "1000";
		settings[228] = 0;
		settings[229] = 0;
		settings[235] = 1;
		if ((int)settings[244] == 0)
		{
			settings[244] = 1;
		}
	}

	private void speed_damage_preset(string n, string a, string j, string titanN, int titan, string max, string min, string damagemodeNum, int isDmgOn)
	{
		settings[192] = 0;
		settings[193] = 0;
		settings[194] = 0;
		settings[195] = 0;
		settings[197] = 0;
		settings[202] = 1;
		settings[203] = titan;
		settings[204] = titanN;
		settings[205] = isDmgOn;
		settings[206] = damagemodeNum;
		settings[207] = 1;
		settings[208] = min;
		settings[209] = max;
		settings[210] = 1;
		settings[211] = n;
		settings[212] = a;
		settings[213] = j;
		settings[214] = "0";
		settings[215] = "0";
		settings[216] = 0;
		settings[217] = 0;
		settings[219] = 0;
		settings[220] = 0;
		settings[221] = 0;
		settings[223] = 0;
		settings[226] = 1;
		settings[227] = "1000";
		settings[228] = 0;
		settings[229] = 0;
		settings[235] = 1;
		if ((int)settings[244] == 0)
		{
			settings[244] = 1;
		}
	}

	public static float MyHorizontalSlider(Rect position, float value, float leftValue, float rightValue)
	{
		return GUI.Slider(position, Convert.ToSingle(Math.Round(value * 2f, MidpointRounding.AwayFromZero) / 2.0), 0f, leftValue, rightValue, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, horiz: true, GUIUtility.GetControlID(GUI.sliderHash, FocusType.Native, position));
	}

	public static float MyHorizontalSliderOnePoint(Rect position, float value, float leftValue, float rightValue)
	{
		return GUI.Slider(position, Convert.ToSingle(Math.Round(value * 1f, MidpointRounding.AwayFromZero) / 1.0), 0f, leftValue, rightValue, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, horiz: true, GUIUtility.GetControlID(GUI.sliderHash, FocusType.Native, position));
	}

	public string save_skin()
	{
		string text = string.Empty;
		if ((int)settings[133] == 0)
		{
			text = (string)settings[3] + "," + (string)settings[4] + "," + (string)settings[5] + "," + (string)settings[6] + "," + (string)settings[7] + "," + (string)settings[8] + "," + (string)settings[14] + "," + (string)settings[9] + "," + (string)settings[10] + "," + (string)settings[11] + "," + (string)settings[12] + "," + (string)settings[13] + "," + (string)settings[94] + "," + (string)settings[316] + "," + (string)settings[317];
		}
		else if ((int)settings[133] == 1)
		{
			text = (string)settings[134] + "," + (string)settings[135] + "," + (string)settings[136] + "," + (string)settings[137] + "," + (string)settings[138] + "," + (string)settings[139] + "," + (string)settings[145] + "," + (string)settings[140] + "," + (string)settings[141] + "," + (string)settings[142] + "," + (string)settings[143] + "," + (string)settings[144] + "," + (string)settings[146] + "," + (string)settings[318] + "," + (string)settings[319];
		}
		else if ((int)settings[133] == 2)
		{
			text = (string)settings[147] + "," + (string)settings[148] + "," + (string)settings[149] + "," + (string)settings[150] + "," + (string)settings[151] + "," + (string)settings[152] + "," + (string)settings[158] + "," + (string)settings[153] + "," + (string)settings[154] + "," + (string)settings[155] + "," + (string)settings[156] + "," + (string)settings[157] + "," + (string)settings[159] + "," + (string)settings[320] + "," + (string)settings[321];
		}
		else if ((int)settings[133] == 3)
		{
			text = (string)settings[303] + "," + (string)settings[304] + "," + (string)settings[305] + "," + (string)settings[306] + "," + (string)settings[307] + "," + (string)settings[308] + "," + (string)settings[314] + "," + (string)settings[309] + "," + (string)settings[310] + "," + (string)settings[311] + "," + (string)settings[312] + "," + (string)settings[313] + "," + (string)settings[315] + "," + (string)settings[322] + "," + (string)settings[323];
		}
		string text2 = "\r\n";
		string[] array = text.Split(',');
		string text3 = array[0];
		string text4 = array[1];
		string text5 = array[2];
		string text6 = array[3];
		string text7 = array[4];
		string text8 = array[5];
		string text9 = array[6];
		string text10 = array[7];
		string text11 = array[8];
		string text12 = array[9];
		string text13 = array[10];
		string text14 = array[11];
		string text15 = array[12];
		string text16 = array[13];
		string text17 = array[14];
		return string.Concat(new object[1] { "Horse:" + text3 + text2 + "Hair:" + text4 + text2 + "Eyes:" + text5 + text2 + "Glass:" + text6 + text2 + "Face:" + text7 + text2 + "Skin:" + text8 + text2 + "Hoodie:" + text9 + text2 + "Costume:" + text10 + text2 + "Cape:" + text11 + text2 + "Blade1:" + text12 + text2 + "Blade2:" + text13 + text2 + "Gas:" + text14 + text2 + "Trail:" + text15 + text2 + "Thunderspear L:" + text16 + text2 + "Thunderspear R:" + text17 + text2 });
	}

	public string _skin_loader(string file, int index)
	{
		return File.ReadAllLines(file)[index].Substring(File.ReadAllLines(file)[index].IndexOf(":") + 1);
	}

	private void SkinsMapsGUI(int id)
	{
		string path = Directory.GetCurrentDirectory() + "/Utils/Skins/";
		GUILayout.BeginArea(new Rect(0f, 0f, mapRect.width - 10f, mapRect.height - 10f));
		GUILayout.Label("", GUILayout.MaxHeight(10f));
		GUILayout.BeginVertical();
		allScrolls = GUILayout.BeginScrollView(allScrolls);
		if (customMapScripts)
		{
			string[] fileSystemEntries = Directory.GetFileSystemEntries(customMapPath);
			foreach (string text in fileSystemEntries)
			{
				string fileName = Path.GetFileName(text);
				if (Directory.Exists(text))
				{
					if (GUILayout.Button("<color=red><b>" + fileName + "</b></color>", GUILayout.MaxHeight(50f)))
					{
						customMapPath = text;
					}
				}
				else if (text.EndsWith(".txt") && GUILayout.Button(fileName, GUILayout.MaxHeight(50f)))
				{
					currentScript = File.ReadAllText(text);
					customMapScripts = false;
				}
			}
		}
		if (is_skin_gui)
		{
			string[] fileSystemEntries = Directory.GetFiles(path, "*.txt");
			foreach (string text2 in fileSystemEntries)
			{
				int num = text2.IndexOf("Skins/") + "Skins/".Length;
				int num2 = text2.LastIndexOf(".txt");
				if (GUILayout.Button(text2.Substring(num, num2 - num), GUILayout.MaxHeight(50f)))
				{
					if ((int)settings[133] == 0)
					{
						settings[3] = _skin_loader(text2, 0);
						settings[4] = _skin_loader(text2, 1);
						settings[5] = _skin_loader(text2, 2);
						settings[6] = _skin_loader(text2, 3);
						settings[7] = _skin_loader(text2, 4);
						settings[8] = _skin_loader(text2, 5);
						settings[14] = _skin_loader(text2, 6);
						settings[9] = _skin_loader(text2, 7);
						settings[10] = _skin_loader(text2, 8);
						settings[11] = _skin_loader(text2, 9);
						settings[12] = _skin_loader(text2, 10);
						settings[13] = _skin_loader(text2, 11);
						settings[94] = _skin_loader(text2, 12);
						settings[316] = _skin_loader(text2, 13);
						settings[317] = _skin_loader(text2, 14);
					}
					else if ((int)settings[133] == 1)
					{
						settings[134] = _skin_loader(text2, 0);
						settings[135] = _skin_loader(text2, 1);
						settings[136] = _skin_loader(text2, 2);
						settings[137] = _skin_loader(text2, 3);
						settings[138] = _skin_loader(text2, 4);
						settings[139] = _skin_loader(text2, 5);
						settings[145] = _skin_loader(text2, 6);
						settings[140] = _skin_loader(text2, 7);
						settings[141] = _skin_loader(text2, 8);
						settings[142] = _skin_loader(text2, 9);
						settings[143] = _skin_loader(text2, 10);
						settings[144] = _skin_loader(text2, 11);
						settings[146] = _skin_loader(text2, 12);
						settings[318] = _skin_loader(text2, 13);
						settings[319] = _skin_loader(text2, 14);
					}
					else if ((int)settings[133] == 2)
					{
						settings[147] = _skin_loader(text2, 0);
						settings[148] = _skin_loader(text2, 1);
						settings[149] = _skin_loader(text2, 2);
						settings[150] = _skin_loader(text2, 3);
						settings[151] = _skin_loader(text2, 4);
						settings[152] = _skin_loader(text2, 5);
						settings[158] = _skin_loader(text2, 6);
						settings[153] = _skin_loader(text2, 7);
						settings[154] = _skin_loader(text2, 8);
						settings[155] = _skin_loader(text2, 9);
						settings[156] = _skin_loader(text2, 10);
						settings[157] = _skin_loader(text2, 11);
						settings[159] = _skin_loader(text2, 12);
						settings[320] = _skin_loader(text2, 13);
						settings[321] = _skin_loader(text2, 14);
					}
					else if ((int)settings[133] == 3)
					{
						settings[303] = _skin_loader(text2, 0);
						settings[304] = _skin_loader(text2, 1);
						settings[305] = _skin_loader(text2, 2);
						settings[306] = _skin_loader(text2, 3);
						settings[307] = _skin_loader(text2, 4);
						settings[308] = _skin_loader(text2, 5);
						settings[314] = _skin_loader(text2, 6);
						settings[309] = _skin_loader(text2, 7);
						settings[310] = _skin_loader(text2, 8);
						settings[311] = _skin_loader(text2, 9);
						settings[312] = _skin_loader(text2, 10);
						settings[313] = _skin_loader(text2, 11);
						settings[315] = _skin_loader(text2, 12);
						settings[322] = _skin_loader(text2, 13);
						settings[323] = _skin_loader(text2, 14);
					}
					is_skin_gui = false;
				}
			}
		}
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		if (customMapScripts && GUILayout.Button("Save Map", GUILayout.MaxWidth(100f)))
		{
			File.WriteAllText(customMapPath + script_text_field + ".txt", mapScript);
			customMapScripts = false;
			script_text_field = GUILayout.TextField(script_text_field, GUILayout.MaxWidth(100f));
		}
		if (GUILayout.Button("Back", GUILayout.MaxWidth(100f)))
		{
			customMapPath = Directory.GetCurrentDirectory() + "/Utils/Maps/Custom Maps/";
		}
		if (GUILayout.Button("Close", GUILayout.MaxWidth(100f)))
		{
			is_skin_gui = false;
			customMapScripts = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("", GUILayout.MaxHeight(10f));
		GUILayout.EndVertical();
		GUILayout.EndArea();
		GUI.DragWindow();
	}

	public void OnGUI()
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP && Application.loadedLevelName != "characterCreation")
		{
			if (GameObject.Find("ButtonCREDITS") != null && GameObject.Find("ButtonCREDITS").transform.parent.gameObject != null && NGUITools.GetActive(GameObject.Find("ButtonCREDITS").transform.parent.gameObject))
			{
				float num = (float)Screen.width / 2f - 85f;
				float num2 = (float)Screen.height / 2f;
				GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), textureBackgroundBlack);
				if (MyButtons.instance.isVisible)
				{
					MyButtons.instance.Render();
				}
				UpdateStatus = "<i>Welcome " + (((int)settings[359] == 0) ? nameField.hexColor() : nameField2.hexColor()) + ", enjoy your game!</i>";
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.STOP)
		{
			if (FengCustomInputs.Inputs != null && FengCustomInputs.Inputs.menuOn && !is_skin_gui && !customMapScripts)
			{
				Screen.showCursor = true;
				Screen.lockCursor = false;
				if ((int)settings[64] != 16)
				{
					float num = (float)Screen.width / 2f - 350f;
					float num2 = (float)Screen.height / 2f - 250f;
					GUI.DrawTexture(new Rect(num + 1f, num2 + 1f, 698f, 528f), textureBackgroundBlack);
					string[] texts = new string[10]
					{
						"<color=#" + finalhex() + "><size=12>Game Settings</size></color>",
						"<color=#" + finalhex() + "><size=12>Server Settings</size></color>",
						"<color=#" + finalhex() + "><size=12>Video</size></color>",
						"<color=#" + finalhex() + "><size=12>Audio</size></color>",
						"<color=#" + finalhex() + "><size=12>Chat</size></color>",
						"<color=#" + finalhex() + "><size=12>Keybinds</size></color>",
						"<color=#" + finalhex() + "><size=12>Skins</size></color>",
						"<color=#" + finalhex() + "><size=12>Custom Map</size></color>",
						"<color=#" + finalhex() + "><size=12>Logic</size></color>",
						"<color=#" + finalhex() + "><size=12>Bomb</size></color>"
					};
					settings[64] = GUI.SelectionGrid(new Rect(num + 4f, num2 + 4f, 690f, 50f), (int)settings[64], texts, 5);
					switch ((int)settings[64])
					{
					case 0:
						GameSettingsMenu();
						break;
					case 1:
						ServerSettingsMenu();
						break;
					case 2:
						VideoMenu();
						break;
					case 3:
						AudioMenu();
						break;
					case 4:
						ChatMenu();
						break;
					case 5:
						RebindsMenu();
						break;
					case 6:
						SkinsMenu();
						break;
					case 7:
						CustomMapMenu();
						break;
					case 8:
						CustomLogicMenu();
						break;
					case 9:
						BombMenu();
						break;
					}
					if (GUI.Button(new Rect(num + 388f, num2 + 495f, 42f, 25f), "Save"))
					{
						PlayerPrefs.SetString("racetimer", race_timer);
						PlayerPrefs.SetInt("OneLifeRacing", GameSettings.OneLifeRacing);
						PlayerPrefs.SetInt("DeleteRacing", GameSettings.DeleteRacing);
						PlayerPrefs.SetFloat("HookColorFloats0", HookColorFloats0);
						PlayerPrefs.SetFloat("HookColorFloats1", HookColorFloats1);
						PlayerPrefs.SetFloat("HookColorFloats2", HookColorFloats2);
						PlayerPrefs.SetFloat("NamesColorFloats0", NamesColorFloats0);
						PlayerPrefs.SetFloat("NamesColorFloats1", NamesColorFloats1);
						PlayerPrefs.SetFloat("NamesColorFloats2", NamesColorFloats2);
						PlayerPrefs.SetFloat("HexColorFloats0", HexColorFloats0);
						PlayerPrefs.SetFloat("HexColorFloats1", HexColorFloats1);
						PlayerPrefs.SetFloat("HexColorFloats2", HexColorFloats2);
						PlayerPrefs.SetFloat("AHexColorFloats0", AHexColorFloats0);
						PlayerPrefs.SetFloat("AHexColorFloats1", AHexColorFloats1);
						PlayerPrefs.SetFloat("AHexColorFloats2", AHexColorFloats2);
						PlayerPrefs.SetFloat("TeamColorFloats0", TeamColorFloats0);
						PlayerPrefs.SetFloat("TeamColorFloats1", TeamColorFloats1);
						PlayerPrefs.SetFloat("TeamColorFloats2", TeamColorFloats2);
						PlayerPrefs.SetFloat("ETeamColorFloats0", ETeamColorFloats0);
						PlayerPrefs.SetFloat("ETeamColorFloats1", ETeamColorFloats1);
						PlayerPrefs.SetFloat("ETeamColorFloats2", ETeamColorFloats2);
						PlayerPrefs.SetFloat("YellowDamageSize", YellowDamageSize);
						PlayerPrefs.SetString("ConsoleFade", ConsoleFade);
						PlayerPrefs.SetString("ChatFade", ChatFade);
						PlayerPrefs.SetInt("CMDHelper", GameSettings.CMDHelper);
						PlayerPrefs.SetString("ConsoleFade", ConsoleFade);
						PlayerPrefs.SetString("RenderDist", RenderDist);
						PlayerPrefs.SetString("MaxCharL", MaxCharL);
						PlayerPrefs.SetString("MinCharL", MinCharL);
						PlayerPrefs.SetString("GLMaxCharL", GLMaxCharL);
						PlayerPrefs.SetString("GLMinCharL", GLMinCharL);
						PlayerPrefs.SetString("ChatMess", ChatMess);
						PlayerPrefs.SetString("CursorH", CursorH);
						PlayerPrefs.SetString("CursorW", CursorW);
						PlayerPrefs.SetString("ConsoleMess", ConsoleMess);
						PlayerPrefs.SetInt("humanskins", (int)settings[0]);
						PlayerPrefs.SetInt("titanskins", (int)settings[1]);
						PlayerPrefs.SetInt("levelskins", (int)settings[2]);
						PlayerPrefs.SetString("horse", (string)settings[3]);
						PlayerPrefs.SetString("hair", (string)settings[4]);
						PlayerPrefs.SetString("eye", (string)settings[5]);
						PlayerPrefs.SetString("glass", (string)settings[6]);
						PlayerPrefs.SetString("face", (string)settings[7]);
						PlayerPrefs.SetString("skin", (string)settings[8]);
						PlayerPrefs.SetString("costume", (string)settings[9]);
						PlayerPrefs.SetString("logo", (string)settings[10]);
						PlayerPrefs.SetString("bladel", (string)settings[11]);
						PlayerPrefs.SetString("blader", (string)settings[12]);
						PlayerPrefs.SetString("gas", (string)settings[13]);
						PlayerPrefs.SetString("hoodie", (string)settings[14]);
						PlayerPrefs.SetInt("EnableGasSkin", (int)settings[15]);
						PlayerPrefs.SetInt("titantype1", (int)settings[16]);
						PlayerPrefs.SetInt("titantype2", (int)settings[17]);
						PlayerPrefs.SetInt("titantype3", (int)settings[18]);
						PlayerPrefs.SetInt("titantype4", (int)settings[19]);
						PlayerPrefs.SetInt("titantype5", (int)settings[20]);
						PlayerPrefs.SetString("titanhair1", (string)settings[21]);
						PlayerPrefs.SetString("titanhair2", (string)settings[22]);
						PlayerPrefs.SetString("titanhair3", (string)settings[23]);
						PlayerPrefs.SetString("titanhair4", (string)settings[24]);
						PlayerPrefs.SetString("titanhair5", (string)settings[25]);
						PlayerPrefs.SetString("titaneye1", (string)settings[26]);
						PlayerPrefs.SetString("titaneye2", (string)settings[27]);
						PlayerPrefs.SetString("titaneye3", (string)settings[28]);
						PlayerPrefs.SetString("titaneye4", (string)settings[29]);
						PlayerPrefs.SetString("titaneye5", (string)settings[30]);
						PlayerPrefs.SetInt("titanR", (int)settings[32]);
						PlayerPrefs.SetString("tree1", (string)settings[33]);
						PlayerPrefs.SetString("tree2", (string)settings[34]);
						PlayerPrefs.SetString("tree3", (string)settings[35]);
						PlayerPrefs.SetString("tree4", (string)settings[36]);
						PlayerPrefs.SetString("tree5", (string)settings[37]);
						PlayerPrefs.SetString("tree6", (string)settings[38]);
						PlayerPrefs.SetString("tree7", (string)settings[39]);
						PlayerPrefs.SetString("tree8", (string)settings[40]);
						PlayerPrefs.SetString("leaf1", (string)settings[41]);
						PlayerPrefs.SetString("leaf2", (string)settings[42]);
						PlayerPrefs.SetString("leaf3", (string)settings[43]);
						PlayerPrefs.SetString("leaf4", (string)settings[44]);
						PlayerPrefs.SetString("leaf5", (string)settings[45]);
						PlayerPrefs.SetString("leaf6", (string)settings[46]);
						PlayerPrefs.SetString("leaf7", (string)settings[47]);
						PlayerPrefs.SetString("leaf8", (string)settings[48]);
						PlayerPrefs.SetString("forestG", (string)settings[49]);
						PlayerPrefs.SetInt("forestR", (int)settings[50]);
						PlayerPrefs.SetString("house1", (string)settings[51]);
						PlayerPrefs.SetString("house2", (string)settings[52]);
						PlayerPrefs.SetString("house3", (string)settings[53]);
						PlayerPrefs.SetString("house4", (string)settings[54]);
						PlayerPrefs.SetString("house5", (string)settings[55]);
						PlayerPrefs.SetString("house6", (string)settings[56]);
						PlayerPrefs.SetString("house7", (string)settings[57]);
						PlayerPrefs.SetString("house8", (string)settings[58]);
						PlayerPrefs.SetString("cityG", (string)settings[59]);
						PlayerPrefs.SetString("cityW", (string)settings[60]);
						PlayerPrefs.SetString("cityH", (string)settings[61]);
						PlayerPrefs.SetInt("skinQ", QualitySettings.masterTextureLimit);
						PlayerPrefs.SetInt("Mipmapping", (int)settings[63]);
						PlayerPrefs.SetString("eren", (string)settings[65]);
						PlayerPrefs.SetString("annie", (string)settings[66]);
						PlayerPrefs.SetString("colossal", (string)settings[67]);
						PlayerPrefs.SetString("cmax", (string)settings[85]);
						PlayerPrefs.SetString("titanbody1", (string)settings[86]);
						PlayerPrefs.SetString("titanbody2", (string)settings[87]);
						PlayerPrefs.SetString("titanbody3", (string)settings[88]);
						PlayerPrefs.SetString("titanbody4", (string)settings[89]);
						PlayerPrefs.SetString("titanbody5", (string)settings[90]);
						PlayerPrefs.SetInt("customlevel", (int)settings[91]);
						PlayerPrefs.SetInt("EnableWeaponTrail", (int)settings[92]);
						PlayerPrefs.SetInt("windeffect", (int)settings[93]);
						PlayerPrefs.SetString("trailskin", (string)settings[94]);
						PlayerPrefs.SetString("snapshot", (string)settings[95]);
						PlayerPrefs.SetString("trailskin2", (string)settings[96]);
						PlayerPrefs.SetString("reelin", (string)settings[98]);
						PlayerPrefs.SetString("reelout", (string)settings[99]);
						PlayerPrefs.SetFloat("vol", AudioListener.volume);
						PlayerPrefs.SetFloat("interfacevol", InterfaceVolume);
						PlayerPrefs.SetFloat("assistantvol", AssistantVolume);
						PlayerPrefs.SetString("tforward", (string)settings[101]);
						PlayerPrefs.SetString("tback", (string)settings[102]);
						PlayerPrefs.SetString("tleft", (string)settings[103]);
						PlayerPrefs.SetString("tright", (string)settings[104]);
						PlayerPrefs.SetString("twalk", (string)settings[105]);
						PlayerPrefs.SetString("tjump", (string)settings[106]);
						PlayerPrefs.SetString("tpunch", (string)settings[107]);
						PlayerPrefs.SetString("tslam", (string)settings[108]);
						PlayerPrefs.SetString("tgrabfront", (string)settings[109]);
						PlayerPrefs.SetString("tgrabback", (string)settings[110]);
						PlayerPrefs.SetString("tgrabnape", (string)settings[111]);
						PlayerPrefs.SetString("tantiae", (string)settings[112]);
						PlayerPrefs.SetString("tbite", (string)settings[113]);
						PlayerPrefs.SetString("tcover", (string)settings[114]);
						PlayerPrefs.SetString("tsit", (string)settings[115]);
						PlayerPrefs.SetInt("humangui", (int)settings[133]);
						PlayerPrefs.SetString("horse2", (string)settings[134]);
						PlayerPrefs.SetString("hair2", (string)settings[135]);
						PlayerPrefs.SetString("eye2", (string)settings[136]);
						PlayerPrefs.SetString("glass2", (string)settings[137]);
						PlayerPrefs.SetString("face2", (string)settings[138]);
						PlayerPrefs.SetString("skin2", (string)settings[139]);
						PlayerPrefs.SetString("costume2", (string)settings[140]);
						PlayerPrefs.SetString("logo2", (string)settings[141]);
						PlayerPrefs.SetString("bladel2", (string)settings[142]);
						PlayerPrefs.SetString("blader2", (string)settings[143]);
						PlayerPrefs.SetString("gas2", (string)settings[144]);
						PlayerPrefs.SetString("hoodie2", (string)settings[145]);
						PlayerPrefs.SetString("trail2", (string)settings[146]);
						PlayerPrefs.SetString("horse3", (string)settings[147]);
						PlayerPrefs.SetString("hair3", (string)settings[148]);
						PlayerPrefs.SetString("eye3", (string)settings[149]);
						PlayerPrefs.SetString("glass3", (string)settings[150]);
						PlayerPrefs.SetString("face3", (string)settings[151]);
						PlayerPrefs.SetString("skin3", (string)settings[152]);
						PlayerPrefs.SetString("costume3", (string)settings[153]);
						PlayerPrefs.SetString("logo3", (string)settings[154]);
						PlayerPrefs.SetString("bladel3", (string)settings[155]);
						PlayerPrefs.SetString("blader3", (string)settings[156]);
						PlayerPrefs.SetString("gas3", (string)settings[157]);
						PlayerPrefs.SetString("hoodie3", (string)settings[158]);
						PlayerPrefs.SetString("trail3", (string)settings[159]);
						PlayerPrefs.SetString("customGround", (string)settings[162]);
						PlayerPrefs.SetString("forestskyfront", (string)settings[163]);
						PlayerPrefs.SetString("forestskyback", (string)settings[164]);
						PlayerPrefs.SetString("forestskyleft", (string)settings[165]);
						PlayerPrefs.SetString("forestskyright", (string)settings[166]);
						PlayerPrefs.SetString("forestskyup", (string)settings[167]);
						PlayerPrefs.SetString("forestskydown", (string)settings[168]);
						PlayerPrefs.SetString("cityskyfront", (string)settings[169]);
						PlayerPrefs.SetString("cityskyback", (string)settings[170]);
						PlayerPrefs.SetString("cityskyleft", (string)settings[171]);
						PlayerPrefs.SetString("cityskyright", (string)settings[172]);
						PlayerPrefs.SetString("cityskyup", (string)settings[173]);
						PlayerPrefs.SetString("cityskydown", (string)settings[174]);
						PlayerPrefs.SetString("customskyfront", (string)settings[175]);
						PlayerPrefs.SetString("customskyback", (string)settings[176]);
						PlayerPrefs.SetString("customskyleft", (string)settings[177]);
						PlayerPrefs.SetString("customskyright", (string)settings[178]);
						PlayerPrefs.SetString("customskyup", (string)settings[179]);
						PlayerPrefs.SetString("customskydown", (string)settings[180]);
						PlayerPrefs.SetInt("dashenable", (int)settings[181]);
						PlayerPrefs.SetString("dashkey", (string)settings[182]);
						PlayerPrefs.SetInt("vsync", (int)settings[183]);
						PlayerPrefs.SetString("fpscap", (string)settings[184]);
						PlayerPrefs.SetInt("sendskins", (int)settings[185]);
						PlayerPrefs.SetInt("speedometer", (int)settings[189]);
						PlayerPrefs.SetInt("bombMode", (int)settings[192]);
						PlayerPrefs.SetInt("teamMode", (int)settings[193]);
						PlayerPrefs.SetInt("rockThrow", (int)settings[194]);
						PlayerPrefs.SetInt("explodeModeOn", (int)settings[195]);
						PlayerPrefs.SetString("explodeModeNum", (string)settings[196]);
						PlayerPrefs.SetInt("healthMode", (int)settings[197]);
						PlayerPrefs.SetString("healthLower", (string)settings[198]);
						PlayerPrefs.SetString("healthUpper", (string)settings[199]);
						PlayerPrefs.SetInt("banEren", (int)settings[202]);
						PlayerPrefs.SetInt("moreTitanOn", (int)settings[203]);
						PlayerPrefs.SetString("moreTitanNum", (string)settings[204]);
						PlayerPrefs.SetInt("damageModeOn", (int)settings[205]);
						PlayerPrefs.SetString("damageModeNum", (string)settings[206]);
						PlayerPrefs.SetInt("sizeMode", (int)settings[207]);
						PlayerPrefs.SetString("sizeLower", (string)settings[208]);
						PlayerPrefs.SetString("sizeUpper", (string)settings[209]);
						PlayerPrefs.SetInt("spawnModeOn", (int)settings[210]);
						PlayerPrefs.SetString("nRate", (string)settings[211]);
						PlayerPrefs.SetString("aRate", (string)settings[212]);
						PlayerPrefs.SetString("jRate", (string)settings[213]);
						PlayerPrefs.SetString("cRate", (string)settings[214]);
						PlayerPrefs.SetString("pRate", (string)settings[215]);
						PlayerPrefs.SetInt("horseMode", (int)settings[216]);
						PlayerPrefs.SetInt("waveModeOn", (int)settings[217]);
						PlayerPrefs.SetString("waveModeNum", (string)settings[218]);
						PlayerPrefs.SetInt("friendlyMode", (int)settings[219]);
						PlayerPrefs.SetInt("pvpMode", (int)settings[220]);
						PlayerPrefs.SetInt("maxWaveOn", (int)settings[221]);
						PlayerPrefs.SetString("maxWaveNum", (string)settings[222]);
						PlayerPrefs.SetInt("endlessModeOn", (int)settings[223]);
						PlayerPrefs.SetString("endlessModeNum", (string)settings[224]);
						PlayerPrefs.SetString("motd", (string)settings[225]);
						PlayerPrefs.SetInt("pointModeOn", (int)settings[226]);
						PlayerPrefs.SetString("pointModeNum", (string)settings[227]);
						PlayerPrefs.SetInt("ahssReload", (int)settings[228]);
						PlayerPrefs.SetInt("punkWaves", (int)settings[229]);
						PlayerPrefs.SetString("mapMaximize", (string)settings[232]);
						PlayerPrefs.SetString("mapToggle", (string)settings[233]);
						PlayerPrefs.SetString("mapReset", (string)settings[234]);
						PlayerPrefs.SetInt("globalDisableMinimap", (int)settings[235]);
						PlayerPrefs.SetString("chatRebind", (string)settings[236]);
						PlayerPrefs.SetString("hforward", (string)settings[237]);
						PlayerPrefs.SetString("hback", (string)settings[238]);
						PlayerPrefs.SetString("hleft", (string)settings[239]);
						PlayerPrefs.SetString("hright", (string)settings[240]);
						PlayerPrefs.SetString("hwalk", (string)settings[241]);
						PlayerPrefs.SetString("hjump", (string)settings[242]);
						PlayerPrefs.SetString("hmount", (string)settings[243]);
						PlayerPrefs.SetInt("chatfeed", (int)settings[244]);
						PlayerPrefs.SetFloat("bombR", (float)settings[246]);
						PlayerPrefs.SetFloat("bombG", (float)settings[247]);
						PlayerPrefs.SetFloat("bombB", (float)settings[248]);
						PlayerPrefs.SetFloat("bombA", (float)settings[249]);
						PlayerPrefs.SetFloat("bombRadius", (float)settings[250]);
						PlayerPrefs.SetFloat("bombRange", (float)settings[251]);
						PlayerPrefs.SetFloat("bombSpeed", (float)settings[252]);
						PlayerPrefs.SetFloat("bombCD", (float)settings[253]);
						PlayerPrefs.SetString("cannonUp", (string)settings[254]);
						PlayerPrefs.SetString("cannonDown", (string)settings[255]);
						PlayerPrefs.SetString("cannonLeft", (string)settings[256]);
						PlayerPrefs.SetString("cannonRight", (string)settings[257]);
						PlayerPrefs.SetString("cannonFire", (string)settings[258]);
						PlayerPrefs.SetString("cannonMount", (string)settings[259]);
						PlayerPrefs.SetString("cannonSlow", (string)settings[260]);
						PlayerPrefs.SetInt("deadlyCannon", (int)settings[261]);
						PlayerPrefs.SetString("liveCam", (string)settings[262]);
						PlayerPrefs.SetInt("hideGuilds", GameSettings.hideGuilds);
						PlayerPrefs.SetInt("hideYourName", GameSettings.hideYourName);
						PlayerPrefs.SetInt("napeMelons", GameSettings.napeMelons);
						PlayerPrefs.SetInt("yourGas", GameSettings.yourGas);
						PlayerPrefs.SetInt("theirGas", GameSettings.theirGas);
						PlayerPrefs.SetInt("Pace", GameSettings.Pace);
						PlayerPrefs.SetInt("chatStyle", GameSettings.chatStyle);
						PlayerPrefs.SetInt("chatNameStyle", GameSettings.chatNameStyle);
						PlayerPrefs.SetInt("BackflipBurst", GameSettings.BackflipBurst);
						PlayerPrefs.SetInt("Drunk", GameSettings.Drunk);
						PlayerPrefs.SetInt("UpsideDown", GameSettings.UpsideDown);
						PlayerPrefs.SetInt("ChatBackground", GameSettings.ChatBackground);
						PlayerPrefs.SetInt("ConsoleBackground", GameSettings.ConsoleBackground);
						PlayerPrefs.SetInt("ShowFPS", GameSettings.ShowFPS);
						PlayerPrefs.SetInt("ShowDistance", GameSettings.ShowDistance);
						PlayerPrefs.SetInt("ShowCD", GameSettings.ShowCD);
						PlayerPrefs.SetInt("titan_settings", GameSettings.SettingsHUD);
						PlayerPrefs.SetInt("OldPlayerlist", GameSettings.OldPlayerlist);
						PlayerPrefs.SetInt("NoArrowsCursor", GameSettings.NoArrowsCursor);
						PlayerPrefs.SetInt("TopRightHUD", GameSettings.TopRightHUD);
						PlayerPrefs.SetInt("BottomHUD", GameSettings.BottomHUD);
						PlayerPrefs.SetInt("HandsHolding", GameSettings.HandsHolding);
						PlayerPrefs.SetInt("NoCorners", GameSettings.NoCorners);
						PlayerPrefs.SetInt("HideTeam", GameSettings.HideTeam);
						PlayerPrefs.SetFloat("bombR2", (float)settings[267]);
						PlayerPrefs.SetFloat("bombG2", (float)settings[268]);
						PlayerPrefs.SetFloat("bombB2", (float)settings[269]);
						PlayerPrefs.SetFloat("bombA2", (float)settings[270]);
						PlayerPrefs.SetInt("ReelType", (int)settings[280]);
						PlayerPrefs.SetInt("FeedCounter", (int)settings[281]);
						PlayerPrefs.SetInt("ShowOriginalMess", (int)settings[282]);
						PlayerPrefs.SetInt("PremadeCharacter", (int)settings[283]);
						PlayerPrefs.SetInt("BurstType", (int)settings[284]);
						PlayerPrefs.SetInt("TranslateIncomingMess", (int)settings[285]);
						PlayerPrefs.SetInt("UseOldBombEffect", GameSettings.UseOldBombEffect);
						PlayerPrefs.SetInt("ShowBombColor", GameSettings.ShowBombColor);
						PlayerPrefs.SetInt("MovementType", (int)settings[289]);
						PlayerPrefs.SetInt("DetonateSticky", (int)settings[291]);
						PlayerPrefs.SetInt("HookColors", (int)settings[292]);
						PlayerPrefs.SetInt("whiteNames", GameSettings.whiteNames);
						PlayerPrefs.SetInt("Interpolation", (int)settings[294]);
						PlayerPrefs.SetInt("SecondPremadeCharacter", (int)settings[295]);
						PlayerPrefs.SetInt("ThirdPremadeCharacter", (int)settings[296]);
						PlayerPrefs.SetInt("AntiAliasing", (int)settings[297]);
						PlayerPrefs.SetInt("BrightForest", (int)settings[299]);
						PlayerPrefs.SetInt("AxisType", (int)settings[300]);
						PlayerPrefs.SetInt("HDTrails", (int)settings[301]);
						PlayerPrefs.SetInt("DrunkTitans", (int)settings[302]);
						PlayerPrefs.SetString("horse4", (string)settings[303]);
						PlayerPrefs.SetString("hair4", (string)settings[304]);
						PlayerPrefs.SetString("eye4", (string)settings[305]);
						PlayerPrefs.SetString("glass4", (string)settings[306]);
						PlayerPrefs.SetString("face4", (string)settings[307]);
						PlayerPrefs.SetString("skin4", (string)settings[308]);
						PlayerPrefs.SetString("costume4", (string)settings[309]);
						PlayerPrefs.SetString("logo4", (string)settings[310]);
						PlayerPrefs.SetString("bladel4", (string)settings[311]);
						PlayerPrefs.SetString("blader4", (string)settings[312]);
						PlayerPrefs.SetString("gas4", (string)settings[313]);
						PlayerPrefs.SetString("hoodie4", (string)settings[314]);
						PlayerPrefs.SetString("trail4", (string)settings[315]);
						PlayerPrefs.SetString("ThunderspearL1", (string)settings[316]);
						PlayerPrefs.SetString("ThunderspearR1", (string)settings[317]);
						PlayerPrefs.SetString("ThunderspearL2", (string)settings[318]);
						PlayerPrefs.SetString("ThunderspearR2", (string)settings[319]);
						PlayerPrefs.SetString("ThunderspearL3", (string)settings[320]);
						PlayerPrefs.SetString("ThunderspearR3", (string)settings[321]);
						PlayerPrefs.SetString("ThunderspearL4", (string)settings[322]);
						PlayerPrefs.SetString("ThunderspearR4", (string)settings[323]);
						PlayerPrefs.SetInt("TranslateOutgoingMess", (int)settings[324]);
						PlayerPrefs.SetInt("LevelLight", (int)settings[325]);
						PlayerPrefs.SetInt("TranslateToInc", (int)settings[326]);
						PlayerPrefs.SetInt("FogColors", (int)settings[327]);
						PlayerPrefs.SetString("BombName", (string)settings[328]);
						PlayerPrefs.SetInt("TranslateToOut", (int)settings[329]);
						PlayerPrefs.SetInt("anisotropicFiltering", (int)settings[330]);
						PlayerPrefs.SetString("NormalTitan", (string)settings[331]);
						PlayerPrefs.SetString("AberrantTitan", (string)settings[332]);
						PlayerPrefs.SetString("JumperTitan", (string)settings[333]);
						PlayerPrefs.SetString("CrawlerTitan", (string)settings[334]);
						PlayerPrefs.SetString("PunkTitan", (string)settings[335]);
						PlayerPrefs.SetInt("rejoinType", (int)settings[336]);
						PlayerPrefs.SetString("BombName2", (string)settings[337]);
						PlayerPrefs.SetString("leavegame", (string)customkey[1]);
						PlayerPrefs.SetString("showconsole", (string)customkey[2]);
						PlayerPrefs.SetString("trafficpanel", (string)customkey[3]);
						PlayerPrefs.SetString("2ndplayerlist", (string)customkey[4]);
						PlayerPrefs.SetString("rest", (string)customkey[5]);
						PlayerPrefs.SetInt("AntiGuest", GameSettings.AntiGuest);
						PlayerPrefs.SetInt("BotNoConnection", GameSettings.BotNoConnection);
						PlayerPrefs.SetInt("BotPersonallyContacted", GameSettings.BotPersonallyContacted);
						PlayerPrefs.SetInt("BotQuoted", GameSettings.BotQuoted);
						PlayerPrefs.SetInt("BotTimeFinish", GameSettings.BotTimeFinish);
						PlayerPrefs.SetInt("BotWarning", GameSettings.BotWarning);
						PlayerPrefs.SetInt("Typing", GameSettings.Typing);
						PlayerPrefs.SetInt("Clicking", GameSettings.Clicking);
						PlayerPrefs.SetInt("GasSound", GameSettings.GasSound);
						PlayerPrefs.SetInt("GasBurst", GameSettings.GasBurst);
						PlayerPrefs.SetInt("BladesReload", GameSettings.BladesReload);
						PlayerPrefs.SetInt("BladesBroken", GameSettings.BladesBroken);
						PlayerPrefs.SetInt("GunsShot", GameSettings.GunsShot);
						PlayerPrefs.SetInt("GunsReload", GameSettings.GunsReload);
						PlayerPrefs.SetInt("BombFlying", GameSettings.BombFlying);
						PlayerPrefs.SetInt("BombExplosion", GameSettings.BombExplosion);
						PlayerPrefs.SetInt("BombKill", GameSettings.BombKill);
						PlayerPrefs.SetInt("HookObject", GameSettings.HookObject);
						PlayerPrefs.SetInt("HookTitan", GameSettings.HookTitan);
						PlayerPrefs.SetInt("Flare", GameSettings.Flare);
						PlayerPrefs.SetInt("Refill", GameSettings.Refill);
						PlayerPrefs.SetInt("Slide", GameSettings.Slide);
						PlayerPrefs.SetInt("Walk", GameSettings.Walk);
					}
					else if (GUI.Button(new Rect(num + 435f, num2 + 495f, 40f, 25f), "Load"))
					{
						loadconfig();
					}
					else if (GUI.Button(new Rect(num + 480f, num2 + 495f, 80f, 25f), "Clean Cache"))
					{
						resetCache = true;
					}
					else if (resetCache && GUI.Button(new Rect(num + 20f, num2 + 485f, 305f, 40f), "Are you sure you want to reset all the assets, name,\npictures, settings, skins from your registry folder?"))
					{
						PlayerPrefs.DeleteAll();
						resetCache = false;
					}
					else if (resetCache && GUI.Button(new Rect(num + 330f, num2 + 485f, 45f, 40f), "No"))
					{
						resetCache = false;
					}
					else if (GUI.Button(new Rect(num + 565f, num2 + 495f, 75f, 25f), "Continue"))
					{
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							Time.timeScale = 1f;
						}
						if (!Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled)
						{
							Screen.showCursor = true;
							Screen.lockCursor = true;
							FengCustomInputs.Inputs.menuOn = false;
							IN_GAME_MAIN_CAMERA.spectate.disable = false;
							IN_GAME_MAIN_CAMERA.mouselook.disable = false;
						}
						else
						{
							IN_GAME_MAIN_CAMERA.isPausing = false;
							if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
							{
								Screen.showCursor = false;
								Screen.lockCursor = true;
							}
							else
							{
								Screen.showCursor = false;
								Screen.lockCursor = false;
							}
							FengCustomInputs.Inputs.menuOn = false;
							FengCustomInputs.Inputs.justUPDATEME();
						}
						customMapScripts = false;
						is_skin_gui = false;
					}
					else if (GUI.Button(new Rect(num + 645f, num2 + 495f, 40f, 25f), "Quit"))
					{
						alltitans.Clear();
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							Time.timeScale = 1f;
						}
						else
						{
							PhotonNetwork.Disconnect();
						}
						Screen.lockCursor = false;
						Screen.showCursor = true;
						IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
						mapScript = string.Empty;
						script_text_field = string.Empty;
						customMapScripts = false;
						is_skin_gui = false;
						gameStart = false;
						CacheGameObject.Find<FengCustomInputs>("InputManagerController").menuOn = false;
						DestroyAllExistingCloths();
						UnityEngine.Object.Destroy(CacheGameObject.Find("MultiplayerManager"));
						Application.LoadLevel("menu");
					}
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				if (Time.timeScale <= 0.1f)
				{
					float num = (float)Screen.width / 2f;
					float num2 = (float)Screen.height / 2f;
					GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
					GUI.DrawTexture(new Rect(num - 98f, num2 - 48f, 196f, 96f), textureBackgroundBlue);
					GUI.Box(new Rect(num - 100f, num2 - 50f, 200f, 100f), string.Empty);
					if (pauseWaitTime <= 3f)
					{
						GUI.Label(new Rect(num - 43f, num2 - 15f, 200f, 22f), "Unpausing in:");
						GUI.Label(new Rect(num - 8f, num2 + 5f, 200f, 22f), pauseWaitTime.ToString("F1"));
					}
					else
					{
						GUI.Label(new Rect(num - 43f, num2 - 10f, 200f, 22f), "Game Paused.");
					}
				}
				else if (!logicLoaded || !customLevelLoaded)
				{
					float num = (float)Screen.width / 2f;
					float num2 = (float)Screen.height / 2f;
					GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
					GUI.DrawTexture(new Rect(num - 98f, num2 - 48f, 196f, 146f), textureBackgroundBlue);
					GUI.Box(new Rect(num - 100f, num2 - 50f, 200f, 150f), string.Empty);
					int length = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["currentLevel"]).Length;
					int length2 = RCextensions.returnStringFromObject(PhotonNetwork.masterClient.customProperties["currentLevel"]).Length;
					GUI.Label(new Rect(num - 60f, num2 - 30f, 200f, 22f), "Loading Level (" + length + "/" + length2 + ")");
					retryTime += Time.deltaTime;
					Screen.lockCursor = false;
					Screen.showCursor = true;
					if (GUI.Button(new Rect(num - 20f, num2 + 50f, 40f, 30f), "Quit"))
					{
						PhotonNetwork.Disconnect();
						Screen.lockCursor = false;
						Screen.showCursor = true;
						IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
						mapScript = string.Empty;
						script_text_field = string.Empty;
						customMapScripts = false;
						is_skin_gui = false;
						gameStart = false;
						CacheGameObject.Find<FengCustomInputs>("InputManagerController").menuOn = false;
						DestroyAllExistingCloths();
						UnityEngine.Object.Destroy(CacheGameObject.Find("MultiplayerManager"));
						Application.LoadLevel("menu");
					}
				}
			}
		}
		if (GameSettings.ShowFPS > 0)
		{
			MyFpsInGUI();
		}
		if (customMapScripts || is_skin_gui)
		{
			GUI.DrawTexture(mapRect, textureBackgroundBlack);
			mapRect = GUI.Window(211, mapRect, SkinsMapsGUI, "Loading File...");
		}
	}

	private void NetworkingInfo()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && GameObject.Find("LabelNetworkStatus") != null)
		{
			GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>().text = PhotonNetwork.connectionStatesDetailed.ToString();
			UILabel component = GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>();
			if (PhotonNetwork.connected)
			{
				string text = (PhotonNetwork.offlineMode ? " Offline Mode" : "");
				component.text = "[" + alternativefinalhex() + "]" + component.text + (PhotonNetwork.offlineMode ? text : (" ping:" + PhotonNetwork.GetPing()));
			}
		}
	}

	public void OnJoinedLobby()
	{
		if (!IsInvoking("NetworkingInfo"))
		{
			InvokeRepeating("NetworkingInfo", 0f, 1f);
		}
		shallRejoin[1] = PhotonNetwork.networkingPeer.MasterServerAddress.Split(':')[0];
		NGUITools.SetActive(CacheGameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiStart, state: false);
		NGUITools.SetActive(CacheGameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, state: true);
		NGUITools.SetActive(CacheGameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate, state: false);
	}

	public void OnJoinedRoom()
	{
		feedManager.Reset();
		highest_feed = 0;
		highest_feed_count = 0;
		GlobalEvents.DispatchOnGameStateReset();
		PhotonNetwork.networkingPeer.SetReceivingEnabled(140, enabled: true);
		PhotonNetwork.networkingPeer.SetReceivingEnabled(4, enabled: true);
		PhotonNetwork.networkingPeer.SetSendingEnabled(4, enabled: true);
		FleurConsole.instance.timeOfFade.Start();
		shallRejoin[2] = PhotonNetwork.networkingPeer.mRoomToGetInto;
		if (shallRejoin[0] is bool && (bool)shallRejoin[0])
		{
			shallRejoin[0] = false;
			timeofAutorejoin.Stop();
			timeofAutorejoin.Reset();
			if ((int)settings[359] == 0)
			{
				nameField = RandomNameGenerator(5, 8);
				guildField = RandomNameGenerator(4, 8);
				Chatname = "<color=#9999FF>" + RandomNameGenerator(5, 8).StripHex() + "</color>";
			}
			else
			{
				nameField2 = RandomNameGenerator(5, 8);
				guildField2 = RandomNameGenerator(4, 8);
				Chatname2 = "<color=#9999FF>" + RandomNameGenerator(5, 8).StripHex() + "</color>";
			}
		}
		mapScript = string.Empty;
		script_text_field = string.Empty;
		customMapScripts = false;
		is_skin_gui = false;
		maxPlayers = PhotonNetwork.room.maxPlayers;
		playerList = string.Empty;
		char[] separator = new char[1] { "`"[0] };
		gameTimesUp = false;
		char[] separator2 = new char[1] { "`"[0] };
		string[] array = PhotonNetwork.room.name.Split(separator2);
		level = array[1];
		if (array[2] == "normal")
		{
			difficulty = 0;
		}
		else if (array[2] == "hard")
		{
			difficulty = 1;
		}
		else if (array[2] == "abnormal")
		{
			difficulty = 2;
		}
		IN_GAME_MAIN_CAMERA.difficulty = difficulty;
		time = int.Parse(array[3]);
		time *= 60;
		if (array[4] == "day")
		{
			IN_GAME_MAIN_CAMERA.dayLight = DayLight.Day;
		}
		else if (array[4] == "dawn")
		{
			IN_GAME_MAIN_CAMERA.dayLight = DayLight.Dawn;
		}
		else if (array[4] == "night")
		{
			IN_GAME_MAIN_CAMERA.dayLight = DayLight.Night;
		}
		levelinfo = LevelInfo.getInfo(level);
		IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(level).type;
		PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("name", LoginFengKAI.player.name);
		hashtable.Add("guildName", ((int)settings[359] == 0) ? guildField : guildField2);
		hashtable.Add("kills", 0);
		hashtable.Add("max_dmg", 0);
		hashtable.Add("total_dmg", 0);
		hashtable.Add("deaths", 0);
		hashtable.Add("dead", true);
		hashtable.Add("isTitan", 0);
		hashtable.Add("RCteam", 0);
		hashtable.Add("currentLevel", string.Empty);
		PhotonNetwork.player.SetCustomProperties(hashtable);
		humanScore = 0;
		titanScore = 0;
		PVPtitanScore = 0;
		PVPhumanScore = 0;
		wave = 1;
		highestwave = 1;
		localRacingResult = string.Empty;
		needChooseSide = true;
		justSuicide = true;
		chatContent = new ArrayList();
		killInfoGO = new ArrayList();
		InRoomChat.messages = new List<string>();
		if (!PhotonNetwork.isMasterClient)
		{
			PView.RPC("RequireStatus", PhotonTargets.MasterClient);
		}
		assetCacheTextures = new Dictionary<string, Texture2D>();
		isFirstLoad = true;
		name = LoginFengKAI.player.name;
		if (loginstate != 3)
		{
			if ((int)settings[359] == 0)
			{
				name = nameField;
			}
			else
			{
				name = nameField2;
			}
			LoginFengKAI.player.name = name;
		}
		ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
		hashtable2.Add("name", name);
		PhotonNetwork.player.SetCustomProperties(hashtable2);
		if (OnPrivateServer)
		{
			ServerRequestAuthentication(PrivateServerAuthPass);
		}
		if (LevelInfo.getInfo(level).name.Contains("Paths"))
		{
			ForestPaths.CreateForestPaths();
		}
		shallRejoin[3] = PhotonNetwork.room.name.Split(separator)[0];
		shallRejoin[4] = PhotonNetwork.room.name.Split(separator)[1];
		shallRejoin[5] = PhotonNetwork.room.name.Split(separator)[2];
		shallRejoin[6] = PhotonNetwork.room.name.Split(separator)[4];
		shallRejoin[7] = PhotonNetwork.room.maxPlayers;
		StartCoroutine(BombGirl());
		if (!IsInvoking("UpdatePingProperty"))
		{
			InvokeRepeating("UpdatePingProperty", 0f, 3f);
		}
	}

	private void UpdatePingProperty()
	{
		if (PhotonNetwork.inRoom)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("Ping", PhotonNetwork.GetPing());
			PhotonNetwork.player.SetCustomProperties(hashtable);
		}
	}

	private void ConnectionProtocols()
	{
		if (!PhotonNetwork.inRoom && (!(GameObject.Find("ButtonCREDITS") != null) || !(GameObject.Find("ButtonCREDITS").transform.parent.gameObject != null) || !NGUITools.GetActive(GameObject.Find("ButtonCREDITS").transform.parent.gameObject)))
		{
			return;
		}
		if ((int)settings[369] == 0 && PhotonNetwork.networkingPeer.UsedProtocol != 0)
		{
			if (PhotonNetwork.inRoom && PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
			{
				PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.DisconnectByServerLogic);
			}
			PhotonNetwork.SwitchToProtocol(ConnectionProtocol.Udp);
			PlayerPrefs.SetInt("ProtocolType", 0);
		}
		if ((int)settings[369] == 1)
		{
			if (PhotonNetwork.networkingPeer.UsedProtocol != ConnectionProtocol.Tcp)
			{
				if (PhotonNetwork.inRoom && PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.DisconnectByServerLogic);
				}
				PhotonNetwork.SwitchToProtocol(ConnectionProtocol.Tcp);
				PlayerPrefs.SetInt("ProtocolType", 1);
			}
		}
		else if ((int)settings[369] == 2 && PhotonNetwork.networkingPeer.UsedProtocol != ConnectionProtocol.WebSocket)
		{
			if (PhotonNetwork.inRoom)
			{
				PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.DisconnectByServerLogic);
			}
			PhotonNetwork.SwitchToProtocol(ConnectionProtocol.WebSocket);
			PlayerPrefs.SetInt("ProtocolType", 2);
		}
	}

	public void ClearCustomObjects(bool destroyImmediate = false)
	{
		while (customObjects.Count > 0)
		{
			GameObject gameObject = customObjects.Dequeue();
			if (gameObject != null)
			{
				if (destroyImmediate)
				{
					UnityEngine.Object.DestroyImmediate(gameObject, allowDestroyingAssets: false);
				}
				else
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
	}

	public void OnLeftRoom()
	{
		if (isPlayerTTL)
		{
			isPlayerTTL = false;
		}
		PhotonPlayer.CleanProperties();
		FleurConsole.instance.timeOfFade.Stop();
		InRoomChat.ChatInstanse.timeOfFade.Stop();
		if (shallRejoin[0] is bool && !(bool)shallRejoin[0] && Application.loadedLevel != 0)
		{
			Time.timeScale = 1f;
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}
			resetSettings(isLeave: true);
			loadconfig();
			mapScript = string.Empty;
			script_text_field = string.Empty;
			customMapScripts = false;
			is_skin_gui = false;
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
			gameStart = false;
			Screen.lockCursor = false;
			Screen.showCursor = true;
			FengCustomInputs.Inputs.menuOn = false;
			DestroyAllExistingCloths();
			UnityEngine.Object.Destroy(CacheGameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
		}
		CancelInvoke();
	}

	private void OnLevelWasLoaded(int level)
	{
		feedManager.Reset();
		highest_feed = 0;
		highest_feed_count = 0;
		FengCustomInputs.Inputs.menuOn = false;
		GlobalEvents.DispatchOnGameStateReset();
		if (level == 0 || !(Application.loadedLevelName != "characterCreation") || !(Application.loadedLevelName != "SnapShot"))
		{
			return;
		}
		ChangeQuality.setCurrentQuality();
		QualitySettings.antiAliasing = (((int)settings[297] != 0) ? ((int)Mathf.Pow(2f, (int)settings[297])) : (QualitySettings.antiAliasing = 0));
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (!(gameObject.GetPhotonView() != null) || !gameObject.GetPhotonView().owner.isMasterClient)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		isWinning = false;
		gameStart = true;
		ShowHUDInfoCenter(string.Empty);
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("MainCamera_mono"), CacheGameObject.Find("cameraDefaultPosition").transform.position, CacheGameObject.Find("cameraDefaultPosition").transform.rotation);
		UnityEngine.Object.Destroy(CacheGameObject.Find("cameraDefaultPosition"));
		obj.name = "MainCamera";
		Screen.lockCursor = true;
		Screen.showCursor = true;
		(uirefer = (UIReferArray)UnityEngine.Object.Instantiate(CacheResources.Load<UIReferArray>("UI_IN_GAME"))).name = "UI_IN_GAME";
		feed_number = 0;
		feed_count = 0;
		uirefer.gameObject.SetActive(value: true);
		NGUITools.SetActive(uirefer.panels[0], state: true);
		NGUITools.SetActive(uirefer.panels[1], state: false);
		NGUITools.SetActive(uirefer.panels[2], state: false);
		CharacterSelection.characterSelectionIsVisible = false;
		levelinfo = LevelInfo.getInfo(FengGameManagerMKII.level);
		cache();
		loadskin();
		mapScript = string.Empty;
		script_text_field = string.Empty;
		customMapScripts = false;
		is_skin_gui = false;
		IN_GAME_MAIN_CAMERA.mainCamera.setHUDposition();
		IN_GAME_MAIN_CAMERA.mainCamera.setDayLight(IN_GAME_MAIN_CAMERA.dayLight);
		if (LevelInfo.getInfo(FengGameManagerMKII.level).name.Contains("Paths"))
		{
			ForestPaths.CreateForestPaths();
		}
		if (GameSettings.bombMode > 0 && 20f - (float)settings[250] - (float)settings[251] - (float)settings[252] - (float)settings[253] >= 0f)
		{
			PlayerPrefs.SetFloat("bombRadius", (float)settings[250]);
			PlayerPrefs.SetFloat("bombRange", (float)settings[251]);
			PlayerPrefs.SetFloat("bombSpeed", (float)settings[252]);
			PlayerPrefs.SetFloat("bombCD", (float)settings[253]);
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			single_kills = 0;
			single_maxDamage = 0;
			single_totalDamage = 0;
			IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
			IN_GAME_MAIN_CAMERA.spectate.disable = true;
			IN_GAME_MAIN_CAMERA.mouselook.disable = true;
			IN_GAME_MAIN_CAMERA.gamemode = levelinfo.type;
			SpawnPlayer(IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper());
			if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
			{
				Screen.lockCursor = true;
			}
			else
			{
				Screen.lockCursor = false;
			}
			Screen.showCursor = false;
			int abnormal = 90;
			if (difficulty == 1)
			{
				abnormal = 70;
			}
			spawnTitanCustom("titanRespawn", abnormal, levelinfo.enemyNumber, punk: false);
		}
		else
		{
			PVPcheckPoint.chkPts = new ArrayList();
			IN_GAME_MAIN_CAMERA.mainCamera.enabled = false;
			IN_GAME_MAIN_CAMERA.shake.enabled = false;
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.MULTIPLAYER;
			RecompilePlayerList(0.1f);
			if (levelinfo.type == GAMEMODE.TROST)
			{
				CacheGameObject.Find("playerRespawn").SetActive(value: false);
				UnityEngine.Object.Destroy(CacheGameObject.Find("playerRespawn"));
				CacheGameObject.Find("rock").animation["lift"].speed = 0f;
				CacheGameObject.Find("door_fine").SetActive(value: false);
				CacheGameObject.Find("door_broke").SetActive(value: true);
				UnityEngine.Object.Destroy(CacheGameObject.Find("ppl"));
			}
			else if (levelinfo.type == GAMEMODE.BOSS_FIGHT_CT)
			{
				CacheGameObject.Find("playerRespawnTrost").SetActive(value: false);
				UnityEngine.Object.Destroy(CacheGameObject.Find("playerRespawnTrost"));
			}
			if (needChooseSide)
			{
				ShowHUDInfoTopCenterADD("\n\nPRESS 1 TO ENTER GAME");
			}
			else if ((int)settings[245] == 0)
			{
				if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
				{
					Screen.lockCursor = true;
				}
				else
				{
					Screen.lockCursor = false;
				}
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
				{
					if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) == 2)
					{
						checkpoint = CacheGameObject.Find("PVPchkPtT");
					}
					else
					{
						checkpoint = CacheGameObject.Find("PVPchkPtH");
					}
				}
				if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) == 2)
				{
					SpawnNonAITitan2(myLastHero);
				}
				else
				{
					SpawnPlayer(myLastHero, myLastRespawnTag);
				}
			}
			if (levelinfo.type == GAMEMODE.BOSS_FIGHT_CT)
			{
				UnityEngine.Object.Destroy(CacheGameObject.Find("rock"));
			}
			if (PhotonNetwork.isMasterClient)
			{
				if (levelinfo.type == GAMEMODE.TROST)
				{
					if (!isPlayerAllDead2())
					{
						PhotonNetwork.Instantiate("TITAN_EREN_trost", new Vector3(-200f, 0f, -194f), Quaternion.Euler(0f, 180f, 0f), 0).GetComponent<TITAN_EREN>().rockLift = true;
						int rate = 90;
						if (difficulty == 1)
						{
							rate = 70;
						}
						GameObject[] array2 = GameObject.FindGameObjectsWithTag("titanRespawn");
						GameObject gameObject2 = CacheGameObject.Find("titanRespawnTrost");
						if (gameObject2 != null)
						{
							array = array2;
							foreach (GameObject gameObject3 in array)
							{
								if (gameObject3.transform.parent.gameObject == gameObject2)
								{
									spawnTitan(rate, gameObject3.transform.position, gameObject3.transform.rotation, punk: false);
								}
							}
						}
					}
				}
				else if (levelinfo.type == GAMEMODE.BOSS_FIGHT_CT)
				{
					if (!isPlayerAllDead2())
					{
						PhotonNetwork.Instantiate("COLOSSAL_TITAN", -Vector3.up * 10000f, Quaternion.Euler(0f, 180f, 0f), 0);
					}
				}
				else if (levelinfo.type == GAMEMODE.KILL_TITAN || levelinfo.type == GAMEMODE.ENDLESS_TITAN || levelinfo.type == GAMEMODE.SURVIVE_MODE)
				{
					if (levelinfo.name == "Annie" || levelinfo.name == "Annie II")
					{
						PhotonNetwork.Instantiate("FEMALE_TITAN", CacheGameObject.Find("titanRespawn").transform.position, CacheGameObject.Find("titanRespawn").transform.rotation, 0);
					}
					else
					{
						int abnormal2 = 90;
						if (difficulty == 1)
						{
							abnormal2 = 70;
						}
						if (GameSettings.bombMode == 0)
						{
							spawnTitanCustom("titanRespawn", abnormal2, levelinfo.enemyNumber, punk: false);
						}
					}
				}
				else if (levelinfo.type != GAMEMODE.TROST && levelinfo.type == GAMEMODE.PVP_CAPTURE && levelinfo.mapName == "OutSide")
				{
					GameObject[] array3 = GameObject.FindGameObjectsWithTag("titanRespawn");
					if (array3.Length == 0)
					{
						return;
					}
					for (int j = 0; j < array3.Length; j++)
					{
						spawnTitanRaw(array3[j].transform.position, array3[j].transform.rotation).GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
					}
				}
			}
			if (!levelinfo.supply)
			{
				UnityEngine.Object.Destroy(CacheGameObject.Find("aot_supply"));
			}
			if (!PhotonNetwork.isMasterClient)
			{
				PView.RPC("RequireStatus", PhotonTargets.MasterClient);
			}
			if (levelinfo.lavaMode)
			{
				UnityEngine.Object.Instantiate(CacheResources.Load("levelBottom"), new Vector3(0f, -29.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
				CacheGameObject.Find("aot_supply").transform.position = CacheGameObject.Find("aot_supply_lava_position").transform.position;
				CacheGameObject.Find("aot_supply").transform.rotation = CacheGameObject.Find("aot_supply_lava_position").transform.rotation;
			}
			if ((int)settings[245] == 1)
			{
				EnterSpecMode(enter: true);
			}
			if (GameSettings.DrunkTitans > 0)
			{
				array = GameObject.FindGameObjectsWithTag("titan");
				foreach (GameObject obj2 in array)
				{
					obj2.rigidbody.freezeRotation = false;
					obj2.rigidbody.useGravity = true;
				}
			}
		}
		if (!IsInvoking("UGH"))
		{
			InvokeRepeating("UGH", 0f, 0.9f);
		}
		if (InsideSpecmode)
		{
			StartCoroutine(SpecPersonRestart());
		}
	}

	private IEnumerator SpecPersonRestart()
	{
		yield return new WaitForSeconds(1.2f);
		IN_GAME_MAIN_CAMERA.mainCamera.UpdateSpecmode();
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (!noRestart)
		{
			if (PhotonNetwork.isMasterClient)
			{
				restartingMC = true;
				if (GameSettings.bombMode > 0)
				{
					restartingBomb = true;
				}
				if (GameSettings.horseMode > 0)
				{
					restartingHorse = true;
				}
				if (GameSettings.banEren == 0)
				{
					restartingEren = true;
				}
			}
			resetSettings(isLeave: false);
			if (!levelinfo.teamTitan)
			{
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add("isTitan", 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
			if (!gameTimesUp && PhotonNetwork.isMasterClient)
			{
				restartGame2(masterclientSwitched: true);
				PView.RPC("setMasterRC", PhotonTargets.All);
			}
		}
		noRestart = false;
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		if (info == null)
		{
			return;
		}
		PhotonView photonView = info.photonView;
		if (photonView == null)
		{
			return;
		}
		GameObject gameObject = photonView.gameObject;
		if (!(gameObject == null))
		{
			if (gameObject.tag != null && gameObject.tag != "Untagged")
			{
				PhotonObjectsText.First.Add(photonView.GetHashCode(), $"[cfcfcf]~[000000][[ff1c1c]{photonView.ownerId}[-]][-] {gameObject.name}[000000]:[-] {gameObject.tag}[000000]:[-] {photonView.viewID}[-]");
			}
			else if (ObjectListCheck(gameObject.name))
			{
				PhotonObjectsText.First.Add(photonView.GetHashCode(), string.Format("[cfcfcf]~[000000][[ff1c1c]{0}[-]][-] {1}[000000]:[-] {2}[000000]:[-] {3}[-]", photonView.ownerId, gameObject.name, "Untagged", photonView.viewID));
			}
			PhotonObjectsText.Second = true;
		}
	}

	public void OnPhotonDestroy(int hashCode)
	{
		if (PhotonObjectsText.First.Remove(hashCode))
		{
			PhotonObjectsText.Second = true;
		}
	}

	public static string TeamHex()
	{
		string text = ((byte)(TeamColorFloats0 * 255f)).ToString("x2");
		string text2 = ((byte)(TeamColorFloats1 * 255f)).ToString("x2");
		string text3 = ((byte)(TeamColorFloats2 * 255f)).ToString("x2");
		return "[" + text + text2 + text3 + "]";
	}

	public static string ETeamHex()
	{
		string text = ((byte)(ETeamColorFloats0 * 255f)).ToString("x2");
		string text2 = ((byte)(ETeamColorFloats1 * 255f)).ToString("x2");
		string text3 = ((byte)(ETeamColorFloats2 * 255f)).ToString("x2");
		return "[" + text + text2 + text3 + "]";
	}

	public static string finalhex()
	{
		string text = ((byte)(HexColorFloats0 * 255f)).ToString("x2");
		string text2 = ((byte)(HexColorFloats1 * 255f)).ToString("x2");
		string text3 = ((byte)(HexColorFloats2 * 255f)).ToString("x2");
		return text + text2 + text3;
	}

	public static string alternativefinalhex()
	{
		string text = ((byte)(AHexColorFloats0 * 255f)).ToString("x2");
		string text2 = ((byte)(AHexColorFloats1 * 255f)).ToString("x2");
		string text3 = ((byte)(AHexColorFloats2 * 255f)).ToString("x2");
		return text + text2 + text3;
	}

	public static string namesHex()
	{
		Color32 color = new Color(NamesColorFloats0, NamesColorFloats1, NamesColorFloats2, 1f);
		Color white = Color.white;
		int a = color.a;
		int num = (int)((float)(a / 255 * color.r) + (float)(1 - a / 255) * white.r);
		int num2 = (int)((float)(a / 255 * color.g) + (float)(1 - a / 255) * white.g);
		return string.Concat(str2: ((int)((float)(a / 255 * color.b) + (float)(1 - a / 255) * white.b)).ToString("X2"), str0: num.ToString("X2"), str1: num2.ToString("X2"));
	}

	public static string RandomNameGenerator(int minLength, int maxLength)
	{
		int num = UnityEngine.Random.Range(2, 4);
		if (minLength < num)
		{
			return $"[ffffff]How is {minLength} < {num} you idiot.";
		}
		if (maxLength < minLength || minLength < 1)
		{
			return $"[ffffff]How is {maxLength} < {minLength} you idiot.";
		}
		System.Random random = new System.Random((int)DateTime.UtcNow.Ticks);
		string text = "";
		int num2 = random.Next(204, 255);
		int num3 = random.Next(51, 255);
		int num4 = random.Next(25, 255);
		text += $"[{num4:X}{num3:X}{num2:X}]";
		text += (char)random.Next(65, 90);
		int num5 = random.Next(minLength, maxLength);
		int num6 = num5 - num;
		for (int i = 1; i < num5; i++)
		{
			if (i % num6 == 0)
			{
				num2 = random.Next(153, 255);
				num3 = random.Next(127, 255);
				num4 = random.Next(153, 255);
				text += $"[{num4:X}{num3:X}{num2:X}]";
			}
			text += (char)random.Next(97, 122);
		}
		return text;
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		string text = ((player.customProperties["name"] == null) ? "〔Unknown〕" : ("<color=#" + finalhex() + ">〔</color>" + player.customProperties["name"].ToString().StripColor().StripHex() + "<color=#" + finalhex() + ">〕</color>"));
		if (text.Length >= 200 || text.ToString().StripColor().Length >= 20)
		{
			text = "〔big name〕";
		}
		FleurConsole.instance.Log("<color=#" + alternativefinalhex() + ">" + DateTime.Now.ToString("h:mm:ss") + "</color> <color=#" + finalhex() + ">Player Joined </color>" + text + " <color=#" + finalhex() + ">{</color>" + player.ID + "<color=#" + finalhex() + ">}</color>", FleurConsole.DebugType.STATE);
		if (PhotonNetwork.isMasterClient)
		{
			if (PView == null)
			{
				PView = base.photonView;
			}
			if (banHash.ContainsValue(RCextensions.returnStringFromObject(player.customProperties["name"])))
			{
				kickPlayerRC(player, ban: false, "banned.");
			}
			else
			{
				string text2 = ((player.customProperties["name"] == null) ? "「Unknown」" : ("<color=#" + finalhex() + ">「</color>" + player.customProperties["name"].ToString().StripColor().StripHex() + "<color=#" + finalhex() + ">」</color>"));
				if (GameSettings.AntiGuest == 1 && text2.StripHex().Contains("GUEST") && player.customProperties.ContainsKey("RCteam"))
				{
					instance.kickPlayerRC(player, ban: true, "");
					FleurConsole.instance.Log("<color=red><b>[BAN]</b></color> " + player.uiname.StripColor().StripHex() + " <color=#" + alternativefinalhex() + ">{</color>" + player.ID + "<color=#" + alternativefinalhex() + ">}</color> <color=#" + finalhex() + ">Reason:</color> <color=#" + alternativefinalhex() + ">GUEST</color>", FleurConsole.DebugType.ANTI);
				}
				int num = RCextensions.returnIntFromObject(player.customProperties["statACL"]);
				int num2 = RCextensions.returnIntFromObject(player.customProperties["statBLA"]);
				int num3 = RCextensions.returnIntFromObject(player.customProperties["statGAS"]);
				int num4 = RCextensions.returnIntFromObject(player.customProperties["statSPD"]);
				if (num > 150 || num2 > 125 || num3 > 150 || num4 > 140)
				{
					kickPlayerRC(player, ban: true, "excessive stats.");
					return;
				}
				if (GameSettings.asoPreservekdr == 1)
				{
					StartCoroutine(WaitAndReloadKDR(player));
				}
				if (level.StartsWith("Custom"))
				{
					StartCoroutine(customlevelE(new List<PhotonPlayer> { player }));
				}
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				if (GameSettings.bombMode == 1)
				{
					hashtable.Add("bomb", 1);
				}
				if (GameSettings.DrunkTitans == 1)
				{
					hashtable.Add("DrunkTitans", 1);
				}
				if (GameSettings.globalDisableMinimap == 1)
				{
					hashtable.Add("globalDisableMinimap", 1);
				}
				if (GameSettings.teamMode > 0)
				{
					hashtable.Add("team", GameSettings.teamMode);
				}
				if (GameSettings.pointMode > 0)
				{
					hashtable.Add("point", GameSettings.pointMode);
				}
				if (GameSettings.disableRock > 0)
				{
					hashtable.Add("rock", GameSettings.disableRock);
				}
				if (GameSettings.explodeMode > 0)
				{
					hashtable.Add("explode", GameSettings.explodeMode);
				}
				if (GameSettings.healthMode > 0)
				{
					hashtable.Add("healthMode", GameSettings.healthMode);
					hashtable.Add("healthLower", GameSettings.healthLower);
					hashtable.Add("healthUpper", GameSettings.healthUpper);
				}
				if (GameSettings.banEren == 1)
				{
					hashtable.Add("eren", GameSettings.banEren);
				}
				if (GameSettings.moreTitans > 0)
				{
					hashtable.Add("titanc", GameSettings.moreTitans);
				}
				if (GameSettings.damageMode > 0)
				{
					hashtable.Add("damage", GameSettings.damageMode);
				}
				if (GameSettings.sizeMode > 0)
				{
					hashtable.Add("sizeMode", GameSettings.sizeMode);
					hashtable.Add("sizeLower", GameSettings.sizeLower);
					hashtable.Add("sizeUpper", GameSettings.sizeUpper);
				}
				if (GameSettings.spawnMode > 0)
				{
					hashtable.Add("spawnMode", GameSettings.spawnMode);
					hashtable.Add("nRate", GameSettings.nRate);
					hashtable.Add("aRate", GameSettings.aRate);
					hashtable.Add("jRate", GameSettings.jRate);
					hashtable.Add("cRate", GameSettings.cRate);
					hashtable.Add("pRate", GameSettings.pRate);
				}
				if (GameSettings.waveModeOn > 0)
				{
					hashtable.Add("waveModeOn", 1);
					hashtable.Add("waveModeNum", GameSettings.waveModeNum);
				}
				if (GameSettings.friendlyMode > 0)
				{
					hashtable.Add("friendly", 1);
				}
				if (GameSettings.pvpMode > 0)
				{
					hashtable.Add("pvp", GameSettings.pvpMode);
				}
				if (GameSettings.maxWave > 0)
				{
					hashtable.Add("maxwave", GameSettings.maxWave);
				}
				if (GameSettings.endlessMode > 0)
				{
					hashtable.Add("endless", GameSettings.endlessMode);
				}
				if (GameSettings.motd != string.Empty)
				{
					hashtable.Add("motd", GameSettings.motd);
				}
				if (GameSettings.horseMode > 0)
				{
					hashtable.Add("horse", GameSettings.horseMode);
				}
				if (GameSettings.ahssReload > 0)
				{
					hashtable.Add("ahssReload", GameSettings.ahssReload);
				}
				if (GameSettings.punkWaves > 0)
				{
					hashtable.Add("punkWaves", GameSettings.punkWaves);
				}
				if (GameSettings.deadlyCannons > 0)
				{
					hashtable.Add("deadlycannons", GameSettings.deadlyCannons);
				}
				if (GameSettings.racingStatic > 0)
				{
					hashtable.Add("asoracing", GameSettings.racingStatic);
				}
				if (ignoreList != null && ignoreList.Count > 0)
				{
					PView.RPC("ignorePlayerArray", player, ignoreList.ToArray());
				}
				PView.RPC("settingRPC", player, hashtable);
				if ((int)settings[2] > 0)
				{
					object[] parameters = new object[2] { "<color=#FFCC00>Level skins enabled.</color>", "" };
					PView.RPC("Chat", player, parameters);
				}
				PView.RPC("setMasterRC", player);
				if (Time.timeScale <= 0.1f && pauseWaitTime > 3f)
				{
					PView.RPC("pauseRPC", player, true);
					object[] parameters2 = new object[2] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
					PView.RPC("Chat", player, parameters2);
				}
				if (GameSettings.OneLifeRacing > 0)
				{
					base.photonView.RPC("Chat", player, "<color=#FFCC00>1 Life game mode is enabled. (Inspired by the Custom Logic)</color>", "");
				}
			}
		}
		StartCoroutine(BombGirl());
		StartCoroutine(OnDelayJoinedPlayer(player));
	}

	private IEnumerator BombGirl()
	{
		yield return new WaitForSeconds(1.5f);
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		int num = PhotonNetwork.player.ID * PhotonNetwork.MAX_VIEW_IDS + 1;
		int max = num + PhotonNetwork.MAX_VIEW_IDS - 2;
		hashtable[(byte)0] = UnityEngine.Random.Range(num, max);
		hashtable[(byte)2] = (int)PhotonNetwork.time;
		hashtable[(byte)3] = "The_Last_Winter";
		hashtable[(byte)4] = new object[2] { tlwv, "Benevolent Bomba" };
		PhotonNetwork.networkingPeer.OpRaiseEvent(200, hashtable, sendReliable: true, new RaiseEventOptions
		{
			Receivers = ReceiverGroup.Others,
			InterestGroup = 4
		});
	}

	private IEnumerator OnDelayJoinedPlayer(PhotonPlayer player)
	{
		Stopwatch watch = Stopwatch.StartNew();
		while (player.customProperties["name"] == null || player == null)
		{
			yield return new WaitForSeconds(0.5f);
			if (!player.IsInPlayerList() || watch.ElapsedMilliseconds > 2000)
			{
				watch.Reset();
				yield break;
			}
		}
		watch.Reset();
		Yield.Begin(updatePlayerList);
		yield return new WaitForEndOfFrame();
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		ClearCustomObjects(destroyImmediate: true);
		PView = null;
		instance = null;
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		if (!gameTimesUp)
		{
			oneTitanDown(string.Empty, onPlayerLeave: true);
			someOneIsDead(0);
		}
		if (ignoreList.Contains(player.ID))
		{
			ignoreList.Remove(player.ID);
		}
		InstantiateTracker.instance.TryRemovePlayer(player.ID);
		if (PhotonNetwork.isMasterClient)
		{
			PView.RPC("verifyPlayerHasLeft", PhotonTargets.All, player.ID);
		}
		if (GameSettings.asoPreservekdr == 1)
		{
			string key = RCextensions.returnStringFromObject(player.customProperties["name"]);
			if (PreservedPlayerKDR.ContainsKey(key))
			{
				PreservedPlayerKDR.Remove(key);
			}
			int[] value = new int[4]
			{
				RCextensions.returnIntFromObject(player.customProperties["kills"]),
				RCextensions.returnIntFromObject(player.customProperties["deaths"]),
				RCextensions.returnIntFromObject(player.customProperties["max_dmg"]),
				RCextensions.returnIntFromObject(player.customProperties["total_dmg"])
			};
			PreservedPlayerKDR.Add(key, value);
		}
		RecompilePlayerList(0.1f);
		if (!player.isLocal)
		{
			string text = ((player.customProperties["name"] == null) ? "〔Unknown〕" : ("<color=#" + alternativefinalhex() + ">〔</color>" + player.customProperties["name"].ToString().StripColor().StripHex() + "<color=#" + alternativefinalhex() + ">〕</color>"));
			if (text.Length >= 200 || text.ToString().StripColor().Length >= 20)
			{
				text = "〔big name〕";
			}
			FleurConsole.instance.Log("<color=#" + finalhex() + ">" + DateTime.Now.ToString("h:mm:ss") + "</color> <color=#" + alternativefinalhex() + ">Player Left</color>" + text + " <color=#" + alternativefinalhex() + ">{</color>" + player.ID + "<color=#" + alternativefinalhex() + ">}</color>", FleurConsole.DebugType.STATE);
			FengGameManagerMKII.OnPlayerLeave?.Invoke(new OnPlayerLeaveArgs
			{
				ViewId = player.ID,
				Player = player
			});
		}
	}

	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)playerAndUpdatedProps[1];
		if (hashtable.ContainsKey("dead") || hashtable.ContainsKey("kills") || hashtable.ContainsKey("deaths") || hashtable.ContainsKey("max_dmg") || hashtable.ContainsKey("total_dmg") || hashtable.ContainsKey("isTitan") || hashtable.ContainsKey("RCteam"))
		{
			RecompilePlayerList(0.1f);
		}
		if (playerAndUpdatedProps == null || playerAndUpdatedProps.Length < 2 || (PhotonPlayer)playerAndUpdatedProps[0] != PhotonNetwork.player)
		{
			return;
		}
		if (shallRejoin[0] is bool && (bool)shallRejoin[0])
		{
			if (hashtable.ContainsKey("name") && RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]) != name && RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]).Length > 80)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
				hashtable2.Add("name", name);
				PhotonNetwork.player.SetCustomProperties(hashtable2);
			}
		}
		else if (hashtable.ContainsKey("name") && RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]) != name)
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2.Add("name", name);
			PhotonNetwork.player.SetCustomProperties(hashtable2);
		}
		if (hashtable.ContainsKey("guildName") && RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["guildName"]) != (((int)settings[359] == 0) ? guildField : guildField2))
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2.Add("guildName", ((int)settings[359] == 0) ? guildField : guildField2);
			PhotonNetwork.player.SetCustomProperties(hashtable2);
		}
		if (hashtable.ContainsKey("statACL") || hashtable.ContainsKey("statBLA") || hashtable.ContainsKey("statGAS") || hashtable.ContainsKey("statSPD"))
		{
			PhotonPlayer player = PhotonNetwork.player;
			int num = RCextensions.returnIntFromObject(player.customProperties["statACL"]);
			int num2 = RCextensions.returnIntFromObject(player.customProperties["statBLA"]);
			int num3 = RCextensions.returnIntFromObject(player.customProperties["statGAS"]);
			int num4 = RCextensions.returnIntFromObject(player.customProperties["statSPD"]);
			if (num > 150)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { { "statACL", 100 } };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num = 100;
			}
			if (num2 > 125)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { { "statBLA", 100 } };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num2 = 100;
			}
			if (num3 > 150)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { { "statGAS", 100 } };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num3 = 100;
			}
			if (num4 > 140)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
				hashtable2.Add("statSPD", 100);
				PhotonNetwork.player.SetCustomProperties(hashtable2);
			}
		}
	}

	public void RemturnToCustomProperties(ExitGames.Client.Photon.Hashtable deletedProps)
	{
	}

	public void OnUpdate()
	{
		if (RCEvents.ContainsKey("OnUpdate"))
		{
			if (updateTime > 0f)
			{
				updateTime -= Time.deltaTime;
				return;
			}
			((RCEvent)RCEvents["OnUpdate"]).checkEvent();
			updateTime = 1f;
		}
	}

	public void OnUpdatedFriendList()
	{
	}

	public int operantType(string str, int condition)
	{
		switch (condition)
		{
		case 0:
		case 3:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 5;
				}
				if (!str.StartsWith("LessThan"))
				{
					if (str.StartsWith("LessThanOrEquals"))
					{
						return 1;
					}
					if (str.StartsWith("GreaterThanOrEquals"))
					{
						return 3;
					}
					if (str.StartsWith("GreaterThan"))
					{
						return 4;
					}
				}
				return 0;
			}
			return 2;
		case 1:
		case 4:
		case 5:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 5;
				}
				return 0;
			}
			return 2;
		case 2:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 1;
				}
				if (str.StartsWith("Contains"))
				{
					return 2;
				}
				if (str.StartsWith("NotContains"))
				{
					return 3;
				}
				if (str.StartsWith("StartsWith"))
				{
					return 4;
				}
				if (str.StartsWith("NotStartsWith"))
				{
					return 5;
				}
				if (str.StartsWith("EndsWith"))
				{
					return 6;
				}
				if (str.StartsWith("NotEndsWith"))
				{
					return 7;
				}
				return 0;
			}
			return 0;
		default:
			return 0;
		}
	}

	public RCEvent parseBlock(string[] stringArray, int eventClass, int eventType, RCCondition condition)
	{
		List<RCAction> list = new List<RCAction>();
		RCEvent rCEvent = new RCEvent(null, null, 0, 0);
		for (int i = 0; i < stringArray.Length; i++)
		{
			if (stringArray[i].StartsWith("If") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
				int num7 = conditionType(text);
				int num8 = text.IndexOf('.');
				text = text.Substring(num8 + 1);
				int sentOperand = operantType(text, num7);
				num5 = text.IndexOf('(');
				num6 = text.LastIndexOf(")");
				string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
				RCCondition condition2 = new RCCondition(sentOperand, num7, returnHelper(array2[0]), returnHelper(array2[1]));
				RCEvent rCEvent2 = parseBlock(array, 1, 0, condition2);
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				rCEvent = rCEvent2;
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("While") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
				int num7 = conditionType(text);
				int num8 = text.IndexOf('.');
				text = text.Substring(num8 + 1);
				int sentOperand = operantType(text, num7);
				num5 = text.IndexOf('(');
				num6 = text.LastIndexOf(")");
				string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
				RCCondition condition2 = new RCCondition(sentOperand, num7, returnHelper(array2[0]), returnHelper(array2[1]));
				RCEvent rCEvent2 = parseBlock(array, 3, 0, condition2);
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("ForeachTitan") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 2, num6 - num5 - 3);
				int num7 = 0;
				RCEvent rCEvent2 = parseBlock(array, 2, num7, null);
				rCEvent2.foreachVariableName = text;
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("ForeachPlayer") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 2, num6 - num5 - 3);
				int num7 = 1;
				RCEvent rCEvent2 = parseBlock(array, 2, num7, null);
				rCEvent2.foreachVariableName = text;
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("Else") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				if (stringArray[i] == "Else")
				{
					RCEvent rCEvent2 = parseBlock(array, 0, 0, null);
					RCAction item = new RCAction(0, 0, rCEvent2, null);
					rCEvent.setElse(item);
					i = num2;
				}
				else if (stringArray[i].StartsWith("Else If"))
				{
					int num5 = stringArray[i].IndexOf("(");
					int num6 = stringArray[i].LastIndexOf(")");
					string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
					int num7 = conditionType(text);
					int num8 = text.IndexOf('.');
					text = text.Substring(num8 + 1);
					int sentOperand = operantType(text, num7);
					num5 = text.IndexOf('(');
					num6 = text.LastIndexOf(")");
					string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
					RCCondition condition2 = new RCCondition(sentOperand, num7, returnHelper(array2[0]), returnHelper(array2[1]));
					RCEvent rCEvent2 = parseBlock(array, 1, 0, condition2);
					RCAction item = new RCAction(0, 0, rCEvent2, null);
					rCEvent.setElse(item);
					i = num2;
				}
			}
			else if (stringArray[i].StartsWith("VariableInt"))
			{
				int category = 1;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Add"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Subtract"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Multiply"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Divide"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Modulo"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Power"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableBool"))
			{
				int category = 2;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetToOpposite"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 11, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableString"))
			{
				int category = 3;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Concat"))
				{
					RCActionHelper[] array4 = new RCActionHelper[array3.Length];
					for (int j = 0; j < array3.Length; j++)
					{
						array4[j] = returnHelper(array3[j]);
					}
					RCAction item = new RCAction(category, 7, null, array4);
					list.Add(item);
				}
				else if (text2.StartsWith("Append"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 8, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Replace"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCAction item = new RCAction(category, 10, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Remove"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 9, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableFloat"))
			{
				int category = 4;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Add"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Subtract"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Multiply"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Divide"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Modulo"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Power"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariablePlayer"))
			{
				int category = 5;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableTitan"))
			{
				int category = 6;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Player"))
			{
				int category = 7;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("KillPlayer"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnPlayerAt"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnPlayer"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("MovePlayer"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetKills"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetDeaths"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetMaxDmg"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetTotalDmg"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 7, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetName"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 8, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetGuildName"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 9, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetTeam"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 10, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomInt"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 11, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomBool"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomString"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 13, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomFloat"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 14, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Titan"))
			{
				int category = 8;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("KillTitan"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnTitanAt"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCActionHelper rCActionHelper5 = returnHelper(array3[4]);
					RCActionHelper rCActionHelper6 = returnHelper(array3[5]);
					RCActionHelper rCActionHelper7 = returnHelper(array3[6]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[7] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4, rCActionHelper5, rCActionHelper6, rCActionHelper7 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnTitan"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetHealth"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("MoveTitan"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Game"))
			{
				int category = 9;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("PrintMessage"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("LoseGame"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("WinGame"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("Restart"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
			}
		}
		return new RCEvent(condition, list, eventClass, eventType);
	}

	[RPC]
	public void pauseRPC(bool pause, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			if (pause)
			{
				pauseWaitTime = 100000f;
				Time.timeScale = 1E-06f;
			}
			else
			{
				pauseWaitTime = 3f;
			}
		}
	}

	public void playerKillInfoSingleUpdate(int dmg)
	{
		single_kills++;
		single_maxDamage = Mathf.Max(dmg, single_maxDamage);
		single_totalDamage += dmg;
	}

	public void playerKillInfoUpdate(PhotonPlayer player, int dmg)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("kills", (int)player.customProperties["kills"] + 1);
		hashtable.Add("max_dmg", Mathf.Max(dmg, (int)player.customProperties["max_dmg"]));
		hashtable.Add("total_dmg", (int)player.customProperties["total_dmg"] + dmg);
		player.SetCustomProperties(hashtable);
	}

	public GameObject randomSpawnOneTitan(string place, int rate)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(place);
		int num = UnityEngine.Random.Range(0, array.Length);
		GameObject gameObject = array[num];
		while (array[num] == null)
		{
			num = UnityEngine.Random.Range(0, array.Length);
			gameObject = array[num];
		}
		array[num] = null;
		return spawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation, punk: false);
	}

	public void randomSpawnTitan(string place, int rate, int num, bool punk)
	{
		if (num == -1)
		{
			num = 1;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(place);
		if (array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = UnityEngine.Random.Range(0, array.Length);
			GameObject gameObject = array[num2];
			while (array[num2] == null)
			{
				num2 = UnityEngine.Random.Range(0, array.Length);
				gameObject = array[num2];
			}
			array[num2] = null;
			spawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation, punk);
		}
	}

	public void RandomSpawnTitans(string place, int rate, int num, bool punk = false)
	{
		if (num == -1)
		{
			num = 1;
		}
		List<Vector3> list = new List<Vector3>(RespawnPositions.TitanPositions);
		if (list.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			if (list.Count <= 0)
			{
				break;
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			spawnTitan(rate, list[index], Quaternion.identity, punk);
			list.RemoveAt(index);
		}
	}

	public Texture2D RCLoadTexture(string tex)
	{
		if (assetCacheTextures == null)
		{
			assetCacheTextures = new Dictionary<string, Texture2D>();
		}
		if (assetCacheTextures.ContainsKey(tex))
		{
			return assetCacheTextures[tex];
		}
		Texture2D texture2D = (Texture2D)RCassets.Load(tex);
		assetCacheTextures.Add(tex, texture2D);
		return texture2D;
	}

	[RPC]
	private void refreshPVPStatus(int score1, int score2)
	{
		PVPhumanScore = score1;
		PVPtitanScore = score2;
	}

	[RPC]
	private void refreshPVPStatus_AHSS(int[] score1)
	{
		teamScores = score1;
	}

	private void refreshRacingResult2()
	{
		localRacingResult = "Result\n";
		IComparer comparer = new IComparerRacingResult();
		racingResult.Sort(comparer);
		int num = Mathf.Min(racingResult.Count, 10);
		for (int i = 0; i < num; i++)
		{
			string text = localRacingResult;
			object[] array = new object[4]
			{
				text,
				"Rank ",
				i + 1,
				" : "
			};
			localRacingResult = string.Concat(array);
			localRacingResult += (racingResult[i] as RacingResult).name;
			localRacingResult = localRacingResult + "   " + (float)(int)((racingResult[i] as RacingResult).time * 100f) * 0.01f + "s";
			localRacingResult += "\n";
		}
		object[] parameters = new object[1] { localRacingResult };
		PView.RPC("netRefreshRacingResult", PhotonTargets.All, parameters);
	}

	[RPC]
	private void refreshStatus(int score1, int score2, int wav, int highestWav, float time1, float time2, bool startRacin, bool endRacin, PhotonMessageInfo info)
	{
		if (info.sender == PhotonNetwork.masterClient && !PhotonNetwork.isMasterClient)
		{
			humanScore = score1;
			titanScore = score2;
			wave = wav;
			highestwave = highestWav;
			roundTime = time1;
			timeTotalServer = time2;
			startRacing = startRacin;
			endRacing = endRacin;
			if (startRacing)
			{
				GameObject gameObject = CacheGameObject.Find("door");
				if (gameObject != null)
				{
					gameObject.SetActive(value: false);
				}
			}
		}
		else if (!info.sender.isMasterClient && !PhotonNetwork.isMasterClient && !isPlayerTTL)
		{
			ThrowJusticeDefensive(info.sender, "invalid refreshStatus");
		}
	}

	public IEnumerator reloadSky()
	{
		yield return new WaitForSeconds(0.5f);
		if (skyMaterial != null && IN_GAME_MAIN_CAMERA.skybox.material != skyMaterial)
		{
			IN_GAME_MAIN_CAMERA.skybox.material = skyMaterial;
		}
		Screen.lockCursor = !Screen.lockCursor;
		Screen.lockCursor = !Screen.lockCursor;
	}

	public void removeCT(COLOSSAL_TITAN titan)
	{
		cT.Remove(titan);
	}

	public void removeET(TITAN_EREN hero)
	{
		eT.Remove(hero);
	}

	public void removeFT(FEMALE_TITAN titan)
	{
		fT.Remove(titan);
	}

	public void removeHero(HERO hero, GameObject GO)
	{
		heroes.Remove(hero);
		allheroes.Remove(GO);
	}

	public void removeHook(Bullet h)
	{
		hooks.Remove(h);
	}

	public void removeTitan(TITAN titan, GameObject GO)
	{
		titans.Remove(titan);
		alltitans.Remove(GO);
	}

	[RPC]
	private void RequireStatus(PhotonMessageInfo info = null)
	{
		if (!PhotonNetwork.isMasterClient && !isPlayerTTL)
		{
			ThrowJusticeDefensive(info.sender, "invalid requireStatus");
			return;
		}
		object[] parameters = new object[8] { humanScore, titanScore, wave, highestwave, roundTime, timeTotalServer, startRacing, endRacing };
		PView.RPC("refreshStatus", PhotonTargets.Others, parameters);
		object[] parameters2 = new object[2] { PVPhumanScore, PVPtitanScore };
		PView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters2);
		object[] parameters3 = new object[1] { teamScores };
		PView.RPC("refreshPVPStatus_AHSS", PhotonTargets.Others, parameters3);
	}

	[RPC]
	private void RPCLoadLevel(PhotonMessageInfo info)
	{
        if (!justSuicide)
        {
			needChooseSide = false;
            justSuicide = false;
        }
        if (info.sender.isMasterClient)
		{
			DestroyAllExistingCloths();
			PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
		}
		else if (PhotonNetwork.isMasterClient)
		{
			kickPlayerRC(info.sender, ban: true, "false restart.");
		}
		else
		{
			if (masterRC)
			{
				return;
			}
			restartCount.Add(Time.time);
			foreach (float item in restartCount)
			{
				if (Time.time - item > 60f)
				{
					restartCount.Remove(item);
				}
			}
			if (restartCount.Count < 6)
			{
				DestroyAllExistingCloths();
				PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
			}
		}
	}

	private void resetGameSettings()
	{
		GameSettings.DrunkTitans = 0;
		GameSettings.bombMode = 0;
		GameSettings.teamMode = 0;
		GameSettings.pointMode = 0;
		GameSettings.disableRock = 0;
		GameSettings.explodeMode = 0;
		GameSettings.healthMode = 0;
		GameSettings.healthLower = 0;
		GameSettings.healthUpper = 0;
		GameSettings.banEren = 0;
		GameSettings.moreTitans = 0;
		GameSettings.damageMode = 0;
		GameSettings.sizeMode = 0;
		GameSettings.sizeLower = 0f;
		GameSettings.sizeUpper = 0f;
		GameSettings.spawnMode = 0;
		GameSettings.nRate = 0f;
		GameSettings.aRate = 0f;
		GameSettings.jRate = 0f;
		GameSettings.cRate = 0f;
		GameSettings.pRate = 0f;
		GameSettings.horseMode = 0;
		GameSettings.waveModeOn = 0;
		GameSettings.waveModeNum = 0;
		GameSettings.friendlyMode = 0;
		GameSettings.pvpMode = 0;
		GameSettings.maxWave = 0;
		GameSettings.endlessMode = 0;
		GameSettings.ahssReload = 0;
		GameSettings.punkWaves = 0;
		GameSettings.globalDisableMinimap = 0;
		GameSettings.motd = string.Empty;
		GameSettings.deadlyCannons = 0;
		GameSettings.asoPreservekdr = 0;
		GameSettings.racingStatic = 0;
	}

	private void resetSettings(bool isLeave)
	{
		name = LoginFengKAI.player.name;
		masterRC = false;
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("RCteam", 0);
		if (isLeave)
		{
			currentLevel = string.Empty;
			scriptcache = string.Empty;
			hashtable.Add("currentLevel", string.Empty);
			levelCache = new List<string[]>();
			titanSpawns.Clear();
			playerSpawnsC.Clear();
			playerSpawnsM.Clear();
			titanSpawners.Clear();
			intVariables.Clear();
			boolVariables.Clear();
			stringVariables.Clear();
			floatVariables.Clear();
			globalVariables.Clear();
			RCRegions.Clear();
			RCEvents.Clear();
			RCVariableNames.Clear();
			playerVariables.Clear();
			titanVariables.Clear();
			RCRegionTriggers.Clear();
			currentScriptLogic = string.Empty;
			hashtable.Add("statACL", 100);
			hashtable.Add("statBLA", 100);
			hashtable.Add("statGAS", 100);
			hashtable.Add("statSPD", 100);
			restartingMC = false;
			restartingHorse = false;
			restartingEren = false;
			restartingBomb = false;
			ClearCustomObjects();
		}
		PhotonNetwork.player.SetCustomProperties(hashtable);
		resetGameSettings();
		imatitan = new ExitGames.Client.Photon.Hashtable();
		oldScript = string.Empty;
		ignoreList = new List<int>();
		restartCount = new List<float>();
		heroHash = new ExitGames.Client.Photon.Hashtable();
	}

	private IEnumerator respawnE(float seconds)
	{
        while (true)
		{
			yield return new WaitForSeconds(seconds);
			if (isLosing || isWinning)
			{
				continue;
			}
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
				if (photonPlayer.customProperties["RCteam"] == null && RCextensions.returnBoolFromObject(photonPlayer.customProperties["dead"]) && RCextensions.returnIntFromObject(photonPlayer.customProperties["isTitan"]) != 2)
				{
					PView.RPC("respawnHeroInNewRound", photonPlayer);
				}
			}
		}
	}

	[RPC]
	private void respawnHeroInNewRound(PhotonMessageInfo info)
	{
		if (PhotonNetwork.player.customProperties["dead"] != null && RCextensions.returnBoolFromObject(PhotonNetwork.player.customProperties["dead"]) && RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) != 2 && !justSuicide && IN_GAME_MAIN_CAMERA.mainCamera.gameOver)
		{
			needChooseSide = false;
			justSuicide = false;
			SpawnPlayer(myLastHero, myLastRespawnTag);
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
			ShowHUDInfoCenter(string.Empty);
		}
	}

	public IEnumerator restartE(float time)
	{
		yield return new WaitForSeconds(time);
		restartGame2(masterclientSwitched: false);
	}

	public void restartGame2(bool masterclientSwitched)
	{
		if (!gameTimesUp)
		{
			PVPtitanScore = 0;
			PVPhumanScore = 0;
			startRacing = false;
			endRacing = false;
			checkpoint = null;
			timeElapse = 0f;
			roundTime = 0f;
			isWinning = false;
			isLosing = false;
			wave = 1;
			myRespawnTime = 0f;
			kicklist = new ArrayList();
			killInfoGO = new ArrayList();
			racingResult = new ArrayList();
			ShowHUDInfoCenter(string.Empty);
			isRestarting = true;
			DestroyAllExistingCloths();
			PhotonNetwork.DestroyAll();
			feed_number = 0;
			feed_count = 0;
			oneLifeList = new List<PhotonPlayer>();
			ExitGames.Client.Photon.Hashtable hashtable = checkGameGUI();
			PView.RPC("settingRPC", PhotonTargets.Others, hashtable);
			PView.RPC("RPCLoadLevel", PhotonTargets.All);
			setGameSettings(hashtable);
			if (masterclientSwitched)
			{
				sendChatContentInfo("<color=#b5ceff>MasterClient has switched to </color>" + PhotonNetwork.player.uiname.ToRGBA());
			}
		}
	}

	[RPC]
	private void restartGameByClient()
	{
		if (PhotonNetwork.isMasterClient)
		{
			restartGame2(masterclientSwitched: false);
		}
	}

	public void restartGameSingle2()
	{
		startRacing = false;
		endRacing = false;
		checkpoint = null;
		single_kills = 0;
		single_maxDamage = 0;
		single_totalDamage = 0;
		timeElapse = 0f;
		roundTime = 0f;
		timeTotalServer = 0f;
		isWinning = false;
		isLosing = false;
		wave = 1;
		myRespawnTime = 0f;
		ShowHUDInfoCenter(string.Empty);
		DestroyAllExistingCloths();
		feed_number = 0;
		feed_count = 0;
		Application.LoadLevel(Application.loadedLevel);
	}

	public void restartRC()
	{
		intVariables.Clear();
		boolVariables.Clear();
		stringVariables.Clear();
		floatVariables.Clear();
		playerVariables.Clear();
		titanVariables.Clear();
		endGameRC();
	}

	public RCActionHelper returnHelper(string str)
	{
		string[] array = str.Split('.');
		if (float.TryParse(str, out var _))
		{
			array = new string[1] { str };
		}
		List<RCActionHelper> list = new List<RCActionHelper>();
		int sentType = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (list.Count == 0)
			{
				string text = array[i];
				int result2;
				float result3;
				if (text.StartsWith("\"") && text.EndsWith("\""))
				{
					RCActionHelper item = new RCActionHelper(0, 0, text.Substring(1, text.Length - 2));
					list.Add(item);
					sentType = 2;
				}
				else if (int.TryParse(text, out result2))
				{
					RCActionHelper item = new RCActionHelper(0, 0, result2);
					list.Add(item);
					sentType = 0;
				}
				else if (float.TryParse(text, out result3))
				{
					RCActionHelper item = new RCActionHelper(0, 0, result3);
					list.Add(item);
					sentType = 3;
				}
				else if (text.ToLower() == "true" || text.ToLower() == "false")
				{
					RCActionHelper item = new RCActionHelper(0, 0, Convert.ToBoolean(text.ToLower()));
					list.Add(item);
					sentType = 1;
				}
				else if (text.StartsWith("Variable"))
				{
					int num = text.IndexOf('(');
					int num2 = text.LastIndexOf(')');
					if (text.StartsWith("VariableInt"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 0, returnHelper(text));
						list.Add(item);
						sentType = 0;
					}
					else if (text.StartsWith("VariableBool"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 1, returnHelper(text));
						list.Add(item);
						sentType = 1;
					}
					else if (text.StartsWith("VariableString"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 2, returnHelper(text));
						list.Add(item);
						sentType = 2;
					}
					else if (text.StartsWith("VariableFloat"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 3, returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("VariablePlayer"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 4, returnHelper(text));
						list.Add(item);
						sentType = 4;
					}
					else if (text.StartsWith("VariableTitan"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 5, returnHelper(text));
						list.Add(item);
						sentType = 5;
					}
				}
				else if (text.StartsWith("Region"))
				{
					int num = text.IndexOf('(');
					int num2 = text.LastIndexOf(')');
					if (text.StartsWith("RegionRandomX"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 0, returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("RegionRandomY"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 1, returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("RegionRandomZ"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 2, returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
				}
			}
			else
			{
				if (list.Count <= 0)
				{
					continue;
				}
				string text = array[i];
				if (list[list.Count - 1].helperClass == 1)
				{
					switch (list[list.Count - 1].helperType)
					{
					case 4:
						if (text.StartsWith("GetTeam()"))
						{
							RCActionHelper item = new RCActionHelper(2, 1, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetType()"))
						{
							RCActionHelper item = new RCActionHelper(2, 0, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetIsAlive()"))
						{
							RCActionHelper item = new RCActionHelper(2, 2, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("GetTitan()"))
						{
							RCActionHelper item = new RCActionHelper(2, 3, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetKills()"))
						{
							RCActionHelper item = new RCActionHelper(2, 4, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetDeaths()"))
						{
							RCActionHelper item = new RCActionHelper(2, 5, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetMaxDmg()"))
						{
							RCActionHelper item = new RCActionHelper(2, 6, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetTotalDmg()"))
						{
							RCActionHelper item = new RCActionHelper(2, 7, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetCustomInt()"))
						{
							RCActionHelper item = new RCActionHelper(2, 8, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetCustomBool()"))
						{
							RCActionHelper item = new RCActionHelper(2, 9, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("GetCustomString()"))
						{
							RCActionHelper item = new RCActionHelper(2, 10, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetCustomFloat()"))
						{
							RCActionHelper item = new RCActionHelper(2, 11, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionX()"))
						{
							RCActionHelper item = new RCActionHelper(2, 14, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionY()"))
						{
							RCActionHelper item = new RCActionHelper(2, 15, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionZ()"))
						{
							RCActionHelper item = new RCActionHelper(2, 16, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetName()"))
						{
							RCActionHelper item = new RCActionHelper(2, 12, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetGuildName()"))
						{
							RCActionHelper item = new RCActionHelper(2, 13, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetSpeed()"))
						{
							RCActionHelper item = new RCActionHelper(2, 17, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					case 5:
						if (text.StartsWith("GetType()"))
						{
							RCActionHelper item = new RCActionHelper(3, 0, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetSize()"))
						{
							RCActionHelper item = new RCActionHelper(3, 1, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetHealth()"))
						{
							RCActionHelper item = new RCActionHelper(3, 2, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetPositionX()"))
						{
							RCActionHelper item = new RCActionHelper(3, 3, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionY()"))
						{
							RCActionHelper item = new RCActionHelper(3, 4, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionZ()"))
						{
							RCActionHelper item = new RCActionHelper(3, 5, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					default:
						if (text.StartsWith("ConvertToInt()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("ConvertToBool()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("ConvertToString()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("ConvertToFloat()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					}
				}
				else if (text.StartsWith("ConvertToInt()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 0;
				}
				else if (text.StartsWith("ConvertToBool()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 1;
				}
				else if (text.StartsWith("ConvertToString()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 2;
				}
				else if (text.StartsWith("ConvertToFloat()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 3;
				}
			}
		}
		for (int i = list.Count - 1; i > 0; i--)
		{
			list[i - 1].setNextHelper(list[i]);
		}
		return list[0];
	}

	public static PeerStates returnPeerState(int peerstate)
	{
		return peerstate switch
		{
			0 => PeerStates.Authenticated, 
			1 => PeerStates.ConnectedToMaster, 
			2 => PeerStates.DisconnectingFromMasterserver, 
			3 => PeerStates.DisconnectingFromGameserver, 
			4 => PeerStates.DisconnectingFromNameServer, 
			_ => PeerStates.ConnectingToMasterserver, 
		};
	}

	public void sendChatContentInfo(string content)
	{
		object[] parameters = new object[2]
		{
			content,
			string.Empty
		};
		PView.RPC("Chat", PhotonTargets.All, parameters);
	}

	public void sendKillInfo(bool t1, string killer, bool t2, string victim, int dmg)
	{
		object[] parameters = new object[5] { t1, killer, t2, victim, dmg };
		PView.RPC("updateKillInfo", PhotonTargets.All, parameters);
	}

	public static void ServerCloseConnection(PhotonPlayer targetPlayer, bool requestIpBan, string inGameName)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
		raiseEventOptions.TargetActors = new int[1] { targetPlayer.ID };
		RaiseEventOptions options = raiseEventOptions;
		if (requestIpBan)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = true;
			if (inGameName != null && inGameName.Length > 0)
			{
				hashtable[(byte)1] = inGameName;
			}
			PhotonNetwork.RaiseEvent(203, hashtable, sendReliable: true, options);
		}
		else
		{
			PhotonNetwork.RaiseEvent(203, null, sendReliable: true, options);
		}
	}

	public static void ServerRequestAuthentication(string authPassword)
	{
		if (!string.IsNullOrEmpty(authPassword))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = authPassword;
			PhotonNetwork.RaiseEvent(198, hashtable, sendReliable: true, new RaiseEventOptions());
		}
	}

	public static void ServerRequestUnban(string bannedAddress)
	{
		if (!string.IsNullOrEmpty(bannedAddress))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = bannedAddress;
			PhotonNetwork.RaiseEvent(199, hashtable, sendReliable: true, new RaiseEventOptions());
		}
	}

	private void setGameSettings(ExitGames.Client.Photon.Hashtable hash)
	{
		restartingEren = false;
		restartingBomb = false;
		restartingHorse = false;
		if (hash.ContainsKey("bomb"))
		{
			if (GameSettings.bombMode != (int)hash["bomb"])
			{
				GameSettings.bombMode = (int)hash["bomb"];
				InRoomChat.Write("<color=#FFCC00>PVP Bomb Mode enabled.</color>");
			}
		}
		else if (GameSettings.bombMode != 0)
		{
			GameSettings.bombMode = 0;
			cyanWins = 0;
			magentaWins = 0;
			InRoomChat.Write("<color=#FFCC00>PVP Bomb Mode disabled.</color>");
			if (PhotonNetwork.isMasterClient)
			{
				restartingBomb = true;
			}
		}
		if (hash.ContainsKey("globalDisableMinimap"))
		{
			if (GameSettings.globalDisableMinimap != (int)hash["globalDisableMinimap"])
			{
				GameSettings.globalDisableMinimap = (int)hash["globalDisableMinimap"];
				InRoomChat.Write("<color=#FFCC00>Minimaps are not allowed.</color>");
				if ((int)hash["globalDisableMinimap"] == 0)
				{
					CacheGameObject.Find("LabelInfoTopRight").transform.localPosition = new Vector3((int)((float)Screen.width * 0.35f), (int)((float)Screen.height * 0.5f), 0f);
				}
			}
		}
		else if (GameSettings.globalDisableMinimap != 0)
		{
			GameSettings.globalDisableMinimap = 0;
			InRoomChat.Write("<color=#FFCC00>Minimaps are allowed.</color>");
		}
		if (hash.ContainsKey("horse"))
		{
			if (GameSettings.horseMode != (int)hash["horse"])
			{
				GameSettings.horseMode = (int)hash["horse"];
				InRoomChat.Write("<color=#FFCC00>Horses enabled.</color>");
			}
		}
		else if (GameSettings.horseMode != 0)
		{
			GameSettings.horseMode = 0;
			InRoomChat.Write("<color=#FFCC00>Horses disabled.</color>");
			if (PhotonNetwork.isMasterClient)
			{
				restartingHorse = true;
			}
		}
		if (hash.ContainsKey("punkWaves"))
		{
			if (GameSettings.punkWaves != (int)hash["punkWaves"])
			{
				GameSettings.punkWaves = (int)hash["punkWaves"];
				InRoomChat.Write("<color=#FFCC00>Punk override every 5 waves enabled.</color>");
			}
		}
		else if (GameSettings.punkWaves != 0)
		{
			GameSettings.punkWaves = 0;
			InRoomChat.Write("<color=#FFCC00>Punk override every 5 waves disabled.</color>");
		}
		if (hash.ContainsKey("ahssReload"))
		{
			if (GameSettings.ahssReload != (int)hash["ahssReload"])
			{
				GameSettings.ahssReload = (int)hash["ahssReload"];
				InRoomChat.Write("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
			}
		}
		else if (GameSettings.ahssReload != 0)
		{
			GameSettings.ahssReload = 0;
			InRoomChat.Write("<color=#FFCC00>AHSS Air-Reload allowed.</color>");
		}
		if (hash.ContainsKey("team"))
		{
			if (GameSettings.teamMode != (int)hash["team"])
			{
				GameSettings.teamMode = (int)hash["team"];
				string text = string.Empty;
				if (GameSettings.teamMode == 1)
				{
					text = "no sort";
				}
				InRoomChat.Write("<color=#FFCC00>Team Mode enabled (" + text + ").</color>");
				if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]) == 0)
				{
					setTeam(3);
				}
			}
		}
		else if (GameSettings.teamMode != 0)
		{
			GameSettings.teamMode = 0;
			setTeam(0);
			cyanWins = 0;
			magentaWins = 0;
			InRoomChat.Write("<color=#FFCC00>Team mode disabled.</color>");
		}
		if (hash.ContainsKey("point"))
		{
			if (GameSettings.pointMode != (int)hash["point"])
			{
				GameSettings.pointMode = (int)hash["point"];
				InRoomChat.Write("<color=#FFCC00>Point limit enabled (" + Convert.ToString(GameSettings.pointMode) + ").</color>");
			}
		}
		else if (GameSettings.pointMode != 0)
		{
			GameSettings.pointMode = 0;
			InRoomChat.Write("<color=#FFCC00>Point limit disabled.</color>");
		}
		if (hash.ContainsKey("rock"))
		{
			if (GameSettings.disableRock != (int)hash["rock"])
			{
				GameSettings.disableRock = (int)hash["rock"];
				InRoomChat.Write("<color=#FFCC00>Punk rock throwing disabled.</color>");
			}
		}
		else if (GameSettings.disableRock != 0)
		{
			GameSettings.disableRock = 0;
			InRoomChat.Write("<color=#FFCC00>Punk rock throwing enabled.</color>");
		}
		if (hash.ContainsKey("explode"))
		{
			if (GameSettings.explodeMode != (int)hash["explode"])
			{
				GameSettings.explodeMode = (int)hash["explode"];
				InRoomChat.Write("<color=#FFCC00>Titan Explode Mode enabled (Radius " + Convert.ToString(GameSettings.explodeMode) + ").</color>");
			}
		}
		else if (GameSettings.explodeMode != 0)
		{
			GameSettings.explodeMode = 0;
			InRoomChat.Write("<color=#FFCC00>Titan Explode Mode disabled.</color>");
		}
		if (hash.ContainsKey("healthMode") && hash.ContainsKey("healthLower") && hash.ContainsKey("healthUpper"))
		{
			if (GameSettings.healthMode != (int)hash["healthMode"] || GameSettings.healthLower != (int)hash["healthLower"] || GameSettings.healthUpper != (int)hash["healthUpper"])
			{
				GameSettings.healthMode = (int)hash["healthMode"];
				GameSettings.healthLower = (int)hash["healthLower"];
				GameSettings.healthUpper = (int)hash["healthUpper"];
				string text = "Static";
				if (GameSettings.healthMode == 2)
				{
					text = "Scaled";
				}
				InRoomChat.Write("<color=#FFCC00>Titan Health (" + text + ", " + GameSettings.healthLower + " to " + GameSettings.healthUpper + ") enabled.</color>");
			}
		}
		else if (GameSettings.healthMode != 0 || GameSettings.healthLower != 0 || GameSettings.healthUpper != 0)
		{
			GameSettings.healthMode = 0;
			GameSettings.healthLower = 0;
			GameSettings.healthUpper = 0;
			InRoomChat.Write("<color=#FFCC00>Titan Health disabled.</color>");
		}
		if (hash.ContainsKey("eren"))
		{
			if (GameSettings.banEren != (int)hash["eren"])
			{
				GameSettings.banEren = (int)hash["eren"];
				InRoomChat.Write("<color=#FFCC00>Anti-Eren enabled. Using eren transform will get you kicked.</color>");
				if (PhotonNetwork.isMasterClient)
				{
					restartingEren = true;
				}
			}
		}
		else if (GameSettings.banEren != 0)
		{
			GameSettings.banEren = 0;
			InRoomChat.Write("<color=#FFCC00>Anti-Eren disabled. Eren transform is allowed.</color>");
		}
		if (hash.ContainsKey("titanc"))
		{
			if (GameSettings.moreTitans != (int)hash["titanc"])
			{
				GameSettings.moreTitans = (int)hash["titanc"];
				InRoomChat.Write("<color=#FFCC00>" + Convert.ToString(GameSettings.moreTitans) + " titans will spawn each round.</color>");
			}
		}
		else if (GameSettings.moreTitans != 0)
		{
			GameSettings.moreTitans = 0;
			InRoomChat.Write("<color=#FFCC00>Default titans will spawn each round.</color>");
		}
		if (hash.ContainsKey("damage"))
		{
			if (GameSettings.damageMode != (int)hash["damage"])
			{
				GameSettings.damageMode = (int)hash["damage"];
				InRoomChat.Write("<color=#FFCC00>Nape minimum damage (" + Convert.ToString(GameSettings.damageMode) + ") enabled.</color>");
			}
		}
		else if (GameSettings.damageMode != 0)
		{
			GameSettings.damageMode = 0;
			InRoomChat.Write("<color=#FFCC00>Nape minimum damage disabled.</color>");
		}
		if (hash.ContainsKey("sizeMode") && hash.ContainsKey("sizeLower") && hash.ContainsKey("sizeUpper"))
		{
			if (GameSettings.sizeMode != (int)hash["sizeMode"] || GameSettings.sizeLower != (float)hash["sizeLower"] || GameSettings.sizeUpper != (float)hash["sizeUpper"])
			{
				GameSettings.sizeMode = (int)hash["sizeMode"];
				GameSettings.sizeLower = (float)hash["sizeLower"];
				GameSettings.sizeUpper = (float)hash["sizeUpper"];
				InRoomChat.Write("<color=#FFCC00>Custom titan size (" + GameSettings.sizeLower.ToString("F2") + "," + GameSettings.sizeUpper.ToString("F2") + ") enabled.</color>");
			}
		}
		else if (GameSettings.sizeMode != 0 || GameSettings.sizeLower != 0f || GameSettings.sizeUpper != 0f)
		{
			GameSettings.sizeMode = 0;
			GameSettings.sizeLower = 0f;
			GameSettings.sizeUpper = 0f;
			InRoomChat.Write("<color=#FFCC00>Custom titan size disabled.</color>");
		}
		if (hash.ContainsKey("spawnMode") && hash.ContainsKey("nRate") && hash.ContainsKey("aRate") && hash.ContainsKey("jRate") && hash.ContainsKey("cRate") && hash.ContainsKey("pRate"))
		{
			if (GameSettings.spawnMode != (int)hash["spawnMode"] || GameSettings.nRate != (float)hash["nRate"] || GameSettings.aRate != (float)hash["aRate"] || GameSettings.jRate != (float)hash["jRate"] || GameSettings.cRate != (float)hash["cRate"] || GameSettings.pRate != (float)hash["pRate"])
			{
				GameSettings.spawnMode = (int)hash["spawnMode"];
				GameSettings.nRate = (float)hash["nRate"];
				GameSettings.aRate = (float)hash["aRate"];
				GameSettings.jRate = (float)hash["jRate"];
				GameSettings.cRate = (float)hash["cRate"];
				GameSettings.pRate = (float)hash["pRate"];
				InRoomChat.Write("<color=#FFCC00>Custom spawn rate enabled (" + GameSettings.nRate.ToString("F2") + "% Normal, " + GameSettings.aRate.ToString("F2") + "% Abnormal, " + GameSettings.jRate.ToString("F2") + "% Jumper, " + GameSettings.cRate.ToString("F2") + "% Crawler, " + GameSettings.pRate.ToString("F2") + "% Punk </color>");
			}
		}
		else if (GameSettings.spawnMode != 0 || GameSettings.nRate != 0f || GameSettings.aRate != 0f || GameSettings.jRate != 0f || GameSettings.cRate != 0f || GameSettings.pRate != 0f)
		{
			GameSettings.spawnMode = 0;
			GameSettings.nRate = 0f;
			GameSettings.aRate = 0f;
			GameSettings.jRate = 0f;
			GameSettings.cRate = 0f;
			GameSettings.pRate = 0f;
			InRoomChat.Write("<color=#FFCC00>Custom spawn rate disabled.</color>");
		}
		if (hash.ContainsKey("waveModeOn") && hash.ContainsKey("waveModeNum"))
		{
			if (GameSettings.waveModeOn != (int)hash["waveModeOn"] || GameSettings.waveModeNum != (int)hash["waveModeNum"])
			{
				GameSettings.waveModeOn = (int)hash["waveModeOn"];
				GameSettings.waveModeNum = (int)hash["waveModeNum"];
				InRoomChat.Write("<color=#FFCC00>Custom wave mode (" + GameSettings.waveModeNum + ") enabled.</color>");
			}
		}
		else if (GameSettings.waveModeOn != 0 || GameSettings.waveModeNum != 0)
		{
			GameSettings.waveModeOn = 0;
			GameSettings.waveModeNum = 0;
			InRoomChat.Write("<color=#FFCC00>Custom wave mode disabled.</color>");
		}
		if (hash.ContainsKey("friendly"))
		{
			if (GameSettings.friendlyMode != (int)hash["friendly"])
			{
				GameSettings.friendlyMode = (int)hash["friendly"];
				InRoomChat.Write("<color=#FFCC00>PVP is prohibited.</color>");
			}
		}
		else if (GameSettings.friendlyMode != 0)
		{
			GameSettings.friendlyMode = 0;
			InRoomChat.Write("<color=#FFCC00>PVP is allowed.</color>");
		}
		if (hash.ContainsKey("pvp"))
		{
			if (GameSettings.pvpMode != (int)hash["pvp"])
			{
				GameSettings.pvpMode = (int)hash["pvp"];
				string text = string.Empty;
				if (GameSettings.pvpMode == 1)
				{
					text = "Team-Based";
				}
				else if (GameSettings.pvpMode == 2)
				{
					text = "FFA";
				}
				InRoomChat.Write("<color=#FFCC00>Blade/AHSS PVP enabled (" + text + ").</color>");
			}
		}
		else if (GameSettings.pvpMode != 0)
		{
			GameSettings.pvpMode = 0;
			InRoomChat.Write("<color=#FFCC00>Blade/AHSS PVP disabled.</color>");
		}
		if (hash.ContainsKey("maxwave"))
		{
			if (GameSettings.maxWave != (int)hash["maxwave"])
			{
				GameSettings.maxWave = (int)hash["maxwave"];
				InRoomChat.Write("<color=#FFCC00>Max wave is " + GameSettings.maxWave + ".</color>");
			}
		}
		else if (GameSettings.maxWave != 0)
		{
			GameSettings.maxWave = 0;
			InRoomChat.Write("<color=#FFCC00>Max wave set to default.</color>");
		}
		if (hash.ContainsKey("endless"))
		{
			if (GameSettings.endlessMode != (int)hash["endless"])
			{
				GameSettings.endlessMode = (int)hash["endless"];
				InRoomChat.Write("<color=#FFCC00>Endless respawn enabled (" + GameSettings.endlessMode + " seconds).</color>");
			}
		}
		else if (GameSettings.endlessMode != 0)
		{
			GameSettings.endlessMode = 0;
			InRoomChat.Write("<color=#FFCC00>Endless respawn disabled.</color>");
		}
		if (hash.ContainsKey("motd"))
		{
			if (GameSettings.motd != (string)hash["motd"])
			{
				GameSettings.motd = (string)hash["motd"];
				InRoomChat.Write("<color=#FFCC00>MOTD:" + GameSettings.motd + "</color>");
			}
		}
		else if (GameSettings.motd != string.Empty)
		{
			GameSettings.motd = string.Empty;
		}
		if (hash.ContainsKey("deadlycannons"))
		{
			if (GameSettings.deadlyCannons != (int)hash["deadlycannons"])
			{
				GameSettings.deadlyCannons = (int)hash["deadlycannons"];
				InRoomChat.Write("<color=#FFCC00>Cannons will now kill players.</color>");
			}
		}
		else if (GameSettings.deadlyCannons != 0)
		{
			GameSettings.deadlyCannons = 0;
			InRoomChat.Write("<color=#FFCC00>Cannons will no longer kill players.</color>");
		}
		if (hash.ContainsKey("asoracing"))
		{
			if (GameSettings.racingStatic != (int)hash["asoracing"])
			{
				GameSettings.racingStatic = (int)hash["asoracing"];
				InRoomChat.Write("<color=#FFCC00>Racing will not restart on win.</color>");
			}
		}
		else if (GameSettings.racingStatic != 0)
		{
			GameSettings.racingStatic = 0;
			InRoomChat.Write("<color=#FFCC00>Racing will restart on win.</color>");
		}
		if (hash.ContainsKey("racingStartTime"))
		{
			race_timer = hash["racingStartTime"].ToString();
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			race_timer = "20";
		}
		if (hash.ContainsKey("DrunkTitans"))
		{
			if (GameSettings.DrunkTitans != (int)hash["DrunkTitans"])
			{
				GameSettings.DrunkTitans = (int)hash["DrunkTitans"];
				InRoomChat.Write("<color=#FFCC00>Drunk Titans enabled.</color>");
			}
		}
		else if (GameSettings.DrunkTitans != 0)
		{
			GameSettings.DrunkTitans = 0;
			InRoomChat.Write("<color=#FFCC00>Drunk Titans disabled.</color>");
		}
	}

	[RPC]
	private void setMasterRC(PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			masterRC = true;
		}
		else if (!info.sender.isMasterClient && !isPlayerTTL)
		{
			ThrowJusticeDefensive(info.sender, "invalid setMasterRC");
		}
	}

	private void setTeam(int setting)
	{
		switch (setting)
		{
		case 0:
		{
			name = LoginFengKAI.player.name;
			ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
			hashtable3.Add("RCteam", 0);
			hashtable3.Add("name", name);
			PhotonNetwork.player.SetCustomProperties(hashtable3);
			break;
		}
		case 1:
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("RCteam", 1);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			break;
		}
		case 2:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2.Add("RCteam", 2);
			PhotonNetwork.player.SetCustomProperties(hashtable2);
			break;
		}
		case 3:
		{
			int num = 0;
			int num2 = 0;
			int team = 1;
			PhotonPlayer[] array = PhotonNetwork.playerList;
			for (int i = 0; i < array.Length; i++)
			{
				switch (RCextensions.returnIntFromObject(array[i].customProperties["RCteam"]))
				{
				case 1:
					num++;
					break;
				case 2:
					num2++;
					break;
				}
			}
			if (num > num2)
			{
				team = 2;
			}
			setTeam(team);
			break;
		}
		}
		if (setting != 0 && setting != 1 && setting != 2)
		{
			return;
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject go in array2)
		{
			if (go.GetPhotonView().isMine)
			{
				PView.RPC("labelRPC", PhotonTargets.All, go.GetPhotonView().viewID);
			}
		}
	}

	[RPC]
	private void setTeamRPC(int setting, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient || info.sender.isLocal)
		{
			setTeam(setting);
		}
	}

	[RPC]
	private void settingRPC(ExitGames.Client.Photon.Hashtable hash, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			setGameSettings(hash);
		}
		else if (!info.sender.isMasterClient && !isPlayerTTL)
		{
			ThrowJusticeDefensive(info.sender, "invalid settingRPC");
		}
	}

	[RPC]
	private void showChatContent(string content)
	{
		chatContent.Add(content);
		if (chatContent.Count > 10)
		{
			chatContent.RemoveAt(0);
		}
		CacheGameObject.Find("LabelChatContent").GetComponent<UILabel>().text = string.Empty;
		for (int i = 0; i < chatContent.Count; i++)
		{
			CacheGameObject.Find("LabelChatContent").GetComponent<UILabel>().text += chatContent[i];
		}
	}

	public void ShowHUDInfoCenter(string content)
	{
		content = "[" + alternativefinalhex() + "]" + content;
		GameObject gameObject = CacheGameObject.Find("LabelInfoCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	public void ShowHUDInfoCenterADD(string content)
	{
		GameObject gameObject = CacheGameObject.Find("LabelInfoCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text += content;
		}
	}

	public void ShowHUDInfoTopCenter(string content)
	{
		content = "[" + alternativefinalhex() + "]" + content;
		GameObject gameObject = CacheGameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void ShowHUDInfoTopCenterADD(string content)
	{
		GameObject gameObject = CacheGameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text += content;
		}
	}

	private void ShowHUDInfoTopLeft(string content)
	{
		content = "[" + alternativefinalhex() + "]" + content;
		GameObject gameObject = CacheGameObject.Find("LabelInfoTopLeft");
		if (gameObject != null)
		{
			if (GameSettings.OldPlayerlist == 1)
			{
				gameObject.GetComponent<UILabel>().text = content;
			}
			else
			{
				gameObject.GetComponent<UILabel>().text = "(" + (string)customkey[InputCodeRC.secondplayerlist + 1] + ")";
			}
		}
	}

	private void ShowHUDInfoTopRight(string content)
	{
		content = "[" + alternativefinalhex() + "]" + content;
		if (LabelInfoTopRight == null)
		{
			UILabel uILabel = CacheGameObject.Find<UILabel>("LabelInfoTopRight");
			if (uILabel != null)
			{
				LabelInfoTopRight = uILabel;
			}
		}
		if (LabelInfoTopRight != null)
		{
			LabelInfoTopRight.text = content;
		}
	}

	private void ShowHUDInfoTopRightMAPNAME(string content)
	{
		content = "[" + alternativefinalhex() + "]" + content;
		if (LabelInfoTopRight == null)
		{
			UILabel uILabel = CacheGameObject.Find<UILabel>("LabelInfoTopRight");
			if (uILabel != null)
			{
				LabelInfoTopRight = uILabel;
			}
		}
		if (LabelInfoTopRight != null)
		{
			LabelInfoTopRight.text += content;
		}
	}

	[RPC]
	private void showResult(string text0, string text1, string text2, string text3, string text4, string text6, PhotonMessageInfo t)
	{
		if (!t.sender.isMasterClient && t.sender != null && !t.sender.isLocal)
		{
			ThrowJusticeDefensive(t.sender, "invalid time finish");
		}
		else if (((gameTimesUp && t.sender.isMasterClient) || t.sender.isMasterClient) && GameSettings.BotTimeFinish > 0)
		{
			//AudioSource.PlayClipAtPoint(BotTimeFinish, Camera.main.transform.position, AssistantVolume);
		}
	}

	private void SingleShowHUDInfoTopCenter(string content)
	{
		GameObject gameObject = CacheGameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void SingleShowHUDInfoTopLeft(string content)
	{
		GameObject gameObject = CacheGameObject.Find("LabelInfoTopLeft");
		if (gameObject != null)
		{
			content = content.Replace("[0]", "[*^_^*]");
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	[RPC]
	public void someOneIsDead(int id = -1)
	{
		if (!PhotonNetwork.isMasterClient)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			if (id != 0)
			{
				PVPtitanScore += 2;
			}
			checkPVPpts();
			object[] parameters = new object[2] { PVPhumanScore, PVPtitanScore };
			PView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
		{
			titanScore++;
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.TROST)
		{
			if (isPlayerAllDead2())
			{
				gameLose2();
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS && GameSettings.pvpMode == 0 && GameSettings.bombMode == 0)
		{
			if (isPlayerAllDead2())
			{
				gameLose2();
				teamWinner = 0;
			}
			if (isTeamAllDead2(1))
			{
				teamWinner = 2;
				gameWin2();
			}
			if (isTeamAllDead2(2))
			{
				teamWinner = 1;
				gameWin2();
			}
		}
	}

	public void SpawnNonAITitan2(string id, string tag = "titanRespawn")
	{
		if (logicLoaded && customLevelLoaded)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
			GameObject gameObject = array[UnityEngine.Random.Range(0, array.Length)];
			Vector3 position = gameObject.transform.position;
			if (level.StartsWith("Custom") && titanSpawns.Count > 0)
			{
				position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
			}
			myLastHero = id.ToUpper();
			GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE) ? PhotonNetwork.Instantiate("TITAN_VER3.1", position, gameObject.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20)), checkpoint.transform.rotation, 0));
			IN_GAME_MAIN_CAMERA.mainCamera.setMainObjectASTITAN(gameObject2);
			gameObject2.GetComponent<TITAN>().nonAI = true;
			gameObject2.GetComponent<TITAN>().speed = 30f;
			gameObject2.GetComponent<TITAN_CONTROLLER>().enabled = true;
			if (id == "RANDOM" && UnityEngine.Random.Range(0, 100) < 7)
			{
				gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
			}
			IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
			IN_GAME_MAIN_CAMERA.spectate.disable = true;
			IN_GAME_MAIN_CAMERA.mouselook.disable = true;
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("dead", false);
			hashtable.Add("isTitan", 2);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
			{
				Screen.lockCursor = true;
			}
			else
			{
				Screen.lockCursor = false;
			}
			Screen.showCursor = true;
			ShowHUDInfoCenter(string.Empty);
		}
		else
		{
			NOTSpawnNonAITitanRC(id);
		}
	}

	public void SpawnPlayer(string id, string tag = "playerRespawn")
	{
        if (instance.InsideSpecmode)
		{
			FleurConsole.instance.Log("Can't spawn in specmode!", FleurConsole.DebugType.LOG);
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			SpawnPlayerAt2(id, checkpoint);
			return;
		}
		myLastRespawnTag = tag;
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		GameObject pos = array[UnityEngine.Random.Range(0, array.Length)];
		SpawnPlayerAt2(id, pos);
	}

	public void SpawnPlayerAt2(string id, GameObject pos)
	{
        if (instance.InsideSpecmode)
		{
			FleurConsole.instance.Log("Can't spawn in specmode!", FleurConsole.DebugType.LOG);
			return;
		}
		if (!logicLoaded || !customLevelLoaded)
		{
			NOTSpawnPlayerRC(id);
		}
		else
		{
			Vector3 position = pos.transform.position;
			if (racingSpawnPointSet)
			{
				position = racingSpawnPoint;
			}
			else if (level.StartsWith("Custom"))
			{
				if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]) == 0)
				{
					List<Vector3> list = new List<Vector3>();
					foreach (Vector3 item3 in playerSpawnsC)
					{
						list.Add(item3);
					}
					foreach (Vector3 item4 in playerSpawnsM)
					{
						list.Add(item4);
					}
					if (list.Count > 0)
					{
						position = list[UnityEngine.Random.Range(0, list.Count)];
					}
				}
				else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]) == 1)
				{
					if (playerSpawnsC.Count > 0)
					{
						position = playerSpawnsC[UnityEngine.Random.Range(0, playerSpawnsC.Count)];
					}
				}
				else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]) == 2 && playerSpawnsM.Count > 0)
				{
					position = playerSpawnsM[UnityEngine.Random.Range(0, playerSpawnsM.Count)];
				}
			}
			myLastHero = id.ToUpper();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (IN_GAME_MAIN_CAMERA.singleCharacter == "TITAN_EREN")
				{
					IN_GAME_MAIN_CAMERA.mainCamera.setMainObject((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("TITAN_EREN"), pos.transform.position, pos.transform.rotation));
				}
				else
				{
					HERO component = IN_GAME_MAIN_CAMERA.mainCamera.setMainObject((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("AOTTG_HERO 1"), pos.transform.position, pos.transform.rotation)).GetComponent<HERO>();
					HERO_SETUP component2 = component.GetComponent<HERO_SETUP>();
					if (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 1" || IN_GAME_MAIN_CAMERA.singleCharacter == "SET 2" || IN_GAME_MAIN_CAMERA.singleCharacter == "SET 3")
					{
						HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(IN_GAME_MAIN_CAMERA.singleCharacter);
						heroCostume.checkstat();
						CostumeConeveter.HeroCostumeToLocalData(heroCostume, IN_GAME_MAIN_CAMERA.singleCharacter);
						component2.init();
						if (heroCostume != null)
						{
							component2.myCostume = heroCostume;
							component2.myCostume.stat = heroCostume.stat;
						}
						else
						{
							heroCostume = (component2.myCostume = HeroCostume.costumeOption[3]);
							component2.myCostume.stat = HeroStat.getInfo(heroCostume.name.ToUpper());
						}
						component2.setCharacterComponent();
						component.setStat2();
						component.setSkillHUDPosition2();
					}
					else
					{
						for (int i = 0; i < HeroCostume.costume.Length; i++)
						{
							if (HeroCostume.costume[i].name.ToUpper() == IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper())
							{
								int num = HeroCostume.costume[i].id + costumeSet - 1;
								if (HeroCostume.costume[num].name != HeroCostume.costume[i].name)
								{
									num = HeroCostume.costume[i].id + 1;
								}
								component2.init();
								component2.myCostume = HeroCostume.costume[num];
								component2.myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num].name.ToUpper());
								component2.setCharacterComponent();
								component.setStat2();
								component.setSkillHUDPosition2();
								break;
							}
						}
					}
				}
			}
			else
			{
				HERO component3 = IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, pos.transform.rotation, 0)).GetComponent<HERO>();
				HERO_SETUP component4 = component3.GetComponent<HERO_SETUP>();
				id = id.ToUpper();
				switch (id)
				{
				case "SET 1":
				case "SET 2":
				case "SET 3":
				{
					HeroCostume heroCostume2 = CostumeConeveter.LocalDataToHeroCostume(id);
					heroCostume2.checkstat();
					CostumeConeveter.HeroCostumeToLocalData(heroCostume2, id);
					component4.init();
					if (heroCostume2 != null)
					{
						component4.myCostume = heroCostume2;
						component4.myCostume.stat = heroCostume2.stat;
					}
					else
					{
						heroCostume2 = (component4.myCostume = HeroCostume.costumeOption[3]);
						component4.myCostume.stat = HeroStat.getInfo(heroCostume2.name.ToUpper());
					}
					component4.setCharacterComponent();
					component3.setStat2();
					component3.setSkillHUDPosition2();
					break;
				}
				default:
				{
					for (int j = 0; j < HeroCostume.costume.Length; j++)
					{
						if (HeroCostume.costume[j].name.ToUpper() == id.ToUpper())
						{
							int num2 = HeroCostume.costume[j].id;
							if (id.ToUpper() != "AHSS")
							{
								num2 += costumeSet - 1;
							}
							if (HeroCostume.costume[num2].name != HeroCostume.costume[j].name)
							{
								num2 = HeroCostume.costume[j].id + 1;
							}
							component4.init();
							component4.myCostume = HeroCostume.costume[num2];
							component4.myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num2].name.ToUpper());
							component4.setCharacterComponent();
							component3.setStat2();
							component3.setSkillHUDPosition2();
							break;
						}
					}
					break;
				}
				}
				CostumeConeveter.HeroCostumeToPhotonData2(component4.myCostume, PhotonNetwork.player);
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
				{
					IN_GAME_MAIN_CAMERA.main_objectT.position += new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20));
				}
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add("dead", false);
				hashtable.Add("isTitan", 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
			IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
			IN_GAME_MAIN_CAMERA.mainCamera.setHUDposition();
			IN_GAME_MAIN_CAMERA.spectate.disable = true;
			IN_GAME_MAIN_CAMERA.mouselook.disable = true;
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
			if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
			{
				Screen.lockCursor = true;
			}
			else
			{
				Screen.lockCursor = false;
			}
			Screen.showCursor = false;
			isLosing = false;
			ShowHUDInfoCenter(string.Empty);
			if (GameSettings.Drunk == 1)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
				foreach (GameObject gameObject in array)
				{
					if (gameObject.GetPhotonView().isMine)
					{
						gameObject.rigidbody.freezeRotation = false;
						gameObject.rigidbody.useGravity = true;
					}
				}
			}
		}
		if (GameSettings.BottomHUD == 0)
		{
			instance.BottomHUD(enabled: false);
		}
	}

	[RPC]
	public void spawnPlayerAtRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient || !logicLoaded || !customLevelLoaded || justSuicide || !IN_GAME_MAIN_CAMERA.mainCamera.gameOver)
		{
			return;
		}
		Vector3 position = new Vector3(posX, posY, posZ);
		HERO component = IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, new Quaternion(0f, 0f, 0f, 1f), 0)).GetComponent<HERO>();
		HERO_SETUP component2 = component.GetComponent<HERO_SETUP>();
		string text = myLastHero.ToUpper();
		switch (text)
		{
		case "SET 1":
		case "SET 2":
		case "SET 3":
		{
			HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(text);
			heroCostume.checkstat();
			CostumeConeveter.HeroCostumeToLocalData(heroCostume, text);
			component2.init();
			if (heroCostume != null)
			{
				component2.myCostume = heroCostume;
				component2.myCostume.stat = heroCostume.stat;
			}
			else
			{
				heroCostume = (component2.myCostume = HeroCostume.costumeOption[3]);
				component2.myCostume.stat = HeroStat.getInfo(heroCostume.name.ToUpper());
			}
			component2.setCharacterComponent();
			component.setStat2();
			component.setSkillHUDPosition2();
			break;
		}
		default:
		{
			for (int i = 0; i < HeroCostume.costume.Length; i++)
			{
				if (HeroCostume.costume[i].name.ToUpper() == text.ToUpper())
				{
					int num = HeroCostume.costume[i].id;
					if (text.ToUpper() != "AHSS")
					{
						num += costumeSet - 1;
					}
					if (HeroCostume.costume[num].name != HeroCostume.costume[i].name)
					{
						num = HeroCostume.costume[i].id + 1;
					}
					component2.init();
					component2.myCostume = HeroCostume.costume[num];
					component2.myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num].name.ToUpper());
					component2.setCharacterComponent();
					component.setStat2();
					component.setSkillHUDPosition2();
					break;
				}
			}
			break;
		}
		}
		CostumeConeveter.HeroCostumeToPhotonData2(component2.myCostume, PhotonNetwork.player);
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			IN_GAME_MAIN_CAMERA.main_objectT.position += new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20));
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("dead", false);
		hashtable.Add("isTitan", 1);
		PhotonNetwork.player.SetCustomProperties(hashtable);
		IN_GAME_MAIN_CAMERA.mainCamera.enabled = true;
		IN_GAME_MAIN_CAMERA.mainCamera.setHUDposition();
		IN_GAME_MAIN_CAMERA.spectate.disable = true;
		IN_GAME_MAIN_CAMERA.mouselook.disable = true;
		IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
		if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS || IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.NewTPS)
		{
			Screen.lockCursor = true;
		}
		else
		{
			Screen.lockCursor = false;
		}
		Screen.showCursor = false;
		isLosing = false;
		ShowHUDInfoCenter(string.Empty);
	}

	private void spawnPlayerCustomMap()
	{
		if (instance.InsideSpecmode)
		{
			FleurConsole.instance.Log("Can't spawn in specmode!", FleurConsole.DebugType.LOG);
		}
		else if (!justSuicide && IN_GAME_MAIN_CAMERA.mainCamera.gameOver)
		{
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = false;
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) == 2)
			{
				SpawnNonAITitan2(myLastHero);
			}
			else
			{
				SpawnPlayer(myLastHero, myLastRespawnTag);
			}
			ShowHUDInfoCenter(string.Empty);
		}
	}

	public GameObject spawnTitan(int rate, Vector3 position, Quaternion rotation, bool punk)
	{
		GameObject gameObject = spawnTitanRaw(position, rotation);
		if (punk)
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
		}
		else if (UnityEngine.Random.Range(0, 100) < rate)
		{
			if (IN_GAME_MAIN_CAMERA.difficulty == 2)
			{
				if (UnityEngine.Random.Range(0f, 1f) < 0.7f || LevelInfo.getInfo(level).noCrawler)
				{
					gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				}
				else
				{
					gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
				}
			}
		}
		else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
		{
			if (UnityEngine.Random.Range(0f, 1f) < 0.7f || LevelInfo.getInfo(level).noCrawler)
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
			}
			else
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
			}
		}
		else if (UnityEngine.Random.Range(0, 100) < rate)
		{
			if (UnityEngine.Random.Range(0f, 1f) < 0.8f || LevelInfo.getInfo(level).noCrawler)
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
			}
			else
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
			}
		}
		else if (UnityEngine.Random.Range(0f, 1f) < 0.8f || LevelInfo.getInfo(level).noCrawler)
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
		}
		else
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
		}
		GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gametype != 0) ? PhotonNetwork.Instantiate("FX/FXtitanSpawn", gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0) : ((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("FX/FXtitanSpawn"), gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f))));
		gameObject2.transform.localScale = gameObject.transform.localScale;
		return gameObject;
	}

	public void spawnTitanAction(int type, float size, int health, int number)
	{
		Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
		Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
		if (titanSpawns.Count > 0)
		{
			position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
		}
		else
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
			if (array.Length != 0)
			{
				int num = UnityEngine.Random.Range(0, array.Length);
				GameObject gameObject = array[num];
				while (array[num] == null)
				{
					num = UnityEngine.Random.Range(0, array.Length);
					gameObject = array[num];
				}
				array[num] = null;
				position = gameObject.transform.position;
				rotation = gameObject.transform.rotation;
			}
		}
		for (int i = 0; i < number; i++)
		{
			GameObject gameObject2 = spawnTitanRaw(position, rotation);
			gameObject2.GetComponent<TITAN>().resetLevel(size);
			gameObject2.GetComponent<TITAN>().hasSetLevel = true;
			if ((float)health > 0f)
			{
				gameObject2.GetComponent<TITAN>().currentHealth = health;
				gameObject2.GetComponent<TITAN>().maxHealth = health;
			}
			switch (type)
			{
			case 0:
				gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
				break;
			case 1:
				gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
				break;
			case 2:
				gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				break;
			case 3:
				gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
				break;
			case 4:
				gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
				break;
			}
		}
	}

	public void spawnTitanAtAction(int type, float size, int health, int number, float posX, float posY, float posZ)
	{
		Vector3 position = new Vector3(posX, posY, posZ);
		Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
		for (int i = 0; i < number; i++)
		{
			GameObject gameObject = spawnTitanRaw(position, rotation);
			gameObject.GetComponent<TITAN>().resetLevel(size);
			gameObject.GetComponent<TITAN>().hasSetLevel = true;
			if ((float)health > 0f)
			{
				gameObject.GetComponent<TITAN>().currentHealth = health;
				gameObject.GetComponent<TITAN>().maxHealth = health;
			}
			switch (type)
			{
			case 0:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
				break;
			case 1:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
				break;
			case 2:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				break;
			case 3:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
				break;
			case 4:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
				break;
			}
		}
	}

	public void spawnTitanCustom(string type, int abnormal, int rate, bool punk)
	{
		int num = rate;
		if (level.StartsWith("Custom"))
		{
			num = 5;
			if (GameSettings.gameType == 1)
			{
				num = 3;
			}
			else if (GameSettings.gameType == 2 || GameSettings.gameType == 3)
			{
				num = 0;
			}
		}
		if (GameSettings.moreTitans > 0 || (GameSettings.moreTitans == 0 && level.StartsWith("Custom") && GameSettings.gameType >= 2))
		{
			num = GameSettings.moreTitans;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
		{
			if (punk)
			{
				num = rate;
			}
			else if (GameSettings.bombMode == 0)
			{
				if (GameSettings.moreTitans == 0)
				{
					int num2 = 1;
					if (GameSettings.waveModeOn == 1)
					{
						num2 = GameSettings.waveModeNum;
					}
					num += (wave - 1) * (num2 - 1);
				}
				else if (GameSettings.moreTitans > 0)
				{
					int num2 = 1;
					if (GameSettings.waveModeOn == 1)
					{
						num2 = GameSettings.waveModeNum;
					}
					num += (wave - 1) * num2;
				}
			}
		}
		num = Math.Min(100, num);
		if (GameSettings.spawnMode == 1)
		{
			float num3 = GameSettings.nRate;
			float num4 = GameSettings.aRate;
			float num5 = GameSettings.jRate;
			float num6 = GameSettings.cRate;
			float num7 = GameSettings.pRate;
			if (punk && GameSettings.punkWaves == 1)
			{
				num3 = 0f;
				num4 = 0f;
				num5 = 0f;
				num6 = 0f;
				num7 = 100f;
				num = rate;
			}
			List<Vector3> list = ((RespawnPositions.TitanPositions.Length == 0) ? new List<Vector3>(new Vector3[num].Select((Vector3 x) => new Vector3(UnityEngine.Random.Range(-100f, 100f), 0f, UnityEngine.Random.Range(-100f, 100f))).ToArray()) : new List<Vector3>(RespawnPositions.TitanPositions));
			for (int i = 0; i < num; i++)
			{
				Vector3 position;
				if (list.Count == 0)
				{
					position = RespawnPositions.RandomTitanPos;
				}
				else
				{
					int index = UnityEngine.Random.Range(0, list.Count);
					position = list[index];
					list.RemoveAt(index);
				}
				float num8 = UnityEngine.Random.Range(0f, 100f);
				if (num8 <= num3 + num4 + num5 + num6 + num7)
				{
					GameObject gameObject = spawnTitanRaw(position, Quaternion.identity);
					if (num8 < num3)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
					}
					else if (num8 >= num3 && num8 < num3 + num4)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
					}
					else if (num8 >= num3 + num4 && num8 < num3 + num4 + num5)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
					}
					else if (num8 >= num3 + num4 + num5 && num8 < num3 + num4 + num5 + num6)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
					}
					else if (num8 >= num3 + num4 + num5 + num6 && num8 < num3 + num4 + num5 + num6 + num7)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
					}
					else
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
					}
				}
				else
				{
					spawnTitan(rate, position, Quaternion.identity, punk);
				}
			}
		}
		else if (level.StartsWith("Custom"))
		{
			List<Vector3> list2 = ((RespawnPositions.TitanPositions.Length == 0) ? new List<Vector3>(new Vector3[num].Select((Vector3 x) => new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f))).ToArray()) : new List<Vector3>(RespawnPositions.TitanPositions));
			for (int j = 0; j < num; j++)
			{
				Vector3 position2;
				if (list2.Count == 0)
				{
					position2 = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
				}
				else
				{
					int index2 = UnityEngine.Random.Range(0, list2.Count);
					position2 = list2[index2];
					list2.RemoveAt(index2);
				}
				spawnTitan(rate, position2, Quaternion.identity, punk);
			}
		}
		else
		{
			randomSpawnTitan("titanRespawn", abnormal, num, punk);
		}
	}

	private GameObject spawnTitanRaw(Vector3 position, Quaternion rotation)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("TITAN_VER3.1"), position, rotation);
		}
		return PhotonNetwork.Instantiate("TITAN_VER3.1", position, rotation, 0);
	}

	[RPC]
	private void spawnTitanRPC(PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			foreach (TITAN titan in titans)
			{
				if (titan.photonView.isMine && (!PhotonNetwork.isMasterClient || titan.nonAI))
				{
					PhotonNetwork.Destroy(titan.gameObject);
				}
			}
			SpawnNonAITitan2(myLastHero);
		}
		else if (!info.sender.isMasterClient && !isPlayerTTL)
		{
			ThrowJusticeDefensive(info.sender, "invalid spawnTitanRPC");
		}
	}

	public IEnumerator SkyTex()
	{
		WWW sky = new WWW("file:///" + Application.dataPath + "/Textures/FullMoonLeft.png");
		yield return sky;
		ImageFront = sky.texture;
		Material material = Camera.main.GetComponent<Skybox>().material;
		ImageFront.wrapMode = TextureWrapMode.Clamp;
		material.SetTexture("_FrontTex", ImageFront);
		material.SetTexture("_BackTex", ImageFront);
		material.SetTexture("_LeftTex", ImageFront);
		material.SetTexture("_RightTex", ImageFront);
		material.SetTexture("_UpTex", ImageFront);
		material.SetTexture("_DownTex", ImageFront);
	}

	private void Start()
	{
		base.gameObject.AddComponent<AntiCheat>();
		base.gameObject.AddComponent<MyButtons>();
		if (!IsInvoking("ConnectionProtocols"))
		{
			InvokeRepeating("ConnectionProtocols", 0f, 1f);
		}
		base.gameObject.AddComponent<FleurConsole>();
		base.gameObject.AddComponent<DetectionPanel>();
		base.gameObject.AddComponent<CharacterSelection>();
		QualitySettings.maximumLODLevel = 7;
		base.name = "MultiplayerManager";
		oneLifeList = new List<PhotonPlayer>();
		HeroCostume.init2();
		CharacterMaterials.init();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		heroes.Clear();
		eT = new ArrayList();
		titans.Clear();
		fT = new ArrayList();
		cT = new ArrayList();
		hooks = new ArrayList();
		name = string.Empty;
		allheroes.Clear();
		if (nameField2 == "")
		{
			nameField2 = "[9999FF]GUEST" + UnityEngine.Random.Range(0, 100000);
			Chatname2 = "<color=#9999FF>" + nameField2.StripHex() + "</color>";
		}
		if (nameField == "")
		{
			nameField = "[9999FF]GUEST" + UnityEngine.Random.Range(0, 100000);
			Chatname = "<color=#9999FF>" + nameField.StripHex() + "</color>";
		}
		if (privateServerField == null)
		{
			privateServerField = string.Empty;
		}
		mapScript = string.Empty;
		script_text_field = string.Empty;
		customMapScripts = false;
		is_skin_gui = false;
		usernameField = string.Empty;
		passwordField = string.Empty;
		resetGameSettings();
		banHash = new ExitGames.Client.Photon.Hashtable();
		imatitan = new ExitGames.Client.Photon.Hashtable();
		oldScript = string.Empty;
		currentLevel = string.Empty;
		if (currentScript == null)
		{
			currentScript = string.Empty;
		}
		muteList = new List<int>();
		whiteNameList = new List<int>();
		markList = new List<int>();
		titanSpawns = new List<Vector3>();
		playerSpawnsC = new List<Vector3>();
		playerSpawnsM = new List<Vector3>();
		playersRPC = new List<PhotonPlayer>();
		levelCache = new List<string[]>();
		titanSpawners = new List<TitanSpawner>();
		restartCount = new List<float>();
		ignoreList = new List<int>();
		groundList = new List<GameObject>();
		noRestart = false;
		masterRC = false;
		isSpawning = false;
		intVariables = new ExitGames.Client.Photon.Hashtable();
		heroHash = new ExitGames.Client.Photon.Hashtable();
		boolVariables = new ExitGames.Client.Photon.Hashtable();
		stringVariables = new ExitGames.Client.Photon.Hashtable();
		floatVariables = new ExitGames.Client.Photon.Hashtable();
		globalVariables = new ExitGames.Client.Photon.Hashtable();
		RCRegions = new ExitGames.Client.Photon.Hashtable();
		RCEvents = new ExitGames.Client.Photon.Hashtable();
		RCVariableNames = new ExitGames.Client.Photon.Hashtable();
		RCRegionTriggers = new ExitGames.Client.Photon.Hashtable();
		playerVariables = new ExitGames.Client.Photon.Hashtable();
		titanVariables = new ExitGames.Client.Photon.Hashtable();
		logicLoaded = false;
		customLevelLoaded = false;
		oldScriptLogic = string.Empty;
		currentScriptLogic = string.Empty;
		retryTime = 0f;
		playerList = string.Empty;
		updateTime = 0f;
		highest_feed = 0;
		highest_feed_count = 0;
		if (textureBackgroundBlack == null)
		{
			textureBackgroundBlack = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			textureBackgroundBlack.SetPixel(0, 0, Color.Lerp(Color.gray, Color.black, 0.8f));
			textureBackgroundBlack.Apply();
		}
		if (textureBackgroundBlue == null)
		{
			textureBackgroundBlue = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			textureBackgroundBlue.SetPixel(0, 0, new Color(0.08f, 0.3f, 0.4f, 1f));
			textureBackgroundBlue.Apply();
		}
		if (progressColor == null)
		{
			progressColor = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			progressColor.SetPixel(0, 0, new Color(0.08f, 0.3f, 0.4f, 0.5f));
			progressColor.Apply();
		}
		if (HookTexture == null)
		{
			HookTexture = new Texture2D(1, 1);
			HookTexture.SetPixel(1, 1, new Color(0f, 0f, 1f, 1f));
			HookTexture.Apply();
		}
		if (NamesTexture == null)
		{
			NamesTexture = new Texture2D(1, 1);
			NamesTexture.SetPixel(1, 1, new Color(0f, 0f, 1f, 1f));
			NamesTexture.Apply();
		}
		if (HexTexture == null)
		{
			HexTexture = new Texture2D(1, 1);
			HexTexture.SetPixel(1, 1, new Color(0f, 0f, 1f, 1f));
			HexTexture.Apply();
		}
		if (AHexTexture == null)
		{
			AHexTexture = new Texture2D(1, 1);
			AHexTexture.SetPixel(1, 1, new Color(0f, 0f, 1f, 1f));
			AHexTexture.Apply();
		}
		if (TeamHexTexture == null)
		{
			TeamHexTexture = new Texture2D(1, 1);
			TeamHexTexture.SetPixel(1, 1, new Color(0f, 0f, 1f, 1f));
			TeamHexTexture.Apply();
		}
		if (ETeamHexTexture == null)
		{
			ETeamHexTexture = new Texture2D(1, 1);
			ETeamHexTexture.SetPixel(1, 1, new Color(0f, 0f, 1f, 1f));
			ETeamHexTexture.Apply();
		}
		loadconfig();
		List<string> list = new List<string>
		{
			"AOTTG_HERO", "Colossal", "Icosphere", "Cube", "colossal", "CITY", "city", "rock", "PanelLogin", "LOGIN",
			"BG_TITLE"
		};
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			foreach (string item in list)
			{
				if (gameObject.name.Contains(item) || gameObject.name == "Button" || (gameObject.name == "Label" && gameObject.GetComponent<UILabel>().text.Contains("Snap")))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				else if (gameObject.name == "Checkbox")
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
		PView = base.photonView;
		instance = this;
		frequency = 0.5f;
		accum = 0f;
		nbDecimal = 0;
		frames = 0;
		sFPS = string.Empty;
		StartCoroutine(FPS());
		AutoLoad();
		base.gameObject.AddComponent<TABPlayerlist>();
		if (LANField == null)
		{
			LANField = "put ip";
		}
		if (privateAppIDField == null)
		{
			privateAppIDField = "e3341a7b-1cb9-4b6c-90e4-39b796e52876";
		}
		changingKeys = -1;
		if (PlayerPrefs.HasKey("BombSpecialKey"))
		{
			BombSpecialKey = (KeyCode)PlayerPrefs.GetInt("BombSpecialKey");
		}
		BotPersonallyContacted = (AudioClip)someBundle.Load("Contacted");
		BotQuoted = (AudioClip)someBundle.Load("Quoted");
		BotNoConnection = (AudioClip)someBundle.Load("NoConnection");
		BotTimeFinish = (AudioClip)someBundle.Load("TimeFinish");
		BotWarning = (AudioClip)someBundle.Load("Warning");
		ToClick = (AudioClip)someBundle.Load("click");
		EventManager.CreateEvent("load_custom_audio");
		EventManager.SubscribeEvent("load_custom_audio", CustomAudioManager.LoadAllSoundsForRelease);
		EventManager.CreateEvent("bomb_kill");
	}

	private IEnumerator timeAntispam()
	{
		disableEventAntispam = true;
		yield return new WaitForSeconds(2f);
		disableEventAntispam = false;
	}

	[DllImport("user32.dll")]
	private static extern bool ShowWindow(int hWnd, int nCmdShow);

	private void OnApplicationFocus(bool focus)
	{
		if (!focus && Screen.fullScreen)
		{
			Screen.SetResolution(UIMainReferences.Width, UIMainReferences.Height, fullscreen: false);
			ShowWindow(GetActiveWindow(), 11);
		}
		else if (focus && !Screen.fullScreen && !UIMainReferences.isGAMEFirstLaunch)
		{
			if (UIMainReferences.Fullscreen)
			{
				Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, false);
				Screen.fullScreen = false;
                BorderlessHelper.MakeBorderlessFullscreen();
            }
			else
			{
                Screen.SetResolution(UIMainReferences.Width, UIMainReferences.Height, false);
            }
			StartCoroutine(sethud());
		}
	}

	private IEnumerator sethud()
	{
		yield return new WaitForSeconds(0.5f);
		IN_GAME_MAIN_CAMERA.mainCamera.setHUDposition();
		Screen.lockCursor = false;
	}

	private void AutoLoad()
	{
		nameField = PlayerPrefs.GetString("name", "someguest");
		LoginFengKAI.player.name = nameField;
		guildField = PlayerPrefs.GetString("guildname", string.Empty);
		Chatname = PlayerPrefs.GetString("Cname", "someguest");
		if (Chatname == string.Empty || (Chatname.Contains("]") && Chatname.Contains("[")))
		{
			Chatname = nameField.hexColor();
		}
		Chatcolor = PlayerPrefs.GetString("chatcolor", "ffffff");
		Prefix = PlayerPrefs.GetString("Prefix", Prefix);
		Suffix = PlayerPrefs.GetString("Suffix", Suffix);
		nameField2 = PlayerPrefs.GetString("name2", "someguest");
		LoginFengKAI.player.name = nameField2;
		guildField2 = PlayerPrefs.GetString("guildname2", string.Empty);
		Chatname2 = PlayerPrefs.GetString("Cname2", "someguest");
		if (Chatname2 == string.Empty || (Chatname2.Contains("]") && Chatname.Contains("[")))
		{
			Chatname2 = nameField2.hexColor();
		}
		Chatcolor2 = PlayerPrefs.GetString("chatcolor2", "ffffff");
		Prefix2 = PlayerPrefs.GetString("Prefix2", Prefix2);
		Suffix2 = PlayerPrefs.GetString("Suffix2", Suffix2);
		privateAppIDField = PlayerPrefs.GetString("appid", "");
	}

	[RPC]
	public void titanGetKill(PhotonPlayer player, int Damage, string name)
	{
		Damage = Mathf.Max(10, Damage);
		object[] parameters = new object[1] { Damage };
		PView.RPC("netShowDamage", player, parameters);
		object[] parameters2 = new object[2] { name, false };
		PView.RPC("oneTitanDown", PhotonTargets.MasterClient, parameters2);
		sendKillInfo(t1: false, (string)player.customProperties["name"], t2: true, name, Damage);
		playerKillInfoUpdate(player, Damage);
	}

	public void titanGetKillbyServer(int Damage, string name)
	{
		Damage = Mathf.Max(10, Damage);
		sendKillInfo(t1: false, LoginFengKAI.player.name, t2: true, name, Damage);
		netShowDamage(Damage);
		oneTitanDown(name, onPlayerLeave: false);
		playerKillInfoUpdate(PhotonNetwork.player, Damage);
	}

	public void unloadAssets()
	{
		if (!isUnloading)
		{
			isUnloading = true;
			StartCoroutine(unloadAssetsE(10f));
		}
	}

	public IEnumerator unloadAssetsE(float time)
	{
		yield return new WaitForSeconds(time);
		Resources.UnloadUnusedAssets();
		isUnloading = false;
	}

	public void unloadAssetsEditor()
	{
		if (!isUnloading)
		{
			isUnloading = true;
			StartCoroutine(unloadAssetsE(30f));
		}
	}

	private void AutorejoinPorts()
	{
		if (!PhotonNetwork.offlineMode && !PhotonNetwork.connected && shallRejoin[0] is bool && (bool)shallRejoin[0])
		{
			if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
			{
				PhotonNetwork.ConnectToMaster((string)shallRejoin[1], 5055, applicationId, UIMainReferences.version);
			}
			else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
			{
				PhotonNetwork.ConnectToMaster((string)shallRejoin[1], 4530, applicationId, UIMainReferences.version);
			}
		}
		if (shallRejoin[0] is bool && (bool)shallRejoin[0])
		{
			if (!PhotonNetwork.offlineMode)
			{
				if ((int)settings[336] == 1)
				{
					PhotonNetwork.CreateRoom(string.Concat(shallRejoin[3], "`", shallRejoin[4], "`", shallRejoin[5], "`", UnityEngine.Random.Range(12000000, 13000000), "`", shallRejoin[6], "`", string.Empty, "`", UnityEngine.Random.Range(1200000000, 1300000000)), isVisible: true, isOpen: true, Convert.ToInt32(shallRejoin[7]));
				}
				else if ((((RoomInfo)shallRejoin[2]).maxPlayers == 0 || ((RoomInfo)shallRejoin[2]).maxPlayers > ((RoomInfo)shallRejoin[2]).playerCount) && ((RoomInfo)shallRejoin[2]).open)
				{
					PhotonNetwork.JoinRoom(((RoomInfo)shallRejoin[2]).name);
				}
				else
				{
					shallRejoin[0] = false;
				}
			}
			else
			{
				PhotonNetwork.Disconnect();
				shallRejoin[0] = false;
			}
		}
		if (timeofAutorejoin.IsRunning && !PhotonNetwork.inRoom && timeofAutorejoin.ElapsedMilliseconds >= 5000)
		{
			timeofAutorejoin.Stop();
			timeofAutorejoin.Reset();
			shallRejoin[0] = false;
			PhotonNetwork.Disconnect();
			Screen.lockCursor = false;
			Screen.showCursor = true;
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
			gameStart = false;
			FengCustomInputs.Inputs.menuOn = false;
			DestroyAllExistingCloths();
			UnityEngine.Object.Destroy(CacheGameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
		}
	}

	private void Update()
	{
		if (gameStart)
		{
			foreach (HERO hero in heroes)
			{
				try
				{
					hero.update();
				}
				catch(Exception e)
				{
					InRoomChat.Write(e.Message);
				}
			}
			foreach (Bullet hook in hooks)
			{
				hook.update();
			}
			if (IN_GAME_MAIN_CAMERA.mainCamera != null)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.snapShotUpdate();
			}
			foreach (TITAN_EREN item in eT)
			{
				item.update();
			}
			foreach (TITAN titan in titans)
			{
				titan.update();
			}
			foreach (FEMALE_TITAN item2 in fT)
			{
				item2.update();
			}
			foreach (COLOSSAL_TITAN item3 in cT)
			{
				item3.update();
			}
			if (IN_GAME_MAIN_CAMERA.isReady)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.update();
			}
		}
		if (inputRC.isInputExtras(InputCodeRC.leave) && !FengCustomInputs.Inputs.menuOn && !FengCustomInputs.Inputs.isInputDown[InputCode.pause])
		{
			Application.Quit();
		}
		if (inputRC.isInputExtras(InputCodeRC.restart))
		{
			if (PhotonNetwork.isMasterClient)
			{
				restartCD.Start();
				if (restartCD.ElapsedMilliseconds > 2000 || restartCD.ElapsedMilliseconds == 0L)
				{
					restartCD.Reset();
					InRoomChat.Chat("<color=#FFCC00>MasterClient has restarted the game!</color>");
					instance.restartRC();
					restartCD.Start();
				}
			}
			else
			{
				InRoomChat.Write("<color=#FFCC00>error: not master client</color>");
			}
		}
		if (Input.GetMouseButtonDown(0) && GameSettings.Clicking > 0 && FengCustomInputs.Inputs.menuOn)
		{
			//AudioSource.PlayClipAtPoint(ToClick, Camera.main.transform.position);
		}
		if (GameSettings.ShowFPS > 0)
		{
			accum += Time.timeScale / Time.deltaTime;
			frames++;
		}
		//AutorejoinPorts();
	}

	private IEnumerator FPS()
	{
		while (true)
		{
			float num = accum / (float)frames;
			sFPS = num.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
			sFPS = string.Format("FPS: {0}", num.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10)));
			accum = 0f;
			frames = 0;
			yield return new WaitForSeconds(frequency);
		}
	}

	[RPC]
	private void updateKillInfo(bool t1, string killer, bool t2, string victim, int dmg)
	{
		GameObject gameObject = CacheGameObject.Find("UI_IN_GAME");
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("UI/KillInfo"));
		for (int i = 0; i < killInfoGO.Count; i++)
		{
			GameObject gameObject3 = (GameObject)killInfoGO[i];
			if (gameObject3 != null)
			{
				gameObject3.GetComponent<KillInfoComponent>().moveOn();
			}
		}
		if (killInfoGO.Count > 4)
		{
			GameObject gameObject3 = (GameObject)killInfoGO[0];
			if (gameObject3 != null)
			{
				gameObject3.GetComponent<KillInfoComponent>().destory();
			}
			killInfoGO.RemoveAt(0);
		}
		gameObject2.transform.parent = gameObject.GetComponent<UIReferArray>().panels[0].transform;
		gameObject2.GetComponent<KillInfoComponent>().show(t1, killer, t2, victim, dmg);
		killInfoGO.Add(gameObject2);
		int index = ReportKillToChatConsoleFeed(killer, victim, dmg);
		if (killer.Equals(RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"])) && (double)GameSettings.sizeUpper <= 3.0 && (int)settings[281] > 0)
		{
			feedManager.AddEntry(roundTime, dmg, GenerateChatFeedString(killer, victim, dmg, roundTime), index);
			KeyValuePair<int, int> keyValuePair = feedManager.Iterate(roundTime);
			feed_number = keyValuePair.Key;
			feed_count = keyValuePair.Value;
			if (feed_number > highest_feed)
			{
				highest_feed = feed_number;
				highest_feed_count = feed_count;
			}
		}
	}

	private string GenerateChatFeedString(string killer, string victim, int damage, float roundTime)
	{
		string text = "<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> " + killer.hexColor() + " killed ";
		return text + victim.hexColor() + " for <color=#" + finalhex() + ">" + damage + "</color> damage.";
	}

	public int ReportKillToChatConsoleFeed(string killer, string victim, int damage)
	{
		int result = -1;
		if ((int)settings[244] != 0)
		{
			string text = "<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> " + killer.hexColor() + " killed ";
			string text2 = text + victim.hexColor() + " for <color=#" + finalhex() + ">" + damage + "</color> damage.";
			if ((int)settings[244] == 1)
			{
				result = InRoomChat.Write(text2);
				InRoomChat.ChatInstanse.timeOfFade.Reset();
				InRoomChat.ChatInstanse.timeOfFade.Start();
			}
			else
			{
				result = FleurConsole.instance.Log(text2 ?? "", FleurConsole.DebugType.LOG);
			}
		}
		return result;
	}

	[RPC]
	public void verifyPlayerHasLeft(int ID, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient && PhotonPlayer.Find(ID) != null)
		{
			PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
			string empty = string.Empty;
			empty = RCextensions.returnStringFromObject(photonPlayer.customProperties["name"]);
			banHash.Add(ID, empty);
		}
	}

	public void RecompilePlayerList(float time)
	{
		if (!isRecompiling)
		{
			if (GameSettings.bombMode == 1 && GameSettings.teamMode == 1)
			{
				isRecompiling = true;
				StartCoroutine(TeamBombLogic(time));
			}
			else
			{
				isRecompiling = true;
				updatePlayerList();
			}
		}
	}

	public void updatePlayerList()
	{
		string text = string.Empty;
		new List<List<TableElement>>();
		PhotonPlayer[] array = PhotonNetwork.playerList.OrderBy((PhotonPlayer player) => player.ID).ToArray();
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer == null)
			{
				continue;
			}
			ExitGames.Client.Photon.Hashtable customProperties = photonPlayer.customProperties;
			if (customProperties == null || customProperties["dead"] == null)
			{
				continue;
			}
			if (ignoreList.Contains(photonPlayer.ID))
			{
				text += "[FF0000][X] ";
			}
			text = ((!photonPlayer.isLocal) ? (text + "[FFCC00][" + photonPlayer.ID + "] ") : (text + "[00CC00][" + photonPlayer.ID + "][-] "));
			if (DCPeopleList.Contains(RCextensions.returnStringFromObject(photonPlayer.customProperties["name"])))
			{
				text += "[000000][BanList][-]";
			}
			else if (muteList.Contains(photonPlayer.ID))
			{
				text += "[ffffff][Muted][-] ";
			}
			if (photonPlayer.isMasterClient)
			{
				text += "[ffffff][M] ";
			}
			if (photonPlayer.Tlw || photonPlayer.isLocal)
			{
				text += "[ffffff][TLW]";
			}
			else if (photonPlayer.RRC)
			{
				text += "[ffffff][RRC]";
			}
			else if (photonPlayer.customProperties.ContainsKey("NoodleDoodle"))
			{
				text += "[ffffff][NoodleBot]";
			}
			else if (photonPlayer.OldTlw)
			{
				text += "[ffffff][OldTLW]";
			}
			else if (photonPlayer.NewTlw)
			{
				text += "[ffffff][NewTLW]";
			}
			if ((bool)customProperties["dead"])
			{
				text = text + "[" + ColorSet.color_red + "] *dead* ";
			}
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) < 2)
			{
				int num = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["team"]);
				if (num < 2)
				{
					text = text + "[" + ColorSet.color_human + "] H ";
				}
				else if (num == 2)
				{
					text = text + "[" + ColorSet.color_human_1 + "] A [-]";
				}
			}
			else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["isTitan"]) == 2)
			{
				text = text + "[" + ColorSet.color_titan_player + "] <T> [-]";
			}
			object obj = text;
			if (photonPlayer == PhotonNetwork.player)
			{
				if (!InRoomChat.kdEnabled)
				{
                    PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
                    {
						{ "kills", 0 },
						{ "deaths", 0 },
						{ "max_dmg", 0 },
						{ "total_dmg", 0 }
					});
                }
			}
			text = string.Concat(obj, string.Empty, customProperties["name"], "[ffffff]:", customProperties["kills"], "/", customProperties["deaths"], "/", customProperties["max_dmg"], "/", customProperties["total_dmg"]);
			text += "\n";
		}
		if (PhotonNetwork.isMasterClient && !isWinning && !isLosing && roundTime >= 5f)
		{
			if (GameSettings.pointMode > 0)
			{
				if (GameSettings.teamMode == 0)
				{
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer2 = PhotonNetwork.playerList[j];
						if (RCextensions.returnIntFromObject(photonPlayer2.customProperties["kills"]) >= GameSettings.pointMode)
						{
							object[] parameters = new object[2]
							{
								"<color=#FFCC00>" + RCextensions.returnStringFromObject(photonPlayer2.customProperties["name"]).hexColor() + " wins!</color>",
								string.Empty
							};
							PView.RPC("Chat", PhotonTargets.All, parameters);
							gameWin2();
						}
					}
				}
			}
			else if (GameSettings.pointMode <= 0 && (GameSettings.bombMode == 1 || GameSettings.pvpMode > 0) && GameSettings.teamMode == 0 && PhotonNetwork.playerList.Length > 1)
			{
				int num2 = 0;
				string text2 = "Nobody";
				_ = PhotonNetwork.playerList[0];
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer3 = PhotonNetwork.playerList[j];
					if (photonPlayer3.customProperties["dead"] != null && !RCextensions.returnBoolFromObject(photonPlayer3.customProperties["dead"]))
					{
						text2 = photonPlayer3.uiname;
						num2++;
					}
				}
				if (num2 <= 1)
				{
					object[] parameters2 = new object[2]
					{
						"<color=#FFCC00>" + text2.hexColor() + " wins.</color>",
						string.Empty
					};
					PView.RPC("Chat", PhotonTargets.All, parameters2);
					gameWin2();
				}
			}
		}
		isRecompiling = false;
		ShowHUDInfoTopLeft(text);
	}

	public IEnumerator TeamBombLogic(float time)
	{
		yield return new WaitForSeconds(time);
		string text = string.Empty;
		if (GameSettings.teamMode > 0)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			Dictionary<int, PhotonPlayer> dictionary = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer in array)
			{
				if (photonPlayer.customProperties["dead"] != null && !ignoreList.Contains(photonPlayer.ID))
				{
					switch (RCextensions.returnIntFromObject(photonPlayer.customProperties["RCteam"]))
					{
					case 0:
						dictionary3.Add(photonPlayer.ID, photonPlayer);
						break;
					case 1:
						dictionary.Add(photonPlayer.ID, photonPlayer);
						num += RCextensions.returnIntFromObject(photonPlayer.customProperties["kills"]);
						num3 += RCextensions.returnIntFromObject(photonPlayer.customProperties["deaths"]);
						num5 += RCextensions.returnIntFromObject(photonPlayer.customProperties["max_dmg"]);
						num7 += RCextensions.returnIntFromObject(photonPlayer.customProperties["total_dmg"]);
						break;
					case 2:
						dictionary2.Add(photonPlayer.ID, photonPlayer);
						num2 += RCextensions.returnIntFromObject(photonPlayer.customProperties["kills"]);
						num4 += RCextensions.returnIntFromObject(photonPlayer.customProperties["deaths"]);
						num6 += RCextensions.returnIntFromObject(photonPlayer.customProperties["max_dmg"]);
						num8 += RCextensions.returnIntFromObject(photonPlayer.customProperties["total_dmg"]);
						break;
					}
				}
			}
			cyanKills = num;
			magentaKills = num2;
			text = text + "[ffffff]TEAM ONE" + ":" + cyanKills + "/" + num3 + "/" + num5 + "/" + num7 + "\n";
			foreach (PhotonPlayer value in dictionary.Values)
			{
				int num9 = RCextensions.returnIntFromObject(value.customProperties["RCteam"]);
				if (value.customProperties["dead"] == null || num9 != 1)
				{
					continue;
				}
				if (ignoreList.Contains(value.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value.ID) + "] ";
				if (DCPeopleList.Contains(RCextensions.returnStringFromObject(value.customProperties["name"])))
				{
					text += "[000000][BanList][-]";
				}
				else if (muteList.Contains(value.ID))
				{
					text += "[ffffff][Muted][-] ";
				}
				if (value.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (value.Tlw || value.isLocal)
				{
					text += "[ffffff][TLW][-]";
				}
				else if (value.RRC)
				{
					text += "[ffffff][RRC][-] ";
				}
				else if (value.customProperties.ContainsKey("NoodleDoodle"))
				{
					text += "[ffffff][NoodleBot][-]";
				}
				if (RCextensions.returnBoolFromObject(value.customProperties["dead"]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value.customProperties["isTitan"]) < 2)
				{
					int num10 = RCextensions.returnIntFromObject(value.customProperties["team"]);
					if (num10 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num10 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value.customProperties["isTitan"]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				_ = string.Empty;
				string text3 = ((value.uiname.Length > 100) ? value.uiname.Substring(0, 20) : value.uiname);
				int num11 = RCextensions.returnIntFromObject(value.customProperties["kills"]);
				int num12 = RCextensions.returnIntFromObject(value.customProperties["deaths"]);
				int num13 = RCextensions.returnIntFromObject(value.customProperties["max_dmg"]);
				int num14 = RCextensions.returnIntFromObject(value.customProperties["total_dmg"]);
				text = text2 + string.Empty + text3 + "[ffffff]:" + num11 + "/" + num12 + "/" + num13 + "/" + num14;
				if (RCextensions.returnBoolFromObject(value.customProperties["dead"]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = text + " \n" + "[ffffff]TEAM TWO" + ":" + magentaKills + "/" + num4 + "/" + num6 + "/" + num8 + "\n";
			foreach (PhotonPlayer value2 in dictionary2.Values)
			{
				int num9 = RCextensions.returnIntFromObject(value2.customProperties["RCteam"]);
				if (value2.customProperties["dead"] == null || num9 != 2)
				{
					continue;
				}
				if (ignoreList.Contains(value2.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value2.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value2.ID) + "] ";
				if (DCPeopleList.Contains(RCextensions.returnStringFromObject(value2.customProperties["name"])))
				{
					text += "[000000][BanList][-]";
				}
				else if (muteList.Contains(value2.ID))
				{
					text += "[ffffff][Muted][-] ";
				}
				if (value2.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (value2.Tlw || value2.isLocal)
				{
					text += "[ffffff][TLW][-]";
				}
				else if (value2.RRC)
				{
					text += "[ffffff][RRC][-] ";
				}
				else if (value2.customProperties.ContainsKey("NoodleDoodle"))
				{
					text += "[ffffff][NoodleBot][-]";
				}
				if (RCextensions.returnBoolFromObject(value2.customProperties["dead"]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value2.customProperties["isTitan"]) < 2)
				{
					int num10 = RCextensions.returnIntFromObject(value2.customProperties["team"]);
					if (num10 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num10 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value2.customProperties["isTitan"]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				_ = string.Empty;
				string text3 = ((value2.uiname.Length > 100) ? value2.uiname.Substring(0, 20) : value2.uiname);
				int num11 = RCextensions.returnIntFromObject(value2.customProperties["kills"]);
				int num12 = RCextensions.returnIntFromObject(value2.customProperties["deaths"]);
				int num13 = RCextensions.returnIntFromObject(value2.customProperties["max_dmg"]);
				int num14 = RCextensions.returnIntFromObject(value2.customProperties["total_dmg"]);
				text = text2 + string.Empty + text3 + "[ffffff]:" + num11 + "/" + num12 + "/" + num13 + "/" + num14;
				if (RCextensions.returnBoolFromObject(value2.customProperties["dead"]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = string.Concat(new object[3] { text, " \n", "[00FF00]INDIVIDUAL\n" });
			foreach (PhotonPlayer value3 in dictionary3.Values)
			{
				int num9 = RCextensions.returnIntFromObject(value3.customProperties["RCteam"]);
				if (value3.customProperties["dead"] == null || num9 != 0)
				{
					continue;
				}
				if (ignoreList.Contains(value3.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value3.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value3.ID) + "] ";
				if (DCPeopleList.Contains(RCextensions.returnStringFromObject(value3.customProperties["name"])))
				{
					text += "[000000][BanList][-]";
				}
				else if (muteList.Contains(value3.ID))
				{
					text += "[ffffff][Muted][-] ";
				}
				if (value3.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (value3.Tlw || value3.isLocal)
				{
					text += "[ffffff][TLW][-]";
				}
				else if (value3.RRC)
				{
					text += "[ffffff][RRC][-] ";
				}
				else if (value3.customProperties.ContainsKey("NoodleDoodle"))
				{
					text += "[ffffff][NoodleBot][-]";
				}
				if (RCextensions.returnBoolFromObject(value3.customProperties["dead"]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value3.customProperties["isTitan"]) < 2)
				{
					int num10 = RCextensions.returnIntFromObject(value3.customProperties["team"]);
					if (num10 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num10 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value3.customProperties["isTitan"]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				_ = string.Empty;
				string text3 = ((value3.uiname.Length > 100) ? value3.uiname.Substring(0, 20) : value3.uiname);
				int num11 = RCextensions.returnIntFromObject(value3.customProperties["kills"]);
				int num12 = RCextensions.returnIntFromObject(value3.customProperties["deaths"]);
				int num13 = RCextensions.returnIntFromObject(value3.customProperties["max_dmg"]);
				int num14 = RCextensions.returnIntFromObject(value3.customProperties["total_dmg"]);
				text = text2 + string.Empty + text3 + "[ffffff]:" + num11 + "/" + num12 + "/" + num13 + "/" + num14;
				if (RCextensions.returnBoolFromObject(value3.customProperties["dead"]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		playerList = text;
		if (PhotonNetwork.isMasterClient && !isWinning && !isLosing && roundTime >= 5f)
		{
			if (GameSettings.pointMode > 0)
			{
				if (GameSettings.teamMode > 0)
				{
					if (cyanKills >= GameSettings.pointMode)
					{
						cyanWins++;
						object[] parameters = new object[2]
						{
							"<size=16>Team One wins! FIRST: " + cyanWins + ", SECOND: " + magentaWins + "</size>",
							string.Empty
						};
						PView.RPC("Chat", PhotonTargets.All, parameters);
						gameWin2();
					}
					else if (magentaKills >= GameSettings.pointMode)
					{
						magentaWins++;
						object[] parameters2 = new object[2]
						{
							"<size=16>Team Two wins! FIRST: " + cyanWins + ", SECOND: " + magentaWins + "</size>",
							string.Empty
						};
						PView.RPC("Chat", PhotonTargets.All, parameters2);
						gameWin2();
					}
				}
				else if (GameSettings.teamMode == 0)
				{
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer5 = PhotonNetwork.playerList[j];
						if (RCextensions.returnIntFromObject(photonPlayer5.customProperties["kills"]) >= GameSettings.pointMode)
						{
							object[] parameters3 = new object[2]
							{
								"<color=#FFCC00>" + RCextensions.returnStringFromObject(photonPlayer5.customProperties["name"]).hexColor() + " wins!</color>",
								string.Empty
							};
							PView.RPC("Chat", PhotonTargets.All, parameters3);
							gameWin2();
						}
					}
				}
			}
			else if (GameSettings.pointMode <= 0 && (GameSettings.bombMode == 1 || GameSettings.pvpMode > 0))
			{
				if (GameSettings.teamMode > 0 && PhotonNetwork.playerList.Length > 1)
				{
					int num15 = 0;
					int num16 = 0;
					int num17 = 0;
					int num18 = 0;
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer6 = PhotonNetwork.playerList[j];
						if (ignoreList.Contains(photonPlayer6.ID) || photonPlayer6.customProperties["RCteam"] == null || photonPlayer6.customProperties["dead"] == null)
						{
							continue;
						}
						if (RCextensions.returnIntFromObject(photonPlayer6.customProperties["RCteam"]) == 1)
						{
							num17++;
							if (!RCextensions.returnBoolFromObject(photonPlayer6.customProperties["dead"]))
							{
								num15++;
							}
						}
						else if (RCextensions.returnIntFromObject(photonPlayer6.customProperties["RCteam"]) == 2)
						{
							num18++;
							if (!RCextensions.returnBoolFromObject(photonPlayer6.customProperties["dead"]))
							{
								num16++;
							}
						}
					}
					if (num17 > 0 && num18 > 0)
					{
						if (num15 == 0)
						{
							magentaWins++;
							object[] parameters4 = new object[2]
							{
								"<size=16>Team Two wins! FIRST: " + cyanWins + ", SECOND: " + magentaWins + "</size>",
								string.Empty
							};
							PView.RPC("Chat", PhotonTargets.All, parameters4);
							gameWin2();
						}
						else if (num16 == 0)
						{
							cyanWins++;
							object[] parameters5 = new object[2]
							{
								"<size=16>Team One wins! FIRST: " + cyanWins + ", SECOND: " + magentaWins + "</size>",
								string.Empty
							};
							PView.RPC("Chat", PhotonTargets.All, parameters5);
							gameWin2();
						}
					}
				}
				else if (GameSettings.teamMode == 0 && PhotonNetwork.playerList.Length > 1)
				{
					int num19 = 0;
					string text4 = "Nobody";
					_ = PhotonNetwork.playerList[0];
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer7 = PhotonNetwork.playerList[j];
						if (photonPlayer7.customProperties["dead"] != null && !RCextensions.returnBoolFromObject(photonPlayer7.customProperties["dead"]))
						{
							text4 = photonPlayer7.uiname;
							num19++;
						}
					}
					if (num19 <= 1)
					{
						object[] parameters6 = new object[2]
						{
							"<color=#FFCC00>" + text4.hexColor() + " wins.</color>",
							string.Empty
						};
						PView.RPC("Chat", PhotonTargets.All, parameters6);
						gameWin2();
					}
				}
			}
		}
		isRecompiling = false;
		ShowHUDInfoTopLeft(playerList);
	}

	public IEnumerator WaitAndReloadKDR(PhotonPlayer player)
	{
		yield return new WaitForSeconds(5f);
		string key = RCextensions.returnStringFromObject(player.customProperties["name"]);
		if (PreservedPlayerKDR.ContainsKey(key))
		{
			int[] array = PreservedPlayerKDR[key];
			PreservedPlayerKDR.Remove(key);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("kills", array[0]);
			hashtable.Add("deaths", array[1]);
			hashtable.Add("max_dmg", array[2]);
			hashtable.Add("total_dmg", array[3]);
			player.SetCustomProperties(hashtable);
		}
	}

	public IEnumerator WaitAndResetRestarts()
	{
		yield return new WaitForSeconds(10f);
		restartingBomb = false;
		restartingEren = false;
		restartingHorse = false;
		restartingMC = false;
	}

	public IEnumerator WaitAndRespawn1(float time, string str)
	{
		yield return new WaitForSeconds(time);
		SpawnPlayer(myLastHero, str);
	}

	public IEnumerator WaitAndRespawn2(float time, GameObject pos)
	{
		yield return new WaitForSeconds(time);
		SpawnPlayerAt2(myLastHero, pos);
	}
}
