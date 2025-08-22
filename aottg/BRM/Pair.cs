using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BRM
{
	[Serializable]
	[ComVisible(true)]
	public class Pair<TKey, TValue>
	{
		public TKey First;

		public TValue Second;

		public Pair(KeyValuePair<TKey, TValue> pair)
		{
			First = pair.Key;
			Second = pair.Value;
		}

		public Pair(DictionaryEntry entry)
		{
			First = (TKey)entry.Key;
			Second = (TValue)entry.Value;
		}

		public Pair(TKey first, TValue second)
		{
			First = first;
			Second = second;
		}

		public bool AnyEquals(object obj)
		{
			if (obj == null)
			{
				if (this != null && First != null)
				{
					return Second == null;
				}
				return true;
			}
			if (this != obj && (!(obj is TKey) || !First.Equals((TKey)obj)) && (!(obj is TValue) || !Second.Equals((TValue)obj)) && this != obj && !First.Equals((TKey)obj))
			{
				return Second.Equals((TValue)obj);
			}
			return true;
		}

		public Pair<TKey, TValue> Clone()
		{
			return new Pair<TKey, TValue>(First, Second);
		}

		public override bool Equals(object obj)
		{
			if (obj is Pair<TKey, TValue>)
			{
				Pair<TKey, TValue> pair = (Pair<TKey, TValue>)obj;
				if (pair != null && First.Equals(pair.First))
				{
					return Second.Equals(pair.Second);
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static implicit operator KeyValuePair<TKey, TValue>(Pair<TKey, TValue> pair)
		{
			return new KeyValuePair<TKey, TValue>(pair.First, pair.Second);
		}

		public static implicit operator Pair<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
		{
			return new Pair<TKey, TValue>(pair);
		}

		public static implicit operator DictionaryEntry(Pair<TKey, TValue> pair)
		{
			return new DictionaryEntry(pair.First, pair.Second);
		}

		public static implicit operator Pair<TKey, TValue>(DictionaryEntry entry)
		{
			return new Pair<TKey, TValue>(entry);
		}

		public override string ToString()
		{
			return string.Format("[{0}, {1}]", new object[2]
			{
				(First != null) ? First.ToString() : string.Empty,
				(Second != null) ? Second.ToString() : string.Empty
			});
		}

		public string ToString(Func<TKey, string> clause1)
		{
			return string.Format("[{0}, {1}]", new object[2]
			{
				(First != null) ? clause1(First).ToString() : string.Empty,
				(Second != null) ? Second.ToString() : string.Empty
			});
		}

		public string ToString(Func<TKey, string> clause1, Func<TValue, string> clause2)
		{
			return string.Format("[{0}, {1}]", new object[2]
			{
				(First != null) ? clause1(First).ToString() : string.Empty,
				(Second != null) ? clause2(Second).ToString() : string.Empty
			});
		}
	}
}
