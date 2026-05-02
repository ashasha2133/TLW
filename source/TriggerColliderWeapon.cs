using System.Collections;
using BRM;
using UnityEngine;

public class TriggerColliderWeapon : MonoBehaviour
{
	public bool active_me;

	public ArrayList currentHits = new ArrayList();

	public ArrayList currentHitsII = new ArrayList();

	//public AudioSource meatDie;

	public int myTeam = 1;

	public float scoreMulti = 1f;

	private GameObject heroG;

	private HERO hero;

	private Transform baseT;

	public static PhotonView basePV;

	private IN_GAME_MAIN_CAMERA mainCamera;

	private bool checkIfBehind(GameObject titan)
	{
		Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
		return Vector3.Angle(-transform.forward, baseT.position - transform.position) < 70f;
	}

	public void clearHits()
	{
		currentHitsII = new ArrayList();
		currentHits = new ArrayList();
	}

	private void napeMeat(Vector3 vkill, Transform titan)
	{
		if (GameSettings.napeMelons <= 0)
		{
			return;
		}
		TITAN component = titan.root.GetComponent<TITAN>();
		if (!(component == null))
		{
			Transform transform = component.neck;
			if (transform == null)
			{
				transform = titan.root.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
			}
			GameObject obj = (GameObject)Object.Instantiate(CacheResources.Load("titanNapeMeat"), transform.position, transform.rotation);
			obj.transform.localScale = titan.localScale;
			Rigidbody obj2 = obj.rigidbody;
			obj2.AddForce(vkill.normalized * 15f, ForceMode.Impulse);
			obj2.AddForce(-titan.forward * 10f, ForceMode.Impulse);
			obj2.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
		}
	}

	private int Damage(Component component)
	{
		return Mathf.Max(10, (int)((hero.rigidbody.velocity - component.rigidbody.velocity).magnitude * 10f));
	}

