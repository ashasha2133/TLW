using System;
using UnityEngine;

public class InputManagerRC
{
	public KeyCode[] cannonKeys;

	public int[] cannonWheel;

	public KeyCode[] horseKeys;

	public int[] horseWheel;

	public KeyCode[] humanKeys;

	public int[] humanWheel;

	public KeyCode[] levelKeys;

	public int[] levelWheel;

	public KeyCode[] titanKeys;

	public int[] titanWheel;

	public KeyCode[] extraKey;

	public int[] extrawheel;

	public bool isInputExtras(int code)
	{
		if (extrawheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)extrawheel[code] > 0f;
		}
		return Input.GetKeyDown(extraKey[code]);
	}

	public bool isInputExtrasUp(int code)
	{
		if (extrawheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)extrawheel[code] > 0f;
		}
		return Input.GetKeyUp(extraKey[code]);
	}

	public void setInputExtras(int code, string setting)
	{
		extraKey[code] = KeyCode.None;
		extrawheel[code] = 0;
		if (setting == "Scroll Up")
		{
			extrawheel[code] = 1;
		}
		else if (setting == "Scroll Down")
		{
			extrawheel[code] = -1;
		}
		else if (Enum.IsDefined(typeof(KeyCode), setting))
		{
			extraKey[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
		}
	}

	public InputManagerRC()
	{
		humanWheel = new int[8];
		humanKeys = new KeyCode[8];
		horseWheel = new int[7];
		horseKeys = new KeyCode[7];
		titanWheel = new int[15];
		titanKeys = new KeyCode[15];
		levelWheel = new int[17];
		levelKeys = new KeyCode[17];
		cannonWheel = new int[7];
		cannonKeys = new KeyCode[7];
		extraKey = new KeyCode[5];
		extrawheel = new int[5];
		for (int i = 0; i < humanWheel.Length; i++)
		{
			humanWheel[i] = 0;
			humanKeys[i] = KeyCode.None;
		}
		for (int i = 0; i < horseWheel.Length; i++)
		{
			horseWheel[i] = 0;
			horseKeys[i] = KeyCode.None;
		}
		for (int i = 0; i < titanWheel.Length; i++)
		{
			titanWheel[i] = 0;
			titanKeys[i] = KeyCode.None;
		}
		for (int i = 0; i < levelWheel.Length; i++)
		{
			levelWheel[i] = 0;
			levelKeys[i] = KeyCode.None;
		}
		for (int j = 0; j < extrawheel.Length; j++)
		{
			extrawheel[j] = 0;
			extraKey[j] = KeyCode.None;
		}
	}

	public bool isInputCannon(int code)
	{
		if (cannonWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)cannonWheel[code] > 0f;
		}
		return Input.GetKey(cannonKeys[code]);
	}

	public bool isInputCannonDown(int code)
	{
		if (cannonWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)cannonWheel[code] > 0f;
		}
		return Input.GetKeyDown(cannonKeys[code]);
	}

	public bool isInputHorse(int code)
	{
		if (horseWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)horseWheel[code] > 0f;
		}
		return Input.GetKey(horseKeys[code]);
	}

	public bool isInputHorseDown(int code)
	{
		if (horseWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)horseWheel[code] > 0f;
		}
		return Input.GetKeyDown(horseKeys[code]);
	}

	public bool isInputHumanUp(int code)
	{
		if (humanWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)humanWheel[code] > 0f;
		}
		return Input.GetKeyUp(humanKeys[code]);
	}

	public bool isInputHuman(int code)
	{
		if (humanWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)humanWheel[code] > 0f;
		}
		return Input.GetKey(humanKeys[code]);
	}

	public bool isInputHumanDown(int code)
	{
		if (humanWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)humanWheel[code] > 0f;
		}
		return Input.GetKeyDown(humanKeys[code]);
	}

	public bool isInputLevel(int code)
	{
		if (levelWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)levelWheel[code] > 0f;
		}
		return Input.GetKey(levelKeys[code]);
	}

	public bool isInputLevelDown(int code)
	{
		if (levelWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)levelWheel[code] > 0f;
		}
		return Input.GetKeyDown(levelKeys[code]);
	}

	public bool isInputTitan(int code)
	{
		if (titanWheel[code] != 0)
		{
			return Input.GetAxis("Mouse ScrollWheel") * (float)titanWheel[code] > 0f;
		}
		return Input.GetKey(titanKeys[code]);
	}

	public void setInputCannon(int code, string setting)
	{
		cannonKeys[code] = KeyCode.None;
		cannonWheel[code] = 0;
		if (setting == "Scroll Up")
		{
			cannonWheel[code] = 1;
		}
		else if (setting == "Scroll Down")
		{
			cannonWheel[code] = -1;
		}
		else if (Enum.IsDefined(typeof(KeyCode), setting))
		{
			cannonKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
		}
	}

	public void setInputHorse(int code, string setting)
	{
		horseKeys[code] = KeyCode.None;
		horseWheel[code] = 0;
		if (setting == "Scroll Up")
		{
			horseWheel[code] = 1;
		}
		else if (setting == "Scroll Down")
		{
			horseWheel[code] = -1;
		}
		else if (Enum.IsDefined(typeof(KeyCode), setting))
		{
			horseKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
		}
	}

	public void setInputHuman(int code, string setting)
	{
		humanKeys[code] = KeyCode.None;
		humanWheel[code] = 0;
		if (setting == "Scroll Up")
		{
			humanWheel[code] = 1;
		}
		else if (setting == "Scroll Down")
		{
			humanWheel[code] = -1;
		}
		else if (Enum.IsDefined(typeof(KeyCode), setting))
		{
			humanKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
		}
	}

	public void setInputLevel(int code, string setting)
	{
		levelKeys[code] = KeyCode.None;
		levelWheel[code] = 0;
		if (setting == "Scroll Up")
		{
			levelWheel[code] = 1;
		}
		else if (setting == "Scroll Down")
		{
			levelWheel[code] = -1;
		}
		else if (Enum.IsDefined(typeof(KeyCode), setting))
		{
			levelKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
		}
	}

	public void setInputTitan(int code, string setting)
	{
		titanKeys[code] = KeyCode.None;
		titanWheel[code] = 0;
		if (setting == "Scroll Up")
		{
			titanWheel[code] = 1;
		}
		else if (setting == "Scroll Down")
		{
			titanWheel[code] = -1;
		}
		else if (Enum.IsDefined(typeof(KeyCode), setting))
		{
			titanKeys[code] = (KeyCode)Enum.Parse(typeof(KeyCode), setting);
		}
	}
}
