using BRM;
using UnityEngine;

public class MapNameChange : MonoBehaviour
{
	private void OnSelectionChange()
	{
		LevelInfo info = LevelInfo.getInfo(GetComponent<UIPopupList>().selection);
		if (info != null)
		{
			CacheGameObject.Find("LabelLevelInfo").GetComponent<UILabel>().text = info.desc;
		}
		if (GetComponent<UIPopupList>().items.Contains("Colossal Titan"))
		{
			GetComponent<UIPopupList>().items.Remove("Colossal Titan");
		}
		if (!GetComponent<UIPopupList>().items.Contains("[S]Forest Survive(no crawler no punk)"))
		{
			GetComponent<UIPopupList>().items.Add("[S]Forest Survive(no crawler no punk)");
		}
		if (!GetComponent<UIPopupList>().items.Contains("Custom"))
		{
			GetComponent<UIPopupList>().items.Add("Custom");
			GetComponent<UIPopupList>().textScale *= 0.8f;
		}
		if (!GetComponent<UIPopupList>().items.Contains("Custom (No PT)"))
		{
			GetComponent<UIPopupList>().items.Add("Custom (No PT)");
		}
		if (!GetComponent<UIPopupList>().items.Contains("Custom-Anarchy (No PT)"))
		{
			GetComponent<UIPopupList>().items.Add("Custom-Anarchy (No PT)");
		}
	}
}
