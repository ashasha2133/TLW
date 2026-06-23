using System;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

internal class PhotonHandler : Photon.MonoBehaviour, IPhotonPeerListener
{
	private const string PlayerPrefsKey = "PUNCloudBestRegion";

	private static bool sendThreadShouldRun;

	private int nextSendTickCount;

	private int nextSendTickCountOnSerialize;

	internal static CloudRegionCode BestRegionCodeCurrently = CloudRegionCode.none;

	public static bool AppQuits;

	public static Type PingImplementation = null;

	public static PhotonHandler SP;

	public int updateInterval;

	public int updateIntervalOnSerialize;

	internal static CloudRegionCode BestRegionCodeInPreferences
	{
		get
		{
			string text = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
			if (!string.IsNullOrEmpty(text))
			{
				return Region.Parse(text);
			}
			return CloudRegionCode.none;
		}
		set
		{
			if (value == CloudRegionCode.none)
			{
				PlayerPrefs.DeleteKey("PUNCloudBestRegion");
			}
			else
			{
				PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
			}
		}
	}

	protected void Awake()
	{
		if (SP != null && SP != this && SP.gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(SP.gameObject);
		}
		SP = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		updateInterval = 1000 / PhotonNetwork.sendRate;
		updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;
		StartFallbackSendAckThread();
	}

	protected void OnApplicationQuit()
	{
		AppQuits = true;
		StopFallbackSendAckThread();
		PhotonNetwork.Disconnect();
	}

	protected void OnCreatedRoom()
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(Application.loadedLevelName);
	}

	protected void OnJoinedRoom()
	{
		PhotonNetwork.networkingPeer.LoadLevelIfSynced();
	}

	protected void OnLevelWasLoaded(int level)
	{
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(Application.loadedLevelName);
	}

	protected void Update()
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.connectionStateDetailed == PeerStates.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerStates.Disconnected || PhotonNetwork.offlineMode || !PhotonNetwork.isMessageQueueRunning)
		{
			return;
		}
		bool flag = true;
		while (PhotonNetwork.isMessageQueueRunning && flag)
		{
			flag = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
		}
		int num = (int)(Time.realtimeSinceStartup * 1000f);
		if (PhotonNetwork.isMessageQueueRunning && num > nextSendTickCountOnSerialize)
		{
			PhotonNetwork.networkingPeer.RunViewUpdate();
			nextSendTickCountOnSerialize = num + updateIntervalOnSerialize;
			nextSendTickCount = 0;
		}
		num = (int)(Time.realtimeSinceStartup * 1000f);
		if (num > nextSendTickCount)
		{
			bool flag2 = true;
			while (PhotonNetwork.isMessageQueueRunning && flag2)
			{
				flag2 = PhotonNetwork.networkingPeer.SendOutgoingCommands();
			}
			nextSendTickCount = num + updateInterval;
		}
	}

	public static bool FallbackSendAckThread()
	{
		if (sendThreadShouldRun && PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.SendAcksOnly();
		}
		return sendThreadShouldRun;
	}

	public static void StartFallbackSendAckThread()
	{
		if (!sendThreadShouldRun)
		{
			sendThreadShouldRun = true;
			SupportClass.StartBackgroundCalls(FallbackSendAckThread);
		}
	}

	public static void StopFallbackSendAckThread()
	{
		sendThreadShouldRun = false;
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		switch (level)
		{
		case DebugLevel.INFO:
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				break;
			}
			goto default;
		default:
			if (level == DebugLevel.ALL)
			{
				_ = PhotonNetwork.logLevel;
				_ = 2;
			}
			break;
		case DebugLevel.ERROR:
		case DebugLevel.WARNING:
			break;
		}
	}

	public void OnEvent(EventData photonEvent)
	{
	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{
	}

	public void OnStatusChanged(StatusCode statusCode)
	{
	}
}
