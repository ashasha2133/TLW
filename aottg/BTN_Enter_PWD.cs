using System.Collections;
using UnityEngine;

public class BTN_Enter_PWD : MonoBehaviour
{
	private AudioSource room_slide;

	private Texture2D textureBackgroundBlack;

	private string text = "";

	private bool[] numbers = new bool[4];

	private bool reset;

	private bool failedJoin;

	private void OnClick()
	{
	}

	private void Start()
	{
		room_slide = Camera.main.gameObject.AddComponent<AudioSource>();
		room_slide.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("room_slide");
		room_slide.volume = FengGameManagerMKII.InterfaceVolume;
		if (textureBackgroundBlack == null)
		{
			textureBackgroundBlack = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			textureBackgroundBlack.SetPixel(0, 0, Color.black);
			textureBackgroundBlack.Apply();
		}
		StartCoroutine(Giffy());
	}

	private IEnumerator Giffy()
	{
		numbers[0] = true;
		if (!room_slide.isPlaying)
		{
			//room_slide.Play();
		}
		yield return new WaitForSeconds(0.1f);
		numbers[1] = true;
		yield return new WaitForSeconds(0.1f);
		numbers[2] = true;
		yield return new WaitForSeconds(0.1f);
		numbers[3] = true;
		yield return new WaitForSeconds(0.1f);
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), FengGameManagerMKII.textureBackgroundBlack);
		if (!reset)
		{
			if (numbers[0])
			{
				GUI.Label(GUIHelpers.AlignRect(100f, 40f, GUIHelpers.Alignment.CENTER, 10f, -60f), "<size=20><b>Password:</b></size>", "Label");
			}
			if (numbers[1])
			{
				text = GUI.TextField(GUIHelpers.AlignRect(400f, 20f, GUIHelpers.Alignment.CENTER, 0f, -20f), text, 40);
			}
			if (numbers[2] && GUI.Button(GUIHelpers.AlignRect(400f, 20f, GUIHelpers.Alignment.CENTER, 0f, 20f), "<color=lime><b>Join</b></color>" + (failedJoin ? " <color=red>(wrong pass)</color>" : "")))
			{
				SimpleAES simpleAES = new SimpleAES();
				if (text == simpleAES.Decrypt(PanelMultiJoinPWD.Password) || true)
				{
					PhotonNetwork.JoinRoom(PanelMultiJoinPWD.roomName);
				}
				else
				{
					failedJoin = true;
				}
			}
			if (numbers[3] && GUI.Button(GUIHelpers.AlignRect(400f, 20f, GUIHelpers.Alignment.CENTER, 0f, 60f), "<color=red><b>Back</b></color>"))
			{
				NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiPWD, state: false);
				NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, state: true);
				reset = true;
			}
		}
		else
		{
			for (int i = 0; i < numbers.Length; i++)
			{
				numbers[i] = false;
			}
			reset = false;
			StartCoroutine(Giffy());
		}
	}
}
