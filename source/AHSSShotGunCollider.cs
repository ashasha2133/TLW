using System.Collections;
using BRM;
using UnityEngine;

public class AHSSShotGunCollider : MonoBehaviour
{
    private Transform baseT;

    public static PhotonView basePV;

    public bool active_me;

	private int count;

	public ArrayList currentHits = new ArrayList();

	public int dmg = 1;

	private int myTeam = 1;

	private string ownerName = string.Empty;

	public float scoreMulti;

	private int viewID = -1;

	private int Damage(Vector3 vector)
	{
		return Mathf.Max(10, (int)((IN_GAME_MAIN_CAMERA.main_objectR.velocity - vector).magnitude * 10f * scoreMulti));
	}

	private bool checkIfBehind(GameObject titan)
	{
		Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
		Vector3 to = base.transform.position - transform.transform.position;
		return Vector3.Angle(-transform.transform.forward, to) < 100f;
	}

	private void FixedUpdate()
	{
		if (count > 1)
		{
			active_me = false;
		}
		else
		{
			count++;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !base.transform.root.gameObject.GetPhotonView().isMine) || !active_me)
		{
			return;
		}
		if (other.gameObject.tag == "playerHitbox")
		{
			if (!LevelInfo.getInfo(FengGameManagerMKII.level).pvp)
			{
				return;
			}
			float b = 1f - Vector3.Distance(other.gameObject.transform.position, base.transform.position) * 0.05f;
			b = Mathf.Min(1f, b);
			HitBox component = other.gameObject.GetComponent<HitBox>();
			if (!(component != null) || !(component.transform.root != null) || component.transform.root.GetComponent<HERO>().myTeam == myTeam || component.transform.root.GetComponent<HERO>().isInvincible())
			{
				return;
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (!component.transform.root.GetComponent<HERO>().isGrabbed)
				{
					Vector3 vector = component.transform.root.transform.position - base.transform.position;
					component.transform.root.GetComponent<HERO>().die(vector.normalized * b * 1000f + Vector3.up * 50f, isBite: false);
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !component.transform.root.GetComponent<HERO>().HasDied() && !component.transform.root.GetComponent<HERO>().isGrabbed)
			{
				component.transform.root.GetComponent<HERO>().markDie();
				object[] parameters = new object[5]
				{
					(component.transform.root.position - base.transform.position).normalized * b * 1000f + Vector3.up * 50f,
					false,
					viewID,
					ownerName,
					false
				};
				component.transform.root.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters);
			}
		}
		else if (other.gameObject.tag == "erenHitbox")
		{
			if (dmg > 0 && !other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().isHit)
			{
				other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByTitan();
			}
		}
		else if (other.gameObject.tag == "titanneck")
		{
			HitBox component2 = other.gameObject.GetComponent<HitBox>();
			if (!(component2 != null) || !checkIfBehind(component2.transform.root.gameObject) || currentHits.Contains(component2))
			{
				return;
			}
			component2.hitPosition = (base.transform.position + component2.transform.position) * 0.5f;
			currentHits.Add(component2);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (component2.transform.root.GetComponent<TITAN>() != null && !component2.transform.root.GetComponent<TITAN>().hasDie)
				{
					int num = Damage(component2.transform.root.rigidbody.velocity);
					FengGameManagerMKII.instance.netShowDamage(num);
					GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatConsoleFeed("You", component2.transform.root.GetComponent<TITAN>().name, num);
					if ((float)num > component2.transform.root.GetComponent<TITAN>().myLevel * 100f)
					{
						component2.transform.root.GetComponent<TITAN>().die();
						if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
						{
							IN_GAME_MAIN_CAMERA.mainCamera.startSnapShot2(component2.transform.position, num, component2.transform.root.gameObject, 0.02f);
						}
						FengGameManagerMKII.instance.playerKillInfoSingleUpdate(num);
					}
				}
			}
			else if (!PhotonNetwork.isMasterClient)
			{
				if (component2.transform.root.GetComponent<TITAN>() != null)
				{
					if (!component2.transform.root.GetComponent<TITAN>().hasDie)
					{
						int num2 = Damage(component2.transform.root.rigidbody.velocity);
						if ((float)num2 > component2.transform.root.GetComponent<TITAN>().myLevel * 100f)
						{
							if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
							{
								IN_GAME_MAIN_CAMERA.mainCamera.startSnapShot2(component2.transform.position, num2, component2.transform.root.gameObject, 0.02f);
								component2.transform.root.GetComponent<TITAN>().asClientLookTarget = false;
							}
							object[] parameters2 = new object[2]
							{
								base.transform.root.gameObject.GetPhotonView().viewID,
								num2
							};
							component2.transform.root.GetComponent<TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<TITAN>().photonView.owner, parameters2);
						}
					}
				}
				else if (component2.transform.root.GetComponent<FEMALE_TITAN>() != null)
				{
					int num3 = Damage(component2.transform.root.rigidbody.velocity);
					if (!component2.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
					{
						object[] parameters3 = new object[2]
						{
							base.transform.root.gameObject.GetPhotonView().viewID,
							num3
						};
						component2.transform.root.GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<FEMALE_TITAN>().photonView.owner, parameters3);
					}
				}
				else if (component2.transform.root.GetComponent<COLOSSAL_TITAN>() != null && !component2.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
				{
					int num4 = Damage(component2.transform.root.rigidbody.velocity);
					object[] parameters4 = new object[2]
					{
						base.transform.root.gameObject.GetPhotonView().viewID,
						num4
					};
					component2.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", component2.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, parameters4);
				}
			}
			else if (component2.transform.root.GetComponent<TITAN>() != null)
			{
				if (!component2.transform.root.GetComponent<TITAN>().hasDie)
				{
					int num5 = Damage(component2.transform.root.rigidbody.velocity);
					if ((float)num5 > component2.transform.root.GetComponent<TITAN>().myLevel * 100f)
					{
						if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
						{
							IN_GAME_MAIN_CAMERA.mainCamera.startSnapShot2(component2.transform.position, num5, component2.transform.root.gameObject, 0.02f);
						}
						component2.transform.root.GetComponent<TITAN>().titanGetHit(basePV.viewID, num5);
					}
				}
			}
			else if (component2.transform.root.GetComponent<FEMALE_TITAN>() != null)
			{
				if (!component2.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
				{
					int speed = Damage(component2.rigidbody.velocity);
					if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
					{
						IN_GAME_MAIN_CAMERA.mainCamera.startSnapShot2(component2.transform.position, speed, null, 0.02f);
					}
					component2.transform.root.GetComponent<FEMALE_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, speed);
				}
			}
			else if (component2.transform.root.GetComponent<COLOSSAL_TITAN>() != null && !component2.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
			{
				int speed2 = Damage(component2.rigidbody.velocity);
				if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
				{
					IN_GAME_MAIN_CAMERA.mainCamera.startSnapShot2(component2.transform.position, speed2, null, 0.02f);
				}
				component2.transform.root.GetComponent<COLOSSAL_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, speed2);
			}
			showCriticalHitFX(other.gameObject.transform.position);
		}
		else if (other.gameObject.tag == "titaneye")
		{
			if (currentHits.Contains(other.gameObject))
			{
				return;
			}
			currentHits.Add(other.gameObject);
			GameObject gameObject = other.gameObject.transform.root.gameObject;
			if (gameObject.GetComponent<FEMALE_TITAN>() != null)
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
					{
						gameObject.GetComponent<FEMALE_TITAN>().hitEye();
					}
				}
				else if (!PhotonNetwork.isMasterClient)
				{
					if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
					{
						object[] parameters5 = new object[1] { base.transform.root.gameObject.GetPhotonView().viewID };
						gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, parameters5);
					}
				}
				else if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
				{
					gameObject.GetComponent<FEMALE_TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
				}
			}
			else
			{
				if (gameObject.GetComponent<TITAN>().abnormalType == AbnormalType.TYPE_CRAWLER)
				{
					return;
				}
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					if (!gameObject.GetComponent<TITAN>().hasDie)
					{
						gameObject.GetComponent<TITAN>().hitEye();
					}
				}
				else if (!PhotonNetwork.isMasterClient)
				{
					if (!gameObject.GetComponent<TITAN>().hasDie)
					{
						object[] parameters6 = new object[1] { base.transform.root.gameObject.GetPhotonView().viewID };
						gameObject.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, parameters6);
					}
				}
				else if (!gameObject.GetComponent<TITAN>().hasDie)
				{
					gameObject.GetComponent<TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
				}
				showCriticalHitFX(other.gameObject.transform.position);
			}
		}
		else
		{
			if (!(other.gameObject.tag == "titanankle") || currentHits.Contains(other.gameObject))
			{
				return;
			}
			currentHits.Add(other.gameObject);
			GameObject gameObject2 = other.gameObject.transform.root.gameObject;
			int num6 = Damage(gameObject2.rigidbody.velocity);
			if (gameObject2.GetComponent<TITAN>() != null && gameObject2.GetComponent<TITAN>().abnormalType != AbnormalType.TYPE_CRAWLER)
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					if (!gameObject2.GetComponent<TITAN>().hasDie)
					{
						gameObject2.GetComponent<TITAN>().hitAnkle();
					}
					return;
				}
				if (!PhotonNetwork.isMasterClient)
				{
					if (!gameObject2.GetComponent<TITAN>().hasDie)
					{
						object[] parameters7 = new object[1] { base.transform.root.gameObject.GetPhotonView().viewID };
						gameObject2.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, parameters7);
					}
				}
				else if (!gameObject2.GetComponent<TITAN>().hasDie)
				{
					gameObject2.GetComponent<TITAN>().hitAnkle();
				}
				showCriticalHitFX(other.gameObject.transform.position);
			}
			else
			{
				if (!(gameObject2.GetComponent<FEMALE_TITAN>() != null))
				{
					return;
				}
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					if (other.gameObject.name == "ankleR")
					{
						if (gameObject2.GetComponent<FEMALE_TITAN>() != null && !gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
						{
							gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleR(num6);
						}
					}
					else if (gameObject2.GetComponent<FEMALE_TITAN>() != null && !gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
					{
						gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleL(num6);
					}
				}
				else if (other.gameObject.name == "ankleR")
				{
					if (!PhotonNetwork.isMasterClient)
					{
						if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
						{
							object[] parameters8 = new object[2]
							{
								base.transform.root.gameObject.GetPhotonView().viewID,
								num6
							};
							gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, parameters8);
						}
					}
					else if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
					{
						gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleRRPC(base.transform.root.gameObject.GetPhotonView().viewID, num6);
					}
				}
				else if (!PhotonNetwork.isMasterClient)
				{
					if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
					{
						object[] parameters9 = new object[2]
						{
							base.transform.root.gameObject.GetPhotonView().viewID,
							num6
						};
						gameObject2.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, parameters9);
					}
				}
				else if (!gameObject2.GetComponent<FEMALE_TITAN>().hasDie)
				{
					gameObject2.GetComponent<FEMALE_TITAN>().hitAnkleLRPC(base.transform.root.gameObject.GetPhotonView().viewID, num6);
				}
				showCriticalHitFX(other.gameObject.transform.position);
			}
		}
	}

	private void showCriticalHitFX(Vector3 position)
	{
		IN_GAME_MAIN_CAMERA.mainCamera.startShake(0.2f, 0.3f);
		GameObject gameObject = ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) ? ((GameObject)Object.Instantiate(CacheResources.Load("redCross1"))) : PhotonNetwork.Instantiate("redCross1", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0));
		gameObject.transform.position = position;
	}

	private void Start()
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			if (!base.transform.root.gameObject.GetPhotonView().isMine)
			{
				base.enabled = false;
				return;
			}
			if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
			{
				viewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
				ownerName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
				myTeam = PhotonView.Find(viewID).gameObject.GetComponent<HERO>().myTeam;
			}
		}
		else
		{
			myTeam = IN_GAME_MAIN_CAMERA.mainHERO.myTeam;
		}
		active_me = true;
		count = 0;
		baseT = base.transform;
		basePV = baseT.root.gameObject.GetPhotonView();
    }
}
