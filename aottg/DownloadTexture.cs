using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class DownloadTexture : MonoBehaviour
{
	private sealed class Startc__Iterator7 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal object current;

		internal int PC;

		internal DownloadTexture f__this;

		internal UITexture ut__1;

		internal WWW www__0;

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
				www__0 = new WWW(f__this.url);
				current = www__0;
				PC = 1;
				return true;
			case 1u:
				f__this.mTex = www__0.texture;
				if (!(f__this.mTex == null))
				{
					ut__1 = f__this.GetComponent<UITexture>();
					if (ut__1.material != null)
					{
						f__this.mMat = new Material(ut__1.material);
					}
					else
					{
						f__this.mMat = new Material(Shader.Find("Unlit/Transparent Colored"));
					}
					ut__1.material = f__this.mMat;
					f__this.mMat.mainTexture = f__this.mTex;
					ut__1.MakePixelPerfect();
				}
				www__0.Dispose();
				PC = -1;
				break;
			}
			return false;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private Material mMat;

	private Texture2D mTex;

	public string url = "http://www.tasharen.com/misc/logo.png";

	private void OnDestroy()
	{
		if (mMat != null)
		{
			UnityEngine.Object.Destroy(mMat);
		}
		if (mTex != null)
		{
			UnityEngine.Object.Destroy(mTex);
		}
	}

	private IEnumerator Start()
	{
		return new Startc__Iterator7
		{
			f__this = this
		};
	}
}
