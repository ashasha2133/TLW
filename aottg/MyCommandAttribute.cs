using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class MyCommandAttribute : Attribute
{
	public string Name { get; private set; }

	public MyCommandAttribute()
	{
		Name = null;
	}

	public MyCommandAttribute(string name)
	{
		Name = name;
	}
}
