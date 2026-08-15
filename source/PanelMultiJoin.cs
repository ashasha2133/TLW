using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;
using Utility;

public class PanelMultiJoin : MonoBehaviour
{
	public enum ROOM_PARAMS : uint
	{
		Name,
		Map,
		Difficulty,
		Time,
		Daylight,
		EncryptedPass,
		RandomNumber
	}

	private int currentPage = 1;

	private string filter = string.Empty;

	public GameObject[] items;

	public bool HideFullServers;

	public bool HidePassServers;

	public bool HideClosedServers;

	public bool HideRMaxServers;

	private Vector2 scroll = Vector2.zero;

	private Vector2 RegionsScroll = Vector2.zero;

	private void Start()
	{
		int num = 0;
		for (num = 0; num < 10; num++)
		{
			items[num].SetActive(value: true);
			items[num].GetComponentInChildren<UILabel>().text = string.Empty;
			items[num].SetActive(value: false);
		}
		base.transform.Find("InputFilter").gameObject.SetActive(value: false);
		base.transform.Find("ButtonPgDn").gameObject.SetActive(value: false);
		base.transform.Find("ButtonPgUp").gameObject.SetActive(value: false);
		base.transform.Find("ButtonRefresh").gameObject.SetActive(value: false);
		base.transform.Find("itemList").gameObject.SetActive(value: false);
		base.transform.Find("LabelName").gameObject.SetActive(value: false);
		base.transform.Find("ButtonBACK").gameObject.SetActive(value: false);
		base.transform.Find("ButtonCreate").gameObject.SetActive(value: false);
	}

