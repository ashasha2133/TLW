using Photon;
using UnityEngine;

public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
	public bool AutoConnect = true;

	private bool ConnectInUpdate = true;

	public virtual void OnConnectedToMaster()
	{
		_ = PhotonNetwork.networkingPeer.AvailableRegions;
		PhotonNetwork.JoinRandomRoom();
	}

	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
	}

	public virtual void OnJoinedLobby()
	{
	}

	public void OnJoinedRoom()
	{
	}

	public virtual void OnPhotonRandomJoinFailed()
	{
		RoomOptions roomOptions = new RoomOptions
		{
			maxPlayers = 4
		};
		PhotonNetwork.CreateRoom(null, roomOptions, null);
	}

	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;
	}

	public virtual void Update()
	{
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings("2." + Application.loadedLevel);
		}
	}
}
