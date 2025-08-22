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

	public TableElement(string content, string type = "", string boxSize = "", bool hasAction = false, string rawContent = "")
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
			size = 18;
		}
		else if (type == "cell")
		{
			color = "white";
			bold = false;
			size = 14;
		}
		else
		{
			color = "white";
			bold = false;
			size = 12;
		}
		switch (boxSize)
		{
		case "small":
			width = 70;
			break;
		case "medium":
			width = 120;
			break;
		case "large":
			width = 180;
			break;
		default:
			width = 120;
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
