using System.Collections;
using Photon;
using UnityEngine;

public class BombExplode : Photon.MonoBehaviour
{
	public static float _sizeMultiplier = 1.1f;

	private IEnumerator WaitAndDestroyCoroutine(float time)
	{
		yield return new WaitForSeconds(time);
		PhotonNetwork.Destroy(base.gameObject);
	}

	public void Awake()
	{
		if (base.photonView != null)
		{
			PhotonPlayer owner = base.photonView.owner;
			float num = Mathf.Clamp(RCextensions.returnFloatFromObject(owner.customProperties["RCBombRadius"]), 20f, 60f) * 2f * _sizeMultiplier;
			ParticleSystem component = GetComponent<ParticleSystem>();
			if (GameSettings.UseOldBombEffect > 0)
			{
				component.Stop();
				component.Clear();
				component = base.transform.Find("OldExplodeEffect").GetComponent<ParticleSystem>();
				component.gameObject.SetActive(value: true);
				num /= _sizeMultiplier;
			}
			if (GameSettings.ShowBombColor > 0)
			{
				component.startColor = BombUtil.GetBombColor(owner);
			}
			owner.BombHasExploded = true;
			component.startSize = num;
			if (base.photonView.isMine)
			{
				StartCoroutine(WaitAndDestroyCoroutine(1.5f));
			}
		}
	}
}
