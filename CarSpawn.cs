using Godot;
using System;

public class CarSpawn : Area2D
{
    [Export]
    private Car.Direction direction;
    private PackedScene carScene;
    private Random random;

    public override void _Ready()
    {
        // Hide the placeholder
        carScene = (PackedScene)ResourceLoader.Load("res://Car.tscn");
        random = new Random();
    }

    public override void _Process(float delta)
    {

    }

    private void OnTimeOut()
    {
        var areas = GetOverlappingAreas();
        foreach (var area in areas)
        {
            if (area is Car car && car.GetDirection() == direction)
            {
                GetTree().SetPause(true);
                OS.Alert("Game Over");
                return;
            }
        }
        var roll = random.Next(0, 3);
        if (roll > 0)
        {
            return;
        }
        var newCar = (Car)carScene.Instance();
        newCar.SetDirection(direction);
        newCar.SetGlobalPosition(GetGlobalPosition());
        GetNode("/root/game").AddChild(newCar);
    }
}
