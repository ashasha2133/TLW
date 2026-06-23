using UnityEngine;

namespace CLEARSKIES;

internal static class RespawnPositions
{
	private static Vector3[] titanpos;

	public static Vector3 RandomTitanPos
	{
		get
		{
			if (titanpos != null)
			{
				return titanpos[Random.Range(0, titanpos.Length)];
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
			if (array != null)
			{
				titanpos = new Vector3[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					titanpos[i] = array[i].transform.position;
				}
			}
			if (titanpos != null)
			{
				return titanpos[Random.Range(0, titanpos.Length)];
			}
			return Vector3.zero;
		}
	}

	public static Vector3[] TitanPositions
	{
		get
		{
			if (titanpos != null)
			{
				return titanpos;
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
			if (array != null)
			{
				titanpos = new Vector3[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					titanpos[i] = array[i].transform.position;
				}
				return titanpos;
			}
			return new Vector3[0];
		}
	}

	public static void Dispose()
	{
		titanpos = null;
	}
}
