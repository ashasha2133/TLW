using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Set Color on Selection")]
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
public class SetColorOnSelection : MonoBehaviour
{
	private static Dictionary<string, int> f__switchmap4;

	private UIWidget mWidget;

	private void OnSelectionChange(string val)
	{
		if (mWidget == null)
		{
			mWidget = GetComponent<UIWidget>();
		}
		if (val == null)
		{
			return;
		}
		if (f__switchmap4 == null)
		{
			f__switchmap4 = new Dictionary<string, int>(7)
			{
				{ "White", 0 },
				{ "Red", 1 },
				{ "Green", 2 },
				{ "Blue", 3 },
				{ "Yellow", 4 },
				{ "Cyan", 5 },
				{ "Magenta", 6 }
			};
		}
		if (f__switchmap4.TryGetValue(val, out var value))
		{
			switch (value)
			{
			case 0:
				mWidget.color = Color.white;
				break;
			case 1:
				mWidget.color = Color.red;
				break;
			case 2:
				mWidget.color = Color.green;
				break;
			case 3:
				mWidget.color = Color.blue;
				break;
			case 4:
				mWidget.color = Color.yellow;
				break;
			case 5:
				mWidget.color = Color.cyan;
				break;
			case 6:
				mWidget.color = Color.magenta;
				break;
			}
		}
	}
}
