using UnityEngine;

namespace SimplerGUI
{
	internal static class Rendering
	{
		public static readonly GUIStyle TextHeader = TextStyle(TextAnchor.UpperCenter, FontStyle.Bold, 20);

		public static readonly GUIStyle TextSubHeader = TextStyle(TextAnchor.MiddleCenter, FontStyle.Bold, 16);

		public static readonly GUIStyle TextLabelMiddleLeft = TextStyle(TextAnchor.MiddleLeft, FontStyle.Normal, 13);

		public static readonly GUIStyle TextLabelMiddleCenter = TextStyle(TextAnchor.MiddleCenter, FontStyle.Normal, 13);

		public static readonly GUIStyle TextLabelMiddleRight = TextStyle(TextAnchor.MiddleRight, FontStyle.Normal, 13);

		private static GUIStyle TextStyle(TextAnchor alignment, FontStyle fontStyle, int fontSize)
		{
			return new GUIStyle
			{
				alignment = alignment,
				fontStyle = fontStyle,
				fontSize = fontSize
			};
		}

		public static void Header(string text)
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("<color=#ffffff>" + text + "</color>", TextHeader, GUILayout.Width(70f), GUILayout.Height(30f));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		public static void SubHeader(string text)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">" + text + "</color>", TextSubHeader, GUILayout.Width(140f), GUILayout.Height(25f));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		public static void Label(string text, int align = 0, float width = 135f, float height = 25f)
		{
			switch (align)
			{
			case 0:
				GUILayout.Label("<color=#" + FengGameManagerMKII.finalhex() + ">" + text + "</color>", TextLabelMiddleLeft, GUILayout.Width(width), GUILayout.Height(height));
				break;
			case 1:
				GUILayout.Label("<color=#" + FengGameManagerMKII.finalhex() + ">" + text + "</color>", TextLabelMiddleCenter, GUILayout.Width(width), GUILayout.Height(height));
				break;
			case 2:
				GUILayout.Label("<color=#" + FengGameManagerMKII.finalhex() + ">" + text + "</color>", TextLabelMiddleRight, GUILayout.Width(width), GUILayout.Height(height));
				break;
			}
		}

		public static void AnotherLabel(string text, int align = 0, float width = 135f, float height = 25f)
		{
			switch (align)
			{
			case 0:
				GUILayout.Label("<color=#ffffff>" + text + "</color>", TextLabelMiddleLeft, GUILayout.Width(width), GUILayout.Height(height));
				break;
			case 1:
				GUILayout.Label("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">" + text + "</color>", TextLabelMiddleCenter, GUILayout.Width(width), GUILayout.Height(height));
				break;
			case 2:
				GUILayout.Label("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">" + text + "</color>", TextLabelMiddleRight, GUILayout.Width(width), GUILayout.Height(height));
				break;
			}
		}

		public static void Slider(string text, bool round, float multiplier, ref float value, float left, float right, string textFormat = "0", float width = 170f, float textWidth = 15f)
		{
			GUILayout.BeginHorizontal();
			Label(text);
			value = GUILayout.HorizontalSlider(value, left, right, GUILayout.Width(width));
			Label(round ? Mathf.Round((multiplier == 0f) ? value : (value * multiplier)).ToString(textFormat) : ((multiplier == 0f) ? value : (value * multiplier)).ToString(textFormat), 1, textWidth);
			GUILayout.EndHorizontal();
		}
	}
}
