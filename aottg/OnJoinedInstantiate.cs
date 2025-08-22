using UnityEngine;

public class OnJoinedInstantiate : MonoBehaviour
{
	public float PositionOffset = 2f;

	public GameObject[] PrefabsToInstantiate;

	public Transform SpawnPosition;

	public void OnJoinedRoom()
	{
		if (PrefabsToInstantiate == null)
		{
			return;
		}
		GameObject[] prefabsToInstantiate = PrefabsToInstantiate;
		foreach (GameObject obj in prefabsToInstantiate)
		{
			Vector3 vector = Vector3.up;
			if (SpawnPosition != null)
			{
				vector = SpawnPosition.position;
			}
			Vector3 vector2 = Random.insideUnitSphere;
			vector2.y = 0f;
			vector2 = vector2.normalized;
			PhotonNetwork.Instantiate(position: vector + PositionOffset * vector2, prefabName: obj.name, rotation: Quaternion.identity, group: 0);
		}
	}
}
