using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BRM
{
	[Serializable]
	[ComVisible(true)]
	public class Triplet<T1, T2, T3>
	{
		public T1 First;

		public T2 Second;

		public T3 Third;

		public KeyValuePair<T2, T3> this[int optional]
		{
			get
			{
				return new KeyValuePair<T2, T3>(Second, Third);
			}
			set
			{
				Second = value.Key;
				Third = value.Value;
			}
		}

		public KeyValuePair<T1, T2> OneTwoPair
		{
			get
			{
				return new KeyValuePair<T1, T2>(First, Second);
			}
			set
			{
				First = value.Key;
				Second = value.Value;
			}
		}

		public KeyValuePair<T2, T3> TwoThreePair
		{
			get
			{
				return new KeyValuePair<T2, T3>(Second, Third);
			}
			set
			{
				Second = value.Key;
				Third = value.Value;
			}
		}

		public KeyValuePair<T1, T3> OneThreePair
		{
			get
			{
				return new KeyValuePair<T1, T3>(First, Third);
			}
			set
			{
				First = value.Key;
				Third = value.Value;
			}
		}

		public static implicit operator KeyValuePair<T1, KeyValuePair<T2, T3>>(Triplet<T1, T2, T3> ykno)
		{
			return new KeyValuePair<T1, KeyValuePair<T2, T3>>(ykno.First, new KeyValuePair<T2, T3>(ykno.Second, ykno.Third));
		}

		public static Triplet<T1, T2, T3>[]operator +(Triplet<T1, T2, T3> triplet, Triplet<T1, T2, T3> triplet2)
		{
			return new Triplet<T1, T2, T3>[2] { triplet, triplet2 };
		}

		public static Triplet<T1, T2, T3>[]operator +(Triplet<T1, T2, T3> triplet, Triplet<T1, T2, T3>[] triplets)
		{
			Array.Resize(ref triplets, triplets.Length + 1);
			triplets[triplets.Length - 1] = triplet;
			return triplets;
		}

		public static Triplet<T1, T2, T3>[]operator +(Triplet<T1, T2, T3>[] triplets, Triplet<T1, T2, T3> triplet)
		{
			Array.Resize(ref triplets, triplets.Length + 1);
			triplets[triplets.Length - 1] = triplet;
			return triplets;
		}

		public static Triplet<T1, T2, T3>[]operator -(Triplet<T1, T2, T3>[] triplets, Triplet<T1, T2, T3> triplet)
		{
			List<Triplet<T1, T2, T3>> list = triplets.ToList();
			if (list.Remove(triplet))
			{
				return list.ToArray();
			}
			return triplets;
		}

		public Triplet(Triplet<T1, T2, T3> triple)
		{
			First = triple.First;
			Second = triple.Second;
			Third = triple.Third;
		}

		public Triplet(T1 first, T2 second, T3 third)
		{
			First = first;
			Second = second;
			Third = third;
		}

		public Triplet(T1 first, KeyValuePair<T2, T3> pair)
		{
			First = first;
			Second = pair.Key;
			Third = pair.Value;
		}

		public Triplet(KeyValuePair<T1, KeyValuePair<T2, T3>> pair)
		{
			First = pair.Key;
			Second = pair.Value.Key;
			Third = pair.Value.Value;
		}

		public override string ToString()
		{
			return $"[{((First != null) ? First.ToString() : string.Empty)}, {((Second != null) ? Second.ToString() : string.Empty)}, {((Third != null) ? Third.ToString() : string.Empty)}]";
		}

		public string ToString(Func<T1, string> clause1)
		{
			return $"[{((First != null) ? clause1(First).ToString() : string.Empty)}, {((Second != null) ? Second.ToString() : string.Empty)}, {((Third != null) ? Third.ToString() : string.Empty)}]";
		}

		public string ToString(Func<T1, string> clause1, Func<T2, string> clause2)
		{
			return $"[{((First != null) ? clause1(First).ToString() : string.Empty)}, {((Second != null) ? clause2(Second).ToString() : string.Empty)}, {((Third != null) ? Third.ToString() : string.Empty)}]";
		}

		public string ToString(Func<T1, string> clause1, Func<T2, string> clause2, Func<T3, string> clause3)
		{
			return $"[{((First != null) ? clause1(First).ToString() : string.Empty)}, {((Second != null) ? clause2(Second).ToString() : string.Empty)}, {((Third != null) ? clause3(Third).ToString() : string.Empty)}]";
		}

		public Triplet<T1, T2, T3> Clone()
		{
			return new Triplet<T1, T2, T3>(this);
		}

		public override bool Equals(object obj)
		{
			if (obj is Triplet<T1, T2, T3>)
			{
				Triplet<T1, T2, T3> triplet = (Triplet<T1, T2, T3>)obj;
				if (triplet != null && First.Equals(triplet.First) && Second.Equals(triplet.Second))
				{
					return Third.Equals(triplet.Third);
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
