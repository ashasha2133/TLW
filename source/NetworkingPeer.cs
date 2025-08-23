using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BRM;
using CLEARSKIES;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

internal class NetworkingPeer : LoadbalancingPeer, IPhotonPeerListener
{
	private HashSet<int> allowedReceivingGroups;

	public static Dictionary<short, List<GameObject>> actorGO;

	private HashSet<int> blockSendingGroups;

	protected internal short currentLevelPrefix;

	protected internal const string CurrentSceneProperty = "curScn";

	private bool didAuthenticate;

	private IPhotonPeerListener externalListener;

	private string[] friendListRequested;

	private int friendListTimestamp;

	public bool hasSwitchedMC;

	public bool insideLobby;

	public static NetworkingPeer instance;

	public Dictionary<int, GameObject> instantiatedObjects;

	private bool isFetchingFriends;

	public bool IsInitialConnect;

	public static Dictionary<int, string> leftList;

	protected internal bool loadingLevelAndPausedNetwork;

	public static Dictionary<int, PhotonPlayer> mActors;

	protected internal string mAppId;

	protected internal string mAppVersion;

	public Dictionary<string, RoomInfo> mGameList;

	public RoomInfo[] mGameListCopy;

	private JoinType mLastJoinType;

	public PhotonPlayer mMasterClient;

	private Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache;

	public PhotonPlayer[] mOtherPlayerListCopy;

	public PhotonPlayer[] mPlayerListCopy;

	private bool mPlayernameHasToBeUpdated;

	public string NameServerAddress;

	protected internal Dictionary<int, PhotonView> photonViewList;

	private string playername;

	private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort;

	public bool requestSecurity;

	private static ExitGames.Client.Photon.Hashtable restartban;

	private static ExitGames.Client.Photon.Hashtable resultban;

	public static List<string> rpcList;

	protected internal static Dictionary<string, int> rpcShortcuts;

	private Dictionary<int, object[]> tempInstantiationData;

	public static bool UsePrefabCache;

	public static List<string> ipAddr;

	internal static Yield yield;

	private static DateTime lastViewUpdate;

	private string[] validProps = new string[49]
	{
		"sender", "ready", "isadmin", "uniform_type", "total_dmg", "team", "statSKILL", "statGAS", "statBLA", "statSPD",
		"statACL", "skin_color", "sex", "RCteam", "RCBombRadius", "RCBombA", "RCBombB", "RCBombG", "RCBombR", "part_chest_skinned_cloth_texture",
		"part_chest_skinned_cloth_mesh", "part_chest_object_texture", "part_chest_object_mesh", "part_chest_1_object_texture", "part_chest_1_object_mesh", "max_dmg", "kills", "isTitan", "heroCostumeId", "hairInfo",
		"dead", "eye_texture_id", "glass_texture_id", "hair_color1", "hair_color2", "hair_color3", "deaths", "guildName", "division", "name",
		"beard_texture_id", "body_texture", "character", "costumeId", "currentLevel", "customBool", "customFloat", "customInt", "customString"
	};

	public PeerStates states { get; internal set; }

	public List<Region> AvailableRegions { get; protected internal set; }

	public CloudRegionCode CloudRegion { get; protected internal set; }

	public AuthenticationValues CustomAuthenticationValues { get; set; }

	protected internal int FriendsListAge
	{
		get
		{
			if (!isFetchingFriends && friendListTimestamp != 0)
			{
				return Environment.TickCount - friendListTimestamp;
			}
			return 0;
		}
	}

	public bool IsAuthorizeSecretAvailable => false;

	public bool IsUsingNameServer { get; protected internal set; }

	public TypedLobby lobby { get; set; }

	protected internal string mAppVersionPun => string.Format("{0}_{1}", mAppVersion, "1.28");

	public string MasterServerAddress { get; protected internal set; }

	public Room mCurrentGame
	{
		get
		{
			if (mRoomToGetInto != null && mRoomToGetInto.isLocalClientInside)
			{
				return mRoomToGetInto;
			}
			return null;
		}
	}

	public int mGameCount { get; internal set; }

	public string mGameserver { get; internal set; }

	public PhotonPlayer mLocalActor { get; internal set; }

	public int mPlayersInRoomsCount { get; internal set; }

	public int mPlayersOnMasterCount { get; internal set; }

	public int mQueuePosition { get; internal set; }

	internal RoomOptions mRoomOptionsForCreate { get; set; }

	internal TypedLobby mRoomToEnterLobby { get; set; }

	internal Room mRoomToGetInto { get; set; }

