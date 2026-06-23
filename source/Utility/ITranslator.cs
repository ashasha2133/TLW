using UnityEngine;

namespace Utility;

public interface ITranslator
{
	string GetErrorMessage();

	bool IsWorking();

	WWW GetWWW(string fromLang, string toLang, string text);

	string ParseTranslation(WWW link, string originalText);
}
