using System;
using System.Collections;
using System.Collections.Generic;
using BRM;
using ExitGames.Client.Photon;
using UnityEngine;

public class TITAN : MONO
{
	private HashSet<string> _ignoreLookTargetAnimations;

	private HashSet<string> _fastHeadRotationAnimations;

	private bool _ignoreLookTarget;

	private bool _fastHeadRotation;

	public bool isThunderSpear;

	private static Dictionary<string, int> f__switchmap7;

	private Vector3 abnorma_jump_bite_horizon_v;

	public AbnormalType abnormalType;

	public int activeRad = int.MaxValue;

	private float angle;

	public bool asClientLookTarget;

	private string attackAnimation;

	private float attackCheckTime;

	private float attackCheckTimeA;

	private float attackCheckTimeB;

	private int attackCount;

	public float attackDistance = 13f;

	private bool attacked;

	private float attackEndWait;

	public float attackWait = 1f;

	//public AudioSource baseAudioSource;

	public List<Collider> baseColliders;

	private float between2;

	public float chaseDistance = 80f;

	public ArrayList checkPoints = new ArrayList();

	public bool colliderEnabled;

	public TITAN_CONTROLLER controller;

	private Transform currentGrabHand;

	public int currentHealth;

	private float desDeg;

	private float dieTime;

	public bool eye;

	internal List<Bullet> hooks = new List<Bullet>();

	private static Dictionary<string, int> fswitchmap5;

	private static Dictionary<string, int> fswitchmap6;

	private string fxName;

	private Vector3 fxPosition;

	private Quaternion fxRotation;

	private float getdownTime;

	public GameObject grabTF;

	private float gravity = 120f;

	private bool grounded;

	public bool hasDie;

	private bool hasDieSteam;

	public bool hasExplode;

	public bool hasload;

	public bool hasSetLevel;

	public bool hasSpawn;

	private new Transform head;

	private Vector3 headscale = Vector3.one;

	public bool healthLabelEnabled;

	public float healthTime;

	private string hitAnimation;

	private float hitPause;

	public bool isAlarm;

	private bool isAttackMoveByCore;

	private bool isGrabHandLeft;

	public bool isHooked;

	public bool isLook;

	public float lagMax;

	private bool leftHandAttack;

	public GameObject mainMaterial;

	public int maxHealth;

	private float maxStamina = 320f;

	public float maxVelocityChange = 10f;

	public static float minusDistance = 99999f;

	public static GameObject minusDistanceEnemy;

	public int myDifficulty;

	public float myDistance;

	public GROUP myGroup = GROUP.T;

	public float myLevel = 1f;

	public TitanTrigger myTitanTrigger;

	public new Transform neck;

	private bool needFreshCorePosition;

	private string nextAttackAnimation;

	public bool nonAI;

	private bool nonAIcombo;

	private Vector3 oldCorePosition;

	private Quaternion oldHeadRotation;

	public PVPcheckPoint PVPfromCheckPt;

	private float random_run_time;

	private float rockInterval;

	public bool rockthrow;

	private string runAnimation;

	private float sbtime;

	public int skin;

	private Vector3 spawnPt;

	public float speed = 7f;

	private float stamina = 320f;

	private TitanState state;

	private int stepSoundPhase = 2;

	private bool stuck;

	private float stuckTime;

	private float stuckTurnAngle;

	private Vector3 targetCheckPt;

	private Quaternion targetHeadRotation;

	private float targetR;

	private float tauntTime;

	private string turnAnimation;

	private float turnDeg;

	private GameObject whoHasTauntMe;

	internal Transform chkOverHead;

	internal Transform chkFront;

	internal Transform chkFrontRight;

	internal Transform chkFrontLeft;

	internal Transform chkBackRight;

	internal Transform chkBackLeft;

	private Transform camT;

	public Transform baseGT;

	public HERO myHERO;

	public Transform myHeroT;

	public Rigidbody myHeroR;

	public PhotonView myHeroPV;

	private GameObject mycurrenthero;

	private HERO grabbedTarget;

	private Transform rockT;

	private GameObject throwRock;

	private UILabel healthLabel;

	private Transform healthLabelT;

