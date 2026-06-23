using System;
using System.Collections.Generic;
using BRM;
using Events;
using ExitGames.Client.Photon;
using UnityEngine;

public static class PhotonNetwork
{
	public delegate void EventCallback(byte eventCode, object content, int senderId);

	private static bool _mAutomaticallySyncScene;

	private static bool autoJoinLobbyField;

	public static bool InstantiateInRoomOnly;

	private static bool isOfflineMode;

	internal static int lastUsedViewSubId;

	internal static int lastUsedViewSubIdStatic;

	public static PhotonLogLevel logLevel;

	private static bool m_autoCleanUpPlayerObjects;

	private static bool m_isMessageQueueRunning;

	internal static List<int> manuallyAllocatedViewIds;

	public static readonly int MAX_VIEW_IDS;

	internal static NetworkingPeer networkingPeer;

	private static Room offlineModeRoom;

	public static EventCallback OnEventCall;

	internal static readonly PhotonHandler photonMono;

	public static ServerSettings PhotonServerSettings;

	public static float precisionForFloatSynchronization;

	public static float precisionForQuaternionSynchronization;

	public static float precisionForVectorSynchronization;

	private static int sendInterval;

	private static int sendIntervalOnSerialize;

	public static HashSet<GameObject> SendMonoMessageTargets;

	public const string serverSettingsAssetFile = "PhotonServerSettings";

	public const string serverSettingsAssetPath = "Assets/Photon Unity Networking/Resources/PhotonServerSettings.asset";

	public static bool UseNameServer;

	public static bool UsePrefabCache;

	public const string versionPUN = "1.28";

	public static PeerStates connectionStatesDetailed
	{
		get
		{
			if (offlineMode)
			{
				if (offlineModeRoom != null)
				{
					return PeerStates.Joined;
				}
				return PeerStates.ConnectedToMaster;
			}
			return networkingPeer.states;
		}
	}

	public static AuthenticationValues AuthValues
	{
		get
		{
			if (networkingPeer != null)
			{
				return networkingPeer.CustomAuthenticationValues;
			}
			return null;
		}
		set
		{
			if (networkingPeer != null)
			{
				networkingPeer.CustomAuthenticationValues = value;
			}
		}
	}

	public static bool autoCleanUpPlayerObjects
	{
		get
		{
			return m_autoCleanUpPlayerObjects;
		}
		set
		{
			if (room == null)
			{
				m_autoCleanUpPlayerObjects = value;
			}
		}
	}

	public static bool autoJoinLobby
	{
		get
		{
			return autoJoinLobbyField;
		}
		set
		{
			autoJoinLobbyField = value;
		}
	}

	public static bool automaticallySyncScene
	{
		get
		{
			return _mAutomaticallySyncScene;
		}
		set
		{
			_mAutomaticallySyncScene = value;
			if (_mAutomaticallySyncScene && room != null)
			{
				networkingPeer.LoadLevelIfSynced();
			}
		}
	}

	public static bool connected
	{
		get
		{
			if (offlineMode)
			{
				return true;
			}
			if (networkingPeer == null)
			{
				return false;
			}
			if (!networkingPeer.IsInitialConnect && networkingPeer.states != PeerStates.PeerCreated && networkingPeer.states != PeerStates.Disconnected && networkingPeer.states != PeerStates.Disconnecting)
			{
				return networkingPeer.states != PeerStates.ConnectingToNameServer;
			}
			return false;
		}
	}

	public static bool connectedAndReady
	{
		get
		{
			if (!connected)
			{
				return false;
			}
			if (!offlineMode)
			{
				switch (connectionStateDetailed)
				{
				case PeerStates.PeerCreated:
				case PeerStates.ConnectingToGameserver:
				case PeerStates.Joining:
				case PeerStates.Leaving:
				case PeerStates.ConnectingToMasterserver:
				case PeerStates.Disconnecting:
				case PeerStates.Disconnected:
				case PeerStates.ConnectingToNameServer:
				case PeerStates.Authenticating:
					return false;
				}
			}
			return true;
		}
	}

	public static bool connecting
	{
		get
		{
			if (networkingPeer.IsInitialConnect)
			{
				return !offlineMode;
			}
			return false;
		}
	}

	public static ConnectionState connectionState
	{
		get
		{
			if (offlineMode)
			{
				return ConnectionState.Connected;
			}
			if (networkingPeer != null)
			{
				switch (networkingPeer.PeerState)
				{
				case PeerStateValue.Disconnected:
					return ConnectionState.Disconnected;
				case PeerStateValue.Connecting:
					return ConnectionState.Connecting;
				case PeerStateValue.Connected:
					return ConnectionState.Connected;
				case PeerStateValue.Disconnecting:
					return ConnectionState.Disconnecting;
				case PeerStateValue.InitializingApplication:
					return ConnectionState.InitializingApplication;
				}
			}
			return ConnectionState.Disconnected;
		}
	}

