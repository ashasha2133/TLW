using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Anarchy.Custom.Interfaces;
using Anarchy.Custom.Level;
using BRM;
//using CustomAudio;
using Events;
using ExitGames.Client.Photon;
using UnityEngine;
using Utility;
using Xft;

public class HERO : MONO, IAnarchyScriptHero
{
	private sealed class RandomClass
	{
		public int randomInt;

		public bool Check(HERO hero)
		{
			if (hero != null)
			{
				return hero.photonView.owner.ID == randomInt;
			}
			return false;
		}
	}

	private bool _cancelGasDisable;

	private bool _animationStopped;

	private HERO_STATE _state;

	private bool almostSingleHook;

	private string attackAnimation;

	private int attackLoop;

	private bool attackMove;

	private bool attackReleased;

	//public AudioSource audio_ally;

	//public AudioSource audio_hitwall;

	private GameObject badGuy;

	public float bombCD;

	public bool bombImmune;

	public float bombRadius;

	public float bombSpeed;

	public float bombTime;

	public float bombTimeMax;

	private float buffTime;

	public GameObject bulletLeft;

	private int bulletMAX = 7;

	public GameObject bulletRight;

	private bool buttonAttackRelease;

	public Dictionary<string, UISprite> cachedSprites;

	public float CameraMultiplier;

	public bool canJump = true;

	public GameObject checkBoxLeft;

	public GameObject checkBoxRight;

	public string currentAnimation;

	private int currentBladeNum = 5;

	private float currentBladeSta = 100f;

	private BUFF currentBuff;

	private float currentGas = 100f;

	public float currentSpeed;

	public Vector3 currentV;

	private bool dashD;

	private Vector3 dashDirection;

	private bool dashL;

	private bool dashR;

	private float dashTime;

	private bool dashU;

	private Vector3 dashV;

	public bool detonate;

	private float dTapTime = -1f;

	private bool EHold;

	private GameObject eren_titan;

	private int escapeTimes = 1;

	private float facingDirection;

	private float flare1CD;

	private float flare2CD;

	private float flare3CD;

	private float flareTotalCD = 30f;

	private Transform forearmL;

	private Transform forearmR;

	private float gravity = 20f;

	private bool grounded;

	private GameObject gunDummy;

	private Vector3 gunTarget;

	private Transform handL;

	private Transform handR;

	private bool hasDied;

	public bool hasspawn;

	private bool hookBySomeOne = true;

	private bool hookSomeOne;

	private GameObject hookTarget;

	private float invincible = 3f;

	public bool isCannon;

	private bool isLaunchLeft;

	private bool isLaunchRight;

	private bool isLeftHandHooked;

	private bool isMounted;

	public bool isPhotonCamera;

	private bool isRightHandHooked;

	private bool justGrounded;

	public UILabel LabelDistance;

	public Transform lastHook;

	private float launchElapsedTimeL;

	private float launchElapsedTimeR;

	private Vector3 launchForce;

	private Vector3 launchPointLeft;

	private Vector3 launchPointRight;

	private bool leanLeft;

	private bool leftArmAim;

	private int leftBulletLeft = 7;

	private bool leftGunHasBullet = true;

	private float lTapTime = -1f;

	public float maxVelocityChange = 10f;

	//public AudioSource meatDie;

	public Bomb myBomb;

	public GameObject myCannon;

	public Transform myCannonBase;

	public Transform myCannonPlayer;

	public CannonPropRegion myCannonRegion;

	public GROUP myGroup;

	private GameObject myHorse;

	public GameObject myNetWorkName;

	public float myScale = 1f;

	public int myTeam = 1;

	public List<TITAN> myTitans;

	private bool needLean;

	private Quaternion oldHeadRotation;

	private float originVM;

	private bool QHold;

	private string reloadAnimation = string.Empty;

	private bool rightArmAim;

	private int rightBulletLeft = 7;

	private bool rightGunHasBullet = true;

	//public AudioSource rope;

	private float rTapTime = -1f;

	public HERO_SETUP setup;

	public GameObject skillCD;

	public float skillCDDuration;

	public float skillCDLast;

	public float skillCDLastCannon;

	private string skillId;

	public string skillIDHUD;

	//public AudioSource slash;

	//public AudioSource slashHit;

	private ParticleSystem smoke_3dmg;

	private bool EnableSmoke;

	private ParticleSystem sparks;

	public float speed = 10f;

	public GameObject speedFX;

	public GameObject speedFX1;

	private ParticleSystem speedFXPS;

	private string standAnimation = "stand";

	private Quaternion targetHeadRotation;

	private Quaternion targetRotation;

	public Vector3 targetV;

	private bool throwedBlades;

	public bool titanForm;

	private GameObject titanWhoGrabMe;

	private int titanWhoGrabMeID;

	private int totalBladeNum = 5;

	public float totalBladeSta = 100f;

	public float totalGas = 100f;

	private Transform upperarmL;

	private Transform upperarmR;

	private float useGasSpeed = 0.2f;

	public bool useGun;

	public bool usingAhss;

	private float uTapTime = -1f;

	private bool wallJump;

	private float wallRunTime;

	private UISprite flare1;

	private UISprite flare2;

	private UISprite flare3;

	private Transform myNetWorkNameT;

	public XWeaponTrail rightbladetrail;

	public XWeaponTrail rightbladetrail2;

	public XWeaponTrail leftbladetrail;

	public XWeaponTrail leftbladetrail2;

	public GameObject hookRefL1;

	public GameObject hookRefL2;

	public GameObject hookRefR1;

	public GameObject hookRefR2;

	public SmoothSyncMovement sync;

	private GameObject cross1;

	private GameObject cross2;

	private Transform crossT1;

	private Transform crossT2;

	public GameObject crossL1;

	public GameObject crossL2;

	public GameObject crossR1;

	public GameObject crossR2;

	private Transform crossL1T;

	private Transform crossL2T;

	private Transform crossR1T;

	private Transform crossR2T;

	private Transform bulletLT;

	private Transform bulletRT;

	private Bullet LBullet;

	private Bullet RBullet;

	private List<Bullet> DashRampage = new List<Bullet>();

	private Transform labelT;

	public Transform hookRefL1T;

	public Transform hookRefL2T;

	public Transform hookRefR1T;

	public Transform hookRefR2T;

	private TriggerColliderWeapon triggerLeft;

	private TriggerColliderWeapon triggerRight;

	public CapsuleCollider capsule;

	public Horse horse;

	public Stopwatch burstCD = new Stopwatch();

	private bool AhssAnimation = (int)FengGameManagerMKII.settings[289] == 1 && GameSettings.bombMode > 0;

	private GameObject ThunderSpearL;

	private GameObject ThunderSpearR;

	private GameObject ThunderSpearLModel;

	private GameObject ThunderSpearRModel;

	private bool _hasRunStart;

	private bool _needSetupThunderspears;

	private float gasMultiplier = 1f;

	public Dictionary<string, Bullet> hook = new Dictionary<string, Bullet>
	{
		{ "left", null },
		{ "right", null }
	};

	public static Dictionary<PhotonPlayer, string> BombStatsCounter = new Dictionary<PhotonPlayer, string>();

	protected GameObject _owner;

	public bool AlreadyReeled;

	private static Func<GameObject, HERO> function;

	public HERO PersonBeingCarried;

	public HERO PersonWhoCarries;

	public bool CarryingSomeone;

	public bool BeingCarried;

	public bool BoolWhileBeingAsked;

	public bool BoolWhileAskingToBeCarried;

	private PhotonPlayer sender;

	public float GasUsageModifier
	{
		get
		{
			return gasMultiplier;
		}
		set
		{
			if (gasMultiplier >= 0f)
			{
				gasMultiplier = value;
			}
		}
	}

	public Bullet currentHook
	{
		get
		{
			if (hook.ContainsKey("left") && hook["left"] != null && hook["left"].isHooked())
			{
				return hook["left"];
			}
			if (hook.ContainsKey("right") && hook["right"] != null && hook["right"].isHooked())
			{
				return hook["right"];
			}
			return null;
		}
	}

	public bool isGrabbed => state == HERO_STATE.Grab;

	private HERO_STATE state
	{
		get
		{
			return _state;
		}
		set
		{
			if (_state == HERO_STATE.AirDodge || _state == HERO_STATE.GroundDodge)
			{
				dashTime = 0f;
			}
			_state = value;
		}
	}

