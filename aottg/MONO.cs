using Photon;
using UnityEngine;

public abstract class MONO : Photon.MonoBehaviour
{
	public readonly SPECIES species;

	public Animation baseA;

	public Transform baseT;

	public Rigidbody baseR;

	public GameObject baseG;

	public PhotonView basePV;

	public Transform Amarture;

	public Transform Controller_Body;

	public Transform Core;

	public Transform head;

	public Transform neck;

	public Transform thigh_R;

	public Transform thigh_L;

	public Transform foot_R;

	public Transform foot_L;

	public Transform chest;

	public Transform shoulder_R;

	public Transform shoulder_L;

	public Transform hip;

	public Transform spine;

	public Transform upper_arm_L;

	public Transform upper_arm_R;

	public Transform forearm_L;

	public Transform forearm_R;

	public Transform hand_L;

	public Transform hand_R;

	public Transform hand_R_001;

	public Transform hand_L_001;

	public Transform AABB;

	public SphereCollider hand_R_001Sphere;

	public SphereCollider hand_L_001Sphere;

	public Transform hand_R_001SphereT;

	public Transform hand_L_001SphereT;

	public Transform snd_titan_foot;

	public Transform snd_eren_foot;

	public AudioSource snd_titan_footAudio;

	public AudioSource snd_eren_footAudio;

	public Transform ap_front_ground;

	public Transform chkAeLeft;

	public Transform chkAeLLeft;

	public Transform chkAeRight;

	public Transform chkAeLRight;

	protected MONO(SPECIES species)
	{
		this.species = species;
	}

	protected void CacheComponnents()
	{
		baseA = GetComponent<Animation>();
		baseT = GetComponent<Transform>();
		baseR = GetComponent<Rigidbody>();
		basePV = GetComponent<PhotonView>();
		baseG = base.gameObject;
	}