	public void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), FengGameManagerMKII.textureBackgroundBlack);
		GUI.Window(12, new Rect(10f, 20f, Screen.width - 300, Screen.height - 30), ServerList, "");
		GUI.Window(13, new Rect(Screen.width - 280, 20f, 270f, 165f), ServerInfo, "<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">INFO</color></b>");
		GUI.Window(14, new Rect(Screen.width - 280, 200f, 270f, 155f), ServerSettings, "<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Server Settings</color></b>");
	}

	public void ServerSettings(int id3)
	{
		GUILayout.BeginVertical(GUI.skin.box);
		GUILayout.BeginHorizontal();
		HideFullServers = GUILayout.Toggle(HideFullServers, "", GUILayout.Width(17f), GUILayout.Height(17f));
		GUILayout.Label("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">Hide full rooms</color>");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		HidePassServers = GUILayout.Toggle(HidePassServers, "", GUILayout.Width(16f), GUILayout.Height(16f));
		GUILayout.Label("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">Hide passworded rooms</color>");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		HideClosedServers = GUILayout.Toggle(HideClosedServers, "", GUILayout.Width(16f), GUILayout.Height(16f));
		GUILayout.Label("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">Hide closed rooms</color>");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		HideRMaxServers = GUILayout.Toggle(HideRMaxServers, "", GUILayout.Width(15f), GUILayout.Height(15f));
		GUILayout.Label("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">Hide 0 and x>25 maxplayers rooms</color>");
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	public void ServerInfo(int id3)
	{
		GUILayout.BeginVertical(GUI.skin.box);
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">RC:</color></b> " + ((UIMainReferences.version == UIMainReferences.fengVersion) ? "Public" : ((UIMainReferences.version == FengGameManagerMKII.s[0]) ? "Private" : "Custom")));
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Region:</color></b> " + TLWWebSocket.Region);
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Total Servers:</color></b> " + PhotonNetwork.countOfRooms);
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Players Online:</color></b> " + PhotonNetwork.countOfPlayersOnMaster);
		GUILayout.Label("<b><color=#" + FengGameManagerMKII.alternativefinalhex() + ">Players In-Game:</color></b> " + PhotonNetwork.countOfPlayersInRooms);
		GUILayout.EndVertical();
	}

	private string AllRegions(string AskingRegion)
	{
		string result = "";
		switch (AskingRegion)
		{
		case "eu":
			if (MyButtons.gameServerSets == 0) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "135.125.239.180" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-eu.exitgamescloud.com"));
			else if(MyButtons.gameServerSets == 1) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "eu.aottg2.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-eu.exitgamescloud.com"));
                break;
		case "us":
			if(MyButtons.gameServerSets == 0) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "142.44.242.29" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-us.exitgamescloud.com"));
			else if (MyButtons.gameServerSets == 1) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "us.aottg2.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-us.exitgamescloud.com"));
			break;
		case "sa":
			if(MyButtons.gameServerSets == 0) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "108.181.69.221" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-sa.exitgames.com"));
            else if (MyButtons.gameServerSets == 1) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "sa.aottg2.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-sa.exitgames.com"));
			break;
		case "asia":
			if (MyButtons.gameServerSets == 0) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "51.79.164.137" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-asia.exitgamescloud.com"));
            else if (MyButtons.gameServerSets == 1) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "asia.aottg2.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-asia.exitgamescloud.com"));
			break;
		case "cn":
			if (MyButtons.gameServerSets == 0) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "47.116.117.128" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-asia.exitgamescloud.com"));
            else if (MyButtons.gameServerSets == 1) result = ((FengGameManagerMKII.LanOrAppID == 0) ? "cn.aottg2.com" : ((FengGameManagerMKII.LanOrAppID == 1) ? FengGameManagerMKII.LANField : "app-asia.exitgamescloud.com"));
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

	private void ConnectToPorts(string region)
	{
		if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Tcp)
		{
			PhotonNetwork.ConnectToMaster(region, 4530, FengGameManagerMKII.applicationId, UIMainReferences.version);
		}
		else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.Udp)
		{
			PhotonNetwork.ConnectToMaster(region, 5055, FengGameManagerMKII.applicationId, UIMainReferences.version);
		}
		else if (PhotonNetwork.networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket)
		{
			PhotonNetwork.ConnectToMaster(region, 9090, FengGameManagerMKII.applicationId, UIMainReferences.version);
		}
	}

	private IEnumerator changeRegion(string region)
	{
		if (PhotonNetwork.connected)
		{
			PhotonNetwork.Disconnect();
			FengGameManagerMKII.OnPrivateServer = true;
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
		}
		yield return new WaitForSeconds(0.2f);
		if (!PhotonNetwork.connected)
		{
			switch (region)
			{
			case "EU":
				ConnectToPorts(AllRegions("eu"));
				TLWWebSocket.Region = "Europe";
				break;
			case "US":
				ConnectToPorts(AllRegions("us"));
				TLWWebSocket.Region = "United States";
				break;
			case "SA":
				ConnectToPorts(AllRegions("sa"));
				TLWWebSocket.Region = "South America";
				break;
			case "Asia":
				ConnectToPorts(AllRegions("asia"));
				TLWWebSocket.Region = "Asia";
				break;
			case "CN":
				ConnectToPorts(AllRegions("cn"));
				TLWWebSocket.Region = "China";
				break;
			case "Turkiye":
				ConnectToPorts(AllRegions("turkey"));
				TLWWebSocket.Region = "Turkiye";
				break;
			case "Australia":
				ConnectToPorts(AllRegions("australia"));
				TLWWebSocket.Region = "Australia";
				break;
			case "Canada":
				ConnectToPorts(AllRegions("canada"));
				TLWWebSocket.Region = "Canada";
				break;
			case "US (West)":
				ConnectToPorts(AllRegions("uswest"));
				TLWWebSocket.Region = "US West";
				break;
			case "US (S.Central)":
				ConnectToPorts(AllRegions("ussouth"));
				TLWWebSocket.Region = "US Central South";
				break;
			case "India":
				ConnectToPorts(AllRegions("india"));
				TLWWebSocket.Region = "India";
				break;
			case "Japan":
				ConnectToPorts(AllRegions("japan"));
				TLWWebSocket.Region = "Japan";
				break;
			case "S.Korea":
				ConnectToPorts(AllRegions("korea"));
				TLWWebSocket.Region = "South Korea";
				break;
			case "S.Africa":
				ConnectToPorts(AllRegions("safrica"));
				TLWWebSocket.Region = "South Africa";
				break;
			}
		}
	}

	public void ServerList(int id3)
	{
		GUILayout.BeginHorizontal(GUI.skin.box);
		GUILayout.Label("<b>Ping: " + PhotonNetwork.GetPing() + "</b>", GUILayout.Width(65f));
		GUILayout.Label("Search:", GUILayout.Width(50f));
		filter = GUILayout.TextField(filter, GUILayout.Width(200f));
		GUILayout.Label("", GUILayout.Width(5f));
		string[] array = new string[14]
		{
			(TLWWebSocket.Region == "Europe") ? "<color=red>EU</color>" : "EU",
			(TLWWebSocket.Region == "United States") ? "<color=red>US</color>" : "US",
			(TLWWebSocket.Region == "South America") ? "<color=red>SA</color>" : "SA",
			(TLWWebSocket.Region == "Asia") ? "<color=red>Asia</color>" : "Asia",
			(TLWWebSocket.Region == "China") ? "<color=red>CN</color>" : "CN",
			(TLWWebSocket.Region == "Turkiye, Istanbul") ? "<color=red>Turkiye</color>" : "Turkiye",
			(TLWWebSocket.Region == "Australia, Melbourne") ? "<color=red>Australia</color>" : "Australia",
			(TLWWebSocket.Region == "Canada, Montreal") ? "<color=red>Canada</color>" : "Canada",
			(TLWWebSocket.Region == "US West, San José") ? "<color=red>US (West)</color>" : "US (West)",
			(TLWWebSocket.Region == "US Central South, Dallas") ? "<color=red>US (S.Central)</color>" : "US (S.Central)",
			(TLWWebSocket.Region == "India, Chennai") ? "<color=red>India</color>" : "India",
			(TLWWebSocket.Region == "Japan, Tokyo") ? "<color=red>Japan</color>" : "Japan",
			(TLWWebSocket.Region == "South Korea, Seoul") ? "<color=red>S.Korea</color>" : "S.Korea",
			(TLWWebSocket.Region == "South Africa, Johannesburg") ? "<color=red>S.Africa</color>" : "S.Africa"
		};
		RegionsScroll = GUILayout.BeginScrollView(RegionsScroll, GUILayout.MaxHeight(45f));
		GUILayout.BeginHorizontal();
		if (FengGameManagerMKII.LanOrAppID == 0)
		{
			for (int i = 0; i < 5; i++)
			{
				if (GUILayout.Button(array[i], GUILayout.MaxWidth(500f), GUILayout.MaxHeight(20f)))
				{
					StartCoroutine(changeRegion(array[i]));
				}
			}
		}
		else if (FengGameManagerMKII.LanOrAppID == 2)
		{
			for (int j = 0; j < 13; j++)
			{
				if (GUILayout.Button(array[j], GUILayout.MaxWidth(500f), GUILayout.MaxHeight(20f)))
				{
					StartCoroutine(changeRegion(array[j]));
				}
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
		GUILayout.Label("", GUILayout.Height(5f));
		GUILayout.BeginHorizontal();
		scroll = GUILayout.BeginScrollView(scroll, GUI.skin.box);
		GUI.backgroundColor = Color.clear;
		if (PhotonNetwork.countOfRooms == 0)
		{
			GUILayout.Label("No servers available!", GUILayout.Width(150f));
		}
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		foreach (RoomInfo roomInfo in roomList)
		{
			string[] array2 = roomInfo.name.Split('`');
			string text = array2[0].ToString().StripHex();
			string text2 = array2[1].ToString().StripHex();
			string text3 = array2[2].ToString().StripHex();
			if ((array2.Length == 7 && text.Length <= 300 && text2.Length > 1 && text3.Length > 1) || true)
			{
				string text4 = array2[0].ToString();
				ThymesUtils.GetAllBracketHexOccurrences(text4);
				string text5 = "<size=27>" + text4.hexColor() + "</size>";
				if (text5.Length > 300)
				{
					text5 = text5.Remove(250).StripColor() + "...";
				}
				if (GUILayout.Button(string.Concat(new object[15]
				{
					(array2[5] != "") ? "<size=15><color=#f51282><b>[PWD]  </b></color>" : "<size=15>",
					(text5.Length <= 540) ? text5 : "<color=#FF0000>Absolute Unit of a server name.</color>",
					" <b>|</b> ",
					(array2[1].ToString().hexColor().Length <= 100) ? array2[1].ToString().hexColor() : "<color=#FF0000>Absolute Unit of a map name.</color>",
					" <b>|</b> ",
					array2[2].ToString().hexColor(),
					" <b>|</b> ",
					array2[4].ToString().hexColor(),
					" <b>|</b> (",
					roomInfo.playerCount.ToString().hexColor(),
					"/",
					roomInfo.maxPlayers.ToString().hexColor(),
					")",
					roomInfo.open ? " <color=lime><b></b></color>" : " <b>::</b> <color=red><b>Closed</b></color>",
					(array2[6].ToString().Length == 10 && array2[5] == "" && roomInfo.maxPlayers == byte.MaxValue) ? "<color=#ffbdf8> (My bot)</color></size>" : "</size>"
				}), GUILayout.Height(40f)))
				{
                    connectToIndex(5, roomInfo.name);
				}
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUI.backgroundColor = Color.cyan;
		if (GUILayout.Button("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">Create Game</color>", GUILayout.Height(50f)))
		{
			NGUITools.SetActive(base.gameObject, state: false);
			NGUITools.SetActive(UIMainReferences.UIRefer.panelMultiSet, state: true);
		}
		GUILayout.Space(1f);
		if (GUILayout.Button("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">Back</color>", GUILayout.Height(50f)))
		{
			NGUITools.SetActive(base.gameObject, state: false);
			GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
			Object.Destroy(GameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
			PhotonNetwork.Disconnect();
		}
		GUILayout.EndHorizontal();
	}

	public void connectToIndex(int index, string roomName)
	{
		int num = 0;
		for (num = 0; num < 10; num++)
		{
			items[num].SetActive(value: false);
		}
		num = 10 * (currentPage - 1) + index;
		char[] separator = new char[1] { "`"[0] };
		string[] array = roomName.Split(separator);
		if ((array[5] != string.Empty && false) || false)
		{
			PanelMultiJoinPWD.Password = array[5];
			PanelMultiJoinPWD.roomName = roomName;
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiPWD, state: true);
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, state: false);
		}
		else
		{
            PhotonNetwork.JoinRoom(roomName);
		}
	}
}
