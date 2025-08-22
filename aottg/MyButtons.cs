using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;

public class MyButtons : MonoBehaviour
{
	public static MyButtons instance;

	private const float ButtonsWidth = 200f;

	private float _buttonsIncWidth;

	private const float LeftPanelWidth = 300f;

	private float _leftPanelIncWidth;

	public bool isVisible = true;

	private bool close;

	private bool EnlargePanel;

	public static GUIStyle _buttonstyles;

	private AudioSource menu_open;

	private AudioSource menu_close;

	private AudioSource lpanel_enlarge;

	private static GUIStyle LPanelOpenButton;

	private static Texture2D LPanel;

	private static Texture2D LPanelH;

	private static GUIStyle LPanelProfileButton;

	private static Texture2D LPanelPoff;

	private static Texture2D LPanelPon;

	private static Texture2D LPanelPhover;

	private static GUIStyle LPanelServersButton;

	private static Texture2D LPanelSoff;

	private static Texture2D LPanelSon;

	private static Texture2D LPanelShover;

	private Texture2D _red;

	private Texture2D _lightpink;

	private static GUIStyle updatestyle;

	private bool OpenProfile;

	private bool OpenServers = true;

	private Vector2 RegionsScroll = Vector2.zero;

	private static GUIStyle currentStyle;

	private static Texture2D Texture(byte r, byte g, byte b, byte a = byte.MaxValue)
	{
		return Texture(MyColor(r, g, b, a));
	}

	private static Color MyColor(byte r, byte g, int b, byte a = byte.MaxValue)
	{
		return new Color((float)(int)r / 255f, (float)(int)g / 255f, (float)b / 255f, (float)(int)a / 255f);
	}

