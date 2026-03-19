using System;
using System.Linq;
using Boo.Lang;
using BRM;
using UnityEngine;

public class IN_GAME_MAIN_CAMERA : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}

	public RotationAxes axes;

	//public AudioSource bgmusic;

	public static float cameraDistance = 0.6f;

	public static int cameraTilt = 1;

	public static int character = 1;

	private float closestDistance;

	private int currentPeekPlayerIndex;

	public static DayLight dayLight = DayLight.Dawn;

	private float decay;

	public static int difficulty;

	private float distance = 10f;

	private float distanceMulti;

	private float distanceOffsetMulti;

	private float duration;

	private float flashDuration;

	private bool flip;

	public static GAMEMODE gamemode;

	public bool gameOver;

	public static GAMETYPE gametype = GAMETYPE.STOP;

	private bool hasSnapShot;

	public float heightMulti;

	public static bool isCheating;

	public static bool isPausing;

	public static bool isTyping;

	public float justHit;

	public int lastScore;

	public static int level;

	private bool lockAngle;

	private Vector3 lockCameraPosition;

	private GameObject lockTarget;

	public float maximumX = 360f;

	public float maximumY = 60f;

	public float minimumX = -360f;

	public float minimumY = -60f;

	private bool needSetHUD;

	private float R;

	public float rotationY;

	public int score;

	public static float sensitivityMulti = 0.5f;

	public static string singleCharacter;

	public Material skyBoxDAWN;

	public Material skyBoxDAY;

	public Material skyBoxNIGHT;

	private Texture2D snapshot1;

	private Texture2D snapshot2;

	private Texture2D snapshot3;

	public GameObject snapShotCamera;

	private int snapShotCount;

	private float snapShotCountDown;

	private int snapShotDmg;

	private float snapShotInterval = 0.02f;

	public RenderTexture snapshotRT;

	private float snapShotStartCountDownTime;

	private GameObject snapShotTarget;

	private Vector3 snapShotTargetPosition;

	public bool spectatorMode;

	private bool startSnapShotFrameCount;

	public static STEREO_3D_TYPE stereoType;

	public float timer;

	public static bool triggerAutoLock;

	public static bool usingTitan;

	private Vector3 verticalHeightOffset = Vector3.zero;

	public static CameraShake shake;

	private static Light mainLightL;

	public static Skybox skybox;

	public static IN_GAME_MAIN_CAMERA mainCamera;

	private static Transform head;

	public static GameObject mainG;

	public static Transform mainT;

	public static Camera mainC;

	private static Transform headT;

	private static Transform snapT;

	public static GameObject mainobject;

	public static TiltShift tiltshift;

	public static SpectatorMovement spectate;

	public static MouseLook mouselook;

	private static Transform lockTargetT;

	private Transform locker;

	public static CAMERA_TYPE cameraMode = CAMERA_TYPE.ORIGINAL;

	public static bool isReady = false;

	public static GameObject main_object
	{
		get
		{
			return mainobject;
		}
		set
		{
			mainobject = value;
			if (value != null)
			{
				main_objectT = value.transform;
				if (main_objectT == null)
				{
					mainobject = null;
					head = null;
					headT = null;
					mainHERO = null;
					mainTITAN = null;
					main_objectT = null;
					main_objectR = null;
					return;
				}
				main_objectR = value.rigidbody;
				MONO component = value.GetComponent<MONO>();
				if (component.species == SPECIES.Hero)
				{
					mainHERO = component as HERO;
					mainTITAN = null;
				}
				else if (component.species == SPECIES.Titan)
				{
					mainTITAN = component as TITAN;
					mainHERO = null;
				}
				else
				{
					mainHERO = null;
					mainTITAN = null;
				}
				head = main_objectT.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
				if (head == null)
				{
					mainobject = null;
					head = null;
					headT = null;
					mainHERO = null;
					mainTITAN = null;
					main_objectT = null;
					main_objectR = null;
					return;
				}
				headT = head.transform;
				if (headT == null)
				{
					mainobject = null;
					head = null;
					headT = null;
					mainHERO = null;
					mainTITAN = null;
					main_objectT = null;
					main_objectR = null;
				}
			}
			else
			{
				head = null;
				headT = null;
				mainHERO = null;
				mainTITAN = null;
				main_objectT = null;
				main_objectR = null;
			}
		}
	}

	private static Vector3 MainObjectPosition
	{
		get
		{
			if (headT != null || (head != null && (headT = head.transform) != null))
			{
				return headT.position;
			}
			if (main_objectT != null)
			{
				return main_objectT.position;
			}
			if (main_object != null)
			{
				return (main_objectT = main_object.transform).position;
			}
			if (mainT == null && (Camera.main == null || (mainT = Camera.main.transform) == null))
			{
				return Vector3.zero;
			}
			return mainT.position;
		}
	}

	public static Transform main_objectT { get; private set; }

	public static Rigidbody main_objectR { get; private set; }

	public static HERO mainHERO { get; private set; }

	public static TITAN mainTITAN { get; private set; }

	public void UpdateSpecmode()
	{
		List<GameObject> list = new List<GameObject>(FengGameManagerMKII.allheroes.Where((GameObject h) => h != null));
		currentPeekPlayerIndex--;
		if (currentPeekPlayerIndex >= list.Count)
		{
			currentPeekPlayerIndex = 0;
		}
		if (currentPeekPlayerIndex < 0)
		{
			currentPeekPlayerIndex = list.Count - 1;
		}
		if (list.Count > 0)
		{
			setMainObject(list[currentPeekPlayerIndex]);
			setSpectorMode(val: false);
			lockAngle = false;
		}
	}

	private void Awake()
	{
		isTyping = false;
		isPausing = false;
		mainCamera = this;
		base.name = "MainCamera";
		mainC = Camera.main;
		mainT = Camera.main.transform;
		mainG = Camera.main.gameObject;
		tiltshift = GetComponent<TiltShift>();
		tiltshift.enabled = !PlayerPrefs.HasKey("GameQuality") || PlayerPrefs.GetFloat("GameQuality") >= 0.9f;
		mainLightL = CacheGameObject.Find<Light>("mainLight");
		skybox = GetComponent<Skybox>();
		rotationY = 0f;
		spectate = GetComponent<SpectatorMovement>();
		mouselook = GetComponent<MouseLook>();
		shake = GetComponent<CameraShake>();
		ChangeFog();
	}

	private void ChangeLight()
	{
		mainLightL = CacheGameObject.Find<Light>("mainLight");
		switch ((int)FengGameManagerMKII.settings[325])
		{
		case 1:
			if (mainLightL.color != Color.red)
			{
				mainLightL.color = Color.red;
			}
			break;
		case 2:
			if (mainLightL.color != Color.yellow)
			{
				mainLightL.color = Color.yellow;
			}
			break;
		case 3:
			if (mainLightL.color != Color.green)
			{
				mainLightL.color = Color.green;
			}
			break;
		case 4:
			if (mainLightL.color != Color.blue)
			{
				mainLightL.color = Color.blue;
			}
			break;
		case 5:
			if (mainLightL.color != Color.cyan)
			{
				mainLightL.color = Color.cyan;
			}
			break;
		case 6:
			if (mainLightL.color != Color.magenta)
			{
				mainLightL.color = Color.magenta;
			}
			break;
		case 7:
			if (mainLightL.color != Color.gray)
			{
				mainLightL.color = Color.gray;
			}
			break;
		case 8:
			if (mainLightL.color != Color.black)
			{
				mainLightL.color = Color.black;
			}
			break;
		}
	}

	private void ChangeFog()
	{
		switch ((int)FengGameManagerMKII.settings[327])
		{
		case 0:
			if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("Forest"))
			{
				if ((int)FengGameManagerMKII.settings[299] == 0)
				{
					RenderSettings.fog = true;
					RenderSettings.fogColor = new Color(0.094f, 0.094f, 0.094f, 0.396f);
				}
				else
				{
					RenderSettings.fog = false;
				}
			}
			else
			{
				RenderSettings.fog = false;
			}
			break;
		case 1:
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.gray;
			break;
		case 2:
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.yellow;
			break;
		case 3:
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.green;
			break;
		case 4:
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.blue;
			break;
		case 5:
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.blue;
			break;
		case 6:
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.magenta;
			break;
		case 7:
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.white;
			break;
		case 8:
			RenderSettings.fog = true;
			RenderSettings.fogColor = Color.red;
			break;
		}
	}

	private void camareMovement()
	{
		float y = mainT.eulerAngles.y;
		Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
		float z = mainT.eulerAngles.z;
		distanceOffsetMulti = cameraDistance * (200f - mainC.fieldOfView) / 150f;
		mainT.position = MainObjectPosition;
		mainT.position += Vector3.up * heightMulti;
		mainT.position -= Vector3.up * (0.6f - cameraDistance) * 2f;
		switch (cameraMode)
		{
		case CAMERA_TYPE.ORIGINAL:
			if (Input.mousePosition.x < (float)Screen.width * 0.4f)
			{
				mainT.RotateAround(mainT.position, Vector3.up, (0f - ((float)Screen.width * 0.4f - Input.mousePosition.x) / (float)Screen.width * 0.4f) * getSensitivityMultiWithDeltaTime() * 150f);
			}
			else if (Input.mousePosition.x > (float)Screen.width * 0.6f)
			{
				mainT.RotateAround(mainT.position, Vector3.up, (Input.mousePosition.x - (float)Screen.width * 0.6f) / (float)Screen.width * 0.4f * getSensitivityMultiWithDeltaTime() * 150f);
			}
			mainT.rotation = Quaternion.Euler(140f * ((float)Screen.height * 0.6f - Input.mousePosition.y) / (float)Screen.height * 0.5f, mainT.rotation.eulerAngles.y, mainT.rotation.eulerAngles.z);
			mainT.position -= mainT.forward * distance * distanceMulti * distanceOffsetMulti;
			break;
		case CAMERA_TYPE.TPS:
		{
			if (!FengCustomInputs.Inputs.menuOn && !gameOver)
			{
				Screen.lockCursor = true;
			}
			float angle = 0f;
			float num = 0f;
			if ((int)FengGameManagerMKII.settings[300] == 0)
			{
				angle = Input.GetAxis("Mouse X") * 10f * getSensitivityMulti();
				num = (0f - Input.GetAxis("Mouse Y")) * 10f * getSensitivityMulti();
			}
			else if ((int)FengGameManagerMKII.settings[300] == 1)
			{
				angle = Input.GetAxisRaw("Mouse X") * 10f * getSensitivityMulti();
				num = (0f - Input.GetAxisRaw("Mouse Y")) * 10f * getSensitivityMulti();
			}
			mainT.RotateAround(mainT.position, Vector3.up, angle);
			float num2 = mainT.rotation.eulerAngles.x % 360f;
			float num3 = num2 + num;
			if ((num <= 0f || ((num2 >= 260f || num3 <= 260f) && (num2 >= 80f || num3 <= 80f))) && (num >= 0f || ((num2 <= 280f || num3 >= 280f) && (num2 <= 100f || num3 >= 100f))))
			{
				mainT.RotateAround(mainT.position, mainT.right, num);
			}
			mainT.position -= mainT.forward * distance * distanceMulti * distanceOffsetMulti;
			break;
		}
		case CAMERA_TYPE.NewTPS:
			if (!FengCustomInputs.Inputs.menuOn)
			{
				Screen.lockCursor = true;
			}
			rotationY += Input.GetAxis("Mouse Y") * 2.5f * (sensitivityMulti * 2f);
			rotationY = Mathf.Clamp(rotationY, -60f, 60f);
			rotationY = Mathf.Max(rotationY, -999f + heightMulti * 2f);
			rotationY = Mathf.Min(rotationY, 999f);
			mainT.localEulerAngles = new Vector3(0f - rotationY, mainT.localEulerAngles.y + Input.GetAxis("Mouse X") * 2.5f * (sensitivityMulti * 2f), z);
			quaternion = Quaternion.Euler(0f, mainT.eulerAngles.y, 0f);
			mainT.position -= quaternion * Vector3.forward * 10f * distanceMulti * distanceOffsetMulti;
			mainT.position += -Vector3.up * rotationY * 0.1f * (float)Math.Pow(heightMulti, 1.1) * distanceOffsetMulti;
			break;
		}
		if (cameraDistance < 0.65f)
		{
			mainT.position += mainT.right * Mathf.Max((0.6f - cameraDistance) * 2f, 0.65f);
		}
	}

	public void CameraMovementLive(HERO hero)
	{
		float magnitude = hero.rigidbody.velocity.magnitude;
		if (magnitude > 10f)
		{
			mainC.fieldOfView = Mathf.Lerp(mainC.fieldOfView, Mathf.Min(100f, magnitude + 40f), 0.1f);
		}
		else
		{
			mainC.fieldOfView = Mathf.Lerp(mainC.fieldOfView, 50f, 0.1f);
		}
		float num = hero.CameraMultiplier * (200f - mainC.fieldOfView) / 150f;
		mainT.position = headT.position + Vector3.up * heightMulti - Vector3.up * (0.6f - cameraDistance) * 2f;
		mainT.position -= mainT.forward * distance * distanceMulti * num;
		if (hero.CameraMultiplier < 0.65f)
		{
			mainT.position += mainT.right * Mathf.Max((0.6f - hero.CameraMultiplier) * 2f, 0.65f);
		}
		mainT.rotation = Quaternion.Lerp(mainC.transform.rotation, hero.GetComponent<SmoothSyncMovement>().correctCameraRot, Time.deltaTime * 5f);
	}

	public void createSnapShotRT()
	{
		if (snapShotCamera.GetComponent<Camera>().targetTexture != null)
		{
			snapShotCamera.GetComponent<Camera>().targetTexture.Release();
		}
		if (QualitySettings.GetQualityLevel() > 3)
		{
			snapShotCamera.GetComponent<Camera>().targetTexture = new RenderTexture((int)((float)Screen.width * 0.8f), (int)((float)Screen.height * 0.8f), 24);
		}
		else
		{
			snapShotCamera.GetComponent<Camera>().targetTexture = new RenderTexture((int)((float)Screen.width * 0.4f), (int)((float)Screen.height * 0.4f), 24);
		}
	}

	public void createSnapShotRT2()
	{
		if (snapshotRT != null)
		{
			snapshotRT.Release();
		}
		if (QualitySettings.GetQualityLevel() > 3)
		{
			snapshotRT = new RenderTexture((int)((float)Screen.width * 0.8f), (int)((float)Screen.height * 0.8f), 24);
			snapShotCamera.GetComponent<Camera>().targetTexture = snapshotRT;
		}
		else
		{
			snapshotRT = new RenderTexture((int)((float)Screen.width * 0.4f), (int)((float)Screen.height * 0.4f), 24);
			snapShotCamera.GetComponent<Camera>().targetTexture = snapshotRT;
		}
	}

	private GameObject findNearestTitan()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		GameObject result = null;
		float num = (closestDistance = float.PositiveInfinity);
		Vector3 position = main_objectT.position;
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			float magnitude = (gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position - position).magnitude;
			if (magnitude < num && (gameObject.GetComponent<TITAN>() == null || !gameObject.GetComponent<TITAN>().hasDie))
			{
				result = gameObject;
				num = (closestDistance = magnitude);
			}
		}
		return result;
	}

	public void flashBlind()
	{
		CacheGameObject.Find("flash").GetComponent<UISprite>().alpha = 1f;
		flashDuration = 2f;
	}

	public float getSensitivityMulti()
	{
		return sensitivityMulti;
	}

	public float getSensitivityMultiWithDeltaTime()
	{
		return sensitivityMulti * Time.deltaTime * 62f;
	}

	private void OnDestroy()
	{
		mainCamera = null;
		if (Camera.main == null)
		{
			mainC = null;
			mainT = null;
			mainG = null;
		}
		else
		{
			mainC = Camera.main;
			mainT = Camera.main.transform;
			mainG = Camera.main.gameObject;
		}
		skybox = null;
		tiltshift = null;
		spectate = null;
		mouselook = null;
	}

	private void reset()
	{
		if (gametype == GAMETYPE.SINGLE)
		{
			FengGameManagerMKII.instance.restartGameSingle2();
		}
	}

	private Texture2D RTImage(Camera cam)
	{
		RenderTexture renderTexture = RenderTexture.active;
		RenderTexture.active = cam.targetTexture;
		cam.Render();
		Texture2D texture2D = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
		int num = (int)((float)cam.targetTexture.width * 0.04f);
		int num2 = (int)((float)cam.targetTexture.width * 0.02f);
		texture2D.ReadPixels(new Rect(num, num, cam.targetTexture.width - num, cam.targetTexture.height - num), num2, num2);
		texture2D.Apply();
		RenderTexture.active = renderTexture;
		return texture2D;
	}

	private Texture2D RTImage2(Camera cam)
	{
		RenderTexture renderTexture = RenderTexture.active;
		RenderTexture.active = cam.targetTexture;
		cam.Render();
		Texture2D texture2D = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
		int num = (int)((float)cam.targetTexture.width * 0.04f);
		int num2 = (int)((float)cam.targetTexture.width * 0.02f);
		texture2D.SetPixel(0, 0, Color.white);
		texture2D.ReadPixels(new Rect(num, num, cam.targetTexture.width - num, cam.targetTexture.height - num), num2, num2);
		texture2D.Apply();
		RenderTexture.active = renderTexture;
		return texture2D;
	}

	public void setDayLight(DayLight val)
	{
		dayLight = val;
		if (dayLight == DayLight.Day)
		{
			RenderSettings.ambientLight = FengColor.dayAmbientLight;
			mainLightL.color = FengColor.dayLight;
			skybox.material = skyBoxDAY;
		}
		if (dayLight == DayLight.DayLight)
		{
			RenderSettings.ambientLight = FengColor.dayLight;
			skybox.material = skyBoxDAY;
		}
		if (dayLight == DayLight.Dawn)
		{
			RenderSettings.ambientLight = FengColor.dawnAmbientLight;
			mainLightL.color = FengColor.dawnAmbientLight;
			skybox.material = skyBoxDAWN;
		}
		if (dayLight == DayLight.DawnLight)
		{
			RenderSettings.ambientLight = FengColor.dawnLight;
			skybox.material = skyBoxDAWN;
		}
		if (dayLight == DayLight.Night)
		{
			Transform obj = ((GameObject)UnityEngine.Object.Instantiate(CacheResources.Load("flashlight"))).transform;
			obj.parent = mainT;
			obj.position = mainT.position;
			obj.rotation = Quaternion.Euler(353f, 0f, 0f);
			RenderSettings.ambientLight = FengColor.nightAmbientLight;
			mainLightL.color = FengColor.nightLight;
			skybox.material = skyBoxNIGHT;
		}
		snapShotCamera.gameObject.GetComponent<Skybox>().material = skybox.material;
	}

	public void setHUDposition()
	{
		float num = (float)(-Screen.width) * 0.5f;
		float num2 = (float)(-Screen.height) * 0.5f;
		float num3 = (float)Screen.height * 0.5f;
		float num4 = (float)Screen.width * 0.5f;
		UILabel uILabel = CacheGameObject.Find<UILabel>("LabelInfoBottomRight");
		CacheGameObject.Find<Transform>("Flare").localPosition = new Vector3((int)num + 14, (int)num2, 0f);
		uILabel.transform.localPosition = new Vector3((int)num4, (int)num2, 0f);
		uILabel.text = "[" + FengGameManagerMKII.alternativefinalhex() + "]" + (((int)FengGameManagerMKII.settings[294] == 0) ? "(I)polation, " : (((int)FengGameManagerMKII.settings[294] == 1) ? "(E)polation, " : "")) + "Menu(" + FengCustomInputs.Inputs.inputString[InputCode.pause] + "), " + cameraMode.ToString() + "(" + FengCustomInputs.Inputs.inputString[InputCode.camera] + ")";
		CacheGameObject.Find<Transform>("LabelInfoTopCenter").localPosition = new Vector3(0f, (int)num3, 0f);
		CacheGameObject.Find<Transform>("LabelInfoTopRight").localPosition = new Vector3((int)num4, (int)num3, 0f);
		CacheGameObject.Find<Transform>("LabelNetworkStatus").localPosition = new Vector3((int)num, (int)num3, 0f);
		CacheGameObject.Find<Transform>("LabelInfoTopLeft").localPosition = new Vector3((int)num, (int)(num3 - 20f), 0f);
		if (InRoomChat.ChatInstanse != null)
		{
			InRoomChat.ChatInstanse.transform.localPosition = new Vector3((int)num2, (int)num2, 0f);
			InRoomChat.ChatInstanse.setPosition();
		}
		if (!usingTitan || gametype == GAMETYPE.SINGLE)
		{
			CacheGameObject.Find<Transform>("skill_cd_bottom").localPosition = new Vector3(0f, (int)(num2 + 5f), 0f);
			CacheGameObject.Find<Transform>("GasUI").localPosition = CacheGameObject.Find("skill_cd_bottom").transform.localPosition;
			CacheGameObject.Find<Transform>("stamina_titan").localPosition = new Vector3(0f, 9999f, 0f);
			CacheGameObject.Find<Transform>("stamina_titan_bottom").localPosition = new Vector3(0f, 9999f, 0f);
		}
		else
		{
			Vector3 localPosition = new Vector3(0f, 9999f, 0f);
			CacheGameObject.Find<Transform>("skill_cd_bottom").localPosition = localPosition;
			CacheGameObject.Find<Transform>("skill_cd_armin").localPosition = localPosition;
			CacheGameObject.Find<Transform>("skill_cd_eren").localPosition = localPosition;
			CacheGameObject.Find<Transform>("skill_cd_jean").localPosition = localPosition;
			CacheGameObject.Find<Transform>("skill_cd_levi").localPosition = localPosition;
			CacheGameObject.Find<Transform>("skill_cd_marco").localPosition = localPosition;
			CacheGameObject.Find<Transform>("skill_cd_mikasa").localPosition = localPosition;
			CacheGameObject.Find<Transform>("skill_cd_petra").localPosition = localPosition;
			CacheGameObject.Find<Transform>("skill_cd_sasha").localPosition = localPosition;
			CacheGameObject.Find<Transform>("GasUI").localPosition = localPosition;
			CacheGameObject.Find<Transform>("stamina_titan").localPosition = new Vector3(-160f, (int)(num2 + 15f), 0f);
			CacheGameObject.Find<Transform>("stamina_titan_bottom").localPosition = new Vector3(-160f, (int)(num2 + 15f), 0f);
		}
		if (mainHERO != null && mainHERO.isLocal)
		{
			mainHERO.setSkillHUDPosition2();
		}
		if (stereoType == STEREO_3D_TYPE.SIDE_BY_SIDE)
		{
			base.gameObject.GetComponent<Camera>().aspect = Screen.width / Screen.height;
		}
		createSnapShotRT2();
	}

	public GameObject setMainObject(GameObject obj, bool resetRotation = true, bool lockAngle = false)
	{
		mainobject = obj;
		mainTITAN = null;
		if (obj == null)
		{
			mainHERO = null;
			main_objectT = null;
			main_objectR = null;
			head = null;
			headT = null;
			distanceMulti = (heightMulti = 1f);
		}
		else if (mainobject != null && (main_objectT = mainobject.transform) != null && (head = main_objectT.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head")) != null)
		{
			mainHERO = mainobject.GetComponent<HERO>();
			main_objectR = mainobject.rigidbody;
			headT = head.transform;
			distanceMulti = ((head != null) ? (Vector3.Distance(headT.position, main_objectT.position) * 0.2f) : 1f);
			heightMulti = ((head != null) ? (Vector3.Distance(headT.position, main_objectT.position) * 0.33f) : 1f);
			if (resetRotation)
			{
				mainT.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		else if (mainobject != null && (main_objectT = mainobject.transform) != null && (head = main_objectT.Find("Amarture/Controller_Body/hip/spine/chest/neck/head")) != null)
		{
			mainHERO = mainobject.GetComponent<HERO>();
			main_objectR = mainobject.rigidbody;
			headT = head.transform;
			distanceMulti = (heightMulti = 0.64f);
			if (resetRotation)
			{
				mainT.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		else
		{
			mainHERO = null;
			main_objectT = null;
			main_objectR = null;
			head = null;
			headT = null;
			distanceMulti = (heightMulti = 1f);
			if (resetRotation)
			{
				mainT.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		this.lockAngle = lockAngle;
		return obj;
	}

	public GameObject setMainObjectASTITAN(GameObject obj)
	{
		mainobject = obj;
		if ((main_objectT = mainobject.transform) != null && (head = main_objectT.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head")) != null)
		{
			mainTITAN = mainobject.GetComponent<TITAN>();
			main_objectR = mainobject.rigidbody;
			headT = head.transform;
			distanceMulti = ((head != null) ? (Vector3.Distance(headT.position, main_objectT.position) * 0.4f) : 1f);
			heightMulti = ((head != null) ? (Vector3.Distance(headT.position, main_objectT.position) * 0.45f) : 1f);
			mainT.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		return obj;
	}

	public void setSpectorMode(bool val)
	{
		spectatorMode = val;
		spectate.disable = !val;
		mouselook.disable = !val;
	}

	private void shakeUpdate()
	{
		if (duration > 0f)
		{
			duration -= Time.deltaTime;
			if (flip)
			{
				mainG.transform.position += Vector3.up * R;
			}
			else
			{
				mainG.transform.position -= Vector3.up * R;
			}
			flip = !flip;
			R *= decay;
		}
	}

	public void snapShot2(int index)
	{
		snapT.position = MainObjectPosition;
		snapT.position += Vector3.up * heightMulti;
		snapT.position -= Vector3.up * 1.1f;
		Vector3 position;
		Vector3 vector = (position = snapT.position);
		Vector3 vector2 = (vector + snapShotTargetPosition) * 0.5f;
		snapT.position = vector2;
		vector = vector2;
		snapT.LookAt(snapShotTargetPosition);
		if (index == 3)
		{
			snapT.RotateAround(mainT.position, Vector3.up, UnityEngine.Random.Range(-180f, 180f));
		}
		else
		{
			snapT.RotateAround(mainT.position, Vector3.up, UnityEngine.Random.Range(-20f, 20f));
		}
		snapT.LookAt(vector);
		snapT.RotateAround(vector, mainT.right, UnityEngine.Random.Range(-20f, 20f));
		float num = Vector3.Distance(snapShotTargetPosition, position);
		if (snapShotTarget != null && snapShotTarget.GetComponent<TITAN>() != null)
		{
			num += (float)(index - 1) * snapShotTarget.transform.localScale.x * 10f;
		}
		snapT.position -= snapT.forward * UnityEngine.Random.Range(num + 3f, num + 10f);
		snapT.LookAt(vector);
		snapT.RotateAround(vector, mainT.forward, UnityEngine.Random.Range(-30f, 30f));
		Vector3 mainObjectPosition = MainObjectPosition;
		Vector3 vector3 = MainObjectPosition - snapT.position;
		mainObjectPosition -= vector3;
		RaycastHit hitInfo;
		if (head != null)
		{
			if (Physics.Linecast(headT.position, mainObjectPosition, out hitInfo, Layer.GroundEnemy))
			{
				snapT.position = hitInfo.point;
			}
			else if (Physics.Linecast(headT.position - vector3 * distanceMulti * 3f, mainObjectPosition, out hitInfo, Layer.GroundEnemy))
			{
				snapT.position = hitInfo.point;
			}
		}
		else if (Physics.Linecast(main_objectT.position + Vector3.up, mainObjectPosition, out hitInfo, Layer.GroundEnemy))
		{
			snapT.position = hitInfo.point;
		}
		switch (index)
		{
		case 1:
			snapshot1 = RTImage2(snapShotCamera.GetComponent<Camera>());
			SnapShotSaves.addIMG(snapshot1, snapShotDmg);
			break;
		case 2:
			snapshot2 = RTImage2(snapShotCamera.GetComponent<Camera>());
			SnapShotSaves.addIMG(snapshot2, snapShotDmg);
			break;
		case 3:
			snapshot3 = RTImage2(snapShotCamera.GetComponent<Camera>());
			SnapShotSaves.addIMG(snapshot3, snapShotDmg);
			break;
		}
		snapShotCount = index;
		hasSnapShot = true;
		snapShotCountDown = 2f;
		if (index == 1)
		{
			CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = snapshot1;
			CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.localScale = new Vector3((float)Screen.width * 0.4f, (float)Screen.height * 0.4f, 1f);
			CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.localPosition = new Vector3((float)(-Screen.width) * 0.225f, (float)Screen.height * 0.225f, 0f);
			CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.rotation = Quaternion.Euler(0f, 0f, 10f);
			if (PlayerPrefs.HasKey("showSSInGame") && PlayerPrefs.GetInt("showSSInGame") == 1)
			{
				CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = true;
			}
			else
			{
				CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = false;
			}
		}
	}

	public void snapShotUpdate()
	{
		if (startSnapShotFrameCount)
		{
			snapShotStartCountDownTime -= Time.deltaTime;
			if (snapShotStartCountDownTime <= 0f)
			{
				snapShot2(1);
				startSnapShotFrameCount = false;
			}
		}
		if (!hasSnapShot)
		{
			return;
		}
		snapShotCountDown -= Time.deltaTime;
		if (snapShotCountDown <= 0f)
		{
			CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = false;
			hasSnapShot = false;
			snapShotCountDown = 0f;
		}
		else if (snapShotCountDown < 1f)
		{
			CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = snapshot3;
		}
		else if (snapShotCountDown < 1.5f)
		{
			CacheGameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = snapshot2;
		}
		if (snapShotCount < 3)
		{
			snapShotInterval -= Time.deltaTime;
			if (snapShotInterval <= 0f)
			{
				snapShotInterval = 0.05f;
				snapShotCount++;
				snapShot2(snapShotCount);
			}
		}
	}

	private void Start()
	{
		mainCamera = this;
		isReady = true;
		isPausing = false;
		locker = CacheGameObject.Find<Transform>("locker");
		setDayLight(dayLight);
		ChangeLight();
		snapT = snapShotCamera.transform;
		sensitivityMulti = PlayerPrefs.GetFloat("MouseSensitivity");
		if (PlayerPrefs.HasKey("cameraTilt"))
		{
			cameraTilt = PlayerPrefs.GetInt("cameraTilt");
		}
		else
		{
			cameraTilt = 1;
		}
		if (PlayerPrefs.HasKey("cameraDistance"))
		{
			cameraDistance = PlayerPrefs.GetFloat("cameraDistance") + 0.3f;
		}
		createSnapShotRT2();
	}

	public void startShake(float R, float duration, float decay = 0.95f)
	{
		if (this.duration < duration)
		{
			this.R = R;
			this.duration = duration;
			this.decay = decay;
		}
	}

	public void startSnapShot2(Vector3 p, int dmg, GameObject target, float startTime)
	{
		if (int.TryParse((string)FengGameManagerMKII.settings[95], out var result))
		{
			if (dmg >= result)
			{
				snapShotCount = 1;
				startSnapShotFrameCount = true;
				snapShotTargetPosition = p;
				snapShotTarget = target;
				snapShotStartCountDownTime = startTime;
				snapShotInterval = 0.05f + UnityEngine.Random.Range(0f, 0.03f);
				snapShotDmg = dmg;
			}
		}
		else
		{
			snapShotCount = 1;
			startSnapShotFrameCount = true;
			snapShotTargetPosition = p;
			snapShotTarget = target;
			snapShotStartCountDownTime = startTime;
			snapShotInterval = 0.05f + UnityEngine.Random.Range(0f, 0.03f);
			snapShotDmg = dmg;
		}
	}

	public void update()
	{
		if (flashDuration > 0f)
		{
			flashDuration -= Time.deltaTime;
			if (flashDuration <= 0f)
			{
				flashDuration = 0f;
			}
			CacheGameObject.Find<UISprite>("flash").alpha = flashDuration * 0.5f;
		}
		if (gametype == GAMETYPE.STOP)
		{
			Screen.showCursor = true;
			Screen.lockCursor = false;
			return;
		}
		//InRoomChat.Write(gameOver.ToString());
		if (gametype != 0 && gameOver)
		{
			if (FengCustomInputs.Inputs.isInputDown[InputCode.attack1])
			{
				if (spectatorMode)
				{
					setSpectorMode(val: false);
				}
				else
				{
					setSpectorMode(val: true);
				}
			}
			if (FengCustomInputs.Inputs.isInputDown[InputCode.flare1])
			{
                List<GameObject> list = new List<GameObject>(FengGameManagerMKII.allheroes.Where((GameObject h) => h != null));
				currentPeekPlayerIndex++;
                if (currentPeekPlayerIndex >= list.Count)
				{
					currentPeekPlayerIndex = 0;
				}
				if (list.Count > 0)
				{
					setMainObject(list[currentPeekPlayerIndex]);
					setSpectorMode(val: false);
					lockAngle = false;
				}
            }
			if (FengCustomInputs.Inputs.isInputDown[InputCode.flare2])
			{
				List<GameObject> list = new List<GameObject>(FengGameManagerMKII.allheroes.Where((GameObject h) => h != null));
				currentPeekPlayerIndex--;
				if (currentPeekPlayerIndex >= list.Count)
				{
					currentPeekPlayerIndex = 0;
				}
				if (currentPeekPlayerIndex < 0)
				{
					currentPeekPlayerIndex = list.Count - 1;
				}
				if (list.Count > 0)
				{
					setMainObject(list[currentPeekPlayerIndex]);
					setSpectorMode(val: false);
					lockAngle = false;
				}
			}
			if (spectatorMode)
			{
				return;
			}
		}
		if (FengGameManagerMKII.instance.needChooseSide && FengCustomInputs.Inputs.isInputDown[InputCode.pause])
		{
            FengCustomInputs.Inputs.justUPDATEME();
			if (!FengCustomInputs.Inputs.menuOn)
			{
				isPausing = true;
				if (gametype == GAMETYPE.SINGLE)
				{
					Time.timeScale = 0f;
				}
				FengCustomInputs.Inputs.menuOn = (Screen.showCursor = true);
				Screen.lockCursor = false;
            }
			else
			{
				isPausing = false;
				FengGameManagerMKII.instance.saveSettings();
				FengGameManagerMKII.instance.quitSetting();
			}
		}
		if (needSetHUD)
		{
			needSetHUD = false;
			setHUDposition();
			Screen.lockCursor = !Screen.lockCursor;
			Screen.lockCursor = !Screen.lockCursor;
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.fullscreen])
		{
			Screen.fullScreen = !Screen.fullScreen;
			if (Screen.fullScreen)
			{
				Screen.SetResolution(UIMainReferences.Width, UIMainReferences.Height, fullscreen: false);
			}
			else
			{
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, false);
                Screen.fullScreen = false;
                BorderlessHelper.MakeBorderlessFullscreen();
            }
            needSetHUD = true;
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.restart])
		{
			reset();
		}
		if (!(main_object != null))
		{
			return;
		}
		if (main_objectT == null)
		{
			main_objectT = main_object.transform;
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.camera])
		{
			switch (cameraMode)
			{
			case CAMERA_TYPE.ORIGINAL:
				cameraMode = CAMERA_TYPE.NewTPS;
				Screen.lockCursor = true;
				break;
			case CAMERA_TYPE.TPS:
				cameraMode = CAMERA_TYPE.ORIGINAL;
				Screen.lockCursor = false;
				break;
			case CAMERA_TYPE.NewTPS:
				cameraMode = CAMERA_TYPE.TPS;
				Screen.lockCursor = false;
				break;
			}
			if ((int)FengGameManagerMKII.settings[245] == 1 || main_object.GetComponent<HERO>() == null)
			{
				Screen.showCursor = false;
			}
			needSetHUD = true;
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.hideCursor])
		{
			Screen.showCursor = !Screen.showCursor;
		}
		if (FengCustomInputs.Inputs.isInputDown[InputCode.focus])
		{
			triggerAutoLock = !triggerAutoLock;
			if (triggerAutoLock)
			{
				lockTarget = findNearestTitan();
				if (closestDistance >= 150f)
				{
					lockTarget = null;
					lockTargetT = null;
					triggerAutoLock = false;
				}
				else
				{
					lockTargetT = lockTarget.transform;
				}
			}
		}
		if (gameOver && main_object != null)
		{
			if (FengGameManagerMKII.inputRC.isInputHumanDown(InputCodeRC.liveCam))
			{
				if ((int)FengGameManagerMKII.settings[263] == 0)
				{
					FengGameManagerMKII.settings[263] = 1;
				}
				else
				{
					FengGameManagerMKII.settings[263] = 0;
				}
			}
			HERO component = main_object.GetComponent<HERO>();
			if (component != null && (int)FengGameManagerMKII.settings[263] == 1 && component.sync.enabled && component.isPhotonCamera)
			{
				CameraMovementLive(component);
			}
			else if (lockAngle)
			{
				mainT.rotation = Quaternion.Lerp(mainT.rotation, main_objectT.rotation, 0.2f);
				mainT.position = Vector3.Lerp(mainT.position, main_objectT.position - main_objectT.forward * 5f, 0.2f);
			}
			else
			{
				camareMovement();
			}
		}
		else
		{
			camareMovement();
		}
		if (triggerAutoLock && lockTarget != null)
		{
			float z = mainT.eulerAngles.z;
			Transform transform = lockTargetT.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
			Vector3 vector = transform.position - MainObjectPosition;
			vector.Normalize();
			mainT.position = Vector3.Lerp(mainT.position, lockCameraPosition = MainObjectPosition - vector * 10f * distanceMulti * distanceOffsetMulti + Vector3.up * 3f * heightMulti * distanceOffsetMulti, Time.deltaTime * 4f);
			if (head != null)
			{
				mainT.LookAt(headT.position * 0.8f + transform.position * 0.2f);
			}
			else
			{
				mainT.LookAt(main_objectT.position * 0.8f + transform.position * 0.2f);
			}
			mainT.localEulerAngles = new Vector3(mainT.eulerAngles.x, mainT.eulerAngles.y, z);
			Vector2 vector2 = mainC.WorldToScreenPoint(transform.position - transform.forward * lockTargetT.localScale.x);
			locker.localPosition = new Vector3(vector2.x - (float)Screen.width * 0.5f, vector2.y - (float)Screen.height * 0.5f, 0f);
			if (lockTarget.GetComponent<TITAN>() != null && lockTarget.GetComponent<TITAN>().hasDie)
			{
				lockTarget = null;
				lockTargetT = null;
			}
		}
		else
		{
			locker.localPosition = new Vector3(0f, (float)(-Screen.height) * 0.5f - 50f, 0f);
		}
		Vector3 mainObjectPosition = MainObjectPosition;
		Vector3 normalized = (MainObjectPosition - mainT.position).normalized;
		mainObjectPosition -= distance * normalized * distanceMulti;
		RaycastHit hitInfo;
		if (head != null)
		{
			if (headT == null)
			{
				headT = head.transform;
			}
			if (Physics.Linecast(headT.position, mainObjectPosition, out hitInfo, Layer.Ground))
			{
				mainT.position = hitInfo.point;
			}
			else if (Physics.Linecast(headT.position - normalized * distanceMulti * 3f, mainObjectPosition, out hitInfo, Layer.Enemy))
			{
				mainT.position = hitInfo.point;
			}
		}
		else if (Physics.Linecast(main_objectT.position + Vector3.up, mainObjectPosition, out hitInfo, Layer.GroundEnemy))
		{
			mainT.position = hitInfo.point;
		}
		shakeUpdate();
	}
}
