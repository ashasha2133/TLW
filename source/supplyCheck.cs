using UnityEngine;

public class supplyCheck : MonoBehaviour
{
	private float elapsedTime;

	private float stepTime = 1f;

	private void Start()
	{
	}

	private void Update()
	{
		elapsedTime += Time.deltaTime;
		if (!(elapsedTime > stepTime))
		{
			return;
		}
		elapsedTime -= stepTime;
		foreach (GameObject allhero in FengGameManagerMKII.allheroes)
		{
			if (!(allhero.GetComponent<HERO>() != null))
			{
				continue;
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (Vector3.Distance(allhero.transform.position, base.transform.position) < 1.5f)
				{
					allhero.GetComponent<HERO>().getSupply();
				}
			}
			else if (allhero.GetPhotonView().isMine && Vector3.Distance(allhero.transform.position, base.transform.position) < 1.5f)
			{
				allhero.GetComponent<HERO>().getSupply();
			}
		}
	}
}