	public static PeerStates connectionStateDetailed
	{
		get
		{
			if (offlineMode)
			{
				if (offlineModeRoom != null)
				{
					return PeerStates.Joined;
				}
				return PeerStates.ConnectedToMaster;
			}
			if (networkingPeer == null)
			{
				return PeerStates.Disconnected;
			}
			return networkingPeer.states;
		}
	}

	public static int countOfPlayers => networkingPeer.mPlayersInRoomsCount + networkingPeer.mPlayersOnMasterCount;

	public static int countOfPlayersInRooms => networkingPeer.mPlayersInRoomsCount;

	public static int countOfPlayersOnMaster => networkingPeer.mPlayersOnMasterCount;

	public static int countOfRooms => networkingPeer.mGameCount;

	public static bool CrcCheckEnabled
	{
		get
		{
			return networkingPeer.CrcEnabled;
		}
		set
		{
			if (!connected && !connecting)
			{
				networkingPeer.CrcEnabled = value;
			}
		}
	}

	public static List<FriendInfo> Friends { get; set; }

	public static int FriendsListAge
	{
		get
		{
			if (networkingPeer != null)
			{
				return networkingPeer.FriendsListAge;
			}
			return 0;
		}
	}

	public static string gameVersion
	{
		get
		{
			return networkingPeer.mAppVersion;
		}
		set
		{
			networkingPeer.mAppVersion = value;
		}
	}

	public static bool inRoom => connectionStateDetailed == PeerStates.Joined;

	public static bool insideLobby => networkingPeer.insideLobby;

	public static bool isMasterClient
	{
		get
		{
			if (!offlineMode)
			{
				return networkingPeer.mMasterClient == networkingPeer.mLocalActor;
			}
			return true;
		}
	}

	public static bool isMessageQueueRunning
	{
		get
		{
			return m_isMessageQueueRunning;
		}
		set
		{
			if (value)
			{
				PhotonHandler.StartFallbackSendAckThread();
			}
			networkingPeer.IsSendingOnlyAcks = !value;
			m_isMessageQueueRunning = value;
		}
	}

	public static bool isNonMasterClientInRoom
	{
		get
		{
			if (!isMasterClient)
			{
				return room != null;
			}
			return false;
		}
	}

	public static TypedLobby lobby
	{
		get
		{
			return networkingPeer.lobby;
		}
		set
		{
			networkingPeer.lobby = value;
		}
	}

	public static PhotonPlayer masterClient
	{
		get
		{
			if (networkingPeer == null)
			{
				return null;
			}
			return networkingPeer.mMasterClient;
		}
	}

	[Obsolete("Used for compatibility with Unity networking only.")]
	public static int maxConnections
	{
		get
		{
			if (room == null)
			{
				return 0;
			}
			return room.maxPlayers;
		}
		set
		{
			room.maxPlayers = value;
		}
	}

	public static int MaxResendsBeforeDisconnect
	{
		get
		{
			return networkingPeer.SentCountAllowance;
		}
		set
		{
			if (value < 3)
			{
				value = 3;
			}
			if (value > 10)
			{
				value = 10;
			}
			networkingPeer.SentCountAllowance = value;
		}
	}

	public static bool NetworkStatisticsEnabled
	{
		get
		{
			return networkingPeer.TrafficStatsEnabled;
		}
		set
		{
			networkingPeer.TrafficStatsEnabled = value;
		}
	}

	public static bool offlineMode
	{
		get
		{
			return isOfflineMode;
		}
		set
		{
			if (value != isOfflineMode && (!value || !connected))
			{
				if (networkingPeer.PeerState != PeerStateValue.Disconnected)
				{
					networkingPeer.Disconnect();
				}
				isOfflineMode = value;
				if (isOfflineMode)
				{
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
					networkingPeer.ChangeLocalID(1);
					networkingPeer.mMasterClient = player;
				}
				else
				{
					offlineModeRoom = null;
					networkingPeer.ChangeLocalID(-1);
					networkingPeer.mMasterClient = null;
				}
			}
		}
	}

