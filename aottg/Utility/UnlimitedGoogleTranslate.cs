using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Utility
{
	public class UnlimitedGoogleTranslate : ITranslator
	{
		private bool _isWorking;

		private int _numMessagesSent;

		private readonly int _maxMessages = 3000;

		public UnlimitedGoogleTranslate()
		{
			_isWorking = true;
		}

		public bool IsWorking()
		{
			return _isWorking;
		}

		public WWW GetWWW(string fromLang, string toLang, string text)
		{
			return new WWW("https://clients5.google.com/translate_a/t?client=dict-chrome-ex&sl=" + fromLang + "&tl=" + toLang + "&q=" + text, null, new Dictionary<string, string> { { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36" } });
		}

		public string ParseTranslation(WWW link, string originalText)
		{
			_numMessagesSent++;
			if (_numMessagesSent > _maxMessages)
			{
				_isWorking = false;
				return originalText;
			}
			if (link.error != null)
			{
				_isWorking = false;
				return originalText;
			}
			if (link.text == "[" + originalText + "]")
			{
				_isWorking = false;
				return originalText;
			}
			return JSON.Parse(link.text).AsArray[0].Value;
		}

		public string GetErrorMessage()
		{
			return "<color=red>[Translation Error - Switching API]</color>";
		}
	}
}
