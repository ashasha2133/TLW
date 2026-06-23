using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class RCextensions
{
	private static readonly string HexPattern = "(\\[)[\\w]{6}(\\])";

	private static readonly Regex HexRegex = new Regex(HexPattern);

	public static string StripHex(this string text)
	{
		return HexRegex.Replace(text, "");
	}

	public static Transform FindChild(this GameObject go, string name)
	{
		UnityEngine.Component[] componentsInChildren = go.GetComponentsInChildren(typeof(Transform), includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Transform transform = (Transform)componentsInChildren[i];
			if (transform.name == name)
			{
				return transform;
			}
		}
		return null;
	}

	public static int ToInt32(this object l)
	{
		return Convert.ToInt32(l);
	}

	public static Color Lerp3(Color a, Color b, Color c, float t)
	{
		if (t < 0.5f)
		{
			return Color.Lerp(a, b, t / 0.5f);
		}
		return Color.Lerp(b, c, (t - 0.5f) / 0.5f);
	}

	public static string FilterSizeTag(this string text)
	{
		MatchCollection matchCollection = Regex.Matches(text.ToLower(), "(<size=(.*?>))");
		List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
		foreach (Match item in matchCollection)
		{
			if (!list.Any((KeyValuePair<int, string> p) => p.Key == item.Index))
			{
				list.Add(new KeyValuePair<int, string>(item.Index, item.Value));
			}
		}
		foreach (KeyValuePair<int, string> item2 in list)
		{
			if (item2.Value.StartsWith("<size=") && item2.Value.Length > 9)
			{
				text = text.Remove(item2.Key, item2.Value.Length);
				text = text.Substring(0, item2.Key) + "<size=20>" + text.Substring(item2.Key, text.Length - item2.Key);
			}
		}
		if (text.Contains("quad material"))
		{
			text = "";
		}
		return text;
	}

	[Description("Removes all HTML formatted color codes from string")]
	public static string StripColor(this string s)
	{
		return Regex.Replace(s, "((<(\\/|)(color(?(?=\\=).*?)>|b>|size.*?>|i>)))", "", RegexOptions.IgnoreCase);
	}

	public static bool CheckIP(string url)
	{
		if (url != string.Empty)
		{
			if (url.StartsWith("file:///C:/Users/"))
			{
				return true;
			}
			url = (url.StartsWith("https://") ? url.Remove(0, 8) : (url.StartsWith("http://") ? url.Remove(0, 7) : (url.StartsWith("www.") ? url.Remove(0, 4) : url.Remove(0, 0))));
			string[] array = new string[24]
			{
				"i.imgur.com/", "imgur.com/", "image.ibb.co/", "i.reddit.it/", "cdn.discordapp.com/attachments/", "media.discordapp.net/attachments/", "images-ext-2.discordapp.net/external/", "i.reddit.it/", "gyazo.com/", "i.gyazo.com",
				"puu.sh/", "i.postimg.cc/", "postimg./", "deviantart.com/", "photobucket.com/", "aotcorehome.files.wordpress.com/", "s1.ax1x.com/", "s27.postimg.io/", "s19.postimg.org/", "1.bp.blogspot.com/",
				"tiebapic.baidu.com/", "s25.postimg.gg/", "s25.postimg.org/", "aotcorehome.files.wordpress.com/"
			};
			foreach (string value in array)
			{
				if (url.StartsWith(value))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void Add<T>(ref T[] source, T value)
	{
		T[] array = new T[source.Length + 1];
		for (int i = 0; i < source.Length; i++)
		{
			array[i] = source[i];
		}
		array[array.Length - 1] = value;
		source = array;
	}

	public static float RoundTo(this float value, int decimals)
	{
		return (float)decimal.Round((decimal)value, decimals);
	}

	public static string hexColor(this string text)
	{
		if (text.Contains("]"))
		{
			text = text.Replace("]", ">");
		}
		bool flag = false;
		while (text.Contains("[") && !flag)
		{
			int num = text.IndexOf("[");
			if (text.Length >= num + 7)
			{
				string text2 = text.Substring(num + 1, 6);
				text = text.Remove(num, 7).Insert(num, "<color=#" + text2);
				int startIndex = text.Length;
				if (text.Contains("["))
				{
					startIndex = text.IndexOf("[");
				}
				text = text.Insert(startIndex, "</color>");
			}
			else
			{
				flag = true;
			}
		}
		if (flag)
		{
			return string.Empty;
		}
		return text;
	}

	public static Texture2D loadimage(WWW link, bool mipmap, int size)
	{
		Texture2D texture2D = new Texture2D(4, 4, TextureFormat.DXT1, mipmap);
		if (link.size >= size)
		{
			return texture2D;
		}
		Texture2D texture = link.texture;
		int width = texture.width;
		int height = texture.height;
		int num = 0;
		if (width < 4 || (width & (width - 1)) != 0)
		{
			num = 4;
			width = Math.Min(width, 1023);
			while (num < width)
			{
				num *= 2;
			}
		}
		else if (height < 4 || (height & (height - 1)) != 0)
		{
			num = 4;
			height = Math.Min(height, 1023);
			while (num < height)
			{
				num *= 2;
			}
		}
		if (num == 0)
		{
			if (mipmap)
			{
				try
				{
					link.LoadImageIntoTexture(texture2D);
				}
				catch
				{
					texture2D = new Texture2D(4, 4, TextureFormat.DXT1, mipmap: false);
					link.LoadImageIntoTexture(texture2D);
				}
				return texture2D;
			}
			link.LoadImageIntoTexture(texture2D);
			return texture2D;
		}
		if (num < 4)
		{
			return texture2D;
		}
		Texture2D texture2D2 = new Texture2D(4, 4, TextureFormat.DXT1, mipmap: false);
		link.LoadImageIntoTexture(texture2D2);
		if (mipmap)
		{
			try
			{
				texture2D2.Resize(num, num, TextureFormat.DXT1, mipmap);
			}
			catch
			{
				texture2D2.Resize(num, num, TextureFormat.DXT1, hasMipMap: false);
			}
		}
		else
		{
			texture2D2.Resize(num, num, TextureFormat.DXT1, mipmap);
		}
		texture2D2.Apply();
		return texture2D2;
	}

	public static void RemoveAt<T>(ref T[] source, int index)
	{
		if (source.Length == 1)
		{
			source = new T[0];
		}
		else
		{
			if (source.Length <= 1)
			{
				return;
			}
			T[] array = new T[source.Length - 1];
			int i = 0;
			int num = 0;
			for (; i < source.Length; i++)
			{
				if (i != index)
				{
					array[num] = source[i];
					num++;
				}
			}
			source = array;
		}
	}

	public static bool returnBoolFromObject(object obj)
	{
		if (obj != null && obj is bool)
		{
			return (bool)obj;
		}
		return false;
	}

	public static float returnFloatFromObject(object obj)
	{
		if (obj != null && obj is float)
		{
			return (float)obj;
		}
		return 0f;
	}

	public static int returnIntFromObject(object obj)
	{
		if (obj != null && obj is int)
		{
			return (int)obj;
		}
		return 0;
	}

	public static bool HasHexAt(this string text, int i)
	{
		if (text[i] == '[' && text.Substring(i).Length > 7 && text[i + 7] == ']' && int.TryParse(text.Substring(i + 1, 2), NumberStyles.HexNumber, null, out var result) && int.TryParse(text.Substring(i + 3, 2), NumberStyles.HexNumber, null, out result))
		{
			return int.TryParse(text.Substring(i + 5, 2), NumberStyles.HexNumber, null, out result);
		}
		return false;
	}

	public static string ToRGBA(this string value)
	{
		try
		{
			string text = value.Replace("[-]", string.Empty);
			if (text.Contains("[") && text.Contains("]"))
			{
				int num = text.IndexOf("[");
				while (num >= 0 && text.HasHexAt(num))
				{
					text = text.Insert(num + 8, "<color=#" + text.Substring(num + 1, 6).ToLower() + ">").Remove(num, 8);
					text += "</color>";
					num = text.IndexOf("[", num);
				}
			}
			return text;
		}
		catch
		{
			try
			{
				return value.StripHex();
			}
			catch
			{
				return value;
			}
		}
	}

	public static string NullFix(this string nullorempty)
	{
		if (nullorempty == null)
		{
			return string.Empty;
		}
		return nullorempty;
	}

	public static bool IsNullOrEmpty(this string value)
	{
		if (value != null)
		{
			return value.Length == 0;
		}
		return true;
	}

	public static T MinValueOrNull<T, TNum>(this IEnumerable<T> collection, Func<T, TNum> func, Func<T, bool> where) where T : class where TNum : IComparable, IFormattable, IConvertible, IComparable<TNum>, IEquatable<TNum>
	{
		List<T> list = collection.ToList();
		if (list.Count > 0)
		{
			bool flag = true;
			TNum other = default(TNum);
			T result = list[0];
			{
				foreach (T item in list)
				{
					TNum val;
					if (item != null && where(item) && (val = func(item)) != null)
					{
						if (flag)
						{
							result = item;
							flag = false;
							other = val;
						}
						else if (val.CompareTo(other) <= 0)
						{
							result = item;
							other = val;
						}
					}
				}
				return result;
			}
		}
		return null;
	}

	public static string ToEngFormat<T>(this IEnumerable<T> collection, string word, Func<T, string> select)
	{
		List<string> list = new List<string>();
		if (typeof(T).IsValueType)
		{
			foreach (T item3 in collection)
			{
				string item;
				if (!string.IsNullOrEmpty(item = select(item3)))
				{
					list.Add(item);
				}
			}
		}
		else
		{
			foreach (T item4 in collection)
			{
				string item2;
				if (item4 != null && !string.IsNullOrEmpty(item2 = select(item4)))
				{
					list.Add(item2);
				}
			}
		}
		return HandleEngFormat(word, list);
	}

	public static bool IsNullOrWhiteSpace(this string value)
	{
		if (value == null)
		{
			return true;
		}
		for (int i = 0; i < value.Length; i++)
		{
			if (!char.IsWhiteSpace(value[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static string ToEngFormat<T>(this IEnumerable<T> collection, string word = "and")
	{
		List<string> list = new List<string>();
		if (typeof(T).IsValueType)
		{
			foreach (T item3 in collection)
			{
				string item;
				if (!string.IsNullOrEmpty(item = item3.ToString()))
				{
					list.Add(item);
				}
			}
			list = new List<string>(collection.Select((T val) => val.ToString()));
		}
		else
		{
			foreach (T item4 in collection)
			{
				string item2;
				if (item4 != null && !string.IsNullOrEmpty(item2 = item4.ToString()))
				{
					list.Add(item2);
				}
			}
		}
		return HandleEngFormat(word, list);
	}

	private static string HandleEngFormat(string word, List<string> list)
	{
		switch (list.Count)
		{
		case 0:
			return string.Empty;
		case 1:
			return list[0];
		case 2:
			return list[0] + " " + word + " " + list[1];
		default:
		{
			if (list.Count < 0)
			{
				throw new Exception("count is less than 0.");
			}
			int index = list.Count - 1;
			list[index] = word + " " + list[index];
			string text = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				text = text + ", " + list[i];
			}
			return text;
		}
		}
	}

	public static string returnStringFromObject(object obj)
	{
		if (obj != null && obj is string result)
		{
			return result;
		}
		return string.Empty;
	}
}
