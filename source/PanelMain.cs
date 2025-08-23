using UnityEngine;

public class PanelMain : MonoBehaviour
{
	public GameObject label_credits;

	public GameObject label_multi;

	public GameObject label_option;

	public GameObject label_single;

	private void Start()
	{
		label_single.GetComponent<UILabel>().enabled = false;
		label_multi.GetComponent<UILabel>().enabled = false;
		label_option.GetComponent<UILabel>().enabled = false;
		label_credits.GetComponent<UILabel>().enabled = false;
		UIButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].transform.position = new Vector3(0f, 9999f, 0f);
		}
	}
}
