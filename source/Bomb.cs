using System;
using System.Collections;
using System.Collections.Generic;
using CustomAudio;
using Events;
using Photon;
using UnityEngine;
using Utility;

public class Bomb : Photon.MonoBehaviour
{
	public static int bombId;

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	private Vector3 correctPlayerVelocity = Vector3.zero;

	public bool Disabled;

	public float SmoothingDelay = 10f;

	public float BombRadius;

	private TITAN _collidedTitan;

	private SphereCollider _sphereCollider;

	private List<GameObject> _hideUponDestroy;

	private ParticleSystem _trail;

	private ParticleSystem _flame;

	private float _DisabledTrailFadeMultiplier = 0.6f;

	private bool _receivedNonZeroVelocity;

	private bool _ownerIsUpdated;

	public HERO myHero;

	private float _minimumLuminance = 0.4f;

	public GameObject myExplosion;

	private Transform baseT;

	private Rigidbody baseR;

	private PhotonView basePV;

	private bool isMine
	{
		get
		{
			if (basePV == null)
			{
				basePV = base.photonView;
			}
			if (basePV != null)
			{
				return basePV.isMine;
			}
			return false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isMine && !Disabled && (int)FengGameManagerMKII.settings[291] == 0)
		{
			Explode(BombRadius);
		}
	}

	public void Awake()
	{
		baseT = base.transform;
		baseR = base.rigidbody;
		basePV = base.photonView;
		if (basePV != null)
		{
			if (basePV.synchronization != ViewSynchronization.UnreliableOnChange)
			{
				basePV.synchronization = ViewSynchronization.UnreliableOnChange;
			}
			basePV.observed = this;
			correctPlayerPos = baseT.position;
			correctPlayerRot = Quaternion.identity;
			PhotonPlayer owner = base.photonView.owner;
			_trail = baseT.Find("Trail").GetComponent<ParticleSystem>();
			_flame = baseT.Find("Flame").GetComponent<ParticleSystem>();
			_sphereCollider = GetComponent<SphereCollider>();
			_sphereCollider.radius /= 2f;
			_hideUponDestroy = new List<GameObject>();
			_hideUponDestroy.Add(baseT.Find("Flame").gameObject);
			_hideUponDestroy.Add(baseT.Find("ThunderSpearModel").gameObject);
			if (GameSettings.ShowBombColor > 0)
			{
				Color bombColor = BombUtil.GetBombColor(owner, 1f);
				_trail.startColor = bombColor;
			}
			if (base.photonView.isMine)
			{
				base.photonView.RPC("IsUpdatedRPC", PhotonTargets.All);
				EventManager.CallEvent("bomb_shot");
			}
			else if (ThymesUtils.RelativeLuminance(_trail.startColor) < _minimumLuminance)
			{
				_trail.startColor = Color.white;
			}
		}
	}

