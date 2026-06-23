using System;
using System.Collections.Generic;
using System.IO;
using Photon;
using UnityEngine;

namespace BRM;

public class Pool
{
	public static Dictionary<int, Pool> AllPools = new Dictionary<int, Pool>();

	public static Dictionary<int, List<GameObject>> AllObjects = new Dictionary<int, List<GameObject>>();

	public int ownerHashCode;

	public PhotonView ownerPV;

	public List<GameObject> Objects;

	public Dictionary<int, string> IDToName;

	public Dictionary<string, string> PathToName;

	public Dictionary<string, int> PathToID;

	public Dictionary<string, int> NameToID;

	public Dictionary<string, int> Instantiations;

	public Pool(int hashCode, PhotonView view, Dictionary<string, int> instantiations)
	{
		ownerPV = view;
		ownerHashCode = hashCode;
		Instantiations = instantiations;
		Objects = new List<GameObject>();
		IDToName = new Dictionary<int, string>();
		NameToID = new Dictionary<string, int>();
		PathToID = new Dictionary<string, int>();
		PathToName = new Dictionary<string, string>();
		foreach (KeyValuePair<string, int> instantiation in instantiations)
		{
			if (instantiation.Value == 0 || instantiation.Key.IsNullOrEmpty())
			{
				continue;
			}
			GameObject gameObject = CacheResources.Load<GameObject>(instantiation.Key);
			if (gameObject == null)
			{
				continue;
			}
			if (instantiation.Value == 1)
			{
				GameObject gameObject2;
				if ((gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject)) == null)
				{
					UnityEngine.Object.Destroy(gameObject2);
					continue;
				}
				Photon.MonoBehaviour[] components = gameObject2.GetComponents<Photon.MonoBehaviour>();
				foreach (Photon.MonoBehaviour monoBehaviour in components)
				{
					if (!(monoBehaviour == null))
					{
						monoBehaviour.homePool = this;
					}
				}
				Objects.Add(gameObject2);
				string name = gameObject2.name;
				int instanceID = gameObject2.GetInstanceID();
				gameObject2.SetActive(value: false);
				if (!PathToID.ContainsKey(instantiation.Key))
				{
					PathToID.Add(instantiation.Key, instanceID);
				}
				if (!name.IsNullOrEmpty())
				{
					if (!PathToName.ContainsKey(instantiation.Key))
					{
						PathToName.Add(instantiation.Key, name);
					}
					if (!IDToName.ContainsKey(instanceID))
					{
						IDToName.Add(instanceID, name);
					}
					if (!NameToID.ContainsKey(name))
					{
						NameToID.Add(name, instanceID);
					}
				}
				continue;
			}
			for (int j = 0; j < instantiation.Value; j++)
			{
				GameObject gameObject3;
				if ((gameObject3 = (GameObject)UnityEngine.Object.Instantiate(gameObject)) == null)
				{
					UnityEngine.Object.Destroy(gameObject3);
					continue;
				}
				Photon.MonoBehaviour[] components = gameObject3.GetComponents<Photon.MonoBehaviour>();
				foreach (Photon.MonoBehaviour monoBehaviour2 in components)
				{
					if (!(monoBehaviour2 == null))
					{
						monoBehaviour2.homePool = this;
					}
				}
				Objects.Add(gameObject3);
				string name2 = gameObject3.name;
				int instanceID2 = gameObject3.GetInstanceID();
				gameObject3.SetActive(value: false);
				if (!PathToID.ContainsKey(instantiation.Key))
				{
					PathToID.Add(instantiation.Key, instanceID2);
				}
				if (!name2.IsNullOrEmpty())
				{
					if (!PathToName.ContainsKey(instantiation.Key))
					{
						PathToName.Add(instantiation.Key, name2);
					}
					if (!IDToName.ContainsKey(instanceID2))
					{
						IDToName.Add(instanceID2, name2);
					}
					if (!NameToID.ContainsKey(name2))
					{
						NameToID.Add(name2, instanceID2);
					}
				}
			}
		}
		AllObjects[ownerHashCode] = Objects;
		AllPools[ownerHashCode] = this;
	}

	public Pool(Photon.MonoBehaviour mono, PhotonView view, Dictionary<string, int> instantiations)
		: this(mono.GetHashCode(), view, instantiations)
	{
	}

	public Pool(GameObject GO, PhotonView view, Dictionary<string, int> instantiations)
		: this(GO.GetHashCode(), view, instantiations)
	{
	}

	public override bool Equals(object obj)
	{
		if (obj is Pool)
		{
			if (obj is Pool pool)
			{
				return pool.GetHashCode() == GetHashCode();
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ownerHashCode;
	}

	public int AmountOf(string objectName)
	{
		if (Instantiations.TryGetValue(objectName, out var value))
		{
			return value;
		}
		return 0;
	}

	public static int AmountOf(int hashCode, string objectName)
	{
		if (AllPools.TryGetValue(hashCode, out var value) && value.Instantiations.TryGetValue(objectName, out var value2))
		{
			return value2;
		}
		return 0;
	}

	public bool Contains(string objectName)
	{
		if (!Instantiations.ContainsKey(objectName))
		{
			return PathToID.ContainsKey(objectName);
		}
		return true;
	}

	public bool Contains(GameObject GO)
	{
		return Objects.Contains(GO);
	}

	public bool Contains(Photon.MonoBehaviour mono)
	{
		if (!(mono == null))
		{
			return Objects.Contains(mono.gameObject);
		}
		return false;
	}

	public void Clear()
	{
		foreach (GameObject item in new Queue<GameObject>(Objects))
		{
			UnityEngine.Object.DestroyImmediate(item, allowDestroyingAssets: false);
		}
		AllObjects.Remove(ownerHashCode);
		AllPools.Remove(ownerHashCode);
	}

	public bool TryEnable(out GameObject GO, string path, Vector3 position, Quaternion rotation, short prefix, int instantiationId, int[] viewIDs, object[] instantiationData)
	{
		if (PathToName.ContainsKey(path))
		{
			string text = PathToName[path];
			for (int i = 0; i < Objects.Count; i++)
			{
				if ((GO = Objects[i]) == null || GO.name.IsNullOrEmpty() || GO.name != text || GO.activeInHierarchy)
				{
					continue;
				}
				PhotonView[] componentsInChildren = GO.GetComponentsInChildren<PhotonView>(includeInactive: true);
				PhotonView photonView = componentsInChildren[0];
				if (FengGameManagerMKII.instance != null)
				{
					PhotonView pView = FengGameManagerMKII.PView;
					if (pView.viewID == instantiationId || pView.viewID == photonView.viewID)
					{
						GO = null;
						return true;
					}
				}
				if (componentsInChildren.Length != viewIDs.Length)
				{
					throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
				}
				int j;
				for (j = 0; j < viewIDs.Length; j++)
				{
					componentsInChildren[j].viewID = 0;
					componentsInChildren[j].prefix = -1;
					componentsInChildren[j].prefixBackup = -1;
					componentsInChildren[j].instantiationId = -1;
				}
				j = 0;
				while (j < viewIDs.Length)
				{
					componentsInChildren[j].viewID = viewIDs[j];
					componentsInChildren[j].prefix = prefix;
					componentsInChildren[j].instantiationId = instantiationId;
					i++;
				}
				PhotonNetwork.networkingPeer.StoreInstantiationData(instantiationId, instantiationData);
				Transform transform = GO.transform;
				if (transform != null)
				{
					transform.position = position;
					transform.rotation = rotation;
				}
				GO.SetActive(value: true);
				PhotonNetwork.networkingPeer.RemoveInstantiationData(instantiationId);
				return true;
			}
		}
		GO = null;
		return false;
	}

	public bool TryEnable<T>(out T GO, string path, Vector3 position, Quaternion rotation, short prefix, int instantiationId, int[] viewIDs, object[] instantiationData) where T : Component
	{
		if (PathToName.ContainsKey(path))
		{
			string text = PathToName[path];
			for (int i = 0; i < Objects.Count; i++)
			{
				if (Objects[i] == null || Objects[i].name.IsNullOrEmpty() || Objects[i].name != text || (GO = Objects[i].GetComponent<T>()) == null || Objects[i].activeInHierarchy)
				{
					continue;
				}
				PhotonView[] componentsInChildren = GO.GetComponentsInChildren<PhotonView>(includeInactive: true);
				PhotonView photonView = componentsInChildren[0];
				if (FengGameManagerMKII.instance != null)
				{
					PhotonView pView = FengGameManagerMKII.PView;
					if (pView.viewID == instantiationId || pView.viewID == photonView.viewID)
					{
						GO = null;
						return true;
					}
				}
				if (componentsInChildren.Length != viewIDs.Length)
				{
					throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
				}
				int j;
				for (j = 0; j < viewIDs.Length; j++)
				{
					componentsInChildren[j].viewID = 0;
					componentsInChildren[j].prefix = -1;
					componentsInChildren[j].prefixBackup = -1;
					componentsInChildren[j].instantiationId = -1;
				}
				j = 0;
				while (j < viewIDs.Length)
				{
					componentsInChildren[j].viewID = viewIDs[j];
					componentsInChildren[j].prefix = prefix;
					componentsInChildren[j].instantiationId = instantiationId;
					i++;
				}
				PhotonNetwork.networkingPeer.StoreInstantiationData(instantiationId, instantiationData);
				Transform transform = GO.transform;
				if (transform != null)
				{
					transform.position = position;
					transform.rotation = rotation;
				}
				Objects[i].SetActive(value: true);
				PhotonNetwork.networkingPeer.RemoveInstantiationData(instantiationId);
				return true;
			}
		}
		GO = null;
		return false;
	}

	public GameObject Enable(string path, Vector3 position, Quaternion rotation, bool localOnly = false)
	{
		if (localOnly || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (PathToName.ContainsKey(path))
			{
				string text = PathToName[path];
				for (int i = 0; i < Objects.Count; i++)
				{
					if (!(Objects[i].name != text) && !Objects[i].activeInHierarchy)
					{
						Transform transform = Objects[i].transform;
						if (transform != null)
						{
							transform.position = position;
							transform.rotation = rotation;
						}
						Objects[i].SetActive(value: true);
						return Objects[i];
					}
				}
			}
			return null;
		}
		return this.PoolEnable(path, position, rotation);
	}

	public GameObject Enable(string path, Vector3 position, Quaternion rotation, RaiseEventOptions options)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (PathToName.ContainsKey(path))
			{
				string text = PathToName[path];
				for (int i = 0; i < Objects.Count; i++)
				{
					if (!Objects[i].activeInHierarchy && !(Objects[i].name != text))
					{
						Transform transform = Objects[i].transform;
						if (transform != null)
						{
							transform.position = position;
							transform.rotation = rotation;
						}
						Objects[i].SetActive(value: true);
						return Objects[i];
					}
				}
			}
			return null;
		}
		return this.PoolEventEnable(path, position, rotation, options);
	}

	public T Enable<T>(string path, Vector3 position, Quaternion rotation, bool localOnly = false) where T : Component
	{
		if (localOnly || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (PathToName.ContainsKey(path))
			{
				string text = PathToName[path];
				for (int i = 0; i < Objects.Count; i++)
				{
					if (!Objects[i].activeInHierarchy && !(Objects[i].GetComponent<T>() == null) && (!(Objects[i] != null) || !(Objects[i].name.NullFix() != text)))
					{
						Transform transform = Objects[i].transform;
						if (transform != null)
						{
							transform.position = position;
							transform.rotation = rotation;
						}
						Objects[i].SetActive(value: true);
						return Objects[i].GetComponent<T>();
					}
				}
			}
			return null;
		}
		return this.PoolEnable<T>(path, position, rotation);
	}

	public T Enable<T>(string path, Vector3 position, Quaternion rotation, RaiseEventOptions options) where T : Component
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (PathToName.ContainsKey(path))
			{
				string text = PathToName[path];
				for (int i = 0; i < Objects.Count; i++)
				{
					if (!Objects[i].activeInHierarchy && !(Objects[i].GetComponent<T>() == null) && (!(Objects[i] != null) || !(Objects[i].name.NullFix() != text)))
					{
						Transform transform = Objects[i].transform;
						if (transform != null)
						{
							transform.position = position;
							transform.rotation = rotation;
						}
						Objects[i].SetActive(value: true);
						return Objects[i].GetComponent<T>();
					}
				}
			}
			return null;
		}
		return this.PoolEventEnable<T>(path, position, rotation, options);
	}

	public GameObject Enable(string path, Vector3 position, Quaternion rotation, PhotonView view)
	{
		if (PathToName.ContainsKey(path))
		{
			string text = PathToName[path];
			for (int i = 0; i < Objects.Count; i++)
			{
				if (!Objects[i].activeInHierarchy && (!(Objects[i] != null) || !(Objects[i].name.NullFix() != text)))
				{
					Transform transform = Objects[i].transform;
					if (transform != null)
					{
						transform.position = position;
						transform.rotation = rotation;
					}
					PhotonView component = Objects[i].GetComponent<PhotonView>();
					if (component != null && view != null)
					{
						component.viewID = view.viewID;
					}
					Objects[i].SetActive(value: true);
					return Objects[i];
				}
			}
		}
		return null;
	}

	public bool Disable(string objectName, bool localOnly = false)
	{
		if (NameToID.ContainsKey(objectName))
		{
			string text = IDToName[NameToID[objectName]];
			for (int i = 0; i < Objects.Count; i++)
			{
				if (Objects[i].activeInHierarchy && (!(Objects[i] != null) || !(Objects[i].name.NullFix() != text)))
				{
					if (localOnly || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						Objects[i].SetActive(value: false);
					}
					else
					{
						this.PoolDisable(Objects[i]);
					}
					return true;
				}
			}
		}
		return false;
	}

	public bool Disable(GameObject GO, bool localOnly = false)
	{
		if (GO == null)
		{
			return false;
		}
		if (Objects.Contains(GO))
		{
			if (localOnly || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				GO.SetActive(value: false);
			}
			else
			{
				this.PoolDisable(GO);
			}
			return true;
		}
		return false;
	}

	public bool Disable(Component mono, bool localOnly = false)
	{
		if (mono == null)
		{
			return false;
		}
		GameObject gameObject = mono.gameObject;
		if (gameObject == null)
		{
			return false;
		}
		if (Objects.Contains(gameObject))
		{
			if (localOnly || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				gameObject.SetActive(value: false);
			}
			else
			{
				this.PoolDisable(gameObject);
			}
			return true;
		}
		return false;
	}

	public static void WriteOutPoolObjects()
	{
		Queue<string> queue = new Queue<string>();
		queue.Enqueue(typeof(EnemyfxIDcontainer).FullName);
		EnemyfxIDcontainer[] array = Resources.LoadAll<EnemyfxIDcontainer>("");
		foreach (EnemyfxIDcontainer enemyfxIDcontainer in array)
		{
			queue.Enqueue(enemyfxIDcontainer.name);
		}
		queue.Enqueue(string.Empty);
		queue.Enqueue(typeof(EnemyCheckCollider).FullName);
		EnemyCheckCollider[] array2 = Resources.LoadAll<EnemyCheckCollider>("");
		foreach (EnemyCheckCollider enemyCheckCollider in array2)
		{
			queue.Enqueue(enemyCheckCollider.name.NullFix());
		}
		queue.Enqueue(string.Empty);
		queue.Enqueue(typeof(UnityEngine.Object).FullName);
		UnityEngine.Object[] array3 = Resources.LoadAll("");
		foreach (UnityEngine.Object obj in array3)
		{
			queue.Enqueue(obj.name.NullFix());
		}
		queue.Enqueue(string.Empty);
		queue.Enqueue(typeof(SelfDestroy).FullName);
		SelfDestroy[] array4 = Resources.LoadAll<SelfDestroy>("");
		foreach (SelfDestroy selfDestroy in array4)
		{
			queue.Enqueue(selfDestroy.name.NullFix());
		}
		queue.Enqueue(string.Empty);
		queue.Enqueue(typeof(UnityEngine.Object).FullName);
		array3 = Resources.LoadAll("FX");
		foreach (UnityEngine.Object obj2 in array3)
		{
			if (obj2 is Component)
			{
				queue.Enqueue(string.Format("{0}\r\n\tComponents:{1}\r\n", obj2.name.NullFix(), ((Component)obj2).GetComponents<Component>().ToEngFormat("and", (Component c) => c.GetType().FullName)));
			}
			else if (obj2 is GameObject)
			{
				queue.Enqueue(string.Format("{0}\r\n\tComponents:{1}\r\n", obj2.name.NullFix(), ((GameObject)obj2).GetComponents<Component>().ToEngFormat("and", (Component c) => c.GetType().FullName)));
			}
			else
			{
				queue.Enqueue(obj2.name.NullFix());
			}
		}
		File.WriteAllLines("fx.rtf", queue.ToArray());
	}
}