	private static Texture2D Texture(Color color)
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(1, 1, color);
		texture2D.Apply();
		return texture2D;
	}

	private GUIStyle ButtonStyles()
	{
		_buttonstyles = new GUIStyle(GUI.skin.button);
		_buttonstyles.normal.background = _lightpink;
		_buttonstyles.hover.background = _red;
		_buttonstyles.active.background = null;
		_buttonstyles.alignment = TextAnchor.MiddleCenter;
		_buttonstyles.fontSize = 16;
		_buttonstyles.font = FengGameManagerMKII.SegoeItalic;
		_buttonstyles.normal.textColor = Color.white;
		return _buttonstyles;
	}

	private static GUIStyle LPanelEyeButton()
	{
		LPanelOpenButton = new GUIStyle(GUI.skin.button);
		LPanelOpenButton.normal.background = LPanelH;
		LPanelOpenButton.hover.background = LPanel;
		LPanelOpenButton.active.background = LPanel;
		return LPanelOpenButton;
	}

	private GUIStyle LPanelProfButton()
	{
		LPanelProfileButton = new GUIStyle(GUI.skin.button);
		LPanelProfileButton.normal.background = (OpenProfile ? LPanelPon : LPanelPoff);
		LPanelProfileButton.hover.background = LPanelPhover;
		LPanelProfileButton.active.background = LPanelPon;
		return LPanelProfileButton;
	}

	private GUIStyle LPanelServButton()
	{
		LPanelServersButton = new GUIStyle(GUI.skin.button);
		LPanelServersButton.normal.background = (OpenServers ? LPanelSon : LPanelSoff);
		LPanelServersButton.hover.background = LPanelShover;
		LPanelServersButton.active.background = LPanelSon;
		return LPanelServersButton;
	}

	private void Start()
	{
		instance = this;
		_red = Texture(153, 0, 0, 140);
		_lightpink = Texture(byte.MaxValue, 182, 193, 140);
		_buttonsIncWidth = 0f;
		_leftPanelIncWidth = 0f;
		menu_open = Camera.main.gameObject.AddComponent<AudioSource>();
		menu_open.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("menu_open");
		menu_open.volume = FengGameManagerMKII.InterfaceVolume;
		menu_close = Camera.main.gameObject.AddComponent<AudioSource>();
		menu_close.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("menu_close");
		menu_close.volume = FengGameManagerMKII.InterfaceVolume;
		lpanel_enlarge = Camera.main.gameObject.AddComponent<AudioSource>();
		lpanel_enlarge.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("LPanel_enlarge");
		lpanel_enlarge.volume = FengGameManagerMKII.InterfaceVolume;
		LPanel = (Texture2D)FengGameManagerMKII.someBundle.Load("LPanel");
		LPanelH = (Texture2D)FengGameManagerMKII.someBundle.Load("LPanel_hover");
		LPanelPoff = (Texture2D)FengGameManagerMKII.someBundle.Load("Profile off");
		LPanelPon = (Texture2D)FengGameManagerMKII.someBundle.Load("Profile on");
		LPanelPhover = (Texture2D)FengGameManagerMKII.someBundle.Load("Profile on_hover");
		LPanelSoff = (Texture2D)FengGameManagerMKII.someBundle.Load("Multi off");
		LPanelSon = (Texture2D)FengGameManagerMKII.someBundle.Load("Multi on");
		LPanelShover = (Texture2D)FengGameManagerMKII.someBundle.Load("Multi on_hover");
		FengGameManagerMKII.ServerType = PlayerPrefs.GetInt("servertype", 0);
	}

	private string AllRegions(string AskingRegion)
	{
		string result = "";
		switch (AskingRegion)
		{
		case "eu":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "135.125.239.180" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-eu.exitgamescloud.com"));
			break;
		case "us":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "142.44.242.29" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-us.exitgamescloud.com"));
			break;
		case "brazil":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "108.181.69.221" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-sa.exitgames.com"));
			break;
		case "asia":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "51.79.164.137" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-asia.exitgamescloud.com"));
			break;
		case "turkey":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-tr.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-tr.exitgames.com"));
			break;
		case "australia":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-au.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-au.exitgames.com"));
			break;
		case "canada":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-cae.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-cae.exitgames.com"));
			break;
		case "uswest":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-usw.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-usw.exitgames.com"));
			break;
		case "ussouth":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-ussc.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-ussc.exitgames.com"));
			break;
		case "india":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-in.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-in.exitgames.com"));
			break;
		case "japan":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-jp.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-jp.exitgames.com"));
			break;
		case "korea":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-kr.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-kr.exitgames.com"));
			break;
		case "safrica":
			result = ((FengGameManagerMKII.LanOrAppID == 0) ? "app-za.exitgames.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-za.exitgames.com"));
			break;
		}
		return result;
	}

	private IEnumerator HideTextures(int number)
	{
		close = true;
		if (!menu_close.isPlaying)
		{
			//menu_close.Play();
		}
		yield return new WaitForSeconds(0.8f);
		isVisible = false;
		Object.Destroy(_lightpink);
		Object.Destroy(_red);
		switch (number)
		{
		case 1:
			PlayerPrefs.SetInt("Region", (int)FengGameManagerMKII.settings[293]);
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMain, state: false);
			FengGameManagerMKII.OnPrivateServer = true;
			if (FengGameManagerMKII.LanOrAppID == 0)
			{
				FengGameManagerMKII.applicationId = "";
			}
			if (FengGameManagerMKII.ServerType == 0)
			{
				UIMainReferences.version = UIMainReferences.fengVersion;
			}
			else if (FengGameManagerMKII.ServerType == 1)
			{
				UIMainReferences.version = FengGameManagerMKII.s[0];
			}
			else if (FengGameManagerMKII.ServerType == 2)
			{
				UIMainReferences.version = FengGameManagerMKII.privateServerField;
			}
			switch ((int)FengGameManagerMKII.settings[293])
			{
			case 0:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("eu"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("eu"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("eu"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "EU";
				break;
			case 1:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("us"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("us"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("us"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "US East";
				break;
			case 2:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("brazil"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("brazil"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("brazil"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "South America";
				break;
			case 3:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("asia"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("asia"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("asia"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "Asia";
				break;
			case 4:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("turkey"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("turkey"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("turkey"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "Turkiye";
				break;
			case 5:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("australia"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("australia"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("australia"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "Australia";
				break;
			case 6:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("canada"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("canada"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("canada"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "Canada";
				break;
			case 7:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("uswest"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("uswest"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("uswest"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "US West";
				break;
			case 8:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("ussouth"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("ussouth"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("ussouth"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "US Central South";
				break;
			case 9:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("india"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("india"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("india"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "India";
				break;
			case 10:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("japan"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("japan"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("japan"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "Japan";
				break;
			case 11:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("korea"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("korea"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("korea"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "South Korea";
				break;
			case 12:
				if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("safrica"), 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("safrica"), 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
				{
					PhotonNetwork.ConnectToMaster(AllRegions("safrica"), 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
				}
				TLWWebSocket.Region = "South Africa";
				break;
			}
			break;
		case 2:
			PhotonNetwork.offlineMode = true;
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMain, state: false);
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiSet, state: true);
			break;
		case 3:
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMain, state: false);
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelSingleSet, state: true);
			break;
		case 4:
			Application.LoadLevel("characterCreation");
			break;
		case 5:
			Application.LoadLevel("SnapShot");
			break;
		}
	}

	public void Render()
	{
		if (!string.IsNullOrEmpty(FengGameManagerMKII.UpdateStatus))
		{
			updatestyle = new GUIStyle(GUI.skin.label);
			updatestyle.font = FengGameManagerMKII.ShentoxLight;
			GUI.Label(new Rect(Screen.width - 270, Screen.height - 30, 300f, 20f), FengGameManagerMKII.UpdateStatus, updatestyle);
		}
		ButtonStyles();
		Rect rect = new Rect(-200f + _buttonsIncWidth, Screen.height - 250, _buttonsIncWidth, 35f);
		GUI.DrawTexture(new Rect(rect.x + 199f, rect.y, 2f, rect.height - 5f), _red);
		GUI.DrawTexture(new Rect(rect.x - 2f, rect.y, rect.width, 1f), _red);
		GUI.DrawTexture(new Rect(rect.x - 2f, rect.y + rect.height - 5f, rect.width, 1f), _red);
		if (GUI.Button(new Rect(rect.x - 5f, rect.y, 205f, 30f), "<color=#ffa07a>Multiplayer</color>", _buttonstyles))
		{
			StartCoroutine(HideTextures(1));
		}
		Rect rect2 = new Rect(-200f + _buttonsIncWidth, Screen.height - 200, _buttonsIncWidth, 35f);
		GUI.DrawTexture(new Rect(rect2.x + 199f, rect2.y, 2f, rect2.height - 5f), _red);
		GUI.DrawTexture(new Rect(rect2.x - 2f, rect2.y, rect2.width, 1f), _red);
		GUI.DrawTexture(new Rect(rect2.x - 2f, rect2.y + rect2.height - 5f, rect2.width, 1f), _red);
		if (GUI.Button(new Rect(rect2.x - 5f, rect2.y, 205f, 30f), "<color=#ffa07a>Offline</color>", _buttonstyles))
		{
			StartCoroutine(HideTextures(2));
		}
		Rect rect3 = new Rect(-200f + _buttonsIncWidth, Screen.height - 150, _buttonsIncWidth, 35f);
		GUI.DrawTexture(new Rect(rect3.x + 199f, rect3.y, 2f, rect3.height - 5f), _red);
		GUI.DrawTexture(new Rect(rect3.x - 2f, rect3.y, rect3.width, 1f), _red);
		GUI.DrawTexture(new Rect(rect3.x - 2f, rect3.y + rect3.height - 5f, rect3.width, 1f), _red);
		if (GUI.Button(new Rect(rect3.x - 5f, rect3.y, 205f, 30f), "<color=#ffa07a>Singleplayer</color>", _buttonstyles))
		{
			StartCoroutine(HideTextures(3));
		}
		Rect rect4 = new Rect((float)Screen.width - _buttonsIncWidth, Screen.height - 200, _buttonsIncWidth, 35f);
		GUI.DrawTexture(new Rect(rect4.x - 2f, rect4.y, 2f, rect4.height - 5f), _red);
		GUI.DrawTexture(new Rect(rect4.x, rect4.y, rect4.width, 1f), _red);
		GUI.DrawTexture(new Rect(rect4.x - 1f, rect4.y + rect4.height - 5f, rect4.width + 1f, 1f), _red);
		if (GUI.Button(new Rect(rect4.x, rect4.y, 205f, 30f), "<color=#ffa07a>Character Creation</color>", _buttonstyles))
		{
			StartCoroutine(HideTextures(4));
		}
		Rect rect5 = new Rect((float)Screen.width - _buttonsIncWidth, Screen.height - 150, _buttonsIncWidth, 35f);
		GUI.DrawTexture(new Rect(rect5.x - 2f, rect5.y, 2f, rect5.height - 5f), _red);
		GUI.DrawTexture(new Rect(rect5.x, rect5.y, rect5.width, 1f), _red);
		GUI.DrawTexture(new Rect(rect5.x - 1f, rect5.y + rect5.height - 5f, rect5.width + 1f, 1f), _red);
		if (GUI.Button(new Rect(rect5.x, rect5.y, 205f, 30f), "<color=#ffa07a>Snapshots</color>", _buttonstyles))
		{
			StartCoroutine(HideTextures(5));
		}
		PlayerOptions();
	}

	private void Style()
	{
		if (currentStyle == null)
		{
			currentStyle = new GUIStyle(GUI.skin.box);
			currentStyle.alignment = TextAnchor.MiddleLeft;
			currentStyle.wordWrap = true;
			currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.6f));
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

	private void PlayerOptions()
	{
		Style();
		LPanelEyeButton();
		LPanelProfButton();
		LPanelServButton();
		Rect rect = new Rect(-160f + _leftPanelIncWidth, 270f, _leftPanelIncWidth, 300f);
		GUI.Box(new Rect(rect.x - 140f, 270f, _leftPanelIncWidth + 4f, 260f), "", currentStyle);
		GUI.DrawTexture(new Rect(rect.x + 179f, rect.y, 2f, 117f), _red);
		if (GUI.Button(new Rect(10f + _leftPanelIncWidth, rect.y + 120f, 20f, 20f), "", LPanelOpenButton))
		{
			if (!lpanel_enlarge.isPlaying)
			{
				//lpanel_enlarge.Play();
			}
			EnlargePanel = !EnlargePanel;
		}
		GUI.DrawTexture(new Rect(rect.x + 179f, rect.y + 143f, 2f, rect.height - 185f), _red);
		if (GUI.Button(new Rect(rect.x + 30f, rect.y + 5f, 30f, 30f), "", LPanelProfileButton))
		{
			if (!OpenProfile)
			{
				OpenProfile = true;
			}
			if (OpenServers)
			{
				OpenServers = false;
			}
		}
		if (GUI.Button(new Rect(rect.x - 10f, rect.y + 5f, 30f, 30f), "", LPanelServersButton))
		{
			if (OpenProfile)
			{
				OpenProfile = false;
			}
			if (!OpenServers)
			{
				OpenServers = true;
			}
		}
		if (OpenProfile)
		{
			GUI.Label(new Rect(rect.x - 130f, rect.y + 55f, 60f, 20f), "<b><i><size=14>Name</size></i></b>");
			if ((int)FengGameManagerMKII.settings[359] == 0)
			{
				FengGameManagerMKII.nameField = GUI.TextField(new Rect(rect.x - 55f, rect.y + 55f, 215f, 20f), FengGameManagerMKII.nameField, 150);
			}
			else if ((int)FengGameManagerMKII.settings[359] == 1)
			{
				FengGameManagerMKII.nameField2 = GUI.TextField(new Rect(rect.x - 55f, rect.y + 55f, 215f, 20f), FengGameManagerMKII.nameField2, 150);
			}
			GUI.Label(new Rect(rect.x - 130f, rect.y + 80f, 80f, 20f), "<b><i><size=14>Guild</size></i></b>");
			if ((int)FengGameManagerMKII.settings[359] == 0)
			{
				FengGameManagerMKII.guildField = GUI.TextField(new Rect(rect.x - 55f, rect.y + 80f, 215f, 20f), FengGameManagerMKII.guildField, 180);
			}
			else if ((int)FengGameManagerMKII.settings[359] == 1)
			{
				FengGameManagerMKII.guildField2 = GUI.TextField(new Rect(rect.x - 55f, rect.y + 80f, 215f, 20f), FengGameManagerMKII.guildField2, 180);
			}
			GUI.Label(new Rect(rect.x - 130f, rect.y + 105f, 80f, 20f), "<b><i><size=14>Chatname</size></i></b>");
			if ((int)FengGameManagerMKII.settings[359] == 0)
			{
				FengGameManagerMKII.Chatname = GUI.TextField(new Rect(rect.x - 55f, rect.y + 105f, 215f, 20f), FengGameManagerMKII.Chatname);
			}
			else if ((int)FengGameManagerMKII.settings[359] == 1)
			{
				FengGameManagerMKII.Chatname2 = GUI.TextField(new Rect(rect.x - 55f, rect.y + 105f, 215f, 20f), FengGameManagerMKII.Chatname2);
			}
			GUI.Label(new Rect(rect.x - 130f, rect.y + 155f, 100f, 20f), "<b><i><size=14>Chat color</size></i></b>");
			if ((int)FengGameManagerMKII.settings[359] == 0)
			{
				FengGameManagerMKII.Chatcolor = GUI.TextField(new Rect(rect.x - 55f, rect.y + 155f, 55f, 20f), FengGameManagerMKII.Chatcolor);
			}
			else if ((int)FengGameManagerMKII.settings[359] == 1)
			{
				FengGameManagerMKII.Chatcolor2 = GUI.TextField(new Rect(rect.x - 55f, rect.y + 155f, 55f, 20f), FengGameManagerMKII.Chatcolor2);
			}
			GUI.Label(new Rect(rect.x + 12f, rect.y + 155f, 100f, 20f), "<b><i><size=13>Prefix</size></i></b>");
			if ((int)FengGameManagerMKII.settings[359] == 0)
			{
				FengGameManagerMKII.Prefix = GUI.TextField(new Rect(rect.x + 50f, rect.y + 155f, 30f, 20f), FengGameManagerMKII.Prefix);
			}
			else if ((int)FengGameManagerMKII.settings[359] == 1)
			{
				FengGameManagerMKII.Prefix2 = GUI.TextField(new Rect(rect.x + 50f, rect.y + 155f, 30f, 20f), FengGameManagerMKII.Prefix2);
			}
			GUI.Label(new Rect(rect.x + 90f, rect.y + 155f, 100f, 20f), "<b><i><size=13>Suffix</size></i></b>");
			if ((int)FengGameManagerMKII.settings[359] == 0)
			{
				FengGameManagerMKII.Suffix = GUI.TextField(new Rect(rect.x + 130f, rect.y + 155f, 30f, 20f), FengGameManagerMKII.Suffix);
			}
			else if ((int)FengGameManagerMKII.settings[359] == 1)
			{
				FengGameManagerMKII.Suffix2 = GUI.TextField(new Rect(rect.x + 130f, rect.y + 155f, 30f, 20f), FengGameManagerMKII.Suffix2);
			}
			GUI.Label(new Rect(rect.x - 130f, rect.y + 180f, 100f, 20f), "<b><i><size=13>Bomb Name</size></i></b>");
			if ((int)FengGameManagerMKII.settings[359] == 0)
			{
				FengGameManagerMKII.settings[328] = GUI.TextField(new Rect(rect.x - 55f, rect.y + 180f, 215f, 20f), (string)FengGameManagerMKII.settings[328]);
			}
			else if ((int)FengGameManagerMKII.settings[359] == 1)
			{
				FengGameManagerMKII.settings[337] = GUI.TextField(new Rect(rect.x - 55f, rect.y + 180f, 215f, 20f), (string)FengGameManagerMKII.settings[337]);
			}
			string[] texts = new string[2] { "<size=13>Set 1</size>", "<size=13>Set 2</size>" };
			FengGameManagerMKII.settings[359] = GUI.SelectionGrid(new Rect(rect.x - 55f, rect.y + 210f, 215f, 20f), (int)FengGameManagerMKII.settings[359], texts, 2);
			PlayerPrefs.SetInt("NameSets", (int)FengGameManagerMKII.settings[359]);
			if (GUI.Button(new Rect(rect.x + 80f, rect.y + 235f, 80f, 20f), "Save"))
			{
				PlayerPrefs.SetString("name", FengGameManagerMKII.nameField);
				PlayerPrefs.SetString("name2", FengGameManagerMKII.nameField2);
				PlayerPrefs.SetString("guildname", FengGameManagerMKII.guildField);
				PlayerPrefs.SetString("guildname2", FengGameManagerMKII.guildField2);
				PlayerPrefs.SetString("Cname", FengGameManagerMKII.Chatname);
				PlayerPrefs.SetString("Cname2", FengGameManagerMKII.Chatname2);
				PlayerPrefs.SetString("chatcolor", FengGameManagerMKII.Chatcolor);
				PlayerPrefs.SetString("chatcolor2", FengGameManagerMKII.Chatcolor2);
				PlayerPrefs.SetString("Prefix", FengGameManagerMKII.Prefix);
				PlayerPrefs.SetString("Prefix2", FengGameManagerMKII.Prefix2);
				PlayerPrefs.SetString("Suffix", FengGameManagerMKII.Suffix);
				PlayerPrefs.SetString("Suffix2", FengGameManagerMKII.Suffix2);
				PlayerPrefs.SetString("appid", FengGameManagerMKII.privateAppIDField);
				PlayerPrefs.SetString("BombName", (string)FengGameManagerMKII.settings[328]);
				PlayerPrefs.SetString("BombName2", (string)FengGameManagerMKII.settings[337]);
				PlayerPrefs.SetInt("servertype", FengGameManagerMKII.ServerType);
			}
		}
		else
		{
			if (!OpenServers)
			{
				return;
			}
			GUI.Label(new Rect(rect.x - 130f, rect.y + 55f, 60f, 20f), "<b><i><size=14>Region</size></i></b>");
			GUILayout.BeginArea(new Rect(rect.x - 55f, rect.y + 45f, 195f, 45f), "");
			RegionsScroll = GUILayout.BeginScrollView(RegionsScroll);
			GUILayout.BeginHorizontal();
			string[] array = new string[13]
			{
				((int)FengGameManagerMKII.settings[293] == 0) ? "<color=red>EU</color>" : "EU",
				((int)FengGameManagerMKII.settings[293] == 1) ? "<color=red>US (East)</color>" : "US (East)",
				((int)FengGameManagerMKII.settings[293] == 2) ? "<color=red>Brazil</color>" : "Brazil",
				((int)FengGameManagerMKII.settings[293] == 3) ? "<color=red>Asia</color>" : "Asia",
				((int)FengGameManagerMKII.settings[293] == 4) ? "<color=red>Turkiye</color>" : "Turkiye",
				((int)FengGameManagerMKII.settings[293] == 5) ? "<color=red>Australia</color>" : "Australia",
				((int)FengGameManagerMKII.settings[293] == 6) ? "<color=red>Canada</color>" : "Canada",
				((int)FengGameManagerMKII.settings[293] == 7) ? "<color=red>US (West)</color>" : "US (West)",
				((int)FengGameManagerMKII.settings[293] == 8) ? "<color=red>US (S.Central)</color>" : "US (S.Central)",
				((int)FengGameManagerMKII.settings[293] == 9) ? "<color=red>India</color>" : "India",
				((int)FengGameManagerMKII.settings[293] == 10) ? "<color=red>Japan</color>" : "Japan",
				((int)FengGameManagerMKII.settings[293] == 11) ? "<color=red>S.Korea</color>" : "S.Korea",
				((int)FengGameManagerMKII.settings[293] == 12) ? "<color=red>S.Africa</color>" : "S.Africa"
			};
			if (FengGameManagerMKII.LanOrAppID == 0)
			{
				for (int i = 0; i < 4; i++)
				{
					if (GUILayout.Button(array[i], GUILayout.MaxWidth(500f), GUILayout.MaxHeight(20f)))
					{
						FengGameManagerMKII.settings[293] = i;
					}
				}
			}
			else if (FengGameManagerMKII.LanOrAppID == 2)
			{
				for (int j = 0; j < 13; j++)
				{
					if (GUILayout.Button(array[j], GUILayout.MaxWidth(500f), GUILayout.MaxHeight(20f)))
					{
						FengGameManagerMKII.settings[293] = j;
					}
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUI.Label(new Rect(rect.x - 130f, rect.y + 100f, 60f, 20f), "<b><i><size=14>Servers</size></i></b>");
			string[] array2 = new string[3] { "Public", "Private", "Custom" };
			FengGameManagerMKII.ServerType = GUI.SelectionGrid(new Rect(rect.x - 55f, rect.y + 100f, 195f, 20f), FengGameManagerMKII.ServerType, array2, 3);
			if (FengGameManagerMKII.ServerType == 2)
			{
				FengGameManagerMKII.privateServerField = GUI.TextField(new Rect(rect.x + 78f, rect.y + 125f, 62f, 20f), FengGameManagerMKII.privateServerField, 50);
			}
			GUI.Label(new Rect(rect.x - 130f, rect.y + 150f, 130f, 20f), "<b><i><size=14>Address</size></i></b>");
			string[] texts2 = new string[3] { "Normal", "LAN", "AppID" };
			FengGameManagerMKII.LanOrAppID = GUI.SelectionGrid(new Rect(rect.x - 55f, rect.y + 150f, 195f, 20f), FengGameManagerMKII.LanOrAppID, texts2, 3);
			if (FengGameManagerMKII.LanOrAppID == 1)
			{
				FengGameManagerMKII.LANField = GUI.TextField(new Rect(rect.x + 10f, rect.y + 180f, 130f, 20f), FengGameManagerMKII.LANField);
			}
			else if (FengGameManagerMKII.LanOrAppID == 2)
			{
				FengGameManagerMKII.privateAppIDField = GUI.TextField(new Rect(rect.x + 10f, rect.y + 180f, 130f, 20f), FengGameManagerMKII.privateAppIDField);
				if (FengGameManagerMKII.applicationId != FengGameManagerMKII.privateAppIDField && FengGameManagerMKII.privateAppIDField != null && FengGameManagerMKII.privateAppIDField.Length == 36)
				{
					PlayerPrefs.SetString("appid", FengGameManagerMKII.privateAppIDField);
					FengGameManagerMKII.applicationId = FengGameManagerMKII.privateAppIDField;
				}
			}
			GUI.Label(new Rect(rect.x - 55f, rect.y + 205f, 200f, 35f), "<b>Connected to " + array[(int)FengGameManagerMKII.settings[293]] + " " + array2[FengGameManagerMKII.ServerType] + ((FengGameManagerMKII.ServerType == 2) ? ("(" + FengGameManagerMKII.privateServerField + ")") : "") + ", " + ((FengGameManagerMKII.LanOrAppID == 1) ? "LAN" : ((FengGameManagerMKII.LanOrAppID == 2) ? "AppID" : "Normal")) + "</b>");
		}
	}

	private void Update()
	{
		if (!isVisible)
		{
			return;
		}
		if (_buttonsIncWidth < 200f && !close)
		{
			if (menu_open != null && !menu_open.isPlaying)
			{
				//menu_open.Play();
			}
			_buttonsIncWidth += (200f + _buttonsIncWidth * 0.75f) * Time.deltaTime;
		}
		if (_buttonsIncWidth > 0f && close)
		{
			_buttonsIncWidth -= (600f + _buttonsIncWidth * -2f) * Time.deltaTime;
		}
		if (EnlargePanel && !close)
		{
			if (_leftPanelIncWidth < 300f && !close)
			{
				_leftPanelIncWidth += (375f + _leftPanelIncWidth * 0.75f) * Time.deltaTime;
			}
		}
		else if (_leftPanelIncWidth > 0f && !close)
		{
			_leftPanelIncWidth -= (900f + _leftPanelIncWidth * -2f) * Time.deltaTime;
		}
		if (_leftPanelIncWidth > 0f && close)
		{
			_leftPanelIncWidth -= (900f + _leftPanelIncWidth * -2f) * Time.deltaTime;
		}
	}
}
