using UnityEngine;

public class TableElement
{
	public string rawContent;

	public bool hasAction;

	public string content;

	public string color;

	public bool bold;

	public int size;

	public int width;

	public TableElement(string content, bool bold = false, string color = "white", int size = 14, int width = 100)
	{
		this.content = content;
		this.color = color;
		this.bold = bold;
		this.size = size;
		this.width = width;
	}

	public TableElement(string content, string type = "", string boxSize = "", bool hasAction = false, string rawContent = "", float scaleFactor = 1f)
	{
		this.content = content;
		this.hasAction = hasAction;
		if (rawContent != string.Empty)
		{
			this.rawContent = rawContent;
		}
		else
		{
			this.rawContent = content;
		}
		if (type == "header")
		{
			color = "white";
			bold = true;
			size = Mathf.RoundToInt(18f * scaleFactor);
		}
		else if (type == "cell")
		{
			color = "white";
			bold = false;
			size = Mathf.RoundToInt(14f * scaleFactor);
		}
		else
		{
			color = "white";
			bold = false;
			size = Mathf.RoundToInt(12f * scaleFactor);
		}
		switch (boxSize)
		{
		case "small":
			width = Mathf.RoundToInt(70f * scaleFactor);
			break;
		case "medium":
			width = Mathf.RoundToInt(120f * scaleFactor);
			break;
		case "large":
			width = Mathf.RoundToInt(180f * scaleFactor);
			break;
		default:
			width = Mathf.RoundToInt(120f * scaleFactor);
			break;
		}
	}

	public override string ToString()
	{
		string text = $"<size={size}><color={color}>{content}</color></size>";
		if (bold)
		{
			text = "<b>" + text + "</b>";
		}
		return text;
	}
}
