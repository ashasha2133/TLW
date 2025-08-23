using UnityEngine;

public static class GUIHelpers
{
	public enum Alignment
	{
		TOPLEFT,
		TOPCENTER,
		TOPRIGHT,
		RIGHT,
		BOTTOMRIGHT,
		BOTTOMCENTER,
		BOTTOMLEFT,
		LEFT,
		CENTER
	}

	public static Rect AlignRect(float width, float height, Alignment alignment)
	{
		return AlignRect(width, height, new Rect(0f, 0f, Screen.width, Screen.height), alignment, 0f, 0f);
	}

	public static Rect AlignRect(float width, float height, Alignment alignment, float xOffset, float yOffset)
	{
		return AlignRect(width, height, new Rect(0f, 0f, Screen.width, Screen.height), alignment, xOffset, yOffset);
	}

	public static Rect AlignRect(float width, float height, Rect parentRect, Alignment alignment, float xOffset, float yOffset)
	{
		Rect result = alignment switch
		{
			Alignment.TOPLEFT => new Rect(0f, 0f, width, height), 
			Alignment.TOPCENTER => new Rect(parentRect.width * 0.5f - width * 0.5f, 0f, width, height), 
			Alignment.TOPRIGHT => new Rect(parentRect.width - width, 0f, width, height), 
			Alignment.RIGHT => new Rect(parentRect.width - width, parentRect.height * 0.5f - height * 0.5f, width, height), 
			Alignment.BOTTOMRIGHT => new Rect(parentRect.width - width, parentRect.height - height, width, height), 
			Alignment.BOTTOMCENTER => new Rect(parentRect.width * 0.5f - width * 0.5f, parentRect.height - height, width, height), 
			Alignment.BOTTOMLEFT => new Rect(0f, parentRect.y + parentRect.height - height, width, height), 
			Alignment.LEFT => new Rect(0f, parentRect.height * 0.5f - height * 0.5f, width, height), 
			Alignment.CENTER => new Rect(parentRect.width * 0.5f - width * 0.5f, parentRect.height * 0.5f - height * 0.5f, width, height), 
			_ => new Rect(0f, 0f, width, height), 
		};
		result.x += parentRect.x + xOffset;
		result.y += parentRect.y + yOffset;
		return result;
	}
}
