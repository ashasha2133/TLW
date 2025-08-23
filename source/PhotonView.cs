using System;
using System.Reflection;
using BRM;
using Photon;
using UnityEngine;

[AddComponentMenu("Miscellaneous/Photon View &v")]
public class PhotonView : Photon.MonoBehaviour
{
	protected internal bool destroyedByPhotonNetworkOrQuit;

	protected internal bool didAwake;

	private bool failedToFindOnSerialize;

	public int group;

	private object[] instantiationDataField;

	public int instantiationId;

	protected internal object[] lastOnSerializeDataReceived;

	protected internal object[] lastOnSerializeDataSent;

	protected internal bool mixedModeIsReliable;

	public Component observed;

	private MethodInfo OnSerializeMethodInfo;

	public OnSerializeRigidBody onSerializeRigidBodyOption = OnSerializeRigidBody.All;

	public OnSerializeTransform onSerializeTransformOption = OnSerializeTransform.PositionAndRotation;

	public int ownerId;

	public int prefixBackup = -1;

	public int subId;

	public ViewSynchronization synchronization;

	public object[] instantiationData
	{
		get
		{
			if (!didAwake)
			{
				instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(instantiationId);
			}
			return instantiationDataField;
		}
		set
		{
			instantiationDataField = value;
		}
	}

	public bool isMine
	{
		get
		{
			if (ownerId != PhotonNetwork.player.ID)
			{
				if (isSceneView)
				{
					return PhotonNetwork.isMasterClient;
				}
				return false;
			}
			return true;
		}
	}

	public bool isSceneView => ownerId == 0;

	public PhotonPlayer owner => PhotonPlayer.Find(ownerId);

	public int OwnerActorNr => ownerId;

	public int prefix
	{
		get
		{
			if (prefixBackup == -1 && PhotonNetwork.networkingPeer != null)
			{
				prefixBackup = PhotonNetwork.networkingPeer.currentLevelPrefix;
			}
			return prefixBackup;
		}
		set
		{
			prefixBackup = value;
		}
	}

	public int viewID
	{
		get
		{
			return ownerId * PhotonNetwork.MAX_VIEW_IDS + subId;
		}
		set
		{
			bool num = didAwake && subId == 0;
			ownerId = value / PhotonNetwork.MAX_VIEW_IDS;
			subId = value % PhotonNetwork.MAX_VIEW_IDS;
			if (num)
			{
				PhotonNetwork.networkingPeer.RegisterPhotonView(this);
			}
		}
	}

	protected internal void Awake()
	{
		PhotonNetwork.networkingPeer.RegisterPhotonView(this);
		instantiationDataField = PhotonNetwork.networkingPeer.FetchInstantiationData(instantiationId);
		didAwake = true;
	}

	protected internal void ExecuteOnSerialize(PhotonStream pStream, PhotonMessageInfo info)
	{
		if (!failedToFindOnSerialize)
		{
			if (OnSerializeMethodInfo == null && !NetworkingPeer.GetMethod(observed as UnityEngine.MonoBehaviour, PhotonNetworkingMessage.OnPhotonSerializeView.ToString(), out OnSerializeMethodInfo))
			{
				failedToFindOnSerialize = true;
				return;
			}
			object[] parameters = new object[2] { pStream, info };
			OnSerializeMethodInfo.Invoke(observed, parameters);
		}
	}

	public static PhotonView Find(int viewID)
	{
		return PhotonNetwork.networkingPeer.GetPhotonView(viewID);
	}

	public static PhotonView Get(Component component)
	{
		return component.GetComponent<PhotonView>();
	}

	public static PhotonView Get(GameObject gameObj)
	{
		return gameObj.GetComponent<PhotonView>();
	}

	protected internal void OnApplicationQuit()
	{
		destroyedByPhotonNetworkOrQuit = true;
	}