	public static PhotonPlayer[] otherPlayers
	{
		get
		{
			if (networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return networkingPeer.mOtherPlayerListCopy;
		}
	}

	public static int PacketLossByCrcCheck => networkingPeer.PacketLossByCrc;

	public static PhotonPlayer player
	{
		get
		{
			if (networkingPeer == null)
			{
				return null;
			}
			return networkingPeer.mLocalActor;
		}
	}

	public static PhotonPlayer[] playerList
	{
		get
		{
			if (networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return networkingPeer.mPlayerListCopy;
		}
	}

	public static string playerName
	{
		get
		{
			return networkingPeer.PlayerName;
		}
		set
		{
			networkingPeer.PlayerName = value;
		}
	}

	public static int ResentReliableCommands => networkingPeer.ResentReliableCommands;

	public static Room room
	{
		get
		{
			if (isOfflineMode)
			{
				return offlineModeRoom;
			}
			return networkingPeer.mCurrentGame;
		}
	}

	public static int sendRate
	{
		get
		{
			return 1000 / sendInterval;
		}
		set
		{
			sendInterval = 1000 / value;
			if (photonMono != null)
			{
				photonMono.updateInterval = sendInterval;
			}
			if (value < sendRateOnSerialize)
			{
				sendRateOnSerialize = value;
			}
		}
	}

	public static int sendRateOnSerialize
	{
		get
		{
			return 1000 / sendIntervalOnSerialize;
		}
		set
		{
			if (value > sendRate)
			{
				value = sendRate;
			}
			sendIntervalOnSerialize = 1000 / value;
			if (photonMono != null)
			{
				photonMono.updateIntervalOnSerialize = sendIntervalOnSerialize;
			}
		}
	}

	public static ServerConnection Server => networkingPeer.server;

	public static string ServerAddress
	{
		get
		{
			if (networkingPeer != null)
			{
				return networkingPeer.ServerAddress;
			}
			return "<not connected>";
		}
	}

	public static double time
	{
		get
		{
			if (offlineMode)
			{
				return Time.time;
			}
			return (double)networkingPeer.ServerTimeInMilliSeconds / 1000.0;
		}
	}

	public static int unreliableCommandsLimit
	{
		get
		{
			return networkingPeer.LimitOfUnreliableCommands;
		}
		set
		{
			networkingPeer.LimitOfUnreliableCommands = value;
		}
	}

	static PhotonNetwork()
	{
		_mAutomaticallySyncScene = false;
		autoJoinLobbyField = true;
		InstantiateInRoomOnly = true;
		isOfflineMode = false;
		lastUsedViewSubId = 0;
		lastUsedViewSubIdStatic = 0;
		logLevel = PhotonLogLevel.ErrorsOnly;
		m_autoCleanUpPlayerObjects = true;
		m_isMessageQueueRunning = true;
		manuallyAllocatedViewIds = new List<int>();
		MAX_VIEW_IDS = 1000;
		offlineModeRoom = null;
		PhotonServerSettings = (ServerSettings)CacheResources.Load("PhotonServerSettings", typeof(ServerSettings));
		precisionForFloatSynchronization = 0.01f;
		precisionForQuaternionSynchronization = 1f;
		precisionForVectorSynchronization = 9.9E-05f;
		sendInterval = 50;
		sendIntervalOnSerialize = 100;
		UseNameServer = true;
		UsePrefabCache = true;
		Application.runInBackground = true;
		GameObject gameObject = new GameObject();
		photonMono = gameObject.AddComponent<PhotonHandler>();
		gameObject.name = "PhotonMono";
		gameObject.hideFlags = HideFlags.HideInHierarchy;
		networkingPeer = new NetworkingPeer(photonMono, string.Empty, ConnectionProtocol.Udp);
		CustomTypes.Register();
	}

	public static void SwitchToProtocol(ConnectionProtocol cp)
	{
		if (networkingPeer.UsedProtocol != cp)
		{
			networkingPeer.Disconnect();
			networkingPeer.StopThread();
			networkingPeer = new NetworkingPeer(photonMono, string.Empty, cp);
		}
	}

	public static bool ConnectToMaster(string masterServerAddress, int port, string appID, string gameVersion)
	{
		switch (networkingPeer.UsedProtocol)
		{
		case ConnectionProtocol.Tcp:
			port = 4530;
			break;
		case ConnectionProtocol.WebSocket:
			masterServerAddress = "ws://" + masterServerAddress;
			port = 9090;
			break;
		case ConnectionProtocol.WebSocketSecure:
			throw new Exception("Service Not Available");
		default:
			port = 5055;
			break;
		}
		if (networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			return false;
		}
		if (offlineMode)
		{
			offlineMode = false;
		}
		if (!isMessageQueueRunning)
		{
			isMessageQueueRunning = true;
		}
		networkingPeer.SetApp(appID, gameVersion);
		networkingPeer.IsUsingNameServer = false;
		networkingPeer.IsInitialConnect = true;
		networkingPeer.MasterServerAddress = masterServerAddress + ":" + port + ((networkingPeer.UsedProtocol == ConnectionProtocol.WebSocket) ? "/Master" : "");
		return networkingPeer.Connect(networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
	}

	private static int[] AllocateSceneViewIDs(int countOfNewViews)
	{
		int[] array = new int[countOfNewViews];
		for (int i = 0; i < countOfNewViews; i++)
		{
			array[i] = AllocateViewID(0);
		}
		return array;
	}

	public static int AllocateViewID()
	{
		int num = AllocateViewID(player.ID);
		manuallyAllocatedViewIds.Add(num);
		return num;
	}

	private static int AllocateViewID(int ownerId)
	{
		if (ownerId == 0)
		{
			int num = lastUsedViewSubIdStatic;
			int num2 = ownerId * MAX_VIEW_IDS;
			for (int i = 1; i < MAX_VIEW_IDS; i++)
			{
				num = (num + 1) % MAX_VIEW_IDS;
				if (num != 0)
				{
					int num3 = num + num2;
					if (!networkingPeer.photonViewList.ContainsKey(num3))
					{
						lastUsedViewSubIdStatic = num;
						return num3;
					}
				}
			}
			throw new Exception($"AllocateViewID() failed. Room (user {ownerId}) is out of subIds, as all room viewIDs are used.");
		}
		int num4 = lastUsedViewSubId;
		int num5 = ownerId * MAX_VIEW_IDS;
		for (int j = 1; j < MAX_VIEW_IDS; j++)
		{
			num4 = (num4 + 1) % MAX_VIEW_IDS;
			if (num4 != 0)
			{
				int num6 = num4 + num5;
				if (!networkingPeer.photonViewList.ContainsKey(num6) && !manuallyAllocatedViewIds.Contains(num6))
				{
					lastUsedViewSubId = num4;
					return num6;
				}
			}
		}
		throw new Exception($"AllocateViewID() failed. User {ownerId} is out of subIds, as all viewIDs are used.");
	}

	public static bool CloseConnection(PhotonPlayer kickPlayer)
	{
		if (!VerifyCanUseNetwork())
		{
			return false;
		}
		if (!player.isMasterClient)
		{
			return false;
		}
		if (kickPlayer == null)
		{
			return false;
		}
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
		raiseEventOptions.TargetActors = new int[1] { kickPlayer.ID };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		return networkingPeer.OpRaiseEvent(203, null, sendReliable: true, raiseEventOptions2);
	}

	public static bool ConnectToBestCloudServer(string gameVersion)
	{
		if (PhotonServerSettings == null)
		{
			return false;
		}
		if (PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			return ConnectUsingSettings(gameVersion);
		}
		networkingPeer.IsInitialConnect = true;
		networkingPeer.SetApp(PhotonServerSettings.AppID, gameVersion);
		CloudRegionCode bestRegionCodeInPreferences = PhotonHandler.BestRegionCodeInPreferences;
		if (bestRegionCodeInPreferences != CloudRegionCode.none)
		{
			return networkingPeer.ConnectToRegionMaster(bestRegionCodeInPreferences);
		}
		return networkingPeer.ConnectToNameServer();
	}

	public static bool ConnectUsingSettings(string gameVersion)
	{
		if (PhotonServerSettings == null)
		{
			return false;
		}
		SwitchToProtocol(PhotonServerSettings.Protocol);
		networkingPeer.SetApp(PhotonServerSettings.AppID, gameVersion);
		if (PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			offlineMode = true;
			return true;
		}
		_ = offlineMode;
		offlineMode = false;
		isMessageQueueRunning = true;
		networkingPeer.IsInitialConnect = true;
		if (PhotonServerSettings.HostType == ServerSettings.HostingOption.SelfHosted)
		{
			networkingPeer.IsUsingNameServer = false;
			networkingPeer.MasterServerAddress = PhotonServerSettings.ServerAddress + ":" + PhotonServerSettings.ServerPort;
			return networkingPeer.Connect(networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
		}
		if (PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
		{
			return ConnectToBestCloudServer(gameVersion);
		}
		return networkingPeer.ConnectToRegionMaster(PhotonServerSettings.PreferredRegion);
	}

	public static bool CreateRoom(string roomName)
	{
		return CreateRoom(roomName, null, null);
	}

	public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		if (offlineMode)
		{
			if (offlineModeRoom != null)
			{
				return false;
			}
			offlineModeRoom = new Room(roomName, roomOptions);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return true;
		}
		if (networkingPeer.server == ServerConnection.MasterServer && connectedAndReady)
		{
			return networkingPeer.OpCreateGame(roomName, roomOptions, typedLobby);
		}
		return false;
	}

	[Obsolete("Use overload with RoomOptions and TypedLobby parameters.")]
	public static bool CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers)
	{
		RoomOptions roomOptions = new RoomOptions
		{
			isVisible = isVisible,
			isOpen = isOpen,
			maxPlayers = maxPlayers
		};
		return CreateRoom(roomName, roomOptions, null);
	}

	[Obsolete("Use overload with RoomOptions and TypedLobby parameters.")]
	public static bool CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers, Hashtable customRoomProperties, string[] propsToListInLobby)
	{
		RoomOptions roomOptions = new RoomOptions
		{
			isVisible = isVisible,
			isOpen = isOpen,
			maxPlayers = maxPlayers,
			customRoomProperties = customRoomProperties,
			customRoomPropertiesForLobby = propsToListInLobby
		};
		return CreateRoom(roomName, roomOptions, null);
	}

	public static void Destroy(PhotonView targetView)
	{
		if (targetView != null)
		{
			networkingPeer.RemoveInstantiatedGO(targetView.gameObject, !inRoom);
		}
	}

	public static void Destroy(GameObject targetGo)
	{
		networkingPeer.RemoveInstantiatedGO(targetGo, !inRoom);
	}

	public static void DestroyAll()
	{
		if (isMasterClient)
		{
			networkingPeer.DestroyAll(localOnly: false);
		}
	}

	public static void DestroyPlayerObjects(PhotonPlayer targetPlayer)
	{
		_ = player;
		DestroyPlayerObjects(targetPlayer.ID);
	}

	public static void DestroyPlayerObjects(int targetPlayerId)
	{
		if (VerifyCanUseNetwork() && (player.isMasterClient || targetPlayerId == player.ID))
		{
			networkingPeer.DestroyPlayerObjects(targetPlayerId, localOnly: false);
		}
	}

	internal static void EventRPC(PhotonView view, string methodName, RaiseEventOptions options, params object[] parameters)
	{
		if (VerifyCanUseNetwork() && room != null)
		{
			_ = player;
			if (networkingPeer != null)
			{
				networkingPeer.EventRPC(view, methodName, options, parameters);
			}
		}
	}

	public static void Disconnect()
	{
		GlobalEvents.DispatchOnGameStateReset();
		if (offlineMode)
		{
			offlineMode = false;
			offlineModeRoom = null;
			networkingPeer.states = PeerStates.Disconnecting;
			networkingPeer.OnStatusChanged(StatusCode.Disconnect);
		}
		else if (networkingPeer != null)
		{
			networkingPeer.Disconnect();
		}
	}

	public static void FetchServerTimestamp()
	{
		if (networkingPeer != null)
		{
			networkingPeer.FetchServerTimestamp();
		}
	}

	public static bool FindFriends(string[] friendsToFind)
	{
		if (networkingPeer != null && !isOfflineMode)
		{
			return networkingPeer.OpFindFriends(friendsToFind);
		}
		return false;
	}

	public static int GetPing()
	{
		return networkingPeer.RoundTripTime;
	}

	public static RoomInfo[] GetRoomList()
	{
		if (!offlineMode && networkingPeer != null)
		{
			return networkingPeer.mGameListCopy;
		}
		return new RoomInfo[0];
	}

	[Obsolete("Used for compatibility with Unity networking only. Encryption is automatically initialized while connecting.")]
	public static void InitializeSecurity()
	{
	}

	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group)
	{
		return Instantiate(prefabName, position, rotation, group, null);
	}

	public static T Instantiate<T>(string prefabName, Vector3 position, Quaternion rotation, int group = 0, object[] data = null) where T : Component
	{
		if (!connected || (InstantiateInRoomOnly && !inRoom))
		{
			return null;
		}
		T val = ((!prefabName.StartsWith("RCAsset/")) ? CacheResources.Load<T>(prefabName) : CacheResources.RCLoad<T>(prefabName.Substring(8)));
		if (val == null)
		{
			return null;
		}
		GameObject gameObject = val.gameObject;
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			return null;
		}
		int[] array = new int[gameObject.GetComponentsInChildren<PhotonView>(includeInactive: true).Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = AllocateViewID();
		}
		return networkingPeer.DoInstantiate(networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, isGlobalObject: false), NetworkingPeer.instance.mLocalActor, val);
	}

	public static void PoolDisable(this Pool pool, GameObject GO)
	{
		if (pool != null && GO != null && pool.Contains(GO))
		{
			PhotonView component = GO.GetComponent<PhotonView>();
			if (!(component == null))
			{
				networkingPeer.PoolDisable(GO);
				UnAllocateViewID(component.viewID);
				GO.SetActive(value: false);
			}
		}
	}

	public static T PoolEnable<T>(this Pool pool, string prefabName, Vector3 position, Quaternion rotation, int group = 0, object[] data = null) where T : Component
	{
		if (!connected || (InstantiateInRoomOnly && !inRoom))
		{
			return null;
		}
		if (pool == null)
		{
			return null;
		}
		T val = pool.Enable<T>(prefabName, position, rotation, localOnly: true);
		if (val == null)
		{
			return null;
		}
		PhotonView component = val.GetComponent<PhotonView>();
		if (component == null)
		{
			pool.Disable(val, localOnly: true);
			return null;
		}
		networkingPeer.instantiatedObjects[component.viewID] = val.gameObject;
		networkingPeer.photonViewList.Remove(component.viewID);
		UnAllocateViewID(component.viewID);
		networkingPeer.SendInstantiate(prefabName, position, rotation, group, PhotonView.CreateNewViewID(prefabName, component), data, isGlobalObject: false);
		networkingPeer.photonViewList[component.viewID] = component;
		return val;
	}

	public static GameObject PoolEnable(this Pool pool, string prefabName, Vector3 position, Quaternion rotation, int group = 0, object[] data = null)
	{
		if (!connected || (InstantiateInRoomOnly && !inRoom))
		{
			return null;
		}
		if (pool == null)
		{
			return null;
		}
		GameObject gameObject = pool.Enable(prefabName, position, rotation, localOnly: true);
		if (gameObject == null)
		{
			return null;
		}
		PhotonView component = gameObject.GetComponent<PhotonView>();
		if (component == null)
		{
			pool.Disable(gameObject, localOnly: true);
			return null;
		}
		networkingPeer.instantiatedObjects[component.viewID] = gameObject;
		networkingPeer.photonViewList.Remove(component.viewID);
		UnAllocateViewID(component.viewID);
		networkingPeer.SendInstantiate(prefabName, position, rotation, group, PhotonView.CreateNewViewID(prefabName, component), data, isGlobalObject: false);
		networkingPeer.photonViewList[component.viewID] = component;
		return gameObject;
	}

	public static T PoolEventEnable<T>(this Pool pool, string prefabName, Vector3 position, Quaternion rotation, RaiseEventOptions options) where T : Component
	{
		if (!connected || (InstantiateInRoomOnly && !inRoom))
		{
			return null;
		}
		if (pool == null)
		{
			return null;
		}
		T val = pool.Enable<T>(prefabName, position, rotation, localOnly: true);
		if (val == null)
		{
			return null;
		}
		PhotonView component = val.GetComponent<PhotonView>();
		if (component == null)
		{
			pool.Disable(val, localOnly: true);
			return null;
		}
		networkingPeer.instantiatedObjects[component.viewID] = val.gameObject;
		networkingPeer.photonViewList.Remove(component.viewID);
		UnAllocateViewID(component.viewID);
		networkingPeer.SendEventInstantiate(prefabName, position, rotation, PhotonView.CreateNewViewID(prefabName, component), options);
		networkingPeer.photonViewList[component.viewID] = component;
		return val;
	}

	public static GameObject PoolEventEnable(this Pool pool, string prefabName, Vector3 position, Quaternion rotation, RaiseEventOptions options)
	{
		if (!connected || (InstantiateInRoomOnly && !inRoom))
		{
			return null;
		}
		if (pool == null)
		{
			return null;
		}
		GameObject gameObject = pool.Enable(prefabName, position, rotation, localOnly: true);
		if (gameObject == null)
		{
			return null;
		}
		PhotonView component = gameObject.GetComponent<PhotonView>();
		if (component == null)
		{
			pool.Disable(gameObject, localOnly: true);
			return null;
		}
		networkingPeer.instantiatedObjects[component.viewID] = gameObject;
		networkingPeer.photonViewList.Remove(component.viewID);
		UnAllocateViewID(component.viewID);
		networkingPeer.SendEventInstantiate(prefabName, position, rotation, PhotonView.CreateNewViewID(prefabName, component), options);
		networkingPeer.photonViewList[component.viewID] = component;
		return gameObject;
	}

	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		if (!connected || (InstantiateInRoomOnly && !inRoom))
		{
			return null;
		}
		GameObject gameObject = ((!prefabName.StartsWith("RCAsset/")) ? CacheResources.Load<GameObject>(prefabName) : CacheResources.RCLoad<GameObject>(prefabName.Substring(8)));
		if (gameObject == null)
		{
			return null;
		}
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			return null;
		}
		int[] array = new int[gameObject.GetPhotonViewsInChildren().Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = AllocateViewID(player.ID);
		}
		Hashtable evData = networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, isGlobalObject: false);
		return networkingPeer.DoInstantiate2(evData, networkingPeer.mLocalActor, gameObject);
	}

	public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		if (!connected || (InstantiateInRoomOnly && !inRoom))
		{
			return null;
		}
		if (!isMasterClient)
		{
			return null;
		}
		GameObject gameObject = (GameObject)CacheResources.Load(prefabName, typeof(GameObject));
		if (gameObject == null)
		{
			return null;
		}
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			return null;
		}
		int[] array = AllocateSceneViewIDs(gameObject.GetPhotonViewsInChildren().Length);
		if (array == null)
		{
			return null;
		}
		Hashtable evData = networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, isGlobalObject: true);
		return networkingPeer.DoInstantiate2(evData, networkingPeer.mLocalActor, gameObject);
	}

	public static void InternalCleanPhotonMonoFromSceneIfStuck()
	{
		if (!(UnityEngine.Object.FindObjectsOfType(typeof(PhotonHandler)) is PhotonHandler[] array) || array.Length == 0)
		{
			return;
		}
		PhotonHandler[] array2 = array;
		foreach (PhotonHandler photonHandler in array2)
		{
			photonHandler.gameObject.hideFlags = HideFlags.None;
			if (photonHandler.gameObject != null && photonHandler.gameObject.name == "PhotonMono")
			{
				UnityEngine.Object.DestroyImmediate(photonHandler.gameObject);
			}
			UnityEngine.Object.DestroyImmediate(photonHandler);
		}
	}

	public static bool JoinLobby()
	{
		return JoinLobby(null);
	}

	public static bool JoinLobby(TypedLobby typedLobby)
	{
		if (!connected || Server != ServerConnection.MasterServer)
		{
			return false;
		}
		if (typedLobby == null)
		{
			typedLobby = TypedLobby.Default;
		}
		bool num = networkingPeer.OpJoinLobby(typedLobby);
		if (num)
		{
			networkingPeer.lobby = typedLobby;
		}
		return num;
	}

	public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		if (offlineMode)
		{
			if (offlineModeRoom != null)
			{
				return false;
			}
			offlineModeRoom = new Room(roomName, roomOptions);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return true;
		}
		if (networkingPeer.server != ServerConnection.MasterServer || !connectedAndReady)
		{
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			return false;
		}
		return networkingPeer.OpJoinRoom(roomName, roomOptions, typedLobby, createIfNotExists: true);
	}

	public static bool JoinRandomRoom()
	{
		return JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null);
	}

	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers)
	{
		return JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, null, null);
	}

	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter)
	{
		if (offlineMode)
		{
			if (offlineModeRoom != null)
			{
				return false;
			}
			offlineModeRoom = new Room("offline room", null);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return true;
		}
		if (networkingPeer.server != ServerConnection.MasterServer || !connectedAndReady)
		{
			return false;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.MergeStringKeys(expectedCustomRoomProperties);
		if (expectedMaxPlayers > 0)
		{
			hashtable[byte.MaxValue] = expectedMaxPlayers;
		}
		return networkingPeer.OpJoinRandomRoom(hashtable, 0, null, matchingType, typedLobby, sqlLobbyFilter);
	}

	public static bool JoinRoom(string roomName)
	{
		if (offlineMode)
		{
			if (offlineModeRoom != null)
			{
				return false;
			}
			offlineModeRoom = new Room(roomName, null);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return true;
		}
		if (networkingPeer.server != ServerConnection.MasterServer || !connectedAndReady)
		{
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			return false;
		}
		return networkingPeer.OpJoinRoom(roomName, null, null, createIfNotExists: false);
	}

	[Obsolete("Use overload with roomOptions and TypedLobby parameter.")]
	public static bool JoinRoom(string roomName, bool createIfNotExists)
	{
		if (connectionStateDetailed != PeerStates.Joining && connectionStateDetailed != PeerStates.Joined && connectionStateDetailed != PeerStates.ConnectedToGameserver && room == null && !(roomName == string.Empty))
		{
			if (offlineMode)
			{
				offlineModeRoom = new Room(roomName, null);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
				return true;
			}
			return networkingPeer.OpJoinRoom(roomName, null, null, createIfNotExists);
		}
		return false;
	}

	public static bool LeaveLobby()
	{
		if (connected && Server == ServerConnection.MasterServer)
		{
			return networkingPeer.OpLeaveLobby();
		}
		return false;
	}

	public static bool LeaveRoom()
	{
		if (offlineMode)
		{
			offlineModeRoom = null;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
			return true;
		}
		_ = room;
		return networkingPeer.OpLeave();
	}

	public static void LoadLevel(int levelNumber)
	{
		networkingPeer.SetLevelInPropsIfSynced(levelNumber);
		isMessageQueueRunning = false;
		networkingPeer.loadingLevelAndPausedNetwork = true;
		Application.LoadLevel(levelNumber);
	}

	public static void LoadLevel(string levelName)
	{
		networkingPeer.SetLevelInPropsIfSynced(levelName);
		isMessageQueueRunning = false;
		networkingPeer.loadingLevelAndPausedNetwork = true;
		Application.LoadLevel(levelName);
	}

	public static void NetworkStatisticsReset()
	{
		networkingPeer.TrafficStatsReset();
	}

	public static string NetworkStatisticsToString()
	{
		if (networkingPeer != null && !offlineMode)
		{
			return networkingPeer.VitalStatsToString(all: false);
		}
		return "Offline or in OfflineMode. No VitalStats available.";
	}

	public static void OverrideBestCloudServer(CloudRegionCode region)
	{
		PhotonHandler.BestRegionCodeInPreferences = region;
	}

	public static bool RaiseEvent(byte eventCode, object eventContent, bool sendReliable, RaiseEventOptions options)
	{
		if (inRoom && eventCode < byte.MaxValue)
		{
			return networkingPeer.OpRaiseEvent(eventCode, eventContent, sendReliable, options);
		}
		return false;
	}

	public static void RefreshCloudServerRating()
	{
		throw new NotImplementedException("not available at the moment");
	}

	public static void RemoveRPCs(PhotonPlayer targetPlayer)
	{
		if (VerifyCanUseNetwork() && (targetPlayer.isLocal || isMasterClient))
		{
			networkingPeer.OpCleanRpcBuffer(targetPlayer.ID);
		}
	}

	public static void RemoveRPCs(PhotonView targetPhotonView)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.CleanRpcBufferIfMine(targetPhotonView);
		}
	}

	public static void RemoveRPCsInGroup(int targetGroup)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.RemoveRPCsInGroup(targetGroup);
		}
	}

	internal static void RPC(PhotonView view, string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		if (VerifyCanUseNetwork() && room != null)
		{
			_ = player;
			if (networkingPeer != null)
			{
				networkingPeer.RPC(view, methodName, targetPlayer, parameters);
			}
		}
	}

	internal static void RPC(PhotonView view, string methodName, PhotonTargets target, params object[] parameters)
	{
		if (VerifyCanUseNetwork() && room != null && networkingPeer != null)
		{
			networkingPeer.RPC(view, methodName, target, parameters);
		}
	}

	public static void SendOutgoingCommands()
	{
		if (VerifyCanUseNetwork())
		{
			while (networkingPeer.SendOutgoingCommands())
			{
			}
		}
	}

	public static void SetLevelPrefix(short prefix)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.SetLevelPrefix(prefix);
		}
	}

	public static bool SetMasterClient(PhotonPlayer masterClientPlayer)
	{
		return networkingPeer.SetMasterClient(masterClientPlayer.ID, sync: true);
	}

	public static void SetPlayerCustomProperties(Hashtable customProperties)
	{
		if (customProperties == null)
		{
			customProperties = new Hashtable();
			foreach (object key in player.customProperties.Keys)
			{
				customProperties[(string)key] = null;
			}
		}
		if (room != null && room.isLocalClientInside)
		{
			player.SetCustomProperties(customProperties);
		}
		else
		{
			player.InternalCacheProperties(customProperties);
		}
	}

	public static void SetReceivingEnabled(int group, bool enabled)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.SetReceivingEnabled(group, enabled);
		}
	}

	public static void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.SetReceivingEnabled(enableGroups, disableGroups);
		}
	}

	public static void SetSendingEnabled(int group, bool enabled)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.SetSendingEnabled(group, enabled);
		}
	}

	public static void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (VerifyCanUseNetwork())
		{
			networkingPeer.SetSendingEnabled(enableGroups, disableGroups);
		}
	}

	public static void UnAllocateViewID(int viewID)
	{
		manuallyAllocatedViewIds.Remove(viewID);
		networkingPeer.photonViewList.ContainsKey(viewID);
	}

	private static bool VerifyCanUseNetwork()
	{
		if (connected)
		{
			return true;
		}
		return false;
	}

	public static bool WebRpc(string name, object parameters)
	{
		return networkingPeer.WebRpc(name, parameters);
	}
}
