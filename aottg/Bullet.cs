using System.Collections;
using Anarchy.Custom.Interfaces;
using Anarchy.Custom.Level;
using CustomAudio;
using Photon;
using UnityEngine;

public class Bullet : Photon.MonoBehaviour, IAnarchyScriptHook
{
	private Vector3 heightOffSet = Vector3.up * 0.48f;

	private bool isdestroying;

	private float killTime;

	private float killTime2;

	private Vector3 launchOffSet = Vector3.zero;

	private bool left = true;

	public bool leviMode;

	public float leviShootTime;

	private LineRenderer lineRenderer;

	private GameObject master;

	private GameObject myRef;

	public TITAN myTitan;

	private ArrayList nodes = new ArrayList();

	public int phase;

	private GameObject rope;

	private int spiralcount;

	private ArrayList spiralNodes;

	private Vector3 velocity = Vector3.zero;

	private Vector3 velocity2 = Vector3.zero;

	private Transform baseT;

	private Transform baseGT;

	private PhotonView basePV;

	private Transform masterT;

	private Rigidbody masterR;

	public HERO MasterHero;

	public string myRefName;

	private Transform myRefT;

	public Transform Transform => base.transform;

	public IAnarchyScriptHero Master
	{
		get
		{
			if (!(master == null))
			{
				return master.GetComponent<HERO>();
			}
			return null;
		}
	}