	protected internal void OnDestroy()
	{
		if (!destroyedByPhotonNetworkOrQuit)
		{
			PhotonNetwork.networkingPeer.LocalCleanPhotonView(this);
		}
		if (!destroyedByPhotonNetworkOrQuit && !Application.isLoadingLevel && instantiationId <= 0 && viewID > 0 && isMine)
		{
			PhotonNetwork.manuallyAllocatedViewIds.Contains(viewID);
		}
		if (PhotonNetwork.networkingPeer.instantiatedObjects.ContainsKey(instantiationId))
		{
			bool flag = PhotonNetwork.networkingPeer.instantiatedObjects[instantiationId] == base.gameObject;
			if (flag)
			{
				_ = new object[5]
				{
					this,
					instantiationId,
					(!Application.isLoadingLevel) ? string.Empty : "Loading new scene caused this.",
					flag,
					destroyedByPhotonNetworkOrQuit
				};
			}
		}
	}

	public void RPC(string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		PhotonNetwork.RPC(this, methodName, targetPlayer, parameters);
	}

	public void RPC(string methodName, PhotonTargets target, params object[] parameters)
	{
		if (PhotonNetwork.networkingPeer.hasSwitchedMC && target == PhotonTargets.MasterClient)
		{
			PhotonNetwork.RPC(this, methodName, PhotonNetwork.masterClient, parameters);
		}
		else
		{
			PhotonNetwork.RPC(this, methodName, target, parameters);
		}
	}

	public override string ToString()
	{
		object[] args = new object[4]
		{
			viewID,
			(base.gameObject == null) ? "GO==null" : base.gameObject.name,
			(!isSceneView) ? string.Empty : "(scene)",
			prefix
		};
		return string.Format("View ({3}){0} on {1} {2}", args);
	}

	internal static int[] CreateNewViewID(string path, PhotonView view)
	{
		if (view == null)
		{
			return new int[0];
		}
		GameObject gameObject = CacheResources.Load<GameObject>(path);
		if (gameObject == null)
		{
			return new int[0];
		}
		int[] array = new int[gameObject.GetComponentsInChildren<PhotonView>(includeInactive: true).Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PhotonNetwork.AllocateViewID();
		}
		GameObject gameObject2 = view.gameObject;
		if (gameObject2 == null)
		{
			return array;
		}
		PhotonView[] componentsInChildren = gameObject2.GetComponentsInChildren<PhotonView>(includeInactive: true);
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return array;
		}
		PhotonView photonView = componentsInChildren[0];
		int num = array[0];
		if (FengGameManagerMKII.instance != null && (FengGameManagerMKII.PView.instantiationId == num || FengGameManagerMKII.PView.viewID == photonView.viewID))
		{
			return new int[0];
		}
		if (componentsInChildren.Length != array.Length)
		{
			throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
		}
		for (int j = 0; j < array.Length; j++)
		{
			componentsInChildren[j].viewID = 0;
			componentsInChildren[j].prefix = -1;
			componentsInChildren[j].prefixBackup = -1;
			componentsInChildren[j].instantiationId = -1;
		}
		short currentLevelPrefix = PhotonNetwork.networkingPeer.currentLevelPrefix;
		PhotonNetwork.networkingPeer.StoreInstantiationData(num, null);
		for (int k = 0; k < array.Length; k++)
		{
			componentsInChildren[k].viewID = array[k];
			componentsInChildren[k].prefix = currentLevelPrefix;
			componentsInChildren[k].instantiationId = num;
			componentsInChildren[k].instantiationData = PhotonNetwork.networkingPeer.FetchInstantiationData(num);
		}
		PhotonView[] componentsInChildren2 = gameObject.GetComponentsInChildren<PhotonView>(includeInactive: true);
		for (int l = 0; l < componentsInChildren2.Length; l++)
		{
			componentsInChildren2[l].viewID = 0;
			componentsInChildren2[l].prefix = -1;
			componentsInChildren2[l].prefixBackup = -1;
			componentsInChildren2[l].instantiationId = -1;
		}
		PhotonNetwork.networkingPeer.RemoveInstantiationData(num);
		return array;
	}
}