	public void CacheTransforms()
	{
		if (baseT == null && (baseT = GetComponent<Transform>()) == null)
		{
			return;
		}
		Amarture = baseT.Find("Amarture");
		string text;
		if (species == SPECIES.Hero || species == SPECIES.Horse)
		{
			AABB = null;
			Core = null;
			text = "Amarture/Controller_Body/";
			Controller_Body = baseT.Find("Amarture/Controller_Body");
		}
		else
		{
			text = "Amarture/Core/Controller_Body/";
			AABB = baseT.Find("AABB");
			Core = baseT.Find("Amarture/Core");
			if (species == SPECIES.TitanEren)
			{
				if ((bool)(snd_eren_foot = baseT.Find("snd_eren_foot")))
				{
					snd_eren_footAudio = snd_eren_foot.GetComponent<AudioSource>();
				}
			}
			else if ((bool)(snd_titan_foot = baseT.Find("snd_titan_foot")))
			{
				snd_titan_footAudio = snd_titan_foot.GetComponent<AudioSource>();
			}
			Controller_Body = baseT.Find("Amarture/Core/Controller_Body");
		}
		head = baseT.Find(text + "hip/spine/chest/neck/head");
		neck = baseT.Find(text + "hip/spine/chest/neck");
		thigh_L = baseT.Find(text + "hip/thigh_L");
		thigh_R = baseT.Find(text + "hip/thigh_R");
		foot_L = baseT.Find(text + "hip/thigh_L/shin_L/foot_L");
		foot_R = baseT.Find(text + "hip/thigh_R/shin_R/foot_R");
		chest = baseT.Find(text + "hip/spine/chest");
		shoulder_L = baseT.Find(text + "hip/spine/chest/shoulder_L");
		shoulder_R = baseT.Find(text + "hip/spine/chest/shoulder_R");
		hip = baseT.Find(text + "hip");
		spine = baseT.Find(text + "hip/spine");
		upper_arm_L = baseT.Find(text + "hip/spine/chest/shoulder_L/upper_arm_L");
		upper_arm_R = baseT.Find(text + "hip/spine/chest/shoulder_R/upper_arm_R");
		forearm_L = baseT.Find(text + "hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
		forearm_R = baseT.Find(text + "hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
		hand_L = baseT.Find(text + "hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
		hand_R = baseT.Find(text + "hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
		if ((bool)(hand_L_001 = baseT.Find(text + "hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001")) && (bool)(hand_L_001Sphere = hand_L_001.GetComponent<SphereCollider>()))
		{
			hand_L_001SphereT = hand_L_001Sphere.transform;
		}
		if ((bool)(hand_R_001 = baseT.Find(text + "hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001")) && (bool)(hand_R_001Sphere = hand_R_001.GetComponent<SphereCollider>()))
		{
			hand_R_001SphereT = hand_R_001Sphere.transform;
		}
		if (species == SPECIES.Titan)
		{
			chkAeLeft = baseT.Find("chkAeLeft");
			chkAeLLeft = baseT.Find("chkAeLLeft");
			chkAeRight = baseT.Find("chkAeRight");
			chkAeLRight = baseT.Find("chkAeLRight");
			ap_front_ground = baseT.Find("ap_front_ground");
		}
	}

	public void RecheckTransforms()
	{
		if (baseT == null && (baseT = GetComponent<Transform>()) == null)
		{
			return;
		}
		string text;
		if (species == SPECIES.Hero || species == SPECIES.Horse)
		{
			text = "Amarture/Controller_Body/";
			Core = null;
			if (Controller_Body == null)
			{
				Controller_Body = baseT.Find("Amarture/Controller_Body");
			}
		}
		else
		{
			text = "Amarture/Core/Controller_Body/";
			if (AABB == null)
			{
				AABB = baseT.Find("AABB");
			}
			if (Core == null)
			{
				Core = baseT.Find("Amarture/Core");
			}
			if (snd_titan_foot == null)
			{
				snd_titan_foot = baseT.Find("snd_titan_foot");
			}
			if (Controller_Body == null)
			{
				Controller_Body = baseT.Find("Amarture/Core/Controller_Body");
			}
		}
		if (Amarture == null)
		{
			Amarture = baseT.Find("Amarture");
		}
		if (head == null)
		{
			head = baseT.Find(text + "hip/spine/chest/neck/head");
		}
		if (neck == null)
		{
			neck = baseT.Find(text + "hip/spine/chest/neck");
		}
		if (thigh_L == null)
		{
			thigh_L = baseT.Find(text + "hip/thigh_L");
		}
		if (thigh_R == null)
		{
			thigh_R = baseT.Find(text + "hip/thigh_R");
		}
		if (foot_L == null)
		{
			foot_L = baseT.Find(text + "hip/thigh_L/shin_L/foot_L");
		}
		if (foot_R == null)
		{
			foot_R = baseT.Find(text + "hip/thigh_R/shin_R/foot_R");
		}
		if (chest == null)
		{
			chest = baseT.Find(text + "hip/spine/chest");
		}
		if (shoulder_L == null)
		{
			shoulder_L = baseT.Find(text + "hip/spine/chest/shoulder_L");
		}
		if (shoulder_R == null)
		{
			shoulder_R = baseT.Find(text + "hip/spine/chest/shoulder_R");
		}
		if (hip == null)
		{
			hip = baseT.Find(text + "hip");
		}
		if (spine == null)
		{
			spine = baseT.Find(text + "hip/spine");
		}
		if (upper_arm_L == null)
		{
			upper_arm_L = baseT.Find(text + "hip/spine/chest/shoulder_L/upper_arm_L");
		}
		if (upper_arm_R == null)
		{
			upper_arm_R = baseT.Find(text + "hip/spine/chest/shoulder_R/upper_arm_R");
		}
		if (forearm_L == null)
		{
			forearm_L = baseT.Find(text + "hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
		}
		if (forearm_R == null)
		{
			forearm_R = baseT.Find(text + "hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
		}
		if (hand_L == null)
		{
			hand_L = baseT.Find(text + "hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
		}
		if (hand_R == null)
		{
			hand_R = baseT.Find(text + "hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
		}
		if (hand_L_001 == null && (bool)(hand_L_001 = baseT.Find(text + "hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001")) && hand_L_001Sphere == null && (bool)(hand_L_001Sphere = hand_L_001.GetComponent<SphereCollider>()) && hand_L_001SphereT == null)
		{
			hand_L_001SphereT = hand_L_001Sphere.transform;
		}
		if (hand_R_001 == null && (bool)(hand_R_001 = baseT.Find(text + "hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001")) && hand_R_001Sphere == null && (bool)(hand_R_001Sphere = hand_R_001.GetComponent<SphereCollider>()) && hand_R_001SphereT == null)
		{
			hand_R_001SphereT = hand_R_001Sphere.transform;
		}
		if (ap_front_ground == null)
		{
			ap_front_ground = (ap_front_ground = baseT.Find("ap_front_ground"));
		}
	}
}
