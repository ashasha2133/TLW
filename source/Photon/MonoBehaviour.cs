using BRM;
using UnityEngine;

namespace Photon;

public class MonoBehaviour : UnityEngine.MonoBehaviour
{
	internal Pool pool;

	internal Pool homePool;

	internal bool isBackground;

	private PhotonView view;

	public new PhotonView networkView => PhotonView.Get(this);

	public PhotonView photonView
	{
		get
		{
			if (view != null)
			{
				return view;
			}
			return view = GetComponent<PhotonView>();
		}
	}

	public bool isLocal
	{
		get
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				return true;
			}
			PhotonView photonView = this.photonView;
			if (photonView != null)
			{
				return photonView.isMine;
			}
			return false;
		}
	}

	private void Awake()
	{
		if (this is PhotonView)
		{
			if ((bool)(view = (PhotonView)this) && !isBackground)
			{
				isBackground = view.isSceneView;
			}
		}
		else if ((bool)(view = GetComponent<PhotonView>()) && !isBackground)
		{
			isBackground = view.isSceneView;
		}
	}

	private void OnDestroy()
	{
		if (pool != null)
		{
			pool.Clear();
		}
	}
}
