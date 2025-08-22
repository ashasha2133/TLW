using System.Collections;
using BRM;
using Photon;
using UnityEngine;

public class TITAN_SETUP : Photon.MonoBehaviour
{
	public GameObject eye;

	private CostumeHair hair;

	private GameObject hair_go_ref;

	private int hairType;

	public bool haseye;

	private GameObject part_hair;

	public int skin;

	private void Awake()
	{
		CostumeHair.init();
		CharacterMaterials.init();
		HeroCostume.init2();
		hair_go_ref = new GameObject();
		eye.transform.parent = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
		hair_go_ref.transform.position = eye.transform.position + Vector3.up * 3.5f + base.transform.forward * 5.2f;
		hair_go_ref.transform.rotation = eye.transform.rotation;
		hair_go_ref.transform.RotateAround(eye.transform.position, base.transform.right, -20f);
		hair_go_ref.transform.localScale = new Vector3(210f, 210f, 210f);
		hair_go_ref.transform.parent = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
	}

	public IEnumerator loadskinE(int hair, int eye, string hairlink)
	{
		bool iteratorVariable0 = false;
		Object.Destroy(part_hair);
		this.hair = CostumeHair.hairsM[hair];
		hairType = hair;
		if (this.hair.hair != string.Empty)
		{
			GameObject iteratorVariable1 = (GameObject)Object.Instantiate(CacheResources.Load("Character/" + this.hair.hair));
			iteratorVariable1.transform.parent = hair_go_ref.transform.parent;
			iteratorVariable1.transform.position = hair_go_ref.transform.position;
			iteratorVariable1.transform.rotation = hair_go_ref.transform.rotation;
			iteratorVariable1.transform.localScale = hair_go_ref.transform.localScale;
			iteratorVariable1.renderer.material = CharacterMaterials.materials[this.hair.texture];
			bool mipmap = true;
			if ((int)FengGameManagerMKII.settings[63] == 1)
			{
				mipmap = false;
			}
			if (RCextensions.CheckIP(hairlink))
			{
				if (!hairlink.EndsWith(".jpg") && !hairlink.EndsWith(".png") && !hairlink.EndsWith(".jpeg"))
				{
					if (hairlink.ToLower() == "transparent")
					{
						iteratorVariable1.renderer.enabled = false;
					}
				}
				else if (FengGameManagerMKII.linkHash[0].ContainsKey(hairlink))
				{
					iteratorVariable1.renderer.material = (Material)FengGameManagerMKII.linkHash[0][hairlink];
				}
				else
				{
					WWW link = new WWW(hairlink);
					yield return link;
					Texture2D mainTexture = RCextensions.loadimage(link, mipmap, 200000);
					link.Dispose();
					if (!FengGameManagerMKII.linkHash[0].ContainsKey(hairlink))
					{
						iteratorVariable0 = true;
						iteratorVariable1.renderer.material.mainTexture = mainTexture;
						FengGameManagerMKII.linkHash[0].Add(hairlink, iteratorVariable1.renderer.material);
						iteratorVariable1.renderer.material = (Material)FengGameManagerMKII.linkHash[0][hairlink];
					}
					else
					{
						iteratorVariable1.renderer.material = (Material)FengGameManagerMKII.linkHash[0][hairlink];
					}
				}
				part_hair = iteratorVariable1;
			}
		}
		if (eye >= 0)
		{
			setFacialTexture(this.eye, eye);
		}
		if (iteratorVariable0)
		{
			FengGameManagerMKII.instance.unloadAssets();
		}
	}

	public void setFacialTexture(GameObject go, int id)
	{
		if (id >= 0)
		{
			float x = 0.125f * (float)(int)((float)id / 8f);
			float y = (0f - 0.25f) * (float)(id % 4);
			go.renderer.material.mainTextureOffset = new Vector2(x, y);
		}
	}