	void IAnarchyScriptHook.KillMaster()
	{
		if (!(master == null))
		{
			master.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, "Trap");
		}
	}

	private void Awake()
	{
		baseT = base.transform;
		basePV = base.photonView;
		baseGT = base.gameObject.transform;
	}

	public void checkTitan()
	{
		GameObject main_object = IN_GAME_MAIN_CAMERA.main_object;
		if (!(main_object != null) || !(master != null) || !(master == main_object) || !Physics.Raycast(layerMask: ((LayerMask)(1 << LayerMask.NameToLayer("PlayerAttackBox"))).value, origin: baseT.position, direction: velocity, hitInfo: out var hitInfo, distance: 10f))
		{
			return;
		}
		Collider collider = hitInfo.collider;
		if (!collider.name.Contains("PlayerDetectorRC"))
		{
			return;
		}
		TITAN component = collider.transform.root.gameObject.GetComponent<TITAN>();
		if (component != null)
		{
			if (myTitan == null)
			{
				myTitan = component;
				myTitan.isHooked = true;
			}
			else if (myTitan != component)
			{
				myTitan.isHooked = false;
				myTitan = component;
				myTitan.isHooked = true;
			}
		}
	}

	public void disable()
	{
		phase = 2;
		killTime = 0f;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			object[] parameters = new object[1] { 1 };
			basePV.RPC("setPhase", PhotonTargets.Others, parameters);
		}
	}

	private void FixedUpdate()
	{
		if ((phase == 2 || phase == 1) && leviMode)
		{
			spiralcount++;
			if (spiralcount >= 60)
			{
				isdestroying = true;
				removeMe();
				return;
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !basePV.isMine)
		{
			if (phase == 0)
			{
				baseGT.position += velocity * Time.deltaTime * 50f + velocity2 * Time.deltaTime;
				nodes.Add(new Vector3(baseGT.position.x, baseGT.position.y, baseGT.position.z));
			}
		}
		else
		{
			if (phase != 0)
			{
				return;
			}
			checkTitan();
			baseGT.position += velocity * Time.deltaTime * 50f + velocity2 * Time.deltaTime;
			LayerMask layerMask = 1 << LayerMask.NameToLayer("EnemyBox");
			LayerMask layerMask2 = 1 << LayerMask.NameToLayer("Ground");
			LayerMask layerMask3 = 1 << LayerMask.NameToLayer("NetworkObject");
			LayerMask layerMask4 = ((PhotonNetwork.offlineMode || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) ? ((LayerMask)((int)layerMask | (int)layerMask2 | (int)layerMask3)) : (HERO.FindMyHero(PhotonNetwork.player.ID).CarryingSomeone ? ((LayerMask)((int)layerMask | (int)layerMask2)) : ((LayerMask)((int)layerMask | (int)layerMask2 | (int)layerMask3))));
			bool flag = false;
			bool flag2 = false;
			if ((nodes.Count <= 1) ? Physics.Linecast((Vector3)nodes[nodes.Count - 1], baseGT.position, out var hitInfo, layerMask4.value) : Physics.Linecast((Vector3)nodes[nodes.Count - 2], baseGT.position, out hitInfo, layerMask4.value))
			{
				bool flag3 = true;
				Transform transform = hitInfo.collider.transform;
				if (transform.gameObject.layer == LayerMask.NameToLayer("EnemyBox"))
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						object[] parameters = new object[1] { transform.root.gameObject.GetPhotonView().viewID };
						basePV.RPC("tieMeToOBJ", PhotonTargets.Others, parameters);
					}
					MasterHero.lastHook = transform.root;
					baseT.parent = transform;
					if (GameSettings.HookTitan > 0)
					{
						//CustomAudioManager.Play(CustomAudioManager.AudioFile.HookTitan);
					}
				}
				else if (transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
				{
					MasterHero.lastHook = null;
					CustomAnarchyLevel.Instance.OnHookAttachedToGround(this, hitInfo.collider.gameObject);
					if (GameSettings.HookObject > 0)
					{
						//CustomAudioManager.Play(CustomAudioManager.AudioFile.HookObject);
					}
				}
				else if (transform.gameObject.layer == LayerMask.NameToLayer("NetworkObject") && transform.gameObject.tag == "Player" && !leviMode)
				{
					if (GameSettings.HookTitan > 0)
					{
						//CustomAudioManager.Play(CustomAudioManager.AudioFile.HookTitan);
					}
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						object[] parameters2 = new object[1] { transform.root.gameObject.GetPhotonView().viewID };
						basePV.RPC("tieMeToOBJ", PhotonTargets.Others, parameters2);
					}
					MasterHero.hookToHuman(transform.root.gameObject, baseT.position);
					baseT.parent = transform;
					MasterHero.lastHook = null;
				}
				else
				{
					flag3 = false;
				}
				if (phase == 2)
				{
					flag3 = false;
				}
				if (flag3)
				{
					MasterHero.launch(hitInfo.point, left, leviMode);
					baseT.position = hitInfo.point;
					if (phase != 2)
					{
						if ((int)FengGameManagerMKII.settings[280] == 0)
						{
							MasterHero.AlreadyReeled = false;
						}
						phase = 1;
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
						{
							object[] parameters3 = new object[1] { 1 };
							basePV.RPC("setPhase", PhotonTargets.Others, parameters3);
							object[] parameters4 = new object[1] { baseT.position };
							basePV.RPC("tieMeTo", PhotonTargets.Others, parameters4);
						}
						if (leviMode)
						{
							getSpiral(masterT.position, masterT.rotation.eulerAngles);
						}
						flag = true;
					}
				}
			}
			nodes.Add(new Vector3(baseGT.position.x, baseGT.position.y, baseGT.position.z));
			if (flag)
			{
				return;
			}
			killTime2 += Time.deltaTime;
			if (killTime2 > 0.8f)
			{
				phase = 4;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					object[] parameters5 = new object[1] { 4 };
					basePV.RPC("setPhase", PhotonTargets.Others, parameters5);
				}
			}
		}
	}

	private void getSpiral(Vector3 masterposition, Vector3 masterrotation)
	{
		float num = 1.2f;
		float num2 = 30f;
		float num3 = 0.5f;
		num = 30f;
		float num4 = 0.05f + (float)spiralcount * 0.03f;
		num = ((spiralcount >= 5) ? (1.2f + (float)(60 - spiralcount) * 0.1f) : Vector2.Distance(new Vector2(masterposition.x, masterposition.z), new Vector2(baseGT.position.x, baseGT.position.z)));
		num3 -= (float)spiralcount * 0.06f;
		float num5 = num / num2;
		float num6 = num4 / num2 * 2f * 3.141593f;
		num3 *= 6.283185f;
		spiralNodes = new ArrayList();
		for (int i = 1; (float)i <= num2; i++)
		{
			float num7 = (float)i * num5 * (1f + 0.05f * (float)i);
			float f = (float)i * num6 + num3 + 1.256637f + masterrotation.y * 0.0173f;
			float x = Mathf.Cos(f) * num7;
			float z = (0f - Mathf.Sin(f)) * num7;
			spiralNodes.Add(new Vector3(x, 0f, z));
		}
	}

	public bool isHooked()
	{
		return phase == 1;
	}

	[RPC]
	private void killObject()
	{
	}

	public void launch(Vector3 v, Vector3 v2, string launcher_ref, bool isLeft, GameObject hero, bool leviMode = false)
	{
		if (phase != 2)
		{
			master = hero;
			masterT = master.transform;
			masterR = master.rigidbody;
			MasterHero = hero.GetComponent<HERO>();
			velocity = v;
			if (Mathf.Abs(Mathf.Acos(Vector3.Dot(v.normalized, v2.normalized)) * 57.29578f) > 90f)
			{
				velocity2 = Vector3.zero;
			}
			else
			{
				velocity2 = Vector3.Project(v2, v);
			}
			if (launcher_ref == "hookRefR1")
			{
				myRef = MasterHero.hookRefR1;
				myRefT = MasterHero.hookRefR1.transform;
				MasterHero.hook["right"] = this;
			}
			if (launcher_ref == "hookRefL1")
			{
				myRef = MasterHero.hookRefL1;
				myRefT = MasterHero.hookRefL1.transform;
				MasterHero.hook["left"] = this;
			}
			if (launcher_ref == "hookRefR2")
			{
				myRef = MasterHero.hookRefR2;
				myRefT = MasterHero.hookRefR2.transform;
				MasterHero.hook["right"] = this;
			}
			if (launcher_ref == "hookRefL2")
			{
				myRef = MasterHero.hookRefL2;
				myRefT = MasterHero.hookRefL2.transform;
				MasterHero.hook["left"] = this;
			}
			nodes = new ArrayList { myRefT.position };
			phase = 0;
			this.leviMode = leviMode;
			left = isLeft;
			if (IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
			{
				object[] parameters = new object[2]
				{
					MasterHero.photonView.viewID,
					launcher_ref
				};
				basePV.RPC("myMasterIs", PhotonTargets.Others, parameters);
				object[] parameters2 = new object[3] { v, velocity2, left };
				basePV.RPC("setVelocityAndLeft", PhotonTargets.Others, parameters2);
			}
			baseT.position = myRefT.position;
			baseT.rotation = Quaternion.LookRotation(v.normalized);
		}
	}

	[RPC]
	private void myMasterIs(int id, string launcherRef, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner || id < 0)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "messing w/ hooks");
			return;
		}
		PhotonView photonView = PhotonView.Find(id);
		if (photonView.owner != info.sender)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "messing w/ hooks2");
		}
		else
		{
			if (!(photonView != null))
			{
				return;
			}
			GameObject gameObject = photonView.gameObject;
			if (!(gameObject != null))
			{
				return;
			}
			HERO component = gameObject.GetComponent<HERO>();
			if (!(component != null))
			{
				return;
			}
			myRefName = launcherRef;
			master = gameObject;
			MasterHero = component;
			masterT = gameObject.transform;
			masterR = gameObject.rigidbody;
			string text = myRefName;
			switch (text)
			{
			case "hookRefL1":
				myRef = component.hookRefL1;
				myRefT = myRef.transform;
				component.hook["left"] = this;
				return;
			case "hookRefL2":
				myRef = component.hookRefL2;
				myRefT = myRef.transform;
				component.hook["left"] = this;
				return;
			case "hookRefR1":
				myRef = component.hookRefR1;
				myRefT = myRef.transform;
				component.hook["right"] = this;
				return;
			}
			if (text != "hookRefR2")
			{
				InRoomChat.Write("whatthefuck??");
				return;
			}
			myRef = component.hookRefR2;
			myRefT = myRef.transform;
			component.hook["right"] = this;
		}
	}

	[RPC]
	private void netLaunch(Vector3 newPosition)
	{
		nodes = new ArrayList();
		nodes.Add(newPosition);
	}

	[RPC]
	private void netUpdateLeviSpiral(Vector3 newPosition, Vector3 masterPosition, Vector3 masterrotation)
	{
		phase = 2;
		leviMode = true;
		getSpiral(masterPosition, masterrotation);
		Vector3 vector = masterPosition - (Vector3)spiralNodes[0];
		lineRenderer.SetVertexCount(spiralNodes.Count - (int)((float)spiralcount * 0.5f));
		for (int i = 0; (float)i <= (float)(spiralNodes.Count - 1) - (float)spiralcount * 0.5f; i++)
		{
			if (spiralcount < 5)
			{
				Vector3 vector2 = (Vector3)spiralNodes[i] + vector;
				float num = (float)(spiralNodes.Count - 1) - (float)spiralcount * 0.5f;
				vector2 = new Vector3(vector2.x, vector2.y * ((num - (float)i) / num) + newPosition.y * ((float)i / num), vector2.z);
				lineRenderer.SetPosition(i, vector2);
			}
			else
			{
				lineRenderer.SetPosition(i, (Vector3)spiralNodes[i] + vector);
			}
		}
	}

	[RPC]
	private void netUpdatePhase1(Vector3 newPosition, Vector3 masterPosition)
	{
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, newPosition);
		lineRenderer.SetPosition(1, masterPosition);
		baseT.position = newPosition;
	}

	private void OnDestroy()
	{
		if (FengGameManagerMKII.instance != null)
		{
			FengGameManagerMKII.instance.removeHook(this);
		}
		if (myTitan != null)
		{
			myTitan.isHooked = false;
		}
		if (master != null)
		{
			if (MasterHero.hook["left"] == this)
			{
				MasterHero.hook["left"] = null;
			}
			if (MasterHero.hook["right"] == this)
			{
				MasterHero.hook["right"] = null;
			}
		}
		Object.Destroy(rope);
	}

	public void removeMe()
	{
		CustomAnarchyLevel.Instance.OnHookUntiedGround(this);
		isdestroying = true;
		if (MasterHero != null)
		{
			if (MasterHero.hook["left"] == this)
			{
				MasterHero.hook["left"] = null;
			}
			else if (MasterHero.hook["right"] == this)
			{
				MasterHero.hook["right"] = null;
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
		{
			PhotonNetwork.Destroy(basePV);
			PhotonNetwork.RemoveRPCs(basePV);
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			Object.Destroy(rope);
			Object.Destroy(base.gameObject);
		}
	}

	private void setLinePhase0()
	{
		if (master == null)
		{
			if (IN_GAME_MAIN_CAMERA.gametype != 0 && basePV.isMine)
			{
				PhotonNetwork.Destroy(base.gameObject);
				PhotonNetwork.RemoveRPCs(basePV);
			}
			else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				Object.Destroy(rope);
				Object.Destroy(base.gameObject);
			}
		}
		else if (nodes.Count > 0)
		{
			Vector3 vector = myRefT.position - (Vector3)nodes[0];
			lineRenderer.SetVertexCount(nodes.Count);
			for (int i = 0; i <= nodes.Count - 1; i++)
			{
				lineRenderer.SetPosition(i, (Vector3)nodes[i] + vector * Mathf.Pow(0.75f, i));
			}
			if (nodes.Count > 1)
			{
				lineRenderer.SetPosition(1, myRefT.position);
			}
		}
	}

	[RPC]
	private void setPhase(int value, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "invalid Hook destroy");
		}
		else
		{
			phase = value;
		}
	}

	[RPC]
	private void setVelocityAndLeft(Vector3 value, Vector3 v2, bool l, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "invalid Hook destroy2");
			return;
		}
		velocity = value;
		velocity2 = v2;
		left = l;
		baseT.rotation = Quaternion.LookRotation(value.normalized);
	}

	private void Start()
	{
		rope = (GameObject)Object.Instantiate(Resources.Load("rope"));
		lineRenderer = rope.GetComponent<LineRenderer>();
		FengGameManagerMKII.instance.addHook(this);
		if ((int)FengGameManagerMKII.settings[292] == 1 && basePV.isMine)
		{
			rope.renderer.material.color = new Color(FengGameManagerMKII.HookColorFloats0, FengGameManagerMKII.HookColorFloats1, FengGameManagerMKII.HookColorFloats2, 1f);
		}
	}

	[RPC]
	private void tieMeTo(Vector3 p, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "invalid tieMeTo");
		}
		else
		{
			baseT.position = p;
		}
	}

	[RPC]
	private void tieMeToOBJ(int id, PhotonMessageInfo info)
	{
		if (info.sender != basePV.owner)
		{
			FengGameManagerMKII.ThrowJusticeDefensive(info.sender, "invalid tieMeToObj");
		}
		else
		{
			baseT.parent = PhotonView.Find(id).gameObject.transform;
		}
	}

	public void update()
	{
		if (master == null)
		{
			removeMe();
		}
		else
		{
			if (isdestroying)
			{
				return;
			}
			if (leviMode)
			{
				leviShootTime += Time.deltaTime;
				if (leviShootTime > 0.4f)
				{
					phase = 2;
					base.gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			if (phase == 0)
			{
				setLinePhase0();
			}
			else if (phase == 1)
			{
				Vector3 vector = baseT.position - myRefT.position;
				_ = baseT.position + myRefT.position;
				Vector3 vector2 = masterR.velocity;
				float magnitude = vector2.magnitude;
				float magnitude2 = vector.magnitude;
				int value = (int)((magnitude2 + magnitude) / 5f);
				value = Mathf.Clamp(value, 2, 6);
				lineRenderer.SetVertexCount(value);
				lineRenderer.SetPosition(0, myRefT.position);
				int i = 1;
				float num = Mathf.Pow(magnitude2, 0.3f);
				for (; i < value; i++)
				{
					int num2 = value / 2;
					float num3 = Mathf.Abs(i - num2);
					float f = ((float)num2 - num3) / (float)num2;
					f = Mathf.Pow(f, 0.5f);
					float num4 = (num + magnitude) * 0.0015f * f;
					lineRenderer.SetPosition(i, new Vector3(Random.Range(0f - num4, num4), Random.Range(0f - num4, num4), Random.Range(0f - num4, num4)) + myRefT.position + vector * ((float)i / (float)value) - Vector3.up * num * 0.05f * f - vector2 * 0.001f * f * num);
				}
				lineRenderer.SetPosition(value - 1, baseT.position);
			}
			else if (phase == 2)
			{
				if (leviMode)
				{
					getSpiral(masterT.position, masterT.rotation.eulerAngles);
					Vector3 vector3 = myRefT.position - (Vector3)spiralNodes[0];
					lineRenderer.SetVertexCount(spiralNodes.Count - (int)((float)spiralcount * 0.5f));
					for (int j = 0; (float)j <= (float)(spiralNodes.Count - 1) - (float)spiralcount * 0.5f; j++)
					{
						if (spiralcount < 5)
						{
							Vector3 vector4 = (Vector3)spiralNodes[j] + vector3;
							float num5 = (float)(spiralNodes.Count - 1) - (float)spiralcount * 0.5f;
							vector4 = new Vector3(vector4.x, vector4.y * ((num5 - (float)j) / num5) + baseGT.position.y * ((float)j / num5), vector4.z);
							lineRenderer.SetPosition(j, vector4);
						}
						else
						{
							lineRenderer.SetPosition(j, (Vector3)spiralNodes[j] + vector3);
						}
					}
				}
				else
				{
					lineRenderer.SetVertexCount(2);
					lineRenderer.SetPosition(0, baseT.position);
					lineRenderer.SetPosition(1, myRefT.position);
					killTime += Time.deltaTime * 0.2f;
					lineRenderer.SetWidth(0.1f - killTime, 0.1f - killTime);
					if (killTime > 0.1f)
					{
						removeMe();
					}
				}
			}
			else
			{
				if (phase != 4)
				{
					return;
				}
				baseGT.position += velocity + velocity2 * Time.deltaTime;
				nodes.Add(new Vector3(baseGT.position.x, baseGT.position.y, baseGT.position.z));
				Vector3 vector5 = myRefT.position - (Vector3)nodes[0];
				for (int k = 0; k <= nodes.Count - 1; k++)
				{
					lineRenderer.SetVertexCount(nodes.Count);
					lineRenderer.SetPosition(k, (Vector3)nodes[k] + vector5 * Mathf.Pow(0.5f, k));
				}
				killTime2 += Time.deltaTime;
				if (killTime2 > 0.8f)
				{
					killTime += Time.deltaTime * 0.2f;
					lineRenderer.SetWidth(0.1f - killTime, 0.1f - killTime);
					if (killTime > 0.1f)
					{
						removeMe();
					}
				}
			}
		}
	}
}
