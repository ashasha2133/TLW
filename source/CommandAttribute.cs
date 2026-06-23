using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CommandAttribute : Attribute
{
	public string Name { get; private set; }

	public CommandAttribute()
	{
		Name = null;
	}

	public CommandAttribute(string name)
	{
		Name = name;
	}
}
