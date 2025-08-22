using Photon;
using UnityEngine;

public class SmoothSyncMovement : Photon.MonoBehaviour
{
	public Quaternion correctCameraRot;

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	private Vector3 correctPlayerVelocity = Vector3.zero;

	public bool disabled;

	public bool noVelocity;

	public bool PhotonCamera;

	public float SmoothingDelay = 5f;

	private Transform baseT;

	private Rigidbody baseR;

	private PhotonView basePV;

	private bool isMine
	{
		get
		{
			if (basePV == null)
			{
				if ((basePV = GetComponent<PhotonView>()) != null)
				{
					return basePV.isMine;
				}
				return false;
			}
			if (basePV != null)
			{
				return basePV.isMine;
			}
			return false;
		}
	}

	public void Awake()
	{
		baseT = base.transform;
		baseR = base.rigidbody;
		basePV = GetComponent<PhotonView>();
		if (!(basePV == null))
		{
			_ = basePV.observed != this;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			base.enabled = false;
		}
		if (baseT == null)
		{
			baseT = base.transform;
		}
		if (baseR == null)
		{
			baseR = base.rigidbody;
		}
		if (basePV == null)
		{
			basePV = GetComponent<PhotonView>();
		}
		correctPlayerPos = base.transform.position;
		correctPlayerRot = base.transform.rotation;
		if (base.rigidbody == null)
		{
			noVelocity = true;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			if (!noVelocity)
			{
				stream.SendNext(base.rigidbody.velocity);
			}
			if (PhotonCamera)
			{
				stream.SendNext(Camera.main.transform.rotation);
			}
		}
		else
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
			if (!noVelocity)
			{
				correctPlayerVelocity = (Vector3)stream.ReceiveNext();
			}
			if (PhotonCamera)
			{
				correctCameraRot = (Quaternion)stream.ReceiveNext();
			}
		}
	}

	public void Update()
	{
		if (disabled || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !(basePV != null) || basePV.isMine)
		{
			return;
		}
		if (baseT == null)
		{
			baseT = base.transform;
		}
		if (baseR == null)
		{
			baseR = base.rigidbody;
		}
		if (baseT != null)
		{
			if (correctPlayerPos != baseT.position)
			{
				baseT.position = Vector3.Lerp(baseT.position, correctPlayerPos, 5f * Time.deltaTime);
			}
			if (correctPlayerRot != baseT.rotation)
			{
				baseT.rotation = Quaternion.Lerp(baseT.rotation, correctPlayerRot, 5f * Time.deltaTime);
			}
		}
		if (baseR != null)
		{
			baseR.velocity = correctPlayerVelocity;
		}
	}
}
