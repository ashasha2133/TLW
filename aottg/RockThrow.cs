using BRM;
using Photon;
using UnityEngine;

public class RockThrow : Photon.MonoBehaviour
{
	private bool launched;

	private Vector3 oldP;

	private Vector3 r;

	private Vector3 v;

	private Transform baseT;

	private PhotonView basePV;

	private int viewID = -1;

	private string titanName = string.Empty;

	private void Awake()
	{
		baseT = base.transform;
		basePV = base.photonView;
	}

	private void explore()
	{
		GameObject gameObject;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && (PhotonNetwork.isMasterClient || basePV.isMine))
		{
			gameObject = PhotonNetwork.Instantiate("FX/boom6", baseT.position, baseT.rotation, 0);
			EnemyfxIDcontainer component = base.gameObject.GetComponent<EnemyfxIDcontainer>();
			if (component != null)
			{
				component.myOwnerViewID = viewID;
				component.titanName = titanName;
			}
		}
		else
		{
			gameObject = (GameObject)Object.Instantiate(CacheResources.Load("FX/boom6"), baseT.position, baseT.rotation);
		}
		gameObject.transform.localScale = baseT.localScale;
		float b = 1f - Vector3.Distance(IN_GAME_MAIN_CAMERA.mainT.position, gameObject.transform.position) * 0.05f;
		b = Mathf.Min(1f, b);
		IN_GAME_MAIN_CAMERA.mainCamera.startShake(b, b);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			PhotonNetwork.Destroy(basePV);
		}
	}

	private void hitPlayer(GameObject hero, HERO component)
	{
		if (!(hero != null) || component.HasDied() || component.isInvincible())
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (!component.isGrabbed)
			{
				component.die(v.normalized * 1000f + Vector3.up * 50f, isBite: false);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !component.HasDied() && !component.isGrabbed)
		{
			component.markDie();
			object[] parameters = new object[5]
			{
				v.normalized * 1000f + Vector3.up * 50f,
				false,
				viewID,
				titanName,
				true
			};
			component.photonView.RPC("netDie", PhotonTargets.All, parameters);
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			GameObject obj = PhotonNetwork.Instantiate("FX/boom6", baseT.position, baseT.rotation, 0);
			obj.transform.localScale = baseT.localScale;
			EnemyfxIDcontainer component2 = obj.GetComponent<EnemyfxIDcontainer>();
			if (component2 != null)
			{
				component2.myOwnerViewID = viewID;
				component2.titanName = titanName;
			}
		}
		else
		{
			((GameObject)Object.Instantiate(CacheResources.Load("FX/boom6"), baseT.position, baseT.rotation)).transform.localScale = baseT.localScale;
		}
	}

	[RPC]
	private void initRPC(int viewID, Vector3 scale, Vector3 pos, float level)
	{
		PhotonView photonView = PhotonView.Find(viewID);
		if (!(photonView != null))
		{
			return;
		}
		Transform transform = photonView.transform;
		if (transform != null)
		{
			Transform transform2 = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			if (transform2 != null)
			{
				baseT.localScale = transform.localScale;
				baseT.parent = transform2;
				baseT.localPosition = pos;
			}
		}
	}

	public void launch(Vector3 v1, string titanName = null, int viewID = -1)
	{
		launched = true;
		oldP = baseT.position;
		v = v1;
		if (!string.IsNullOrEmpty(titanName))
		{
			this.titanName = titanName;
		}
		if (viewID != -1)
		{
			this.viewID = viewID;
		}
		EnemyfxIDcontainer component = GetComponent<EnemyfxIDcontainer>();
		if (component != null)
		{
			component.titanName = titanName;
			component.myOwnerViewID = viewID;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			object[] parameters = new object[2] { v, oldP };
			base.photonView.RPC("launchRPC", PhotonTargets.Others, parameters);
		}
	}

	[RPC]
	private void launchRPC(Vector3 v, Vector3 p)
	{
		if (basePV != null && !basePV.isMine)
		{
			launched = true;
			baseT.position = p;
			oldP = p;
			baseT.parent = null;
			launch(v);
		}
	}

	private void Start()
	{
		r = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
	}

	private void Update()
	{
		if (!launched)
		{
			return;
		}
		baseT.Rotate(r);
		v -= 20f * Vector3.up * Time.deltaTime;
		baseT.position += v * Time.deltaTime;
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine)
		{
			return;
		}
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask2 = 1 << LayerMask.NameToLayer("Players");
		LayerMask layerMask3 = 1 << LayerMask.NameToLayer("EnemyAABB");
		LayerMask layerMask4 = (int)layerMask2 | (int)layerMask | (int)layerMask3;
		RaycastHit[] array = Physics.SphereCastAll(baseT.position, 2.5f * baseT.lossyScale.x, baseT.position - oldP, Vector3.Distance(baseT.position, oldP), layerMask4);
		foreach (RaycastHit raycastHit in array)
		{
			Collider collider = raycastHit.collider;
			if (collider == null)
			{
				continue;
			}
			GameObject gameObject = collider.gameObject;
			if (gameObject == null)
			{
				continue;
			}
			GameObject gameObject2 = gameObject.transform.root.gameObject;
			if (gameObject2 == null)
			{
				continue;
			}
			switch (gameObject.layer)
			{
			case 8:
			case 13:
			{
				if (!(gameObject2.tag.NullFix() == "Player"))
				{
					break;
				}
				MONO component2 = gameObject2.GetComponent<MONO>();
				if (component2 == null)
				{
					break;
				}
				if (component2.species == SPECIES.Hero)
				{
					HERO hERO = component2 as HERO;
					if (!hERO.HasDied() && !hERO.isInvincible())
					{
						hitPlayer(gameObject2, hERO);
					}
				}
				else if (component2.species == SPECIES.TitanEren)
				{
					TITAN_EREN tITAN_EREN = component2 as TITAN_EREN;
					if (!tITAN_EREN.isHit)
					{
						tITAN_EREN.hitByTitan();
					}
				}
				break;
			}
			case 9:
				explore();
				break;
			case 11:
			{
				TITAN component = gameObject2.GetComponent<TITAN>();
				if (component != null && !component.hasDie)
				{
					_ = baseT.position;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						component.hitAnkle();
					}
					else
					{
						PhotonView photonView = PhotonView.Find(viewID);
						if (photonView != null)
						{
							_ = photonView.transform.position;
						}
						component.photonView.RPC("hitAnkleRPC", PhotonTargets.All);
					}
				}
				explore();
				break;
			}
			}
		}
		oldP = baseT.position;
	}
}
