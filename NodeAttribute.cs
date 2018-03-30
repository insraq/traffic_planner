using Godot;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class NodeAttribute : Attribute
{
    public string nodePath;

    public NodeAttribute(string np)
    {
        nodePath = np;
    }
}