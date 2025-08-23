using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterList<T>
{
	private sealed class GetEnumeratorc__Iterator9 : IEnumerator, IDisposable, IEnumerator<T>
	{
		internal T current;

		internal int PC;

		internal BetterList<T> f__this;

		internal int __0;

		T IEnumerator<T>.Current => current;

		object IEnumerator.Current => current;

		public void Dispose()
		{
			PC = -1;
		}

		public bool MoveNext()
		{
			uint pC = (uint)PC;
			PC = -1;
			if (pC != 0)
			{
				if (pC != 1)
				{
					goto IL_007a;
				}
				__0++;
			}
			else
			{
				if (f__this.buffer == null)
				{
					goto IL_0073;
				}
				__0 = 0;
			}
			if (__0 < f__this.size)
			{
				current = f__this.buffer[__0];
				PC = 1;
				return true;
			}
			goto IL_0073;
			IL_0073:
			PC = -1;
			goto IL_007a;
			IL_007a:
			return false;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public T[] buffer;

	public int size;

	public T this[int i]
	{
		get
		{
			return buffer[i];
		}
		set
		{
			buffer[i] = value;
		}
	}

	public void Add(T item)
	{
		if (buffer == null || size == buffer.Length)
		{
			AllocateMore();
		}
		buffer[size++] = item;
	}

	private void AllocateMore()
	{
		T[] array = ((buffer == null) ? new T[32] : new T[Mathf.Max(buffer.Length << 1, 32)]);
		if (buffer != null && size > 0)
		{
			buffer.CopyTo(array, 0);
		}
		buffer = array;
	}

	public void Clear()
	{
		size = 0;
	}

	public bool Contains(T item)
	{
		if (buffer != null)
		{
			for (int i = 0; i < size; i++)
			{
				if (buffer[i].Equals(item))
				{
					return true;
				}
			}
		}
		return false;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new GetEnumeratorc__Iterator9
		{
			f__this = this
		};
	}

	public void Insert(int index, T item)
	{
		if (buffer == null || size == buffer.Length)
		{
			AllocateMore();
		}
		if (index < size)
		{
			for (int num = size; num > index; num--)
			{
				buffer[num] = buffer[num - 1];
			}
			buffer[index] = item;
			size++;
		}
		else
		{
			Add(item);
		}
	}

	public T Pop()
	{
		if (buffer != null && size != 0)
		{
			T result = buffer[--size];
			buffer[size] = default(T);
			return result;
		}
		return default(T);
	}

	public void Release()
	{
		size = 0;
		buffer = null;
	}

	public bool Remove(T item)
	{
		if (buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < size; i++)
			{
				if (@default.Equals(buffer[i], item))
				{
					size--;
					buffer[i] = default(T);
					for (int j = i; j < size; j++)
					{
						buffer[j] = buffer[j + 1];
					}
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		if (buffer != null && index < size)
		{
			size--;
			buffer[index] = default(T);
			for (int i = index; i < size; i++)
			{
				buffer[i] = buffer[i + 1];
			}
		}
	}

	public void Sort(Comparison<T> comparer)
	{
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = 1; i < size; i++)
			{
				if (comparer(buffer[i - 1], buffer[i]) > 0)
				{
					T val = buffer[i];
					buffer[i] = buffer[i - 1];
					buffer[i - 1] = val;
					flag = true;
				}
			}
		}
	}

	public T[] ToArray()
	{
		Trim();
		return buffer;
	}

	private void Trim()
	{
		if (size > 0)
		{
			if (size < buffer.Length)
			{
				T[] array = new T[size];
				for (int i = 0; i < size; i++)
				{
					array[i] = buffer[i];
				}
				buffer = array;
			}
		}
		else
		{
			buffer = null;
		}
	}
}
