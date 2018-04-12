using Godot;
using System;

public class Level7 : Node
{
    [Node("Instructions")] private Node2D instructions;

    public override void _Ready()
    {
        this.WireNodes();
        GetTree().CallGroup("Crossroads", "SetTouchable", false);
    }

    public override void _Process(float delta)
    {
        // Called every frame. Delta is time since last frame.
        // Update game logic here.
    }

    public override void _Input(InputEvent @event)
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
