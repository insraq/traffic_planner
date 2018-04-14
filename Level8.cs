using Godot;
using System;

public class Level8 : Node
{
    [Node("Instructions")] private Node2D instructions;

    public override void _Ready()
    {
        this.WireNodes();
        GetTree().CallGroup("Crossroads", "SetTouchable", false);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton ev && ev.IsPressed())
        {
            if (instructions.Visible)
            {
                instructions.Visible = false;
            }
            GetTree().CallGroup("Crossroads", "ToggleMode");
        }
    }
}
