using Godot;
using System;

public class TestScene : Node2D
{
    [Node("Car1")] private Car car1;
    [Node("Car2")] private Car car2;

    public override void _Ready()
    {
        this.WireNodes();
        car1.Stop();
        car2.Stop();
        car1.Start();
        car2.Start();

    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
}
