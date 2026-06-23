using UnityEngine;

namespace CLEARSKIES.Pooling;

internal class SetUpPooling : MonoBehaviour
{
	[SerializeField]
	private PoolManager.PoolPart[] pools = new PoolManager.PoolPart[2];

	private void OnValidate()
	{
		for (int i = 0; i < pools.Length; i++)
		{
			pools[i].name = pools[i].prefab.name;
		}
	}

	private void Awake()
	{
		Initialize();
	}

	private void Initialize()
	{
		PoolManager.Initialize(pools);
	}
}
