using System.Collections.Generic;
using UnityEngine;

namespace BRM
{
	public static class CacheGameObject
	{
		private static Dictionary<string, GameObject> cache = new Dictionary<string, GameObject>();

		private static Dictionary<string, Component> cacheType = new Dictionary<string, Component>();

		public static T Find<T>(string name) where T : Component
		{
			string key = name + typeof(T).FullName;
			if (cacheType.ContainsKey(key))
			{
				Component component = cacheType[key];
				if (component != null)
				{
					T val = component as T;
					if ((Object)val != (Object)null)
					{
						return val;
					}
					Component component3 = (cacheType[key] = component.GetComponent<T>());
					return (T)component3;
				}
			}
			GameObject gameObject = Find(name);
			if (gameObject != null)
			{
				Component component3 = (cacheType[key] = gameObject.GetComponent<T>());
				return (T)component3;
			}
			if ((bool)(gameObject = GameObject.Find(name)))
			{
				Component component3 = (cacheType[key] = gameObject.GetComponent<T>());
				return (T)component3;
			}
			return null;
		}

		public static GameObject Find(string name)
		{
			string text = name.ToLower().Trim();
			switch (text)
			{
			case "maincamera":
				if (cache.ContainsKey(name) && cache[name] != null)
				{
					return cache[name];
				}
				return cache[name] = GameObject.Find(name);
			case "aottg_hero1":
			case "aottg_hero1(clone)":
			case "colossal_titan":
			case "femaletitan":
			case "female_titan":
			case "crawler":
			case "punk":
			case "abberant":
			case "jumper":
			case "titan":
			case "tree":
			case "cube001":
				return GameObject.Find(name);
			default:
			{
				GameObject gameObject;
				if (cache.ContainsKey(name) && (bool)(gameObject = cache[name]) && (gameObject.activeInHierarchy || text.StartsWith("ui") || text.StartsWith("label") || text.StartsWith("ngui")))
				{
					return gameObject;
				}
				if ((bool)(gameObject = GameObject.Find(name)))
				{
					return cache[name] = gameObject;
				}
				return gameObject;
			}
			}
		}
	}
}