	public string PlayerName
	{
		get
		{
			return playername;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && !value.Equals(playername))
			{
				if (mLocalActor != null)
				{
					mLocalActor.name = value;
				}
				playername = value;
				if (mCurrentGame != null)
				{
					SendPlayerName();
				}
			}
		}
	}

	protected internal ServerConnection server { get; private set; }

	static NetworkingPeer()
	{
		actorGO = new Dictionary<short, List<GameObject>>();
		leftList = new Dictionary<int, string>();
		restartban = new ExitGames.Client.Photon.Hashtable();
		resultban = new ExitGames.Client.Photon.Hashtable();
		rpcList = new List<string>();
		UsePrefabCache = true;
		ipAddr = new List<string>();
		lastViewUpdate = DateTime.Now;
		ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>
		{
			{
				ConnectionProtocol.Udp,
				5058
			},
			{
				ConnectionProtocol.Tcp,
				4533
			}
		};
	}

	public NetworkingPeer(IPhotonPeerListener listener, string playername, ConnectionProtocol connectionProtocol)
		: base(listener, connectionProtocol)
	{
		this.playername = string.Empty;
		mActors = new Dictionary<int, PhotonPlayer>();
		mOtherPlayerListCopy = new PhotonPlayer[0];
		mPlayerListCopy = new PhotonPlayer[0];
		requestSecurity = true;
		monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();
		mGameList = new Dictionary<string, RoomInfo>();
		mGameListCopy = new RoomInfo[0];
		instantiatedObjects = new Dictionary<int, GameObject>();
		allowedReceivingGroups = new HashSet<int>();
		blockSendingGroups = new HashSet<int>();
		photonViewList = new Dictionary<int, PhotonView>();
		NameServerAddress = "ns.exitgamescloud.com";
		tempInstantiationData = new Dictionary<int, object[]>();
		base.Listener = this;
		lobby = TypedLobby.Default;
		base.LimitOfUnreliableCommands = 40;
		externalListener = listener;
		PlayerName = playername;
		mLocalActor = new PhotonPlayer(isLocal: true, -1, this.playername);
		AddNewPlayer(mLocalActor.ID, mLocalActor);
		rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
		for (int i = 0; i < PhotonNetwork.PhotonServerSettings.RpcList.Count; i++)
		{
			string key = PhotonNetwork.PhotonServerSettings.RpcList[i];
			rpcShortcuts[key] = i;
		}
		states = PeerStates.PeerCreated;
	}

	internal void PoolDisable(GameObject go, bool localOnly = false)
	{
		if (go == null)
		{
			return;
		}
		PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>(includeInactive: true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int ownerActorNr = photonView.OwnerActorNr;
		int instantiationId = photonView.instantiationId;
		if (!localOnly)
		{
			byte eventCode = 202;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(7, instantiationId);
			OpRaiseEvent(eventCode, hashtable, sendReliable: true, new RaiseEventOptions
			{
				CachingOption = EventCaching.RemoveFromRoomCache,
				TargetActors = new int[1] { ownerActorNr }
			});
			byte eventCode2 = 204;
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2.Add(0, instantiationId);
			OpRaiseEvent(eventCode2, hashtable2, sendReliable: true, null);
		}
		instantiatedObjects.Remove(instantiationId);
		for (int num = componentsInChildren.Length - 1; num >= 0; num--)
		{
			if ((photonView = componentsInChildren[num]) != null)
			{
				if (photonView.instantiationId >= 1)
				{
					photonView.destroyedByPhotonNetworkOrQuit = true;
					photonViewList.Remove(photonView.viewID);
				}
				if (!localOnly)
				{
					byte eventCode3 = 200;
					ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
					hashtable3.Add(0, photonView.viewID);
					OpRaiseEvent(eventCode3, hashtable3, sendReliable: true, new RaiseEventOptions
					{
						CachingOption = EventCaching.RemoveFromRoomCache
					});
				}
			}
		}
	}

	internal ExitGames.Client.Photon.Hashtable SendEventInstantiate(string prefabName, Vector3 position, Quaternion rotation, int[] viewIDs, RaiseEventOptions options, int group = 0, object[] data = null)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = prefabName;
		if (position != Vector3.zero)
		{
			hashtable[1] = position;
		}
		if (rotation != Quaternion.identity)
		{
			hashtable[2] = rotation;
		}
		if (group != 0)
		{
			hashtable[3] = group;
		}
		if (viewIDs.Length > 1)
		{
			hashtable[4] = viewIDs;
		}
		if (data != null)
		{
			hashtable[5] = data;
		}
		if (currentLevelPrefix > 0)
		{
			hashtable[8] = currentLevelPrefix;
		}
		hashtable[6] = base.ServerTimeInMilliSeconds;
		hashtable[7] = viewIDs[0];
		if (options != null)
		{
			OpRaiseEvent(202, hashtable, sendReliable: true, options);
		}
		return hashtable;
	}

	public void AddNewPlayer(int ID, PhotonPlayer player)
	{
		if (!mActors.ContainsKey(ID))
		{
			mActors[ID] = player;
			RebuildPlayerListCopies();
		}
	}

	private bool AlmostEquals(object[] lastData, object[] currentContent)
	{
		if (lastData != null || currentContent != null)
		{
			if (lastData == null || currentContent == null || lastData.Length != currentContent.Length)
			{
				return false;
			}
			for (int i = 0; i < currentContent.Length; i++)
			{
				object one = currentContent[i];
				object two = lastData[i];
				if (!ObjectIsSameWithInprecision(one, two))
				{
					return false;
				}
			}
		}
		return true;
	}

	public void ChangeLocalID(int newID)
	{
		_ = mLocalActor;
		if (mActors.ContainsKey(mLocalActor.ID))
		{
			mActors.Remove(mLocalActor.ID);
		}
		mLocalActor.InternalChangeLocalID(newID);
		mActors[mLocalActor.ID] = mLocalActor;
		RebuildPlayerListCopies();
	}

	public void checkLAN()
	{
		if (FengGameManagerMKII.OnPrivateServer && MasterServerAddress != null && MasterServerAddress != string.Empty && mGameserver != null && mGameserver != string.Empty && MasterServerAddress.Contains(":") && mGameserver.Contains(":"))
		{
			mGameserver = MasterServerAddress.Split(':')[0] + ":" + mGameserver.Split(':')[1];
		}
	}

	private void CheckMasterClient(int leavingPlayerId)
	{
		bool flag = mMasterClient != null && mMasterClient.ID == leavingPlayerId;
		bool flag2;
		if (!(!(flag2 = leavingPlayerId > 0) || flag))
		{
			return;
		}
		if (mActors.Count <= 1)
		{
			mMasterClient = mLocalActor;
		}
		else
		{
			int num = int.MaxValue;
			foreach (int key in mActors.Keys)
			{
				if (key < num && key != leavingPlayerId)
				{
					num = key;
				}
			}
			mMasterClient = mActors[num];
		}
		if (flag2)
		{
			object[] parameters = new object[1] { mMasterClient };
			SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, parameters);
		}
	}

	private bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
	{
		if (methodParameters.Length < callParameterTypes.Length)
		{
			return false;
		}
		for (int i = 0; i < callParameterTypes.Length; i++)
		{
			Type parameterType = methodParameters[i].ParameterType;
			if (callParameterTypes[i] != null && !parameterType.Equals(callParameterTypes[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void CleanRpcBufferIfMine(PhotonView view)
	{
		if (view.ownerId == mLocalActor.ID || mLocalActor.isMasterClient)
		{
			OpCleanRpcBuffer(view);
		}
	}

	public bool Connect(string serverAddress, ServerConnection type)
	{
		if (PhotonHandler.AppQuits)
		{
			return false;
		}
		if (PhotonNetwork.connectionStatesDetailed == PeerStates.Disconnecting)
		{
			return false;
		}
		bool result;
		if (result = base.Connect(serverAddress, string.Empty))
		{
			switch (type)
			{
			case ServerConnection.MasterServer:
				states = PeerStates.ConnectingToMasterserver;
				return result;
			case ServerConnection.GameServer:
				states = PeerStates.ConnectingToGameserver;
				return result;
			case ServerConnection.NameServer:
				states = PeerStates.ConnectingToNameServer;
				return result;
			}
		}
		return result;
	}

	public override bool Connect(string serverAddress, string applicationName)
	{
		return false;
	}

	public bool ConnectToNameServer()
	{
		if (PhotonHandler.AppQuits)
		{
			return false;
		}
		IsUsingNameServer = true;
		CloudRegion = CloudRegionCode.none;
		if (states != PeerStates.ConnectedToNameServer)
		{
			string text = NameServerAddress;
			if (!text.Contains(":"))
			{
				int value = 0;
				ProtocolToNameServerPort.TryGetValue(base.UsedProtocol, out value);
				text = $"{text}:{value}";
			}
			if (!base.Connect(text, "ns"))
			{
				return false;
			}
			states = PeerStates.ConnectingToNameServer;
		}
		return true;
	}

	public bool ConnectToRegionMaster(CloudRegionCode region)
	{
		if (PhotonHandler.AppQuits)
		{
			return false;
		}
		IsUsingNameServer = true;
		CloudRegion = region;
		if (states == PeerStates.ConnectedToNameServer)
		{
			return OpAuthenticate(mAppId, mAppVersionPun, PlayerName, CustomAuthenticationValues, region.ToString());
		}
		string text = NameServerAddress;
		if (!text.Contains(":"))
		{
			int value = 0;
			ProtocolToNameServerPort.TryGetValue(base.UsedProtocol, out value);
			text = $"{text}:{value}";
		}
		if (!base.Connect(text, "ns"))
		{
			return false;
		}
		states = PeerStates.ConnectingToNameServer;
		return true;
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		externalListener.DebugReturn(level, message);
	}

	private bool DeltaCompressionRead(PhotonView view, ExitGames.Client.Photon.Hashtable data)
	{
		if (!data.ContainsKey((byte)1))
		{
			if (view.lastOnSerializeDataReceived == null)
			{
				return false;
			}
			object[] array = data[(byte)2] as object[];
			if (array == null)
			{
				return false;
			}
			int[] array2 = data[(byte)3] as int[];
			if (array2 == null)
			{
				array2 = new int[0];
			}
			object[] lastOnSerializeDataReceived = view.lastOnSerializeDataReceived;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null && !array2.Contains(i))
				{
					array[i] = lastOnSerializeDataReceived[i];
				}
			}
			data[(byte)1] = array;
		}
		return true;
	}

	private bool DeltaCompressionWrite(PhotonView view, ExitGames.Client.Photon.Hashtable data)
	{
		if (view.lastOnSerializeDataSent != null)
		{
			object[] lastOnSerializeDataSent = view.lastOnSerializeDataSent;
			object[] array = data[(byte)1] as object[];
			if (array == null)
			{
				return false;
			}
			if (lastOnSerializeDataSent.Length != array.Length)
			{
				return true;
			}
			object[] array2 = new object[array.Length];
			int num = 0;
			List<int> list = new List<int>();
			for (int i = 0; i < array2.Length; i++)
			{
				object obj = array[i];
				object two = lastOnSerializeDataSent[i];
				if (ObjectIsSameWithInprecision(obj, two))
				{
					num++;
					continue;
				}
				array2[i] = array[i];
				if (obj == null)
				{
					list.Add(i);
				}
			}
			if (num > 0)
			{
				data.Remove((byte)1);
				if (num == array.Length)
				{
					return false;
				}
				data[(byte)2] = array2;
				if (list.Count > 0)
				{
					data[(byte)3] = list.ToArray();
				}
			}
		}
		return true;
	}

	public void DestroyAll(bool localOnly)
	{
		if (!localOnly)
		{
			OpRemoveCompleteCache();
			SendDestroyOfAll();
		}
		LocalCleanupAnythingInstantiated(destroyInstantiatedGameObjects: true);
	}

	public void DestroyPlayerObjects(int playerId, bool localOnly)
	{
		if (playerId == PhotonNetwork.player.ID || playerId <= 0)
		{
			return;
		}
		if (!localOnly)
		{
			OpRemoveFromServerInstantiationsOfPlayer(playerId);
			OpCleanRpcBuffer(playerId);
			SendDestroyOfPlayer(playerId);
		}
		Queue<GameObject> queue = new Queue<GameObject>();
		int num = playerId * PhotonNetwork.MAX_VIEW_IDS;
		int num2 = num + PhotonNetwork.MAX_VIEW_IDS;
		foreach (KeyValuePair<int, GameObject> instantiatedObject in instantiatedObjects)
		{
			if (instantiatedObject.Key > num && instantiatedObject.Key < num2)
			{
				queue.Enqueue(instantiatedObject.Value);
			}
		}
		foreach (GameObject item in queue)
		{
			RemoveInstantiatedGO(item, localOnly: true);
		}
	}

	public override void Disconnect()
	{
		if (base.PeerState == PeerStateValue.Disconnected)
		{
			_ = PhotonHandler.AppQuits;
			return;
		}
		states = PeerStates.Disconnecting;
		base.Disconnect();
	}

	private void DisconnectToReconnect2()
	{
		checkLAN();
		switch (server)
		{
		case ServerConnection.MasterServer:
			states = FengGameManagerMKII.returnPeerState(2);
			base.Disconnect();
			break;
		case ServerConnection.GameServer:
			states = FengGameManagerMKII.returnPeerState(3);
			base.Disconnect();
			break;
		case ServerConnection.NameServer:
			states = FengGameManagerMKII.returnPeerState(4);
			base.Disconnect();
			break;
		}
	}

	internal T DoInstantiate<T>(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, T resourceGameObject) where T : Component
	{
		if (evData == null || photonPlayer == null || !evData.ContainsKey(7) || !evData.ContainsKey(0))
		{
			return null;
		}
		string text = (string)evData[0];
		if (text.IsNullOrEmpty() || photonPlayer == null)
		{
			return null;
		}
		int num = (int)evData[7];
		if (instance.mLocalActor.isMasterClient && (text.Contains("monsterprefab") || text.Contains("BoxPrefab")))
		{
			ServerCleanInstantiateAndDestroy(num, photonPlayer.ID);
			return null;
		}
		int[] array = (evData.ContainsKey(4) ? ((int[])evData[4]) : new int[1] { num });
		if (!InstantiateTracker.instance.checkObj(text, photonPlayer, array))
		{
			return null;
		}
		_ = (int)evData[6];
		int num2 = (evData.ContainsKey(3) ? ((int)evData[3]) : 0);
		short prefix = (short)(evData.ContainsKey(8) ? ((short)evData[8]) : 0);
		Vector3 position = (evData.ContainsKey(1) ? ((Vector3)evData[1]) : Vector3.zero);
		Quaternion rotation = (evData.ContainsKey(2) ? ((Quaternion)evData[2]) : Quaternion.identity);
		object[] instantiationData = (evData.ContainsKey(5) ? ((object[])evData[5]) : null);
		if (num2 != 0 && !allowedReceivingGroups.Contains(num2))
		{
			return null;
		}
		if ((UnityEngine.Object)resourceGameObject == (UnityEngine.Object)null)
		{
			if (text.StartsWith("RCAsset/"))
			{
				resourceGameObject = CacheResources.RCLoad<T>(text.Substring(8));
			}
			else if (!CacheResources.Load(text, out resourceGameObject))
			{
				return null;
			}
		}
		PhotonView[] componentsInChildren = resourceGameObject.GetComponentsInChildren<PhotonView>(includeInactive: true);
		PhotonView photonView = componentsInChildren[0];
		if (FengGameManagerMKII.instance != null)
		{
			PhotonView pView = FengGameManagerMKII.PView;
			if (pView.viewID == num || pView.viewID == photonView.viewID)
			{
				return null;
			}
		}
		if (componentsInChildren.Length != array.Length)
		{
			throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
		}
		for (int i = 0; i < array.Length; i++)
		{
			componentsInChildren[i].viewID = array[i];
			componentsInChildren[i].prefix = prefix;
			componentsInChildren[i].instantiationId = num;
		}
		StoreInstantiationData(num, instantiationData);
		T val = (T)UnityEngine.Object.Instantiate(resourceGameObject, position, rotation);
		for (int j = 0; j < array.Length; j++)
		{
			componentsInChildren[j].viewID = 0;
			componentsInChildren[j].prefix = -1;
			componentsInChildren[j].prefixBackup = -1;
			componentsInChildren[j].instantiationId = -1;
		}
		RemoveInstantiationData(num);
		if (instantiatedObjects.ContainsKey(num))
		{
			string text2 = string.Empty;
			GameObject gameObject = instantiatedObjects[num];
			if (gameObject != null)
			{
				PhotonView[] componentsInChildren2 = gameObject.GetComponentsInChildren<PhotonView>(includeInactive: true);
				foreach (PhotonView photonView2 in componentsInChildren2)
				{
					if (photonView2 != null)
					{
						text2 = text2 + photonView2.ToString() + ", ";
					}
				}
			}
			RemoveInstantiatedGO(gameObject, localOnly: true, immediate: false, 0f);
		}
		GameObject gameObject2;
		GameObject gameObject4 = (instantiatedObjects[num] = (gameObject2 = val.gameObject));
		photonView = gameObject4.GetComponent<PhotonView>();
		if (photonView != null)
		{
			int viewID = photonView.viewID;
			if (!photonViewList.ContainsKey(viewID))
			{
				photonViewList.Add(viewID, photonView);
			}
		}
		if (actorGO.ContainsKey(photonPlayer.ActorID))
		{
			actorGO[photonPlayer.ActorID].Add(gameObject2);
		}
		else
		{
			actorGO.Add(photonPlayer.ActorID, new List<GameObject> { gameObject2 });
		}
		FengGameManagerMKII.instance.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), new PhotonMessageInfo(photonPlayer, (int)evData[6], photonView), SendMessageOptions.DontRequireReceiver);
		return val;
	}

	internal GameObject DoInstantiate2(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, GameObject resourceGameObject)
	{
		if (!(evData[(byte)0] is string))
		{
			FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)0");
			return null;
		}
		string text = (string)evData[(byte)0];
		if (!(evData[(byte)6] is int))
		{
			FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)6");
			return null;
		}
		int timestamp = (int)evData[(byte)6];
		if (!(evData[(byte)7] is int))
		{
			FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)7");
			return null;
		}
		int num = (int)evData[(byte)7];
		Vector3 position;
		if (evData.ContainsKey((byte)1))
		{
			if (!(evData[(byte)1] is Vector3))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)1");
				return null;
			}
			position = (Vector3)evData[(byte)1];
		}
		else
		{
			position = Vector3.zero;
		}
		Quaternion rotation = Quaternion.identity;
		if (evData.ContainsKey((byte)2))
		{
			if (!(evData[(byte)2] is Quaternion))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)2");
				return null;
			}
			rotation = (Quaternion)evData[(byte)2];
		}
		int num2 = 0;
		if (evData.ContainsKey((byte)3))
		{
			if (!(evData[(byte)3] is int))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)3");
				return null;
			}
			num2 = (int)evData[(byte)3];
		}
		short prefix = 0;
		if (evData.ContainsKey((byte)8))
		{
			if (!(evData[(byte)8] is short))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)8");
				return null;
			}
			prefix = (short)evData[(byte)8];
		}
		int[] array;
		if (evData.ContainsKey((byte)4))
		{
			if (!(evData[(byte)4] is int[]))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)4");
				return null;
			}
			array = (int[])evData[(byte)4];
		}
		else
		{
			array = new int[1] { num };
		}
		if (!InstantiateTracker.instance.checkObj(text, photonPlayer, array))
		{
			return null;
		}
		object[] instantiationData;
		if (evData.ContainsKey((byte)5))
		{
			if (!(evData[(byte)5] is object[]))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "DI: invalid (byte)5");
				return null;
			}
			instantiationData = (object[])evData[(byte)5];
		}
		else
		{
			instantiationData = null;
		}
		if (num2 != 0 && !allowedReceivingGroups.Contains(num2))
		{
			return null;
		}
		if (resourceGameObject == null)
		{
			if (text.StartsWith("RCAsset/"))
			{
				resourceGameObject = CacheResources.RCLoad<GameObject>(text.Substring(8));
			}
			else if (!CacheResources.Load(text, out resourceGameObject))
			{
				return null;
			}
		}
		PhotonView[] photonViewsInChildren = resourceGameObject.GetPhotonViewsInChildren();
		if (FengGameManagerMKII.instance != null)
		{
			PhotonView pView = FengGameManagerMKII.PView;
			if (pView.viewID == num || pView.viewID == photonViewsInChildren[0].viewID)
			{
				return null;
			}
		}
		if (photonViewsInChildren.Length != array.Length)
		{
			throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
		}
		for (int i = 0; i < array.Length; i++)
		{
			photonViewsInChildren[i].viewID = array[i];
			photonViewsInChildren[i].prefix = prefix;
			photonViewsInChildren[i].instantiationId = num;
		}
		StoreInstantiationData(num, instantiationData);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(resourceGameObject, position, rotation);
		for (int j = 0; j < array.Length; j++)
		{
			photonViewsInChildren[j].viewID = 0;
			photonViewsInChildren[j].prefix = -1;
			photonViewsInChildren[j].prefixBackup = -1;
			photonViewsInChildren[j].instantiationId = -1;
		}
		RemoveInstantiationData(num);
		if (instantiatedObjects.ContainsKey(num))
		{
			GameObject gameObject2 = instantiatedObjects[num];
			string text2 = string.Empty;
			if (gameObject2 != null)
			{
				PhotonView[] photonViewsInChildren2 = gameObject2.GetPhotonViewsInChildren();
				foreach (PhotonView photonView in photonViewsInChildren2)
				{
					if (photonView != null)
					{
						text2 = text2 + photonView.ToString() + ", ";
					}
				}
			}
			RemoveInstantiatedGO(gameObject2, localOnly: true);
		}
		instantiatedObjects.Add(num, gameObject);
		PhotonView component = gameObject.GetComponent<PhotonView>();
		if (component != null)
		{
			int viewID = component.viewID;
			if (!photonViewList.ContainsKey(viewID))
			{
				photonViewList.Add(viewID, component);
			}
		}
		gameObject.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), new PhotonMessageInfo(photonPlayer, timestamp, null), SendMessageOptions.DontRequireReceiver);
		return gameObject;
	}

	internal void EventRPC(PhotonView view, string methodName, RaiseEventOptions options, params object[] parameters)
	{
		if (blockSendingGroups.Contains(view.group))
		{
			return;
		}
		_ = view.viewID;
		_ = 1;
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			_ = options?.TargetActors;
		}
		int value = 0;
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable { 
		{
			(byte)0,
			view.viewID
		} };
		if (view.prefix > 0)
		{
			hashtable[(byte)1] = (short)view.prefix;
		}
		hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
		if (rpcShortcuts.TryGetValue(methodName, out value))
		{
			hashtable[(byte)5] = (byte)value;
		}
		else
		{
			hashtable[(byte)3] = methodName;
		}
		if (parameters != null && parameters.Length != 0)
		{
			hashtable[(byte)4] = parameters;
		}
		if (options != null && options.TargetActors.Contains(mLocalActor.ID))
		{
			options.TargetActors = options.TargetActors.Where((int id) => id != mLocalActor.ID).ToArray();
			ExecuteRPC(hashtable, mLocalActor);
			if (options.TargetActors.Length != 0)
			{
				OpRaiseEvent(200, hashtable, sendReliable: true, options);
			}
		}
		else
		{
			OpRaiseEvent(200, hashtable, sendReliable: true, options);
		}
	}

	public void ExecuteRPC(ExitGames.Client.Photon.Hashtable rpcData, PhotonPlayer sender)
	{
		if (rpcData.Keys.Count != 4 && rpcData.Keys.Count != 3)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC: < | > keys length. " + rpcData);
		}
		else if (!rpcData.ContainsKey((byte)0) || !rpcData.ContainsKey((byte)2) || (!rpcData.ContainsKey((byte)3) && !rpcData.ContainsKey((byte)5)))
		{
			FengGameManagerMKII.ThrowJusticeDefensive(sender, "rpc: missing key. " + rpcData);
		}
		else if (!(rpcData[(byte)0] is int) || !(rpcData[(byte)2] is int))
		{
			FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC: invalid (byte)0/2. " + rpcData);
		}
		else
		{
			if (rpcData == null || !rpcData.ContainsKey((byte)0))
			{
				return;
			}
			int num = (int)rpcData[(byte)0];
			int num2 = 0;
			if (rpcData.ContainsKey((byte)1))
			{
				if (!(rpcData[(byte)1] is short))
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC: (byte)1 not short. " + rpcData);
					return;
				}
				num2 = (short)rpcData[(byte)1];
			}
			bool flag = false;
			string text;
			if (rpcData.ContainsKey((byte)5))
			{
				if (!(rpcData[(byte)5] is byte))
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC: invalid (byte5). " + rpcData);
					return;
				}
				int num3 = (byte)rpcData[(byte)5];
				if (num3 > PhotonNetwork.PhotonServerSettings.RpcList.Count - 1)
				{
					return;
				}
				text = PhotonNetwork.PhotonServerSettings.RpcList[num3];
			}
			else
			{
				if (!(rpcData[(byte)3] is string))
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC: invalid (byte3). " + rpcData);
					return;
				}
				text = (string)rpcData[(byte)3];
				if (!sender.isLocal)
				{
					if (text == "loadskinRPC" && (int)rpcData[(byte)0] / 1000 == mLocalActor.ID)
					{
						FengGameManagerMKII.ThrowJusticeDefensive(sender, "Change skin" + rpcData);
						return;
					}
					switch (text)
					{
					case "loadskinRPC":
					case "SetWeatherRPC":
						if (base.ByteCountCurrentDispatch > 2700)
						{
							FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge (skin/weather) - ({base.ByteCountCurrentDispatch}). " + rpcData);
							return;
						}
						break;
					case "The_Last_Winter":
						flag = true;
						goto default;
					case "Among_The_Same_Clouds":
						FengGameManagerMKII.ThrowJusticeDefensive(sender, "Old Bad TLW");
						return;
					default:
						if (FengGameManagerMKII.level.StartsWith("Custom") || text == "customlevelRPC")
						{
							if (base.ByteCountCurrentDispatch > 20000)
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge (200) - ({base.ByteCountCurrentDispatch}). " + rpcData);
								return;
							}
						}
						else if (base.ByteCountCurrentDispatch > 1400)
						{
							FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge (200) - ({base.ByteCountCurrentDispatch}). " + rpcData);
							return;
						}
						break;
					}
				}
			}
			object[] array = null;
			if (rpcData.ContainsKey((byte)4))
			{
				if (!(rpcData[(byte)4] is object[]))
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC: invalid (byte4). " + rpcData);
					return;
				}
				array = (object[])rpcData[(byte)4];
				if (array.Length > 8)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC: long args length. " + rpcData);
					return;
				}
				if (flag)
				{
					if (array.Length != 2)
					{
						FengGameManagerMKII.ThrowJusticeDefensive(sender, "invalid TLW Detection: " + rpcData);
						return;
					}
					if (!(array[0] is int) || !(array[1] is string))
					{
						FengGameManagerMKII.ThrowJusticeDefensive(sender, "invalid TLW Detection: " + rpcData);
						return;
					}
					if (array[1].ToString() == "Benevolent Bomba")
					{
						if (FengGameManagerMKII.instance.tlwv > (int)array[0])
						{
							sender.OldTlw = true;
						}
						else if (FengGameManagerMKII.instance.tlwv < (int)array[0])
						{
							sender.NewTlw = true;
						}
						else
						{
							sender.Tlw = true;
						}
					}
				}
			}
			if (array == null)
			{
				array = new object[0];
			}
			PhotonView photonView = GetPhotonView(num);
			if (photonView == null)
			{
				bool flag2 = num / PhotonNetwork.MAX_VIEW_IDS == mLocalActor.ID;
				_ = sender.ID;
			}
			else
			{
				if (photonView.prefix != num2)
				{
					return;
				}
				if (text == string.Empty)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "Null String RPC. " + rpcData);
					return;
				}
				_ = PhotonNetwork.logLevel;
				_ = 2;
				if (photonView.group != 0 && !allowedReceivingGroups.Contains(photonView.group))
				{
					return;
				}
				Type[] array2 = new Type[0];
				if (array.Length != 0)
				{
					array2 = new Type[array.Length];
					int num4 = 0;
					foreach (object obj in array)
					{
						if (obj == null)
						{
							array2[num4] = null;
						}
						else
						{
							array2[num4] = obj.GetType();
						}
						num4++;
					}
				}
				int num5 = 0;
				int num6 = 0;
				UnityEngine.MonoBehaviour[] components = photonView.GetComponents<UnityEngine.MonoBehaviour>();
				foreach (UnityEngine.MonoBehaviour monoBehaviour in components)
				{
					if (monoBehaviour == null)
					{
						continue;
					}
					Type type = monoBehaviour.GetType();
					List<MethodInfo> list = null;
					if (monoRPCMethodsCache.ContainsKey(type))
					{
						list = monoRPCMethodsCache[type];
					}
					if (list == null)
					{
						List<MethodInfo> methods = SupportClass.GetMethods(type, typeof(RPC));
						monoRPCMethodsCache[type] = methods;
						list = methods;
					}
					if (list == null)
					{
						continue;
					}
					for (int k = 0; k < list.Count; k++)
					{
						MethodInfo methodInfo = list[k];
						if (!(methodInfo.Name == text))
						{
							continue;
						}
						rpcList.Add(text);
						num6++;
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (parameters.Length == array2.Length)
						{
							if (CheckTypeMatch(parameters, array2))
							{
								num5++;
								object obj2 = methodInfo.Invoke(monoBehaviour, array);
								if (methodInfo.ReturnType == typeof(IEnumerator))
								{
									monoBehaviour.StartCoroutine((IEnumerator)obj2);
								}
							}
						}
						else if (parameters.Length - 1 == array2.Length)
						{
							if (CheckTypeMatch(parameters, array2) && parameters[parameters.Length - 1].ParameterType == typeof(PhotonMessageInfo))
							{
								num5++;
								int timestamp = (int)rpcData[(byte)2];
								object[] array3 = new object[array.Length + 1];
								array.CopyTo(array3, 0);
								array3[array3.Length - 1] = new PhotonMessageInfo(sender, timestamp, photonView);
								object obj3 = methodInfo.Invoke(monoBehaviour, array3);
								if (methodInfo.ReturnType == typeof(IEnumerator))
								{
									monoBehaviour.StartCoroutine((IEnumerator)obj3);
								}
							}
						}
						else if (parameters.Length == 1 && parameters[0].ParameterType.IsArray)
						{
							num5++;
							object[] parameters2 = new object[1] { array };
							object obj4 = methodInfo.Invoke(monoBehaviour, parameters2);
							if (methodInfo.ReturnType == typeof(IEnumerator))
							{
								monoBehaviour.StartCoroutine((IEnumerator)obj4);
							}
						}
					}
				}
				if (num5 == 1)
				{
					return;
				}
				string text2 = string.Empty;
				foreach (Type type2 in array2)
				{
					if (text2 != string.Empty)
					{
						text2 += ", ";
					}
					text2 = ((type2 != null) ? (text2 + type2.Name) : (text2 + "null"));
				}
				if (!rpcList.Contains(text))
				{
					num = sender.ID;
					if (string.Concat(array).Length >= 0)
					{
						return;
					}
				}
				else if (rpcList.Contains(text))
				{
					int num7 = 0;
					foreach (string rpc in rpcList)
					{
						num7 = ((!(rpc == text)) ? (num7 + 1) : (num7 + 1));
					}
					return;
				}
				if (num5 != 0)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC " + text + " with too many params. " + rpcData);
				}
			}
		}
	}

	public object[] FetchInstantiationData(int instantiationId)
	{
		object[] value = null;
		if (instantiationId == 0)
		{
			return null;
		}
		tempInstantiationData.TryGetValue(instantiationId, out value);
		return value;
	}

	private void GameEnteredOnGameServer(OperationResponse operationResponse)
	{
		if (operationResponse.ReturnCode == 0)
		{
			states = PeerStates.Joined;
			mRoomToGetInto.isLocalClientInside = true;
			ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
			ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
			ReadoutProperties(gameProperties, pActorProperties, 0);
			int newID = (int)operationResponse[254];
			ChangeLocalID(newID);
			CheckMasterClient(-1);
			if (mPlayernameHasToBeUpdated)
			{
				SendPlayerName();
			}
			if (operationResponse.OperationCode == 227)
			{
				SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			}
			return;
		}
		switch (operationResponse.OperationCode)
		{
		case 225:
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				_ = operationResponse.ReturnCode;
				_ = 32758;
			}
			object[] parameters3 = new object[2] { operationResponse.ReturnCode, operationResponse.DebugMessage };
			SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, parameters3);
			break;
		}
		case 226:
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				_ = operationResponse.ReturnCode;
				_ = 32758;
			}
			object[] parameters2 = new object[2] { operationResponse.ReturnCode, operationResponse.DebugMessage };
			SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, parameters2);
			break;
		}
		case 227:
		{
			_ = PhotonNetwork.logLevel;
			_ = 1;
			object[] parameters = new object[2] { operationResponse.ReturnCode, operationResponse.DebugMessage };
			SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, parameters);
			break;
		}
		}
		DisconnectToReconnect2();
	}

	private ExitGames.Client.Photon.Hashtable GetActorPropertiesForActorNr(ExitGames.Client.Photon.Hashtable actorProperties, int actorNr)
	{
		if (actorProperties.ContainsKey(actorNr))
		{
			return (ExitGames.Client.Photon.Hashtable)actorProperties[actorNr];
		}
		return actorProperties;
	}

	public int GetInstantiatedObjectsId(GameObject go)
	{
		int result = -1;
		if (go == null)
		{
			return result;
		}
		PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
		if (photonViewsInChildren != null && photonViewsInChildren.Length != 0 && photonViewsInChildren[0] != null)
		{
			return photonViewsInChildren[0].instantiationId;
		}
		_ = PhotonNetwork.logLevel;
		_ = 1;
		return result;
	}

	private ExitGames.Client.Photon.Hashtable GetLocalActorProperties()
	{
		if (PhotonNetwork.player != null)
		{
			return PhotonNetwork.player.allProperties;
		}
		return new ExitGames.Client.Photon.Hashtable { [byte.MaxValue] = PlayerName };
	}

	protected internal static bool GetMethod(UnityEngine.MonoBehaviour monob, string methodType, out MethodInfo mi)
	{
		mi = null;
		if (monob != null && !string.IsNullOrEmpty(methodType))
		{
			List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), null);
			for (int i = 0; i < methods.Count; i++)
			{
				MethodInfo methodInfo = methods[i];
				if (methodInfo.Name.Equals(methodType))
				{
					mi = methodInfo;
					return true;
				}
			}
		}
		return false;
	}

	public PhotonView GetPhotonView(int viewID)
	{
		PhotonView value = null;
		if (!photonViewList.TryGetValue(viewID, out value))
		{
			PhotonView[] array = UnityEngine.Object.FindObjectsOfType<PhotonView>();
			foreach (PhotonView photonView in array)
			{
				if (photonView.viewID == viewID)
				{
					if (photonView.didAwake)
					{
						photonViewList[photonView.viewID] = photonView;
					}
					return photonView;
				}
			}
		}
		return value;
	}

	private PhotonPlayer GetPlayerWithID(int number)
	{
		if (mActors != null && mActors.ContainsKey(number))
		{
			return mActors[number];
		}
		return null;
	}

	public bool GetRegions()
	{
		if (server != ServerConnection.NameServer)
		{
			return false;
		}
		bool num = OpGetRegions(mAppId);
		if (num)
		{
			AvailableRegions = null;
		}
		return num;
	}

	private void HandleEventLeave(int actorID)
	{
		_ = PhotonNetwork.logLevel;
		_ = 1;
		if (actorID >= 0 && mActors.ContainsKey(actorID))
		{
			PhotonPlayer playerWithID = GetPlayerWithID(actorID);
			CheckMasterClient(actorID);
			if (mCurrentGame != null)
			{
				DestroyPlayerObjects(actorID, localOnly: true);
			}
			RemovePlayer(actorID, playerWithID);
			object[] parameters = new object[1] { playerWithID };
			SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerDisconnected, parameters);
		}
	}

	private void LeftLobbyCleanup()
	{
		mGameList = new Dictionary<string, RoomInfo>();
		mGameListCopy = new RoomInfo[0];
		if (insideLobby)
		{
			insideLobby = false;
			SendMonoMessage(PhotonNetworkingMessage.OnLeftLobby);
		}
	}

	private void LeftRoomCleanup()
	{
		bool num = mRoomToGetInto != null;
		bool num2 = ((mRoomToGetInto == null) ? PhotonNetwork.autoCleanUpPlayerObjects : mRoomToGetInto.autoCleanUp);
		hasSwitchedMC = false;
		mRoomToGetInto = null;
		mActors.Clear();
		mPlayerListCopy = new PhotonPlayer[0];
		mOtherPlayerListCopy = new PhotonPlayer[0];
		mMasterClient = null;
		allowedReceivingGroups = new HashSet<int>();
		blockSendingGroups = new HashSet<int>();
		mGameList = new Dictionary<string, RoomInfo>();
		mGameListCopy = new RoomInfo[0];
		isFetchingFriends = false;
		ChangeLocalID(-1);
		if (num2)
		{
			LocalCleanupAnythingInstantiated(destroyInstantiatedGameObjects: true);
			PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
		}
		if (num)
		{
			SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
		}
		HERO.BombStatsCounter.Clear();
	}

	protected internal void LoadLevelIfSynced()
	{
		if (!PhotonNetwork.automaticallySyncScene || PhotonNetwork.isMasterClient || PhotonNetwork.room == null || !PhotonNetwork.room.customProperties.ContainsKey("curScn"))
		{
			return;
		}
		object obj = PhotonNetwork.room.customProperties["curScn"];
		if (obj is int)
		{
			if (Application.loadedLevel != (int)obj)
			{
				PhotonNetwork.LoadLevel((int)obj);
			}
		}
		else if (obj is string && Application.loadedLevelName != (string)obj)
		{
			PhotonNetwork.LoadLevel((string)obj);
		}
	}

	public void LocalCleanPhotonView(PhotonView view)
	{
		view.destroyedByPhotonNetworkOrQuit = true;
		photonViewList.Remove(view.viewID);
	}

	protected internal void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
	{
		_ = tempInstantiationData.Count;
		_ = 0;
		if (destroyInstantiatedGameObjects)
		{
			foreach (GameObject item in new HashSet<GameObject>(instantiatedObjects.Values))
			{
				if (item != null && !FengGameManagerMKII.customObjects.Contains(item))
				{
					RemoveInstantiatedGO(item, localOnly: true);
				}
			}
		}
		tempInstantiationData.Clear();
		instantiatedObjects = new Dictionary<int, GameObject>();
		PhotonNetwork.lastUsedViewSubId = 0;
		PhotonNetwork.lastUsedViewSubIdStatic = 0;
	}

	public void NewSceneLoaded()
	{
		if (loadingLevelAndPausedNetwork)
		{
			loadingLevelAndPausedNetwork = false;
			PhotonNetwork.isMessageQueueRunning = true;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
		{
			if (photonView.Value == null)
			{
				list.Add(photonView.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			int key = list[i];
			photonViewList.Remove(key);
		}
		if (list.Count > 0)
		{
			_ = PhotonNetwork.logLevel;
			_ = 1;
		}
	}

	private bool ObjectIsSameWithInprecision(object one, object two)
	{
		if (one == null || two == null)
		{
			if (one == null)
			{
				return two == null;
			}
			return false;
		}
		if (one.Equals(two))
		{
			return true;
		}
		if (one is Vector3)
		{
			Vector3 target = (Vector3)one;
			Vector3 second = (Vector3)two;
			if (target.AlmostEquals(second, PhotonNetwork.precisionForVectorSynchronization))
			{
				return true;
			}
		}
		else if (one is Vector2)
		{
			Vector2 target2 = (Vector2)one;
			Vector2 second2 = (Vector2)two;
			if (target2.AlmostEquals(second2, PhotonNetwork.precisionForVectorSynchronization))
			{
				return true;
			}
		}
		else if (one is Quaternion)
		{
			Quaternion target3 = (Quaternion)one;
			Quaternion second3 = (Quaternion)two;
			if (target3.AlmostEquals(second3, PhotonNetwork.precisionForQuaternionSynchronization))
			{
				return true;
			}
		}
		else if (one is float)
		{
			float target4 = (float)one;
			float second4 = (float)two;
			if (target4.AlmostEquals(second4, PhotonNetwork.precisionForFloatSynchronization))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckOSR(object Evdata, PhotonPlayer sender)
	{
		if (sender == null)
		{
			return true;
		}
		if (sender.isLocal)
		{
			return true;
		}
		if (Evdata is ExitGames.Client.Photon.Hashtable)
		{
			ExitGames.Client.Photon.Hashtable hashtable = Evdata as ExitGames.Client.Photon.Hashtable;
			bool flag = false;
			if (hashtable.ContainsKey((byte)0))
			{
				if (hashtable[(byte)0] is int)
				{
					if (hashtable.ContainsKey((byte)1) && !(hashtable[(byte)1] is short))
					{
						FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) invalid (byte)1" + hashtable);
						return false;
					}
					foreach (object key in hashtable.Keys)
					{
						if (key is byte && (byte)key >= 2)
						{
							FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) byte keys length" + hashtable);
							return false;
						}
					}
					short num = 2;
					if (!hashtable.ContainsKey((byte)1))
					{
						num = (short)(num - 1);
					}
					short num2 = num;
					while (num2 < hashtable.Count)
					{
						ExitGames.Client.Photon.Hashtable hashtable2;
						if ((hashtable2 = hashtable[num2] as ExitGames.Client.Photon.Hashtable) != null)
						{
							if (hashtable2 == null)
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) hashtable2 null " + hashtable);
								return false;
							}
							flag = true;
							if (hashtable2.Keys.Count != 2)
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) keys length " + hashtable);
								return false;
							}
							if (!hashtable2.ContainsKey((byte)0) || !hashtable2.ContainsKey((byte)1))
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) missing key(s) " + hashtable);
								return false;
							}
							if (!(hashtable[(byte)0] is int) || !(hashtable2[(byte)1] is object[]))
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) invalid value type " + hashtable);
								return false;
							}
							if ((int)hashtable2[(byte)0] / 1000 != sender.ID)
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) photonview " + hashtable);
								return false;
							}
							object[] array = hashtable2[(byte)1] as object[];
							if (array.Length != 2 && array.Length != 3 && array.Length != 4)
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) invalid objArray length (" + array.Length + ") " + hashtable);
								return false;
							}
							bool flag2 = false;
							switch (array.Length)
							{
							case 2:
								if (array[0] is Vector3 && array[1] is Quaternion)
								{
									flag2 = true;
								}
								break;
							case 3:
								if (array[0] is Vector3 && array[1] is Quaternion && (array[2] is Vector3 || array[2] is Quaternion))
								{
									flag2 = true;
								}
								break;
							case 4:
								if (array[0] is Vector3 && array[1] is Quaternion && array[2] is Vector3 && array[3] is Quaternion)
								{
									flag2 = true;
								}
								break;
							}
							if (!flag2)
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, string.Concat("OSR(", base.ByteCountCurrentDispatch, "b) objArray not customType (length: ", array.Length, ")", hashtable, "\n0:", array[0], ", 1:", array[1], ",2:", array[2]));
								return false;
							}
							if (!flag)
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) not (short)2 " + hashtable);
								return false;
							}
							num2 = (short)(num2 + 1);
							continue;
						}
						FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) invalid index " + hashtable);
						return false;
					}
					return true;
				}
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) missing (byte)0 " + hashtable);
				return false;
			}
			FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) missing (byte)1 " + hashtable);
			return false;
		}
		FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR(" + base.ByteCountCurrentDispatch + "b) not hash ");
		return false;
	}

	public void OnEvent(EventData photonEvent)
	{
		int num = -1;
		PhotonPlayer photonPlayer = null;
		if (photonEvent.Parameters.ContainsKey(254))
		{
			num = (int)photonEvent[254];
			if (mActors.ContainsKey(num))
			{
				photonPlayer = mActors[num];
			}
			else if (photonEvent.Code != byte.MaxValue && photonEvent.Code != 254 && num != 0)
			{
				mActors.Add(num, new PhotonPlayer(isLocal: false, num, $"InvalidPlayerID{num}"));
				InRoomChat.Write("Received event" + photonEvent.Code + ", but we do not have this player:  " + num);
			}
		}
		if (photonEvent.Parameters.Count == 0)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(photonPlayer, "Event null parameters " + photonEvent);
			return;
		}
		if (photonPlayer == null || (photonPlayer != null && !FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID)) || photonEvent.Code == 254)
		{
			EventHandler(photonEvent.Code, photonPlayer, photonEvent, num);
		}
		externalListener.OnEvent(photonEvent);
	}

	private void EventHandler(byte code, PhotonPlayer sender, EventData photonEvent, int key)
	{
		switch (code)
		{
		case 176:
			if (base.ByteCountCurrentDispatch > 100)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
				return;
			}
			switch ((string)photonEvent[245])
			{
			case "bronze":
				sender.RRC = true;
				break;
			case "silver":
				sender.RRC = true;
				break;
			case "gold":
				sender.RRC = true;
				break;
			case "platin":
				sender.RRC = true;
				break;
			case "diamond":
				sender.RRC = true;
				break;
			case "master":
				sender.RRC = true;
				break;
			case "grandmaster":
				sender.RRC = true;
				break;
			case "top5":
				sender.RRC = true;
				break;
			case "legendary":
				sender.RRC = true;
				break;
			default:
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "Fake RRC Detection " + photonEvent.Parameters.ToStringFull());
				break;
			}
			return;
		case 200:
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "RPC not hash " + photonEvent.Parameters.ToStringFull());
			}
			else
			{
				ExecuteRPC(photonEvent[245] as ExitGames.Client.Photon.Hashtable, sender);
			}
			return;
		case 201:
		case 206:
		{
			if (base.ByteCountCurrentDispatch > 6500)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
				return;
			}
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "OSR not hash (" + base.ByteCountCurrentDispatch + ") " + photonEvent.Parameters.ToStringFull());
				return;
			}
			ExitGames.Client.Photon.Hashtable hashtable2 = photonEvent[245] as ExitGames.Client.Photon.Hashtable;
			if (hashtable2 == null || !(hashtable2[(byte)0] is int))
			{
				return;
			}
			int networkTime = (int)hashtable2[(byte)0];
			short correctPrefix = -1;
			short num5 = 1;
			if (hashtable2.ContainsKey((byte)1))
			{
				if (!(hashtable2[(byte)1] is short))
				{
					return;
				}
				correctPrefix = (short)hashtable2[(byte)1];
				num5 = 2;
			}
			for (short num6 = num5; num6 < hashtable2.Count; num6 = (short)(num6 + 1))
			{
				if (CheckOSR(photonEvent[245], sender))
				{
					OnSerializeRead(hashtable2[num6] as ExitGames.Client.Photon.Hashtable, sender, networkTime, correctPrefix);
				}
			}
			return;
		}
		case 202:
		{
			if (base.ByteCountCurrentDispatch > 200)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
				return;
			}
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "DI not hashtable " + photonEvent.Parameters.ToStringFull());
				return;
			}
			ExitGames.Client.Photon.Hashtable hashtable2 = photonEvent[245] as ExitGames.Client.Photon.Hashtable;
			if (hashtable2[(byte)0] is string)
			{
				string text3 = (string)hashtable2[(byte)0];
				if (text3 == null)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "DI null string " + photonEvent.Parameters.ToStringFull());
				}
				else if (text3.Length > 30)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "DI big string " + photonEvent.Parameters.ToStringFull());
				}
				else if (hashtable2.ContainsKey((byte)7))
				{
					int num7 = (int)hashtable2[(byte)7];
					int num8 = sender.ID * PhotonNetwork.MAX_VIEW_IDS;
					int num9 = num8 + PhotonNetwork.MAX_VIEW_IDS;
					if (num7 < num8 || num7 > num9)
					{
						FengGameManagerMKII.ThrowJusticeDefensive(sender, "DI: view " + photonEvent.Parameters.ToStringFull());
					}
					else if (hashtable2.ContainsKey((byte)6))
					{
						_ = (int)hashtable2[(byte)6];
						DoInstantiate2(hashtable2, sender, null);
					}
				}
			}
			else
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "DI: (byte)0 " + photonEvent.Parameters.ToStringFull());
			}
			return;
		}
		case 203:
			if (base.ByteCountCurrentDispatch > 500)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
			}
			else if (photonEvent.Parameters.Count != 1 && photonEvent.Parameters.Count != 2)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "Invalid Kick " + photonEvent.Parameters.ToStringFull());
			}
			else if (!sender.isMasterClient && !sender.isLocal)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "not MC kick " + photonEvent.Parameters.ToStringFull());
			}
			else if (sender.isMasterClient && !sender.isLocal)
			{
				string text = ((sender.customProperties["name"] == null) ? "「Unknown」" : ("「" + sender.customProperties["name"].ToString().StripColor().StripHex() + "」"));
				FleurConsole.instance.Log("<color=red><b>[Kicked]</b></color> by " + text + " <color=#" + FengGameManagerMKII.alternativefinalhex() + ">{" + sender.ID + "}</color>", FleurConsole.DebugType.ANTI);
				PhotonNetwork.LeaveRoom();
				PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.DisconnectByServerLogic);
			}
			return;
		case 204:
		{
			if (base.ByteCountCurrentDispatch > 100)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
				return;
			}
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "204 not hash " + photonEvent.Parameters.ToStringFull());
				return;
			}
			ExitGames.Client.Photon.Hashtable hashtable2 = photonEvent[245] as ExitGames.Client.Photon.Hashtable;
			if (hashtable2 == null)
			{
				return;
			}
			if (hashtable2.Keys.Count != 1)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "204 keys length " + photonEvent.Parameters.ToStringFull());
			}
			else if (hashtable2[(byte)0] is int)
			{
				int key2 = (int)hashtable2[(byte)0];
				GameObject value = null;
				instantiatedObjects.TryGetValue(key2, out value);
				if (value != null && sender != null)
				{
					RemoveInstantiatedGO(value, localOnly: true);
				}
			}
			else
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "204 invalid (byte)0 " + photonEvent.Parameters.ToStringFull());
			}
			return;
		}
		case 207:
		{
			if (base.ByteCountCurrentDispatch > 100)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
				return;
			}
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "207 not hash " + photonEvent.Parameters.ToStringFull());
				return;
			}
			ExitGames.Client.Photon.Hashtable hashtable2 = photonEvent[245] as ExitGames.Client.Photon.Hashtable;
			if (hashtable2 == null)
			{
				return;
			}
			if (hashtable2.Keys.Count != 1)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "207 keys length " + photonEvent.Parameters.ToStringFull());
			}
			else if (hashtable2[(byte)0] is int)
			{
				int num2 = (int)hashtable2[(byte)0];
				if (num2 < 0 && sender != null && (sender.isMasterClient || sender.isLocal))
				{
					DestroyAll(localOnly: true);
				}
				else if (sender != null && (sender.isMasterClient || sender.isLocal || num2 != PhotonNetwork.player.ID))
				{
					DestroyPlayerObjects(num2, localOnly: true);
				}
			}
			else
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "207 invalid (byte)0 " + photonEvent.Parameters.ToStringFull());
			}
			return;
		}
		case 208:
		{
			if (base.ByteCountCurrentDispatch > 500)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
				return;
			}
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "208 not hash" + photonEvent.Parameters.ToStringFull());
				return;
			}
			ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			if (hashtable2.Keys.Count != 1)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "208 keys length" + photonEvent.Parameters.ToStringFull());
				return;
			}
			if (!(hashtable2[(byte)1] is int))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "208 invalid (byte)1" + photonEvent.Parameters.ToStringFull());
				return;
			}
			int num4 = (int)hashtable2[(byte)1];
			if (sender != null && sender.isMasterClient && num4 == sender.ID)
			{
				return;
			}
			if (sender != null || !sender.isMasterClient || !sender.isLocal)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "passing/stealing MC.");
				if (PhotonNetwork.isMasterClient)
				{
					FengGameManagerMKII.noRestart = true;
					PhotonNetwork.SetMasterClient(PhotonNetwork.player);
				}
			}
			else if (num4 == mLocalActor.ID)
			{
				SetMasterClient(num4, sync: false);
			}
			else if (sender == null || sender.isMasterClient || sender.isLocal)
			{
				SetMasterClient(num4, sync: false);
			}
			return;
		}
		case 226:
			if (sender == null)
			{
				object obj2 = photonEvent[229];
				object obj3 = photonEvent[227];
				object obj4 = photonEvent[228];
				if (obj2 is int && obj3 is int && obj4 is int)
				{
					mPlayersInRoomsCount = (int)obj2;
					mPlayersOnMasterCount = (int)obj3;
					mGameCount = (int)obj4;
				}
			}
			else if (!FengGameManagerMKII.ignoreList.Contains(key))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "226" + photonEvent.Parameters.ToStringFull());
			}
			return;
		case 228:
			if (!FengGameManagerMKII.ignoreList.Contains(sender.ID))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "Roast" + photonEvent.Parameters.ToStringFull());
			}
			return;
		case 229:
			if (sender == null)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = photonEvent[222] as ExitGames.Client.Photon.Hashtable;
				if (hashtable2 == null)
				{
					return;
				}
				foreach (DictionaryEntry item in hashtable2)
				{
					string text4 = (string)item.Key;
					RoomInfo roomInfo = new RoomInfo(text4, (ExitGames.Client.Photon.Hashtable)item.Value);
					if (roomInfo.removedFromList)
					{
						mGameList.Remove(text4);
					}
					else
					{
						mGameList[text4] = roomInfo;
					}
				}
				mGameListCopy = new RoomInfo[mGameList.Count];
				mGameList.Values.CopyTo(mGameListCopy, 0);
				SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
			}
			else if (!FengGameManagerMKII.ignoreList.Contains(key))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "229 " + photonEvent.Parameters.ToStringFull());
			}
			return;
		case 230:
			if (sender == null)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = photonEvent[222] as ExitGames.Client.Photon.Hashtable;
				if (hashtable2 == null)
				{
					return;
				}
				mGameList = new Dictionary<string, RoomInfo>();
				foreach (DictionaryEntry item2 in hashtable2)
				{
					string text2 = (string)item2.Key;
					mGameList[text2] = new RoomInfo(text2, (ExitGames.Client.Photon.Hashtable)item2.Value);
				}
				mGameListCopy = new RoomInfo[mGameList.Count];
				mGameList.Values.CopyTo(mGameListCopy, 0);
				SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
			}
			else if (!FengGameManagerMKII.ignoreList.Contains(key))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "230 " + photonEvent.Parameters.ToStringFull());
			}
			return;
		case 253:
		{
			if (base.ByteCountCurrentDispatch > 580)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
				return;
			}
			if (photonEvent == null || !(photonEvent[253] is int))
			{
				if (sender != null)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(sender, "false prop change");
				}
				return;
			}
			object obj = photonEvent[253];
			if (!(obj is int))
			{
				return;
			}
			int num = (int)obj;
			ExitGames.Client.Photon.Hashtable gameProperties = null;
			ExitGames.Client.Photon.Hashtable hashtable = null;
			if (num != 0)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = photonEvent[251] as ExitGames.Client.Photon.Hashtable;
				if (hashtable2 != null)
				{
					hashtable = hashtable2;
					if (sender != null)
					{
						PhotonPlayer photonPlayer = PhotonPlayer.Find(num);
						if (photonPlayer == null)
						{
							FengGameManagerMKII.ThrowJusticeDefensive(sender, "deletes all props");
							return;
						}
						hashtable["sender"] = sender;
						if (sender != photonPlayer)
						{
							if (hashtable.ContainsKey("isTitan"))
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "changing race");
								return;
							}
							if (hashtable.ContainsKey("dead"))
							{
								FengGameManagerMKII.ThrowJusticeDefensive(sender, "changing visibility");
								return;
							}
							ExitGames.Client.Photon.Hashtable customProperties = photonPlayer.customProperties;
							List<object> list = new List<object>();
							ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
							foreach (DictionaryEntry item3 in hashtable)
							{
								if (!list.Contains(item3.Key) && !validProps.Contains(item3.Key.ToString()))
								{
									FengGameManagerMKII.ThrowJusticeDefensive(sender, "sets custom prop on " + photonPlayer.uiname.hexColor() + " {" + photonPlayer.ID + "}: " + hashtable);
									hashtable3.Add(item3.Key, null);
								}
								if (!customProperties.ContainsKey(item3.Key))
								{
									continue;
								}
								if (item3.Value == null)
								{
									FengGameManagerMKII.ThrowJusticeDefensive(sender, string.Concat("deletes prop ", item3.Key, "of ", photonPlayer.uiname.hexColor(), " {", photonPlayer.ID, "}"));
									hashtable3.Add(item3.Key, customProperties[item3.Key]);
								}
								else if (item3.Value != null && !hashtable.ContainsKey("kills") && !hashtable.ContainsKey("deaths") && !hashtable.ContainsKey("max_dmg") && !hashtable.ContainsKey("total_dmg"))
								{
									FengGameManagerMKII.ThrowJusticeDefensive(sender, string.Concat("sets value on ", photonPlayer.uiname.hexColor(), " {", photonPlayer.ID, "}: (", item3.Key, ") (", item3.Value, ")"));
									hashtable3.Add(item3.Key, customProperties[item3.Key]);
								}
								else if (!sender.isMasterClient)
								{
									if (hashtable.ContainsKey("kills") && (Convert.ToInt32(hashtable["kills"].ToString()) - photonPlayer.customProperties["kills"].ToInt32() > 1 || Convert.ToInt32(hashtable["kills"].ToString()) < 0))
									{
										FengGameManagerMKII.ThrowJusticeDefensive(sender, string.Concat("sets kill on ", photonPlayer.uiname.hexColor(), " {", photonPlayer.ID, "}: (", item3.Value, ")"));
										hashtable3.Add(item3.Key, customProperties[item3.Key]);
									}
									else if (hashtable.ContainsKey("max_dmg") && (Convert.ToInt32(hashtable["max_dmg"].ToString()) >= 16000 || Convert.ToInt32(hashtable["max_dmg"].ToString()) < 0))
									{
										FengGameManagerMKII.ThrowJusticeDefensive(sender, string.Concat("sets max_dmg on ", photonPlayer.uiname.hexColor(), " {", photonPlayer.ID, "}: (", item3.Value, ")"));
										hashtable3.Add(item3.Key, customProperties[item3.Key]);
									}
									else if (hashtable.ContainsKey("total_dmg") && (Convert.ToInt32(hashtable["total_dmg"].ToString()) - photonPlayer.customProperties["total_dmg"].ToInt32() >= 16000 || Convert.ToInt32(hashtable["total_dmg"].ToString()) < 0))
									{
										FengGameManagerMKII.ThrowJusticeDefensive(sender, string.Concat("sets total_dmg on ", photonPlayer.uiname.hexColor(), " {", photonPlayer.ID, "}: (", item3.Value, ")"));
										hashtable3.Add(item3.Key, customProperties[item3.Key]);
									}
									else if (hashtable.ContainsKey("deaths") && (Convert.ToInt32(hashtable["deaths"].ToString()) - photonPlayer.customProperties["deaths"].ToInt32() > 1 || Convert.ToInt32(hashtable["deaths"].ToString()) < 0))
									{
										FengGameManagerMKII.ThrowJusticeDefensive(sender, string.Concat("sets deaths on ", photonPlayer.uiname.hexColor(), " {", photonPlayer.ID, "}: (", item3.Value, ")"));
										hashtable3.Add(item3.Key, customProperties[item3.Key]);
									}
								}
							}
							if (hashtable3.Count > 0)
							{
								PhotonNetwork.player.SetCustomProperties(hashtable3);
							}
						}
						if (hashtable.ContainsKey("RCBombRadius") && (RCextensions.returnFloatFromObject(hashtable["RCBombRadius"]) - 20f) / 4f > 10f)
						{
							FengGameManagerMKII.ThrowJusticeDefensive(sender, "Bomb Rad " + (RCextensions.returnFloatFromObject(hashtable["RCBombRadius"]) - 20f) / 4f);
							return;
						}
						if (!sender.isLocal && num == sender.ID && (hashtable.ContainsKey("statACL") || hashtable.ContainsKey("statBLA") || hashtable.ContainsKey("statGAS") || hashtable.ContainsKey("statSPD")))
						{
							if (hashtable.ContainsKey("statACL") && RCextensions.returnIntFromObject(hashtable["statACL"]) > 150)
							{
								FengGameManagerMKII.instance.kickPlayerRC(sender, ban: true, "excessive stats.");
								return;
							}
							if (hashtable.ContainsKey("statBLA") && RCextensions.returnIntFromObject(hashtable["statBLA"]) > 125)
							{
								FengGameManagerMKII.instance.kickPlayerRC(sender, ban: true, "excessive stats.");
								return;
							}
							if (hashtable.ContainsKey("statGAS") && RCextensions.returnIntFromObject(hashtable["statGAS"]) > 150)
							{
								FengGameManagerMKII.instance.kickPlayerRC(sender, ban: true, "excessive stats.");
								return;
							}
							if (hashtable.ContainsKey("statSPD") && RCextensions.returnIntFromObject(hashtable["statSPD"]) > 140)
							{
								FengGameManagerMKII.instance.kickPlayerRC(sender, ban: true, "excessive stats.");
								return;
							}
						}
						if (hashtable.ContainsKey("name"))
						{
							if (num != sender.ID)
							{
								InstantiateTracker.instance.resetPropertyTracking(num);
							}
							else if (!InstantiateTracker.instance.PropertiesChanged(sender))
							{
								return;
							}
						}
						if (FengGameManagerMKII.instance.DCPeopleList.Contains(RCextensions.returnStringFromObject(sender.customProperties["name"])))
						{
							FengGameManagerMKII.ThrowJusticeDefensive(sender, "DC List");
						}
					}
				}
			}
			else
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = photonEvent[251] as ExitGames.Client.Photon.Hashtable;
				if (hashtable2 == null)
				{
					return;
				}
				gameProperties = hashtable2;
			}
			ReadoutProperties(gameProperties, hashtable, num);
			return;
		}
		case 254:
			HandleEventLeave(key);
			return;
		case byte.MaxValue:
		{
			if (sender == null)
			{
				bool isLocal = mLocalActor.ID == key;
				AddNewPlayer(key, new PhotonPlayer(isLocal, key, photonEvent[249] as ExitGames.Client.Photon.Hashtable));
				ResetPhotonViewsOnSerialize();
			}
			if (key != mLocalActor.ID)
			{
				object[] parameters = new object[1] { mActors[key] };
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerConnected, parameters);
				return;
			}
			int[] array = (int[])photonEvent[252];
			foreach (int num3 in array)
			{
				if (mLocalActor.ID != num3 && !mActors.ContainsKey(num3))
				{
					AddNewPlayer(num3, new PhotonPlayer(isLocal: false, num3, string.Empty));
				}
			}
			if (mLastJoinType == JoinType.JoinOrCreateOnDemand && mLocalActor.ID == 1)
			{
				SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			}
			SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			if (FengGameManagerMKII.instance.DCPeopleList.Contains(RCextensions.returnStringFromObject(sender.customProperties["name"])))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "Ban List");
			}
			return;
		}
		}
		if (photonEvent.Code < 200 && PhotonNetwork.OnEventCall != null)
		{
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, "Invalid event no hash " + photonEvent.Parameters.ToStringFull());
				return;
			}
			if (base.ByteCountCurrentDispatch > 1000)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(sender, $"Huge Invalid ({photonEvent.Code}) - ({base.ByteCountCurrentDispatch}) - {photonEvent.Parameters.ToStringFull()}");
				return;
			}
			object obj = photonEvent[245];
			PhotonNetwork.OnEventCall(photonEvent.Code, obj, key);
		}
	}

	public void rejoin()
	{
		PhotonNetwork.JoinRoom(mRoomToGetInto.name);
	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{
		if (PhotonNetwork.networkingPeer.states == PeerStates.Disconnecting)
		{
			_ = PhotonNetwork.logLevel;
			_ = 1;
			return;
		}
		if (operationResponse.ReturnCode == 0)
		{
			if (PhotonNetwork.logLevel < PhotonLogLevel.Informational)
			{
			}
		}
		else if (operationResponse.ReturnCode != -3 && operationResponse.ReturnCode != 32752)
		{
			_ = PhotonNetwork.logLevel;
			_ = 1;
		}
		if (operationResponse.Parameters.ContainsKey(221))
		{
			if (CustomAuthenticationValues == null)
			{
				CustomAuthenticationValues = new AuthenticationValues();
			}
			CustomAuthenticationValues.Secret = operationResponse[221] as string;
		}
		switch (operationResponse.OperationCode)
		{
		case 219:
		{
			object[] parameters6 = new object[1] { operationResponse };
			SendMonoMessage(PhotonNetworkingMessage.OnWebRpcResponse, parameters6);
			break;
		}
		case 220:
		{
			if (operationResponse.ReturnCode == short.MaxValue)
			{
				object[] parameters7 = new object[1] { DisconnectCause.InvalidAuthentication };
				SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters7);
				states = PeerStates.Disconnecting;
				Disconnect();
				return;
			}
			string[] array3 = operationResponse[210] as string[];
			string[] array4 = operationResponse[230] as string[];
			if (array3 == null || array4 == null || array3.Length != array4.Length)
			{
				break;
			}
			AvailableRegions = new List<Region>(array3.Length);
			for (int j = 0; j < array3.Length; j++)
			{
				string text2 = array3[j];
				if (!string.IsNullOrEmpty(text2))
				{
					CloudRegionCode code = Region.Parse(text2.ToLower());
					Region item2 = new Region
					{
						Code = code,
						HostAndPort = array4[j]
					};
					AvailableRegions.Add(item2);
				}
			}
			break;
		}
		case 222:
		{
			bool[] array = operationResponse[1] as bool[];
			string[] array2 = operationResponse[2] as string[];
			if (array != null && array2 != null && friendListRequested != null && array.Length == friendListRequested.Length)
			{
				List<FriendInfo> list = new List<FriendInfo>(friendListRequested.Length);
				for (int i = 0; i < friendListRequested.Length; i++)
				{
					FriendInfo item = new FriendInfo
					{
						Name = friendListRequested[i],
						Room = array2[i],
						IsOnline = array[i]
					};
					list.Insert(i, item);
				}
				PhotonNetwork.Friends = list;
			}
			friendListRequested = null;
			isFetchingFriends = false;
			friendListTimestamp = Environment.TickCount;
			if (friendListTimestamp == 0)
			{
				friendListTimestamp = 1;
			}
			SendMonoMessage(PhotonNetworkingMessage.OnUpdatedFriendList);
			break;
		}
		case 225:
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.ReturnCode != 32760)
				{
					if (PhotonNetwork.logLevel < PhotonLogLevel.Informational)
					{
					}
				}
				else
				{
					_ = PhotonNetwork.logLevel;
					_ = 2;
				}
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed);
			}
			else
			{
				string name = (string)operationResponse[byte.MaxValue];
				mRoomToGetInto.name = name;
				mGameserver = (string)operationResponse[230];
				DisconnectToReconnect2();
			}
			break;
		case 226:
			if (server != ServerConnection.GameServer)
			{
				if (operationResponse.ReturnCode == 0)
				{
					mGameserver = (string)operationResponse[230];
					DisconnectToReconnect2();
				}
				else
				{
					_ = PhotonNetwork.logLevel;
					_ = 1;
					SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed);
				}
			}
			else
			{
				GameEnteredOnGameServer(operationResponse);
			}
			break;
		case 227:
			if (server == ServerConnection.GameServer)
			{
				GameEnteredOnGameServer(operationResponse);
			}
			else if (operationResponse.ReturnCode == 0)
			{
				string text = (string)operationResponse[byte.MaxValue];
				if (!string.IsNullOrEmpty(text))
				{
					mRoomToGetInto.name = text;
				}
				mGameserver = (string)operationResponse[230];
				DisconnectToReconnect2();
			}
			else
			{
				_ = PhotonNetwork.logLevel;
				_ = 1;
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed);
			}
			break;
		case 228:
			states = PeerStates.Authenticated;
			LeftLobbyCleanup();
			break;
		case 229:
			states = PeerStates.JoinedLobby;
			insideLobby = true;
			SendMonoMessage(PhotonNetworkingMessage.OnJoinedLobby);
			break;
		case 230:
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.ReturnCode != -2)
				{
					if (operationResponse.ReturnCode == short.MaxValue)
					{
						object[] parameters = new object[1] { DisconnectCause.InvalidAuthentication };
						SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters);
					}
					else if (operationResponse.ReturnCode == 32755)
					{
						object[] parameters2 = new object[1] { operationResponse.DebugMessage };
						SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationFailed, parameters2);
					}
					else
					{
						FengGameManagerMKII.UpdateStatus = "Authentication failed: " + operationResponse.DebugMessage;
						GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
						UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
						Application.LoadLevel("menu");
						PhotonNetwork.Disconnect();
					}
				}
				states = PeerStates.Disconnecting;
				Disconnect();
				if (operationResponse.ReturnCode == 32757)
				{
					_ = PhotonNetwork.logLevel;
					_ = 1;
					SendMonoMessage(PhotonNetworkingMessage.OnPhotonMaxCccuReached);
					object[] parameters3 = new object[1] { DisconnectCause.MaxCcuReached };
					SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters3);
				}
				else if (operationResponse.ReturnCode == 32756)
				{
					_ = PhotonNetwork.logLevel;
					_ = 1;
					object[] parameters4 = new object[1] { DisconnectCause.InvalidRegion };
					SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters4);
				}
				else if (operationResponse.ReturnCode == 32753)
				{
					_ = PhotonNetwork.logLevel;
					_ = 1;
					object[] parameters5 = new object[1] { DisconnectCause.AuthenticationTicketExpired };
					SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters5);
				}
			}
			else if (server != ServerConnection.NameServer)
			{
				if (server == ServerConnection.MasterServer)
				{
					if (PhotonNetwork.autoJoinLobby)
					{
						states = PeerStates.Authenticated;
						OpJoinLobby(lobby);
					}
					else
					{
						states = PeerStates.ConnectedToMaster;
						SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
					}
				}
				else if (server == ServerConnection.GameServer)
				{
					states = PeerStates.Joining;
					if (mLastJoinType == JoinType.JoinGame || mLastJoinType == JoinType.JoinRandomGame || mLastJoinType == JoinType.JoinOrCreateOnDemand)
					{
						OpJoinRoom(mRoomToGetInto.name, mRoomOptionsForCreate, mRoomToEnterLobby, mLastJoinType == JoinType.JoinOrCreateOnDemand);
					}
					else if (mLastJoinType == JoinType.CreateGame)
					{
						OpCreateGame(mRoomToGetInto.name, mRoomOptionsForCreate, mRoomToEnterLobby);
					}
				}
			}
			else
			{
				MasterServerAddress = operationResponse[230] as string;
				DisconnectToReconnect2();
			}
			break;
		case 251:
		{
			ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
			ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
			ReadoutProperties(gameProperties, pActorProperties, 0);
			break;
		}
		case 254:
			DisconnectToReconnect2();
			break;
		}
		externalListener.OnOperationResponse(operationResponse);
	}

	private void OnSerializeRead(ExitGames.Client.Photon.Hashtable data, PhotonPlayer sender, int networkTime, short correctPrefix)
	{
		int viewID = (int)data[(byte)0];
		PhotonView photonView = GetPhotonView(viewID);
		if (photonView == null || (photonView.prefix > 0 && correctPrefix != photonView.prefix) || (photonView.group != 0 && !allowedReceivingGroups.Contains(photonView.group)))
		{
			return;
		}
		if (photonView.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			if (!DeltaCompressionRead(photonView, data))
			{
				_ = PhotonNetwork.logLevel;
				_ = 1;
				return;
			}
			photonView.lastOnSerializeDataReceived = data[(byte)1] as object[];
		}
		if (photonView.observed is UnityEngine.MonoBehaviour)
		{
			object[] incomingData = data[(byte)1] as object[];
			PhotonStream pStream = new PhotonStream(write: false, incomingData);
			PhotonMessageInfo info = new PhotonMessageInfo(sender, networkTime, photonView);
			photonView.ExecuteOnSerialize(pStream, info);
		}
		else if (photonView.observed is Transform)
		{
			object[] array = data[(byte)1] as object[];
			Transform transform = (Transform)photonView.observed;
			if (array.Length >= 1 && array[0] != null)
			{
				transform.localPosition = (Vector3)array[0];
			}
			if (array.Length >= 2 && array[1] != null)
			{
				transform.localRotation = (Quaternion)array[1];
			}
			if (array.Length >= 3 && array[2] != null)
			{
				transform.localScale = (Vector3)array[2];
			}
		}
		else if (photonView.observed is Rigidbody)
		{
			object[] array2 = data[(byte)1] as object[];
			Rigidbody rigidbody = (Rigidbody)photonView.observed;
			if (array2.Length >= 1 && array2[0] != null)
			{
				rigidbody.velocity = (Vector3)array2[0];
			}
			if (array2.Length >= 2 && array2[1] != null)
			{
				rigidbody.angularVelocity = (Vector3)array2[1];
			}
		}
	}

	private ExitGames.Client.Photon.Hashtable OnSerializeWrite(PhotonView view)
	{
		List<object> list = new List<object>();
		if (view.observed is UnityEngine.MonoBehaviour)
		{
			PhotonStream photonStream = new PhotonStream(write: true, null);
			PhotonMessageInfo info = new PhotonMessageInfo(mLocalActor, base.ServerTimeInMilliSeconds, view);
			view.ExecuteOnSerialize(photonStream, info);
			if (photonStream.Count == 0)
			{
				return null;
			}
			list = photonStream.data;
		}
		else if (view.observed is Transform)
		{
			Transform transform = (Transform)view.observed;
			if (view.onSerializeTransformOption != 0 && view.onSerializeTransformOption != OnSerializeTransform.PositionAndRotation && view.onSerializeTransformOption != OnSerializeTransform.All)
			{
				list.Add(null);
			}
			else
			{
				list.Add(transform.localPosition);
			}
			if (view.onSerializeTransformOption != OnSerializeTransform.OnlyRotation && view.onSerializeTransformOption != OnSerializeTransform.PositionAndRotation && view.onSerializeTransformOption != OnSerializeTransform.All)
			{
				list.Add(null);
			}
			else
			{
				list.Add(transform.localRotation);
			}
			if (view.onSerializeTransformOption == OnSerializeTransform.OnlyScale || view.onSerializeTransformOption == OnSerializeTransform.All)
			{
				list.Add(transform.localScale);
			}
		}
		else
		{
			if (!(view.observed is Rigidbody))
			{
				return null;
			}
			Rigidbody rigidbody = (Rigidbody)view.observed;
			if (view.onSerializeRigidBodyOption != OnSerializeRigidBody.OnlyAngularVelocity)
			{
				list.Add(rigidbody.velocity);
			}
			else
			{
				list.Add(null);
			}
			if (view.onSerializeRigidBodyOption != 0)
			{
				list.Add(rigidbody.angularVelocity);
			}
		}
		object[] array = list.ToArray();
		if (view.synchronization == ViewSynchronization.UnreliableOnChange)
		{
			if (AlmostEquals(array, view.lastOnSerializeDataSent))
			{
				if (view.mixedModeIsReliable)
				{
					return null;
				}
				view.mixedModeIsReliable = true;
				view.lastOnSerializeDataSent = array;
			}
			else
			{
				view.mixedModeIsReliable = false;
				view.lastOnSerializeDataSent = array;
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		hashtable[(byte)1] = array;
		if (view.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			bool num = DeltaCompressionWrite(view, hashtable);
			view.lastOnSerializeDataSent = array;
			if (!num)
			{
				return null;
			}
		}
		return hashtable;
	}

	public void OnStatusChanged(StatusCode statusCode)
	{
		_ = PhotonNetwork.logLevel;
		_ = 1;
		switch (statusCode)
		{
		case StatusCode.SecurityExceptionOnConnect:
		case StatusCode.ExceptionOnConnect:
		{
			states = PeerStates.PeerCreated;
			if (CustomAuthenticationValues != null)
			{
				CustomAuthenticationValues.Secret = null;
			}
			DisconnectCause disconnectCause = (DisconnectCause)statusCode;
			object[] parameters = new object[1] { disconnectCause };
			SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters);
			break;
		}
		case StatusCode.Connect:
			if (states == PeerStates.ConnectingToNameServer)
			{
				_ = PhotonNetwork.logLevel;
				_ = 2;
				server = ServerConnection.NameServer;
				if (CustomAuthenticationValues != null)
				{
					CustomAuthenticationValues.Secret = null;
				}
			}
			if (states == PeerStates.ConnectingToGameserver)
			{
				_ = PhotonNetwork.logLevel;
				_ = 2;
				server = ServerConnection.GameServer;
				states = PeerStates.ConnectedToGameserver;
			}
			if (states == PeerStates.ConnectingToMasterserver)
			{
				_ = PhotonNetwork.logLevel;
				_ = 2;
				server = ServerConnection.MasterServer;
				states = PeerStates.ConnectedToMaster;
				if (IsInitialConnect)
				{
					IsInitialConnect = false;
					SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton);
				}
			}
			EstablishEncryption();
			if (IsAuthorizeSecretAvailable)
			{
				didAuthenticate = OpAuthenticate(mAppId, mAppVersionPun, PlayerName, CustomAuthenticationValues, CloudRegion.ToString());
				if (didAuthenticate)
				{
					states = PeerStates.Authenticating;
				}
			}
			break;
		case StatusCode.Disconnect:
			didAuthenticate = false;
			isFetchingFriends = false;
			if (server == ServerConnection.GameServer)
			{
				LeftRoomCleanup();
			}
			if (server == ServerConnection.MasterServer)
			{
				LeftLobbyCleanup();
			}
			if (states == PeerStates.DisconnectingFromMasterserver)
			{
				if (Connect(mGameserver, ServerConnection.GameServer))
				{
					states = PeerStates.ConnectingToGameserver;
				}
				break;
			}
			if (states == PeerStates.DisconnectingFromGameserver || states == PeerStates.DisconnectingFromNameServer)
			{
				if (Connect(MasterServerAddress, ServerConnection.MasterServer))
				{
					states = PeerStates.ConnectingToMasterserver;
				}
				break;
			}
			if (CustomAuthenticationValues != null)
			{
				CustomAuthenticationValues.Secret = null;
			}
			states = PeerStates.PeerCreated;
			SendMonoMessage(PhotonNetworkingMessage.OnDisconnectedFromPhoton);
			break;
		case StatusCode.Exception:
			if (IsInitialConnect)
			{
				if (base.ServerAddress == null || base.ServerAddress.StartsWith("127.0.0.1"))
				{
					_ = base.ServerAddress == mGameserver;
				}
				states = PeerStates.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				object[] parameters4 = new object[1] { disconnectCause };
				SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters4);
			}
			else
			{
				states = PeerStates.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				object[] parameters5 = new object[1] { disconnectCause };
				SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters5);
			}
			Disconnect();
			break;
		case StatusCode.ExceptionOnReceive:
		case StatusCode.TimeoutDisconnect:
		case StatusCode.DisconnectByServerTimeout:
		case StatusCode.DisconnectByServerUserLimit:
		case StatusCode.DisconnectByServerLogic:
			if (IsInitialConnect)
			{
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				object[] parameters2 = new object[1] { disconnectCause };
				SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters2);
			}
			else
			{
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				object[] parameters3 = new object[1] { disconnectCause };
				SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters3);
			}
			if (CustomAuthenticationValues != null)
			{
				CustomAuthenticationValues.Secret = null;
			}
			Disconnect();
			break;
		case StatusCode.EncryptionEstablished:
			if (server == ServerConnection.NameServer)
			{
				states = PeerStates.ConnectedToNameServer;
				if (!didAuthenticate && CloudRegion == CloudRegionCode.none)
				{
					OpGetRegions(mAppId);
				}
			}
			if (!didAuthenticate && (!IsUsingNameServer || CloudRegion != CloudRegionCode.none))
			{
				didAuthenticate = OpAuthenticate(mAppId, mAppVersionPun, PlayerName, CustomAuthenticationValues, CloudRegion.ToString());
				if (didAuthenticate)
				{
					states = PeerStates.Authenticating;
				}
			}
			break;
		case StatusCode.EncryptionFailedToEstablish:
			OpAuthenticate(mAppId, mAppVersionPun, PlayerName, CustomAuthenticationValues, CloudRegion.ToString());
			break;
		}
		externalListener.OnStatusChanged(statusCode);
	}

	public void OpCleanRpcBuffer(PhotonView view)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions);
	}

	public void OpCleanRpcBuffer(int actorNumber)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		raiseEventOptions.TargetActors = new int[1] { actorNumber };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		OpRaiseEvent(200, null, sendReliable: true, raiseEventOptions2);
	}

	public bool OpCreateGame(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		bool onGameServer;
		if (!(onGameServer = server == ServerConnection.GameServer))
		{
			mRoomOptionsForCreate = roomOptions;
			mRoomToGetInto = new Room(roomName, roomOptions);
			mRoomToEnterLobby = typedLobby ?? ((!insideLobby) ? null : lobby);
		}
		mLastJoinType = JoinType.CreateGame;
		return base.OpCreateRoom(roomName, roomOptions, mRoomToEnterLobby, GetLocalActorProperties(), onGameServer);
	}

	public override bool OpFindFriends(string[] friendsToFind)
	{
		if (isFetchingFriends)
		{
			return false;
		}
		friendListRequested = friendsToFind;
		isFetchingFriends = true;
		return base.OpFindFriends(friendsToFind);
	}

	public override bool OpJoinRandomRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, ExitGames.Client.Photon.Hashtable playerProperties, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter)
	{
		mRoomToGetInto = new Room(null, null);
		mRoomToEnterLobby = null;
		mLastJoinType = JoinType.JoinRandomGame;
		return base.OpJoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, playerProperties, matchingType, typedLobby, sqlLobbyFilter);
	}

	public bool OpJoinRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, bool createIfNotExists)
	{
		bool onGameServer;
		if (!(onGameServer = server == ServerConnection.GameServer))
		{
			mRoomOptionsForCreate = roomOptions;
			mRoomToGetInto = new Room(roomName, roomOptions);
			mRoomToEnterLobby = null;
			if (createIfNotExists)
			{
				mRoomToEnterLobby = typedLobby ?? ((!insideLobby) ? null : lobby);
			}
		}
		mLastJoinType = ((!createIfNotExists) ? JoinType.JoinGame : JoinType.JoinOrCreateOnDemand);
		return base.OpJoinRoom(roomName, roomOptions, mRoomToEnterLobby, createIfNotExists, GetLocalActorProperties(), onGameServer);
	}

	public virtual bool OpLeave()
	{
		if (states != PeerStates.Joined)
		{
			return false;
		}
		return OpCustom(254, null, sendReliable: true, 0);
	}

	public override bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
	{
		if (PhotonNetwork.offlineMode)
		{
			return false;
		}
		return base.OpRaiseEvent(eventCode, customEventContent, sendReliable, raiseEventOptions);
	}

	public void OpRemoveCompleteCache()
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			Receivers = ReceiverGroup.MasterClient
		};
		OpRaiseEvent(0, null, sendReliable: true, raiseEventOptions);
	}

	public void OpRemoveCompleteCacheOfPlayer(int actorNumber)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		raiseEventOptions.TargetActors = new int[1] { actorNumber };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		OpRaiseEvent(0, null, sendReliable: true, raiseEventOptions2);
	}

	private void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		raiseEventOptions.TargetActors = new int[1] { actorNr };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		OpRaiseEvent(202, null, sendReliable: true, raiseEventOptions2);
	}

	private void ReadoutProperties(ExitGames.Client.Photon.Hashtable gameProperties, ExitGames.Client.Photon.Hashtable pActorProperties, int targetActorNr)
	{
		if (mCurrentGame != null && gameProperties != null)
		{
			mCurrentGame.CacheProperties(gameProperties);
			object[] parameters = new object[1] { gameProperties };
			if (gameProperties.ContainsKey((byte)246) && RCextensions.returnIntFromObject(gameProperties[(byte)246]) != 0)
			{
				FengGameManagerMKII.instance.isPlayerTTL = true;
			}
			SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, parameters);
			if (PhotonNetwork.automaticallySyncScene)
			{
				LoadLevelIfSynced();
			}
		}
		if (pActorProperties == null || pActorProperties.Count <= 0)
		{
			return;
		}
		if (targetActorNr > 0)
		{
			PhotonPlayer playerWithID = GetPlayerWithID(targetActorNr);
			if (playerWithID != null)
			{
				ExitGames.Client.Photon.Hashtable actorPropertiesForActorNr = GetActorPropertiesForActorNr(pActorProperties, targetActorNr);
				playerWithID.InternalCacheProperties(actorPropertiesForActorNr);
				object[] parameters2 = new object[2] { playerWithID, actorPropertiesForActorNr };
				SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, parameters2);
			}
			return;
		}
		foreach (object key in pActorProperties.Keys)
		{
			int num = (int)key;
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)pActorProperties[key];
			string name = (string)hashtable[byte.MaxValue];
			PhotonPlayer photonPlayer = GetPlayerWithID(num);
			if (photonPlayer == null)
			{
				photonPlayer = new PhotonPlayer(isLocal: false, num, name);
				AddNewPlayer(num, photonPlayer);
			}
			photonPlayer.InternalCacheProperties(hashtable);
			object[] parameters3 = new object[2] { photonPlayer, hashtable };
			SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, parameters3);
		}
	}

	private void RebuildPlayerListCopies()
	{
		mPlayerListCopy = new PhotonPlayer[mActors.Count];
		mActors.Values.CopyTo(mPlayerListCopy, 0);
		List<PhotonPlayer> list = new List<PhotonPlayer>();
		PhotonPlayer[] array = mPlayerListCopy;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (!photonPlayer.isLocal)
			{
				list.Add(photonPlayer);
			}
		}
		mOtherPlayerListCopy = list.ToArray();
	}

	public void RegisterPhotonView(PhotonView netView)
	{
		if (!Application.isPlaying)
		{
			photonViewList = new Dictionary<int, PhotonView>();
		}
		else
		{
			if (netView.subId == 0)
			{
				return;
			}
			int viewID = netView.viewID;
			if (photonViewList.ContainsKey(viewID))
			{
				if (netView != photonViewList[viewID])
				{
					RemoveInstantiatedGO(photonViewList[netView.viewID].gameObject, localOnly: true);
					photonViewList[viewID] = netView;
					return;
				}
			}
			else
			{
				photonViewList.Add(netView.viewID, netView);
			}
			_ = PhotonNetwork.logLevel;
			_ = 2;
		}
	}

	public void RemoveAllInstantiatedObjects()
	{
		GameObject[] array = new GameObject[instantiatedObjects.Count];
		instantiatedObjects.Values.CopyTo(array, 0);
		foreach (GameObject gameObject in array)
		{
			if (gameObject != null)
			{
				RemoveInstantiatedGO(gameObject, localOnly: false);
			}
		}
		_ = instantiatedObjects.Count;
		_ = 0;
		instantiatedObjects = new Dictionary<int, GameObject>();
	}

	private void RemoveCacheOfLeftPlayers()
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[244] = (byte)0;
		dictionary[247] = (byte)7;
		OpCustom(253, dictionary, sendReliable: true, 0);
	}

	protected internal void RemoveInstantiatedGO(GameObject go, bool localOnly, bool immediate = false, float delay = 0f)
	{
		if (go == null)
		{
			return;
		}
		PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int ownerId = photonView.ownerId;
		int instantiationId = photonView.instantiationId;
		if (!localOnly && ((!photonView.isMine && (!mLocalActor.isMasterClient || mActors.ContainsKey(ownerId))) || instantiationId < 1))
		{
			return;
		}
		if (!localOnly)
		{
			ServerCleanInstantiateAndDestroy(instantiationId, ownerId);
		}
		instantiatedObjects.Remove(instantiationId);
		for (int num = componentsInChildren.Length - 1; num >= 0; num--)
		{
			if ((photonView = componentsInChildren[num]) != null)
			{
				if (photonView.instantiationId > 0)
				{
					LocalCleanPhotonView(photonView);
				}
				if (!localOnly)
				{
					OpCleanRpcBuffer(photonView);
				}
			}
		}
		FengGameManagerMKII.instance.SendMessage(PhotonNetworkingMessage.OnPhotonDestroy.ToString(), photonView.GetHashCode(), SendMessageOptions.DontRequireReceiver);
		string name = go.name;
		if (immediate || (name != null && name.ToLower().StartsWith("fx")))
		{
			UnityEngine.Object.DestroyImmediate(go, allowDestroyingAssets: false);
		}
		else if (delay > 0f)
		{
			UnityEngine.Object.Destroy(go, delay);
		}
		else
		{
			UnityEngine.Object.Destroy(go);
		}
	}

	internal void DestroyLeftOver<T>(T mono, bool localOnly = false) where T : Photon.MonoBehaviour
	{
		if ((UnityEngine.Object)mono == (UnityEngine.Object)null)
		{
			return;
		}
		GameObject gameObject = mono.gameObject;
		PhotonView[] componentsInChildren = gameObject.GetComponentsInChildren<PhotonView>(includeInactive: true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int ownerActorNr = photonView.OwnerActorNr;
		int instantiationId = photonView.instantiationId;
		if (!localOnly)
		{
			OpRaiseEvent(202, new ExitGames.Client.Photon.Hashtable { { 7, instantiationId } }, sendReliable: true, new RaiseEventOptions
			{
				CachingOption = EventCaching.RemoveFromRoomCache,
				TargetActors = new int[1] { ownerActorNr }
			});
			OpRaiseEvent(204, new ExitGames.Client.Photon.Hashtable { { 0, instantiationId } }, sendReliable: true, null);
		}
		instantiatedObjects.Remove(instantiationId);
		for (int num = componentsInChildren.Length - 1; num >= 0; num--)
		{
			if ((photonView = componentsInChildren[num]) != null)
			{
				if (photonView.instantiationId >= 1)
				{
					photonView.destroyedByPhotonNetworkOrQuit = true;
					photonViewList.Remove(photonView.viewID);
				}
				if (!localOnly)
				{
					OpRaiseEvent(200, new ExitGames.Client.Photon.Hashtable { { 0, photonView.viewID } }, sendReliable: true, new RaiseEventOptions
					{
						CachingOption = EventCaching.RemoveFromRoomCache
					});
				}
			}
		}
		FengGameManagerMKII.instance.SendMessage(PhotonNetworkingMessage.OnPhotonDestroy.ToString(), photonView.GetHashCode(), SendMessageOptions.DontRequireReceiver);
		UnityEngine.Object.Destroy(gameObject);
	}

	public void RemoveInstantiatedGO(GameObject go, bool localOnly)
	{
		if (go == null)
		{
			return;
		}
		PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int ownerActorNr = photonView.OwnerActorNr;
		int instantiationId = photonView.instantiationId;
		if (!localOnly && ((!photonView.isMine && (!mLocalActor.isMasterClient || mActors.ContainsKey(ownerActorNr))) || instantiationId < 1))
		{
			return;
		}
		if (!localOnly)
		{
			ServerCleanInstantiateAndDestroy(instantiationId, ownerActorNr);
		}
		instantiatedObjects.Remove(instantiationId);
		for (int num = componentsInChildren.Length - 1; num >= 0; num--)
		{
			PhotonView photonView2 = componentsInChildren[num];
			if (photonView2 != null)
			{
				if (photonView2.instantiationId >= 1)
				{
					LocalCleanPhotonView(photonView2);
				}
				if (!localOnly)
				{
					OpCleanRpcBuffer(photonView2);
				}
			}
		}
		_ = PhotonNetwork.logLevel;
		_ = 2;
		UnityEngine.Object.Destroy(go);
	}

	public void RemoveInstantiationData(int instantiationId)
	{
		tempInstantiationData.Remove(instantiationId);
	}

	private void RemovePlayer(int ID, PhotonPlayer player)
	{
		mActors.Remove(ID);
		if (!player.isLocal)
		{
			RebuildPlayerListCopies();
		}
	}

	public void RemoveRPCsInGroup(int group)
	{
		foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
		{
			PhotonView value = photonView.Value;
			if (value.group == group)
			{
				CleanRpcBufferIfMine(value);
			}
		}
	}

	private void ResetPhotonViewsOnSerialize()
	{
		foreach (PhotonView value in photonViewList.Values)
		{
			value.lastOnSerializeDataSent = null;
		}
	}

	private static int ReturnLowestPlayerId(PhotonPlayer[] players, int playerIdToIgnore)
	{
		if (players == null || players.Length == 0)
		{
			return -1;
		}
		int num = int.MaxValue;
		foreach (PhotonPlayer photonPlayer in players)
		{
			if (photonPlayer.ID != playerIdToIgnore && photonPlayer.ID < num)
			{
				num = photonPlayer.ID;
			}
		}
		return num;
	}

	internal void RPC(PhotonView view, string methodName, PhotonPlayer player, params object[] parameters)
	{
		if (!blockSendingGroups.Contains(view.group))
		{
			_ = view.viewID;
			_ = 1;
			_ = PhotonNetwork.logLevel;
			_ = 2;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = view.viewID;
			if (view.prefix > 0)
			{
				hashtable[(byte)1] = (short)view.prefix;
			}
			hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
			int value = 0;
			if (rpcShortcuts.TryGetValue(methodName, out value))
			{
				hashtable[(byte)5] = (byte)value;
			}
			else
			{
				hashtable[(byte)3] = methodName;
			}
			if (parameters != null && parameters.Length != 0)
			{
				hashtable[(byte)4] = parameters;
			}
			if (mLocalActor == player)
			{
				ExecuteRPC(hashtable, player);
				return;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.TargetActors = new int[1] { player.ID };
			RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions2);
		}
	}

	internal void RPC(PhotonView view, string methodName, PhotonTargets target, params object[] parameters)
	{
		if (blockSendingGroups.Contains(view.group))
		{
			return;
		}
		_ = view.viewID;
		_ = 1;
		_ = PhotonNetwork.logLevel;
		_ = 2;
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		if (view.prefix > 0)
		{
			hashtable[(byte)1] = (short)view.prefix;
		}
		hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
		int value = 0;
		if (rpcShortcuts.TryGetValue(methodName, out value))
		{
			hashtable[(byte)5] = (byte)value;
		}
		else
		{
			hashtable[(byte)3] = methodName;
		}
		if (parameters != null && parameters.Length != 0)
		{
			hashtable[(byte)4] = parameters;
		}
		switch (target)
		{
		case PhotonTargets.All:
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				InterestGroup = (byte)view.group
			};
			RaiseEventOptions raiseEventOptions3 = raiseEventOptions;
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions3);
			ExecuteRPC(hashtable, mLocalActor);
			break;
		}
		case PhotonTargets.Others:
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				InterestGroup = (byte)view.group
			};
			RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions2);
			break;
		}
		case PhotonTargets.AllBuffered:
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache
			};
			RaiseEventOptions raiseEventOptions5 = raiseEventOptions;
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions5);
			ExecuteRPC(hashtable, mLocalActor);
			break;
		}
		case PhotonTargets.OthersBuffered:
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache
			};
			RaiseEventOptions raiseEventOptions4 = raiseEventOptions;
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions4);
			break;
		}
		case PhotonTargets.MasterClient:
		{
			if (mMasterClient == mLocalActor)
			{
				ExecuteRPC(hashtable, mLocalActor);
				break;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.MasterClient
			};
			RaiseEventOptions raiseEventOptions8 = raiseEventOptions;
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions8);
			break;
		}
		case PhotonTargets.AllViaServer:
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				InterestGroup = (byte)view.group,
				Receivers = ReceiverGroup.All
			};
			RaiseEventOptions raiseEventOptions7 = raiseEventOptions;
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions7);
			break;
		}
		case PhotonTargets.AllBufferedViaServer:
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				InterestGroup = (byte)view.group,
				Receivers = ReceiverGroup.All,
				CachingOption = EventCaching.AddToRoomCache
			};
			RaiseEventOptions raiseEventOptions6 = raiseEventOptions;
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions6);
			break;
		}
		case PhotonTargets.TLWUsers:
		{
			int[] array2 = PhotonPlayer.TLWUsersIDs();
			if (array2.Length == 0)
			{
				ExecuteRPC(hashtable, mLocalActor);
				break;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				TargetActors = array2
			};
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions);
			ExecuteRPC(hashtable, mLocalActor);
			break;
		}
		case PhotonTargets.TLWandRRC:
		{
			int[] array = PhotonPlayer.TLWandRRCIDs();
			if (array.Length == 0)
			{
				ExecuteRPC(hashtable, mLocalActor);
				break;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				TargetActors = array
			};
			OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions);
			ExecuteRPC(hashtable, mLocalActor);
			break;
		}
		case PhotonTargets.Local:
			ExecuteRPC(hashtable, mLocalActor);
			break;
		}
	}

	public void RunViewUpdate()
	{
		if (!PhotonNetwork.connected || PhotonNetwork.offlineMode || mActors == null || mActors.Count <= 1)
		{
			return;
		}
		Dictionary<int, ExitGames.Client.Photon.Hashtable> dictionary = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();
		Dictionary<int, ExitGames.Client.Photon.Hashtable> dictionary2 = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();
		foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
		{
			PhotonView value = photonView.Value;
			if (!(value.observed != null) || value.synchronization == ViewSynchronization.Off || (value.ownerId != mLocalActor.ID && (!value.isSceneView || mMasterClient != mLocalActor)) || !value.gameObject.activeInHierarchy || blockSendingGroups.Contains(value.group))
			{
				continue;
			}
			ExitGames.Client.Photon.Hashtable hashtable = OnSerializeWrite(value);
			if (hashtable == null)
			{
				continue;
			}
			if (value.synchronization != ViewSynchronization.ReliableDeltaCompressed && !value.mixedModeIsReliable)
			{
				if (!dictionary2.ContainsKey(value.group))
				{
					dictionary2[value.group] = new ExitGames.Client.Photon.Hashtable();
					dictionary2[value.group][(byte)0] = base.ServerTimeInMilliSeconds;
					if (currentLevelPrefix >= 0)
					{
						dictionary2[value.group][(byte)1] = currentLevelPrefix;
					}
				}
				ExitGames.Client.Photon.Hashtable hashtable2 = dictionary2[value.group];
				hashtable2.Add((short)hashtable2.Count, hashtable);
			}
			else
			{
				if (!hashtable.ContainsKey((byte)1) && !hashtable.ContainsKey((byte)2))
				{
					continue;
				}
				if (!dictionary.ContainsKey(value.group))
				{
					dictionary[value.group] = new ExitGames.Client.Photon.Hashtable();
					dictionary[value.group][(byte)0] = base.ServerTimeInMilliSeconds;
					if (currentLevelPrefix >= 0)
					{
						dictionary[value.group][(byte)1] = currentLevelPrefix;
					}
				}
				ExitGames.Client.Photon.Hashtable hashtable3 = dictionary[value.group];
				hashtable3.Add((short)hashtable3.Count, hashtable);
			}
		}
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
		foreach (KeyValuePair<int, ExitGames.Client.Photon.Hashtable> item in dictionary)
		{
			raiseEventOptions.InterestGroup = (byte)item.Key;
			OpRaiseEvent(206, item.Value, sendReliable: true, raiseEventOptions);
		}
		foreach (KeyValuePair<int, ExitGames.Client.Photon.Hashtable> item2 in dictionary2)
		{
			raiseEventOptions.InterestGroup = (byte)item2.Key;
			OpRaiseEvent(201, item2.Value, sendReliable: false, raiseEventOptions);
		}
	}

	private void SendDestroyOfAll()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = -1;
		OpRaiseEvent(207, hashtable, sendReliable: true, null);
	}

	private void SendDestroyOfPlayer(int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = actorNr;
		OpRaiseEvent(207, hashtable, sendReliable: true, null);
	}

	internal ExitGames.Client.Photon.Hashtable SendInstantiate(string prefabName, Vector3 position, Quaternion rotation, int group, int[] viewIDs, object[] data, bool isGlobalObject)
	{
		int num = viewIDs[0];
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = prefabName;
		if (position != Vector3.zero)
		{
			hashtable[(byte)1] = position;
		}
		if (rotation != Quaternion.identity)
		{
			hashtable[(byte)2] = rotation;
		}
		if (group != 0)
		{
			hashtable[(byte)3] = group;
		}
		if (viewIDs.Length > 1)
		{
			hashtable[(byte)4] = viewIDs;
		}
		if (data != null)
		{
			hashtable[(byte)5] = data;
		}
		if (currentLevelPrefix > 0)
		{
			hashtable[(byte)8] = currentLevelPrefix;
		}
		hashtable[(byte)6] = base.ServerTimeInMilliSeconds;
		hashtable[(byte)7] = num;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = ((!isGlobalObject) ? EventCaching.AddToRoomCache : EventCaching.AddToRoomCacheGlobal)
		};
		OpRaiseEvent(202, hashtable, sendReliable: true, raiseEventOptions);
		return hashtable;
	}

	public static void SendMonoMessage(PhotonNetworkingMessage methodString, params object[] parameters)
	{
		HashSet<GameObject> hashSet;
		if (PhotonNetwork.SendMonoMessageTargets != null)
		{
			hashSet = PhotonNetwork.SendMonoMessageTargets;
		}
		else
		{
			hashSet = new HashSet<GameObject>();
			Component[] array = (Component[])UnityEngine.Object.FindObjectsOfType(typeof(UnityEngine.MonoBehaviour));
			for (int i = 0; i < array.Length; i++)
			{
				hashSet.Add(array[i].gameObject);
			}
		}
		string methodName = methodString.ToString();
		foreach (GameObject item in hashSet)
		{
			if (parameters != null && parameters.Length == 1)
			{
				item.SendMessage(methodName, parameters[0], SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				item.SendMessage(methodName, parameters, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void SendPlayerName()
	{
		if (states == PeerStates.Joining)
		{
			mPlayernameHasToBeUpdated = true;
		}
		else if (mLocalActor != null)
		{
			mLocalActor.name = PlayerName;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[byte.MaxValue] = PlayerName;
			if (mLocalActor.ID > 0)
			{
				OpSetPropertiesOfActor(mLocalActor.ID, hashtable, broadcast: true, 0);
				mPlayernameHasToBeUpdated = false;
			}
		}
	}

	private void ServerCleanInstantiateAndDestroy(int instantiateId, int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)7] = instantiateId;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		raiseEventOptions.TargetActors = new int[1] { actorNr };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		OpRaiseEvent(202, hashtable, sendReliable: true, raiseEventOptions2);
		ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
		hashtable2[(byte)0] = instantiateId;
		OpRaiseEvent(204, hashtable2, sendReliable: true, null);
	}

	public void SetApp(string appId, string gameVersion)
	{
		mAppId = appId.Trim();
		if (!string.IsNullOrEmpty(gameVersion))
		{
			mAppVersion = gameVersion.Trim();
		}
	}

	protected internal void SetLevelInPropsIfSynced(object levelId)
	{
		if (!PhotonNetwork.automaticallySyncScene || !PhotonNetwork.isMasterClient || PhotonNetwork.room == null || levelId == null)
		{
			return;
		}
		if (PhotonNetwork.room.customProperties.ContainsKey("curScn"))
		{
			object obj = PhotonNetwork.room.customProperties["curScn"];
			if ((obj is int && Application.loadedLevel == (int)obj) || (obj is string && Application.loadedLevelName.Equals((string)obj)))
			{
				return;
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if (levelId is int)
		{
			hashtable["curScn"] = (int)levelId;
		}
		else if (levelId is string)
		{
			hashtable["curScn"] = (string)levelId;
		}
		PhotonNetwork.room.SetCustomProperties(hashtable);
		SendOutgoingCommands();
	}

	public void SetLevelPrefix(short prefix)
	{
		currentLevelPrefix = prefix;
	}

	protected internal bool SetMasterClient(int playerId, bool sync)
	{
		if (mMasterClient == null || mMasterClient.ID == -1 || !mActors.ContainsKey(playerId))
		{
			return false;
		}
		if (sync)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add((byte)1, playerId);
			if (!OpRaiseEvent(208, hashtable, sendReliable: true, null))
			{
				return false;
			}
		}
		hasSwitchedMC = true;
		mMasterClient = mActors[playerId];
		object[] parameters = new object[1] { mMasterClient };
		SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, parameters);
		return true;
	}

	public void SetReceivingEnabled(int group, bool enabled)
	{
		if (group <= 0)
		{
			return;
		}
		if (enabled)
		{
			if (!allowedReceivingGroups.Contains(group))
			{
				allowedReceivingGroups.Add(group);
				byte[] groupsToAdd = new byte[1] { (byte)group };
				OpChangeGroups(null, groupsToAdd);
			}
		}
		else if (allowedReceivingGroups.Contains(group))
		{
			allowedReceivingGroups.Remove(group);
			byte[] groupsToRemove = new byte[1] { (byte)group };
			OpChangeGroups(groupsToRemove, null);
		}
	}

	public void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
	{
		List<byte> list = new List<byte>();
		List<byte> list2 = new List<byte>();
		if (enableGroups != null)
		{
			foreach (int num in enableGroups)
			{
				if (num > 0 && !allowedReceivingGroups.Contains(num))
				{
					allowedReceivingGroups.Add(num);
					list.Add((byte)num);
				}
			}
		}
		if (disableGroups != null)
		{
			foreach (int num2 in disableGroups)
			{
				if (num2 > 0 && !list.Contains((byte)num2) && allowedReceivingGroups.Contains(num2))
				{
					allowedReceivingGroups.Remove(num2);
					list2.Add((byte)num2);
				}
			}
		}
		OpChangeGroups((list2.Count <= 0) ? null : list2.ToArray(), (list.Count <= 0) ? null : list.ToArray());
	}

	public void SetSendingEnabled(int group, bool enabled)
	{
		if (!enabled)
		{
			blockSendingGroups.Add(group);
		}
		else
		{
			blockSendingGroups.Remove(group);
		}
	}

	public void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
	{
		int[] array;
		if (enableGroups != null)
		{
			array = enableGroups;
			foreach (int item in array)
			{
				if (blockSendingGroups.Contains(item))
				{
					blockSendingGroups.Remove(item);
				}
			}
		}
		if (disableGroups == null)
		{
			return;
		}
		array = disableGroups;
		foreach (int item2 in array)
		{
			if (!blockSendingGroups.Contains(item2))
			{
				blockSendingGroups.Add(item2);
			}
		}
	}

	public void StoreInstantiationData(int instantiationId, object[] instantiationData)
	{
		tempInstantiationData[instantiationId] = instantiationData;
	}

	public bool WebRpc(string uriPath, object parameters)
	{
		Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
		{
			{ 209, uriPath },
			{ 208, parameters }
		};
		return OpCustom(219, customOpParameters, sendReliable: true, 0);
	}
}
