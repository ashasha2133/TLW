using UnityEngine;

namespace CLEARSKIES.Pooling;

public class PoolGameObject : MonoBehaviour
{
	public void ReturnToPool()
	{
		base.gameObject.SetActive(value: false);
	}
}
