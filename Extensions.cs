using Godot;
using System;
using System.Reflection;

public static class Extensions
{
    public static void WireNodes(this Node node)
    {
        FieldInfo[] info = node
            .GetType()
            .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var f in info)
        {
            NodeAttribute attr = (NodeAttribute)Attribute.GetCustomAttribute(f, typeof(NodeAttribute));
            if (attr != null)
            {
                Node nodeInstnace = node.GetNode(attr.nodePath);
                if (nodeInstnace == null)
                {
                    throw new NullReferenceException($"Cannot find Node for NodePath '{attr.nodePath}'");
                }
                if (f.FieldType != nodeInstnace.GetType())
                {
                    throw new InvalidCastException($"For NodePath '{attr.nodePath}', expect Type '{f.FieldType}', but get '{nodeInstnace.GetType()}'");
                }
                f.SetValue(node, nodeInstnace);
            }
        }
    }

    public static float FloatRange(this Random random, float min = 0.0f, float max = 1.0f)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}