	public GameObject myHero
	{
		get
		{
			return mycurrenthero;
		}
		set
		{
			mycurrenthero = value;
			if (value != null)
			{
				myHERO = value.GetComponent<HERO>();
				myHeroT = value.transform;
				myHeroR = value.rigidbody;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					myHeroPV = value.GetComponent<PhotonView>();
				}
			}
			else
			{
				myHERO = null;
				myHeroT = null;
				myHeroR = null;
				myHeroPV = null;
			}
		}
	}

	public TITAN()
		: base(SPECIES.Titan)
	{
	}

	private void attack2(string type)
	{
		state = TitanState.attack;
		attacked = false;
		isAlarm = true;
		if (attackAnimation == type)
		{
			attackAnimation = type;
			playAnimationAt("attack_" + type, 0f);
		}
		else
		{
			attackAnimation = type;
			playAnimationAt("attack_" + type, 0f);
		}
		nextAttackAnimation = null;
		fxName = null;
		isAttackMoveByCore = false;
		attackCheckTime = 0f;
		attackCheckTimeA = 0f;
		attackCheckTimeB = 0f;
		attackEndWait = 0f;
		fxRotation = Quaternion.Euler(270f, 0f, 0f);
		if (type != null)
		{
			if (fswitchmap6 == null)
			{
				fswitchmap6 = new Dictionary<string, int>(22)
				{
					{ "abnormal_getup", 0 },
					{ "abnormal_jump", 1 },
					{ "combo_1", 2 },
					{ "combo_2", 3 },
					{ "combo_3", 4 },
					{ "front_ground", 5 },
					{ "kick", 6 },
					{ "slap_back", 7 },
					{ "slap_face", 8 },
					{ "stomp", 9 },
					{ "bite", 10 },
					{ "bite_l", 11 },
					{ "bite_r", 12 },
					{ "jumper_0", 13 },
					{ "crawler_jump_0", 14 },
					{ "anti_AE_l", 15 },
					{ "anti_AE_r", 16 },
					{ "anti_AE_low_l", 17 },
					{ "anti_AE_low_r", 18 },
					{ "quick_turn_l", 19 },
					{ "quick_turn_r", 20 },
					{ "throw", 21 }
				};
			}
			if (fswitchmap6.TryGetValue(type, out var value))
			{
				switch (value)
				{
				case 0:
					attackCheckTime = 0f;
					fxName = string.Empty;
					break;
				case 1:
					nextAttackAnimation = "abnormal_getup";
					if (nonAI)
					{
						attackEndWait = 0f;
					}
					else
					{
						attackEndWait = ((myDifficulty <= 0) ? UnityEngine.Random.Range(1f, 4f) : UnityEngine.Random.Range(0f, 1f));
					}
					attackCheckTime = 0.75f;
					fxName = "boom4";
					fxRotation = Quaternion.Euler(270f, baseT.rotation.eulerAngles.y, 0f);
					break;
				case 2:
					nextAttackAnimation = "combo_2";
					attackCheckTimeA = 0.54f;
					attackCheckTimeB = 0.76f;
					nonAIcombo = false;
					isAttackMoveByCore = true;
					leftHandAttack = false;
					break;
				case 3:
					if (abnormalType != AbnormalType.TYPE_PUNK && !nonAI)
					{
						nextAttackAnimation = "combo_3";
					}
					attackCheckTimeA = 0.37f;
					attackCheckTimeB = 0.57f;
					nonAIcombo = false;
					isAttackMoveByCore = true;
					leftHandAttack = true;
					break;
				case 4:
					nonAIcombo = false;
					isAttackMoveByCore = true;
					attackCheckTime = 0.21f;
					fxName = "boom1";
					break;
				case 5:
					fxName = "boom1";
					attackCheckTime = 0.45f;
					break;
				case 6:
					fxName = "boom5";
					fxRotation = baseT.rotation;
					attackCheckTime = 0.43f;
					break;
				case 7:
					fxName = "boom3";
					attackCheckTime = 0.66f;
					break;
				case 8:
					fxName = "boom3";
					attackCheckTime = 0.655f;
					break;
				case 9:
					fxName = "boom2";
					attackCheckTime = 0.42f;
					break;
				case 10:
					fxName = "bite";
					attackCheckTime = 0.6f;
					break;
				case 11:
					fxName = "bite";
					attackCheckTime = 0.4f;
					break;
				case 12:
					fxName = "bite";
					attackCheckTime = 0.4f;
					break;
				case 13:
					abnorma_jump_bite_horizon_v = Vector3.zero;
					break;
				case 14:
					abnorma_jump_bite_horizon_v = Vector3.zero;
					break;
				case 15:
					attackCheckTimeA = 0.31f;
					attackCheckTimeB = 0.4f;
					leftHandAttack = true;
					break;
				case 16:
					attackCheckTimeA = 0.31f;
					attackCheckTimeB = 0.4f;
					leftHandAttack = false;
					break;
				case 17:
					attackCheckTimeA = 0.31f;
					attackCheckTimeB = 0.4f;
					leftHandAttack = true;
					break;
				case 18:
					attackCheckTimeA = 0.31f;
					attackCheckTimeB = 0.4f;
					leftHandAttack = false;
					break;
				case 19:
					attackCheckTimeA = 2f;
					attackCheckTimeB = 2f;
					isAttackMoveByCore = true;
					break;
				case 20:
					attackCheckTimeA = 2f;
					attackCheckTimeB = 2f;
					isAttackMoveByCore = true;
					break;
				case 21:
					isAlarm = true;
					chaseDistance = 99999f;
					break;
				}
			}
		}
		needFreshCorePosition = true;
	}

	private void Awake()
	{
		cache();
		baseR.freezeRotation = true;
		baseR.useGravity = false;
		_ignoreLookTargetAnimations = new HashSet<string> { "sit_hunt_down", "hit_eren_L", "hit_eren_R", "idle_recovery", "eat_l", "eat_r", "sit_hit_eye", "hit_eye" };
		_fastHeadRotationAnimations = new HashSet<string> { "hit_eren_L", "hit_eren_R", "sit_hit_eye", "hit_eye" };
		foreach (AnimationState item in base.animation)
		{
			if (item.name.StartsWith("attack_"))
			{
				_ignoreLookTargetAnimations.Add(item.name);
				_fastHeadRotationAnimations.Add(item.name);
			}
		}
	}

	private void CheckAnimationLookTarget(string animation)
	{
		_ignoreLookTarget = _ignoreLookTargetAnimations.Contains(animation);
		_fastHeadRotation = _fastHeadRotationAnimations.Contains(animation);
	}

	public void beLaughAttacked()
	{
		if (!hasDie && abnormalType != AbnormalType.TYPE_CRAWLER)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters = new object[1] { 0f };
				basePV.RPC("laugh", PhotonTargets.All, parameters);
			}
			else if (state == TitanState.idle || state == TitanState.turn || state == TitanState.chase)
			{
				laugh();
			}
		}
	}

	public void beTauntedBy(GameObject target, float tauntTime)
	{
		whoHasTauntMe = target;
		this.tauntTime = tauntTime;
		isAlarm = true;
	}

	public void cache()
	{
		CacheComponnents();
		CacheTransforms();
		//baseAudioSource = baseT.Find("snd_titan_foot").GetComponent<AudioSource>();
		camT = Camera.main.transform;
		baseR = base.rigidbody;
		baseColliders = new List<Collider>();
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			if (collider.name != "AABB")
			{
				baseColliders.Add(collider);
			}
		}
		GameObject gameObject = new GameObject
		{
			name = "PlayerDetectorRC"
		};
		CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
		CapsuleCollider component = baseT.Find("AABB").GetComponent<CapsuleCollider>();
		capsuleCollider.center = component.center;
		capsuleCollider.radius = Math.Abs(baseT.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position.y - baseT.position.y);
		capsuleCollider.height = component.height * 1.2f;
		capsuleCollider.material = component.material;
		capsuleCollider.isTrigger = true;
		capsuleCollider.name = "PlayerDetectorRC";
		myTitanTrigger = gameObject.AddComponent<TitanTrigger>();
		myTitanTrigger.isCollide = false;
		gameObject.layer = 16;
		gameObject.transform.parent = baseT.Find("AABB");
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine)
		{
			baseGT = base.gameObject.transform;
		}
		if (baseT != null)
		{
			chkFrontRight = baseT.Find("chkFrontRight");
			chkFrontLeft = baseT.Find("chkFrontLeft");
			chkBackRight = baseT.Find("chkBackRight");
			chkBackLeft = baseT.Find("chkBackLeft");
			chkOverHead = baseT.Find("chkOverHead");
			chkFront = baseT.Find("chkFront");
		}
		else
		{
			chkFrontRight = base.transform.Find("chkFrontRight");
			chkFrontLeft = base.transform.Find("chkFrontLeft");
			chkBackRight = base.transform.Find("chkBackRight");
			chkBackLeft = base.transform.Find("chkBackLeft");
			chkOverHead = base.transform.Find("chkOverHead");
			chkFront = base.transform.Find("chkFront");
		}
	}

	private void chase()
	{
		state = TitanState.chase;
		isAlarm = true;
		crossFade(runAnimation, 0.5f);
	}

	private MONO checkIfHitCrawlerMouth(Transform head, float rad)
	{
		float num = rad * myLevel;
		Vector3 b = head.position - Vector3.up * 1.5f * myLevel;
		foreach (GameObject allhero in FengGameManagerMKII.allheroes)
		{
			if (!(allhero != null))
			{
				continue;
			}
			MONO component = allhero.GetComponent<MONO>();
			if (!(component != null))
			{
				continue;
			}
			if (component.species == SPECIES.Hero)
			{
				HERO hERO = component as HERO;
				if (!hERO.isInvincible())
				{
					float num2 = hERO.capsule.height * 0.5f;
					if (Vector3.Distance(hERO.transform.position + Vector3.up * num2, b) < num + num2)
					{
						return component;
					}
				}
			}
			else if (component.species == SPECIES.TitanEren)
			{
				float num3 = (component as TITAN_EREN).capsule.height * 0.5f;
				if (Vector3.Distance(component.transform.position + Vector3.up * num3, b) < num + num3)
				{
					return component;
				}
			}
		}
		return null;
	}

	private HERO checkIfHitHand(Transform hand)
	{
		float num = 2.4f * myLevel;
		Collider[] array = Physics.OverlapSphere(hand.position, num + 1f);
		foreach (Collider collider in array)
		{
			if (collider == null)
			{
				continue;
			}
			Transform transform = collider.transform;
			if (transform == null)
			{
				continue;
			}
			transform = transform.root;
			if (!(transform.tag == "Player"))
			{
				continue;
			}
			MONO component = transform.GetComponent<MONO>();
			if (!(component != null))
			{
				continue;
			}
			if (component.species == SPECIES.TitanEren)
			{
				TITAN_EREN tITAN_EREN = component as TITAN_EREN;
				if (!tITAN_EREN.isHit)
				{
					tITAN_EREN.hitByTitan();
				}
			}
			else if (component.species == SPECIES.Hero)
			{
				HERO hERO = component as HERO;
				if (!hERO.isInvincible())
				{
					return hERO;
				}
			}
		}
		return null;
	}

	private MONO checkIfHitHead(Transform head, float rad)
	{
		float num = rad * myLevel;
		Vector3 b = head.position + Vector3.up * 1.5f * myLevel;
		foreach (GameObject allhero in FengGameManagerMKII.allheroes)
		{
			if (allhero == null)
			{
				continue;
			}
			MONO component = allhero.GetComponent<MONO>();
			if (!(component != null))
			{
				continue;
			}
			if (component.species == SPECIES.Hero)
			{
				HERO hERO = component as HERO;
				if (!hERO.isInvincible())
				{
					float num2 = hERO.capsule.height * 0.5f;
					if (Vector3.Distance(hERO.transform.position + Vector3.up * num2, b) < num + num2)
					{
						return hERO;
					}
				}
			}
			else if (component.species == SPECIES.TitanEren)
			{
				float num3 = (component as TITAN_EREN).capsule.height * 0.5f;
				if (Vector3.Distance(component.transform.position + Vector3.up * num3, b) < num + num3)
				{
					return component;
				}
			}
		}
		return null;
	}

	private void crossFadeIfNotPlaying(string aniName, float time)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.offlineMode)
		{
			crossFade(aniName, time);
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !PhotonNetwork.offlineMode && !base.animation.IsPlaying(aniName))
		{
			crossFade(aniName, time);
		}
	}

	private void crossFade(string aniName, float time)
	{
		baseA.CrossFade(aniName, time);
		CheckAnimationLookTarget(aniName);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			object[] parameters = new object[2] { aniName, time };
			basePV.RPC("netCrossFade", PhotonTargets.Others, parameters);
		}
	}

	public bool die()
	{
		if (hasDie)
		{
			return false;
		}
		hasDie = true;
		FengGameManagerMKII.instance.oneTitanDown(string.Empty, onPlayerLeave: false);
		dieAnimation();
		return true;
	}

	private void dieAnimation()
	{
		if (baseA.IsPlaying("sit_idle") || baseA.IsPlaying("sit_hit_eye"))
		{
			crossFade("sit_die", 0.1f);
		}
		else if (abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			crossFade("crawler_die", 0.2f);
		}
		else if (abnormalType == AbnormalType.NORMAL)
		{
			crossFade("die_front", 0.05f);
		}
		else if ((baseA.IsPlaying("attack_abnormal_jump") && baseA["attack_abnormal_jump"].normalizedTime > 0.7f) || (baseA.IsPlaying("attack_abnormal_getup") && baseA["attack_abnormal_getup"].normalizedTime < 0.7f) || baseA.IsPlaying("tired"))
		{
			crossFade("die_ground", 0.2f);
		}
		else
		{
			crossFade("die_back", 0.05f);
		}
	}

	public void dieBlow(Vector3 attacker, float hitPauseTime)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			dieBlowFunc(attacker, hitPauseTime);
			if (GameObject.FindGameObjectsWithTag("titan").Length <= 1)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
			}
		}
		else
		{
			object[] parameters = new object[2] { attacker, hitPauseTime };
			basePV.RPC("dieBlowRPC", PhotonTargets.All, parameters);
		}
	}

	public void dieBlowFunc(Vector3 attacker, float hitPauseTime)
	{
		if (hasDie)
		{
			return;
		}
		baseT.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker - baseT.position).eulerAngles.y, 0f);
		hasDie = true;
		hitAnimation = "die_blow";
		hitPause = hitPauseTime;
		playAnimation(hitAnimation);
		baseA[hitAnimation].time = 0f;
		baseA[hitAnimation].speed = 0f;
		needFreshCorePosition = true;
		FengGameManagerMKII.instance.oneTitanDown(string.Empty, onPlayerLeave: false);
		if (basePV.isMine)
		{
			if (grabbedTarget != null)
			{
				grabbedTarget.photonView.RPC("netUngrabbed", PhotonTargets.All);
			}
			if (nonAI)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
				IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
				IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add("dead", true);
                hashtable.Add("deaths", (int)PhotonNetwork.player.customProperties["deaths"] + 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
		}
	}

	[RPC]
	private void dieBlowRPC(Vector3 attacker, float hitPauseTime)
	{
		if (basePV.isMine && (attacker - baseT.position).magnitude < 80f)
		{
			dieBlowFunc(attacker, hitPauseTime);
		}
	}

	[RPC]
	public void DieByCannon(int viewID, PhotonMessageInfo info)
	{
		if (!FengGameManagerMKII.level.StartsWith("Custom"))
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "Shot by cannons not in custom maps");
			return;
		}
		PhotonView photonView = PhotonView.Find(viewID);
		if (photonView != null)
		{
			int damage = 0;
			if (PhotonNetwork.isMasterClient)
			{
				OnTitanDie(photonView);
			}
			if (nonAI)
			{
				FengGameManagerMKII.instance.titanGetKill(photonView.owner, damage, (string)PhotonNetwork.player.customProperties["name"]);
			}
			else
			{
				FengGameManagerMKII.instance.titanGetKill(photonView.owner, damage, baseG.name);
			}
		}
		else
		{
			FengGameManagerMKII.PView.RPC("netShowDamage", photonView.owner, speed);
		}
	}

	public void dieHeadBlow(Vector3 attacker, float hitPauseTime)
	{
		if (abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			dieHeadBlowFunc(attacker, hitPauseTime);
			if (GameObject.FindGameObjectsWithTag("titan").Length <= 1)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
			}
		}
		else
		{
			object[] parameters = new object[2] { attacker, hitPauseTime };
			basePV.RPC("dieHeadBlowRPC", PhotonTargets.All, parameters);
		}
	}

	public void dieHeadBlowFunc(Vector3 attacker, float hitPauseTime)
	{
		if (hasDie)
		{
			return;
		}
		//playSound("snd_titan_head_blow");
		baseT.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker - baseT.position).eulerAngles.y, 0f);
		hasDie = true;
		hitAnimation = "die_headOff";
		hitPause = hitPauseTime;
		playAnimation(hitAnimation);
		baseA[hitAnimation].time = 0f;
		baseA[hitAnimation].speed = 0f;
		FengGameManagerMKII.instance.oneTitanDown(string.Empty, onPlayerLeave: false);
		needFreshCorePosition = true;
		Transform transform = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !basePV.isMine) ? ((Transform)UnityEngine.Object.Instantiate(CacheResources.Load("bloodExplore"), head.position + Vector3.up * 1f * myLevel, Quaternion.Euler(270f, 0f, 0f))) : PhotonNetwork.Instantiate<Transform>("bloodExplore", head.position + Vector3.up * 1f * myLevel, Quaternion.Euler(270f, 0f, 0f)));
		transform.localScale = baseT.localScale;
		transform = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !basePV.isMine) ? ((Transform)UnityEngine.Object.Instantiate(CacheResources.Load("bloodsplatter"), head.position, Quaternion.Euler(270f + neck.rotation.eulerAngles.x, neck.rotation.eulerAngles.y, neck.rotation.eulerAngles.z))) : PhotonNetwork.Instantiate<Transform>("bloodsplatter", head.position, Quaternion.Euler(270f + neck.rotation.eulerAngles.x, neck.rotation.eulerAngles.y, neck.rotation.eulerAngles.z)));
		transform.localScale = baseT.localScale;
		transform.parent = neck;
		transform = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !basePV.isMine) ? ((Transform)UnityEngine.Object.Instantiate(CacheResources.Load("FX/justSmoke"), neck.position, Quaternion.Euler(270f, 0f, 0f))) : PhotonNetwork.Instantiate<Transform>("FX/justSmoke", neck.position, Quaternion.Euler(270f, 0f, 0f)));
		transform.parent = neck;
		if (basePV.isMine)
		{
			if (grabbedTarget != null)
			{
				grabbedTarget.photonView.RPC("netUngrabbed", PhotonTargets.All);
			}
			if (nonAI)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
				IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
				IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add("dead", true);
                hashtable.Add("deaths", (int)PhotonNetwork.player.customProperties["deaths"] + 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
		}
	}

	[RPC]
	private void dieHeadBlowRPC(Vector3 attacker, float hitPauseTime)
	{
		if (basePV.isMine && (attacker - neck.position).magnitude < lagMax)
		{
			dieHeadBlowFunc(attacker, hitPauseTime);
		}
	}

	private void eat()
	{
		state = TitanState.eat;
		attacked = false;
		if (isGrabHandLeft)
		{
			attackAnimation = "eat_l";
			crossFade("eat_l", 0.1f);
		}
		else
		{
			attackAnimation = "eat_r";
			crossFade("eat_r", 0.1f);
		}
	}

	private void eatSet(HERO grabTarget)
	{
		if (!(grabTarget == null) && ((IN_GAME_MAIN_CAMERA.gametype != 0 && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !basePV.isMine)) || !grabTarget.isGrabbed))
		{
			grabToRight();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
			{
				basePV.RPC("grabToRight", PhotonTargets.Others);
				PhotonView photonView = grabTarget.photonView;
				photonView.RPC("netPlayAnimation", PhotonTargets.All, "grabbed");
				photonView.RPC("netGrabbed", PhotonTargets.All, basePV.viewID, false);
			}
			else
			{
				grabTarget.grabbed(baseG, leftHand: false);
				grabTarget.animation.Play("grabbed");
			}
		}
	}

	private void eatSetL(HERO grabTarget)
	{
		if (!(grabTarget == null) && ((IN_GAME_MAIN_CAMERA.gametype != 0 && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !basePV.isMine)) || !grabTarget.GetComponent<HERO>().isGrabbed))
		{
			grabToLeft();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
			{
				basePV.RPC("grabToLeft", PhotonTargets.Others);
				PhotonView photonView = grabTarget.photonView;
				photonView.RPC("netPlayAnimation", PhotonTargets.All, "grabbed");
				photonView.RPC("netGrabbed", PhotonTargets.All, basePV.viewID, true);
			}
			else
			{
				grabTarget.grabbed(baseG, leftHand: true);
				grabTarget.animation.Play("grabbed");
			}
		}
	}

	private bool executeAttack2(string decidedAction)
	{
		if (decidedAction != null)
		{
			if (fswitchmap5 == null)
			{
				fswitchmap5 = new Dictionary<string, int>(18)
				{
					{ "grab_ground_front_l", 0 },
					{ "grab_ground_front_r", 1 },
					{ "grab_ground_back_l", 2 },
					{ "grab_ground_back_r", 3 },
					{ "grab_head_front_l", 4 },
					{ "grab_head_front_r", 5 },
					{ "grab_head_back_l", 6 },
					{ "grab_head_back_r", 7 },
					{ "attack_abnormal_jump", 8 },
					{ "attack_combo", 9 },
					{ "attack_front_ground", 10 },
					{ "attack_kick", 11 },
					{ "attack_slap_back", 12 },
					{ "attack_slap_face", 13 },
					{ "attack_stomp", 14 },
					{ "attack_bite", 15 },
					{ "attack_bite_l", 16 },
					{ "attack_bite_r", 17 }
				};
			}
			if (fswitchmap5.TryGetValue(decidedAction, out var value))
			{
				switch (value)
				{
				case 0:
					grab("ground_front_l");
					return true;
				case 1:
					grab("ground_front_r");
					return true;
				case 2:
					grab("ground_back_l");
					return true;
				case 3:
					grab("ground_back_r");
					return true;
				case 4:
					grab("head_front_l");
					return true;
				case 5:
					grab("head_front_r");
					return true;
				case 6:
					grab("head_back_l");
					return true;
				case 7:
					grab("head_back_r");
					return true;
				case 8:
					attack2("abnormal_jump");
					return true;
				case 9:
					attack2("combo_1");
					return true;
				case 10:
					attack2("front_ground");
					return true;
				case 11:
					attack2("kick");
					return true;
				case 12:
					attack2("slap_back");
					return true;
				case 13:
					attack2("slap_face");
					return true;
				case 14:
					attack2("stomp");
					return true;
				case 15:
					attack2("bite");
					return true;
				case 16:
					attack2("bite_l");
					return true;
				case 17:
					attack2("bite_r");
					return true;
				}
			}
		}
		return false;
	}

	public void explode()
	{
		if (GameSettings.explodeMode <= 0 || !hasDie || !(dieTime >= 1f) || hasExplode)
		{
			return;
		}
		int num = 0;
		float num2 = myLevel * 10f;
		if (abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			if (dieTime >= 2f)
			{
				hasExplode = true;
				num2 = 0f;
				num = 1;
			}
		}
		else
		{
			num = 1;
			hasExplode = true;
		}
		if (num != 1)
		{
			return;
		}
		Vector3 vector = baseT.position + Vector3.up * num2;
		PhotonNetwork.Instantiate("FX/Thunder", vector, Quaternion.Euler(270f, 0f, 0f), 0);
		PhotonNetwork.Instantiate("FX/boom1", vector, Quaternion.Euler(270f, 0f, 0f), 0);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject gameObject in array)
		{
			if (Vector3.Distance(gameObject.transform.position, vector) < (float)GameSettings.explodeMode)
			{
				gameObject.GetComponent<HERO>().markDie();
				gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, "Server ");
			}
		}
	}

	private void findNearestFacingHero()
	{
		GameObject gameObject = null;
		float num = float.PositiveInfinity;
		Vector3 position = baseT.position;
		float num2 = ((abnormalType != 0) ? 180f : 100f);
		foreach (GameObject allhero in FengGameManagerMKII.allheroes)
		{
			Transform transform;
			if (!(allhero != null) || !((transform = allhero.transform) != null))
			{
				continue;
			}
			float sqrMagnitude = (transform.position - position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				Vector3 vector = transform.position - baseT.position;
				if (Mathf.Abs(0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, baseGT.rotation.eulerAngles.y - 90f)) < num2)
				{
					gameObject = allhero;
					num = sqrMagnitude;
				}
			}
		}
		if (!(gameObject != null))
		{
			return;
		}
		GameObject obj = myHero;
		myHero = gameObject;
		if (obj != myHero && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			if (myHero == null)
			{
				basePV.RPC("setMyTarget", PhotonTargets.Others, -1);
			}
			else
			{
				basePV.RPC("setMyTarget", PhotonTargets.Others, myHeroPV.viewID);
			}
		}
		tauntTime = 5f;
	}

	private void findNearestHero()
	{
		GameObject gameObject = myHero;
		myHero = getNearestHero();
		if (myHero != gameObject && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			if (myHero == null)
			{
				basePV.RPC("setMyTarget", PhotonTargets.Others, -1);
			}
			else
			{
				basePV.RPC("setMyTarget", PhotonTargets.Others, myHeroPV.viewID);
			}
		}
		oldHeadRotation = head.rotation;
	}

	private void FixedUpdate()
	{
		if ((IN_GAME_MAIN_CAMERA.isPausing && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine))
		{
			return;
		}
		baseR.AddForce(new Vector3(0f, (0f - gravity) * baseR.mass, 0f));
		if (needFreshCorePosition)
		{
			oldCorePosition = baseT.position - Core.position;
			needFreshCorePosition = false;
		}
		if (hasDie)
		{
			if (hitPause <= 0f && baseA.IsPlaying("die_headOff"))
			{
				Vector3 vector = baseT.position - Core.position - oldCorePosition;
				baseR.velocity = vector / Time.deltaTime + Vector3.up * baseR.velocity.y;
			}
			oldCorePosition = baseT.position - Core.position;
		}
		else if ((state == TitanState.attack && isAttackMoveByCore) || state == TitanState.hit)
		{
			Vector3 vector2 = baseT.position - Core.position - oldCorePosition;
			baseR.velocity = vector2 / Time.deltaTime + Vector3.up * baseR.velocity.y;
			oldCorePosition = baseT.position - Core.position;
		}
		if (hasDie)
		{
			if (hitPause > 0f)
			{
				hitPause -= Time.deltaTime;
				if (hitPause <= 0f)
				{
					baseA[hitAnimation].speed = 1f;
					hitPause = 0f;
				}
			}
			else if (baseA.IsPlaying("die_blow"))
			{
				if (baseA["die_blow"].normalizedTime < 0.55f)
				{
					baseR.velocity = -baseT.forward * 300f + Vector3.up * baseR.velocity.y;
				}
				else if (baseA["die_blow"].normalizedTime < 0.83f)
				{
					baseR.velocity = -baseT.forward * 100f + Vector3.up * baseR.velocity.y;
				}
				else
				{
					baseR.velocity = Vector3.up * baseR.velocity.y;
				}
			}
			return;
		}
		if (nonAI && !IN_GAME_MAIN_CAMERA.isPausing && (state == TitanState.idle || (state == TitanState.attack && attackAnimation == "jumper_1")))
		{
			Vector3 vector3 = Vector3.zero;
			if (controller.targetDirection != -874f)
			{
				bool flag = false;
				if (stamina < 5f)
				{
					flag = true;
				}
				else if (!(stamina >= 40f) && !baseA.IsPlaying("run_abnormal") && !baseA.IsPlaying("crawler_run"))
				{
					flag = true;
				}
				vector3 = ((!(controller.isWALKDown || flag)) ? (baseT.forward * speed * Mathf.Sqrt(myLevel)) : (baseT.forward * speed * Mathf.Sqrt(myLevel) * 0.2f));
				baseGT.rotation = Quaternion.Lerp(baseGT.rotation, Quaternion.Euler(0f, controller.targetDirection, 0f), speed * 0.15f * Time.deltaTime);
				if (state == TitanState.idle)
				{
					if (controller.isWALKDown || flag)
					{
						if (abnormalType == AbnormalType.TYPE_CRAWLER)
						{
							if (!baseA.IsPlaying("crawler_run"))
							{
								crossFade("crawler_run", 0.1f);
							}
						}
						else if (!baseA.IsPlaying("run_walk"))
						{
							crossFade("run_walk", 0.1f);
						}
					}
					else if (abnormalType == AbnormalType.TYPE_CRAWLER)
					{
						if (!baseA.IsPlaying("crawler_run"))
						{
							crossFade("crawler_run", 0.1f);
						}
						MONO mONO = checkIfHitCrawlerMouth(head, 2.2f);
						if (mONO != null)
						{
							if (chest == null)
							{
								chest = baseT.Find("Amarture/Core/Controller_Body/hip/spine/chest");
							}
							if (mONO.species == SPECIES.Hero)
							{
								HERO hERO = mONO as HERO;
								if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
								{
									hERO.die((hERO.transform.position - chest.position) * 15f * myLevel, isBite: false);
								}
								else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine && !hERO.HasDied())
								{
									hERO.markDie();
									object[] parameters = new object[5]
									{
										(hERO.transform.position - chest.position) * 15f * myLevel,
										true,
										(!nonAI) ? (-1) : basePV.viewID,
										base.name,
										true
									};
									hERO.photonView.RPC("netDie", PhotonTargets.All, parameters);
								}
							}
						}
					}
					else if (!baseA.IsPlaying("run_abnormal"))
					{
						crossFade("run_abnormal", 0.1f);
					}
				}
			}
			else if (state == TitanState.idle)
			{
				if (abnormalType == AbnormalType.TYPE_CRAWLER)
				{
					if (!baseA.IsPlaying("crawler_idle"))
					{
						crossFade("crawler_idle", 0.1f);
					}
				}
				else if (!baseA.IsPlaying("idle"))
				{
					crossFade("idle", 0.1f);
				}
				vector3 = Vector3.zero;
			}
			if (state == TitanState.idle)
			{
				Vector3 velocity = baseR.velocity;
				Vector3 force = vector3 - velocity;
				force.x = Mathf.Clamp(force.x, 0f - maxVelocityChange, maxVelocityChange);
				force.z = Mathf.Clamp(force.z, 0f - maxVelocityChange, maxVelocityChange);
				force.y = 0f;
				baseR.AddForce(force, ForceMode.VelocityChange);
			}
			else if (state == TitanState.attack && attackAnimation == "jumper_0")
			{
				Vector3 velocity2 = baseR.velocity;
				Vector3 force2 = vector3 * 0.8f - velocity2;
				force2.x = Mathf.Clamp(force2.x, 0f - maxVelocityChange, maxVelocityChange);
				force2.z = Mathf.Clamp(force2.z, 0f - maxVelocityChange, maxVelocityChange);
				force2.y = 0f;
				baseR.AddForce(force2, ForceMode.VelocityChange);
			}
		}
		if ((abnormalType == AbnormalType.TYPE_I || abnormalType == AbnormalType.TYPE_JUMPER) && !nonAI && state == TitanState.attack && attackAnimation == "jumper_0")
		{
			Vector3 vector4 = baseT.forward * speed * myLevel * 0.5f;
			Vector3 velocity3 = baseR.velocity;
			if (baseA["attack_jumper_0"].normalizedTime <= 0.28f || baseA["attack_jumper_0"].normalizedTime >= 0.8f)
			{
				vector4 = Vector3.zero;
			}
			Vector3 force3 = vector4 - velocity3;
			force3.x = Mathf.Clamp(force3.x, 0f - maxVelocityChange, maxVelocityChange);
			force3.z = Mathf.Clamp(force3.z, 0f - maxVelocityChange, maxVelocityChange);
			force3.y = 0f;
			baseR.AddForce(force3, ForceMode.VelocityChange);
		}
		if (state != TitanState.chase && state != TitanState.wander && state != TitanState.to_check_point && state != TitanState.to_pvp_pt && state != TitanState.random_run)
		{
			return;
		}
		Vector3 vector5 = baseT.forward * speed;
		Vector3 velocity4 = baseR.velocity;
		Vector3 force4 = vector5 - velocity4;
		force4.x = Mathf.Clamp(force4.x, 0f - maxVelocityChange, maxVelocityChange);
		force4.z = Mathf.Clamp(force4.z, 0f - maxVelocityChange, maxVelocityChange);
		force4.y = 0f;
		baseR.AddForce(force4, ForceMode.VelocityChange);
		if (!stuck && abnormalType != AbnormalType.TYPE_CRAWLER && !nonAI)
		{
			if (baseA.IsPlaying(runAnimation) && baseR.velocity.magnitude < speed * 0.5f)
			{
				stuck = true;
				stuckTime = 2f;
				stuckTurnAngle = (float)UnityEngine.Random.Range(0, 2) * 140f - 70f;
			}
			if (state == TitanState.chase && myHero != null && myDistance > attackDistance && myDistance < 150f)
			{
				float num = 0.05f;
				if (myDifficulty > 1)
				{
					num += 0.05f;
				}
				if (abnormalType != 0)
				{
					num += 0.1f;
				}
				if (UnityEngine.Random.Range(0f, 1f) < num)
				{
					stuck = true;
					stuckTime = 1f;
					float num2 = UnityEngine.Random.Range(20f, 50f);
					stuckTurnAngle = (float)UnityEngine.Random.Range(0, 2) * num2 * 2f - num2;
				}
			}
		}
		float num3 = 0f;
		if (state == TitanState.wander)
		{
			num3 = baseT.rotation.eulerAngles.y - 90f;
		}
		else if (state == TitanState.to_check_point || state == TitanState.to_pvp_pt || state == TitanState.random_run)
		{
			Vector3 vector6 = targetCheckPt - baseT.position;
			num3 = (0f - Mathf.Atan2(vector6.z, vector6.x)) * 57.29578f;
		}
		else
		{
			if (myHero == null)
			{
				return;
			}
			Vector3 vector7 = myHeroT.position - baseT.position;
			num3 = (0f - Mathf.Atan2(vector7.z, vector7.x)) * 57.29578f;
		}
		if (stuck)
		{
			stuckTime -= Time.deltaTime;
			if (stuckTime < 0f)
			{
				stuck = false;
			}
			if (stuckTurnAngle > 0f)
			{
				stuckTurnAngle -= Time.deltaTime * 10f;
			}
			else
			{
				stuckTurnAngle += Time.deltaTime * 10f;
			}
			num3 += stuckTurnAngle;
		}
		float num4 = 0f - Mathf.DeltaAngle(num3, baseGT.rotation.eulerAngles.y - 90f);
		if (abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			baseGT.rotation = Quaternion.Lerp(baseGT.rotation, Quaternion.Euler(0f, baseGT.rotation.eulerAngles.y + num4, 0f), speed * 0.3f * Time.deltaTime / myLevel);
		}
		else
		{
			baseGT.rotation = Quaternion.Lerp(baseGT.rotation, Quaternion.Euler(0f, baseGT.rotation.eulerAngles.y + num4, 0f), speed * 0.5f * Time.deltaTime / myLevel);
		}
	}

	private string[] GetAttackStrategy()
	{
		string[] array = null;
		int num = 0;
		if (isAlarm || myHeroT.position.y + 3f <= neck.position.y + 10f * myLevel)
		{
			if (myHeroT.position.y > neck.position.y - 3f * myLevel)
			{
				if (myDistance < attackDistance * 0.5f)
				{
					if (Vector3.Distance(myHeroT.position, chkOverHead.position) < 3.6f * myLevel)
					{
						array = ((!(between2 > 0f)) ? new string[1] { "grab_head_front_l" } : new string[1] { "grab_head_front_r" });
					}
					else if (Mathf.Abs(between2) < 90f)
					{
						if (Mathf.Abs(between2) < 30f)
						{
							if (Vector3.Distance(myHeroT.position, chkFront.position) < 2.5f * myLevel)
							{
								array = new string[3] { "attack_bite", "attack_bite", "attack_slap_face" };
							}
						}
						else if (between2 > 0f)
						{
							if (Vector3.Distance(myHeroT.position, chkFrontRight.position) < 2.5f * myLevel)
							{
								array = new string[1] { "attack_bite_r" };
							}
						}
						else if (Vector3.Distance(myHeroT.position, chkFrontLeft.position) < 2.5f * myLevel)
						{
							array = new string[1] { "attack_bite_l" };
						}
					}
					else if (between2 > 0f)
					{
						if (Vector3.Distance(myHeroT.position, chkBackRight.position) < 2.8f * myLevel)
						{
							array = new string[3] { "grab_head_back_r", "grab_head_back_r", "attack_slap_back" };
						}
					}
					else if (Vector3.Distance(myHeroT.position, chkBackLeft.position) < 2.8f * myLevel)
					{
						array = new string[3] { "grab_head_back_l", "grab_head_back_l", "attack_slap_back" };
					}
				}
				if (array != null)
				{
					return array;
				}
				if (abnormalType == AbnormalType.NORMAL || abnormalType == AbnormalType.TYPE_PUNK)
				{
					if (myDifficulty <= 0 && UnityEngine.Random.Range(0, 1000) >= 3)
					{
						return array;
					}
					if (Mathf.Abs(between2) >= 60f)
					{
						return array;
					}
					return new string[1] { "attack_combo" };
				}
				if (abnormalType != AbnormalType.TYPE_I && abnormalType != AbnormalType.TYPE_JUMPER)
				{
					return array;
				}
				if (myDifficulty > 0 || UnityEngine.Random.Range(0, 100) < 50)
				{
					return new string[1] { "attack_abnormal_jump" };
				}
				return array;
			}
			switch ((Mathf.Abs(between2) < 90f) ? ((between2 > 0f) ? 1 : 2) : ((!(between2 > 0f)) ? 3 : 4))
			{
			case 1:
				if (myDistance >= attackDistance * 0.25f)
				{
					if (myDistance < attackDistance * 0.5f)
					{
						if (abnormalType != AbnormalType.TYPE_PUNK && abnormalType == AbnormalType.NORMAL)
						{
							return new string[3] { "grab_ground_front_r", "grab_ground_front_r", "attack_stomp" };
						}
						return new string[3] { "grab_ground_front_r", "grab_ground_front_r", "attack_abnormal_jump" };
					}
					if (abnormalType != AbnormalType.TYPE_PUNK)
					{
						if (abnormalType == AbnormalType.NORMAL)
						{
							if (myDifficulty <= 0)
							{
								return new string[5] { "attack_front_ground", "attack_front_ground", "attack_front_ground", "attack_front_ground", "attack_combo" };
							}
							return new string[3] { "attack_front_ground", "attack_combo", "attack_combo" };
						}
						return new string[1] { "attack_abnormal_jump" };
					}
					return new string[3] { "attack_combo", "attack_combo", "attack_abnormal_jump" };
				}
				if (abnormalType != AbnormalType.TYPE_PUNK)
				{
					if (abnormalType != 0)
					{
						return new string[1] { "attack_kick" };
					}
					return new string[2] { "attack_front_ground", "attack_stomp" };
				}
				return new string[2] { "attack_kick", "attack_stomp" };
			case 2:
				if (myDistance >= attackDistance * 0.25f)
				{
					if (myDistance < attackDistance * 0.5f)
					{
						if (abnormalType != AbnormalType.TYPE_PUNK && abnormalType == AbnormalType.NORMAL)
						{
							return new string[3] { "grab_ground_front_l", "grab_ground_front_l", "attack_stomp" };
						}
						return new string[3] { "grab_ground_front_l", "grab_ground_front_l", "attack_abnormal_jump" };
					}
					if (abnormalType != AbnormalType.TYPE_PUNK)
					{
						if (abnormalType == AbnormalType.NORMAL)
						{
							if (myDifficulty <= 0)
							{
								return new string[5] { "attack_front_ground", "attack_front_ground", "attack_front_ground", "attack_front_ground", "attack_combo" };
							}
							return new string[3] { "attack_front_ground", "attack_combo", "attack_combo" };
						}
						return new string[1] { "attack_abnormal_jump" };
					}
					return new string[3] { "attack_combo", "attack_combo", "attack_abnormal_jump" };
				}
				if (abnormalType != AbnormalType.TYPE_PUNK)
				{
					if (abnormalType != 0)
					{
						return new string[1] { "attack_kick" };
					}
					return new string[2] { "attack_front_ground", "attack_stomp" };
				}
				return new string[2] { "attack_kick", "attack_stomp" };
			case 3:
				if (myDistance >= attackDistance * 0.5f)
				{
					return array;
				}
				_ = abnormalType;
				return new string[1] { "grab_ground_back_l" };
			case 4:
				if (myDistance >= attackDistance * 0.5f)
				{
					return array;
				}
				_ = abnormalType;
				return new string[1] { "grab_ground_back_r" };
			}
		}
		return array;
	}

	private void getDown()
	{
		state = TitanState.down;
		isAlarm = true;
		playAnimation("sit_hunt_down");
		getdownTime = UnityEngine.Random.Range(3f, 5f);
	}

	private GameObject getNearestHero()
	{
		return FengGameManagerMKII.allheroes.MinValueOrNull((GameObject hero) => Vector3.Distance(hero.transform.position, baseT.position), (GameObject hero) => hero != null);
	}

	private int getPunkNumber()
	{
		int num = 0;
		foreach (TITAN titan in FengGameManagerMKII.titans)
		{
			if (titan != null && titan.name.StripHex().StartsWith("Punk"))
			{
				num++;
			}
		}
		return num;
	}

	private void grab(string type)
	{
		state = TitanState.grab;
		attacked = false;
		isAlarm = true;
		attackAnimation = type;
		crossFade("grab_" + type, 0.1f);
		isGrabHandLeft = true;
		grabbedTarget = null;
		if (type != null)
		{
			if (f__switchmap7 == null)
			{
				f__switchmap7 = new Dictionary<string, int>(8)
				{
					{ "ground_back_l", 0 },
					{ "ground_back_r", 1 },
					{ "ground_front_l", 2 },
					{ "ground_front_r", 3 },
					{ "head_back_l", 4 },
					{ "head_back_r", 5 },
					{ "head_front_l", 6 },
					{ "head_front_r", 7 }
				};
			}
			if (f__switchmap7.TryGetValue(type, out var value))
			{
				switch (value)
				{
				case 0:
					attackCheckTimeA = 0.34f;
					attackCheckTimeB = 0.49f;
					break;
				case 1:
					attackCheckTimeA = 0.34f;
					attackCheckTimeB = 0.49f;
					isGrabHandLeft = false;
					break;
				case 2:
					attackCheckTimeA = 0.37f;
					attackCheckTimeB = 0.6f;
					break;
				case 3:
					attackCheckTimeA = 0.37f;
					attackCheckTimeB = 0.6f;
					isGrabHandLeft = false;
					break;
				case 4:
					attackCheckTimeA = 0.45f;
					attackCheckTimeB = 0.5f;
					isGrabHandLeft = false;
					break;
				case 5:
					attackCheckTimeA = 0.45f;
					attackCheckTimeB = 0.5f;
					break;
				case 6:
					attackCheckTimeA = 0.38f;
					attackCheckTimeB = 0.55f;
					break;
				case 7:
					attackCheckTimeA = 0.38f;
					attackCheckTimeB = 0.55f;
					isGrabHandLeft = false;
					break;
				}
			}
		}
		if (isGrabHandLeft)
		{
			currentGrabHand = hand_L_001SphereT;
		}
		else
		{
			currentGrabHand = hand_R_001SphereT;
		}
	}

	[RPC]
	public void grabbedTargetEscape(PhotonMessageInfo info)
	{
		if (info.sender != grabbedTarget.photonView.owner && PhotonNetwork.isMasterClient)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "false target escape");
		}
		else
		{
			grabbedTarget = null;
		}
	}

	public void grabbedTargetEscape()
	{
		grabbedTarget = null;
	}

	[RPC]
	public void grabToLeft(PhotonMessageInfo info)
	{
		if (PhotonNetwork.isMasterClient && info.sender != base.networkView.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "false grab");
		}
		else
		{
			grabToLeft();
		}
	}

	public void grabToLeft()
	{
		Transform transform = grabTF.transform;
		transform.parent = hand_L_001;
		transform.position = hand_L_001SphereT.position;
		transform.rotation = hand_L_001SphereT.rotation;
		transform.localPosition -= Vector3.right * hand_L_001Sphere.radius * 0.3f;
		transform.localPosition -= Vector3.up * hand_L_001Sphere.radius * 0.51f;
		transform.localPosition -= Vector3.forward * hand_L_001Sphere.radius * 0.3f;
		transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + 180f, transform.localRotation.eulerAngles.z + 180f);
	}

	[RPC]
	public void grabToRight(PhotonMessageInfo info)
	{
		if (PhotonNetwork.isMasterClient && info.sender != base.networkView.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "false grab");
		}
		else
		{
			grabToRight();
		}
	}

	public void grabToRight()
	{
		Transform transform = grabTF.transform;
		transform.parent = hand_R_001;
		transform.position = hand_R_001SphereT.position;
		transform.rotation = hand_R_001SphereT.rotation;
		transform.localPosition -= Vector3.right * hand_R_001Sphere.radius * 0.3f;
		transform.localPosition += Vector3.up * hand_R_001Sphere.radius * 0.51f;
		transform.localPosition -= Vector3.forward * hand_R_001Sphere.radius * 0.3f;
		transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + 180f, transform.localRotation.eulerAngles.z);
	}

	public void headMovement2()
	{
		if (!hasDie)
		{
			targetHeadRotation = head.rotation;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.offlineMode)
			{
				if (abnormalType != AbnormalType.TYPE_CRAWLER && state != TitanState.attack && state != TitanState.down && state != TitanState.hit && state != TitanState.recover && state != TitanState.hit_eye && !hasDie && myDistance < 100f && myHero != null)
				{
					Vector3 vector = myHero.transform.position - baseT.position;
					angle = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
					float value = 0f - Mathf.DeltaAngle(angle, baseT.rotation.eulerAngles.y - 90f);
					value = Mathf.Clamp(value, -40f, 40f);
					float value2 = Mathf.Atan2(neck.position.y + myLevel * 2f - myHero.transform.position.y, myDistance) * 57.29578f;
					value2 = Mathf.Clamp(value2, -40f, 30f);
					targetHeadRotation = Quaternion.Euler(head.rotation.eulerAngles.x + value2, head.rotation.eulerAngles.y + value, head.rotation.eulerAngles.z);
				}
				if (state == TitanState.attack || state == TitanState.hit || state == TitanState.hit_eye)
				{
					oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 20f);
				}
				else
				{
					oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 10f);
				}
			}
			else
			{
				if (!_ignoreLookTarget && abnormalType != AbnormalType.TYPE_CRAWLER && !hasDie && myDistance < 100f && myHero != null)
				{
					Vector3 vector2 = myHero.transform.position - baseT.position;
					angle = (0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f;
					float value3 = 0f - Mathf.DeltaAngle(angle, baseT.rotation.eulerAngles.y - 90f);
					value3 = Mathf.Clamp(value3, -40f, 40f);
					float value4 = Mathf.Atan2(neck.position.y + myLevel * 2f - myHero.transform.position.y, myDistance) * 57.29578f;
					value4 = Mathf.Clamp(value4, -40f, 30f);
					targetHeadRotation = Quaternion.Euler(head.rotation.eulerAngles.x + value4, head.rotation.eulerAngles.y + value3, head.rotation.eulerAngles.z);
				}
				if (_fastHeadRotation)
				{
					oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 20f);
				}
				else
				{
					oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 10f);
				}
			}
			head.rotation = oldHeadRotation;
		}
		if (!baseA.IsPlaying("die_headOff"))
		{
			head.localScale = headscale;
		}
	}

	private void hit(string animationName, Vector3 attacker, float hitPauseTime)
	{
		state = TitanState.hit;
		hitAnimation = animationName;
		hitPause = hitPauseTime;
		playAnimation(hitAnimation);
		baseA[hitAnimation].time = 0f;
		baseA[hitAnimation].speed = 0f;
		baseT.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker - baseT.position).eulerAngles.y, 0f);
		needFreshCorePosition = true;
		if (basePV.isMine && grabbedTarget != null)
		{
			grabbedTarget.photonView.RPC("netUngrabbed", PhotonTargets.All);
		}
	}

	public void hitAnkle()
	{
		if (!hasDie && state != TitanState.down)
		{
			if (grabbedTarget != null)
			{
				grabbedTarget.photonView.RPC("netUngrabbed", PhotonTargets.All);
			}
			getDown();
		}
	}

	[RPC]
	public void hitAnkleRPC(int viewID)
	{
		if (hasDie || state == TitanState.down)
		{
			return;
		}
		PhotonView photonView = PhotonView.Find(viewID);
		if (photonView != null && (photonView.gameObject.transform.position - baseT.position).magnitude < 20f)
		{
			if (basePV.isMine && grabbedTarget != null)
			{
				grabbedTarget.photonView.RPC("netUngrabbed", PhotonTargets.All);
			}
			getDown();
		}
	}

	public void hitEye()
	{
		if (!hasDie)
		{
			justHitEye();
		}
	}

	[RPC]
	public void hitEyeRPC(int viewID)
	{
		if (!hasDie && (PhotonView.Find(viewID).gameObject.transform.position - neck.position).magnitude < 20f)
		{
			if (basePV.isMine && grabbedTarget != null)
			{
				grabbedTarget.photonView.RPC("netUngrabbed", PhotonTargets.All);
			}
			if (!hasDie)
			{
				justHitEye();
			}
		}
	}

	public void hitL(Vector3 attacker, float hitPauseTime)
	{
		if (abnormalType != AbnormalType.TYPE_CRAWLER)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				hit("hit_eren_L", attacker, hitPauseTime);
				return;
			}
			object[] parameters = new object[2] { attacker, hitPauseTime };
			basePV.RPC("hitLRPC", PhotonTargets.All, parameters);
		}
	}

	[RPC]
	private void hitLRPC(Vector3 attacker, float hitPauseTime)
	{
		if (basePV.isMine && (attacker - baseT.position).magnitude < 80f)
		{
			hit("hit_eren_L", attacker, hitPauseTime);
		}
	}

	public void hitR(Vector3 attacker, float hitPauseTime)
	{
		if (abnormalType != AbnormalType.TYPE_CRAWLER)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				hit("hit_eren_R", attacker, hitPauseTime);
				return;
			}
			object[] parameters = new object[2] { attacker, hitPauseTime };
			basePV.RPC("hitRRPC", PhotonTargets.All, parameters);
		}
	}

	[RPC]
	private void hitRRPC(Vector3 attacker, float hitPauseTime)
	{
		if (basePV.isMine && !hasDie && (attacker - baseT.position).magnitude < 80f)
		{
			hit("hit_eren_R", attacker, hitPauseTime);
		}
	}

	private void idle(float sbtime = 0f)
	{
		stuck = false;
		this.sbtime = sbtime;
		if (myDifficulty == 2 && (abnormalType == AbnormalType.TYPE_JUMPER || abnormalType == AbnormalType.TYPE_I))
		{
			this.sbtime = UnityEngine.Random.Range(0f, 1.5f);
		}
		else if (myDifficulty >= 1)
		{
			this.sbtime = 0f;
		}
		this.sbtime = Mathf.Max(0.5f, this.sbtime);
		if (abnormalType == AbnormalType.TYPE_PUNK)
		{
			this.sbtime = 0.1f;
			if (myDifficulty == 1)
			{
				this.sbtime += 0.4f;
			}
		}
		state = TitanState.idle;
		if (abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			crossFadeIfNotPlaying("crawler_idle", 0.2f);
		}
		else
		{
			crossFadeIfNotPlaying("idle", 0.2f);
		}
	}

	public bool IsGrounded()
	{
		return Physics.Raycast(baseGT.position + Vector3.up * 0.1f, -Vector3.up, 0.3f, Layer.GroundAABB.value);
	}

	private void justEatHero(HERO target, Transform hand)
	{
		if (!(target != null))
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			if (!target.HasDied())
			{
				target.markDie();
				if (nonAI)
				{
					target.photonView.RPC("netDie2", PhotonTargets.All, basePV.viewID, baseG.name);
				}
				else
				{
					target.photonView.RPC("netDie2", PhotonTargets.All, -1, baseG.name);
				}
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			target.GetComponent<HERO>().die2(hand);
		}
	}

	private void justHitEye()
	{
		if (state != TitanState.hit_eye)
		{
			if (state == TitanState.down || state == TitanState.sit)
			{
				playAnimation("sit_hit_eye");
			}
			else
			{
				playAnimation("hit_eye");
			}
			state = TitanState.hit_eye;
		}
	}

	[RPC]
	public void labelRPC(int health, int maxHealth)
	{
		if (health < 0)
		{
			if (healthLabel != null)
			{
				UnityEngine.Object.Destroy(healthLabel);
			}
			return;
		}
		if (healthLabel == null)
		{
			healthLabel = (UILabel)UnityEngine.Object.Instantiate(CacheResources.Load<UILabel>("UI/LabelNameOverHead"));
			healthLabelT = healthLabel.transform;
			healthLabelT.LookAt(-new Vector3(baseT.forward.x, healthLabelT.position.y, baseT.forward.z));
			healthLabel.name = "LabelNameOverHead";
			healthLabelT.parent = baseT;
			healthLabelT.localPosition = new Vector3(0f, 20f + 1f / myLevel, 0f);
			if (abnormalType == AbnormalType.TYPE_CRAWLER)
			{
				healthLabelT.localPosition = new Vector3(0f, 10f + 1f / myLevel, 0f);
			}
			float num = 1f;
			if (myLevel < 1f)
			{
				num = 1f / myLevel;
			}
			healthLabelT.localScale = new Vector3(num, num, num);
			healthLabelEnabled = true;
		}
		if (healthLabelEnabled)
		{
			string text = "[7FFF00]";
			float num2 = (float)health / (float)maxHealth;
			if (num2 < 0.75f && num2 >= 0.5f)
			{
				text = "[f2b50f]";
			}
			else if (num2 < 0.5f && num2 >= 0.25f)
			{
				text = "[ff8100]";
			}
			else if (num2 < 0.25f)
			{
				text = "[ff3333]";
			}
			healthLabel.text = text + health;
		}
	}

	public void lateUpdate()
	{
		if (IN_GAME_MAIN_CAMERA.isPausing && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return;
		}
		if (baseA.IsPlaying("run_walk"))
		{
			if (baseA["run_walk"].normalizedTime % 1f > 0.1f && baseA["run_walk"].normalizedTime % 1f < 0.6f && stepSoundPhase == 2)
			{
				stepSoundPhase = 1;
				//baseAudioSource.Stop();
				//baseAudioSource.Play();
			}
			else if (baseA["run_walk"].normalizedTime % 1f > 0.6f && stepSoundPhase == 1)
			{
				stepSoundPhase = 2;
				//baseAudioSource.Stop();
				//baseAudioSource.Play();
			}
		}
		else if (baseA.IsPlaying("crawler_run"))
		{
			if (baseA["crawler_run"].normalizedTime % 1f > 0.1f && baseA["crawler_run"].normalizedTime % 1f < 0.56f && stepSoundPhase == 2)
			{
				stepSoundPhase = 1;
				//baseAudioSource.Stop();
				//baseAudioSource.Play();
			}
			else if (baseA["crawler_run"].normalizedTime % 1f > 0.56f && stepSoundPhase == 1)
			{
				stepSoundPhase = 2;
				//baseAudioSource.Stop();
				//baseAudioSource.Play();
			}
		}
		else if (baseA.IsPlaying("run_abnormal"))
		{
			if (baseA["run_abnormal"].normalizedTime % 1f > 0.47f && baseA["run_abnormal"].normalizedTime % 1f < 0.95f && stepSoundPhase == 2)
			{
				stepSoundPhase = 1;
				//baseAudioSource.Stop();
				//baseAudioSource.Play();
			}
			else if ((baseA["run_abnormal"].normalizedTime % 1f > 0.95f || baseA["run_abnormal"].normalizedTime % 1f < 0.47f) && stepSoundPhase == 1)
			{
				stepSoundPhase = 2;
				//baseAudioSource.Stop();
				//baseAudioSource.Play();
			}
		}
		headMovement2();
		grounded = false;
		updateLabel();
		updateCollider();
	}

	[RPC]
	private void laugh(float sbtime = 0f)
	{
		if (state == TitanState.idle || state == TitanState.turn || state == TitanState.chase)
		{
			this.sbtime = sbtime;
			state = TitanState.laugh;
			crossFade("laugh", 0.2f);
		}
	}

	public void loadskin()
	{
		skin = 86;
		eye = false;
		if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine) && (int)FengGameManagerMKII.settings[1] > 0)
		{
			int num = (int)UnityEngine.Random.Range(86f, 90f);
			int num2 = num - 60;
			if ((int)FengGameManagerMKII.settings[32] == 1)
			{
				num2 = UnityEngine.Random.Range(26, 30);
			}
			string text = (string)FengGameManagerMKII.settings[num];
			string text2 = (string)FengGameManagerMKII.settings[num2];
			skin = num;
			if (text2.EndsWith(".jpg") || text2.EndsWith(".png") || text2.EndsWith(".jpeg"))
			{
				eye = true;
			}
			GetComponent<TITAN_SETUP>().setVar(skin, eye);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				StartCoroutine(loadskinE(text, text2));
				return;
			}
			if ((int)FengGameManagerMKII.settings[1] == 2)
			{
				StartCoroutine(loadskinE("https://i.imgur.com/nmpD1oo.gif", "https://i.imgur.com/nmpD1oo.gif"));
				return;
			}
			basePV.RPC("loadskinRPC", PhotonTargets.AllBuffered, text, text2);
		}
	}

	public IEnumerator loadskinE(string body, string eye)
	{
		while (!hasSpawn)
		{
			yield return null;
		}
		bool mipmap = true;
		bool iteratorVariable1 = false;
		if ((int)FengGameManagerMKII.settings[63] == 1)
		{
			mipmap = false;
		}
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer iteratorVariable2 in componentsInChildren)
		{
			if (iteratorVariable2.name.Contains("eye"))
			{
				if (eye.ToLower() == "transparent")
				{
					iteratorVariable2.enabled = false;
				}
				else
				{
					if ((!eye.EndsWith(".jpg") && !eye.EndsWith(".png") && !eye.EndsWith(".jpeg")) || !RCextensions.CheckIP(eye))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(eye))
					{
						WWW link2 = new WWW(eye);
						yield return link2;
						Texture2D mainTexture = RCextensions.loadimage(link2, mipmap, 200000);
						link2.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(eye))
						{
							iteratorVariable1 = true;
							iteratorVariable2.material.mainTextureScale = new Vector2(iteratorVariable2.material.mainTextureScale.x * 4f, iteratorVariable2.material.mainTextureScale.y * 8f);
							iteratorVariable2.material.mainTextureOffset = new Vector2(0f, 0f);
							iteratorVariable2.material.mainTexture = mainTexture;
							FengGameManagerMKII.linkHash[0].Add(eye, iteratorVariable2.material);
							iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[0][eye];
						}
						else
						{
							iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[0][eye];
						}
					}
					else
					{
						iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[0][eye];
					}
				}
			}
			else
			{
				if (!(iteratorVariable2.name == "hair") || (!body.EndsWith(".jpg") && !body.EndsWith(".png") && !body.EndsWith(".jpeg")) || !RCextensions.CheckIP(body))
				{
					continue;
				}
				if (!FengGameManagerMKII.linkHash[2].ContainsKey(body))
				{
					WWW link2 = new WWW(body);
					yield return link2;
					Texture2D mainTexture2 = RCextensions.loadimage(link2, mipmap, 1000000);
					link2.Dispose();
					if (!FengGameManagerMKII.linkHash[2].ContainsKey(body))
					{
						iteratorVariable1 = true;
						iteratorVariable2.material = mainMaterial.GetComponent<SkinnedMeshRenderer>().material;
						iteratorVariable2.material.mainTexture = mainTexture2;
						FengGameManagerMKII.linkHash[2].Add(body, iteratorVariable2.material);
						iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[2][body];
					}
					else
					{
						iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[2][body];
					}
				}
				else
				{
					iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[2][body];
				}
			}
		}
		if (iteratorVariable1)
		{
			FengGameManagerMKII.instance.unloadAssets();
		}
	}

	[RPC]
	public void loadskinRPC(string body, string eye)
	{
		if ((int)FengGameManagerMKII.settings[1] == 1)
		{
			StartCoroutine(loadskinE(body, eye));
		}
	}

	private bool longRangeAttackCheck2()
	{
		if (myHero == null || myHeroT == null || myHeroR == null)
		{
			return false;
		}
		if (abnormalType == AbnormalType.TYPE_PUNK && myHero != null)
		{
			Vector3 vector = myHero.rigidbody.velocity * Time.deltaTime * 30f;
			if (vector.sqrMagnitude > 10f)
			{
				if (simpleHitTestLineAndBall(vector, chkAeLeft.position - myHeroT.position, 5f * myLevel))
				{
					attack2("anti_AE_l");
					return true;
				}
				if (simpleHitTestLineAndBall(vector, chkAeLLeft.position - myHeroT.position, 5f * myLevel))
				{
					attack2("anti_AE_low_l");
					return true;
				}
				if (simpleHitTestLineAndBall(vector, chkAeRight.position - myHeroT.position, 5f * myLevel))
				{
					attack2("anti_AE_r");
					return true;
				}
				if (simpleHitTestLineAndBall(vector, chkAeLRight.position - myHeroT.position, 5f * myLevel))
				{
					attack2("anti_AE_low_r");
					return true;
				}
			}
			Vector3 vector2 = myHeroT.position - baseT.position;
			float f = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f, baseGT.rotation.eulerAngles.y - 90f);
			if (rockInterval > 0f)
			{
				rockInterval -= Time.deltaTime;
			}
			else if (Mathf.Abs(f) < 5f)
			{
				float sqrMagnitude = (myHeroT.position + vector - baseT.position).sqrMagnitude;
				if (sqrMagnitude > 8000f && sqrMagnitude < 90000f && GameSettings.disableRock == 0)
				{
					attack2("throw");
					rockInterval = 2f;
					return true;
				}
			}
		}
		return false;
	}

	public void moveTo(float posX, float posY, float posZ)
	{
		baseT.position = new Vector3(posX, posY, posZ);
	}

	[RPC]
	public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			baseT.position = new Vector3(posX, posY, posZ);
		}
	}

	[RPC]
	private void netCrossFade(string aniName, float time)
	{
		baseA.CrossFade(aniName, time);
		CheckAnimationLookTarget(aniName);
	}

	[RPC]
	private void netDie()
	{
		asClientLookTarget = false;
		if (!hasDie)
		{
			hasDie = true;
			if (nonAI)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
				IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
				IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add("dead", true);
                hashtable.Add("deaths", (int)PhotonNetwork.player.customProperties["deaths"] + 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
			dieAnimation();
		}
	}

	[RPC]
	private void netPlayAnimation(string aniName)
	{
		baseA.Play(aniName);
		CheckAnimationLookTarget(aniName);
	}

	[RPC]
	private void netPlayAnimationAt(string aniName, float normalizedTime)
	{
		baseA.Play(aniName);
		CheckAnimationLookTarget(aniName);
		baseA[aniName].normalizedTime = normalizedTime;
	}

	[RPC]
	private void netSetAbnormalType(int type)
	{
		if (!hasload)
		{
			hasload = true;
			loadskin();
		}
		switch (type)
		{
		case 0:
			abnormalType = AbnormalType.NORMAL;
			base.name = (string)FengGameManagerMKII.settings[331];
			runAnimation = "run_walk";
			GetComponent<TITAN_SETUP>().setHair2();
			break;
		case 1:
			abnormalType = AbnormalType.TYPE_I;
			base.name = (string)FengGameManagerMKII.settings[332];
			runAnimation = "run_abnormal";
			GetComponent<TITAN_SETUP>().setHair2();
			break;
		case 2:
			abnormalType = AbnormalType.TYPE_JUMPER;
			base.name = (string)FengGameManagerMKII.settings[333];
			runAnimation = "run_abnormal";
			GetComponent<TITAN_SETUP>().setHair2();
			break;
		case 3:
			abnormalType = AbnormalType.TYPE_CRAWLER;
			base.name = (string)FengGameManagerMKII.settings[334];
			runAnimation = "crawler_run";
			GetComponent<TITAN_SETUP>().setHair2();
			break;
		case 4:
			abnormalType = AbnormalType.TYPE_PUNK;
			base.name = (string)FengGameManagerMKII.settings[335];
			runAnimation = "run_abnormal_1";
			GetComponent<TITAN_SETUP>().setHair2();
			break;
		}
		if (abnormalType == AbnormalType.TYPE_I || abnormalType == AbnormalType.TYPE_JUMPER || abnormalType == AbnormalType.TYPE_PUNK)
		{
			speed = 18f;
			if (myLevel > 1f)
			{
				speed *= Mathf.Sqrt(myLevel);
			}
			if (myDifficulty == 1)
			{
				speed *= 1.4f;
			}
			if (myDifficulty == 2)
			{
				speed *= 1.6f;
			}
			baseA["turnaround1"].speed = 2f;
			baseA["turnaround2"].speed = 2f;
		}
		if (abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			chaseDistance += 50f;
			speed = 25f;
			if (myLevel > 1f)
			{
				speed *= Mathf.Sqrt(myLevel);
			}
			if (myDifficulty == 1)
			{
				speed *= 2f;
			}
			if (myDifficulty == 2)
			{
				speed *= 2.2f;
			}
			CapsuleCollider component = (AABB = baseT.Find("AABB")).GetComponent<CapsuleCollider>();
			component.height = 10f;
			component.radius = 5f;
			component.center = new Vector3(0f, 5.05f, 0f);
		}
		if (nonAI)
		{
			if (abnormalType == AbnormalType.TYPE_CRAWLER)
			{
				speed = Mathf.Min(70f, speed);
			}
			else
			{
				speed = Mathf.Min(60f, speed);
			}
			baseA["attack_jumper_0"].speed = 7f;
			baseA["attack_crawler_jump_0"].speed = 4f;
		}
		baseA["attack_combo_1"].speed = 1f;
		baseA["attack_combo_2"].speed = 1f;
		baseA["attack_combo_3"].speed = 1f;
		baseA["attack_quick_turn_l"].speed = 1f;
		baseA["attack_quick_turn_r"].speed = 1f;
		baseA["attack_anti_AE_l"].speed = 1.1f;
		baseA["attack_anti_AE_low_l"].speed = 1.1f;
		baseA["attack_anti_AE_r"].speed = 1.1f;
		baseA["attack_anti_AE_low_r"].speed = 1.1f;
		idle();
	}

	[RPC]
	private void netSetLevel(float level, int AI, int skinColor, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient && !info.sender.isLocal)
		{
			if (basePV.owner != info.sender)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "messing w/ titans");
				return;
			}
			if (GameSettings.bombMode > 0 && PhotonNetwork.isMasterClient)
			{
				FengGameManagerMKII.instance.kickPlayerRC(info.sender, ban: true, "Titan spawn in bomb");
				return;
			}
		}
		setLevel2(level, AI, skinColor);
		if (level > 5f)
		{
			headscale = new Vector3(1f, 1f, 1f);
		}
		else if (level < 1f && FengGameManagerMKII.level.StartsWith("Custom"))
		{
			myTitanTrigger.GetComponent<CapsuleCollider>().radius *= 2.5f - level;
		}
	}

	private void OnCollisionStay()
	{
		grounded = true;
	}

	private void OnDestroy()
	{
		if (FengGameManagerMKII.instance != null)
		{
			FengGameManagerMKII.instance.removeTitan(this, base.gameObject);
		}
	}

	public void OnTitanDie(PhotonView view)
	{
		if (FengGameManagerMKII.logicLoaded && FengGameManagerMKII.RCEvents.ContainsKey("OnTitanDie"))
		{
			RCEvent obj = (RCEvent)FengGameManagerMKII.RCEvents["OnTitanDie"];
			string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnTitanDie"];
			if (FengGameManagerMKII.titanVariables.ContainsKey(array[0]))
			{
				FengGameManagerMKII.titanVariables[array[0]] = this;
			}
			else
			{
				FengGameManagerMKII.titanVariables.Add(array[0], this);
			}
			if (FengGameManagerMKII.playerVariables.ContainsKey(array[1]))
			{
				FengGameManagerMKII.playerVariables[array[1]] = view.owner;
			}
			else
			{
				FengGameManagerMKII.playerVariables.Add(array[1], view.owner);
			}
			obj.checkEvent();
		}
	}

	private void playAnimation(string aniName)
	{
		baseA.Play(aniName);
		CheckAnimationLookTarget(aniName);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			object[] parameters = new object[1] { aniName };
			basePV.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
		}
	}

	private void playAnimationAt(string aniName, float normalizedTime)
	{
		baseA.Play(aniName);
		CheckAnimationLookTarget(aniName);
		baseA[aniName].normalizedTime = normalizedTime;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			object[] parameters = new object[2] { aniName, normalizedTime };
			basePV.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
		}
	}

	public void UpdateHeroDistance()
	{
		if ((!IN_GAME_MAIN_CAMERA.isPausing || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && myDifficulty >= 0 && !nonAI)
		{
			if (myHero == null)
			{
				myDistance = float.MaxValue;
				return;
			}
			Vector2 a = new Vector2(myHero.transform.position.x, myHero.transform.position.z);
			Vector2 b = new Vector2(baseT.position.x, baseT.position.z);
			myDistance = Vector2.Distance(a, b);
		}
	}

	private void playSound(string sndname)
	{
		//playsoundRPC(sndname);
		if (basePV.isMine)
		{
			object[] parameters = new object[1] { sndname };
			//basePV.RPC("playsoundRPC", PhotonTargets.Others, parameters);
		}
	}

	[RPC]
	private void playsoundRPC(string sndname)
	{
		//baseT.Find(sndname).GetComponent<AudioSource>().Play();
	}

	public void pt()
	{
		if (controller.bite)
		{
			attack2("bite");
		}
		if (controller.bitel)
		{
			attack2("bite_l");
		}
		if (controller.biter)
		{
			attack2("bite_r");
		}
		if (controller.chopl)
		{
			attack2("anti_AE_low_l");
		}
		if (controller.chopr)
		{
			attack2("anti_AE_low_r");
		}
		if (controller.choptl)
		{
			attack2("anti_AE_l");
		}
		if (controller.choptr)
		{
			attack2("anti_AE_r");
		}
		if (controller.cover && stamina > 75f)
		{
			recoverpt();
			stamina -= 75f;
		}
		if (controller.grabbackl)
		{
			grab("ground_back_l");
		}
		if (controller.grabbackr)
		{
			grab("ground_back_r");
		}
		if (controller.grabfrontl)
		{
			grab("ground_front_l");
		}
		if (controller.grabfrontr)
		{
			grab("ground_front_r");
		}
		if (controller.grabnapel)
		{
			grab("head_back_l");
		}
		if (controller.grabnaper)
		{
			grab("head_back_r");
		}
	}

	public void randomRun(Vector3 targetPt, float r)
	{
		state = TitanState.random_run;
		targetCheckPt = targetPt;
		targetR = r;
		random_run_time = UnityEngine.Random.Range(1f, 2f);
		crossFade(runAnimation, 0.5f);
	}

	private void recover()
	{
		state = TitanState.recover;
		playAnimation("idle_recovery");
		getdownTime = UnityEngine.Random.Range(2f, 5f);
	}

	private void recoverpt()
	{
		state = TitanState.recover;
		playAnimation("idle_recovery");
		getdownTime = UnityEngine.Random.Range(1.8f, 2.5f);
	}

	public IEnumerator reloadSky()
	{
		yield return new WaitForSeconds(0.5f);
		if (FengGameManagerMKII.skyMaterial != null && Camera.main.GetComponent<Skybox>().material != FengGameManagerMKII.skyMaterial)
		{
			Camera.main.GetComponent<Skybox>().material = FengGameManagerMKII.skyMaterial;
		}
	}

	private void remainSitdown()
	{
		state = TitanState.sit;
		playAnimation("sit_idle");
		getdownTime = UnityEngine.Random.Range(10f, 30f);
	}

	public void resetLevel(float level)
	{
		myLevel = level;
		setmyLevel();
	}

	public void setAbnormalType2(AbnormalType type, bool forceCrawler)
	{
		bool flag = false;
		if (GameSettings.spawnMode > 0 || ((int)FengGameManagerMKII.settings[91] == 1 && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient))
		{
			flag = true;
		}
		if (FengGameManagerMKII.level.StartsWith("Custom"))
		{
			flag = true;
		}
		int num = 0;
		float num2 = 0.02f * (float)(IN_GAME_MAIN_CAMERA.difficulty + 1);
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			num2 = 100f;
		}
		switch (type)
		{
		case AbnormalType.NORMAL:
			num = ((UnityEngine.Random.Range(0f, 1f) < num2) ? 4 : 0);
			if (flag)
			{
				num = 0;
			}
			break;
		case AbnormalType.TYPE_I:
			num = ((!(UnityEngine.Random.Range(0f, 1f) < num2)) ? 1 : 4);
			if (flag)
			{
				num = 1;
			}
			break;
		case AbnormalType.TYPE_JUMPER:
			num = ((!(UnityEngine.Random.Range(0f, 1f) < num2)) ? 2 : 4);
			if (flag)
			{
				num = 2;
			}
			break;
		case AbnormalType.TYPE_CRAWLER:
			num = 3;
			if (CacheGameObject.Find("Crawler") != null && UnityEngine.Random.Range(0, 1000) > 5)
			{
				num = 2;
			}
			if (flag)
			{
				num = 3;
			}
			break;
		case AbnormalType.TYPE_PUNK:
			num = 4;
			break;
		}
		if (forceCrawler)
		{
			num = 3;
		}
		if (num == 4)
		{
			if (!LevelInfo.getInfo(FengGameManagerMKII.level).punk)
			{
				num = 1;
			}
			else
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && getPunkNumber() >= 3)
				{
					num = 1;
				}
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					int wave = FengGameManagerMKII.instance.wave;
					if (wave != 5 && wave != 10 && wave != 15 && wave != 20)
					{
						num = 1;
					}
				}
			}
			if (flag)
			{
				num = 4;
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
		{
			object[] parameters = new object[1] { num };
			basePV.RPC("netSetAbnormalType", PhotonTargets.AllBuffered, parameters);
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			netSetAbnormalType(num);
		}
	}

	[RPC]
	private void setIfLookTarget(bool bo)
	{
		asClientLookTarget = bo;
	}

	private void setLevel2(float level, int AI, int skinColor)
	{
		myLevel = level;
		myLevel = Mathf.Clamp(myLevel, 0.1f, 50f);
		attackWait += UnityEngine.Random.Range(0f, 2f);
		chaseDistance += myLevel * 10f;
		baseT.localScale = new Vector3(myLevel, myLevel, myLevel);
		float num = Mathf.Min(Mathf.Pow(2f / myLevel, 0.35f), 1.25f);
		headscale = new Vector3(num, num, num);
		head = baseT.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
		head.localScale = headscale;
		CacheTransforms();
		if (skinColor != 0)
		{
			mainMaterial.GetComponent<SkinnedMeshRenderer>().material.color = skinColor switch
			{
				1 => FengColor.titanSkin1, 
				2 => FengColor.titanSkin2, 
				_ => FengColor.titanSkin3, 
			};
		}
		float value = 1.4f - (myLevel - 0.7f) * 0.15f;
		value = Mathf.Clamp(value, 0.9f, 1.5f);
		foreach (AnimationState item in baseA)
		{
			item.speed = value;
		}
		baseR.mass *= myLevel;
		baseR.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360), 0f);
		if (myLevel > 1f)
		{
			speed *= Mathf.Sqrt(myLevel);
		}
		myDifficulty = AI;
		if (myDifficulty == 1 || myDifficulty == 2)
		{
			foreach (AnimationState item2 in baseA)
			{
				item2.speed = value * 1.05f;
			}
			if (nonAI)
			{
				speed *= 1.1f;
			}
			else
			{
				speed *= 1.4f;
			}
			chaseDistance *= 1.15f;
		}
		if (myDifficulty == 2)
		{
			foreach (AnimationState item3 in baseA)
			{
				item3.speed = value * 1.05f;
			}
			if (nonAI)
			{
				speed *= 1.1f;
			}
			else
			{
				speed *= 1.5f;
			}
			chaseDistance *= 1.3f;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
		{
			chaseDistance = 999999f;
		}
		if (nonAI)
		{
			if (abnormalType == AbnormalType.TYPE_CRAWLER)
			{
				speed = Mathf.Min(70f, speed);
			}
			else
			{
				speed = Mathf.Min(60f, speed);
			}
		}
		attackDistance = Vector3.Distance(baseT.position, ap_front_ground.position) * 1.65f;
	}

	private void setmyLevel()
	{
		baseA.cullingType = AnimationCullingType.BasedOnRenderers;
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
		{
			object[] parameters = new object[3]
			{
				myLevel,
				FengGameManagerMKII.instance.difficulty,
				UnityEngine.Random.Range(0, 4)
			};
			basePV.RPC("netSetLevel", PhotonTargets.AllBuffered, parameters);
			baseA.cullingType = AnimationCullingType.AlwaysAnimate;
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			setLevel2(myLevel, IN_GAME_MAIN_CAMERA.difficulty, UnityEngine.Random.Range(0, 4));
		}
	}

	[RPC]
	private void setMyTarget(int ID)
	{
		if (ID == -1)
		{
			myHero = null;
			return;
		}
		PhotonView photonView = PhotonView.Find(ID);
		if (photonView != null)
		{
			mycurrenthero = photonView.gameObject;
			if (mycurrenthero != null)
			{
				myHero = photonView.gameObject;
				myHERO = mycurrenthero.GetComponent<HERO>();
				myHeroT = mycurrenthero.transform;
				myHeroR = myHero.rigidbody;
				myHeroPV = base.photonView;
			}
			else
			{
				myHERO = null;
				myHeroT = null;
				myHeroR = null;
				myHeroPV = null;
			}
		}
	}

	public void setRoute(GameObject route)
	{
		checkPoints = new ArrayList();
		for (int i = 1; i <= 10; i++)
		{
			checkPoints.Add(route.transform.Find("r" + i).position);
		}
		checkPoints.Add("end");
	}

	private bool simpleHitTestLineAndBall(Vector3 line, Vector3 ball, float R)
	{
		Vector3 vector = Vector3.Project(ball, line);
		if ((ball - vector).magnitude > R)
		{
			return false;
		}
		if (Vector3.Dot(line, vector) < 0f)
		{
			return false;
		}
		if (vector.sqrMagnitude > line.sqrMagnitude)
		{
			return false;
		}
		return true;
	}

	private void sitdown()
	{
		state = TitanState.sit;
		playAnimation("sit_down");
		getdownTime = UnityEngine.Random.Range(10f, 30f);
	}

	private void Start()
	{
		FengGameManagerMKII.addTitan(this, baseG);
		runAnimation = "run_walk";
		grabTF = new GameObject();
		grabTF.name = "titansTmpGrabTF";
		head = baseT.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
		neck = baseT.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
		oldHeadRotation = head.rotation;
		if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || basePV.isMine)
		{
			if (!hasSetLevel)
			{
				myLevel = UnityEngine.Random.Range(0.7f, 3f);
				if (GameSettings.sizeMode > 0)
				{
					float sizeLower = GameSettings.sizeLower;
					float sizeUpper = GameSettings.sizeUpper;
					myLevel = UnityEngine.Random.Range(sizeLower, sizeUpper);
				}
				hasSetLevel = true;
			}
			spawnPt = baseT.position;
			setmyLevel();
			setAbnormalType2(abnormalType, forceCrawler: false);
			if (myHero == null)
			{
				findNearestHero();
			}
			controller = GetComponent<TITAN_CONTROLLER>();
			if (nonAI)
			{
				StartCoroutine(reloadSky());
			}
		}
		if (maxHealth == 0 && GameSettings.healthMode > 0)
		{
			if (GameSettings.healthMode == 1)
			{
				maxHealth = (currentHealth = UnityEngine.Random.Range(GameSettings.healthLower, GameSettings.healthUpper + 1));
			}
			else if (GameSettings.healthMode == 2)
			{
				maxHealth = (currentHealth = Mathf.Clamp(Mathf.RoundToInt(myLevel / 4f * (float)UnityEngine.Random.Range(GameSettings.healthLower, GameSettings.healthUpper + 1)), GameSettings.healthLower, GameSettings.healthUpper));
			}
		}
		lagMax = 150f + myLevel * 3f;
		healthTime = Time.time;
		if (currentHealth > 0 && basePV.isMine)
		{
			basePV.RPC("labelRPC", PhotonTargets.AllBuffered, currentHealth, maxHealth);
		}
		hasExplode = false;
		colliderEnabled = true;
		isHooked = false;
		isLook = false;
		hasSpawn = true;
		isThunderSpear = false;
	}

	public void suicide()
	{
		netDie();
		if (nonAI)
		{
			FengGameManagerMKII.instance.sendKillInfo(t1: false, string.Empty, t2: true, (string)PhotonNetwork.player.customProperties["name"], 0);
		}
		FengGameManagerMKII.instance.needChooseSide = true;
		FengGameManagerMKII.justSuicide = true;
	}

	public void testVisual(bool setCollider)
	{
		if (setCollider)
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.color = Color.white;
			}
		}
		else
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.color = Color.black;
			}
		}
	}

	[RPC]
	public void titanGetHit(int viewID, int speed)
	{
		PhotonView photonView = PhotonView.Find(viewID);
		if (!(photonView != null) || !((photonView.gameObject.transform.position - neck.position).magnitude < lagMax) || hasDie || !(Time.time - healthTime > 0.2f))
		{
			return;
		}
		healthTime = Time.time;
		if (speed >= GameSettings.damageMode || abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			currentHealth -= speed;
		}
		if ((float)maxHealth > 0f)
		{
			basePV.RPC("labelRPC", PhotonTargets.AllBuffered, currentHealth, maxHealth);
		}
		if ((float)currentHealth < 0f)
		{
			if (PhotonNetwork.isMasterClient)
			{
				OnTitanDie(photonView);
			}
			basePV.RPC("netDie", PhotonTargets.OthersBuffered);
			if (grabbedTarget != null)
			{
				grabbedTarget.photonView.RPC("netUngrabbed", PhotonTargets.All);
			}
			netDie();
			if (nonAI)
			{
				FengGameManagerMKII.instance.titanGetKill(photonView.owner, speed, (string)PhotonNetwork.player.customProperties["name"]);
			}
			else
			{
				FengGameManagerMKII.instance.titanGetKill(photonView.owner, speed, base.name);
			}
		}
		else
		{
            FengGameManagerMKII.PView.RPC("netShowDamage", photonView.owner, speed);
		}
	}

	public void toCheckPoint(Vector3 targetPt, float r)
	{
		state = TitanState.to_check_point;
		targetCheckPt = targetPt;
		targetR = r;
		crossFade(runAnimation, 0.5f);
	}

	public void toPVPCheckPoint(Vector3 targetPt, float r)
	{
		state = TitanState.to_pvp_pt;
		targetCheckPt = targetPt;
		targetR = r;
		crossFade(runAnimation, 0.5f);
	}

	private void turn(float d)
	{
		if (abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			if (d > 0f)
			{
				turnAnimation = "crawler_turnaround_R";
			}
			else
			{
				turnAnimation = "crawler_turnaround_L";
			}
		}
		else if (d > 0f)
		{
			turnAnimation = "turnaround2";
		}
		else
		{
			turnAnimation = "turnaround1";
		}
		playAnimation(turnAnimation);
		baseA[turnAnimation].time = 0f;
		d = Mathf.Clamp(d, -120f, 120f);
		turnDeg = d;
		desDeg = baseGT.rotation.eulerAngles.y + turnDeg;
		state = TitanState.turn;
	}

	public void update()
	{
		UpdateHeroDistance();
		if ((IN_GAME_MAIN_CAMERA.isPausing && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || myDifficulty < 0 || (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine))
		{
			return;
		}
		explode();
		if (!nonAI)
		{
			if (activeRad < int.MaxValue && (state == TitanState.idle || state == TitanState.wander || state == TitanState.chase))
			{
				if (checkPoints.Count > 1)
				{
					if (Vector3.Distance((Vector3)checkPoints[0], baseT.position) > (float)activeRad)
					{
						toCheckPoint((Vector3)checkPoints[0], 10f);
					}
				}
				else if (Vector3.Distance(spawnPt, baseT.position) > (float)activeRad)
				{
					toCheckPoint(spawnPt, 10f);
				}
			}
			if (whoHasTauntMe != null)
			{
				tauntTime -= Time.deltaTime;
				if (tauntTime <= 0f)
				{
					whoHasTauntMe = null;
				}
				myHero = whoHasTauntMe;
				myHERO = (mycurrenthero = whoHasTauntMe).GetComponent<HERO>();
				myHeroT = whoHasTauntMe.transform;
				myHeroR = whoHasTauntMe.rigidbody;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
				{
					myHeroPV = whoHasTauntMe.GetComponent<PhotonView>();
				}
			}
		}
		if (hasDie)
		{
			dieTime += Time.deltaTime;
			if (dieTime > 2f && !hasDieSteam)
			{
				hasDieSteam = true;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("FX/FXtitanDie1"), hip.position, Quaternion.Euler(-90f, 0f, 0f))).transform.localScale = baseT.localScale;
				}
				else if (basePV.isMine)
				{
					PhotonNetwork.Instantiate("FX/FXtitanDie1", hip.position, Quaternion.Euler(-90f, 0f, 0f), 0, null).transform.localScale = baseT.localScale;
				}
			}
			if (dieTime > 5f)
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("FX/FXtitanDie"), hip.position, Quaternion.Euler(-90f, 0f, 0f))).transform.localScale = baseT.localScale;
					UnityEngine.Object.Destroy(baseG);
				}
				else if (basePV.isMine)
				{
					PhotonNetwork.Instantiate("FX/FXtitanDie", hip.position, Quaternion.Euler(-90f, 0f, 0f), 0, null).transform.localScale = baseT.localScale;
					PhotonNetwork.Destroy(baseG);
					myDifficulty = -1;
				}
			}
			return;
		}
		if (state == TitanState.hit)
		{
			if (hitPause > 0f)
			{
				hitPause -= Time.deltaTime;
				if (hitPause <= 0f)
				{
					baseA[hitAnimation].speed = 1f;
					hitPause = 0f;
				}
			}
			if (baseA[hitAnimation].normalizedTime >= 1f)
			{
				idle();
			}
		}
		if (!nonAI)
		{
			if (myHero == null)
			{
				findNearestHero();
			}
			if ((state == TitanState.idle || state == TitanState.chase || state == TitanState.wander) && whoHasTauntMe == null && UnityEngine.Random.Range(0, 100) < 10)
			{
				findNearestFacingHero();
			}
		}
		else
		{
			if (stamina < maxStamina)
			{
				if (baseA.IsPlaying("idle"))
				{
					stamina += Time.deltaTime * 30f;
				}
				if (baseA.IsPlaying("crawler_idle"))
				{
					stamina += Time.deltaTime * 35f;
				}
				if (baseA.IsPlaying("run_walk"))
				{
					stamina += Time.deltaTime * 10f;
				}
			}
			if (baseA.IsPlaying("run_abnormal_1"))
			{
				stamina -= Time.deltaTime * 5f;
			}
			if (baseA.IsPlaying("crawler_run"))
			{
				stamina -= Time.deltaTime * 15f;
			}
			if (stamina < 0f)
			{
				stamina = 0f;
			}
			if (!IN_GAME_MAIN_CAMERA.isPausing)
			{
				CacheGameObject.Find("stamina_titan").transform.localScale = new Vector3(stamina, 16f);
			}
		}
		if (state == TitanState.laugh)
		{
			if (baseA["laugh"].normalizedTime >= 1f)
			{
				idle(2f);
			}
		}
		else if (state == TitanState.idle)
		{
			if (nonAI)
			{
				if (IN_GAME_MAIN_CAMERA.isPausing)
				{
					return;
				}
				pt();
				if (abnormalType != AbnormalType.TYPE_CRAWLER)
				{
					if (controller.isAttackDown && stamina > 25f)
					{
						stamina -= 25f;
						attack2("combo_1");
					}
					else if (controller.isAttackIIDown && stamina > 50f)
					{
						stamina -= 50f;
						attack2("abnormal_jump");
					}
					else if (controller.isJumpDown && stamina > 15f)
					{
						stamina -= 15f;
						attack2("jumper_0");
					}
				}
				else if (controller.isAttackDown && stamina > 40f)
				{
					stamina -= 40f;
					attack2("crawler_jump_0");
				}
				if (controller.isSuicide)
				{
					suicide();
				}
				return;
			}
			if (sbtime > 0f)
			{
				sbtime -= Time.deltaTime;
				return;
			}
			if (!isAlarm)
			{
				if (abnormalType != AbnormalType.TYPE_PUNK && abnormalType != AbnormalType.TYPE_CRAWLER && UnityEngine.Random.Range(0f, 1f) < 0.005f)
				{
					sitdown();
					return;
				}
				if (UnityEngine.Random.Range(0f, 1f) < 0.02f)
				{
					wander();
					return;
				}
				if (UnityEngine.Random.Range(0f, 1f) < 0.01f)
				{
					turn(UnityEngine.Random.Range(30, 120));
					return;
				}
				if (UnityEngine.Random.Range(0f, 1f) < 0.01f)
				{
					turn(UnityEngine.Random.Range(-30, -120));
					return;
				}
			}
			angle = 0f;
			between2 = 0f;
			if (myDistance < chaseDistance || whoHasTauntMe != null)
			{
				Vector3 vector = myHeroT.position - baseT.position;
				angle = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
				between2 = 0f - Mathf.DeltaAngle(angle, baseGT.rotation.eulerAngles.y - 90f);
				if (myDistance >= attackDistance)
				{
					if (isAlarm || Mathf.Abs(between2) < 90f)
					{
						chase();
						return;
					}
					if (!isAlarm && !(myDistance >= chaseDistance * 0.1f))
					{
						chase();
						return;
					}
				}
			}
			if (longRangeAttackCheck2())
			{
				return;
			}
			if (myDistance < chaseDistance)
			{
				if (abnormalType == AbnormalType.TYPE_JUMPER && (myDistance > attackDistance || myHeroT.position.y > head.position.y + 4f * myLevel) && Mathf.Abs(between2) < 120f && Vector3.Distance(baseT.position, myHeroT.position) < 1.5f * myHeroT.position.y)
				{
					attack2("jumper_0");
					return;
				}
				if (abnormalType == AbnormalType.TYPE_CRAWLER && myDistance < attackDistance * 3f && Mathf.Abs(between2) < 90f && myHeroT.position.y < neck.position.y + 30f * myLevel && myHeroT.position.y > neck.position.y + 10f * myLevel)
				{
					attack2("crawler_jump_0");
					return;
				}
			}
			if (abnormalType == AbnormalType.TYPE_PUNK && myDistance < 90f && Mathf.Abs(between2) > 90f)
			{
				if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
				{
					randomRun(baseT.position + new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f)), 10f);
				}
				if (UnityEngine.Random.Range(0f, 1f) < 0.2f)
				{
					recover();
				}
				else if (UnityEngine.Random.Range(0, 2) == 0)
				{
					attack2("quick_turn_l");
				}
				else
				{
					attack2("quick_turn_r");
				}
				return;
			}
			if (myDistance < attackDistance)
			{
				if (abnormalType == AbnormalType.TYPE_CRAWLER)
				{
					if (myHeroT.position.y + 3f <= neck.position.y + 20f * myLevel && UnityEngine.Random.Range(0f, 1f) < 0.1f)
					{
						chase();
					}
					return;
				}
				string text = string.Empty;
				string[] attackStrategy = GetAttackStrategy();
				if (attackStrategy != null)
				{
					text = attackStrategy[UnityEngine.Random.Range(0, attackStrategy.Length)];
				}
				if ((abnormalType == AbnormalType.TYPE_JUMPER || abnormalType == AbnormalType.TYPE_I) && Mathf.Abs(between2) > 40f)
				{
					if (text.Contains("grab") || text.Contains("kick") || text.Contains("slap") || text.Contains("bite"))
					{
						if (UnityEngine.Random.Range(0, 100) < 30)
						{
							turn(between2);
							return;
						}
					}
					else if (UnityEngine.Random.Range(0, 100) < 90)
					{
						turn(between2);
						return;
					}
				}
				if (executeAttack2(text))
				{
					return;
				}
				if (abnormalType == AbnormalType.NORMAL)
				{
					if (UnityEngine.Random.Range(0, 100) < 30 && Mathf.Abs(between2) > 45f)
					{
						turn(between2);
						return;
					}
				}
				else if (Mathf.Abs(between2) > 45f)
				{
					turn(between2);
					return;
				}
			}
			if (!(PVPfromCheckPt != null))
			{
				return;
			}
			if (PVPfromCheckPt.state == CheckPointState.Titan)
			{
				if (UnityEngine.Random.Range(0, 100) > 48)
				{
					GameObject chkPtNext = PVPfromCheckPt.chkPtNext;
					if (chkPtNext != null && (chkPtNext.GetComponent<PVPcheckPoint>().state != CheckPointState.Titan || UnityEngine.Random.Range(0, 100) < 20))
					{
						toPVPCheckPoint(chkPtNext.transform.position, 5 + UnityEngine.Random.Range(0, 10));
						PVPfromCheckPt = chkPtNext.GetComponent<PVPcheckPoint>();
					}
				}
				else
				{
					GameObject chkPtNext = PVPfromCheckPt.chkPtPrevious;
					if (chkPtNext != null && (chkPtNext.GetComponent<PVPcheckPoint>().state != CheckPointState.Titan || UnityEngine.Random.Range(0, 100) < 5))
					{
						toPVPCheckPoint(chkPtNext.transform.position, 5 + UnityEngine.Random.Range(0, 10));
						PVPfromCheckPt = chkPtNext.GetComponent<PVPcheckPoint>();
					}
				}
			}
			else
			{
				toPVPCheckPoint(PVPfromCheckPt.transform.position, 5 + UnityEngine.Random.Range(0, 10));
			}
		}
		else if (state == TitanState.attack)
		{
            //foreach (var src in GameObject.FindObjectsOfType<AudioSource>())
            //{
            //    if (src.name != "NotificationSound")
            //    {
            //        src.mute = true;
            //    }
            //}
            if (attackAnimation == "combo")
			{
				if (nonAI)
				{
					if (controller.isAttackDown)
					{
						nonAIcombo = true;
					}
					if (!nonAIcombo && !(baseA["attack_" + attackAnimation].normalizedTime < 0.385f))
					{
						idle();
						return;
					}
				}
				if (baseA["attack_" + attackAnimation].normalizedTime >= 0.11f && baseA["attack_" + attackAnimation].normalizedTime <= 0.16f)
				{
					HERO hERO = checkIfHitHand(hand_R_001SphereT);
					if (hERO != null)
					{
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							hERO.die((hERO.transform.position - chest.position) * 15f * myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine && !hERO.HasDied())
						{
							hERO.markDie();
							object[] parameters = new object[5]
							{
								(hERO.transform.position - chest.position) * 15f * myLevel,
								false,
								(!nonAI) ? (-1) : basePV.viewID,
								baseG.name,
								true
							};
							hERO.photonView.RPC("netDie", PhotonTargets.All, parameters);
						}
					}
				}
				if (baseA["attack_" + attackAnimation].normalizedTime >= 0.27f && baseA["attack_" + attackAnimation].normalizedTime <= 0.32f)
				{
					HERO hERO2 = checkIfHitHand(hand_L_001SphereT);
					if (hERO2 != null)
					{
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							hERO2.die((hERO2.transform.position - chest.position) * 15f * myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine && !hERO2.HasDied())
						{
							hERO2.markDie();
							object[] parameters2 = new object[5]
							{
								(hERO2.transform.position - chest.position) * 15f * myLevel,
								false,
								(!nonAI) ? (-1) : basePV.viewID,
								baseG.name,
								true
							};
							hERO2.photonView.RPC("netDie", PhotonTargets.All, parameters2);
						}
					}
				}
			}
			if (attackCheckTimeA != 0f && baseA["attack_" + attackAnimation].normalizedTime >= attackCheckTimeA && baseA["attack_" + attackAnimation].normalizedTime <= attackCheckTimeB)
			{
				if (leftHandAttack)
				{
					HERO hERO3 = checkIfHitHand(hand_L_001SphereT);
					if (hERO3 != null)
					{
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							hERO3.die((hERO3.transform.position - chest.position) * 15f * myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine && !hERO3.HasDied())
						{
							hERO3.markDie();
							object[] parameters3 = new object[5]
							{
								(hERO3.transform.position - chest.position) * 15f * myLevel,
								false,
								(!nonAI) ? (-1) : basePV.viewID,
								baseG.name,
								true
							};
							hERO3.photonView.RPC("netDie", PhotonTargets.All, parameters3);
						}
					}
				}
				else
				{
					HERO hERO4 = checkIfHitHand(hand_R_001SphereT);
					if (hERO4 != null)
					{
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							hERO4.die((hERO4.transform.position - chest.position) * 15f * myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine && !hERO4.HasDied())
						{
							hERO4.markDie();
							object[] parameters4 = new object[5]
							{
								(hERO4.transform.position - chest.position) * 15f * myLevel,
								false,
								(!nonAI) ? (-1) : basePV.viewID,
								baseG.name,
								true
							};
							hERO4.photonView.RPC("netDie", PhotonTargets.All, parameters4);
						}
					}
				}
			}
			if (!attacked && attackCheckTime != 0f && baseA["attack_" + attackAnimation].normalizedTime >= attackCheckTime)
			{
				attacked = true;
				fxPosition = baseT.Find("ap_" + attackAnimation).position;
				GameObject gameObject;
				Transform transform = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !basePV.isMine) ? (gameObject = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("FX/" + fxName), fxPosition, fxRotation)).transform : (gameObject = PhotonNetwork.Instantiate("FX/" + fxName, fxPosition, fxRotation, 0)).transform);
				EnemyfxIDcontainer component = gameObject.GetComponent<EnemyfxIDcontainer>();
				if (nonAI)
				{
					transform.localScale = baseT.localScale * 1.5f;
					if (component != null)
					{
						component.myOwnerViewID = basePV.viewID;
					}
				}
				else
				{
					transform.localScale = baseT.localScale;
				}
				if (component != null)
				{
					component.titanName = baseG.name;
				}
				float b = 1f - Vector3.Distance(camT.position, gameObject.transform.position) * 0.05f;
				b = Mathf.Min(1f, b);
				if (IN_GAME_MAIN_CAMERA.mainCamera != null)
				{
					IN_GAME_MAIN_CAMERA.mainCamera.startShake(b, b);
				}
			}
			if (attackAnimation == "throw")
			{
				if (!attacked && baseA["attack_" + attackAnimation].normalizedTime >= 0.11f)
				{
					attacked = true;
					Transform transform2 = hand_R_001;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
					{
						throwRock = PhotonNetwork.Instantiate("FX/rockThrow", transform2.position, transform2.rotation, 0);
					}
					else
					{
						throwRock = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("FX/rockThrow"), transform2.position, transform2.rotation);
					}
					rockT = throwRock.transform;
					rockT.localScale = baseT.localScale;
					rockT.position -= rockT.forward * 2.5f * myLevel;
					EnemyfxIDcontainer component2 = throwRock.GetComponent<EnemyfxIDcontainer>();
					if (component2 != null)
					{
						if (nonAI)
						{
							component2.myOwnerViewID = basePV.viewID;
						}
						component2.titanName = baseG.name;
					}
					rockT.parent = hand_R_001;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
					{
						object[] parameters5 = new object[4] { basePV.viewID, baseT.localScale, rockT.localPosition, myLevel };
						throwRock.GetComponent<PhotonView>().RPC("initRPC", PhotonTargets.Others, parameters5);
					}
				}
				if (baseA["attack_" + attackAnimation].normalizedTime >= 0.11f)
				{
					float y = Mathf.Atan2(myHeroT.position.x - baseT.position.x, myHeroT.position.z - baseT.position.z) * 57.29578f;
					baseGT.rotation = Quaternion.Euler(0f, y, 0f);
				}
				if (throwRock != null && baseA["attack_" + attackAnimation].normalizedTime >= 0.62f)
				{
					float num = 1f;
					float num2 = -20f;
					Vector3 vector2;
					if (myHero != null)
					{
						vector2 = (myHeroT.position - rockT.position) / num + myHeroR.velocity;
						float num3 = myHeroT.position.y + 2f * myLevel - rockT.position.y;
						vector2 = new Vector3(vector2.x, num3 / num - 0.5f * num2 * num, vector2.z);
					}
					else
					{
						vector2 = baseT.forward * 60f + Vector3.up * 10f;
					}
					throwRock.GetComponent<RockThrow>().launch(vector2);
					rockT.parent = null;
					throwRock = null;
				}
			}
			if (attackAnimation == "jumper_0" || attackAnimation == "crawler_jump_0")
			{
				if (!attacked)
				{
					if (baseA["attack_" + attackAnimation].normalizedTime >= 0.68f)
					{
						attacked = true;
						if (myHero == null || nonAI)
						{
							float num4 = 120f;
							Vector3 velocity = baseT.forward * speed + Vector3.up * num4;
							if (nonAI && abnormalType == AbnormalType.TYPE_CRAWLER)
							{
								num4 = 100f;
								float a = speed * 2.5f;
								a = Mathf.Min(a, 100f);
								velocity = baseT.forward * a + Vector3.up * num4;
							}
							baseR.velocity = velocity;
						}
						else
						{
							float y2 = myHeroR.velocity.y;
							float num5 = -20f;
							float num6 = gravity;
							float y3 = neck.position.y;
							float num7 = (num5 - num6) * 0.5f;
							float num8 = y2;
							float num9 = myHeroT.position.y - y3;
							float num10 = Mathf.Abs((Mathf.Sqrt(num8 * num8 - 4f * num7 * num9) - num8) / (2f * num7));
							Vector3 vector3 = myHeroT.position + myHeroR.velocity * num10 + Vector3.up * 0.5f * num5 * num10 * num10;
							float y4 = vector3.y;
							float num11;
							if (num9 < 0f || y4 - y3 < 0f)
							{
								num11 = 60f;
								float a2 = speed * 2.5f;
								a2 = Mathf.Min(a2, 100f);
								Vector3 velocity2 = baseT.forward * a2 + Vector3.up * num11;
								baseR.velocity = velocity2;
								return;
							}
							float num12 = y4 - y3;
							float num13 = Mathf.Sqrt(2f * num12 / gravity);
							num11 = gravity * num13;
							num11 = Mathf.Max(30f, num11);
							Vector3 vector4 = (vector3 - baseT.position) / num10;
							abnorma_jump_bite_horizon_v = new Vector3(vector4.x, 0f, vector4.z);
							Vector3 velocity3 = baseR.velocity;
							Vector3 force = new Vector3(abnorma_jump_bite_horizon_v.x, velocity3.y, abnorma_jump_bite_horizon_v.z) - velocity3;
							baseR.AddForce(force, ForceMode.VelocityChange);
							baseR.AddForce(Vector3.up * num11, ForceMode.VelocityChange);
							float num14 = Vector2.Angle(new Vector2(baseT.position.x, baseT.position.z), new Vector2(myHeroT.position.x, myHeroT.position.z));
							num14 = Mathf.Atan2(myHeroT.position.x - baseT.position.x, myHeroT.position.z - baseT.position.z) * 57.29578f;
							baseGT.rotation = Quaternion.Euler(0f, num14, 0f);
						}
					}
					else
					{
						baseR.velocity = Vector3.zero;
					}
				}
				if (!(baseA["attack_" + attackAnimation].normalizedTime >= 1f))
				{
					return;
				}
				MONO mONO = checkIfHitHead(head, 3f);
				if (mONO != null && mONO.species == SPECIES.Hero)
				{
					HERO hERO5 = mONO as HERO;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						hERO5.die((hERO5.transform.position - chest.position) * 15f * myLevel, isBite: false);
					}
					else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine && !hERO5.HasDied())
					{
						hERO5.markDie();
						object[] parameters6 = new object[5]
						{
							(hERO5.transform.position - chest.position) * 15f * myLevel,
							true,
							(!nonAI) ? (-1) : basePV.viewID,
							baseG.name,
							true
						};
						hERO5.photonView.RPC("netDie", PhotonTargets.All, parameters6);
					}
					if (abnormalType == AbnormalType.TYPE_CRAWLER)
					{
						attackAnimation = "crawler_jump_1";
					}
					else
					{
						attackAnimation = "jumper_1";
					}
					playAnimation("attack_" + attackAnimation);
				}
				if (Mathf.Abs(baseR.velocity.y) < 0.5f || baseR.velocity.y < 0f || IsGrounded())
				{
					if (abnormalType == AbnormalType.TYPE_CRAWLER)
					{
						attackAnimation = "crawler_jump_1";
					}
					else
					{
						attackAnimation = "jumper_1";
					}
					playAnimation("attack_" + attackAnimation);
				}
			}
			else if (attackAnimation == "jumper_1" || attackAnimation == "crawler_jump_1")
			{
				if (baseA["attack_" + attackAnimation].normalizedTime >= 1f && grounded)
				{
					if (abnormalType == AbnormalType.TYPE_CRAWLER)
					{
						attackAnimation = "crawler_jump_2";
					}
					else
					{
						attackAnimation = "jumper_2";
					}
					crossFade("attack_" + attackAnimation, 0.1f);
					fxPosition = baseT.position;
					GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !basePV.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("FX/boom2"), fxPosition, fxRotation)) : PhotonNetwork.Instantiate("FX/boom2", fxPosition, fxRotation, 0));
					gameObject2.transform.localScale = baseT.localScale * 1.6f;
					float b2 = 1f - Vector3.Distance(camT.position, gameObject2.transform.position) * 0.05f;
					b2 = Mathf.Min(1f, b2);
					if (IN_GAME_MAIN_CAMERA.mainCamera != null)
					{
						IN_GAME_MAIN_CAMERA.mainCamera.startShake(b2, b2);
					}
				}
			}
			else if (attackAnimation == "jumper_2" || attackAnimation == "crawler_jump_2")
			{
				if (baseA["attack_" + attackAnimation].normalizedTime >= 1f)
				{
					idle();
				}
			}
			else if (baseA.IsPlaying("tired"))
			{
				if (baseA["tired"].normalizedTime >= 1f + Mathf.Max(attackEndWait * 2f, 3f))
				{
					idle(UnityEngine.Random.Range(attackWait - 1f, 3f));
				}
			}
			else
			{
				if (!(baseA["attack_" + attackAnimation].normalizedTime >= 1f + attackEndWait))
				{
					return;
				}
				if (nextAttackAnimation != null)
				{
					attack2(nextAttackAnimation);
				}
				else if (attackAnimation == "quick_turn_l" || attackAnimation == "quick_turn_r")
				{
					baseT.rotation = Quaternion.Euler(baseT.rotation.eulerAngles.x, baseT.rotation.eulerAngles.y + 180f, baseT.rotation.eulerAngles.z);
					idle(UnityEngine.Random.Range(0.5f, 1f));
					playAnimation("idle");
				}
				else if (abnormalType == AbnormalType.TYPE_I || abnormalType == AbnormalType.TYPE_JUMPER)
				{
					attackCount++;
					if (attackCount > 3 && attackAnimation == "abnormal_getup")
					{
						attackCount = 0;
						crossFade("tired", 0.5f);
					}
					else
					{
						idle(UnityEngine.Random.Range(attackWait - 1f, 3f));
					}
				}
				else
				{
					idle(UnityEngine.Random.Range(attackWait - 1f, 3f));
				}
			}
		}
		else if (state == TitanState.grab)
		{
			if (baseA["grab_" + attackAnimation].normalizedTime >= attackCheckTimeA && baseA["grab_" + attackAnimation].normalizedTime <= attackCheckTimeB && grabbedTarget == null)
			{
				HERO hERO6 = checkIfHitHand(currentGrabHand);
				if (hERO6 != null)
				{
					if (isGrabHandLeft)
					{
						eatSetL(hERO6);
						grabbedTarget = hERO6;
					}
					else
					{
						eatSet(hERO6);
						grabbedTarget = hERO6;
					}
				}
			}
			if (baseA["grab_" + attackAnimation].normalizedTime >= 1f)
			{
				if (grabbedTarget != null)
				{
					eat();
				}
				else
				{
					idle(UnityEngine.Random.Range(attackWait - 1f, 2f));
				}
			}
		}
		else if (state == TitanState.eat)
		{
			if (!attacked && !(baseA[attackAnimation].normalizedTime < 0.48f))
			{
				attacked = true;
				justEatHero(grabbedTarget, currentGrabHand);
			}
			_ = grabbedTarget == null;
			if (baseA[attackAnimation].normalizedTime >= 1f)
			{
				idle();
			}
		}
		else if (state == TitanState.chase)
		{
			if (myHero == null)
			{
				idle();
			}
			else
			{
				if (longRangeAttackCheck2())
				{
					return;
				}
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE && PVPfromCheckPt != null && myDistance > chaseDistance)
				{
					idle();
				}
				else if (abnormalType == AbnormalType.TYPE_CRAWLER)
				{
					Vector3 vector5 = myHeroT.position - baseT.position;
					float f = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector5.z, vector5.x)) * 57.29578f, baseGT.rotation.eulerAngles.y - 90f);
					if (myDistance < attackDistance * 3f && UnityEngine.Random.Range(0f, 1f) < 0.1f && Mathf.Abs(f) < 90f && myHeroT.position.y < neck.position.y + 30f * myLevel && myHeroT.position.y > neck.position.y + 10f * myLevel)
					{
						attack2("crawler_jump_0");
						return;
					}
					MONO mONO2 = checkIfHitCrawlerMouth(head, 2.2f);
					if (mONO2 != null)
					{
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							if (mONO2.species == SPECIES.Hero)
							{
								(mONO2 as HERO).die((mONO2.transform.position - chest.position) * 15f * myLevel, isBite: false);
							}
							else if (mONO2.species == SPECIES.TitanEren)
							{
								(mONO2 as TITAN_EREN).hitByTitan();
							}
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
						{
							if (mONO2.species == SPECIES.TitanEren)
							{
								(mONO2 as TITAN_EREN).hitByTitan();
							}
							else
							{
								HERO hERO7 = mONO2 as HERO;
								if (!hERO7.HasDied())
								{
									hERO7.markDie();
									object[] parameters7 = new object[5]
									{
										(hERO7.transform.position - chest.position) * 15f * myLevel,
										true,
										(!nonAI) ? (-1) : basePV.viewID,
										base.name,
										true
									};
									hERO7.photonView.RPC("netDie", PhotonTargets.All, parameters7);
								}
							}
						}
					}
					if (myDistance < attackDistance && UnityEngine.Random.Range(0f, 1f) < 0.02f)
					{
						idle(UnityEngine.Random.Range(0.05f, 0.2f));
					}
				}
				else if (abnormalType == AbnormalType.TYPE_JUMPER && ((myDistance > attackDistance && myHeroT.position.y > head.position.y + 4f * myLevel) || myHeroT.position.y > head.position.y + 4f * myLevel) && Vector3.Distance(baseT.position, myHeroT.position) < 1.5f * myHeroT.position.y)
				{
					attack2("jumper_0");
				}
				else if (myDistance < attackDistance)
				{
					idle(UnityEngine.Random.Range(0.05f, 0.2f));
				}
			}
		}
		else if (state == TitanState.wander)
		{
			float num15 = 0f;
			if (myDistance < chaseDistance || whoHasTauntMe != null)
			{
				Vector3 vector6 = myHeroT.position - baseT.position;
				num15 = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector6.z, vector6.x)) * 57.29578f, baseGT.rotation.eulerAngles.y - 90f);
				if (isAlarm || Mathf.Abs(num15) < 90f)
				{
					chase();
					return;
				}
				if (!isAlarm && !(myDistance >= chaseDistance * 0.1f))
				{
					chase();
					return;
				}
			}
			if (UnityEngine.Random.Range(0f, 1f) < 0.01f)
			{
				idle();
			}
		}
		else if (state == TitanState.turn)
		{
			baseGT.rotation = Quaternion.Lerp(baseGT.rotation, Quaternion.Euler(0f, desDeg, 0f), Time.deltaTime * Mathf.Abs(turnDeg) * 0.015f);
			if (baseA[turnAnimation].normalizedTime >= 1f)
			{
				idle();
			}
		}
		else if (state == TitanState.hit_eye)
		{
			if (baseA.IsPlaying("sit_hit_eye") && baseA["sit_hit_eye"].normalizedTime >= 1f)
			{
				remainSitdown();
			}
			else if (baseA.IsPlaying("hit_eye") && baseA["hit_eye"].normalizedTime >= 1f)
			{
				if (nonAI)
				{
					idle();
				}
				else
				{
					attack2("combo_1");
				}
			}
		}
		else if (state == TitanState.to_check_point)
		{
			if (checkPoints.Count <= 0 && myDistance < attackDistance)
			{
				string decidedAction = string.Empty;
				string[] attackStrategy2 = GetAttackStrategy();
				if (attackStrategy2 != null)
				{
					decidedAction = attackStrategy2[UnityEngine.Random.Range(0, attackStrategy2.Length)];
				}
				if (executeAttack2(decidedAction))
				{
					return;
				}
			}
			if (!(Vector3.Distance(baseT.position, targetCheckPt) < targetR))
			{
				return;
			}
			if (checkPoints.Count > 0)
			{
				if (checkPoints.Count == 1)
				{
					if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
					{
						FengGameManagerMKII.instance.gameLose2();
						checkPoints = new ArrayList();
						idle();
					}
				}
				else
				{
					if (checkPoints.Count == 4)
					{
						FengGameManagerMKII.instance.sendChatContentInfo("<color=#A8FF24>*WARNING!* An abnormal titan is approaching the north gate!</color>");
					}
					Vector3 vector7 = (targetCheckPt = (Vector3)checkPoints[0]);
					checkPoints.RemoveAt(0);
				}
			}
			else
			{
				idle();
			}
		}
		else if (state == TitanState.to_pvp_pt)
		{
			if (myDistance < chaseDistance * 0.7f)
			{
				chase();
			}
			if (Vector3.Distance(baseT.position, targetCheckPt) < targetR)
			{
				idle();
			}
		}
		else if (state == TitanState.random_run)
		{
			random_run_time -= Time.deltaTime;
			if (Vector3.Distance(baseT.position, targetCheckPt) < targetR || random_run_time <= 0f)
			{
				idle();
			}
		}
		else if (state == TitanState.down)
		{
			getdownTime -= Time.deltaTime;
			if (baseA.IsPlaying("sit_hunt_down") && baseA["sit_hunt_down"].normalizedTime >= 1f && !base.animation.IsPlaying("sit_idle"))
			{
				playAnimation("sit_idle");
			}
			if (getdownTime <= 0f)
			{
				crossFadeIfNotPlaying("sit_getup", 0.1f);
			}
			if (baseA.IsPlaying("sit_getup") && baseA["sit_getup"].normalizedTime >= 1f)
			{
				idle();
			}
		}
		else if (state == TitanState.sit)
		{
			getdownTime -= Time.deltaTime;
			angle = 0f;
			between2 = 0f;
			if (myDistance < chaseDistance || whoHasTauntMe != null)
			{
				if (myDistance < 50f)
				{
					isAlarm = true;
				}
				else
				{
					Vector3 vector8 = myHeroT.position - baseT.position;
					angle = (0f - Mathf.Atan2(vector8.z, vector8.x)) * 57.29578f;
					between2 = 0f - Mathf.DeltaAngle(angle, baseGT.rotation.eulerAngles.y - 90f);
					if (Mathf.Abs(between2) < 100f)
					{
						isAlarm = true;
					}
				}
			}
			if (baseA.IsPlaying("sit_down") && baseA["sit_down"].normalizedTime >= 1f)
			{
				playAnimation("sit_idle");
			}
			if ((getdownTime <= 0f || isAlarm) && baseA.IsPlaying("sit_idle"))
			{
				crossFadeIfNotPlaying("sit_getup", 0.1f);
			}
			if (baseA.IsPlaying("sit_getup") && baseA["sit_getup"].normalizedTime >= 1f)
			{
				idle();
			}
		}
		else if (state == TitanState.recover)
		{
			getdownTime -= Time.deltaTime;
			if (getdownTime <= 0f)
			{
				idle();
			}
			if (baseA.IsPlaying("idle_recovery") && baseA["idle_recovery"].normalizedTime >= 1f)
			{
				idle();
			}
		}
	}

	public void updateCollider()
	{
		if (colliderEnabled)
		{
			if (isHooked || myTitanTrigger.isCollide || isLook || isThunderSpear)
			{
				return;
			}
			foreach (Collider baseCollider in baseColliders)
			{
				if (baseCollider != null)
				{
					baseCollider.enabled = false;
				}
			}
			colliderEnabled = false;
		}
		else
		{
			if (!isHooked && !myTitanTrigger.isCollide && (!isLook || isThunderSpear))
			{
				return;
			}
			foreach (Collider baseCollider2 in baseColliders)
			{
				if (baseCollider2 != null)
				{
					baseCollider2.enabled = true;
				}
			}
			colliderEnabled = true;
		}
	}

	public void updateLabel()
	{
		if (healthLabel != null && healthLabel.GetComponent<UILabel>().isVisible)
		{
			healthLabel.transform.LookAt(2f * healthLabel.transform.position - Camera.main.transform.position);
		}
	}

	private void wander(float sbtime = 0f)
	{
		state = TitanState.wander;
		crossFade(runAnimation, 0.5f);
	}
}