	public void setHair2()
	{
		if ((int)FengGameManagerMKII.settings[1] == 1 && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
		{
			int num = Random.Range(0, 9);
			if (num == 3)
			{
				num = 9;
			}
			int num2 = skin - 70;
			if ((int)FengGameManagerMKII.settings[32] == 1)
			{
				num2 = Random.Range(16, 20);
			}
			if ((int)FengGameManagerMKII.settings[num2] >= 0)
			{
				num = (int)FengGameManagerMKII.settings[num2];
			}
			string text = (string)FengGameManagerMKII.settings[num2 + 5];
			int num3 = Random.Range(1, 8);
			if (haseye)
			{
				num3 = 0;
			}
			bool flag = false;
			if (text.EndsWith(".jpg") || text.EndsWith(".png") || text.EndsWith(".jpeg"))
			{
				flag = true;
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
			{
				if (flag)
				{
					object[] parameters = new object[3] { num, num3, text };
					base.photonView.RPC("setHairRPC2", PhotonTargets.AllBuffered, parameters);
				}
				else
				{
					Color hair_color = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
					object[] parameters = new object[5] { num, num3, hair_color.r, hair_color.g, hair_color.b };
					base.photonView.RPC("setHairPRC", PhotonTargets.AllBuffered, parameters);
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (flag)
				{
					StartCoroutine(loadskinE(num, num3, text));
					return;
				}
				Color hair_color = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
				setHairPRC(num, num3, hair_color.r, hair_color.g, hair_color.b);
			}
		}
		else
		{
			int num = Random.Range(0, CostumeHair.hairsM.Length);
			if (num == 3)
			{
				num = 9;
			}
			Object.Destroy(part_hair);
			hairType = num;
			hair = CostumeHair.hairsM[num];
			if (hair.hair == string.Empty)
			{
				hair = CostumeHair.hairsM[9];
				hairType = 9;
			}
			part_hair = (GameObject)Object.Instantiate(CacheResources.Load("Character/" + hair.hair));
			part_hair.transform.parent = hair_go_ref.transform.parent;
			part_hair.transform.position = hair_go_ref.transform.position;
			part_hair.transform.rotation = hair_go_ref.transform.rotation;
			part_hair.transform.localScale = hair_go_ref.transform.localScale;
			part_hair.renderer.material = CharacterMaterials.materials[hair.texture];
			part_hair.renderer.material.color = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
			int num4 = Random.Range(1, 8);
			setFacialTexture(eye, num4);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
			{
				object[] parameters = new object[5]
				{
					hairType,
					num4,
					part_hair.renderer.material.color.r,
					part_hair.renderer.material.color.g,
					part_hair.renderer.material.color.b
				};
				base.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, parameters);
			}
		}
	}

	[RPC]
	private void setHairPRC(int type, int eye_type, float c1, float c2, float c3)
	{
		Object.Destroy(part_hair);
		hair = CostumeHair.hairsM[type];
		hairType = type;
		if (hair.hair != string.Empty)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(CacheResources.Load("Character/" + hair.hair));
			gameObject.transform.parent = hair_go_ref.transform.parent;
			gameObject.transform.position = hair_go_ref.transform.position;
			gameObject.transform.rotation = hair_go_ref.transform.rotation;
			gameObject.transform.localScale = hair_go_ref.transform.localScale;
			gameObject.renderer.material = CharacterMaterials.materials[hair.texture];
			gameObject.renderer.material.color = new Color(c1, c2, c3);
			part_hair = gameObject;
		}
		setFacialTexture(eye, eye_type);
	}

	[RPC]
	public void setHairRPC2(int hair, int eye, string hairlink)
	{
		if ((int)FengGameManagerMKII.settings[1] == 1)
		{
			StartCoroutine(loadskinE(hair, eye, hairlink));
		}
	}

	public void setPunkHair()
	{
		Object.Destroy(part_hair);
		hair = CostumeHair.hairsM[3];
		hairType = 3;
		GameObject gameObject = (GameObject)Object.Instantiate(CacheResources.Load("Character/" + hair.hair));
		gameObject.transform.parent = hair_go_ref.transform.parent;
		gameObject.transform.position = hair_go_ref.transform.position;
		gameObject.transform.rotation = hair_go_ref.transform.rotation;
		gameObject.transform.localScale = hair_go_ref.transform.localScale;
		gameObject.renderer.material = CharacterMaterials.materials[hair.texture];
		switch (Random.Range(1, 4))
		{
		case 1:
			gameObject.renderer.material.color = FengColor.hairPunk1;
			break;
		case 2:
			gameObject.renderer.material.color = FengColor.hairPunk2;
			break;
		case 3:
			gameObject.renderer.material.color = FengColor.hairPunk3;
			break;
		}
		part_hair = gameObject;
		setFacialTexture(eye, 0);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			object[] parameters = new object[5]
			{
				hairType,
				0,
				part_hair.renderer.material.color.r,
				part_hair.renderer.material.color.g,
				part_hair.renderer.material.color.b
			};
			base.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, parameters);
		}
	}

	public void setVar(int skin, bool haseye)
	{
		this.skin = skin;
		this.haseye = haseye;
	}
}
