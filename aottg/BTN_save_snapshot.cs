using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTN_save_snapshot : MonoBehaviour
{
	private sealed class ScreenshotEncodec__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal object current;

		internal int PC;

		internal GameObject[] s_5__2;

		internal int s_6__3;

		internal BTN_save_snapshot f__this;

		internal GameObject go__4;

		internal string img_name__5;

		internal float r__0;

		internal Texture2D texture__1;

		object IEnumerator<object>.Current => current;

		object IEnumerator.Current => current;

		public void Dispose()
		{
			PC = -1;
		}

		public bool MoveNext()
		{
			uint pC = (uint)PC;
			PC = -1;
			switch (pC)
			{
			case 0u:
				current = new WaitForEndOfFrame();
				PC = 1;
				break;
			case 1u:
				r__0 = (float)Screen.height / 600f;
				texture__1 = new Texture2D((int)(r__0 * f__this.targetTexture.transform.localScale.x), (int)(r__0 * f__this.targetTexture.transform.localScale.y), TextureFormat.RGB24, mipmap: false);
				texture__1.ReadPixels(new Rect((float)Screen.width * 0.5f - (float)texture__1.width * 0.5f, (float)Screen.height * 0.5f - (float)texture__1.height * 0.5f - r__0 * 0f, texture__1.width, texture__1.height), 0, 0);
				texture__1.Apply();
				current = 0;
				PC = 2;
				break;
			case 2u:
			{
				s_5__2 = f__this.thingsNeedToHide;
				for (s_6__3 = 0; s_6__3 < s_5__2.Length; s_6__3++)
				{
					go__4 = s_5__2[s_6__3];
					go__4.transform.position -= Vector3.up * 10000f;
				}
				string[] array = new string[13]
				{
					"aottg_ss-",
					DateTime.Today.Month.ToString(),
					"_",
					DateTime.Today.Day.ToString(),
					"_",
					DateTime.Today.Year.ToString(),
					"-",
					DateTime.Now.Hour.ToString(),
					"_",
					DateTime.Now.Minute.ToString(),
					"_",
					DateTime.Now.Second.ToString(),
					".png"
				};
				img_name__5 = string.Concat(array);
				object[] args = new object[4]
				{
					img_name__5,
					texture__1.width,
					texture__1.height,
					Convert.ToBase64String(texture__1.EncodeToPNG())
				};
				Application.ExternalCall("SaveImg", args);
				UnityEngine.Object.DestroyObject(texture__1);
				PC = -1;
				goto default;
			}
			default:
				return false;
			}
			return true;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public GameObject info;

	public GameObject targetTexture;

	public GameObject[] thingsNeedToHide;

	private void OnClick()
	{
		GameObject[] array = thingsNeedToHide;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.position += Vector3.up * 10000f;
		}
		StartCoroutine(ScreenshotEncode());
		info.GetComponent<UILabel>().text = "trying..";
	}

	private IEnumerator ScreenshotEncode()
	{
		return new ScreenshotEncodec__Iterator0
		{
			f__this = this
		};
	}
}
