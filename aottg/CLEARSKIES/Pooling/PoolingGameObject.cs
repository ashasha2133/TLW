using System.Collections.Generic;
using UnityEngine;

namespace CLEARSKIES.Pooling
{
	public class PoolingGameObject : MonoBehaviour
	{
		private List<PoolGameObject> objects;

		private Transform objectsParent;

		public void Initialize(int count, PoolGameObject sample, Transform objects_parent)
		{
			objects = new List<PoolGameObject>();
			objectsParent = objects_parent;
			for (int i = 0; i < count; i++)
			{
				AddObject(sample, objects_parent);
			}
		}

		public PoolGameObject GetObject()
		{
			for (int i = 0; i < objects.Count; i++)
			{
				if (!objects[i].gameObject.activeInHierarchy)
				{
					return objects[i];
				}
			}
			AddObject(objects[0], objectsParent);
			return objects[objects.Count - 1];
		}

		private void AddObject(PoolGameObject sample, Transform objects_parent)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(sample.gameObject);
			gameObject.name = sample.name;
			gameObject.transform.SetParent(objects_parent);
			objects.Add(gameObject.GetComponent<PoolGameObject>());
			if ((bool)gameObject.GetComponent<Animator>())
			{
				gameObject.GetComponent<Animator>().StartPlayback();
			}
			gameObject.SetActive(value: false);
		}
	}
}
