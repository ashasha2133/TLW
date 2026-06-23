using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class PhotonPlayer
{
	private int actorID;

	public readonly bool isLocal;

	private string nameField;

	public object TagObject;

	public Dictionary<string, long> RPCList = new Dictionary<string, long>();

	public Dictionary<byte, long> EventList = new Dictionary<byte, long>();

	public long BytesReceived;

	public bool Tlw { get; set; }

	public bool OldTlw { get; set; }

	public bool NewTlw { get; set; }

	public float BombSpeed { get; set; }

	public string FriendName
	{
		get
		{
			if (customProperties[byte.MaxValue] == null)
			{
				customProperties[byte.MaxValue] = nameField;
			}
			if (!(customProperties[byte.MaxValue] is string))
			{
				customProperties[byte.MaxValue] = "null";
			}
			return customProperties[byte.MaxValue] as string;
		}
	}

	public bool RRC { get; set; }

	public bool BombHasExploded { get; set; }

	protected internal string chatname { get; set; }

	public string allProps
	{
		get
		{
			string text = "";
			foreach (object key in allProperties.Keys)
			{
				text = string.Concat(text, key, ": ", allProperties[key], "\n");
			}
			return text;
		}
	}

	public string RPCs
	{
		get
		{
			string text = string.Empty;
			foreach (string key in RPCList.Keys)
			{
				text = text + key + ": " + RPCList[key] + "\n";
			}
			return text;
		}
	}

	public string Events
	{
		get
		{
			string text = string.Empty;
			foreach (byte key in EventList.Keys)
			{
				text = text + key + ": " + EventList[key] + "\n";
			}
			return text;
		}
	}

	protected internal string guildname
	{
		get
		{
			object obj = customProperties["guildName"];
			if (obj != null && obj is string)
			{
				return (string)obj;
			}
			return string.Empty;
		}
		private set
		{
			SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "guildName", value } });
		}
	}

	protected internal bool isTitan
	{
		get
		{
			object obj = customProperties["isTitan"];
			if (obj != null && obj is int)
			{
				return (int)obj == 2;
			}
			return false;
		}
		private set
		{
			SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "isTitan", value } });
		}
	}

	protected internal string Chatname
	{
		get
		{
			if (!chatname.IsNullOrEmpty())
			{
				return chatname;
			}
			if (customProperties.ContainsKey("chatname") && customProperties["chatname"] != null)
			{
				return (string)customProperties["chatname"];
			}
			return string.Empty;
		}
		private set
		{
			SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "chatname", value } });
		}
	}

	public ExitGames.Client.Photon.Hashtable allProperties
	{
		get
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Merge(customProperties);
			hashtable[byte.MaxValue] = name;
			return hashtable;
		}
	}

	public ExitGames.Client.Photon.Hashtable customProperties { get; private set; }

	public string uiname
	{
		get
		{
			object obj = customProperties["name"];
			if (obj != null && obj is string)
			{
				return (string)obj;
			}
			return string.Empty;
		}
		set
		{
			SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "name", value } });
		}
	}

	public short ActorID => (short)actorID;

	public int ID => actorID;

	public bool isMasterClient => PhotonNetwork.networkingPeer.mMasterClient == this;

	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			if (isLocal)
			{
				nameField = value;
			}
		}
	}

	public static void CleanProperties()
	{
		if (PhotonNetwork.player != null)
		{
			PhotonNetwork.player.customProperties.Clear();
			string value = FengGameManagerMKII.RandomNameGenerator(5, 8);
			PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "name", value } });
		}
	}

	public static int[] TLWUsersIDs()
	{
		List<int> list = new List<int>();
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (photonPlayer.Tlw || photonPlayer.NewTlw)
			{
				list.Add(photonPlayer.ID);
			}
		}
		return list.ToArray();
	}

	public static int[] TLWandRRCIDs()
	{
		List<int> list = new List<int>();
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (photonPlayer.Tlw || photonPlayer.RRC)
			{
				list.Add(photonPlayer.ID);
			}
		}
		return list.ToArray();
	}

	protected internal PhotonPlayer(bool isLocal, int actorID, ExitGames.Client.Photon.Hashtable properties)
	{
		this.actorID = -1;
		nameField = string.Empty;
		customProperties = new ExitGames.Client.Photon.Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		InternalCacheProperties(properties);
	}

	public PhotonPlayer(bool isLocal, int actorID, string name)
	{
		this.actorID = -1;
		nameField = string.Empty;
		customProperties = new ExitGames.Client.Photon.Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		nameField = name;
	}

	public PhotonPlayer()
	{
	}

	public void AddToRPC(string rpc)
	{
		if (!RPCList.ContainsKey(rpc))
		{
			RPCList.Add(rpc, 1L);
		}
		else
		{
			RPCList[rpc]++;
		}
	}

	public void AddToEvent(byte code)
	{
		if (!EventList.ContainsKey(code))
		{
			EventList.Add(code, 1L);
		}
		else
		{
			EventList[code]++;
		}
	}

	public static bool Find(int ID, out PhotonPlayer player)
	{
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (photonPlayer != null && photonPlayer.ID == ID)
			{
				player = photonPlayer;
				return true;
			}
		}
		player = null;
		return false;
	}

	public static bool IsInPlayerList(int ID)
	{
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (photonPlayer != null && photonPlayer.ID == ID)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsInPlayerList()
	{
		if (this == null)
		{
			return false;
		}
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (photonPlayer != null && this == photonPlayer)
			{
				return true;
			}
		}
		return false;
	}

	public override bool Equals(object p)
	{
		if (p is PhotonPlayer photonPlayer)
		{
			return GetHashCode() == photonPlayer.GetHashCode();
		}
		return false;
	}

	public static PhotonPlayer Find(int ID)
	{
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			if (photonPlayer.ID == ID)
			{
				return photonPlayer;
			}
		}
		return null;
	}

	public PhotonPlayer Get(int id)
	{
		return Find(id);
	}

	public override int GetHashCode()
	{
		return ID;
	}

	public PhotonPlayer GetNext()
	{
		return GetNextFor(ID);
	}

	public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
	{
		if (currentPlayer == null)
		{
			return null;
		}
		return GetNextFor(currentPlayer.ID);
	}

	public PhotonPlayer GetNextFor(int currentPlayerId)
	{
		if (PhotonNetwork.networkingPeer == null || NetworkingPeer.mActors == null || NetworkingPeer.mActors.Count < 2)
		{
			return null;
		}
		Dictionary<int, PhotonPlayer> mActors = NetworkingPeer.mActors;
		int num = int.MaxValue;
		int num2 = currentPlayerId;
		foreach (int key in mActors.Keys)
		{
			if (key < num2)
			{
				num2 = key;
			}
			else if (key > currentPlayerId && key < num)
			{
				num = key;
			}
		}
		if (num != int.MaxValue)
		{
			return mActors[num];
		}
		return mActors[num2];
	}

	internal void InternalCacheProperties(ExitGames.Client.Photon.Hashtable properties)
	{
		if (properties != null && properties.Count != 0 && !customProperties.Equals(properties))
		{
			if (properties.ContainsKey(byte.MaxValue))
			{
				nameField = (string)properties[byte.MaxValue];
			}
			customProperties.MergeStringKeys(properties);
			customProperties.StripKeysWithNullValues();
		}
	}

	internal void InternalChangeLocalID(int newID)
	{
		if (isLocal)
		{
			actorID = newID;
		}
	}

	public void SetCustomProperties(ExitGames.Client.Photon.Hashtable propertiesToSet)
	{
		if (propertiesToSet != null)
		{
			customProperties.MergeStringKeys(propertiesToSet);
			customProperties.StripKeysWithNullValues();
			ExitGames.Client.Photon.Hashtable actorProperties = propertiesToSet.StripToStringKeys();
			if (actorID > 0 && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(actorID, actorProperties, broadcast: true, 0);
			}
			object[] parameters = new object[2] { this, propertiesToSet };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, parameters);
		}
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Format("#{0:00}{1}", ID, (!isMasterClient) ? string.Empty : "(master)");
		}
		return string.Format("'{0}'{1}", name, (!isMasterClient) ? string.Empty : "(master)");
	}

	public string ToStringFull()
	{
		return $"#{ID:00} '{name}' {customProperties.ToStringFull()}";
	}

	public void RemoveFromCustomProperties(ExitGames.Client.Photon.Hashtable removeProps)
	{
		ExitGames.Client.Photon.Hashtable hashtable = customProperties;
		foreach (DictionaryEntry removeProp in removeProps)
		{
			hashtable.Remove(removeProp.Key);
		}
		SetCustomProperties(hashtable);
	}
}
