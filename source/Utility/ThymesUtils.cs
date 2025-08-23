using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Utility
{
	public static class ThymesUtils
	{
		public enum LANGUAGE
		{
			en,
			es,
			pt,
			ru,
			tl,
			th,
			fr,
			it,
			tr
		}

		private static ITranslator[] _translators = new ITranslator[2]
		{
			new LimitedGoogleTranslate(),
			new UnlimitedGoogleTranslate()
		};

		private static bool _firstAllTranslatorsDownMessage = true;

		public static bool BracketHexIsValid(string color)
		{
			return Regex.IsMatch(color, "^\\[(?:[0-9a-fA-F]{3}){2}\\]$");
		}

		public static bool HashHexIsValid(string color)
		{
			return Regex.IsMatch(color, "^#(?:[0-9a-fA-F]{3}){2}$");
		}

		public static bool RawHexIsValid(string color)
		{
			return Regex.IsMatch(color, "^(?:[0-9a-fA-F]{3}){2}$");
		}

		public static string[] GetAllBracketHexOccurrences(string text)
		{
			MatchCollection matchCollection = Regex.Matches(text, "\\[(?:[0-9a-fA-F]{3}){2}\\]");
			string[] array = new string[matchCollection.Count];
			int num = 0;
			foreach (Match item in matchCollection)
			{
				array[num] = item.Value;
				num++;
			}
			return array;
		}

		public static string RemoveAllColorCodes(string text)
		{
			return Regex.Replace(text, "\\[(?:[0-9a-fA-F]{3}){2}\\]", string.Empty);
		}

		public static string TruncateTextWithHex(string text, int maxLength)
		{
			string[] array = TokenizeText(text);
			string text2 = string.Empty;
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (num < maxLength)
				{
					if (array[i].Length == 1)
					{
						num++;
					}
					text2 += array[i];
				}
			}
			return text2;
		}

		public static string[] TokenizeText(string text)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(text, "\\[(?:[0-9a-fA-F]{3}){2}\\]");
			int num = 0;
			foreach (Match item in matchCollection)
			{
				int index = item.Index;
				for (int i = num; i < index; i++)
				{
					list.Add(text.Substring(i, 1));
				}
				list.Add(item.Value);
				num = index + item.Value.Length;
			}
			for (int j = num; j < text.Length; j++)
			{
				list.Add(text.Substring(j, 1));
			}
			return list.ToArray();
		}

		public static Color ColorFromHex(string color)
		{
			color = color.Replace("#", string.Empty);
			if (BracketHexIsValid(color))
			{
				color = color.Replace("[", string.Empty);
				color = color.Replace("]", string.Empty);
			}
			else if (HashHexIsValid(color))
			{
				color = color.Replace("#", string.Empty);
			}
			else if (!RawHexIsValid(color))
			{
				throw new Exception("Passed param " + color + " was not in a legal Hex color format.");
			}
			int num = int.Parse(color.Substring(0, 2), NumberStyles.AllowHexSpecifier);
			int num2 = int.Parse(color.Substring(2, 2), NumberStyles.AllowHexSpecifier);
			int num3 = int.Parse(color.Substring(4, 2), NumberStyles.AllowHexSpecifier);
			return new Color((float)num / 255f, (float)num2 / 255f, (float)num3 / 255f, 1f);
		}

		public static float RelativeLuminance(string color)
		{
			return RelativeLuminance(ColorFromHex(color));
		}

		public static float RelativeLuminance(Color color)
		{
			return (color.r * 0.2126f + color.g * 0.7152f + color.b * 0.0722f) * color.a;
		}

		public static IEnumerator TranslateText(string text, int fengSettingCode, Action<string, string> callback)
		{
			string fromLang = "auto";
			string toLang = Enum.GetName(typeof(LANGUAGE), (LANGUAGE)fengSettingCode);
			string textEscaped = WWW.EscapeURL(text);
			string errors = "";
			ITranslator[] translators = _translators;
			foreach (ITranslator tr in translators)
			{
				if (!tr.IsWorking())
				{
					continue;
				}
				using WWW link = tr.GetWWW(fromLang, toLang, textEscaped);
				yield return link;
				string arg = tr.ParseTranslation(link, text);
				if (tr.IsWorking())
				{
					_firstAllTranslatorsDownMessage = true;
					callback(arg, errors);
					yield break;
				}
				errors = tr.GetErrorMessage();
			}
			if (_firstAllTranslatorsDownMessage)
			{
				errors = "<color=red>[Translation Error - No API Available]</color>";
				_firstAllTranslatorsDownMessage = !_firstAllTranslatorsDownMessage;
			}
			callback(text, errors);
		}

		public static void RecursiveUniformRandomNumberSets(ref List<List<int>> solutions, List<int> solution, List<int> minRanges, List<int> maxRanges, int sum)
		{
			if (minRanges.Count == 0)
			{
				if (sum < 0)
				{
					return;
				}
				List<int> list = new List<int>();
				foreach (int item in solution)
				{
					list.Add(item);
				}
				solutions.Add(list);
			}
			else
			{
				int num = minRanges[0];
				int num2 = maxRanges[0];
				for (int i = num; i <= num2 && sum - i >= 0; i++)
				{
					solution.Add(i);
					RecursiveUniformRandomNumberSets(ref solutions, solution, minRanges.GetRange(1, minRanges.Count - 1), maxRanges.GetRange(1, minRanges.Count - 1), sum);
					solution.RemoveAt(solution.Count - 1);
				}
			}
		}

		public static float Map(float x, float inMin, float inMax, float outMin, float outMax)
		{
			if (x > inMax || x < inMin)
			{
				throw new Exception("Error,\npublic float map(float x, float inMin, float inMax, float outMin, float outMax)\nis not defined for values (x > inMax || x < inMin)");
			}
			return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		}
	}
}
