using System.Text;
using UnityEngine;

public class SupportLogging : MonoBehaviour
{
	public bool LogTrafficStats;

	private void LogBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", "1.28");
		stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} ", PhotonNetwork.networkingPeer.mAppId.Substring(0, 8), PhotonNetwork.networkingPeer.mAppVersionPun);
		stringBuilder.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.networkingPeer.CloudRegion);
		stringBuilder.AppendFormat("HostType: {0} ", PhotonNetwork.PhotonServerSettings.HostType);
	}

	public void LogStats()
	{
		InRoomChat.Write("LogStats: " + Random.Range(1, 10));
		_ = LogTrafficStats;
	}

	public void OnApplicationQuit()
	{
		CancelInvoke();
	}

	public void OnConnectedToPhoton()
	{
		LogBasics();
		if (LogTrafficStats)
		{
			PhotonNetwork.NetworkStatisticsEnabled = true;
		}
	}

	public void OnCreatedRoom()
	{
	}

	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		LogBasics();
	}

	public void OnJoinedLobby()
	{
	}

	public void OnJoinedRoom()
	{
	}

	public void OnLeftRoom()
	{
	}

	public void Start()
	{
		if (LogTrafficStats && !IsInvoking("LogStats"))
		{
			InvokeRepeating("LogStats", 10f, 10f);
		}
	}
}
