using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class BotCommandAttribute : Attribute
{
    public string Name { get; private set; }

    public BotCommandAttribute()
    {
        Name = null;
    }

    public BotCommandAttribute(string name)
    {
        Name = name;
    }
}