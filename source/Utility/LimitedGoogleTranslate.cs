using System;
using SimpleJSON;
using UnityEngine;

namespace Utility;

public class LimitedGoogleTranslate : ITranslator
{
	private TimeSpan _timeOutDuration;

	private DateTime? _lastTimeOut;

	public LimitedGoogleTranslate()
	{
		_timeOutDuration = TimeSpan.FromMinutes(0.0);
		_lastTimeOut = null;
	}

	public bool IsWorking()
	{
		return DateTime.Now.Subtract(_lastTimeOut ?? DateTime.MinValue) > _timeOutDuration;
	}

	public WWW GetWWW(string fromLang, string toLang, string text)
	{
		return new WWW("https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + fromLang + "&tl=" + toLang + "&dt=t&q=" + text);
	}

	public string ParseTranslation(WWW link, string originalText)
	{
		if (link.error != null)
		{
			string value;
			bool flag = link.responseHeaders.TryGetValue("Retry-After", out value);
			_lastTimeOut = DateTime.Now;
			if (link.error.Contains("429"))
			{
				if (flag)
				{
					try
					{
						_timeOutDuration = TimeSpan.FromMinutes(int.Parse(value));
					}
					catch
					{
						_timeOutDuration = TimeSpan.FromMinutes(30.0);
					}
				}
				else
				{
					_timeOutDuration = TimeSpan.FromMinutes(30.0);
				}
			}
			else
			{
				_timeOutDuration = TimeSpan.FromMinutes(1.0);
			}
			return originalText;
		}
		return JSON.Parse(link.text).AsArray[0].AsArray[0].AsArray[0].Value;
	}

	public string GetErrorMessage()
	{
		return "<color=red>[Translation Error - Switching API]</color>";
	}
}