	private void OnTriggerStay(Collider other)
	{
		if (!active_me)
		{
			return;
		}
		GameObject gameObject = other.gameObject;
		Transform transform = other.transform;
		if (!currentHitsII.Contains(gameObject))
		{
			currentHitsII.Add(gameObject);
			mainCamera.startShake(0.1f, 0.1f);
			if (transform.root.gameObject.CompareTag("titan"))
			{
				//hero.slashHit.Play();
				if (IN_GAME_MAIN_CAMERA.gametype != 0)
				{
					PhotonNetwork.Instantiate("hitMeat", baseT.position, Quaternion.Euler(270f, 0f, 0f), 0, null);
				}
				else
				{
					((GameObject)Object.Instantiate(CacheResources.Load("hitMeat"))).transform.position = baseT.position;
				}
				hero.useBlade(0);
			}
		}
		string text;
		if ((text = gameObject.tag) == null)
		{
			return;
		}
		HitBox component = gameObject.GetComponent<HitBox>();
		switch (text)
		{
		case "playerHitbox":
		{
			if (!LevelInfo.getInfo(FengGameManagerMKII.level).pvp)
			{
				break;
			}
			float b = 1f - Vector3.Distance(transform.position, baseT.position) * 0.05f;
			b = Mathf.Min(1f, b);
			Transform root;
			Transform transform2;
			if (!((root = (transform2 = component.transform).root) != null))
			{
				break;
			}
			HERO component3 = root.GetComponent<HERO>();
			if (!(component3 != null) || component3.myTeam == myTeam || component3.isInvincible() || component3.isGrabbed)
			{
				break;
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				component3.die((root.position - baseT.position).normalized * b * 1000f + Vector3.up * 50f, isBite: false);
			}
			else if (!component3.HasDied())
			{
				HERO hERO = HERO.FindMyHero(PhotonNetwork.player.ID);
				if (!hERO.BeingCarried || (hERO.PersonWhoCarries != null && hERO.PersonWhoCarries != component.transform.root.GetComponent<HERO>()))
				{
					component3.markDie();
					object[] parameters4 = new object[5]
					{
						(root.position - baseT.position).normalized * b * 1000f + Vector3.up * 50f,
						false,
						basePV.viewID,
						basePV.owner.customProperties["name"],
						false
					};
					component3.photonView.RPC("netDie", PhotonTargets.All, parameters4);
				}
			}
			break;
		}
		case "titanneck":
		{
			GameObject gameObject2;
			Transform root;
			Transform transform2;
			if (!((gameObject2 = (root = (transform2 = component.transform).root).gameObject) != null) || !checkIfBehind(gameObject2) || currentHits.Contains(component))
			{
				break;
			}
			component.hitPosition = (baseT.position + transform2.position) * 0.5f;
			currentHits.Add(component);
			//meatDie.Play();
			MONO component2 = root.GetComponent<MONO>();
			switch (component2.species)
			{
			case SPECIES.Titan:
			{
				TITAN tITAN2 = component2 as TITAN;
				if (!(tITAN2 != null) || tITAN2.hasDie)
				{
					break;
				}
				int num3 = Damage(root);
				napeMeat(hero.rigidbody.velocity, root);
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
					{
						mainCamera.startSnapShot2(transform2.position, num3, gameObject2, 0.02f);
					}
					tITAN2.die();
					FengGameManagerMKII.instance.netShowDamage(num3);
					GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatConsoleFeed("You", tITAN2.transform.root.GetComponent<TITAN>().name, num3);
					FengGameManagerMKII.instance.playerKillInfoSingleUpdate(num3);
				}
				else if (!PhotonNetwork.isMasterClient)
				{
					if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
					{
						mainCamera.startSnapShot2(transform2.position, num3, gameObject2, 0.02f);
						tITAN2.asClientLookTarget = false;
					}
					object[] parameters6 = new object[2] { basePV.viewID, num3 };
					tITAN2.photonView.RPC("titanGetHit", tITAN2.photonView.owner, parameters6);
				}
				else
				{
					if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
					{
						mainCamera.startSnapShot2(transform2.position, num3, gameObject2, 0.02f);
					}
					tITAN2.titanGetHit(basePV.viewID, num3);
				}
				break;
			}
			case SPECIES.FemaleTitan:
			{
				FEMALE_TITAN fEMALE_TITAN2 = component2 as FEMALE_TITAN;
				hero.useBlade(int.MaxValue);
				if (!(fEMALE_TITAN2 != null) || fEMALE_TITAN2.hasDie)
				{
					break;
				}
				int num4 = Damage(root);
				if (!PhotonNetwork.isMasterClient)
				{
					object[] parameters7 = new object[2] { basePV.viewID, num4 };
					fEMALE_TITAN2.photonView.RPC("titanGetHit", fEMALE_TITAN2.photonView.owner, parameters7);
				}
				else
				{
					if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
					{
						mainCamera.startSnapShot2(transform2.position, num4, null, 0.02f);
					}
					fEMALE_TITAN2.titanGetHit(basePV.viewID, num4);
				}
				break;
			}
			default:
			{
				if (component2.species != SPECIES.ColossalTitan)
				{
					break;
				}
				COLOSSAL_TITAN cOLOSSAL_TITAN = component2 as COLOSSAL_TITAN;
				hero.useBlade(int.MaxValue);
				if (!cOLOSSAL_TITAN.hasDie)
				{
					int num2 = Damage(root);
					if (!PhotonNetwork.isMasterClient)
					{
						object[] parameters5 = new object[2] { basePV.viewID, num2 };
						cOLOSSAL_TITAN.photonView.RPC("titanGetHit", cOLOSSAL_TITAN.photonView.owner, parameters5);
					}
					else if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
					{
						mainCamera.startSnapShot2(transform2.position, num2, null, 0.02f);
					}
					cOLOSSAL_TITAN.titanGetHit(basePV.viewID, num2);
				}
				break;
			}
			case SPECIES.Hero:
				break;
			}
			showCriticalHitFX();
			break;
		}
		case "titaneye":
		{
			if (currentHits.Contains(gameObject))
			{
				break;
			}
			currentHits.Add(gameObject);
			Transform transform2 = gameObject.transform;
			if (!(transform2 != null))
			{
				break;
			}
			MONO component2 = transform2.root.GetComponent<MONO>();
			if (!(component2 != null))
			{
				break;
			}
			switch (component2.species)
			{
			case SPECIES.Titan:
			{
				TITAN tITAN3 = component2 as TITAN;
				if (tITAN3.abnormalType != AbnormalType.TYPE_CRAWLER && !tITAN3.hasDie)
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						tITAN3.hitEye();
					}
					else if (!PhotonNetwork.isMasterClient)
					{
						object[] parameters9 = new object[1] { basePV.viewID };
						tITAN3.photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, parameters9);
					}
					else
					{
						tITAN3.hitEyeRPC(baseT.root.gameObject.GetPhotonView().viewID);
					}
					showCriticalHitFX();
				}
				break;
			}
			case SPECIES.FemaleTitan:
			{
				FEMALE_TITAN fEMALE_TITAN3 = component2 as FEMALE_TITAN;
				if (!fEMALE_TITAN3.hasDie)
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						fEMALE_TITAN3.hitEye();
					}
					else if (!PhotonNetwork.isMasterClient)
					{
						object[] parameters8 = new object[1] { basePV.viewID };
						fEMALE_TITAN3.photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, parameters8);
					}
					else
					{
						fEMALE_TITAN3.hitEyeRPC(basePV.viewID);
					}
					showCriticalHitFX();
				}
				break;
			}
			}
			break;
		}
		case "titanankle":
		{
			if (currentHits.Contains(gameObject))
			{
				break;
			}
			currentHits.Add(gameObject);
			Transform transform2 = gameObject.transform;
			if (!(transform2 != null))
			{
				break;
			}
			MONO component2 = transform2.root.GetComponent<MONO>();
			switch (component2.species)
			{
			case SPECIES.Titan:
			{
				TITAN tITAN = component2 as TITAN;
				if (tITAN.abnormalType != AbnormalType.TYPE_CRAWLER && !tITAN.hasDie)
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						tITAN.hitAnkle();
					}
					else if (!PhotonNetwork.isMasterClient)
					{
						object[] parameters3 = new object[1] { basePV.viewID };
						tITAN.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, parameters3);
					}
					else
					{
						tITAN.hitAnkle();
					}
					showCriticalHitFX();
				}
				break;
			}
			case SPECIES.FemaleTitan:
			{
				FEMALE_TITAN fEMALE_TITAN = component2 as FEMALE_TITAN;
				if (fEMALE_TITAN.hasDie)
				{
					break;
				}
				int num = Damage(fEMALE_TITAN);
				if (gameObject.name == "ankleR")
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						fEMALE_TITAN.hitAnkleR(num);
						FengGameManagerMKII.instance.netShowDamage(num);
					}
					else if (!PhotonNetwork.isMasterClient)
					{
						object[] parameters = new object[1] { basePV.viewID };
						fEMALE_TITAN.photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, parameters);
					}
					else
					{
						fEMALE_TITAN.hitAnkleRRPC(basePV.viewID, num);
					}
				}
				else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					fEMALE_TITAN.hitAnkleL(num);
					FengGameManagerMKII.instance.netShowDamage(num);
				}
				else if (!PhotonNetwork.isMasterClient)
				{
					object[] parameters2 = new object[1] { basePV.viewID };
					fEMALE_TITAN.photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, parameters2);
				}
				else
				{
					fEMALE_TITAN.hitAnkleLRPC(basePV.viewID, num);
				}
				showCriticalHitFX();
				break;
			}
			}
			break;
		}
		}
	}

	private void showCriticalHitFX()
	{
		mainCamera.startShake(0.2f, 0.3f);
		GameObject gameObject = ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) ? ((GameObject)Object.Instantiate(CacheResources.Load("redCross"))) : PhotonNetwork.Instantiate("redCross", baseT.position, Quaternion.Euler(270f, 0f, 0f), 0));
		gameObject.transform.position = baseT.position;
		gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}

	private void Start()
	{
		hero = (heroG = (baseT = base.transform).root.gameObject).GetComponent<HERO>();
		basePV = baseT.root.GetComponent<PhotonView>();
		mainCamera = IN_GAME_MAIN_CAMERA.mainCamera;
	}
}
