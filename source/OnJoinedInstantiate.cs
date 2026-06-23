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
			Vector3 insideUnitSphere = Random.insideUnitSphere;
			insideUnitSphere.y = 0f;
			insideUnitSphere = insideUnitSphere.normalized;
			PhotonNetwork.Instantiate(position: vector + PositionOffset * insideUnitSphere, prefabName: obj.name, rotation: Quaternion.identity, group: 0);
		}
	}
}
