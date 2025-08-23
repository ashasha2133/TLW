using BRM;
using Photon;
using UnityEngine;

public class HERO_ON_MENU : Photon.MonoBehaviour
{
	private Vector3 cameraOffset;

	private Transform cameraPref;

	public int costumeId;

	private Transform head;

	public float headRotationX;

	public float headRotationY;

	private Transform mainCameraT;

	private static Vector3 ThunderPos;

	private static Vector3 TitanDiePos;

	private static Object thunder = CacheResources.Load("FX/Thunder");

	private float time;

	private float time2;

	private static Object smoke = CacheResources.Load("FX/colossal_steam");

	private static Quaternion TitanDieQuat = Quaternion.Euler(-90f, 0f, 0f);

	private static Quaternion ThunderQuat = Quaternion.Euler(270f, 0f, 0f);

	private void Awake()
	{
		QualitySettings.vSyncCount = 0;
		ChangeQuality.setCurrentQuality();
	}

	private void LateUpdate()
	{
		if (costumeId == 9)
		{
			time += Time.fixedDeltaTime;
			time2 += Time.fixedDeltaTime;
			if (time > Mathf.Min(Random.Range(0.25f, 1.55f), 0.89f))
			{
				time = 0f;
				Object.Instantiate(thunder, ThunderPos, ThunderQuat * Quaternion.Inverse(mainCameraT.rotation));
			}
			if (time2 > Random.Range(5f, 15f))
			{
				time2 = 0f;
				((GameObject)Object.Instantiate(smoke, TitanDiePos, TitanDieQuat)).transform.rotation *= Quaternion.Inverse(mainCameraT.rotation);
			}
			if (QualitySettings.masterTextureLimit != 0)
			{
				QualitySettings.masterTextureLimit = 0;
			}
		}
	}

	private void Start()
	{
		HERO_SETUP component = base.gameObject.GetComponent<HERO_SETUP>();
		HeroCostume.init2();
		component.init();
		component.myCostume = HeroCostume.costume[costumeId];
		component.setCharacterComponent();
		head = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
		cameraPref = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
		if (costumeId == 9)
		{
			cameraOffset = GameObject.Find("MainCamera_Mono").transform.position - cameraPref.position;
		}
		if (component.myCostume.sex == SEX.FEMALE)
		{
			base.animation.Play("stand");
			base.animation["stand"].normalizedTime = Random.Range(0f, 1f);
		}
		else
		{
			base.animation.Play("stand_levi");
			base.animation["stand_levi"].normalizedTime = Random.Range(0f, 1f);
		}
		float speed = 0.5f;
		base.animation["stand"].speed = speed;
		base.animation["stand_levi"].speed = speed;
	}
}