	[RPC]
	private void IsUpdatedRPC(PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			_ownerIsUpdated = true;
		}
	}

	public void DestroySelf()
	{
		if (isMine && !Disabled)
		{
			base.photonView.RPC("DisableRPC", PhotonTargets.All);
			StartCoroutine(WaitAndFinishDestroyCoroutine(1.5f));
		}
	}

	public void Explode(float radius)
	{
		if (!Disabled)
		{
			try
			{
				base.rigidbody.isKinematic = false;
				PhotonNetwork.Instantiate("RCAsset/BombExplodeMain", baseT.position, Quaternion.Euler(0f, 0f, 0f), 0);
				KillPlayersInRadius(radius);
			}
			catch (Exception)
			{
			}
			DestroySelf();
			if (GameSettings.BombExplosion > 0 && AudioListener.volume > 0f)
			{
				CustomAudioManager.Play(CustomAudioManager.AudioFile.BombExplosion);
			}
			myHero.skillCDDuration = myHero.bombCD;
		}
	}

	private void KillPlayersInRadius(float radius)
	{
		foreach (HERO hero in FengGameManagerMKII.heroes)
		{
			GameObject gameObject = hero.gameObject;
			if (!(Vector3.Distance(gameObject.transform.position, baseT.position) < radius) || gameObject.GetPhotonView().isMine || hero.bombImmune)
			{
				continue;
			}
			PhotonPlayer owner = hero.basePV.owner;
			if (owner.ID == PhotonNetwork.player.ID)
			{
				continue;
			}
			if (GameSettings.teamMode > 0 && PhotonNetwork.player.customProperties["RCteam"] != null && owner.customProperties["RCteam"] != null)
			{
				int num = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]);
				int num2 = RCextensions.returnIntFromObject(owner.customProperties["RCteam"]);
				if (num == 0 || num != num2)
				{
					KillPlayer(hero);
					if (GameSettings.BombKill > 0 && AudioListener.volume > 0f)
					{
						float x = Mathf.Clamp(IN_GAME_MAIN_CAMERA.main_objectR.velocity.magnitude / 100f, 0f, 8f);
						float pitch = ThymesUtils.Map(x, 0f, 8f, 1f, 2f);
						float volume = ThymesUtils.Map(x, 0f, 8f, 0.2f, 0.6f) * PlayerPrefs.GetFloat("vol", 1f);
						CustomAudioManager.PlayDynamic(CustomAudioManager.AudioFile.BombKill, pitch, volume);
					}
				}
			}
			else
			{
				KillPlayer(hero);
				if (GameSettings.BombKill > 0 && AudioListener.volume > 0f)
				{
					float x2 = Mathf.Clamp(IN_GAME_MAIN_CAMERA.main_objectR.velocity.magnitude / 100f, 0f, 8f);
					float pitch2 = ThymesUtils.Map(x2, 0f, 8f, 1f, 2f);
					float volume2 = ThymesUtils.Map(x2, 0f, 8f, 0.2f, 0.6f) * PlayerPrefs.GetFloat("vol", 1f);
					CustomAudioManager.PlayDynamic(CustomAudioManager.AudioFile.BombKill, pitch2, volume2);
				}
			}
		}
	}

	private void KillPlayer(HERO hero)
	{
		HERO hERO = HERO.FindMyHero(PhotonNetwork.player.ID);
		if (hERO.PersonWhoCarries != hero.transform.root.GetComponent<HERO>() && hERO.PersonBeingCarried != hero.transform.root.GetComponent<HERO>() && !(Vector3.Distance(hero.transform.position, hERO.transform.position) < 3f))
		{
			hero.markDie();
			hero.photonView.RPC("netDie2", PhotonTargets.All, -1, ((int)FengGameManagerMKII.settings[359] == 0) ? ((string)FengGameManagerMKII.settings[328] + " ") : ((string)FengGameManagerMKII.settings[337] + " "));
			FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
		}
	}

	private void SetDisabledTrailFade()
	{
		int particleCount = _trail.particleCount;
		float startLifetime = _trail.startLifetime * _DisabledTrailFadeMultiplier;
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[particleCount];
		_trail.GetParticles(array);
		for (int i = 0; i < particleCount; i++)
		{
			ParticleSystem.Particle[] array2 = array;
			int num = i;
			array2[num].lifetime = array2[num].lifetime * _DisabledTrailFadeMultiplier;
			array[i].startLifetime = startLifetime;
		}
		_trail.SetParticles(array, particleCount);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(baseT.position);
			stream.SendNext(baseT.rotation);
			stream.SendNext(baseR.velocity);
		}
		else
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
			correctPlayerVelocity = (Vector3)stream.ReceiveNext();
		}
	}

	[RPC]
	public void DisableRPC(PhotonMessageInfo info = null)
	{
		if (Disabled || (info != null && info.sender != base.photonView.owner))
		{
			return;
		}
		foreach (GameObject item in _hideUponDestroy)
		{
			item.SetActive(value: false);
		}
		_sphereCollider.enabled = false;
		SetDisabledTrailFade();
		base.rigidbody.velocity = Vector3.zero;
		Disabled = true;
	}

	public void Update()
	{
		if (isMine)
		{
			return;
		}
		if (baseT == null)
		{
			baseT = base.transform;
		}
		if (baseR == null)
		{
			baseR = base.rigidbody;
		}
		if (baseT != null)
		{
			baseT.position = Vector3.Lerp(baseT.position, correctPlayerPos, Time.deltaTime * SmoothingDelay);
			baseT.rotation = Quaternion.Lerp(baseT.rotation, correctPlayerRot, Time.deltaTime * SmoothingDelay);
		}
		if (baseR != null)
		{
			baseR.velocity = correctPlayerVelocity;
		}
		if (base.rigidbody.velocity != Vector3.zero)
		{
			_receivedNonZeroVelocity = true;
		}
		else
		{
			if (_ownerIsUpdated || !_receivedNonZeroVelocity || Disabled)
			{
				return;
			}
			Disabled = true;
			foreach (GameObject item in _hideUponDestroy)
			{
				item.SetActive(value: false);
			}
		}
	}

	private IEnumerator WaitAndFinishDestroyCoroutine(float time)
	{
		yield return new WaitForSeconds(time);
		if (_collidedTitan != null)
		{
			_collidedTitan.isThunderSpear = false;
		}
		PhotonNetwork.Destroy(base.gameObject);
	}
}
