using System;
using System.Collections.Generic;

namespace Events
{
	public static class EventManager
	{
		public static Dictionary<string, EventElement> defaultMapping = new Dictionary<string, EventElement>();

		public static void CreateEvent(string name)
		{
			if (defaultMapping.ContainsKey(name))
			{
				defaultMapping.Remove(name);
			}
			defaultMapping.Add(name, new EventElement());
		}

		public static void SubscribeEvent(string name, Action<string> callback)
		{
			if (!defaultMapping.ContainsKey(name))
			{
				CreateEvent(name);
			}
			defaultMapping[name] += new EventDelegate(callback.Invoke);
		}

		public static void CallEvent(string name)
		{
			if (defaultMapping.ContainsKey(name))
			{
				defaultMapping[name].Dispatch(name);
			}
		}
	}
}
