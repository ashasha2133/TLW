using System.Collections.Generic;
using Photon;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItemSyncer : Photon.MonoBehaviour
{
	public bool IsWaitingForPickupInit;

	private const float TimeDeltaToIgnore = 0.2f;

	public void AskForPickupItemSpawnTimes()
	{
		if (!IsWaitingForPickupInit)
		{
			return;
		}
		if (PhotonNetwork.playerList.Length < 2)
		{
			IsWaitingForPickupInit = false;
			return;
		}
		PhotonPlayer next = PhotonNetwork.masterClient.GetNext();
		if (next == null || next.Equals(PhotonNetwork.player))
		{
			next = PhotonNetwork.player.GetNext();
		}
		if (next != null && !next.Equals(PhotonNetwork.player))
		{
			base.photonView.RPC("RequestForPickupTimes", next);
		}
		else
		{
			IsWaitingForPickupInit = false;
		}
	}

	public void OnJoinedRoom()
	{
		IsWaitingForPickupInit = !PhotonNetwork.isMasterClient;
		if (PhotonNetwork.playerList.Length >= 2)
		{
			Invoke("AskForPickupItemSpawnTimes", 2f);
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		if (PhotonNetwork.isMasterClient)
		{
			SendPickedUpItems(newPlayer);
		}
	}

	[RPC]
	public void PickupItemInit(double timeBase, float[] inactivePickupsAndTimes)
	{
		IsWaitingForPickupInit = false;
		for (int i = 0; i < inactivePickupsAndTimes.Length / 2; i++)
		{
			int num = i * 2;
			int viewID = (int)inactivePickupsAndTimes[num];
			float num2 = inactivePickupsAndTimes[num + 1];
			PickupItem component = PhotonView.Find(viewID).GetComponent<PickupItem>();
			if (num2 <= 0f)
			{
				component.PickedUp(0f);
				continue;
			}
			double num3 = (double)num2 + timeBase - PhotonNetwork.time;
			if (num2 <= 0f)
			{
				num3 = 0.0;
			}
			component.PickedUp((float)num3);
		}
	}

	[RPC]
	public void RequestForPickupTimes(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender != null)
		{
			SendPickedUpItems(msgInfo.sender);
		}
	}

	private void SendPickedUpItems(PhotonPlayer targtePlayer)
	{
		if (targtePlayer == null)
		{
			return;
		}
		double num = PhotonNetwork.time + 0.20000000298023224;
		PickupItem[] array = new PickupItem[PickupItem.DisabledPickupItems.Count];
		PickupItem.DisabledPickupItems.CopyTo(array);
		List<float> list = new List<float>(array.Length * 2);
		foreach (PickupItem pickupItem in array)
		{
			if (pickupItem.SecondsBeforeRespawn <= 0f)
			{
				list.Add(pickupItem.ViewID);
				list.Add(0f);
				continue;
			}
			double num2 = pickupItem.TimeOfRespawn - PhotonNetwork.time;
			if (pickupItem.TimeOfRespawn > num)
			{
				list.Add(pickupItem.ViewID);
				list.Add((float)num2);
			}
		}
		object[] parameters = new object[2]
		{
			PhotonNetwork.time,
			list.ToArray()
		};
		base.photonView.RPC("PickupItemInit", targtePlayer, parameters);
	}
}