	public bool IsMine()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			return base.photonView.isMine;
		}
		return true;
	}

	private void SetupThunderSpears()
	{
		if (base.photonView.isMine)
		{
			base.photonView.RPC("SetupThunderSpearsRPC", PhotonTargets.AllBuffered);
		}
	}

	[RPC]
	private void SetupThunderSpearsRPC(PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			if (!_hasRunStart)
			{
				_needSetupThunderspears = true;
			}
			else
			{
				CreateAndAttachThunderSpears();
			}
		}
	}

	private void CreateAndAttachThunderSpears()
	{
		ThunderSpearL = (GameObject)UnityEngine.Object.Instantiate(FengGameManagerMKII.RCassets.Load("ThunderSpearProp"));
		ThunderSpearR = (GameObject)UnityEngine.Object.Instantiate(FengGameManagerMKII.RCassets.Load("ThunderSpearProp"));
		ThunderSpearLModel = ThunderSpearL.transform.Find("ThunderSpearModel").gameObject;
		ThunderSpearRModel = ThunderSpearR.transform.Find("ThunderSpearModel").gameObject;
		AttachThunderSpear(ThunderSpearL, handL.transform, left: true);
		AttachThunderSpear(ThunderSpearR, handR.transform, left: false);
		if (GameSettings.HandsHolding == 0 || GameSettings.HandsHolding == 1)
		{
			ThunderSpearRModel.renderer.enabled = false;
			ThunderSpearLModel.renderer.enabled = false;
		}
		if (GameSettings.HandsHolding == 0 || GameSettings.HandsHolding == 2)
		{
			currentBladeNum = (totalBladeNum = 0);
			totalBladeSta = (currentBladeSta = 0f);
			setup.part_blade_l.SetActive(value: false);
			setup.part_blade_r.SetActive(value: false);
		}
	}

	private void AttachThunderSpear(GameObject thunderSpear, Transform mount, bool left)
	{
		thunderSpear.transform.parent = mount.parent;
		Vector3 localPosition = (left ? new Vector3(-0.001649f, 0.000775f, -0.000227f) : new Vector3(-0.001649f, -0.000775f, -0.000227f));
		Quaternion localRotation = (left ? Quaternion.Euler(5f, -85f, 10f) : Quaternion.Euler(-5f, -85f, -10f));
		thunderSpear.transform.localPosition = localPosition;
		thunderSpear.transform.localRotation = localRotation;
	}

	private void SetThunderSpears(bool hasLeft, bool hasRight)
	{
		base.photonView.RPC("SetThunderSpearsRPC", PhotonTargets.All, hasLeft, hasRight);
	}

	[RPC]
	private void SetThunderSpearsRPC(bool hasLeft, bool hasRight, PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			if (ThunderSpearLModel != null)
			{
				ThunderSpearLModel.SetActive(hasLeft);
			}
			if (ThunderSpearRModel != null)
			{
				ThunderSpearRModel.SetActive(hasRight);
			}
		}
	}

	public void LaunchThunderSpear()
	{
		if (myBomb != null && !myBomb.Disabled)
		{
			myBomb.Explode(bombRadius);
		}
		detonate = false;
		bombTime = 0f;
		skillCDDuration = 10f;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("EnemyBox"));
		Vector3 vector = baseT.position + ray.direction * 1000f;
		if (Physics.Raycast(ray, out var hitInfo, 1000000f, layerMask.value))
		{
			vector = hitInfo.point;
		}
		Vector3 vector2 = Vector3.Normalize(vector - baseT.position);
		Vector3 position;
		if (Vector3.Cross(baseT.forward, vector2).y < 0f && state != HERO_STATE.Land)
		{
			position = ThunderSpearL.transform.position;
			//ThunderSpearL.audio.Play();
			SetThunderSpears(hasLeft: false, hasRight: true);
			attackAnimation = "AHSS_shoot_l";
		}
		else
		{
			position = ThunderSpearR.transform.position;
			//ThunderSpearR.audio.Play();
			SetThunderSpears(hasLeft: true, hasRight: false);
			attackAnimation = "AHSS_shoot_r";
		}
		Vector3 vector3 = Vector3.Normalize(vector - position);
		if (grounded)
		{
			position += vector3 * 1f;
		}
		if (state != HERO_STATE.Slide)
		{
			if (state == HERO_STATE.Attack)
			{
				buttonAttackRelease = true;
			}
			playAnimationAt(attackAnimation, 0.1f);
			state = HERO_STATE.Attack;
			facingDirection = Quaternion.LookRotation(vector2).eulerAngles.y;
			targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
		}
		GameObject gameObject = PhotonNetwork.Instantiate("RCAsset/BombMain", position, Quaternion.LookRotation(vector3), 0);
		gameObject.rigidbody.velocity = vector3 * bombSpeed;
		myBomb = gameObject.GetComponent<Bomb>();
		myBomb.GetComponent<Bomb>().myHero = this;
		myBomb.BombRadius = bombRadius;
	}

	public void UpdateThunderSpear()
	{
		if (!(skillId == "bomb"))
		{
			return;
		}
		leftArmAim = false;
		rightArmAim = false;
		if (skillCDDuration <= 0f && (!ThunderSpearLModel.activeSelf || !ThunderSpearRModel.activeSelf))
		{
			SetThunderSpears(hasLeft: true, hasRight: true);
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.attack1] && skillCDDuration <= 0f)
		{
			//if (GameSettings.BombFlying > 0)
			//{
			//	CustomAudioManager.Play(CustomAudioManager.AudioFile.BombFlying);
			//}
			LaunchThunderSpear();
		}
		else if (myBomb != null && !myBomb.Disabled)
		{
			bombTime += Time.deltaTime;
			bool flag = false;
			if (FengCustomInputs.Inputs.isInputUp[InputCode.attack1])
			{
				detonate = true;
			}
			else if (FengCustomInputs.Inputs.isInputDown[InputCode.attack1] && detonate)
			{
				detonate = false;
				flag = true;
			}
			if (bombTime >= bombTimeMax)
			{
				flag = true;
			}
			if (flag)
			{
				myBomb.Explode(bombRadius);
				detonate = false;
			}
		}
	}

	private bool IsFiringThunderSpear()
	{
		if (skillId == "bomb")
		{
			if (!baseA.IsPlaying("AHSS_shoot_r"))
			{
				return baseA.IsPlaying("AHSS_shoot_l");
			}
			return true;
		}
		return false;
	}

	private void Start()
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV == null)
		{
			cache();
		}
		FengGameManagerMKII.instance.addHero(this, baseG);
		if ((LevelInfo.getInfo(FengGameManagerMKII.level).horse || GameSettings.horseMode == 1) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine && GameSettings.bombMode != 1)
		{
			myHorse = PhotonNetwork.Instantiate("horse", baseT.position + Vector3.up * 5f, baseT.rotation, 0);
			myHorse.GetComponent<Horse>().myHero = baseG;
			myHorse.GetComponent<TITAN_CONTROLLER>().isHorse = true;
		}
		sparks = baseT.Find("slideSparks").GetComponent<ParticleSystem>();
		smoke_3dmg = baseT.Find("3dmg_smoke").GetComponent<ParticleSystem>();
		baseT.localScale = new Vector3(myScale, myScale, myScale);
		facingDirection = baseT.rotation.eulerAngles.y;
		targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
		smoke_3dmg.enableEmission = false;
		EnableSmoke = false;
		sparks.enableEmission = false;
		speedFXPS = speedFX1.GetComponent<ParticleSystem>();
		speedFXPS.enableEmission = false;
		hookRefL1T = hookRefL1.transform;
		hookRefL2T = hookRefL2.transform;
		hookRefR1T = hookRefR1.transform;
		hookRefR2T = hookRefR2.transform;
		hookRefR2.transform.position += new Vector3(0f, 0.1f, 0f);
		if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
		{
			EventManager.CallEvent("load_custom_audio");
		}
		else
		{
			if (PhotonNetwork.isMasterClient)
			{
				int iD = basePV.owner.ID;
				if (FengGameManagerMKII.heroHash.ContainsKey(iD))
				{
					FengGameManagerMKII.heroHash[iD] = this;
				}
				else
				{
					FengGameManagerMKII.heroHash.Add(iD, this);
				}
			}
			GameObject gameObject = CacheGameObject.Find("UI_IN_GAME");
			myNetWorkName = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("UI/LabelNameOverHead"));
			myNetWorkName.name = "LabelNameOverHead";
			myNetWorkNameT = myNetWorkName.transform;
			myNetWorkNameT.parent = gameObject.GetComponent<UIReferArray>().panels[0].transform;
			myNetWorkNameT.localScale = new Vector3(14f, 14f, 14f);
			UILabel component = myNetWorkName.GetComponent<UILabel>();
			component.text = string.Empty;
			if (basePV.isMine)
			{
				sync.PhotonCamera = true;
				basePV.RPC("SetMyPhotonCamera", PhotonTargets.OthersBuffered, PlayerPrefs.GetFloat("cameraDistance") + 0.3f);
				for (int i = 0; i < 10; i++)
				{
					DashRampage.Add(new Bullet());
				}
				EventManager.CallEvent("load_custom_audio");
			}
			else
			{
				if (basePV.owner.customProperties["RCteam"] != null)
				{
					int num = RCextensions.returnIntFromObject(basePV.owner.customProperties["RCteam"]);
					if (num != 1)
					{
						_ = 2;
					}
				}
				if (RCextensions.returnIntFromObject(basePV.owner.customProperties["team"]) == 2 && GameSettings.bombMode == 0)
				{
					component.text = "[FF0000]AHSS\n[FFFFFF]";
				}
			}
			if (GameSettings.teamMode == 0)
			{
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add("name", LoginFengKAI.player.name);
				hashtable.Add("guildName", ((int)FengGameManagerMKII.settings[359] == 0) ? FengGameManagerMKII.guildField : FengGameManagerMKII.guildField2);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
			base.photonView.owner.BombHasExploded = false;
			string text = RCextensions.returnStringFromObject(base.photonView.owner.customProperties["name"]);
			string text2 = RCextensions.returnStringFromObject(base.photonView.owner.customProperties["guildName"]) + "[-]";
			string text3 = text.StripHex();
			string text4 = RCextensions.returnStringFromObject(base.photonView.owner.customProperties["guildName"]).StripHex();
			int num2 = RCextensions.returnIntFromObject(base.photonView.owner.customProperties["RCteam"]);
			int num3 = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]);
			if (FengGameManagerMKII.instance.MinCharLength > FengGameManagerMKII.instance.MaxCharLength)
			{
				FengGameManagerMKII.instance.MinCharL = "3";
				FengGameManagerMKII.instance.MaxCharL = "12";
			}
			if (FengGameManagerMKII.instance.GLMinCharLength > FengGameManagerMKII.instance.GLMaxCharLength)
			{
				FengGameManagerMKII.instance.GLMinCharL = "3";
				FengGameManagerMKII.instance.GLMaxCharL = "12";
			}
			if (text4.Length > 20)
			{
				if (GameSettings.teamMode == 0)
				{
					text2 = "long guild (" + base.photonView.owner.ID + ")";
				}
				else if (GameSettings.teamMode > 0 && num2 == 1)
				{
					text2 = "[00FFFF]long guild (" + base.photonView.owner.ID + ")";
				}
				else if (GameSettings.teamMode > 0 && num2 == 2)
				{
					text2 = "[FA8072]long guild (" + base.photonView.owner.ID + ")";
				}
			}
			if (GameSettings.bombMode > 0 && GameSettings.teamMode > 0 && GameSettings.HideTeam > 0 && num3 == num2)
			{
				text = "";
				text2 = "";
			}
			if (GameSettings.teamMode > 0)
			{
				text = ThymesUtils.RemoveAllColorCodes(text);
				text2 = ThymesUtils.RemoveAllColorCodes(text2);
				if (num3 == 0)
				{
					switch (num2)
					{
					case 1:
						text = "[00FFFF]" + text;
						text2 = "[00FFFF]" + text2;
						break;
					case 2:
						text = "[FF00FF]" + text;
						text2 = "[FF00FF]" + text2;
						break;
					}
				}
				else if (num3 != num2)
				{
					if (num2 != 0)
					{
						text = FengGameManagerMKII.ETeamHex() + text;
						text2 = FengGameManagerMKII.ETeamHex() + text2;
					}
					else
					{
						text = ThymesUtils.RemoveAllColorCodes(text);
						text = "[ffffff]" + text;
						text2 = ThymesUtils.RemoveAllColorCodes(text2);
						text2 = "[ffffff]" + text2;
					}
				}
				else
				{
					text = FengGameManagerMKII.TeamHex() + text;
					text2 = FengGameManagerMKII.TeamHex() + text2;
				}
			}
			else if (GameSettings.teamMode == 0 && GameSettings.whiteNames > 0)
			{
				text = ThymesUtils.RemoveAllColorCodes(text);
				text = "[ffffff]" + text;
			}
			text = text.Replace("  ", "abcd");
			text = text.Replace("\t", "abcd");
			if (text3.Length < FengGameManagerMKII.instance.MinCharLength)
			{
				for (int j = 0; j < FengGameManagerMKII.instance.MinCharLength - text3.Length; j++)
				{
					text += "#";
				}
			}
			else if (text3.Length > FengGameManagerMKII.instance.MaxCharLength)
			{
				text = ThymesUtils.TruncateTextWithHex(text, FengGameManagerMKII.instance.MaxCharLength);
			}
			if (text4.Length < FengGameManagerMKII.instance.GLMinCharLength)
			{
				for (int k = 0; k < FengGameManagerMKII.instance.GLMinCharLength - text4.Length; k++)
				{
					text2 += "#";
				}
			}
			else if (text4.Length > FengGameManagerMKII.instance.GLMaxCharLength)
			{
				text2 = ThymesUtils.TruncateTextWithHex(text2, FengGameManagerMKII.instance.GLMaxCharLength);
			}
			if (text2 != string.Empty)
			{
				UILabel component2 = myNetWorkName.GetComponent<UILabel>();
				string text5 = component2.text;
				if (base.photonView.isMine && GameSettings.hideYourName > 0)
				{
					component2.text = "";
				}
				else if (FengGameManagerMKII.instance.markList.Contains(base.photonView.owner.ID))
				{
					string[] array = new string[4]
					{
						text5,
						"[FFFF00]",
						"[FF0000][X][-]",
						"\n[-]" + text
					};
					component2.text = string.Concat(array);
				}
				else
				{
					string[] array2 = new string[4]
					{
						text5,
						"[FFFF00]",
						(GameSettings.hideGuilds > 0) ? "" : (text2 + "[-]"),
						"\n[-]" + text
					};
					component2.text = string.Concat(array2);
				}
			}
			else
			{
				UILabel component3 = myNetWorkName.GetComponent<UILabel>();
				if (base.photonView.isMine && GameSettings.hideYourName > 0)
				{
					component3.text = "";
				}
				else
				{
					component3.text += text;
				}
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine)
		{
			baseG.layer = LayerMask.NameToLayer("NetworkObject");
			if (IN_GAME_MAIN_CAMERA.dayLight == DayLight.Night)
			{
				Transform obj = ((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("flashlight"))).transform;
				obj.parent = baseT;
				obj.position = baseT.position + Vector3.up;
				obj.rotation = Quaternion.Euler(353f, 0f, 0f);
			}
			setup.init();
			setup.myCostume = new HeroCostume();
			setup.myCostume = CostumeConeveter.PhotonDataToHeroCostume2(basePV.owner);
			setup.setCharacterComponent();
			UnityEngine.Object.Destroy(checkBoxLeft);
			UnityEngine.Object.Destroy(checkBoxRight);
			UnityEngine.Object.Destroy(leftbladetrail);
			UnityEngine.Object.Destroy(rightbladetrail);
			UnityEngine.Object.Destroy(leftbladetrail2);
			UnityEngine.Object.Destroy(rightbladetrail2);
			hasspawn = true;
		}
		else
		{
			SetInterpolationIfEnabled(interpolate: true);
			triggerLeft = checkBoxLeft.GetComponent<TriggerColliderWeapon>();
			triggerRight = checkBoxRight.GetComponent<TriggerColliderWeapon>();
			loadskin();
			hasspawn = true;
			StartCoroutine(reloadSky());
		}
		bombImmune = false;
		if (GameSettings.bombMode == 1)
		{
			bombImmune = true;
			StartCoroutine(stopImmunity());
			SetupThunderSpears();
		}
		if (_needSetupThunderspears)
		{
			CreateAndAttachThunderSpears();
		}
		_hasRunStart = true;
	}

	public void SetInterpolationIfEnabled(bool interpolate)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || (base.photonView.isMine && interpolate))
		{
			if ((int)FengGameManagerMKII.settings[294] == 0)
			{
				baseR.interpolation = RigidbodyInterpolation.Interpolate;
			}
			else if ((int)FengGameManagerMKII.settings[294] == 1)
			{
				baseR.interpolation = RigidbodyInterpolation.Extrapolate;
			}
			else if ((int)FengGameManagerMKII.settings[294] == 2)
			{
				baseR.interpolation = RigidbodyInterpolation.None;
			}
		}
		else if (!interpolate)
		{
			baseR.interpolation = RigidbodyInterpolation.None;
		}
	}

	public IEnumerator stopImmunity()
	{
		yield return new WaitForSeconds(5f);
		bombImmune = false;
	}

	private HERO()
		: base(SPECIES.Hero)
	{
	}

	private void applyForceToBody(GameObject GO, Vector3 v)
	{
		Rigidbody obj = GO.rigidbody;
		obj.AddForce(v);
		obj.AddTorque(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
	}

	private void attackAccordingToMouse()
	{
		if ((double)Input.mousePosition.x < (double)Screen.width * 0.5)
		{
			attackAnimation = "attack2";
		}
		else
		{
			attackAnimation = "attack1";
		}
	}

	private void attackAccordingToTarget(Transform a)
	{
		Vector3 vector = a.position - baseT.position;
		float num = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, baseT.rotation.eulerAngles.y - 90f);
		if (Mathf.Abs(num) < 90f && vector.magnitude < 6f && a.position.y <= baseT.position.y + 2f && a.position.y >= baseT.position.y - 5f)
		{
			attackAnimation = "attack4";
		}
		else if (num > 0f)
		{
			attackAnimation = "attack1";
		}
		else
		{
			attackAnimation = "attack2";
		}
	}

	private void Awake()
	{
		cache();
		setup = baseG.GetComponent<HERO_SETUP>();
		baseR.freezeRotation = true;
		baseR.useGravity = false;
		handL = baseT.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
		handR = baseT.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
		forearmL = baseT.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
		forearmR = baseT.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
		upperarmL = baseT.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
		upperarmR = baseT.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
		CustomAnarchyLevel.Instance.AddScriptHero(this);
	}

	public void backToHuman()
	{
		sync.disabled = false;
		baseR.velocity = Vector3.zero;
		titanForm = false;
		ungrabbed();
		falseAttack();
		skillCDDuration = skillCDLast;
		IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(baseG);
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			basePV.RPC("backToHumanRPC", PhotonTargets.Others);
		}
	}

	[RPC]
	private void backToHumanRPC()
	{
		titanForm = false;
		eren_titan = null;
		sync.disabled = false;
	}

	[RPC]
	private void badGuyReleaseMe()
	{
		hookBySomeOne = false;
		badGuy = null;
	}

	[RPC]
	public void blowAway(Vector3 force, PhotonMessageInfo info = null)
	{
		if (Math.Abs(force.x) > 500f || Math.Abs(force.y) > 500f || Math.Abs(force.z) > 500f)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "blue screen");
		}
		else if (!info.sender.isLocal && !info.sender.isMasterClient && (Convert.ToBoolean(info.sender.customProperties["dead"]) || (!FengGameManagerMKII.level.StartsWith("Custom") && Convert.ToInt32(info.sender.customProperties["isTitan"]) != 2)))
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "blowaway invalid sender");
		}
		else if (((info != null && (info.sender == PhotonNetwork.player || info.sender.isMasterClient)) || (Vector3.Distance(force, new Vector3(0f, 0f, 0f)) <= 18f && Vector3.Distance(force, new Vector3(0f, 0f, 0f)) >= -18f)) && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
		{
			base.rigidbody.AddForce(force, ForceMode.Impulse);
			base.transform.LookAt(base.transform.position);
		}
	}

	private void bodyLean()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine)
		{
			return;
		}
		float z = 0f;
		needLean = false;
		if (!usingAhss && state == HERO_STATE.Attack && attackAnimation != "attack3_1" && attackAnimation != "attack3_2" && !IsFiringThunderSpear())
		{
			targetRotation = Quaternion.Euler((0f - Mathf.Atan2(baseR.velocity.y, Mathf.Sqrt(baseR.velocity.x * baseR.velocity.x + baseR.velocity.z * baseR.velocity.z)) * 57.29578f) * (1f - Vector3.Angle(baseR.velocity, baseT.forward) / 90f), facingDirection, 0f);
			if ((isLeftHandHooked && bulletLeft != null) || (isRightHandHooked && bulletRight != null))
			{
				baseT.rotation = targetRotation;
			}
			return;
		}
		if (isLeftHandHooked && bulletLeft != null && isRightHandHooked && bulletRight != null)
		{
			if (almostSingleHook)
			{
				needLean = true;
				z = getLeanAngle(bulletRT.position, left: true);
			}
		}
		else if (isLeftHandHooked && bulletLeft != null)
		{
			needLean = true;
			z = getLeanAngle(bulletLT.position, left: true);
		}
		else if (isRightHandHooked && bulletRight != null)
		{
			needLean = true;
			z = getLeanAngle(bulletRT.position, left: false);
		}
		if (needLean)
		{
			float num = 0f;
			if (!usingAhss && state != HERO_STATE.Attack)
			{
				num = currentSpeed * 0.1f;
				num = Mathf.Min(num, 20f);
			}
			targetRotation = Quaternion.Euler(0f - num, facingDirection, z);
		}
		else if (state != HERO_STATE.Attack)
		{
			targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
		}
	}

	private void bombInit()
	{
		skillIDHUD = skillId;
		skillCDDuration = skillCDLast;
		if (GameSettings.bombMode == 1)
		{
			float num = (float)FengGameManagerMKII.settings[250];
			float num2 = (float)FengGameManagerMKII.settings[251];
			float num3 = (float)FengGameManagerMKII.settings[252];
			float num4 = (float)FengGameManagerMKII.settings[253];
			if (num + num2 + num3 + num4 > 20f)
			{
				num = 5f;
				num2 = 5f;
				num3 = 5f;
				num4 = 5f;
			}
			bombRadius = BombUtil.GetBombRadius(num, 5.4f, 7.4f, 7f);
			bombSpeed = BombUtil.GetBombSpeed(num3, 3f, 10.5f, 10.5f);
			bombTimeMax = BombUtil.GetBombRange(num2, 0f, 4f, 7f) / bombSpeed;
			bombCD = BombUtil.GetBombCooldown(num4, 4f, 7f, 7f);
			float oldRadiusCost = BombUtil.GetOldRadiusCost(bombRadius);
			float oldRangeCost = BombUtil.GetOldRangeCost(BombUtil.GetBombRange(num2, 0f, 4f, 7f));
			float oldSpeedCost = BombUtil.GetOldSpeedCost(bombSpeed);
			float oldCooldownCost = BombUtil.GetOldCooldownCost(bombCD);
			base.photonView.RPC("BombStats", PhotonTargets.TLWandRRC, oldRadiusCost.ToString("0.##") + " / " + oldRangeCost.ToString("0.##") + " / " + oldSpeedCost.ToString("0.##") + " / " + oldCooldownCost.ToString("0.##") + " + " + num.ToString("0.##") + " / " + num2.ToString("0.##") + " / " + num3.ToString("0.##") + " / " + num4.ToString("0.##"));
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			if ((int)FengGameManagerMKII.settings[359] == 0)
			{
				hashtable.Add("RCBombR", (float)FengGameManagerMKII.settings[246]);
				hashtable.Add("RCBombG", (float)FengGameManagerMKII.settings[247]);
				hashtable.Add("RCBombB", (float)FengGameManagerMKII.settings[248]);
				hashtable.Add("RCBombA", (float)FengGameManagerMKII.settings[249]);
			}
			else if ((int)FengGameManagerMKII.settings[359] == 1)
			{
				hashtable.Add("RCBombR", (float)FengGameManagerMKII.settings[267]);
				hashtable.Add("RCBombG", (float)FengGameManagerMKII.settings[268]);
				hashtable.Add("RCBombB", (float)FengGameManagerMKII.settings[269]);
				hashtable.Add("RCBombA", (float)FengGameManagerMKII.settings[270]);
			}
			hashtable.Add("RCBombRadius", bombRadius);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			skillId = "bomb";
			skillIDHUD = "armin";
			skillCDLast = bombCD;
			skillCDDuration = 10f;
			if (FengGameManagerMKII.instance.roundTime > 10f)
			{
				skillCDDuration = 1f;
			}
		}
	}

	[RPC]
	private void BombStats(string fu, PhotonMessageInfo info)
	{
		if (info.sender.customProperties["name"] != null)
		{
			_ = "「" + info.sender.customProperties["name"].ToString().StripColor().StripHex() + "」";
		}
		if (BombStatsCounter.ContainsKey(info.sender))
		{
			BombStatsCounter.Remove(info.sender);
		}
		if (!BombStatsCounter.ContainsKey(info.sender))
		{
			BombStatsCounter.Add(info.sender, fu);
		}
	}

	private void breakApart(Vector3 v, bool isBite)
	{
		RecheckTransforms();
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/AOTTG_HERO_body"), baseT.position, baseT.rotation);
		gameObject.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
		gameObject.gameObject.GetComponent<HERO_SETUP>().isDeadBody = true;
		gameObject.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, baseA[currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
		if (!isBite)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/AOTTG_HERO_body"), baseT.position, baseT.rotation);
			GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/AOTTG_HERO_body"), baseT.position, baseT.rotation);
			GameObject gameObject4 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/AOTTG_HERO_body"), baseT.position, baseT.rotation);
			gameObject2.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
			gameObject3.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
			gameObject4.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
			gameObject2.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, baseA[currentAnimation].normalizedTime, BODY_PARTS.UPPER);
			gameObject3.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, baseA[currentAnimation].normalizedTime, BODY_PARTS.LOWER);
			gameObject4.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, baseA[currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
			applyForceToBody(gameObject2, v);
			applyForceToBody(gameObject3, v);
			applyForceToBody(gameObject4, v);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(gameObject2, resetRotation: false);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine)
		{
			IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(gameObject, resetRotation: false);
		}
		applyForceToBody(gameObject, v);
		GameObject gameObject5;
		GameObject gameObject6;
		GameObject gameObject7;
		GameObject gameObject8;
		GameObject gameObject9;
		if (useGun)
		{
			gameObject5 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_gun_l"), hand_L.position, hand_L.rotation);
			gameObject6 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_gun_r"), hand_R.position, hand_R.rotation);
			gameObject7 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_3dmg_2"), baseT.position, baseT.rotation);
			gameObject8 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_gun_mag_l"), baseT.position, baseT.rotation);
			gameObject9 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_gun_mag_r"), baseT.position, baseT.rotation);
		}
		else
		{
			gameObject5 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_blade_l"), hand_L.position, hand_L.rotation);
			gameObject6 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_blade_r"), hand_R.position, hand_R.rotation);
			gameObject7 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_3dmg"), baseT.position, baseT.rotation);
			gameObject8 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_3dmg_gas_l"), baseT.position, baseT.rotation);
			gameObject9 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_3dmg_gas_r"), baseT.position, baseT.rotation);
		}
		gameObject5.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
		gameObject6.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
		gameObject7.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
		gameObject8.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
		gameObject9.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
		applyForceToBody(gameObject5, v);
		applyForceToBody(gameObject6, v);
		applyForceToBody(gameObject7, v);
		applyForceToBody(gameObject8, v);
		applyForceToBody(gameObject9, v);
	}

	private void bufferUpdate()
	{
		if (!(buffTime > 0f))
		{
			return;
		}
		buffTime -= Time.deltaTime;
		if (buffTime <= 0f)
		{
			buffTime = 0f;
			if (currentBuff == BUFF.SpeedUp && baseA.IsPlaying("run_sasha"))
			{
				crossFade("run", 0.1f);
			}
			currentBuff = BUFF.NoBuff;
		}
	}

	private void cache()
	{
		CacheComponnents();
		CacheTransforms();
		sync = GetComponent<SmoothSyncMovement>();
		capsule = GetComponent<CapsuleCollider>();
		hookRefL1T = hookRefL1.transform;
		hookRefL2T = hookRefL2.transform;
		hookRefR1T = hookRefR1.transform;
		hookRefR2T = hookRefR2.transform;
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine)
		{
			return;
		}
		cross1 = CacheGameObject.Find("cross1");
		cross2 = CacheGameObject.Find("cross2");
		crossT1 = CacheGameObject.Find<Transform>("cross1");
		crossT2 = CacheGameObject.Find<Transform>("cross2");
		crossL1 = CacheGameObject.Find("crossL1");
		crossL2 = CacheGameObject.Find("crossL2");
		crossR1 = CacheGameObject.Find("crossR1");
		crossR2 = CacheGameObject.Find("crossR2");
		crossL1T = crossL1.transform;
		crossL2T = crossL2.transform;
		crossR1T = crossR1.transform;
		crossR2T = crossR2.transform;
		LabelDistance = CacheGameObject.Find<UILabel>("LabelDistance");
		labelT = LabelDistance.transform;
		flare1 = CacheGameObject.Find<UISprite>("UIflare1");
		flare2 = CacheGameObject.Find<UISprite>("UIflare2");
		flare3 = CacheGameObject.Find<UISprite>("UIflare3");
		cachedSprites = new Dictionary<string, UISprite>();
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.GetComponent<UISprite>() != null && gameObject.activeInHierarchy)
			{
				string text = gameObject.name;
				if ((text.Contains("blade") || text.Contains("bullet") || text.Contains("gas") || text.Contains("flare") || text.Contains("skill_cd")) && !cachedSprites.ContainsKey(text))
				{
					cachedSprites.Add(text, gameObject.GetComponent<UISprite>());
				}
			}
		}
		SetupCrosshairs();
	}

	private void SetupCrosshairs()
	{
		cross1.transform.localPosition = Vector3.up * 10000f;
		cross2.transform.localPosition = Vector3.up * 10000f;
		LabelDistance.transform.localPosition = Vector3.up * 10000f;
	}

	private void calcFlareCD()
	{
		if (flare1CD > 0f)
		{
			flare1CD -= Time.deltaTime;
			if (flare1CD < 0f)
			{
				flare1CD = 0f;
			}
		}
		if (flare2CD > 0f)
		{
			flare2CD -= Time.deltaTime;
			if (flare2CD < 0f)
			{
				flare2CD = 0f;
			}
		}
		if (flare3CD > 0f)
		{
			flare3CD -= Time.deltaTime;
			if (flare3CD < 0f)
			{
				flare3CD = 0f;
			}
		}
	}

	private void changeBlade()
	{
		if (useGun && !grounded && LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.PVP_AHSS)
		{
			return;
		}
		state = HERO_STATE.ChangeBlade;
		throwedBlades = false;
		if (useGun)
		{
			if (basePV.isMine && GameSettings.GunsReload > 0)
			{
				//CustomAudioManager.Play(CustomAudioManager.AudioFile.GunsReload);
			}
			if (!leftGunHasBullet && !rightGunHasBullet)
			{
				if (grounded)
				{
					reloadAnimation = "AHSS_gun_reload_both";
				}
				else
				{
					reloadAnimation = "AHSS_gun_reload_both_air";
				}
			}
			else if (!leftGunHasBullet)
			{
				if (grounded)
				{
					reloadAnimation = "AHSS_gun_reload_l";
				}
				else
				{
					reloadAnimation = "AHSS_gun_reload_l_air";
				}
			}
			else if (!rightGunHasBullet)
			{
				if (grounded)
				{
					reloadAnimation = "AHSS_gun_reload_r";
				}
				else
				{
					reloadAnimation = "AHSS_gun_reload_r_air";
				}
			}
			else
			{
				if (grounded)
				{
					reloadAnimation = "AHSS_gun_reload_both";
				}
				else
				{
					reloadAnimation = "AHSS_gun_reload_both_air";
				}
                if (!InRoomChat.god) leftGunHasBullet = (rightGunHasBullet = false);
                else leftGunHasBullet = (rightGunHasBullet = true);
            }
			crossFade(reloadAnimation, 0.05f);
		}
		else
		{
			if (basePV.isMine && GameSettings.BladesReload > 0)
			{
				//CustomAudioManager.Play(CustomAudioManager.AudioFile.BladesReload);
			}
			if (!grounded)
			{
				reloadAnimation = "changeBlade_air";
			}
			else
			{
				reloadAnimation = "changeBlade";
			}
			crossFade(reloadAnimation, 0.1f);
		}
	}

	private void checkDashDoubleTap()
	{
		if (uTapTime >= 0f)
		{
			uTapTime += Time.deltaTime;
			if (uTapTime > 0.2f)
			{
				uTapTime = -1f;
			}
		}
		if (dTapTime >= 0f)
		{
			dTapTime += Time.deltaTime;
			if (dTapTime > 0.2f)
			{
				dTapTime = -1f;
			}
		}
		if (lTapTime >= 0f)
		{
			lTapTime += Time.deltaTime;
			if (lTapTime > 0.2f)
			{
				lTapTime = -1f;
			}
		}
		if (rTapTime >= 0f)
		{
			rTapTime += Time.deltaTime;
			if (rTapTime > 0.2f)
			{
				rTapTime = -1f;
			}
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.up])
		{
			if (uTapTime == -1f)
			{
				uTapTime = 0f;
			}
			if (uTapTime != 0f)
			{
				dashU = true;
			}
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.down])
		{
			if (dTapTime == -1f)
			{
				dTapTime = 0f;
			}
			if (dTapTime != 0f)
			{
				dashD = true;
			}
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.left])
		{
			if (lTapTime == -1f)
			{
				lTapTime = 0f;
			}
			if (lTapTime != 0f)
			{
				dashL = true;
			}
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.right])
		{
			if (rTapTime == -1f)
			{
				rTapTime = 0f;
			}
			if (rTapTime != 0f)
			{
				dashR = true;
			}
		}
	}

	private void checkDashRebind()
	{
		if (FengGameManagerMKII.inputRC.isInputHumanDown(InputCodeRC.dash))
		{
			if (FengCustomInputs.Inputs.isInput[InputCode.up])
			{
				dashU = true;
			}
			else if (FengCustomInputs.Inputs.isInput[InputCode.down])
			{
				dashD = true;
			}
			else if (FengCustomInputs.Inputs.isInput[InputCode.left])
			{
				dashL = true;
			}
			else if (FengCustomInputs.Inputs.isInput[InputCode.right])
			{
				dashR = true;
			}
		}
	}

	private void checkTitan()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask layerMask = 1 << LayerMask.NameToLayer("PlayerAttackBox");
		LayerMask layerMask2 = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask3 = 1 << LayerMask.NameToLayer("EnemyBox");
		RaycastHit[] array = Physics.RaycastAll(ray, 180f, ((LayerMask)((int)layerMask | (int)layerMask2 | (int)layerMask3)).value);
		List<RaycastHit> list = new List<RaycastHit>();
		List<TITAN> list2 = new List<TITAN>();
		foreach (RaycastHit item in array)
		{
			list.Add(item);
		}
		list.Sort((RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
		float num = 180f;
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = list[i].collider.gameObject;
			if (gameObject.layer == 16)
			{
				if (!gameObject.name.Contains("PlayerDetectorRC"))
				{
					continue;
				}
				RaycastHit raycastHit;
				RaycastHit raycastHit2 = (raycastHit = list[i]);
				if (raycastHit2.distance < num)
				{
					num -= 60f;
					if (num <= 60f)
					{
						i = list.Count;
					}
					TITAN component = gameObject.transform.root.gameObject.GetComponent<TITAN>();
					if (component != null)
					{
						list2.Add(component);
					}
				}
			}
			else
			{
				i = list.Count;
			}
		}
		for (int i = 0; i < myTitans.Count; i++)
		{
			TITAN tITAN = myTitans[i];
			if (!list2.Contains(tITAN))
			{
				tITAN.isLook = false;
			}
		}
		for (int i = 0; i < list2.Count; i++)
		{
			list2[i].isLook = true;
		}
		myTitans = list2;
	}

	public void ClearPopup()
	{
		FengGameManagerMKII.instance.ShowHUDInfoCenter(string.Empty);
	}

	private void continueAnimation()
	{
		if (!_animationStopped)
		{
			return;
		}
		_animationStopped = false;
		foreach (AnimationState item in baseA)
		{
			if (item.speed == 1f)
			{
				return;
			}
			item.speed = 1f;
		}
		customAnimationSpeed();
		playAnimation(currentPlayingClipName());
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
		{
			basePV.RPC("netContinueAnimation", PhotonTargets.Others);
		}
	}

	public void crossFade(string aniName, float time)
	{
		currentAnimation = aniName;
		baseA.CrossFade(aniName, time);
		if (PhotonNetwork.connected && basePV.isMine)
		{
			object[] parameters = new object[2] { aniName, time };
			basePV.RPC("netCrossFade", PhotonTargets.Others, parameters);
		}
	}

	public string currentPlayingClipName()
	{
		foreach (AnimationState item in baseA)
		{
			if (baseA.IsPlaying(item.name))
			{
				return item.name;
			}
		}
		return string.Empty;
	}

	private void customAnimationSpeed()
	{
		baseA["attack5"].speed = 1.85f;
		baseA["changeBlade"].speed = 1.2f;
		baseA["air_release"].speed = 0.6f;
		baseA["changeBlade_air"].speed = 0.8f;
		baseA["AHSS_gun_reload_both"].speed = 0.38f;
		baseA["AHSS_gun_reload_both_air"].speed = 0.5f;
		baseA["AHSS_gun_reload_l"].speed = 0.4f;
		baseA["AHSS_gun_reload_l_air"].speed = 0.5f;
		baseA["AHSS_gun_reload_r"].speed = 0.4f;
		baseA["AHSS_gun_reload_r_air"].speed = 0.5f;
	}

	private void dash(float horizontal, float vertical)
	{
		if (dashTime <= 0f && currentGas > 0f && !isMounted && (burstCD.ElapsedMilliseconds <= 1 || burstCD.ElapsedMilliseconds >= 250))
		{
			burstCD.Reset();
			burstCD.Start();
			useGas(totalGas * 0.04f);
			facingDirection = getGlobalFacingDirection(horizontal, vertical);
			dashV = getGlobaleFacingVector3(facingDirection);
			originVM = currentSpeed;
			Quaternion rotation = Quaternion.Euler(0f, facingDirection, 0f);
			baseR.rotation = rotation;
			targetRotation = rotation;
			//if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			//{
			//	UnityEngine.Object.Instantiate(Resources.Load("FX/boost_smoke"), base.transform.position, base.transform.rotation);
			//}
			//else
			//{
			//	PhotonNetwork.Instantiate("FX/boost_smoke", base.transform.position, base.transform.rotation, 0);
			//}
			dashTime = 0.5f;
			if (GameSettings.BackflipBurst == 0)
			{
				crossFade("dash", 0.1f);
				baseA["dash"].time = 0.1f;
			}
			else
			{
				crossFade("air_release", 0.1f);
				baseA["air_release"].time = 0.1f;
			}
			state = HERO_STATE.AirDodge;
			falseAttack();
			baseR.AddForce(dashV * 40f, ForceMode.VelocityChange);
			if (GameSettings.GasBurst > 0)
			{
				//CustomAudioManager.Play(CustomAudioManager.AudioFile.GasBurst);
			}
		}
	}

	public void die(Vector3 v, bool isBite)
    {
        if (invincible <= 0f)
		{
			if (titanForm && eren_titan != null)
			{
				eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (bulletLeft != null)
			{
				LBullet.removeMe();
			}
			if (bulletRight != null)
			{
				RBullet.removeMe();
			}
			//meatDie.Play();
			if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine) && !useGun)
			{
				leftbladetrail.Deactivate();
				rightbladetrail.Deactivate();
				leftbladetrail2.Deactivate();
				rightbladetrail2.Deactivate();
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatConsoleFeed("Titan", "You", 0);
			}
			breakApart(v, isBite);
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
			FengGameManagerMKII.instance.gameLose2();
			if (InRoomChat.god)
			{
				FengGameManagerMKII.instance.needChooseSide = true;
				FengGameManagerMKII.justSuicide = false;
			}
            falseAttack();
			hasDied = true;
			//Transform obj = baseT.Find("audio_die");
			//obj.parent = null;
			//obj.GetComponent<AudioSource>().Play();
			if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.startSnapShot2(baseT.position, 0, null, 0.02f);
			}
			UnityEngine.Object.Destroy(baseG);
		}
	}

	public void die2(Transform tf)
    {
        if (invincible <= 0f)
		{
			if (titanForm && eren_titan != null)
			{
				eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (bulletLeft != null)
			{
				LBullet.removeMe();
			}
			if (bulletRight != null)
			{
				RBullet.removeMe();
			}
			//Transform obj = baseT.Find("audio_die");
			//obj.parent = null;
			//obj.GetComponent<AudioSource>().Play();
			//meatDie.Play();
			IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatConsoleFeed("Titan", "You", 0);
			}
			FengGameManagerMKII.instance.gameLose2();
			if (InRoomChat.god)
			{
				FengGameManagerMKII.instance.needChooseSide = true;
				FengGameManagerMKII.justSuicide = false;
			}
            falseAttack();
			hasDied = true;
			((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("hitMeat2"))).transform.position = baseT.position;
			UnityEngine.Object.Destroy(baseG);
		}
	}

	private void dodge2(bool offTheWall = false)
	{
		if (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseMount) && !(myHorse == null) && !isMounted && !(Vector3.Distance(myHorse.transform.position, baseT.position) >= 15f))
		{
			return;
		}
		state = HERO_STATE.GroundDodge;
		if (!offTheWall)
		{
			float num = (FengCustomInputs.Inputs.isInput[InputCode.up] ? 1f : ((!FengCustomInputs.Inputs.isInput[InputCode.down]) ? 0f : (-1f)));
			float num2 = (FengCustomInputs.Inputs.isInput[InputCode.left] ? (-1f) : ((!FengCustomInputs.Inputs.isInput[InputCode.right]) ? 0f : 1f));
			float globalFacingDirection = getGlobalFacingDirection(num2, num);
			if (num2 != 0f || num != 0f)
			{
				facingDirection = globalFacingDirection + 180f;
				targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
			}
			crossFade("dodge", 0.1f);
		}
		else
		{
			playAnimation("dodge");
			playAnimationAt("dodge", 0.2f);
		}
		sparks.enableEmission = false;
	}

	private void erenTransform()
	{
		skillCDDuration = skillCDLast;
		if (bulletLeft != null)
		{
			LBullet.removeMe();
		}
		if (bulletRight != null)
		{
			RBullet.removeMe();
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			eren_titan = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("TITAN_EREN"), baseT.position, baseT.rotation);
		}
		else
		{
			eren_titan = PhotonNetwork.Instantiate("TITAN_EREN", baseT.position, baseT.rotation, 0);
		}
		eren_titan.GetComponent<TITAN_EREN>().realBody = baseG;
		IN_GAME_MAIN_CAMERA.mainCamera.flashBlind();
		IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(eren_titan);
		eren_titan.GetComponent<TITAN_EREN>().born();
		eren_titan.rigidbody.velocity = baseR.velocity;
		baseR.velocity = Vector3.zero;
		baseT.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
		titanForm = true;
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			object[] parameters = new object[1] { eren_titan.GetPhotonView().viewID };
			basePV.RPC("whoIsMyErenTitan", PhotonTargets.Others, parameters);
		}
		if (smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
		{
			object[] parameters2 = new object[1] { false };
			basePV.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters2);
		}
		smoke_3dmg.enableEmission = false;
	}

	private void escapeFromGrab()
	{
	}

	private void falseAttack()
	{
		attackMove = false;
		if (useGun)
		{
			if (!attackReleased)
			{
				continueAnimation();
				attackReleased = true;
			}
			return;
		}
		if (base.isLocal)
		{
			triggerLeft.active_me = false;
			triggerRight.active_me = false;
			triggerLeft.clearHits();
			triggerRight.clearHits();
			leftbladetrail.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
			rightbladetrail.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
			leftbladetrail2.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
			rightbladetrail2.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
		}
		attackLoop = 0;
		if (!attackReleased)
		{
			continueAnimation();
			attackReleased = true;
		}
	}

	public void fillGas()
	{
		currentGas = totalGas;
	}

	private GameObject findNearestTitan()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		GameObject result = null;
		float num = float.PositiveInfinity;
		Vector3 position = baseT.position;
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			float sqrMagnitude = (gameObject.transform.position - position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				result = gameObject;
				num = sqrMagnitude;
			}
		}
		return result;
	}

	private void OnCollisionEnter()
	{
		if (LevelInfo.getInfo(FengGameManagerMKII.level).name.Contains("Forest V") && (base.isLocal || (!base.isLocal && PhotonNetwork.isMasterClient)) && Mathf.Abs(currentSpeed - base.rigidbody.velocity.magnitude) >= 50f && !HasDied())
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				die(base.rigidbody.velocity * 60f, isBite: false);
				return;
			}
			markDie();
			base.photonView.RPC("netDie", PhotonTargets.All, base.rigidbody.velocity * 60f, false, -1, "[000000]Collision", false);
		}
	}

	private void FixedUpdate()
	{
		if (titanForm || isCannon || (IN_GAME_MAIN_CAMERA.isPausing && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE))
		{
			return;
		}
		currentSpeed = baseR.velocity.magnitude;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine)
		{
			if (state == HERO_STATE.Attack && usingAhss)
			{
				baseT.rotation = Quaternion.Lerp(baseT.rotation, gunDummy.transform.rotation, Time.deltaTime * 30f);
			}
			if (!baseA.IsPlaying("attack3_2") && !baseA.IsPlaying("attack5") && !baseA.IsPlaying("special_petra"))
			{
				baseR.rotation = Quaternion.Lerp(baseG.transform.rotation, targetRotation, Time.deltaTime * 6f);
			}
			if (state == HERO_STATE.Grab)
			{
				baseR.AddForce(-baseR.velocity, ForceMode.VelocityChange);
			}
			else
			{
				if (IsGrounded())
				{
					if (!grounded)
					{
						justGrounded = true;
					}
					grounded = true;
				}
				else
				{
					grounded = false;
				}
				if (hookSomeOne)
				{
					if (hookTarget != null)
					{
						Vector3 vector = hookTarget.transform.position - baseT.position;
						float magnitude = vector.magnitude;
						if (magnitude > 2f)
						{
							baseR.AddForce(vector.normalized * Mathf.Pow(magnitude, 0.15f) * 30f - baseR.velocity * 0.95f, ForceMode.VelocityChange);
						}
					}
					else
					{
						hookSomeOne = false;
					}
				}
				else if (hookBySomeOne && badGuy != null)
				{
					if (badGuy != null)
					{
						Vector3 vector2 = badGuy.transform.position - baseT.position;
						float magnitude2 = vector2.magnitude;
						if (magnitude2 > 5f)
						{
							baseR.AddForce(vector2.normalized * Mathf.Pow(magnitude2, 0.15f) * 0.2f, ForceMode.Impulse);
						}
					}
					else
					{
						hookBySomeOne = false;
					}
				}
				float num = 0f;
				float num2 = 0f;
				if (!IN_GAME_MAIN_CAMERA.isTyping)
				{
					num2 = (FengCustomInputs.Inputs.isInput[InputCode.up] ? 1f : ((!FengCustomInputs.Inputs.isInput[InputCode.down]) ? 0f : (-1f)));
					num = (FengCustomInputs.Inputs.isInput[InputCode.left] ? (-1f) : ((!FengCustomInputs.Inputs.isInput[InputCode.right]) ? 0f : 1f));
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				isLeftHandHooked = false;
				isRightHandHooked = false;
				if (isLaunchLeft)
				{
					if (bulletLeft != null && LBullet.isHooked())
					{
						isLeftHandHooked = true;
						Vector3 vector3 = bulletLT.position - baseT.position;
						vector3.Normalize();
						vector3 *= 10f;
						if (!isLaunchRight)
						{
							vector3 *= 2f;
						}
						if (Vector3.Angle(baseR.velocity, vector3) > 90f && FengCustomInputs.Inputs.isInput[InputCode.jump])
						{
							flag2 = true;
							flag = true;
						}
						if (!flag2)
						{
							baseR.AddForce(vector3);
							if (Vector3.Angle(baseR.velocity, vector3) > 90f)
							{
								baseR.AddForce(-baseR.velocity * 2f, ForceMode.Acceleration);
							}
						}
					}
					launchElapsedTimeL += Time.deltaTime;
					if (QHold && currentGas > 0f)
					{
						useGas(useGasSpeed * Time.deltaTime);
					}
					else if (launchElapsedTimeL > 0.3f)
					{
						isLaunchLeft = false;
						if (bulletLeft != null)
						{
							LBullet.disable();
							releaseIfIHookSb();
							bulletLeft = null;
							bulletLT = null;
							LBullet = null;
							flag2 = false;
						}
					}
				}
				if (isLaunchRight)
				{
					if (bulletRight != null && RBullet.isHooked())
					{
						isRightHandHooked = true;
						Vector3 vector4 = bulletRT.position - baseT.position;
						vector4.Normalize();
						vector4 *= 10f;
						if (!isLaunchLeft)
						{
							vector4 *= 2f;
						}
						if (Vector3.Angle(baseR.velocity, vector4) > 90f && FengCustomInputs.Inputs.isInput[InputCode.jump])
						{
							flag3 = true;
							flag = true;
						}
						if (!flag3)
						{
							baseR.AddForce(vector4);
							if (Vector3.Angle(baseR.velocity, vector4) > 90f)
							{
								baseR.AddForce(-baseR.velocity * 2f, ForceMode.Acceleration);
							}
						}
					}
					launchElapsedTimeR += Time.deltaTime;
					if (EHold && currentGas > 0f)
					{
						useGas(useGasSpeed * Time.deltaTime);
					}
					else if (launchElapsedTimeR > 0.3f)
					{
						isLaunchRight = false;
						if (bulletRight != null)
						{
							RBullet.disable();
							releaseIfIHookSb();
							bulletRight = null;
							bulletRT = null;
							RBullet = null;
							flag3 = false;
						}
					}
				}
				if (grounded)
				{
					Vector3 vector5 = Vector3.zero;
					if (state == HERO_STATE.Attack)
					{
						if (attackAnimation == "attack5")
						{
							if (baseA[attackAnimation].normalizedTime > 0.4f && baseA[attackAnimation].normalizedTime < 0.61f)
							{
								baseR.AddForce(baseG.transform.forward * 200f);
							}
						}
						else if (attackAnimation == "special_petra")
						{
							if (baseA[attackAnimation].normalizedTime > 0.35f && baseA[attackAnimation].normalizedTime < 0.48f)
							{
								baseR.AddForce(baseG.transform.forward * 200f);
							}
						}
						else if (baseA.IsPlaying("attack3_2"))
						{
							vector5 = Vector3.zero;
						}
						else if (baseA.IsPlaying("attack1") || baseA.IsPlaying("attack2"))
						{
							baseR.AddForce(baseG.transform.forward * 200f);
						}
						if (baseA.IsPlaying("attack3_2"))
						{
							vector5 = Vector3.zero;
						}
					}
					if (justGrounded)
					{
						if (state != HERO_STATE.Attack || (attackAnimation != "attack3_1" && attackAnimation != "attack5" && attackAnimation != "special_petra"))
						{
							if (state != HERO_STATE.Attack && num == 0f && num2 == 0f && bulletLeft == null && bulletRight == null && state != HERO_STATE.FillGas)
							{
								state = HERO_STATE.Land;
								crossFade("dash_land", 0.01f);
							}
							else
							{
								buttonAttackRelease = true;
								if (state != HERO_STATE.Attack && baseR.velocity.x * baseR.velocity.x + baseR.velocity.z * baseR.velocity.z > speed * speed * 1.5f && state != HERO_STATE.FillGas)
								{
									state = HERO_STATE.Slide;
									crossFade("slide", 0.05f);
									facingDirection = Mathf.Atan2(baseR.velocity.x, baseR.velocity.z) * 57.29578f;
									targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
									sparks.enableEmission = true;
								}
							}
						}
						justGrounded = false;
						vector5 = baseR.velocity;
					}
					if (state == HERO_STATE.Attack && attackAnimation == "attack3_1" && baseA[attackAnimation].normalizedTime >= 1f)
					{
						playAnimation("attack3_2");
						resetAnimationSpeed();
						Vector3 zero = Vector3.zero;
						baseR.velocity = zero;
						vector5 = zero;
						IN_GAME_MAIN_CAMERA.mainCamera.startShake(0.2f, 0.3f);
					}
					if (state == HERO_STATE.GroundDodge)
					{
						if (baseA["dodge"].normalizedTime >= 0.2f && baseA["dodge"].normalizedTime < 0.8f)
						{
							vector5 = -baseT.forward * 2.4f * speed;
						}
						if (baseA["dodge"].normalizedTime > 0.8f)
						{
							vector5 = baseR.velocity * 0.9f;
						}
					}
					else if (state == HERO_STATE.Idle)
					{
						Vector3 vector6 = new Vector3(num, 0f, num2);
						float num3 = getGlobalFacingDirection(num, num2);
						vector5 = getGlobaleFacingVector3(num3);
						float num4 = ((!(vector6.magnitude <= 0.95f)) ? 1f : ((vector6.magnitude >= 0.25f) ? vector6.magnitude : 0f));
						vector5 *= num4;
						vector5 *= speed;
						if (buffTime > 0f && currentBuff == BUFF.SpeedUp)
						{
							vector5 *= 4f;
						}
						if (num != 0f || num2 != 0f)
						{
							//if (basePV.isMine && GameSettings.Walk > 0 && !CustomAudioManager.Walk.isPlaying && baseA.IsPlaying("run"))
							//{
							//	//CustomAudioManager.Play(CustomAudioManager.AudioFile.Walk);
							//}
							if (!baseA.IsPlaying("run") && !baseA.IsPlaying("jump") && !baseA.IsPlaying("run_sasha") && (!baseA.IsPlaying("horse_geton") || baseA["horse_geton"].normalizedTime >= 0.5f))
							{
								if (buffTime > 0f && currentBuff == BUFF.SpeedUp)
								{
									crossFade("run_sasha", 0.1f);
								}
								else
								{
									crossFade("run", 0.1f);
								}
							}
						}
						else
						{
							if (!baseA.IsPlaying(standAnimation) && state != HERO_STATE.Land && !baseA.IsPlaying("jump") && !baseA.IsPlaying("horse_geton") && !baseA.IsPlaying("grabbed"))
							{
								crossFade(standAnimation, 0.1f);
								vector5 *= 0f;
							}
							num3 = -874f;
						}
						if (num3 != -874f)
						{
							facingDirection = num3;
							targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
						}
					}
					else if (state == HERO_STATE.Land)
					{
						vector5 = baseR.velocity * 0.96f;
					}
					else if (state == HERO_STATE.Slide)
					{
						vector5 = baseR.velocity * 0.99f;
						if (currentSpeed < speed * 1.2f)
						{
							idle();
							sparks.enableEmission = false;
						}
					}
					Vector3 velocity = baseR.velocity;
					Vector3 force = vector5 - velocity;
					force.x = Mathf.Clamp(force.x, 0f - maxVelocityChange, maxVelocityChange);
					force.z = Mathf.Clamp(force.z, 0f - maxVelocityChange, maxVelocityChange);
					force.y = 0f;
					if (baseA.IsPlaying("jump") && baseA["jump"].normalizedTime > 0.18f)
					{
						force.y += 8f;
					}
					if (baseA.IsPlaying("horse_geton") && baseA["horse_geton"].normalizedTime > 0.18f && baseA["horse_geton"].normalizedTime < 1f)
					{
						float num5 = 6f;
						force = -baseR.velocity;
						force.y = num5;
						float num6 = Vector3.Distance(myHorse.transform.position, baseT.position);
						float num7 = 0.6f * gravity * num6 / (2f * num5);
						force += num7 * (myHorse.transform.position - baseT.position).normalized;
					}
					if (state != HERO_STATE.Attack || !usingAhss)
					{
						baseR.AddForce(force, ForceMode.VelocityChange);
						baseR.rotation = Quaternion.Lerp(baseG.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
					}
				}
				else
				{
					if (sparks.enableEmission)
					{
						sparks.enableEmission = false;
					}
					if (myHorse != null && (baseA.IsPlaying("horse_geton") || baseA.IsPlaying("air_fall")) && baseR.velocity.y < 0f && Vector3.Distance(myHorse.transform.position + Vector3.up * 1.65f, baseT.position) < 0.5f)
					{
						baseT.position = myHorse.transform.position + Vector3.up * 1.65f;
						baseT.rotation = myHorse.transform.rotation;
						isMounted = true;
						if (!base.animation.IsPlaying("horse_idle"))
						{
							crossFade("horse_idle", 0.1f);
						}
						myHorse.GetComponent<Horse>().mounted();
					}
					if ((state == HERO_STATE.Idle && !baseA.IsPlaying("dash") && !baseA.IsPlaying("wallrun") && !baseA.IsPlaying("toRoof") && !baseA.IsPlaying("horse_geton") && !baseA.IsPlaying("horse_getoff") && !baseA.IsPlaying("air_release") && !isMounted && (!baseA.IsPlaying("air_hook_l_just") || baseA["air_hook_l_just"].normalizedTime >= 1f) && (!baseA.IsPlaying("air_hook_r_just") || baseA["air_hook_r_just"].normalizedTime >= 1f)) || baseA["dash"].normalizedTime >= 0.99f)
					{
						if (!isLeftHandHooked && !isRightHandHooked && (baseA.IsPlaying("air_hook_l") || baseA.IsPlaying("air_hook_r") || baseA.IsPlaying("air_hook")) && baseR.velocity.y > 20f)
						{
							baseA.CrossFade("air_release");
						}
						else
						{
							bool num8 = Mathf.Abs(baseR.velocity.x) + Mathf.Abs(baseR.velocity.z) > 25f;
							bool flag4 = baseR.velocity.y < 0f;
							if (!num8)
							{
								if (flag4)
								{
									if (!baseA.IsPlaying("air_fall"))
									{
										crossFade("air_fall", 0.2f);
									}
								}
								else if (!baseA.IsPlaying("air_rise"))
								{
									crossFade("air_rise", 0.2f);
								}
							}
							else if (!isLeftHandHooked && !isRightHandHooked)
							{
								float num9 = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(baseR.velocity.z, baseR.velocity.x)) * 57.29578f, baseT.rotation.eulerAngles.y - 90f);
								if (Mathf.Abs(num9) < 45f)
								{
									if (!baseA.IsPlaying("air2"))
									{
										crossFade("air2", 0.2f);
									}
								}
								else if (num9 < 135f && num9 > 0f)
								{
									if (!baseA.IsPlaying("air2_right"))
									{
										crossFade("air2_right", 0.2f);
									}
								}
								else if (num9 > -135f && num9 < 0f)
								{
									if (!baseA.IsPlaying("air2_left"))
									{
										crossFade("air2_left", 0.2f);
									}
								}
								else if (!baseA.IsPlaying("air2_backward"))
								{
									crossFade("air2_backward", 0.2f);
								}
							}
							else if (useGun || AhssAnimation)
							{
								if (!isRightHandHooked)
								{
									if (!baseA.IsPlaying("AHSS_hook_forward_l"))
									{
										crossFade("AHSS_hook_forward_l", 0.1f);
									}
								}
								else if (!isLeftHandHooked)
								{
									if (!baseA.IsPlaying("AHSS_hook_forward_r"))
									{
										crossFade("AHSS_hook_forward_r", 0.1f);
									}
								}
								else if (!baseA.IsPlaying("AHSS_hook_forward_both"))
								{
									crossFade("AHSS_hook_forward_both", 0.1f);
								}
							}
							else if (!isRightHandHooked)
							{
								if (!baseA.IsPlaying("air_hook_l"))
								{
									crossFade("air_hook_l", 0.1f);
								}
							}
							else if (!isLeftHandHooked)
							{
								if (!baseA.IsPlaying("air_hook_r"))
								{
									crossFade("air_hook_r", 0.1f);
								}
							}
							else if (!baseA.IsPlaying("air_hook"))
							{
								crossFade("air_hook", 0.1f);
							}
						}
					}
					if (!baseA.IsPlaying("air_rise"))
					{
						if (state == HERO_STATE.Idle && baseA.IsPlaying("air_release") && baseA["air_release"].normalizedTime >= 1f)
						{
							crossFade("air_rise", 0.2f);
						}
						if (baseA.IsPlaying("horse_getoff") && baseA["horse_getoff"].normalizedTime >= 1f)
						{
							crossFade("air_rise", 0.2f);
						}
					}
					if (baseA.IsPlaying("toRoof"))
					{
						if (baseA["toRoof"].normalizedTime < 0.22f)
						{
							baseR.velocity = Vector3.zero;
							baseR.AddForce(new Vector3(0f, gravity * baseR.mass, 0f));
						}
						else
						{
							if (!wallJump)
							{
								wallJump = true;
								baseR.AddForce(Vector3.up * 8f, ForceMode.Impulse);
							}
							baseR.AddForce(baseT.forward * 0.05f, ForceMode.Impulse);
						}
						if (baseA["toRoof"].normalizedTime >= 1f)
						{
							playAnimation("air_rise");
						}
					}
					else if (state == HERO_STATE.Idle && isPressDirectionTowardsHero(num, num2) && !FengCustomInputs.Inputs.isInput[InputCode.jump] && !FengCustomInputs.Inputs.isInput[InputCode.leftRope] && !FengCustomInputs.Inputs.isInput[InputCode.rightRope] && !FengCustomInputs.Inputs.isInput[InputCode.bothRope] && IsFrontGrounded() && !baseA.IsPlaying("wallrun") && !baseA.IsPlaying("dodge"))
					{
						crossFade("wallrun", 0.1f);
						wallRunTime = 0f;
					}
					else if (baseA.IsPlaying("wallrun"))
					{
						//if (basePV.isMine && GameSettings.Walk > 0 && !CustomAudioManager.Walk.isPlaying)
						//{
						//	//CustomAudioManager.Play(CustomAudioManager.AudioFile.Walk);
						//}
						baseR.AddForce(Vector3.up * speed - baseR.velocity, ForceMode.VelocityChange);
						wallRunTime += Time.deltaTime;
						if (wallRunTime > 1f || (num2 == 0f && num == 0f))
						{
							baseR.AddForce(-baseT.forward * speed * 0.75f, ForceMode.Impulse);
							dodge2(offTheWall: true);
						}
						else if (!IsUpFrontGrounded())
						{
							wallJump = false;
							crossFade("toRoof", 0.1f);
						}
						else if (!IsFrontGrounded())
						{
							crossFade("air_fall", 0.1f);
						}
					}
					else if (!baseA.IsPlaying("attack5") && !baseA.IsPlaying("special_petra") && !baseA.IsPlaying("dash") && !baseA.IsPlaying("jump") && !IsFiringThunderSpear())
					{
						Vector3 vector7 = new Vector3(num, 0f, num2);
						float num10 = getGlobalFacingDirection(num, num2);
						Vector3 globaleFacingVector = getGlobaleFacingVector3(num10);
						float num11 = ((!(vector7.magnitude <= 0.95f)) ? 1f : ((vector7.magnitude >= 0.25f) ? vector7.magnitude : 0f));
						globaleFacingVector *= num11;
						globaleFacingVector *= (float)setup.myCostume.stat.ACL * 1f / 10f * 2f;
						if (num == 0f && num2 == 0f)
						{
							if (state == HERO_STATE.Attack)
							{
								globaleFacingVector *= 0f;
							}
							num10 = -874f;
						}
						if (num10 != -874f)
						{
							facingDirection = num10;
							targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
						}
						if (!flag2 && !flag3 && !isMounted && FengCustomInputs.Inputs.isInput[InputCode.jump] && currentGas > 0f)
						{
							if (num != 0f || num2 != 0f)
							{
								baseR.AddForce(globaleFacingVector, ForceMode.Acceleration);
							}
							else
							{
								baseR.AddForce(baseT.forward * globaleFacingVector.magnitude, ForceMode.Acceleration);
							}
							flag = true;
						}
					}
					if (baseA.IsPlaying("air_fall") && currentSpeed < 0.2f && IsFrontGrounded())
					{
						crossFade("onWall", 0.3f);
					}
				}
				Vector3 vector8 = new Vector3(0f, 0f, 0f);
				int num12 = 0;
				if (flag2 || flag3)
				{
					if (flag2)
					{
						num12++;
						vector8 += bulletLeft.transform.position;
					}
					if (flag3)
					{
						num12++;
						vector8 += bulletRight.transform.position;
					}
					float num13 = currentSpeed + 0.1f;
					baseR.AddForce(-baseR.velocity, ForceMode.VelocityChange);
					Vector3 current = vector8 / num12 - baseT.position;
					float value = 0f;
					float axis = Input.GetAxis("Mouse ScrollWheel");
					if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelout))
					{
						value = 1f;
					}
					if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelin))
					{
						if ((int)FengGameManagerMKII.settings[280] == 0)
						{
							if (!AlreadyReeled)
							{
								value = -1f;
								AlreadyReeled = true;
							}
						}
						else
						{
							value = -1f;
						}
					}
					if (((string)FengGameManagerMKII.settings[98] == "Scroll Up" && axis > 0f) || ((string)FengGameManagerMKII.settings[98] == "Scroll Down" && axis < 0f))
					{
						value = Input.GetAxis("Mouse ScrollWheel") * 5555f;
					}
					value = 1f + Mathf.Clamp(value, -0.8f, 0.8f);
					Vector3 vector9 = Vector3.RotateTowards(current, baseR.velocity, 1.53938f * value, 1.53938f * value);
					vector9.Normalize();
					baseR.velocity = vector9 * num13;
				}
				if (state == HERO_STATE.Attack && (attackAnimation == "attack5" || attackAnimation == "special_petra") && baseA[attackAnimation].normalizedTime > 0.4f && !attackMove)
				{
					attackMove = true;
					if (launchPointRight.magnitude > 0f)
					{
						Vector3 force2 = launchPointRight - baseT.position;
						force2.Normalize();
						force2 *= 13f;
						baseR.AddForce(force2, ForceMode.Impulse);
					}
					if (attackAnimation == "special_petra" && launchPointLeft.magnitude > 0f)
					{
						Vector3 force3 = launchPointLeft - baseT.position;
						force3.Normalize();
						force3 *= 13f;
						baseR.AddForce(force3, ForceMode.Impulse);
						if (bulletRight != null)
						{
							RBullet.disable();
							releaseIfIHookSb();
						}
						if (bulletLeft != null)
						{
							LBullet.disable();
							releaseIfIHookSb();
						}
					}
					baseR.AddForce(Vector3.up * 2f, ForceMode.Impulse);
				}
				bool flag5 = false;
				if (bulletLeft != null || bulletRight != null)
				{
					if (bulletLeft != null && bulletLT.position.y > baseT.position.y && isLaunchLeft && LBullet.isHooked())
					{
						flag5 = true;
					}
					if (bulletRight != null && bulletRT.position.y > baseT.position.y && isLaunchRight && RBullet.isHooked())
					{
						flag5 = true;
					}
				}
				if (flag5)
				{
					baseR.AddForce(new Vector3(0f, -10f * baseR.mass, 0f));
				}
				else
				{
					baseR.AddForce(new Vector3(0f, (0f - gravity) * baseR.mass, 0f));
				}
				if (currentSpeed > 10f)
				{
					Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Min(100f, currentSpeed + 40f), 0.1f);
				}
				else
				{
					Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50f, 0.1f);
				}
				if (!_cancelGasDisable)
				{
					bool flag6 = ((GameSettings.yourGas > 0) ? flag : flag);
					bool flag7 = ((GameSettings.yourGas > 0) ? smoke_3dmg.enableEmission : EnableSmoke);
					if (flag6 != flag7)
					{
						if (IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
						{
							object[] parameters = new object[1] { flag6 };
							basePV.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
						}
						if (GameSettings.yourGas > 0)
						{
							useGas(useGasSpeed * Time.deltaTime);
							smoke_3dmg.enableEmission = flag6;
						}
						else
						{
							useGas(useGasSpeed * Time.deltaTime);
							EnableSmoke = flag6;
						}
					}
				}
				else
				{
					_cancelGasDisable = false;
				}
				if (currentSpeed > 80f)
				{
					if (!speedFXPS.enableEmission)
					{
						speedFXPS.enableEmission = true;
					}
					speedFXPS.startSpeed = currentSpeed;
					speedFX.transform.LookAt(baseT.position + baseR.velocity);
				}
				else if (speedFXPS.enableEmission)
				{
					speedFXPS.enableEmission = false;
				}
			}
			if (GameSettings.UpsideDown == 1 && !baseA.IsPlaying("supply") && !Input.GetKey(KeyCode.LeftControl))
			{
				baseT.rotation = new Quaternion(-180f, baseT.rotation.y, baseT.rotation.z, baseT.rotation.w);
			}
		}
		setHookedPplDirection();
		bodyLean();
	}

	private Vector3 getGlobaleFacingVector3(float resultAngle)
	{
		float num = 0f - resultAngle + 90f;
		return new Vector3(Mathf.Cos(num * 0.01745329f), 0f, Mathf.Sin(num * 0.01745329f));
	}

	private Vector3 getGlobaleFacingVector3(float horizontal, float vertical)
	{
		float num = 0f - getGlobalFacingDirection(horizontal, vertical) + 90f;
		return new Vector3(Mathf.Cos(num * 0.01745329f), 0f, Mathf.Sin(num * 0.01745329f));
	}

	private float getGlobalFacingDirection(float horizontal, float vertical)
	{
		if (vertical == 0f && horizontal == 0f)
		{
			return baseT.rotation.eulerAngles.y;
		}
		float y = IN_GAME_MAIN_CAMERA.mainT.rotation.eulerAngles.y;
		float num = Mathf.Atan2(vertical, horizontal) * 57.29578f;
		num = 0f - num + 90f;
		return y + num;
	}

	private float getLeanAngle(Vector3 p, bool left)
	{
		if (!useGun && state == HERO_STATE.Attack && AhssAnimation)
		{
			return 0f;
		}
		float num = p.y - baseT.position.y;
		float num2 = Vector3.Distance(p, baseT.position);
		float num3 = Mathf.Acos(num / num2) * 57.29578f;
		num3 *= 0.1f;
		num3 *= 1f + Mathf.Pow(baseR.velocity.magnitude, 0.2f);
		Vector3 vector = p - baseT.position;
		float current = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
		float target = Mathf.Atan2(baseR.velocity.x, baseR.velocity.z) * 57.29578f;
		float num4 = Mathf.DeltaAngle(current, target);
		num3 += Mathf.Abs(num4 * 0.5f);
		if (state != HERO_STATE.Attack)
		{
			num3 = Mathf.Min(num3, 80f);
		}
		if (num4 > 0f)
		{
			leanLeft = true;
		}
		else
		{
			leanLeft = false;
		}
		if (useGun || AhssAnimation)
		{
			return num3 * ((num4 >= 0f) ? 1f : (-1f));
		}
		float num5 = 0f;
		num5 = (((!left || !(num4 < 0f)) && (left || !(num4 > 0f))) ? 0.5f : 0.1f);
		return num3 * ((num4 >= 0f) ? num5 : (0f - num5));
	}

	private void getOffHorse()
	{
		playAnimation("horse_getoff");
		baseR.AddForce(Vector3.up * 10f - baseT.forward * 2f - baseT.right * 1f, ForceMode.VelocityChange);
		unmounted();
	}

	private void getOnHorse()
	{
		playAnimation("horse_geton");
		facingDirection = myHorse.transform.rotation.eulerAngles.y;
		targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
	}

	public void getSupply()
	{
		if ((baseA.IsPlaying(standAnimation) || baseA.IsPlaying("run") || baseA.IsPlaying("run_sasha")) && (currentBladeSta != totalBladeSta || currentBladeNum != totalBladeNum || currentGas != totalGas || leftBulletLeft != bulletMAX || rightBulletLeft != bulletMAX))
		{
			if (GameSettings.Refill > 0 && basePV.isMine)
			{
				//CustomAudioManager.Play(CustomAudioManager.AudioFile.Refill);
			}
			state = HERO_STATE.FillGas;
			crossFade("supply", 0.1f);
		}
	}

	public void grabbed(GameObject titan, bool leftHand)
	{
		if (isMounted)
		{
			unmounted();
		}
		state = HERO_STATE.Grab;
		GetComponent<CapsuleCollider>().isTrigger = true;
		falseAttack();
		titanWhoGrabMe = titan;
		if (titanForm && eren_titan != null)
		{
			eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
		}
		if (!useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine))
		{
			leftbladetrail.Deactivate();
			rightbladetrail.Deactivate();
			leftbladetrail2.Deactivate();
			rightbladetrail2.Deactivate();
		}
		smoke_3dmg.enableEmission = false;
		sparks.enableEmission = false;
	}

	public bool HasDied()
	{
		if (!hasDied)
		{
			return isInvincible();
		}
		return true;
	}

	private void headMovement()
	{
		float x = Mathf.Sqrt((gunTarget.x - baseT.position.x) * (gunTarget.x - baseT.position.x) + (gunTarget.z - baseT.position.z) * (gunTarget.z - baseT.position.z));
		targetHeadRotation = head.rotation;
		Vector3 vector = gunTarget - baseT.position;
		float value = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, baseT.rotation.eulerAngles.y - 90f);
		value = Mathf.Clamp(value, -40f, 40f);
		float value2 = Mathf.Atan2(neck.position.y - gunTarget.y, x) * 57.29578f;
		value2 = Mathf.Clamp(value2, -40f, 30f);
		targetHeadRotation = Quaternion.Euler(head.rotation.eulerAngles.x + value2, head.rotation.eulerAngles.y + value, head.rotation.eulerAngles.z);
		oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 60f);
		head.rotation = oldHeadRotation;
	}

	private void hookedByHuman(int hooker, Vector3 hookPosition)
	{
		object[] parameters = new object[2] { hooker, hookPosition };
		basePV.RPC("RPCHookedByHuman", basePV.owner, parameters);
	}

	[RPC]
	private void hookFail()
	{
		hookTarget = null;
		hookSomeOne = false;
	}

	public void hookToHuman(GameObject target, Vector3 hookPosition, bool dash = false)
	{
		releaseIfIHookSb();
		hookTarget = target;
		hookSomeOne = true;
		HERO component = target.GetComponent<HERO>();
		if (component != null)
		{
			component.hookedByHuman(basePV.viewID, hookPosition);
		}
		launchForce = hookPosition - baseT.position;
		float num = Mathf.Pow(launchForce.magnitude, 0.1f);
		if (grounded)
		{
			baseR.AddForce(Vector3.up * Mathf.Min(launchForce.magnitude * 0.2f, 10f), ForceMode.Impulse);
		}
		baseR.AddForce(launchForce * num * 0.1f, ForceMode.Impulse);
	}

	private void idle()
	{
		if (state == HERO_STATE.Attack)
		{
			falseAttack();
		}
		state = HERO_STATE.Idle;
		crossFade(standAnimation, 0.1f);
	}

	private bool IsFrontGrounded()
	{
		return Physics.Raycast(baseT.position + baseT.up * 1f, baseT.forward, 1f, Layer.GroundEnemy.value);
	}

	public bool IsGrounded()
	{
		return Physics.Raycast(baseT.position + Vector3.up * 0.1f, -Vector3.up, 0.3f, Layer.GroundEnemy.value);
	}

	public bool isInvincible()
	{
		return invincible > 0f;
	}

	private bool isPressDirectionTowardsHero(float h, float v)
	{
		if (h == 0f && v == 0f)
		{
			return false;
		}
		return Mathf.Abs(Mathf.DeltaAngle(getGlobalFacingDirection(h, v), baseT.rotation.eulerAngles.y)) < 45f;
	}

	private bool IsUpFrontGrounded()
	{
		return Physics.Raycast(baseT.position + baseT.up * 3f, baseT.forward, 1.2f, Layer.GroundEnemy.value);
	}

	[RPC]
	private void killObject()
	{
	}

	public void lateUpdate()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && myNetWorkNameT != null)
		{
			if (titanForm && eren_titan != null)
			{
				myNetWorkNameT.localPosition = Vector3.up * Screen.height * 2f;
			}
			Vector3 vector = new Vector3(baseT.position.x, baseT.position.y + 2f, baseT.position.z);
			LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
			LayerMask layerMask2 = (int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask;
			if (Vector3.Angle(IN_GAME_MAIN_CAMERA.mainT.forward, vector - IN_GAME_MAIN_CAMERA.mainT.position) > 90f || Physics.Linecast(vector, IN_GAME_MAIN_CAMERA.mainT.position, layerMask2))
			{
				myNetWorkNameT.localPosition = Vector3.up * Screen.height * 2f;
			}
			else
			{
				Vector2 vector2 = Camera.main.WorldToScreenPoint(vector);
				myNetWorkNameT.localPosition = new Vector3((int)(vector2.x - (float)Screen.width * 0.5f), (int)(vector2.y - (float)Screen.height * 0.5f), 0f);
			}
		}
		if (titanForm || isCannon)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.cameraTilt == 1 && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine))
		{
			Vector3 vector3 = Vector3.zero;
			Vector3 vector4 = Vector3.zero;
			if (isLaunchLeft && bulletLeft != null && LBullet.isHooked())
			{
				vector3 = bulletLT.position;
			}
			if (isLaunchRight && bulletRight != null && RBullet.isHooked())
			{
				vector4 = bulletRT.position;
			}
			Vector3 vector5 = Vector3.zero;
			if (!grounded)
			{
				if (vector3.magnitude != 0f && vector4.magnitude == 0f)
				{
					vector5 = vector3;
				}
				else if (vector3.magnitude == 0f && vector4.magnitude != 0f)
				{
					vector5 = vector4;
				}
				else if (vector3.magnitude != 0f && vector4.magnitude != 0f)
				{
					vector5 = (vector3 + vector4) * 0.5f;
				}
			}
			Vector3 vector6 = Vector3.Project(vector5 - baseT.position, IN_GAME_MAIN_CAMERA.mainT.up);
			Vector3 vector7 = Vector3.Project(vector5 - baseT.position, IN_GAME_MAIN_CAMERA.mainT.right);
			Quaternion to2;
			if (vector5.magnitude > 0f)
			{
				Vector3 to = vector6 + vector7;
				float num = Vector3.Angle(vector5 - baseT.position, baseR.velocity) * 0.001f * FengGameManagerMKII.instance.tiltSlider;
				to2 = Quaternion.Euler(z: ((IN_GAME_MAIN_CAMERA.mainT.right + vector7.normalized).magnitude >= 1f) ? ((0f - Vector3.Angle(vector6, to)) * num) : (Vector3.Angle(vector6, to) * num), x: IN_GAME_MAIN_CAMERA.mainT.rotation.eulerAngles.x, y: IN_GAME_MAIN_CAMERA.mainT.rotation.eulerAngles.y);
			}
			else
			{
				to2 = Quaternion.Euler(IN_GAME_MAIN_CAMERA.mainT.rotation.eulerAngles.x, IN_GAME_MAIN_CAMERA.mainT.rotation.eulerAngles.y, 0f);
			}
			IN_GAME_MAIN_CAMERA.mainT.rotation = Quaternion.Lerp(IN_GAME_MAIN_CAMERA.mainT.rotation, to2, Time.deltaTime * 2f);
		}
		if (state == HERO_STATE.Grab && titanWhoGrabMe != null)
		{
			if (titanWhoGrabMe.GetComponent<TITAN>() != null)
			{
				baseT.position = titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.position;
				baseT.rotation = titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.rotation;
			}
			else if (titanWhoGrabMe.GetComponent<FEMALE_TITAN>() != null)
			{
				baseT.position = titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
				baseT.rotation = titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
			}
		}
		if (!useGun)
		{
			return;
		}
		if (leftArmAim || rightArmAim)
		{
			Vector3 vector8 = gunTarget - baseT.position;
			float num2 = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector8.z, vector8.x)) * 57.29578f, baseT.rotation.eulerAngles.y - 90f);
			headMovement();
			if (!isLeftHandHooked && leftArmAim && num2 < 40f && num2 > -90f)
			{
				leftArmAimTo(gunTarget);
			}
			if (!isRightHandHooked && rightArmAim && num2 > -40f && num2 < 90f)
			{
				rightArmAimTo(gunTarget);
			}
		}
		else if (!grounded)
		{
			handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
			handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
		}
		if (isLeftHandHooked && bulletLeft != null)
		{
			leftArmAimTo(bulletLT.position);
		}
		if (isRightHandHooked && bulletRight != null)
		{
			rightArmAimTo(bulletRT.position);
		}
	}

	public void launch(Vector3 des, bool left = true, bool leviMode = false)
	{
		if (isMounted)
		{
			unmounted();
		}
		if (state != HERO_STATE.Attack)
		{
			idle();
		}
		Vector3 vector = des - baseT.position;
		if (left)
		{
			launchPointLeft = des;
		}
		else
		{
			launchPointRight = des;
		}
		vector.Normalize();
		vector *= 20f;
		if (bulletLeft != null && bulletRight != null && LBullet.isHooked() && RBullet.isHooked())
		{
			vector *= 0.8f;
		}
		leviMode = ((baseA.IsPlaying("attack5") || baseA.IsPlaying("special_petra")) ? true : false);
		if (!leviMode)
		{
			falseAttack();
			idle();
			if (useGun || AhssAnimation)
			{
				crossFade("AHSS_hook_forward_both", 0.1f);
			}
			else if (left && !isRightHandHooked)
			{
				crossFade("air_hook_l_just", 0.1f);
			}
			else if (!left && !isLeftHandHooked)
			{
				crossFade("air_hook_r_just", 0.1f);
			}
			else
			{
				crossFade("dash", 0.1f);
				baseA["dash"].time = 0f;
			}
		}
		if (left)
		{
			isLaunchLeft = true;
		}
		if (!left)
		{
			isLaunchRight = true;
		}
		launchForce = vector;
		if (!leviMode)
		{
			if (vector.y < 30f)
			{
				launchForce += Vector3.up * (30f - vector.y);
			}
			if (des.y >= baseT.position.y)
			{
				launchForce += Vector3.up * (des.y - baseT.position.y) * 10f;
			}
			baseR.AddForce(launchForce);
		}
		facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * 57.29578f;
		Quaternion rotation = Quaternion.Euler(0f, facingDirection, 0f);
		baseT.rotation = rotation;
		baseR.rotation = rotation;
		targetRotation = rotation;
		if (left)
		{
			launchElapsedTimeL = 0f;
		}
		else
		{
			launchElapsedTimeR = 0f;
		}
		if (leviMode)
		{
			launchElapsedTimeR = -100f;
		}
		if (baseA.IsPlaying("special_petra"))
		{
			launchElapsedTimeR = -100f;
			launchElapsedTimeL = -100f;
			if (bulletRight != null)
			{
				RBullet.disable();
				releaseIfIHookSb();
			}
			if (bulletLeft != null)
			{
				LBullet.disable();
				releaseIfIHookSb();
			}
		}
		_cancelGasDisable = true;
		sparks.enableEmission = false;
	}

	private void launchLeftRope(RaycastHit hit, bool single, int mode)
	{
		if (currentGas != 0f && !BeingCarried)
		{
			useGas(0f);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				LBullet = (bulletLeft = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("hook"))).GetComponent<Bullet>();
				bulletLT = bulletLeft.transform;
			}
			else if (basePV.isMine)
			{
				LBullet = (bulletLeft = PhotonNetwork.Instantiate("hook", baseT.position, baseT.rotation, 0)).GetComponent<Bullet>();
				bulletLT = bulletLeft.transform;
			}
			bulletLT.position = ((!useGun && !AhssAnimation) ? hookRefL1T : hookRefL2T).position;
			float num = (single ? 0f : ((hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f)));
			Vector3 vector = hit.point - baseT.right * num - bulletLT.position;
			vector.Normalize();
			LBullet.launch(vector * 3f, baseR.velocity, (!useGun && !AhssAnimation) ? "hookRefL1" : "hookRefL2", isLeft: true, baseG, mode == 1);
			launchPointLeft = Vector3.zero;
		}
	}

	private void launchRightRope(RaycastHit hit, bool single, int mode = 0)
	{
		if (currentGas != 0f && !BeingCarried)
		{
			useGas(0f);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				RBullet = (bulletRight = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("hook"))).GetComponent<Bullet>();
				bulletRT = bulletRight.transform;
			}
			else if (basePV.isMine)
			{
				RBullet = (bulletRight = PhotonNetwork.Instantiate("hook", baseT.position, baseT.rotation, 0)).GetComponent<Bullet>();
				bulletRT = bulletRight.transform;
			}
			bulletRT.position = ((!useGun && !AhssAnimation) ? hookRefR1T : hookRefR2T).position;
			float num = (single ? 0f : ((hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f)));
			Vector3 vector = hit.point + baseT.right * num - bulletRT.position;
			vector.Normalize();
			if (mode == 1)
			{
				RBullet.launch(vector * 5f, baseR.velocity, (!useGun && !AhssAnimation) ? "hookRefR1" : "hookRefR2", isLeft: false, baseG, leviMode: true);
			}
			else
			{
				RBullet.launch(vector * 3f, baseR.velocity, (!useGun && !AhssAnimation) ? "hookRefR1" : "hookRefR2", isLeft: false, baseG);
			}
			launchPointRight = Vector3.zero;
		}
	}

	private void leftArmAimTo(Vector3 target)
	{
		float num = target.x - upperarmL.transform.position.x;
		float y = target.y - upperarmL.transform.position.y;
		float num2 = target.z - upperarmL.transform.position.z;
		float x = Mathf.Sqrt(num * num + num2 * num2);
		handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
		forearmL.localRotation = Quaternion.Euler(-90f, 0f, 0f);
		upperarmL.rotation = Quaternion.Euler(0f, 90f + Mathf.Atan2(num, num2) * 57.29578f, (0f - Mathf.Atan2(y, x)) * 57.29578f);
	}

	private void loadskin()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine)
		{
			return;
		}
		if ((int)FengGameManagerMKII.settings[93] == 0)
		{
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer.name.Contains("speed"))
				{
					renderer.enabled = false;
				}
			}
		}
		if ((int)FengGameManagerMKII.settings[0] <= 0 && (int)FengGameManagerMKII.settings[185] <= 0)
		{
			return;
		}
		int num = 14;
		int num2 = 4;
		int num3 = 5;
		int num4 = 6;
		int num5 = 7;
		int num6 = 8;
		int num7 = 9;
		int num8 = 10;
		int num9 = 11;
		int num10 = 12;
		int num11 = 13;
		int num12 = 3;
		int num13 = 94;
		int num14 = 316;
		int num15 = 317;
		if ((int)FengGameManagerMKII.settings[133] == 1)
		{
			num12 = 134;
			num2 = 135;
			num3 = 136;
			num4 = 137;
			num5 = 138;
			num6 = 139;
			num7 = 140;
			num8 = 141;
			num9 = 142;
			num10 = 143;
			num11 = 144;
			num = 145;
			num13 = 146;
			num14 = 318;
			num15 = 319;
		}
		else if ((int)FengGameManagerMKII.settings[133] == 2)
		{
			num12 = 147;
			num2 = 148;
			num3 = 149;
			num4 = 150;
			num5 = 151;
			num6 = 152;
			num7 = 153;
			num8 = 154;
			num9 = 155;
			num10 = 156;
			num11 = 157;
			num = 158;
			num13 = 159;
			num14 = 320;
			num15 = 321;
		}
		else if ((int)FengGameManagerMKII.settings[133] == 3)
		{
			num12 = 303;
			num2 = 304;
			num3 = 305;
			num4 = 306;
			num5 = 307;
			num6 = 308;
			num7 = 309;
			num8 = 310;
			num9 = 311;
			num10 = 312;
			num11 = 313;
			num = 314;
			num13 = 315;
			num14 = 322;
			num15 = 323;
		}
		string text = (string)FengGameManagerMKII.settings[num];
		string text2 = (string)FengGameManagerMKII.settings[num2];
		string text3 = (string)FengGameManagerMKII.settings[num3];
		string text4 = (string)FengGameManagerMKII.settings[num4];
		string text5 = (string)FengGameManagerMKII.settings[num5];
		string text6 = (string)FengGameManagerMKII.settings[num6];
		string text7 = (string)FengGameManagerMKII.settings[num7];
		string text8 = (string)FengGameManagerMKII.settings[num8];
		string text9 = (string)FengGameManagerMKII.settings[num9];
		string text10 = (string)FengGameManagerMKII.settings[num10];
		string text11 = (string)FengGameManagerMKII.settings[num11];
		string text12 = (string)FengGameManagerMKII.settings[num12];
		string text13 = (string)FengGameManagerMKII.settings[num13];
		string text14 = (string)FengGameManagerMKII.settings[num14];
		string text15 = (string)FengGameManagerMKII.settings[num15];
		string text16 = text12 + "," + text2 + "," + text3 + "," + text4 + "," + text5 + "," + text6 + "," + text7 + "," + text8 + "," + text9 + "," + text10 + "," + text11 + "," + text + "," + text13 + "," + text14 + "," + text15;

		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if ((int)FengGameManagerMKII.settings[0] != 0)
			{
				StartCoroutine(loadskinE(-1, text16, null));
			}
		}
		else if ((int)FengGameManagerMKII.settings[185] == 1)
		{
			int num16 = -1;
			if (myHorse != null)
			{
				num16 = myHorse.GetPhotonView().viewID;
			}
			basePV.RPC("loadskinRPC", PhotonTargets.AllBuffered, num16, text16);
		}
		else
		{
			int num17 = -1;
			if (myHorse != null)
			{
				num17 = myHorse.GetPhotonView().viewID;
			}
			basePV.RPC("loadskinRPC", PhotonTargets.Local, num17, text16);
		}
	}

	public IEnumerator loadskinE(int horse, string url, PhotonPlayer sender)
	{
		while (!hasspawn)
		{
			yield return null;
		}
		bool iteratorVariable1 = false;
		bool mipmap = true;
		if ((int)FengGameManagerMKII.settings[63] == 1)
		{
			mipmap = false;
		}
		string[] iteratorVariable2 = url.Split(',');
		bool iteratorVariable3 = false;
		if ((int)FengGameManagerMKII.settings[15] == 1)
		{
			iteratorVariable3 = true;
		}
		bool iteratorVariable4 = false;
		if (LevelInfo.getInfo(FengGameManagerMKII.level).horse || GameSettings.horseMode == 1)
		{
			iteratorVariable4 = true;
		}
		bool iteratorVariable5 = false;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
		{
			iteratorVariable5 = true;
		}
		if (ThunderSpearLModel != null)
		{
			Renderer renderer7 = ThunderSpearLModel.renderer;
			if (iteratorVariable2[13].EndsWith(".jpg") || iteratorVariable2[13].EndsWith(".png") || iteratorVariable2[13].EndsWith(".jpeg"))
			{
				if (RCextensions.CheckIP(iteratorVariable2[13]))
				{
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[13]))
					{
						WWW link18 = new WWW(iteratorVariable2[13]);
						yield return link18;
						Texture2D mainTexture = RCextensions.loadimage(link18, mipmap, 1000000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[13]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[13], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[13]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[13]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[13]];
					}
				}
			}
			else if (iteratorVariable2[13].ToLower() == "transparent")
			{
				renderer7.enabled = false;
			}
		}
		if (ThunderSpearRModel != null)
		{
			Renderer renderer7 = ThunderSpearRModel.renderer;
			if (iteratorVariable2[14].EndsWith(".jpg") || iteratorVariable2[14].EndsWith(".png") || iteratorVariable2[14].EndsWith(".jpeg"))
			{
				if (RCextensions.CheckIP(iteratorVariable2[14]))
				{
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[14]))
					{
						WWW link18 = new WWW(iteratorVariable2[14]);
						yield return link18;
						Texture2D mainTexture2 = RCextensions.loadimage(link18, mipmap, 1000000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[14]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture2;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[14], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[14]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[14]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[14]];
					}
				}
			}
			else if (iteratorVariable2[14].ToLower() == "transparent")
			{
				renderer7.enabled = false;
			}
		}
		if (setup.part_hair_1 != null)
		{
			Renderer renderer7 = setup.part_hair_1.renderer;
			if (iteratorVariable2[1].EndsWith(".jpg") || iteratorVariable2[1].EndsWith(".png") || iteratorVariable2[1].EndsWith(".jpeg"))
			{
				if (RCextensions.CheckIP(iteratorVariable2[1]))
				{
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
					{
						WWW link18 = new WWW(iteratorVariable2[1]);
						yield return link18;
						Texture2D mainTexture3 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
						{
							iteratorVariable1 = true;
							if (setup.myCostume.hairInfo.id >= 0)
							{
								renderer7.material = CharacterMaterials.materials[setup.myCostume.hairInfo.texture];
							}
							renderer7.material.mainTexture = mainTexture3;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[1], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
					}
				}
			}
			else if (iteratorVariable2[1].ToLower() == "transparent")
			{
				renderer7.enabled = false;
			}
		}
		if (setup.part_cape != null)
		{
			Renderer renderer7 = setup.part_cape.renderer;
			if (iteratorVariable2[7].EndsWith(".jpg") || iteratorVariable2[7].EndsWith(".png") || iteratorVariable2[7].EndsWith(".jpeg"))
			{
				if (RCextensions.CheckIP(iteratorVariable2[7]))
				{
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
					{
						WWW link18 = new WWW(iteratorVariable2[7]);
						yield return link18;
						Texture2D mainTexture4 = RCextensions.loadimage(link18, mipmap, 300000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture4;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[7], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
					}
				}
			}
			else if (iteratorVariable2[7].ToLower() == "transparent")
			{
				renderer7.enabled = false;
			}
		}
		if (setup.part_chest_3 != null)
		{
			Renderer renderer7 = setup.part_chest_3.renderer;
			if (iteratorVariable2[6].EndsWith(".jpg") || iteratorVariable2[6].EndsWith(".png") || iteratorVariable2[6].EndsWith(".jpeg"))
			{
				if (RCextensions.CheckIP(iteratorVariable2[6]))
				{
					if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
					{
						WWW link18 = new WWW(iteratorVariable2[6]);
						yield return link18;
						Texture2D mainTexture5 = RCextensions.loadimage(link18, mipmap, 500000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture5;
							FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[6], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
					}
				}
			}
			else if (iteratorVariable2[6].ToLower() == "transparent")
			{
				renderer7.enabled = false;
			}
		}
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer7 in componentsInChildren)
		{
			if (renderer7.name.Contains(FengGameManagerMKII.s[1]))
			{
				if (iteratorVariable2[1].EndsWith(".jpg") || iteratorVariable2[1].EndsWith(".png") || iteratorVariable2[1].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[1]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
					{
						WWW link18 = new WWW(iteratorVariable2[1]);
						yield return link18;
						Texture2D mainTexture6 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
						{
							iteratorVariable1 = true;
							if (setup.myCostume.hairInfo.id >= 0)
							{
								renderer7.material = CharacterMaterials.materials[setup.myCostume.hairInfo.texture];
							}
							renderer7.material.mainTexture = mainTexture6;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[1], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
					}
				}
				else if (iteratorVariable2[1].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name.Contains(FengGameManagerMKII.s[2]))
			{
				if (iteratorVariable2[2].EndsWith(".jpg") || iteratorVariable2[2].EndsWith(".png") || iteratorVariable2[2].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[2]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
					{
						WWW link18 = new WWW(iteratorVariable2[2]);
						yield return link18;
						Texture2D mainTexture7 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTextureScale = renderer7.material.mainTextureScale * 8f;
							renderer7.material.mainTextureOffset = new Vector2(0f, 0f);
							renderer7.material.mainTexture = mainTexture7;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[2], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
					}
				}
				else if (iteratorVariable2[2].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name.Contains(FengGameManagerMKII.s[3]))
			{
				if (iteratorVariable2[3].EndsWith(".jpg") || iteratorVariable2[3].EndsWith(".png") || iteratorVariable2[3].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[3]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
					{
						WWW link18 = new WWW(iteratorVariable2[3]);
						yield return link18;
						Texture2D mainTexture8 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTextureScale = renderer7.material.mainTextureScale * 8f;
							renderer7.material.mainTextureOffset = new Vector2(0f, 0f);
							renderer7.material.mainTexture = mainTexture8;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[3], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
					}
				}
				else if (iteratorVariable2[3].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name.Contains(FengGameManagerMKII.s[4]))
			{
				if (iteratorVariable2[4].EndsWith(".jpg") || iteratorVariable2[4].EndsWith(".png") || iteratorVariable2[4].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[4]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
					{
						WWW link18 = new WWW(iteratorVariable2[4]);
						yield return link18;
						Texture2D mainTexture9 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTextureScale = renderer7.material.mainTextureScale * 8f;
							renderer7.material.mainTextureOffset = new Vector2(0f, 0f);
							renderer7.material.mainTexture = mainTexture9;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[4], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
					}
				}
				else if (iteratorVariable2[4].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name.Contains(FengGameManagerMKII.s[5]) || renderer7.name.Contains(FengGameManagerMKII.s[6]) || renderer7.name.Contains(FengGameManagerMKII.s[10]))
			{
				if (iteratorVariable2[5].EndsWith(".jpg") || iteratorVariable2[5].EndsWith(".png") || iteratorVariable2[5].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[5]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
					{
						WWW link18 = new WWW(iteratorVariable2[5]);
						yield return link18;
						Texture2D mainTexture10 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture10;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[5], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
					}
				}
				else if (iteratorVariable2[5].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name.Contains(FengGameManagerMKII.s[7]) || renderer7.name.Contains(FengGameManagerMKII.s[8]) || renderer7.name.Contains(FengGameManagerMKII.s[9]) || renderer7.name.Contains(FengGameManagerMKII.s[24]))
			{
				if (iteratorVariable2[6].EndsWith(".jpg") || iteratorVariable2[6].EndsWith(".png") || iteratorVariable2[6].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[6]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
					{
						WWW link18 = new WWW(iteratorVariable2[6]);
						yield return link18;
						Texture2D mainTexture11 = RCextensions.loadimage(link18, mipmap, 500000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture11;
							FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[6], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
					}
				}
				else if (iteratorVariable2[6].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name.Contains(FengGameManagerMKII.s[11]) || renderer7.name.Contains(FengGameManagerMKII.s[12]))
			{
				if (iteratorVariable2[7].EndsWith(".jpg") || iteratorVariable2[7].EndsWith(".png") || iteratorVariable2[7].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[7]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
					{
						WWW link18 = new WWW(iteratorVariable2[7]);
						yield return link18;
						Texture2D mainTexture12 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture12;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[7], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
					}
				}
				else if (iteratorVariable2[7].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name.Contains(FengGameManagerMKII.s[15]) || ((renderer7.name.Contains(FengGameManagerMKII.s[13]) || renderer7.name.Contains(FengGameManagerMKII.s[26])) && !renderer7.name.Contains("_r")))
			{
				if (iteratorVariable2[8].EndsWith(".jpg") || iteratorVariable2[8].EndsWith(".png") || iteratorVariable2[8].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[8]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
					{
						WWW link18 = new WWW(iteratorVariable2[8]);
						yield return link18;
						Texture2D mainTexture13 = RCextensions.loadimage(link18, mipmap, 500000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture13;
							FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[8], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
					}
				}
				else if (iteratorVariable2[8].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name.Contains(FengGameManagerMKII.s[17]) || renderer7.name.Contains(FengGameManagerMKII.s[16]) || (renderer7.name.Contains(FengGameManagerMKII.s[26]) && renderer7.name.Contains("_r")))
			{
				if (iteratorVariable2[9].EndsWith(".jpg") || iteratorVariable2[9].EndsWith(".png") || iteratorVariable2[9].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[9]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
					{
						WWW link18 = new WWW(iteratorVariable2[9]);
						yield return link18;
						Texture2D mainTexture14 = RCextensions.loadimage(link18, mipmap, 500000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture14;
							FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[9], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
					}
				}
				else if (iteratorVariable2[9].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else if (renderer7.name == FengGameManagerMKII.s[18] && iteratorVariable3)
			{
				if (iteratorVariable2[10].EndsWith(".jpg") || iteratorVariable2[10].EndsWith(".png") || iteratorVariable2[10].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[10]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
					{
						WWW link18 = new WWW(iteratorVariable2[10]);
						yield return link18;
						Texture2D mainTexture15 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture15;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[10], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
					}
				}
				else if (iteratorVariable2[10].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
			else
			{
				if (!renderer7.name.Contains(FengGameManagerMKII.s[25]))
				{
					continue;
				}
				if (iteratorVariable2[11].EndsWith(".jpg") || iteratorVariable2[11].EndsWith(".png") || iteratorVariable2[11].EndsWith(".jpeg"))
				{
					if (!RCextensions.CheckIP(iteratorVariable2[11]))
					{
						continue;
					}
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
					{
						WWW link18 = new WWW(iteratorVariable2[11]);
						yield return link18;
						Texture2D mainTexture16 = RCextensions.loadimage(link18, mipmap, 200000);
						link18.Dispose();
						if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
						{
							iteratorVariable1 = true;
							renderer7.material.mainTexture = mainTexture16;
							FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[11], renderer7.material);
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
						}
					}
					else
					{
						renderer7.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
					}
				}
				else if (iteratorVariable2[11].ToLower() == "transparent")
				{
					renderer7.enabled = false;
				}
			}
		}
		if (iteratorVariable4 && horse >= 0)
		{
			GameObject gameObject = PhotonView.Find(horse).gameObject;
			if (gameObject != null)
			{
				componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer7 in componentsInChildren)
				{
					if (!renderer7.name.Contains(FengGameManagerMKII.s[19]))
					{
						continue;
					}
					if (iteratorVariable2[0].EndsWith(".jpg") || iteratorVariable2[0].EndsWith(".png") || iteratorVariable2[0].EndsWith(".jpeg"))
					{
						if (!RCextensions.CheckIP(iteratorVariable2[0]))
						{
							continue;
						}
						if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
						{
							WWW link18 = new WWW(iteratorVariable2[0]);
							yield return link18;
							Texture2D mainTexture17 = RCextensions.loadimage(link18, mipmap, 500000);
							link18.Dispose();
							if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
							{
								iteratorVariable1 = true;
								renderer7.material.mainTexture = mainTexture17;
								FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[0], renderer7.material);
								renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
							}
							else
							{
								renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
							}
						}
						else
						{
							renderer7.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
						}
					}
					else if (iteratorVariable2[0].ToLower() == "transparent")
					{
						renderer7.enabled = false;
					}
				}
			}
		}
		if (!iteratorVariable5 || (!iteratorVariable2[12].EndsWith(".jpg") && !iteratorVariable2[12].EndsWith(".png") && !iteratorVariable2[12].EndsWith(".jpeg")))
		{
			yield break;
		}
		if (RCextensions.CheckIP(iteratorVariable2[12]))
		{
			if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
			{
				WWW link18 = new WWW(iteratorVariable2[12]);
				yield return link18;
				Texture2D mainTexture18 = RCextensions.loadimage(link18, mipmap, 200000);
				link18.Dispose();
				if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
				{
					iteratorVariable1 = true;
					leftbladetrail.MyMaterial.mainTexture = mainTexture18;
					rightbladetrail.MyMaterial.mainTexture = mainTexture18;
					FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[12], leftbladetrail.MyMaterial);
					leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
					rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
					leftbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
					rightbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
				}
				else
				{
					leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
					rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
					leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
					rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
				}
			}
			else
			{
				leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
				rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
				leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
				rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
			}
		}
		if (iteratorVariable1)
		{
			FengGameManagerMKII.instance.unloadAssets();
		}
	}

	[RPC]
	private void loadskinRPC(int horse, string url, PhotonMessageInfo info)
	{
		if ((int)FengGameManagerMKII.settings[0] != 0 && ((int)FengGameManagerMKII.settings[0] == 2 || info.sender.isLocal))
		{
			StartCoroutine(loadskinE(horse, url, info.sender));
		}
	}

	public void markDie()
	{
		hasDied = true;
		state = HERO_STATE.Die;
		if (!basePV.isMine)
		{
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonPlayer.Find(base.photonView.owner.ID), "<color=#FFCC00>type -rev to revive</color>", string.Empty);
        }
	}

	[RPC]
	private void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			baseT.position = new Vector3(posX, posY, posZ);
		}
	}

	[RPC]
	private void net3DMGSMOKE(bool ifON, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "Messing w/ smoke");
		}
		else if (smoke_3dmg != null && GameSettings.theirGas > 0)
		{
			smoke_3dmg.enableEmission = ifON;
		}
	}

	[RPC]
	private void netContinueAnimation(PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "netContinueAnimation");
			return;
		}
		foreach (AnimationState item in baseA)
		{
			if (item.speed == 1f)
			{
				return;
			}
			item.speed = 1f;
		}
		playAnimation(currentPlayingClipName());
	}

	[RPC]
	private void netCrossFade(string aniName, float time, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "changing animation");
			return;
		}
		currentAnimation = aniName;
		if (baseA != null)
		{
			baseA.CrossFade(aniName, time);
		}
	}

	[RPC]
	public void netDie(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true, PhotonMessageInfo info = null)
    {
        if (basePV.isMine && info != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
		{
			if (InRoomChat.god)
			{
				FengGameManagerMKII.instance.needChooseSide = true;
				FengGameManagerMKII.justSuicide = false;
			}
			if (invincible > 0f || (PhotonNetwork.player.customProperties["dead"] != null && RCextensions.returnBoolFromObject(PhotonNetwork.player.customProperties["dead"])))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "malicious netDie1");
			}
			else if (GameSettings.bombMode > 0 && !info.sender.BombHasExploded)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "kill without bomb explosion");
			}
			if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
			{
				basePV.RPC("backToHumanRPC", PhotonTargets.Others);
				return;
			}
			if (!info.sender.isLocal && !info.sender.isMasterClient)
			{
				if (info.sender.customProperties["name"] == null || info.sender.customProperties["isTitan"] == null)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "NetDie1");
					return;
				}
				if (viewID < 0)
				{
					if (titanName == "")
					{
						FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "NetDie2 (possibly valid)");
					}
					else
					{
						FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "NetDie3");
					}
					return;
				}
				if (PhotonView.Find(viewID) == null)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "NetDie4");
					return;
				}
				if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "NetDie5");
					return;
				}
			}
		}
		if (PhotonNetwork.isMasterClient)
		{
			onDeathEvent(viewID, killByTitan);
			int iD = basePV.owner.ID;
			if (FengGameManagerMKII.heroHash.ContainsKey(iD))
			{
				FengGameManagerMKII.heroHash.Remove(iD);
			}
		}
		if (basePV.isMine)
		{
			Vector3 localPosition = Vector3.up * 5000f;
			if (myBomb != null)
			{
				myBomb.DestroySelf();
			}
			if (myCannon != null)
			{
				PhotonNetwork.Destroy(myCannon);
			}
			if (titanForm && eren_titan != null)
			{
				eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (skillCD != null)
			{
				skillCD.transform.localPosition = localPosition;
			}
		}
		if (bulletLeft != null)
		{
			LBullet.removeMe();
		}
		if (bulletRight != null)
		{
			RBullet.removeMe();
		}
		//meatDie.Play();
		if (!useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine))
		{
			leftbladetrail.Deactivate();
			rightbladetrail.Deactivate();
			leftbladetrail2.Deactivate();
			rightbladetrail2.Deactivate();
		}
		falseAttack();
		breakApart(v, isBite);
		if (basePV.isMine)
		{
			IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: false);
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
			FengGameManagerMKII.instance.myRespawnTime = 0f;
		}
		hasDied = true;
		Transform transform = baseT.Find("audio_die");
		if (transform != null)
		{
			transform.parent = null;
			//transform.GetComponent<AudioSource>().Play();
		}
		sync.disabled = true;
		if (basePV.isMine)
		{
			PhotonNetwork.RemoveRPCs(basePV);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("dead", true);
			hashtable.Add("deaths",RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["deaths"]) + 1);
			PhotonNetwork.player.SetCustomProperties(hashtable); //tasw
            object[] parameters = new object[1] { (!(titanName == string.Empty)) ? 1 : 0 };
			FengGameManagerMKII.PView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
			if (viewID != -1)
			{
				PhotonView photonView = PhotonView.Find(viewID);
				if (photonView != null)
				{
					FengGameManagerMKII.instance.sendKillInfo(killByTitan, "[FFC000][" + info.sender.ID + "][FFFFFF]" + RCextensions.returnStringFromObject(photonView.owner.customProperties["name"]), t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]), 0);
					hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add("kills", RCextensions.returnIntFromObject(photonView.owner.customProperties["kills"]) + 1); //tasw
                    photonView.owner.SetCustomProperties(hashtable);
				}
			}
			else
			{
				FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), "[FFC000][" + info.sender.ID + "][FFFFFF]" + titanName, t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]), 0);
			}
		}
		if (basePV.isMine)
		{
			PhotonNetwork.Destroy(basePV);
		}
	}

	private void diefromRPC(int viewID = -1, string titanName = "", PhotonMessageInfo info = null)
	{
		if (basePV.isMine)
		{
			Vector3 localPosition = Vector3.up * 5000f;
			if (myBomb != null)
			{
				myBomb.DestroySelf();
			}
			if (myCannon != null)
			{
				PhotonNetwork.Destroy(myCannon);
			}
			PhotonNetwork.RemoveRPCs(basePV);
			if (titanForm && eren_titan != null)
			{
				eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (skillCD != null)
			{
				skillCD.transform.localPosition = localPosition;
			}
		}
		//meatDie.Play();
		if (bulletLeft != null)
		{
			LBullet.removeMe();
		}
		if (bulletRight != null)
		{
			RBullet.removeMe();
		}
		//Transform transform = baseT.Find("audio_die");
		//if (transform != null)
		//{
		//	transform.parent = null;
		//	//transform.GetComponent<AudioSource>().Play();
		//}
		if (basePV.isMine)
		{
			IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(null);
			IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: true);
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
			FengGameManagerMKII.instance.myRespawnTime = 0f;
			if (InRoomChat.god)
			{
				FengGameManagerMKII.instance.needChooseSide = true;
				FengGameManagerMKII.justSuicide = false;
			}
        }
        falseAttack();
		hasDied = true;
		sync.disabled = true;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			PhotonNetwork.RemoveRPCs(basePV);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("dead", true);
			hashtable.Add("deaths", (int)PhotonNetwork.player.customProperties["deaths"] + 1);
			PhotonNetwork.player.SetCustomProperties(hashtable); //tasw
            if (viewID != -1)
			{
				PhotonView photonView = PhotonView.Find(viewID);
				if (photonView != null)
				{
					FengGameManagerMKII.instance.sendKillInfo(t1: true, "[FFC000][" + info.sender.ID + "][FFFFFF]" + RCextensions.returnStringFromObject(photonView.owner.customProperties["name"]), t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]), 0);
					hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add("kills", RCextensions.returnIntFromObject(photonView.owner.customProperties["kills"]) + 1); //tasw
					photonView.owner.SetCustomProperties(hashtable);
				}
			}
			else
			{
				FengGameManagerMKII.instance.sendKillInfo(t1: true, "[FFC000][" + info.sender.ID + "][FFFFFF]" + titanName, t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]), 0);
			}
			object[] parameters = new object[1] { (!(titanName == string.Empty)) ? 1 : 0 };
			FengGameManagerMKII.PView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
		{
			PhotonNetwork.Instantiate("hitMeat2", baseT.position, Quaternion.Euler(270f, 0f, 0f), 0, null);
		}
		else
		{
			_ = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("hitMeat2"));
		}
		if (basePV.isMine)
		{
			PhotonNetwork.Destroy(basePV);
		}
		if (PhotonNetwork.isMasterClient)
		{
			onDeathEvent(viewID, isTitan: true);
			int iD = basePV.owner.ID;
			if (FengGameManagerMKII.heroHash.ContainsKey(iD))
			{
				FengGameManagerMKII.heroHash.Remove(iD);
			}
		}
	}

	[RPC]
	private void netDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = null)
	{
		if (basePV.isMine && info != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
        {
			if (InRoomChat.god)
			{
				FengGameManagerMKII.instance.needChooseSide = true;
				FengGameManagerMKII.justSuicide = false;
			}
            if (invincible > 0f || (GameSettings.bombMode == 0 && PhotonNetwork.player.customProperties["dead"] != null && RCextensions.returnBoolFromObject(PhotonNetwork.player.customProperties["dead"])))
			{
				FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "malicious netDie2");
			}
			else if (GameSettings.bombMode > 0 && !info.sender.BombHasExploded)
			{
				FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "kill without bomb explosion");
			}
			if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
			{
				basePV.RPC("backToHumanRPC", PhotonTargets.Others);
				return;
			}
			if (!info.sender.isLocal && !info.sender.isMasterClient)
			{
				if (info.sender.customProperties["name"] == null || info.sender.customProperties["isTitan"] == null)
				{
					FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "Unusual Kill1");
					return;
				}
				if (viewID < 0)
				{
					if (titanName == "")
					{
						FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "Unusual Kill2 (possibly valid)");
						return;
					}
					if (GameSettings.bombMode == 0 && GameSettings.deadlyCannons == 0)
					{
						FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "Unusual Kill3");
						return;
					}
				}
				else
				{
					if (PhotonView.Find(viewID) == null)
					{
						FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "Unusual Kill4");
						return;
					}
					if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
					{
						FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "Unusual Kill5");
						return;
					}
				}
			}
		}
		diefromRPC(viewID, titanName, info);
	}

	public void netDieLocal(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
	{
        if (basePV.isMine)
		{
			Vector3 localPosition = Vector3.up * 5000f;
			if (titanForm && eren_titan != null)
			{
				eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (myBomb != null)
			{
				myBomb.DestroySelf();
			}
			if (myCannon != null)
			{
				PhotonNetwork.Destroy(myCannon);
			}
			if (skillCD != null)
			{
				skillCD.transform.localPosition = localPosition;
			}
		}
		if (bulletLeft != null)
		{
			LBullet.removeMe();
		}
		if (bulletRight != null)
		{
			RBullet.removeMe();
		}
		//meatDie.Play();
		if (!useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine))
		{
			leftbladetrail.Deactivate();
			rightbladetrail.Deactivate();
			leftbladetrail2.Deactivate();
			rightbladetrail2.Deactivate();
		}
		falseAttack();
		breakApart(v, isBite);
		if (basePV.isMine)
		{
			IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: false);
			IN_GAME_MAIN_CAMERA.mainCamera.gameOver = true;
			FengGameManagerMKII.instance.myRespawnTime = 0f;
			if (InRoomChat.god)
			{
				FengGameManagerMKII.instance.needChooseSide = true;
				FengGameManagerMKII.justSuicide = false;
			}
        }
		hasDied = true;
		//Transform obj = baseT.Find("audio_die");
		//obj.parent = null;
		//obj.GetComponent<AudioSource>().Play();
		sync.disabled = true;
		if (basePV.isMine)
		{
			PhotonNetwork.RemoveRPCs(basePV);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("dead", true);
			if (!InRoomChat.god)
				hashtable.Add("deaths", RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["deaths"]) + 1);
			PhotonNetwork.player.SetCustomProperties(hashtable); //tasw
            object[] parameters = new object[1] { (!(titanName == string.Empty)) ? 1 : 0 };
			FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
			if (viewID != -1)
			{
				PhotonView photonView = PhotonView.Find(viewID);
				if (photonView != null)
				{
					FengGameManagerMKII.instance.sendKillInfo(killByTitan, RCextensions.returnStringFromObject(photonView.owner.customProperties["name"]), t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]), 0);
					hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add("kills", RCextensions.returnIntFromObject(photonView.owner.customProperties["kills"]) + 1); //tasw
					photonView.owner.SetCustomProperties(hashtable);
				}
			}
			else
			{
				FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), titanName, t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]), 0);
			}
		}
		if (basePV.isMine)
		{
			PhotonNetwork.Destroy(basePV);
		}
		if (PhotonNetwork.isMasterClient)
		{
			onDeathEvent(viewID, killByTitan);
			int iD = basePV.owner.ID;
			if (FengGameManagerMKII.heroHash.ContainsKey(iD))
			{
				FengGameManagerMKII.heroHash.Remove(iD);
			}
		}
	}

	[RPC]
	private void netGrabbed(int id, bool leftHand, PhotonMessageInfo info)
	{
		PhotonView photonView = PhotonView.Find(id);
		if (info.sender != photonView?.owner || (!(photonView.gameObject.GetComponent<TITAN>() != null) && !(photonView.gameObject.GetComponent<FEMALE_TITAN>() != null)))
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "invalid netGrabbed");
		}
		else if (info.sender.isMasterClient || info.sender.isTitan)
		{
			titanWhoGrabMeID = id;
			grabbed(PhotonView.Find(id).gameObject, leftHand);
		}
	}

	[RPC]
	private void netlaughAttack(PhotonMessageInfo info = null)
	{
		if (info.sender != basePV.owner && info.sender != null)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "making titans laugh");
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (Vector3.Distance(gameObject.transform.position, baseT.position) < 50f && Vector3.Angle(gameObject.transform.forward, baseT.position - gameObject.transform.position) < 90f && gameObject.GetComponent<TITAN>() != null)
			{
				gameObject.GetComponent<TITAN>().beLaughAttacked();
			}
		}
	}

	private void netPlayAnimation(string aniName)
	{
		currentAnimation = aniName;
		if (baseA != null)
		{
			baseA.Play(aniName);
		}
	}

	[RPC]
	private void netPauseAnimation(PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "netPauseAnimation");
			return;
		}
		foreach (AnimationState item in baseA)
		{
			item.speed = 0f;
		}
	}

	[RPC]
	private void netPlayAnimation(string aniName, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner && aniName != "grabbed")
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "false net play animation");
			return;
		}
		currentAnimation = aniName;
		if (baseA != null)
		{
			baseA.Play(aniName);
		}
	}

	[RPC]
	private void netPlayAnimationAt(string aniName, float normalizedTime, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "false net play animation");
			return;
		}
		currentAnimation = aniName;
		if (baseA != null)
		{
			baseA.Play(aniName);
			baseA[aniName].normalizedTime = normalizedTime;
		}
	}

	[RPC]
	private void netSetIsGrabbedFalse(PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "netSetIsGrabbedFalse");
		}
		else
		{
			state = HERO_STATE.Idle;
		}
	}

	[RPC]
	private void netTauntAttack(float tauntTime, float distance = 100f, PhotonMessageInfo info = null)
	{
		if (info.sender != basePV.owner && info.sender != null)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "netTauntAttack");
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (Vector3.Distance(gameObject.transform.position, baseT.position) < distance && gameObject.GetComponent<TITAN>() != null)
			{
				gameObject.GetComponent<TITAN>().beTauntedBy(baseG, tauntTime);
			}
		}
	}

	private void netTauntAttackSelf(float tauntTime, float distance = 100f)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (Vector3.Distance(gameObject.transform.position, baseT.position) < distance && gameObject.GetComponent<TITAN>() != null)
			{
				gameObject.GetComponent<TITAN>().beTauntedBy(baseG, tauntTime);
			}
		}
	}

	[RPC]
	private void netUngrabbed()
	{
		ungrabbed();
		netPlayAnimation(standAnimation);
		falseAttack();
	}

	private void onDeathEvent(int viewID, bool isTitan)
	{
		if (isTitan)
		{
			if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByTitan"))
			{
				RCEvent obj = (RCEvent)FengGameManagerMKII.RCEvents["OnPlayerDieByTitan"];
				string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnPlayerDieByTitan"];
				if (FengGameManagerMKII.playerVariables.ContainsKey(array[0]))
				{
					FengGameManagerMKII.playerVariables[array[0]] = basePV.owner;
				}
				else
				{
					FengGameManagerMKII.playerVariables.Add(array[0], basePV.owner);
				}
				if (FengGameManagerMKII.titanVariables.ContainsKey(array[1]))
				{
					FengGameManagerMKII.titanVariables[array[1]] = PhotonView.Find(viewID).gameObject.GetComponent<TITAN>();
				}
				else
				{
					FengGameManagerMKII.titanVariables.Add(array[1], PhotonView.Find(viewID).gameObject.GetComponent<TITAN>());
				}
				obj.checkEvent();
			}
		}
		else if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByPlayer"))
		{
			RCEvent obj2 = (RCEvent)FengGameManagerMKII.RCEvents["OnPlayerDieByPlayer"];
			string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnPlayerDieByPlayer"];
			if (FengGameManagerMKII.playerVariables.ContainsKey(array[0]))
			{
				FengGameManagerMKII.playerVariables[array[0]] = basePV.owner;
			}
			else
			{
				FengGameManagerMKII.playerVariables.Add(array[0], basePV.owner);
			}
			if (FengGameManagerMKII.playerVariables.ContainsKey(array[1]))
			{
				FengGameManagerMKII.playerVariables[array[1]] = PhotonView.Find(viewID).owner;
			}
			else
			{
				FengGameManagerMKII.playerVariables.Add(array[1], PhotonView.Find(viewID).owner);
			}
			obj2.checkEvent();
		}
	}

	private void OnDestroy()
	{
		CustomAnarchyLevel.Instance.RemoveHero(this);
		if (myNetWorkName != null)
		{
			UnityEngine.Object.Destroy(myNetWorkName);
		}
		if (gunDummy != null)
		{
			UnityEngine.Object.Destroy(gunDummy);
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			releaseIfIHookSb();
		}
		if (FengGameManagerMKII.instance != null)
		{
			FengGameManagerMKII.instance.removeHero(this, baseG);
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			Vector3 localPosition = Vector3.up * 5000f;
			cross1.transform.localPosition = localPosition;
			cross2.transform.localPosition = localPosition;
			crossL1.transform.localPosition = localPosition;
			crossL2.transform.localPosition = localPosition;
			crossR1.transform.localPosition = localPosition;
			crossR2.transform.localPosition = localPosition;
			LabelDistance.transform.localPosition = localPosition;
		}
		if (setup.part_cape != null)
		{
			ClothFactory.DisposeObject(setup.part_cape);
		}
		if (setup.part_hair_1 != null)
		{
			ClothFactory.DisposeObject(setup.part_hair_1);
		}
		if (setup.part_hair_2 != null)
		{
			ClothFactory.DisposeObject(setup.part_hair_2);
		}
	}

	private void pauseAnimation()
	{
		if (_animationStopped)
		{
			return;
		}
		foreach (AnimationState item in baseA)
		{
			item.speed = 0f;
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
		{
			basePV.RPC("netPauseAnimation", PhotonTargets.Others);
		}
		_animationStopped = true;
	}

	private void playAnimation(string aniName)
	{
		if (!(baseA == null) && !(baseA[aniName] == null))
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !basePV.isMine)
			{
				currentAnimation = aniName;
			}
			baseA.Play(aniName);
			if (PhotonNetwork.connected && basePV.isMine)
			{
				basePV.RPC("netPlayAnimation", PhotonTargets.Others, aniName);
			}
		}
	}

	private void playAnimationAt(string aniName, float normalizedTime)
	{
		if (!(baseA == null) && !(baseA[aniName] == null))
		{
			baseA.Play(aniName);
			baseA[aniName].normalizedTime = normalizedTime;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV != null && !basePV.isMine)
			{
				currentAnimation = aniName;
			}
			if (PhotonNetwork.connected && basePV.isMine)
			{
				object[] parameters = new object[2] { aniName, normalizedTime };
				basePV.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
			}
		}
	}

	private void releaseIfIHookSb()
	{
		if (hookSomeOne && hookTarget != null)
		{
			hookTarget.GetPhotonView().RPC("badGuyReleaseMe", hookTarget.GetPhotonView().owner);
			hookTarget = null;
			hookSomeOne = false;
		}
	}

	public IEnumerator reloadSky()
	{
		yield return new WaitForSeconds(0.5f);
		if (FengGameManagerMKII.skyMaterial != null && Camera.main.GetComponent<Skybox>().material != FengGameManagerMKII.skyMaterial)
		{
			Camera.main.GetComponent<Skybox>().material = FengGameManagerMKII.skyMaterial;
		}
	}

	private void resetAnimationSpeed()
	{
		foreach (AnimationState item in baseA)
		{
			item.speed = 1f;
		}
		customAnimationSpeed();
	}

	[RPC]
	private void ReturnFromCannon(PhotonMessageInfo info)
	{
		if (info.sender == basePV.owner)
		{
			isCannon = false;
			sync.disabled = false;
		}
	}

	private void rightArmAimTo(Vector3 target)
	{
		float num = target.x - upperarmR.transform.position.x;
		float y = target.y - upperarmR.transform.position.y;
		float num2 = target.z - upperarmR.transform.position.z;
		float x = Mathf.Sqrt(num * num + num2 * num2);
		handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
		forearmR.localRotation = Quaternion.Euler(90f, 0f, 0f);
		upperarmR.rotation = Quaternion.Euler(180f, 90f + Mathf.Atan2(num, num2) * 57.29578f, Mathf.Atan2(y, x) * 57.29578f);
	}

	[RPC]
	private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
	{
		hookBySomeOne = true;
		badGuy = PhotonView.Find(hooker).gameObject;
		if (Vector3.Distance(hookPosition, baseT.position) < 15f)
		{
			launchForce = PhotonView.Find(hooker).gameObject.transform.position - baseT.position;
			baseR.AddForce(-baseR.velocity * 0.9f, ForceMode.VelocityChange);
			float num = Mathf.Pow(launchForce.magnitude, 0.1f);
			if (grounded)
			{
				baseR.AddForce(Vector3.up * Mathf.Min(launchForce.magnitude * 0.2f, 10f), ForceMode.Impulse);
			}
			baseR.AddForce(launchForce * num * 0.1f, ForceMode.Impulse);
			if (state != HERO_STATE.Grab)
			{
				dashTime = 1f;
				crossFade("dash", 0.05f);
				baseA["dash"].time = 0.1f;
				state = HERO_STATE.AirDodge;
				falseAttack();
				facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * 57.29578f;
				Quaternion rotation = Quaternion.Euler(0f, facingDirection, 0f);
				baseG.transform.rotation = rotation;
				baseR.rotation = rotation;
				targetRotation = rotation;
			}
		}
		else
		{
			hookBySomeOne = false;
			badGuy = null;
			PhotonView.Find(hooker).RPC("hookFail", PhotonView.Find(hooker).owner);
		}
	}

	private void salute()
	{
		state = HERO_STATE.Salute;
		crossFade("salute", 0.1f);
	}

	private void setHookedPplDirection()
	{
		almostSingleHook = false;
		float num = facingDirection;
		if (isRightHandHooked && isLeftHandHooked)
		{
			if (bulletLeft != null && bulletRight != null)
			{
				Vector3 normal = bulletLT.position - bulletRT.position;
				if (normal.sqrMagnitude < 4f)
				{
					Vector3 vector = (bulletLT.position + bulletRT.position) * 0.5f - baseT.position;
					facingDirection = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
					if ((useGun || AhssAnimation) && state != HERO_STATE.Attack)
					{
						float current = (0f - Mathf.Atan2(baseR.velocity.z, baseR.velocity.x)) * 57.29578f;
						float target = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
						float num2 = 0f - Mathf.DeltaAngle(current, target);
						facingDirection += num2;
					}
					almostSingleHook = true;
				}
				else
				{
					Vector3 to = baseT.position - bulletLT.position;
					Vector3 to2 = baseT.position - bulletRT.position;
					Vector3 vector2 = (bulletLT.position + bulletRT.position) * 0.5f;
					Vector3 from = baseT.position - vector2;
					if (Vector3.Angle(from, to) < 30f && Vector3.Angle(from, to2) < 30f)
					{
						almostSingleHook = true;
						Vector3 vector3 = vector2 - baseT.position;
						facingDirection = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
					}
					else
					{
						almostSingleHook = false;
						Vector3 tangent = baseT.forward;
						Vector3.OrthoNormalize(ref normal, ref tangent);
						facingDirection = Mathf.Atan2(tangent.x, tangent.z) * 57.29578f;
						if (Mathf.DeltaAngle(Mathf.Atan2(to.x, to.z) * 57.29578f, facingDirection) > 0f)
						{
							facingDirection += 180f;
						}
					}
				}
			}
		}
		else
		{
			almostSingleHook = true;
			Vector3 zero = Vector3.zero;
			if (isRightHandHooked && bulletRight != null)
			{
				zero = bulletRT.position - baseT.position;
			}
			else
			{
				if (!isLeftHandHooked || !(bulletLeft != null))
				{
					return;
				}
				zero = bulletLT.position - baseT.position;
			}
			facingDirection = Mathf.Atan2(zero.x, zero.z) * 57.29578f;
			if (state != HERO_STATE.Attack)
			{
				float current2 = (0f - Mathf.Atan2(baseR.velocity.z, baseR.velocity.x)) * 57.29578f;
				float target2 = (0f - Mathf.Atan2(zero.z, zero.x)) * 57.29578f;
				float num3 = 0f - Mathf.DeltaAngle(current2, target2);
				if (useGun || AhssAnimation)
				{
					facingDirection += num3;
				}
				else
				{
					float num4 = 0f;
					num4 = (((!isLeftHandHooked || !(num3 < 0f)) && (!isRightHandHooked || !(num3 > 0f))) ? 0.1f : (-0.1f));
					facingDirection += num3 * num4;
				}
			}
		}
		if (IsFiringThunderSpear())
		{
			facingDirection = num;
		}
	}

	[RPC]
	private void SetMyCannon(int viewID, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner || viewID < 0)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "messing w/ cannons");
		}
		else if (PhotonView.Find(viewID).owner != info.sender)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "messing w/ cannons2");
		}
		else
		{
			if (info.sender != basePV.owner)
			{
				return;
			}
			PhotonView photonView = PhotonView.Find(viewID);
			if (photonView != null)
			{
				myCannon = photonView.gameObject;
				if (myCannon != null)
				{
					myCannonBase = myCannon.transform;
					myCannonPlayer = myCannonBase.Find("PlayerPoint");
					isCannon = true;
				}
			}
		}
	}

	[RPC]
	private void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
	{
		if (basePV.owner == info.sender)
		{
			CameraMultiplier = offset;
			sync.PhotonCamera = true;
			isPhotonCamera = true;
		}
	}

	[RPC]
	private void setMyTeam(int val)
	{
		myTeam = val;
		checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = val;
		checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = val;
		if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
		{
			return;
		}
		if (GameSettings.friendlyMode > 0)
		{
			if (val != 1)
			{
				object[] parameters = new object[1] { 1 };
				basePV.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
			}
		}
		else if (GameSettings.pvpMode == 1)
		{
			int num = 0;
			if (basePV.owner.customProperties["RCteam"] != null)
			{
				num = RCextensions.returnIntFromObject(basePV.owner.customProperties["RCteam"]);
			}
			if (val != num)
			{
				object[] parameters = new object[1] { num };
				basePV.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
			}
		}
		else if (GameSettings.pvpMode == 2 && val != basePV.owner.ID)
		{
			object[] parameters = new object[1] { basePV.owner.ID };
			basePV.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
		}
	}

	public void setSkillHUDPosition2()
	{
		skillCD = CacheGameObject.Find("skill_cd_" + skillIDHUD);
		if (skillCD != null)
		{
			skillCD.transform.localPosition = CacheGameObject.Find("skill_cd_bottom").transform.localPosition;
		}
		if (useGun && AhssAnimation)
		{
			skillCD.transform.localPosition = Vector3.up * 5000f;
		}
	}

	public void setStat2()
	{
		skillCDLast = 1.5f;
		skillId = setup.myCostume.stat.skillId;
		if (skillId == "levi")
		{
			skillCDLast = 3.5f;
		}
		customAnimationSpeed();
		if (skillId == "armin")
		{
			skillCDLast = 5f;
		}
		if (skillId == "marco")
		{
			skillCDLast = 10f;
		}
		if (skillId == "jean")
		{
			skillCDLast = 0.001f;
		}
		if (skillId == "eren")
		{
			skillCDLast = 120f;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				if (LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.RACING || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.PVP_CAPTURE || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.TROST)
				{
					skillId = "petra";
					skillCDLast = 1f;
				}
				else
				{
					int num = 0;
					PhotonPlayer[] playerList = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer in playerList)
					{
						if (RCextensions.returnIntFromObject(photonPlayer.customProperties["isTitan"]) == 1 && RCextensions.returnStringFromObject(photonPlayer.customProperties["character"]).ToUpper() == "EREN")
						{
							num++;
						}
					}
					if (num > 1)
					{
						skillId = "petra";
						skillCDLast = 1f;
					}
				}
			}
		}
		if (skillId == "sasha")
		{
			skillCDLast = 20f;
		}
		if (skillId == "petra")
		{
			skillCDLast = 3.5f;
		}
		bombInit();
		speed = (float)setup.myCostume.stat.SPD / 10f;
		totalGas = (currentGas = setup.myCostume.stat.GAS);
		totalBladeSta = (currentBladeSta = setup.myCostume.stat.BLA);
		baseR.mass = 0.5f - (float)(setup.myCostume.stat.ACL - 100) * 0.001f;
		CacheGameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (float)(-Screen.height) * 0.5f + 5f, 0f);
		skillCD = CacheGameObject.Find("skill_cd_" + skillIDHUD);
		skillCD.transform.localPosition = CacheGameObject.Find("skill_cd_bottom").transform.localPosition;
		CacheGameObject.Find("GasUI").transform.localPosition = CacheGameObject.Find("skill_cd_bottom").transform.localPosition;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine)
		{
			CacheGameObject.Find<UISprite>("bulletL").enabled = false;
			CacheGameObject.Find<UISprite>("bulletR").enabled = false;
			CacheGameObject.Find<UISprite>("bulletL1").enabled = false;
			CacheGameObject.Find<UISprite>("bulletR1").enabled = false;
			CacheGameObject.Find<UISprite>("bulletL2").enabled = false;
			CacheGameObject.Find<UISprite>("bulletR2").enabled = false;
			CacheGameObject.Find<UISprite>("bulletL3").enabled = false;
			CacheGameObject.Find<UISprite>("bulletR3").enabled = false;
			CacheGameObject.Find<UISprite>("bulletL4").enabled = false;
			CacheGameObject.Find<UISprite>("bulletR4").enabled = false;
			CacheGameObject.Find<UISprite>("bulletL5").enabled = false;
			CacheGameObject.Find<UISprite>("bulletR5").enabled = false;
			CacheGameObject.Find<UISprite>("bulletL6").enabled = false;
			CacheGameObject.Find<UISprite>("bulletR6").enabled = false;
			CacheGameObject.Find<UISprite>("bulletL7").enabled = false;
			CacheGameObject.Find<UISprite>("bulletR7").enabled = false;
		}
		if (setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
		{
			standAnimation = "AHSS_stand_gun";
			useGun = true;
			usingAhss = true;
			gunDummy = new GameObject();
			gunDummy.name = "gunDummy";
			gunDummy.transform.position = baseT.position;
			gunDummy.transform.rotation = baseT.rotation;
			myGroup = GROUP.A;
			setTeam2(2);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine)
			{
				CacheGameObject.Find<UISprite>("bladeCL").enabled = false;
				CacheGameObject.Find<UISprite>("bladeCR").enabled = false;
				CacheGameObject.Find<UISprite>("bladel1").enabled = false;
				CacheGameObject.Find<UISprite>("blader1").enabled = false;
				CacheGameObject.Find<UISprite>("bladel2").enabled = false;
				CacheGameObject.Find<UISprite>("blader2").enabled = false;
				CacheGameObject.Find<UISprite>("bladel3").enabled = false;
				CacheGameObject.Find<UISprite>("blader3").enabled = false;
				CacheGameObject.Find<UISprite>("bladel4").enabled = false;
				CacheGameObject.Find<UISprite>("blader4").enabled = false;
				CacheGameObject.Find<UISprite>("bladel5").enabled = false;
				CacheGameObject.Find<UISprite>("blader5").enabled = false;
				CacheGameObject.Find<UISprite>("bulletL").enabled = true;
				CacheGameObject.Find<UISprite>("bulletR").enabled = true;
				CacheGameObject.Find<UISprite>("bulletL1").enabled = true;
				CacheGameObject.Find<UISprite>("bulletR1").enabled = true;
				CacheGameObject.Find<UISprite>("bulletL2").enabled = true;
				CacheGameObject.Find<UISprite>("bulletR2").enabled = true;
				CacheGameObject.Find<UISprite>("bulletL3").enabled = true;
				CacheGameObject.Find<UISprite>("bulletR3").enabled = true;
				CacheGameObject.Find<UISprite>("bulletL4").enabled = true;
				CacheGameObject.Find<UISprite>("bulletR4").enabled = true;
				CacheGameObject.Find<UISprite>("bulletL5").enabled = true;
				CacheGameObject.Find<UISprite>("bulletR5").enabled = true;
				CacheGameObject.Find<UISprite>("bulletL6").enabled = true;
				CacheGameObject.Find<UISprite>("bulletR6").enabled = true;
				CacheGameObject.Find<UISprite>("bulletL7").enabled = true;
				CacheGameObject.Find<UISprite>("bulletR7").enabled = true;
				if (skillId != "bomb")
				{
					skillCD.transform.localPosition = Vector3.up * 5000f;
				}
			}
		}
		else if (setup.myCostume.sex == SEX.FEMALE)
		{
			if (AhssAnimation)
			{
				standAnimation = "AHSS_stand_gun";
			}
			else
			{
				standAnimation = "stand";
			}
			setTeam2(1);
		}
		else
		{
			if (AhssAnimation)
			{
				standAnimation = "AHSS_stand_gun";
			}
			else
			{
				standAnimation = "stand_levi";
			}
			setTeam2(1);
		}
	}

	public void setTeam2(int team)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
		{
			object[] parameters = new object[1] { team };
			basePV.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add("team", team);
			PhotonNetwork.player.SetCustomProperties(hashtable);
		}
		else
		{
			setMyTeam(team);
		}
	}

	public void shootFlare(int type)
	{
		bool flag = false;
		if (type == 1 && flare1CD == 0f)
		{
			flare1CD = flareTotalCD;
			flag = true;
		}
		if (type == 2 && flare2CD == 0f)
		{
			flare2CD = flareTotalCD;
			flag = true;
		}
		if (type == 3 && flare3CD == 0f)
		{
			flare3CD = flareTotalCD;
			flag = true;
		}
		if (flag)
		{
			if (GameSettings.Flare > 0)
			{
				//CustomAudioManager.Play(CustomAudioManager.AudioFile.Flare);
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				GameObject obj = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("FX/flareBullet" + type), baseT.position, baseT.rotation);
				obj.GetComponent<FlareMovement>().dontShowHint();
				UnityEngine.Object.Destroy(obj, 25f);
			}
			else
			{
				PhotonNetwork.Instantiate("FX/flareBullet" + type, baseT.position, baseT.rotation, 0).GetComponent<FlareMovement>().dontShowHint();
			}
		}
	}

	private void showAimUI2()
	{
		if (Screen.showCursor)
		{
			Vector3 localPosition = Vector3.up * 10000f;
			crossR2T.localPosition = localPosition;
			crossR1T.localPosition = localPosition;
			crossL2T.localPosition = localPosition;
			crossL1T.localPosition = localPosition;
			labelT.localPosition = localPosition;
			return;
		}
		checkTitan();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask2 = (int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask;
		if (!Physics.Raycast(ray, out var hitInfo, 1E+07f, layerMask2.value))
		{
			return;
		}
		crossT1.localPosition = Input.mousePosition;
		crossT1.localPosition -= new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
		crossT2.localPosition = crossT1.localPosition;
		crossT1.localScale = new Vector3(FengGameManagerMKII.instance.CursorWidth, FengGameManagerMKII.instance.CursorHeight, 1f);
		crossT2.localScale = new Vector3(FengGameManagerMKII.instance.CursorWidth, FengGameManagerMKII.instance.CursorHeight, 1f);
		float magnitude = (hitInfo.point - baseT.position).magnitude;
		string text = string.Empty;
		if (GameSettings.bombMode > 0)
		{
			if (skillId == "bomb")
			{
				if (GameSettings.ShowCD > 0)
				{
					text = ((!(skillCDDuration > 0f)) ? "[9efeff]Shoot[-]\n" : ("[FF0000]" + skillCDDuration.RoundTo(1) + "s[-]\n"));
				}
			}
			else
			{
				text = "";
			}
		}
		if (GameSettings.ShowDistance > 0)
		{
			text = ((!(magnitude <= 1000f)) ? (text + "???\n") : (text + (int)magnitude + "\n"));
		}
		if ((int)FengGameManagerMKII.settings[189] == 1)
		{
			text = text + currentSpeed.ToString("F1") + " u/s";
		}
		else if (myGroup == GROUP.A && (int)FengGameManagerMKII.settings[189] == 2)
		{
			text = text + (currentSpeed * 0.4f / 100f).ToString("F2") + "K  |  " + (currentSpeed / 100f).ToString("F2") + "K  |  " + (currentSpeed * 0.6f / 100f).ToString("F2") + "K";
		}
		else if ((int)FengGameManagerMKII.settings[189] == 2)
		{
			text = text + (currentSpeed / 100f).ToString("F2") + " K";
		}
		LabelDistance.text = text;
		if (magnitude > 120f)
		{
			crossT1.localPosition += Vector3.up * 10000f;
			labelT.localPosition = crossT2.localPosition;
		}
		else
		{
			crossT2.localPosition += Vector3.up * 10000f;
			labelT.localPosition = crossT1.transform.localPosition;
		}
		labelT.localPosition -= new Vector3(0f, 15f, 0f);
		if (GameSettings.NoArrowsCursor != 0)
		{
			return;
		}
		Vector3 vector = new Vector3(0f, 0.4f, 0f);
		vector -= baseT.right * 0.3f;
		Vector3 vector2 = new Vector3(0f, 0.4f, 0f);
		vector2 += baseT.right * 0.3f;
		float num = ((hitInfo.distance <= 50f) ? (hitInfo.distance * 0.05f) : (hitInfo.distance * 0.3f));
		Vector3 vector3 = hitInfo.point - baseT.right * num - (baseT.position + vector);
		Vector3 vector4 = hitInfo.point + baseT.right * num - (baseT.position + vector2);
		vector3.Normalize();
		vector4.Normalize();
		vector3 *= 1000000f;
		vector4 *= 1000000f;
		if (Physics.Linecast(baseT.position + vector, baseT.position + vector + vector3, out var hitInfo2, layerMask2.value))
		{
			crossL1T.localPosition = Camera.main.WorldToScreenPoint(hitInfo2.point);
			crossL1T.localPosition -= new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
			crossL1T.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(crossL1T.localPosition.y - (Input.mousePosition.y - (float)Screen.height * 0.5f), crossL1T.localPosition.x - (Input.mousePosition.x - (float)Screen.width * 0.5f)) * 57.29578f + 180f);
			crossL2T.localPosition = crossL1T.localPosition;
			crossL2T.localRotation = crossL1T.localRotation;
			if (hitInfo2.distance > 120f)
			{
				crossL1T.localPosition += Vector3.up * 10000f;
			}
			else
			{
				crossL1T.localPosition += Vector3.up * 10000f;
			}
		}
		if (Physics.Linecast(baseT.position + vector2, baseT.position + vector2 + vector4, out hitInfo2, layerMask2.value))
		{
			crossR1T.localPosition = Camera.main.WorldToScreenPoint(hitInfo2.point);
			crossR1T.localPosition -= new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
			crossR1T.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(crossR1T.localPosition.y - (Input.mousePosition.y - (float)Screen.height * 0.5f), crossR1T.localPosition.x - (Input.mousePosition.x - (float)Screen.width * 0.5f)) * 57.29578f);
			crossR2T.localPosition = crossR1T.localPosition;
			crossR2T.localRotation = crossR1T.localRotation;
			if (hitInfo2.distance > 120f)
			{
				crossR1T.localPosition += Vector3.up * 10000f;
			}
			else
			{
				crossR2T.localPosition += Vector3.up * 10000f;
			}
		}
	}

	private void showFlareCD2()
	{
		if (cachedSprites["UIflare1"] != null)
		{
			cachedSprites["UIflare1"].fillAmount = (flareTotalCD - flare1CD) / flareTotalCD;
			cachedSprites["UIflare2"].fillAmount = (flareTotalCD - flare2CD) / flareTotalCD;
			cachedSprites["UIflare3"].fillAmount = (flareTotalCD - flare3CD) / flareTotalCD;
		}
	}

	private void showGas2()
	{
		float num = currentGas / totalGas;
		float num2 = currentBladeSta / totalBladeSta;
		cachedSprites["gasL1"].fillAmount = currentGas / totalGas;
		cachedSprites["gasR1"].fillAmount = currentGas / totalGas;
		if (!useGun)
		{
			cachedSprites["bladeCL"].fillAmount = currentBladeSta / totalBladeSta;
			cachedSprites["bladeCR"].fillAmount = currentBladeSta / totalBladeSta;
			if (num < 0.3f)
			{
				cachedSprites["gasL"].color = Color.yellow;
				cachedSprites["gasR"].color = Color.yellow;
			}
			if (num < 0.1f)
			{
				cachedSprites["gasL"].color = new Color(1f, 0.35f, 0f, 1f);
				cachedSprites["gasR"].color = new Color(1f, 0.35f, 0f, 1f);
			}
			if (num <= 0f)
			{
				cachedSprites["gasL"].color = Color.red;
				cachedSprites["gasR"].color = Color.red;
			}
			if (num >= 0.3f)
			{
				cachedSprites["gasL"].color = Color.white;
				cachedSprites["gasR"].color = Color.white;
			}
			if (num2 < 0.3f)
			{
				cachedSprites["bladel1"].color = Color.yellow;
				cachedSprites["blader1"].color = Color.yellow;
			}
			if (num2 < 0.1f)
			{
				cachedSprites["bladel1"].color = new Color(1f, 0.35f, 0f, 1f);
				cachedSprites["blader1"].color = new Color(1f, 0.35f, 0f, 1f);
			}
			if (num2 <= 0f)
			{
				cachedSprites["bladel1"].color = Color.red;
				cachedSprites["blader1"].color = Color.red;
			}
			if (num2 >= 0.3f)
			{
				cachedSprites["bladel1"].color = Color.white;
				cachedSprites["blader1"].color = Color.white;
			}
			if (currentBladeNum <= 4)
			{
				cachedSprites["bladel5"].enabled = false;
				cachedSprites["blader5"].enabled = false;
			}
			else
			{
				cachedSprites["bladel5"].enabled = true;
				cachedSprites["blader5"].enabled = true;
			}
			if (currentBladeNum <= 3)
			{
				cachedSprites["bladel4"].enabled = false;
				cachedSprites["blader4"].enabled = false;
			}
			else
			{
				cachedSprites["bladel4"].enabled = true;
				cachedSprites["blader4"].enabled = true;
			}
			if (currentBladeNum <= 2)
			{
				cachedSprites["bladel3"].enabled = false;
				cachedSprites["blader3"].enabled = false;
			}
			else
			{
				cachedSprites["bladel3"].enabled = true;
				cachedSprites["blader3"].enabled = true;
			}
			if (currentBladeNum <= 1)
			{
				cachedSprites["bladel2"].enabled = false;
				cachedSprites["blader2"].enabled = false;
			}
			else
			{
				cachedSprites["bladel2"].enabled = true;
				cachedSprites["blader2"].enabled = true;
			}
			if (currentBladeNum <= 0)
			{
				cachedSprites["bladel1"].enabled = false;
				cachedSprites["blader1"].enabled = false;
			}
			else
			{
				cachedSprites["bladel1"].enabled = true;
				cachedSprites["blader1"].enabled = true;
			}
		}
		else
		{
			if (leftGunHasBullet)
			{
				cachedSprites["bulletL"].enabled = true;
			}
			else
			{
				cachedSprites["bulletL"].enabled = false;
			}
			if (rightGunHasBullet)
			{
				cachedSprites["bulletR"].enabled = true;
			}
			else
			{
				cachedSprites["bulletR"].enabled = false;
			}
		}
	}

	[RPC]
	private void showHitDamage()
	{
		if (FengGameManagerMKII.LabelScore != null)
		{
			speed = Mathf.Max(10f, speed);
			FengGameManagerMKII.LabelScoreUI.text = speed.ToString();
			FengGameManagerMKII.LabelScoreT.localScale = Vector3.zero;
			speed = (int)(speed * 0.1f);
			speed = Mathf.Clamp(speed, 40f, 150f);
			iTween.Stop(FengGameManagerMKII.LabelScore);
			object[] args = new object[10]
			{
				"x",
				speed,
				"y",
				speed,
				"z",
				speed,
				"easetype",
				iTween.EaseType.easeOutElastic,
				"time",
				1f
			};
			iTween.ScaleTo(FengGameManagerMKII.LabelScore, iTween.Hash(args));
			object[] args2 = new object[12]
			{
				"x",
				0,
				"y",
				0,
				"z",
				0,
				"easetype",
				iTween.EaseType.easeInBounce,
				"time",
				0.5f,
				"delay",
				2f
			};
			iTween.ScaleTo(FengGameManagerMKII.LabelScore, iTween.Hash(args2));
		}
	}

	private void showSkillCD()
	{
		if (skillCD != null)
		{
			skillCD.GetComponent<UISprite>().fillAmount = (skillCDLast - skillCDDuration) / skillCDLast;
		}
	}

	[RPC]
	public void SpawnCannonRPC(string settings, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient && basePV.isMine && myCannon == null)
		{
			if (myHorse != null && isMounted)
			{
				getOffHorse();
			}
			idle();
			if (bulletLeft != null)
			{
				LBullet.removeMe();
			}
			if (bulletRight != null)
			{
				RBullet.removeMe();
			}
			if (smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
			{
				object[] parameters = new object[1] { false };
				basePV.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
			}
			smoke_3dmg.enableEmission = false;
			baseR.velocity = Vector3.zero;
			string[] array = settings.Split(',');
			if (array.Length > 15)
			{
				myCannon = PhotonNetwork.Instantiate("RCAsset/" + array[1], new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])), 0);
			}
			else
			{
				myCannon = PhotonNetwork.Instantiate("RCAsset/" + array[1], new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])), 0);
			}
			myCannonBase = myCannon.transform;
			myCannonPlayer = myCannon.transform.Find("PlayerPoint");
			isCannon = true;
			myCannon.GetComponent<Cannon>().myHero = this;
			myCannonRegion = null;
			IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(myCannon.transform.Find("Barrel").Find("FiringPoint").gameObject);
			Camera.main.fieldOfView = 55f;
			basePV.RPC("SetMyCannon", PhotonTargets.OthersBuffered, myCannon.GetPhotonView().viewID);
			skillCDLastCannon = skillCDLast;
			skillCDLast = 3.5f;
			skillCDDuration = 3.5f;
		}
	}

	private void suicide2()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			netDieLocal(baseR.velocity * 50f, isBite: false, -1, string.Empty);
			FengGameManagerMKII.instance.needChooseSide = true;
			FengGameManagerMKII.justSuicide = true;
			if (GameSettings.bombMode > 0)
			{
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add("deaths", RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["deaths"]) - 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
            }
			FengGameManagerMKII.instance.BottomHUD(enabled: true);
		}
	}

	private void throwBlades()
	{
		//if (!CustomAudioManager.BladesReload.isPlaying && GameSettings.BladesBroken > 0)
		//{
		//	//CustomAudioManager.Play(CustomAudioManager.AudioFile.BladesBroken);
		//}
		Transform transform = setup.part_blade_l.transform;
		Transform transform2 = setup.part_blade_r.transform;
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
		gameObject.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
		obj.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
		Vector3 force = baseT.forward + baseT.up * 2f - baseT.right;
		gameObject.rigidbody.AddForce(force, ForceMode.Impulse);
		Vector3 force2 = baseT.forward + baseT.up * 2f + baseT.right;
		obj.rigidbody.AddForce(force2, ForceMode.Impulse);
		Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
		torque.Normalize();
		gameObject.rigidbody.AddTorque(torque);
		torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
		torque.Normalize();
		obj.rigidbody.AddTorque(torque);
		setup.part_blade_l.SetActive(value: false);
		setup.part_blade_r.SetActive(value: false);
		currentBladeNum--;
		if (currentBladeNum == 0)
		{
			currentBladeSta = 0f;
		}
		if (state == HERO_STATE.Attack)
		{
			falseAttack();
		}
	}

	public void ungrabbed()
	{
		facingDirection = 0f;
		targetRotation = Quaternion.Euler(0f, 0f, 0f);
		baseT.parent = null;
		GetComponent<CapsuleCollider>().isTrigger = false;
		state = HERO_STATE.Idle;
	}

	private void unmounted()
	{
		myHorse.GetComponent<Horse>().unmounted();
		isMounted = false;
	}

	private void calcSkillCD()
	{
		if (skillCDDuration > 0f)
		{
			skillCDDuration -= Time.deltaTime;
			if (skillCDDuration < 0f)
			{
				skillCDDuration = 0f;
			}
		}
	}

	private int Damage(Vector3 vector)
	{
		return Mathf.Max(10, (int)((IN_GAME_MAIN_CAMERA.main_objectR.velocity - vector).magnitude * 10f * 0.4f));
	}

	public void update()
	{
        if (useGun && !grounded)
		{
			usingAhss = true;
		}
		else
		{
			usingAhss = false;
		}
        if (invincible > 0f)
		{
			invincible -= Time.deltaTime;
		}
		if (hasDied)
		{
			return;
		}
		if (titanForm && eren_titan != null)
		{
			baseT.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
			sync.disabled = true;
		}
		else if (isCannon && myCannon != null)
		{
			updateCannon();
			sync.disabled = true;
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine)
		{
			return;
		}
        if (FengCustomInputs.Inputs.isInputDown[InputCode.restart])
        {
            suicide2();
        }
        if (myCannonRegion != null)
		{
			FengGameManagerMKII.instance.ShowHUDInfoCenter("Press 'Cannon Mount' key to use Cannon.");
			if (FengGameManagerMKII.inputRC.isInputCannonDown(InputCodeRC.cannonMount))
			{
				myCannonRegion.photonView.RPC("RequestControlRPC", PhotonTargets.MasterClient, basePV.viewID);
			}
		}
		if (state == HERO_STATE.Grab && !useGun)
		{
			if (skillId == "jean")
			{
				if (state != HERO_STATE.Attack && (FengCustomInputs.Inputs.isInputDown[InputCode.attack0] || FengCustomInputs.Inputs.isInputDown[InputCode.attack1]) && escapeTimes > 0 && !baseA.IsPlaying("grabbed_jean"))
				{
					playAnimation("grabbed_jean");
					baseA["grabbed_jean"].time = 0f;
					escapeTimes--;
				}
				if (baseA.IsPlaying("grabbed_jean") && baseA["grabbed_jean"].normalizedTime > 0.64f && titanWhoGrabMe.GetComponent<TITAN>() != null)
				{
					ungrabbed();
					baseR.velocity = Vector3.up * 30f;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
					}
					else
					{
						basePV.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
						if (PhotonNetwork.isMasterClient)
						{
							titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
						}
						else
						{
							PhotonView.Find(titanWhoGrabMeID).RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
						}
					}
				}
			}
			else if (skillId == "eren")
			{
				showSkillCD();
				if (IN_GAME_MAIN_CAMERA.gametype != 0 || (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !IN_GAME_MAIN_CAMERA.isPausing))
				{
					calcSkillCD();
					calcFlareCD();
				}
				if (FengCustomInputs.Inputs.isInputDown[InputCode.attack1])
				{
					bool flag = false;
					if (skillCDDuration > 0f || flag)
					{
						flag = true;
					}
					else
					{
						skillCDDuration = skillCDLast;
						if (skillId == "eren" && titanWhoGrabMe.GetComponent<TITAN>() != null)
						{
							ungrabbed();
							if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
							{
								titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
							}
							else
							{
								basePV.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
								if (PhotonNetwork.isMasterClient)
								{
									titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
								}
								else
								{
									PhotonView.Find(titanWhoGrabMeID).photonView.RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
								}
							}
							erenTransform();
						}
					}
				}
			}
		}
		else if (!titanForm && !isCannon)
		{
			bufferUpdate();
			UpdateThunderSpear();
			if (!grounded && state != HERO_STATE.AirDodge)
			{
				if ((int)FengGameManagerMKII.settings[284] == 0)
				{
					checkDashDoubleTap();
				}
				else if ((int)FengGameManagerMKII.settings[284] == 1)
				{
					checkDashRebind();
				}
				else
				{
					checkDashDoubleTap();
					checkDashRebind();
				}
				if (dashD)
				{
					dashD = false;
					dash(0f, -1f);
					return;
				}
				if (dashU)
				{
					dashU = false;
					dash(0f, 1f);
					return;
				}
				if (dashL)
				{
					dashL = false;
					dash(-1f, 0f);
					return;
				}
				if (dashR)
				{
					dashR = false;
					dash(1f, 0f);
					return;
				}
			}
			if (grounded && (state == HERO_STATE.Idle || state == HERO_STATE.Slide))
			{
				if (FengCustomInputs.Inputs.isInputDown[InputCode.jump] && !baseA.IsPlaying("jump") && !baseA.IsPlaying("horse_geton"))
				{
					idle();
					crossFade("jump", 0.1f);
					sparks.enableEmission = false;
				}
				if (FengGameManagerMKII.inputRC.isInputHorseDown(InputCodeRC.horseMount) && !baseA.IsPlaying("jump") && !baseA.IsPlaying("horse_geton") && myHorse != null && !isMounted && Vector3.Distance(myHorse.transform.position, baseT.position) < 15f)
				{
					getOnHorse();
				}
				if (FengCustomInputs.Inputs.isInputDown[InputCode.dodge] && !baseA.IsPlaying("jump") && !baseA.IsPlaying("horse_geton"))
				{
					dodge2();
					return;
				}
			}
			if ((int)FengGameManagerMKII.settings[280] == 0 && FengGameManagerMKII.inputRC.isInputHumanUp(InputCodeRC.reelin))
			{
				AlreadyReeled = false;
			}
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (((string)FengGameManagerMKII.settings[98] == "Scroll Up" && axis > 0f) || ((string)FengGameManagerMKII.settings[98] == "Scroll Down" && axis < 0f))
			{
				bool flag2 = false;
				bool flag3 = false;
				if (isLaunchLeft && bulletLeft != null && LBullet.isHooked())
				{
					isLeftHandHooked = true;
					Vector3 to = bulletLT.position - baseT.position;
					to.Normalize();
					to *= 10f;
					if (!isLaunchRight)
					{
						to *= 2f;
					}
					if (Vector3.Angle(baseR.velocity, to) > 90f && FengCustomInputs.Inputs.isInput[InputCode.jump])
					{
						flag2 = true;
					}
				}
				if (isLaunchRight && bulletRight != null && RBullet.isHooked())
				{
					isRightHandHooked = true;
					Vector3 to2 = bulletRT.position - baseT.position;
					to2.Normalize();
					to2 *= 10f;
					if (!isLaunchLeft)
					{
						to2 *= 2f;
					}
					if (Vector3.Angle(baseR.velocity, to2) > 90f && FengCustomInputs.Inputs.isInput[InputCode.jump])
					{
						flag3 = true;
					}
				}
				Vector3 current = Vector3.zero;
				if (flag2 && flag3)
				{
					current = (bulletRT.position + bulletLT.position) * 0.5f - baseT.position;
				}
				else if (flag2 && !flag3)
				{
					current = bulletLT.position - baseT.position;
				}
				else if (flag3 && !flag2)
				{
					current = bulletRT.position - baseT.position;
				}
				if (flag2 || flag3)
				{
					baseR.AddForce(-baseR.velocity, ForceMode.VelocityChange);
					float num = 1.53938f * (1f + Mathf.Clamp((axis > 0f) ? 1f : (-1f), -0.8f, 0.8f));
					baseR.velocity = Vector3.RotateTowards(current, baseR.velocity, num, num).normalized * (currentSpeed + 0.1f);
				}
			}

			switch (state)
			{
				case HERO_STATE.Idle:
					{
						if (FengCustomInputs.Inputs.isInputDown[InputCode.flare1])
						{
							shootFlare(1);
						}
						if (FengCustomInputs.Inputs.isInputDown[InputCode.flare2])
						{
							shootFlare(2);
						}
						if (FengCustomInputs.Inputs.isInputDown[InputCode.flare3])
						{
							shootFlare(3);
						}
						if (myHorse != null && isMounted && FengGameManagerMKII.inputRC.isInputHorseDown(InputCodeRC.horseMount))
						{
							getOffHorse();
						}
						if ((baseA.IsPlaying(standAnimation) || !grounded) && FengCustomInputs.Inputs.isInputDown[InputCode.reload] && (!useGun || GameSettings.ahssReload != 1 || grounded))
						{
							changeBlade();
							return;
						}
						if (baseA.IsPlaying(standAnimation) && FengCustomInputs.Inputs.isInputDown[InputCode.salute])
						{
							salute();
							return;
						}
						if (!isMounted && (FengCustomInputs.Inputs.isInputDown[InputCode.attack0] || FengCustomInputs.Inputs.isInputDown[InputCode.attack1]) && !usingAhss)
						{
							bool flag5 = false;
							if (FengCustomInputs.Inputs.isInputDown[InputCode.attack1])
							{
								if (skillCDDuration > 0f || flag5)
								{
									flag5 = true;
								}
								else
								{
									skillCDDuration = skillCDLast;
									switch (skillId)
									{
										case "eren":
											erenTransform();
											break;
										case "marco":
											if (IsGrounded())
											{
												attackAnimation = ((UnityEngine.Random.Range(0, 2) != 0) ? "special_marco_1" : "special_marco_0");
												playAnimation(attackAnimation);
											}
											else
											{
												flag5 = true;
												skillCDDuration = 0f;
											}
											break;
										case "armin":
											if (IsGrounded())
											{
												attackAnimation = "special_armin";
												playAnimation("special_armin");
											}
											else
											{
												flag5 = true;
												skillCDDuration = 0f;
											}
											break;
										case "sasha":
											attackAnimation = "special_sasha";
											playAnimation("special_sasha");
											currentBuff = BUFF.SpeedUp;
											buffTime = 10f;
											break;
										case "mikasa":
											attackAnimation = "attack3_1";
											playAnimation("attack3_1");
											baseR.velocity = Vector3.up * 10f;
											break;
										case "levi":
											{
												attackAnimation = "attack5";
												playAnimation("attack5");
												baseR.velocity += Vector3.up * 5f;
												if (Physics.Raycast(IN_GAME_MAIN_CAMERA.mainC.ScreenPointToRay(Input.mousePosition), out var hitInfo, 1E+07f, Layer.GroundEnemy.value))
												{
													if (bulletRight != null)
													{
														RBullet.disable();
														releaseIfIHookSb();
													}
													dashDirection = hitInfo.point - baseT.position;
													launchRightRope(hitInfo, single: true, 1);
													//rope.Play();
												}
												facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * 57.29578f;
												targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
												attackLoop = 3;
												break;
											}
										case "petra":
											{
												attackAnimation = "special_petra";
												playAnimation("special_petra");
												baseR.velocity += Vector3.up * 5f;
												if (Physics.Raycast(IN_GAME_MAIN_CAMERA.mainC.ScreenPointToRay(Input.mousePosition), out var hitInfo2, 1E+07f, Layer.GroundEnemy.value))
												{
													if (bulletRight != null)
													{
														RBullet.disable();
														releaseIfIHookSb();
													}
													if (bulletLeft != null)
													{
														LBullet.disable();
														releaseIfIHookSb();
													}
													dashDirection = hitInfo2.point - baseT.position;
													launchLeftRope(hitInfo2, single: true, 0);
													launchRightRope(hitInfo2, single: true);
													//rope.Play();
												}
												facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * 57.29578f;
												targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
												attackLoop = 3;
												break;
											}
										default:
											if (needLean)
											{
												if (leanLeft)
												{
													attackAnimation = ((UnityEngine.Random.Range(0, 2) == 0) ? "attack1_hook_l1" : "attack1_hook_l2");
												}
												else
												{
													attackAnimation = ((UnityEngine.Random.Range(0, 2) == 0) ? "attack1_hook_r1" : "attack1_hook_r2");
												}
											}
											else
											{
												attackAnimation = "attack1";
											}
											playAnimation(attackAnimation);
											break;
									}
								}
							}
							else if (FengCustomInputs.Inputs.isInputDown[InputCode.attack0])
							{
								if (needLean)
								{
									if (FengCustomInputs.Inputs.isInput[InputCode.left])
									{
										attackAnimation = ((UnityEngine.Random.Range(0, 2) == 0) ? "attack1_hook_l1" : "attack1_hook_l2");
									}
									else if (FengCustomInputs.Inputs.isInput[InputCode.right])
									{
										attackAnimation = ((UnityEngine.Random.Range(0, 2) == 0) ? "attack1_hook_r1" : "attack1_hook_r2");
									}
									else if (leanLeft)
									{
										attackAnimation = ((UnityEngine.Random.Range(0, 2) == 0) ? "attack1_hook_l1" : "attack1_hook_l2");
									}
									else
									{
										attackAnimation = ((UnityEngine.Random.Range(0, 2) == 0) ? "attack1_hook_r1" : "attack1_hook_r2");
									}
								}
								else if (FengCustomInputs.Inputs.isInput[InputCode.left])
								{
									attackAnimation = "attack2";
								}
								else if (FengCustomInputs.Inputs.isInput[InputCode.right])
								{
									attackAnimation = "attack1";
								}
								else if (lastHook != null)
								{
									if (lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck") != null)
									{
										attackAccordingToTarget(lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"));
									}
									else
									{
										flag5 = true;
									}
								}
								else if (bulletLeft != null && bulletLT.parent != null)
								{
									Transform transform = bulletLT.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
									if (transform != null)
									{
										attackAccordingToTarget(transform);
									}
									else
									{
										attackAccordingToMouse();
									}
								}
								else if (bulletRight != null && bulletRT.parent != null)
								{
									Transform transform2 = bulletRT.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
									if (transform2 != null)
									{
										attackAccordingToTarget(transform2);
									}
									else
									{
										attackAccordingToMouse();
									}
								}
								else
								{
									GameObject gameObject3 = findNearestTitan();
									if (gameObject3 != null)
									{
										Transform transform3 = gameObject3.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
										if (transform3 != null)
										{
											attackAccordingToTarget(transform3);
										}
										else
										{
											attackAccordingToMouse();
										}
									}
									else
									{
										attackAccordingToMouse();
									}
								}
							}
							if (!flag5)
							{
								triggerLeft.clearHits();
								triggerRight.clearHits();
								if (grounded)
								{
									baseR.AddForce(baseG.transform.forward * 200f);
                                }
                                playAnimation(attackAnimation);
								baseA[attackAnimation].time = 0f;
								buttonAttackRelease = false;
								state = HERO_STATE.Attack;
								if (grounded || attackAnimation == "attack3_1" || attackAnimation == "attack5" || attackAnimation == "special_petra")
								{
									attackReleased = true;
									buttonAttackRelease = true;
								}
								else
								{
									attackReleased = false;
								}
								sparks.enableEmission = false;
							}
						}
						if (!usingAhss)
						{
							break;
						}
                        if (FengCustomInputs.Inputs.isInput[InputCode.attack1])
						{
							leftArmAim = true;
							rightArmAim = true;
						}
						else if (FengCustomInputs.Inputs.isInput[InputCode.attack0])
						{
							if (leftGunHasBullet)
							{
								leftArmAim = true;
								rightArmAim = false;
							}
							else
							{
								leftArmAim = false;
								if (rightGunHasBullet)
								{
									rightArmAim = true;
								}
								else
								{
									rightArmAim = false;
								}
							}
						}
						else
						{
							leftArmAim = false;
							rightArmAim = false;
						}
						if ((leftArmAim || rightArmAim) && Physics.Raycast(IN_GAME_MAIN_CAMERA.mainC.ScreenPointToRay(Input.mousePosition), out var hitInfo3, 1E+07f, Layer.GroundEnemy.value))
						{
							gunTarget = hitInfo3.point;
						}
						bool flag6 = false;
						bool flag7 = false;
						bool flag8 = false;
						if (FengCustomInputs.Inputs.isInputUp[InputCode.attack1] && skillId != "bomb")
						{
							if (leftGunHasBullet && rightGunHasBullet)
							{
								if (grounded)
								{
									attackAnimation = "AHSS_shoot_both";
								}
								else
								{
									attackAnimation = "AHSS_shoot_both_air";
								}
								flag6 = true;
							}
							else if (!leftGunHasBullet && !rightGunHasBullet)
							{
								flag7 = true;
							}
							else
							{
								flag8 = true;
							}
						}
						if (flag8 || FengCustomInputs.Inputs.isInputUp[InputCode.attack0])
						{
							if (grounded)
							{
								if (leftGunHasBullet && rightGunHasBullet)
								{
									if (isLeftHandHooked)
									{
										attackAnimation = "AHSS_shoot_r";
									}
									else
									{
										attackAnimation = "AHSS_shoot_l";
									}
								}
								else if (leftGunHasBullet)
								{
									attackAnimation = "AHSS_shoot_l";
								}
								else if (rightGunHasBullet)
								{
									attackAnimation = "AHSS_shoot_r";
								}
							}
							else if (leftGunHasBullet && rightGunHasBullet)
							{
								if (isLeftHandHooked)
								{
									attackAnimation = "AHSS_shoot_r_air";
								}
								else
								{
									attackAnimation = "AHSS_shoot_l_air";
								}
							}
							else if (leftGunHasBullet)
							{
								attackAnimation = "AHSS_shoot_l_air";
							}
							else if (rightGunHasBullet)
							{
								attackAnimation = "AHSS_shoot_r_air";
							}
							if (leftGunHasBullet || rightGunHasBullet)
							{
								flag6 = true;
							}
							else
							{
								flag7 = true;
							}
						}
						if (flag6)
						{
							state = HERO_STATE.Attack;
							crossFade(attackAnimation, 0.05f);
							gunDummy.transform.position = baseT.position;
							gunDummy.transform.rotation = baseT.rotation;
							gunDummy.transform.LookAt(gunTarget);
							attackReleased = false;
							facingDirection = gunDummy.transform.rotation.eulerAngles.y;
							targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
						}
						else if (flag7 && (grounded || (LevelInfo.getInfo(FengGameManagerMKII.level).type != GAMEMODE.PVP_AHSS && GameSettings.ahssReload == 0)))
						{
							changeBlade();
						}
                        break;
					}
				case HERO_STATE.Attack:
					if (usingAhss)
					{
                        //foreach (var src in GameObject.FindObjectsOfType<AudioSource>())
                        //{
                        //    if (src.name != "NotificationSound")
                        //    {
                        //        src.mute = true;
                        //    }
                        //}
                    }
                    if (!usingAhss)
					{
                        if (!FengCustomInputs.Inputs.isInput[InputCode.attack0])
						{
							buttonAttackRelease = true;
						}
						if (!attackReleased)
						{
							if (buttonAttackRelease)
							{
								continueAnimation();
								attackReleased = true;
							}
							else if (baseA[attackAnimation].normalizedTime >= 0.32f)
							{
								pauseAnimation();
							}
						}
						if (attackAnimation == "attack3_1" && currentBladeSta > 0f && !useGun)
						{
							if (baseA[attackAnimation].normalizedTime >= 0.8f)
							{
								if (!triggerLeft.active_me)
								{
									triggerLeft.active_me = true;
									if ((int)FengGameManagerMKII.settings[92] == 1)
									{
										leftbladetrail2.Activate();
										rightbladetrail2.Activate();
										leftbladetrail.Activate();
										rightbladetrail.Activate();
									}
									baseR.velocity = -Vector3.up * 30f;
								}
								if (!triggerRight.active_me)
								{
									triggerRight.active_me = true;
									//slash.Play();
								}
							}
							else if (triggerLeft.active_me)
							{
								triggerLeft.active_me = false;
								triggerRight.active_me = false;
								triggerLeft.clearHits();
								triggerRight.clearHits();
								leftbladetrail.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
								rightbladetrail.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
								leftbladetrail2.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
								rightbladetrail2.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
							}
						}
						else
						{
							float num3;
							float num2;
							if (currentBladeSta == 0f)
							{
								num3 = (num2 = -1f);
							}
							else if (attackAnimation == "attack5")
							{
								num3 = 0.35f;
								num2 = 0.5f;
							}
							else if (attackAnimation == "special_petra")
							{
								num3 = 0.35f;
								num2 = 0.48f;
							}
							else if (attackAnimation == "special_armin")
							{
								num3 = 0.25f;
								num2 = 0.35f;
							}
							else if (attackAnimation == "attack4")
							{
								num3 = 0.6f;
								num2 = 0.9f;
							}
							else if (attackAnimation == "special_sasha")
							{
								num3 = (num2 = -1f);
							}
							else
							{
								num3 = 0.5f;
								num2 = 0.85f;
							}
							if (baseA[attackAnimation].normalizedTime > num3 && baseA[attackAnimation].normalizedTime < num2)
							{
								if (!triggerLeft.active_me)
								{
									triggerLeft.active_me = true;
									//slash.Play();
									if ((int)FengGameManagerMKII.settings[92] == 1 && !useGun)
									{
										leftbladetrail2.Activate();
										rightbladetrail2.Activate();
										leftbladetrail.Activate();
										rightbladetrail.Activate();
									}
								}
								if (!triggerRight.active_me)
								{
									triggerRight.active_me = true;
								}
							}
							else if (triggerLeft.active_me && !useGun)
							{
								triggerLeft.active_me = false;
								triggerRight.active_me = false;
								triggerLeft.clearHits();
								triggerRight.clearHits();
								leftbladetrail2.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
								rightbladetrail2.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
								leftbladetrail.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
								rightbladetrail.StopSmoothly(FengGameManagerMKII.instance.SomeFadeTime / 4f);
							}
							if (attackLoop > 0 && baseA[attackAnimation].normalizedTime > num2)
							{
								attackLoop--;
								playAnimationAt(attackAnimation, num3);
							}
						}
						if (baseA[attackAnimation].normalizedTime >= 1f)
						{
							if (attackAnimation == "special_marco_0" || attackAnimation == "special_marco_1")
							{
								if (IN_GAME_MAIN_CAMERA.gametype != 0)
								{
									if (!PhotonNetwork.isMasterClient)
									{
										object[] parameters = new object[2] { 5f, 100f };
										basePV.RPC("netTauntAttack", PhotonTargets.MasterClient, parameters);
									}
									else
									{
										netTauntAttackSelf(5f);
									}
								}
								else
								{
									netTauntAttackSelf(5f);
								}
								falseAttack();
								idle();
							}
							else if (attackAnimation == "special_armin")
							{
								if (IN_GAME_MAIN_CAMERA.gametype != 0)
								{
									if (!PhotonNetwork.isMasterClient)
									{
										basePV.RPC("netlaughAttack", PhotonTargets.MasterClient);
									}
									else
									{
										netlaughAttack();
									}
								}
								else
								{
									GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
									foreach (GameObject gameObject in array)
									{
										if (Vector3.Distance(gameObject.transform.position, baseT.position) < 50f && Vector3.Angle(gameObject.transform.forward, baseT.position - gameObject.transform.position) < 90f && gameObject.GetComponent<TITAN>() != null)
										{
											gameObject.GetComponent<TITAN>().beLaughAttacked();
										}
									}
								}
								falseAttack();
								idle();
							}
							else if (attackAnimation == "attack3_1")
							{
								baseR.velocity -= Vector3.up * Time.deltaTime * 30f;
							}
							else
							{
								falseAttack();
								idle();
							}
						}
						if (baseA.IsPlaying("attack3_2") && baseA["attack3_2"].normalizedTime >= 1f)
						{
							falseAttack();
							idle();
						}
						break;
					}
					if (!attackReleased && baseA[attackAnimation].normalizedTime > 0.167f && usingAhss)
					{
                        attackReleased = true;
						if (basePV.isMine && GameSettings.GunsShot > 0)
						{
							//CustomAudioManager.Play(CustomAudioManager.AudioFile.GunsShot);
						}
						bool flag4 = false;
						if (attackAnimation == "AHSS_shoot_both" || attackAnimation == "AHSS_shoot_both_air")
						{
							flag4 = true;
                            if (!InRoomChat.god) leftGunHasBullet = false;
                            else leftGunHasBullet = true;
                            if (!InRoomChat.god) rightGunHasBullet = false;
                            else rightGunHasBullet = true;
                            baseR.AddForce(-baseT.forward * 1000f, ForceMode.Acceleration);
						}
						else
						{
							if (attackAnimation == "AHSS_shoot_l" || attackAnimation == "AHSS_shoot_l_air")
							{
                                if (!InRoomChat.god) leftGunHasBullet = false;
                                else leftGunHasBullet = true;
                            }
							else
							{
                                if (!InRoomChat.god) rightGunHasBullet = false;
                                else rightGunHasBullet = true;
                            }
							baseR.AddForce(-baseT.forward * 600f, ForceMode.Acceleration);
						}
						baseR.AddForce(Vector3.up * 200f, ForceMode.Acceleration);
						string text = "FX/shotGun";
						if (flag4)
						{
							text = "FX/shotGun 1";
						}
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && basePV.isMine)
						{
							GameObject gameObject2 = PhotonNetwork.Instantiate(text, baseT.position + baseT.up * 0.8f - baseT.right * 0.1f, baseT.rotation, 0);
							if (gameObject2.GetComponent<EnemyfxIDcontainer>() != null)
							{
								gameObject2.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = basePV.viewID;
							}
						}
						else
						{
							GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load(text), baseT.position + baseT.up * 0.8f - baseT.right * 0.1f, baseT.rotation);
						}
					}
					if (baseA[attackAnimation].normalizedTime >= 1f)
					{
						falseAttack();
						idle();
					}
					if (!baseA.IsPlaying(attackAnimation))
					{
						falseAttack();
						idle();
					}
					break;
				case HERO_STATE.ChangeBlade:
					if (usingAhss)
					{
						if (baseA[reloadAnimation].normalizedTime > 0.22f)
						{
							if (!leftGunHasBullet && setup.part_blade_l.activeSelf)
							{
								setup.part_blade_l.SetActive(value: false);
								Transform transform4 = setup.part_blade_l.transform;
								GameObject obj = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_gun_l"), transform4.position, transform4.rotation);
								obj.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
								Vector3 force = -baseT.forward * 10f + baseT.up * 5f - baseT.right;
								obj.rigidbody.AddForce(force, ForceMode.Impulse);
								Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
								obj.rigidbody.AddTorque(torque, ForceMode.Acceleration);
							}
							if (!rightGunHasBullet && setup.part_blade_r.activeSelf)
							{
								setup.part_blade_r.SetActive(value: false);
								Transform transform5 = setup.part_blade_r.transform;
								GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("Character_parts/character_gun_r"), transform5.position, transform5.rotation);
								obj2.renderer.material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
								Vector3 force2 = -baseT.forward * 10f + baseT.up * 5f + baseT.right;
								obj2.rigidbody.AddForce(force2, ForceMode.Impulse);
								Vector3 torque2 = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300));
								obj2.rigidbody.AddTorque(torque2, ForceMode.Acceleration);
							}
						}
						if (baseA[reloadAnimation].normalizedTime > 0.62f && !throwedBlades)
						{
							throwedBlades = true;
							if (leftBulletLeft > 0 && !leftGunHasBullet)
							{
								if (!InRoomChat.god) leftBulletLeft--;
								setup.part_blade_l.SetActive(value: true);
								leftGunHasBullet = true;
							}
							if (rightBulletLeft > 0 && !rightGunHasBullet)
							{
								setup.part_blade_r.SetActive(value: true);
								if (!InRoomChat.god) rightBulletLeft--;
								rightGunHasBullet = true;
							}
							updateRightMagUI();
							updateLeftMagUI();
						}
						if (baseA[reloadAnimation].normalizedTime > 1f)
						{
							idle();
						}
						break;
					}
					if (!grounded)
					{
						if (baseA[reloadAnimation].normalizedTime >= 0.2f && !throwedBlades)
						{
							throwedBlades = true;
							if (setup.part_blade_l.activeSelf)
							{
								throwBlades();
							}
						}
						if (baseA[reloadAnimation].normalizedTime >= 0.56f && currentBladeNum > 0)
						{
							setup.part_blade_l.SetActive(value: true);
							setup.part_blade_r.SetActive(value: true);
							currentBladeSta = totalBladeSta;
						}
					}
					else
					{
						if (baseA[reloadAnimation].normalizedTime >= 0.13f && !throwedBlades)
						{
							throwedBlades = true;
							if (setup.part_blade_l.activeSelf)
							{
								throwBlades();
							}
						}
						if (baseA[reloadAnimation].normalizedTime >= 0.37f && currentBladeNum > 0)
						{
							setup.part_blade_l.SetActive(value: true);
							setup.part_blade_r.SetActive(value: true);
							currentBladeSta = totalBladeSta;
						}
					}
					if (baseA[reloadAnimation].normalizedTime >= 1f)
					{
						idle();
					}
					break;
				case HERO_STATE.Salute:
					if (baseA["salute"].normalizedTime >= 1f)
					{
						idle();
					}
					break;
				case HERO_STATE.GroundDodge:
					if (baseA.IsPlaying("dodge"))
					{
						if (!grounded && !(baseA["dodge"].normalizedTime <= 0.6f))
						{
							idle();
						}
						if (baseA["dodge"].normalizedTime >= 1f)
						{
							idle();
						}
					}
					break;
				case HERO_STATE.Land:
					if (baseA.IsPlaying("dash_land") && baseA["dash_land"].normalizedTime >= 1f)
					{
						idle();
					}
					break;
				case HERO_STATE.FillGas:
					if (baseA.IsPlaying("supply") && baseA["supply"].normalizedTime >= 1f)
					{
						currentBladeSta = totalBladeSta;
						currentBladeNum = totalBladeNum;
						currentGas = totalGas;
						if (!usingAhss)
						{
							setup.part_blade_l.SetActive(value: true);
							setup.part_blade_r.SetActive(value: true);
						}
						else
						{
							leftBulletLeft = (rightBulletLeft = bulletMAX);
							leftGunHasBullet = (rightGunHasBullet = true);
							setup.part_blade_l.SetActive(value: true);
							setup.part_blade_r.SetActive(value: true);
							updateRightMagUI();
							updateLeftMagUI();
						}
						idle();
					}
					break;
				case HERO_STATE.Slide:
					if (!grounded)
					{
						idle();
					}
					break;
				case HERO_STATE.AirDodge:
					if (dashTime > 0f)
					{
						dashTime -= Time.deltaTime;
						if (currentSpeed > originVM)
						{
							baseR.AddForce(-baseR.velocity * Time.deltaTime * 1.7f, ForceMode.VelocityChange);
						}
					}
					else
					{
						dashTime = 0f;
						idle();
					}
					break;
			}
			if (FengCustomInputs.Inputs.isInput[InputCode.leftRope] && ((!baseA.IsPlaying("attack3_1") && !baseA.IsPlaying("attack5") && !baseA.IsPlaying("special_petra") && state != HERO_STATE.Grab) || state == HERO_STATE.Idle))
			{
				if (bulletLeft != null)
				{
					QHold = true;
				}
				else
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
					if (Physics.Raycast(ray, out var hitInfo4, 10000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask)).value))
					{
						launchLeftRope(hitInfo4, single: true, 0);
						//rope.Play();
					}
				}
			}
			else
			{
				QHold = false;
			}
			if (FengCustomInputs.Inputs.isInput[InputCode.rightRope] && ((!baseA.IsPlaying("attack3_1") && !baseA.IsPlaying("attack5") && !baseA.IsPlaying("special_petra") && state != HERO_STATE.Grab) || state == HERO_STATE.Idle))
			{
				if (bulletRight != null)
				{
					EHold = true;
				}
				else
				{
					Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
					LayerMask layerMask2 = 1 << LayerMask.NameToLayer("Ground");
					if (Physics.Raycast(ray2, out var hitInfo5, 10000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask2)).value))
					{
						launchRightRope(hitInfo5, single: true);
						//rope.Play();
					}
				}
			}
			else
			{
				EHold = false;
			}
			if (FengCustomInputs.Inputs.isInput[InputCode.bothRope] && ((!baseA.IsPlaying("attack3_1") && !baseA.IsPlaying("attack5") && !baseA.IsPlaying("special_petra") && state != HERO_STATE.Grab) || state == HERO_STATE.Idle))
			{
				QHold = true;
				EHold = true;
				if (bulletLeft == null && bulletRight == null)
				{
					Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
					LayerMask layerMask3 = 1 << LayerMask.NameToLayer("Ground");
					if (Physics.Raycast(ray3, out var hitInfo6, 1000000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask3)).value))
					{
						launchLeftRope(hitInfo6, single: false, 0);
						launchRightRope(hitInfo6, single: false);
						//rope.Play();
					}
				}
			}
			if (IN_GAME_MAIN_CAMERA.gametype != 0 || (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !IN_GAME_MAIN_CAMERA.isPausing))
			{
				calcSkillCD();
				calcFlareCD();
			}
			if (!useGun)
			{
				if (leftbladetrail.gameObject.GetActive())
				{
					leftbladetrail.update();
					rightbladetrail.update();
				}
				if (leftbladetrail2.gameObject.GetActive())
				{
					leftbladetrail2.update();
					rightbladetrail2.update();
				}
				if (leftbladetrail.gameObject.GetActive())
				{
					leftbladetrail.lateUpdate();
					rightbladetrail.lateUpdate();
				}
				if (leftbladetrail2.gameObject.GetActive())
				{
					leftbladetrail2.lateUpdate();
					rightbladetrail2.lateUpdate();
				}
			}
			if (!IN_GAME_MAIN_CAMERA.isPausing)
			{
				if (!PhotonNetwork.offlineMode && IN_GAME_MAIN_CAMERA.gametype != 0)
				{
					ReceiveSendMount();
				}
				showSkillCD();
				if (GameSettings.BottomHUD > 0)
				{
					showFlareCD2();
					showGas2();
				}
				showAimUI2();
			}
		}
		else if (isCannon && !IN_GAME_MAIN_CAMERA.isPausing)
		{
			showAimUI2();
			calcSkillCD();
			showSkillCD();
		}
  //      if (GameSettings.Slide > 0)
		//{
		//	if (state == HERO_STATE.Slide && !CustomAudioManager.AudioIsPlaying(CustomAudioManager.AudioFile.Slide))
		//	{
		//		//CustomAudioManager.Play(CustomAudioManager.AudioFile.Slide);
		//	}
		//	if (state != HERO_STATE.Slide && CustomAudioManager.AudioIsPlaying(CustomAudioManager.AudioFile.Slide))
		//	{
		//		CustomAudioManager.Stop(CustomAudioManager.AudioFile.Slide);
		//	}
		//}
		if (GameSettings.bombMode > 0 && Input.GetKeyDown(FengGameManagerMKII.BombSpecialKey))
		{
			if ((int)FengGameManagerMKII.settings[291] == 1)
			{
				FengGameManagerMKII.settings[291] = 0;
			}
			else
			{
				FengGameManagerMKII.settings[291] = 1;
			}
		}
    }

	public static HERO FindMyHero(int ownerID)
	{
		RandomClass randomClass = new RandomClass();
		randomClass.randomInt = ownerID;
		GameObject[] source = GameObject.FindGameObjectsWithTag("Player");
		if (function == null)
		{
			function = getComp;
		}
		List<HERO> source2 = source.Select(function).ToList();
		source2 = source2.Where(randomClass.Check).ToList();
		if (source2.Count != 0)
		{
			return source2[0];
		}
		return null;
	}

	private static HERO getComp(GameObject go)
	{
		return go.GetComponent<HERO>();
	}

	[RPC]
	private void MountCharacter(int targetViewID)
	{
		GameObject gameObject = PhotonView.Find(targetViewID).gameObject;
		if (gameObject.GetPhotonView().isMine)
		{
			gameObject.GetComponent<HERO>().CarryingSomeone = true;
			gameObject.GetComponent<HERO>().PersonBeingCarried = this;
		}
		GetComponent<SmoothSyncMovement>().disabled = true;
		base.transform.parent = gameObject.FindChild("head").transform;
		base.transform.localPosition = (Vector3.right + Vector3.back * 0.3f) / gameObject.FindChild("head").lossyScale.x * 0.5f;
		base.transform.localRotation = Quaternion.Euler(0f, -20f, 90f);
		base.transform.rigidbody.isKinematic = true;
		SetInterpolationIfEnabled(interpolate: false);
	}

	[RPC]
	private void UnmountCharacter(int carrierViewID, PhotonMessageInfo msgInfo)
	{
		if (PhotonView.Find(carrierViewID) != null)
		{
			GameObject gameObject = PhotonView.Find(carrierViewID).gameObject;
			if (gameObject.GetPhotonView().isMine)
			{
				gameObject.GetComponent<HERO>().CarryingSomeone = false;
				gameObject.GetComponent<HERO>().PersonBeingCarried = null;
			}
		}
		if (base.photonView.isMine)
		{
			PersonWhoCarries = null;
			BeingCarried = false;
		}
		GetComponent<SmoothSyncMovement>().disabled = false;
		base.transform.parent = null;
		base.transform.rigidbody.isKinematic = false;
		SetInterpolationIfEnabled(interpolate: true);
	}

	[RPC]
	private void AskForMount(PhotonMessageInfo msgInfo)
	{
		sender = msgInfo.sender;
		StartCoroutine(ReactToRequestString(6, msgInfo.sender.uiname.StripHex()));
	}

	[RPC]
	private void AcceptMount(PhotonMessageInfo msgInfo)
	{
		base.photonView.RPC("MountCharacter", PhotonTargets.All, FindMyHero(msgInfo.sender.ID).photonView.viewID);
		BeingCarried = true;
		crossFade("horse_idle", 0.1f);
		PersonWhoCarries = FindMyHero(msgInfo.sender.ID);
	}

	private IEnumerator ReactToRequestString(int seconds, string name)
	{
		float num = 0f;
		BoolWhileBeingAsked = true;
		while (num < (float)seconds && BoolWhileBeingAsked)
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowHUDInfoTopCenter(name + " asks to be carried. Press [FFFF00]Alpha5[-] to accept.\nPress [FFFF00]Alpha6[-] to decline.");
			num += Time.deltaTime;
			yield return null;
		}
		BoolWhileBeingAsked = false;
	}

	private IEnumerator AskToBeCarriedString(int seconds, string name)
	{
		float num = 0f;
		BoolWhileAskingToBeCarried = true;
		while (num < (float)seconds && BoolWhileAskingToBeCarried)
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowHUDInfoTopCenter("Press[FFFF00] Alpha4[-] to ask " + name + "[-] to carry you.\n");
			num += Time.deltaTime;
			yield return null;
		}
		BoolWhileAskingToBeCarried = false;
	}

	public static HERO NearestPlayer(HERO aHERO)
	{
		HERO result = null;
		float num = float.PositiveInfinity;
		Vector3 position = aHERO.transform.position;
		HERO[] array = UnityEngine.Object.FindObjectsOfType(typeof(HERO)) as HERO[];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].photonView.owner.isLocal)
			{
				continue;
			}
			_ = IN_GAME_MAIN_CAMERA.mainCamera;
			Transform obj = array[i].transform;
			Vector3 vector = obj.position + Vector3.up * 0.6f + array[i].rigidbody.velocity * 30f * Time.deltaTime - aHERO.transform.position;
			_ = obj.position + Vector3.up * 0.6f + array[i].rigidbody.velocity * (vector.magnitude / (aHERO.bombSpeed * Time.deltaTime)) * Time.deltaTime;
			if (vector.magnitude < 500f)
			{
				float sqrMagnitude = (array[i].transform.position - position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = array[i];
					num = sqrMagnitude;
				}
			}
		}
		return result;
	}

	private void ReceiveSendMount()
	{
		if (!BoolWhileBeingAsked)
		{
			HERO hERO = NearestPlayer(this);
			if (hERO != null && Vector3.Distance(hERO.transform.position, base.transform.position) < 15f)
			{
				string content = string.Concat(new string[3]
				{
					"Press [FFFF00] Alpha4[-] to ask ",
					(string)hERO.photonView.owner.customProperties["name"],
					"[-] to carry you.\n"
				});
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ShowHUDInfoTopCenter(content);
				if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					hERO.photonView.RPC("AskForMount", hERO.photonView.owner);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) && BoolWhileBeingAsked)
		{
			BoolWhileBeingAsked = false;
			FindMyHero(sender.ID).photonView.RPC("AcceptMount", sender);
			sender = null;
			CarryingSomeone = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			if (BoolWhileBeingAsked)
			{
				BoolWhileBeingAsked = false;
			}
			if (CarryingSomeone)
			{
				base.gameObject.FindChild("head").FindChild("AOTTG_HERO 1(Clone)").gameObject.GetComponent<HERO>().photonView.RPC("UnmountCharacter", PhotonTargets.AllBuffered, base.photonView.viewID);
			}
			if (PersonWhoCarries != null && BeingCarried)
			{
				base.photonView.RPC("UnmountCharacter", PhotonTargets.AllBuffered, base.transform.root.gameObject.GetPhotonView().viewID);
			}
		}
	}

	public void updateCannon()
	{
		baseT.position = myCannonPlayer.position;
		baseT.rotation = myCannonBase.rotation;
	}

	private void updateLeftMagUI()
	{
		for (int i = 1; i <= bulletMAX; i++)
		{
			CacheGameObject.Find<UISprite>("bulletL" + i).enabled = false;
		}
		for (int j = 1; j <= leftBulletLeft; j++)
		{
			CacheGameObject.Find<UISprite>("bulletL" + j).enabled = true;
		}
	}

	private void updateRightMagUI()
	{
		for (int i = 1; i <= bulletMAX; i++)
		{
			CacheGameObject.Find<UISprite>("bulletR" + i).enabled = false;
		}
		for (int j = 1; j <= rightBulletLeft; j++)
		{
			CacheGameObject.Find<UISprite>("bulletR" + j).enabled = true;
		}
	}

	public void useBlade(int amount)
	{
		if (!InRoomChat.god)
		{
			if (amount == 0)
			{
				amount = 1;
			}
			amount *= 2;
			if (!(currentBladeSta > 0f))
			{
				return;
			}
			currentBladeSta -= amount;
			if (currentBladeSta <= 0f)
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || basePV.isMine)
				{
					leftbladetrail.Deactivate();
					rightbladetrail.Deactivate();
					leftbladetrail2.Deactivate();
					rightbladetrail2.Deactivate();
					triggerLeft.active_me = false;
					triggerRight.active_me = false;
				}
				currentBladeSta = 0f;
				throwBlades();
			}
		}
	}

	private void useGas(float amount)
	{
		if (!InRoomChat.god)
		{
			if (amount == 0f)
			{
				amount = useGasSpeed;
			}
			amount *= GasUsageModifier;
			if (currentGas > 0f && GameSettings.bombMode <= 0)
			{
				currentGas -= amount;
				if (currentGas < 0f)
				{
					currentGas = 0f;
				}
			}
		}
	}

	[RPC]
	private void whoIsMyErenTitan(int id, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "messing w/ eren transform");
			return;
		}
		eren_titan = PhotonView.Find(id).gameObject;
		titanForm = true;
	}
}
