using Godot;
using System;

public class CarSpawn : Area2D
{
    [Export] private Car.CarDirection direction;
    [Export] private readonly PackedScene carScene;
    [Export] private readonly NodePath spawnTargetNode;

    [Node("/root/ScoreManager")] private readonly ScoreManager scoreManager;
    [Node("Explode")] private readonly Sprite explode;
    [Node("Animation")] private readonly AnimationPlayer animationPlayer;

    private readonly Random random = new Random();
    private Node spawnTarget;

    public override void _Ready()
    {
        this.WireNodes();
        explode.Visible = false;
        spawnTarget = GetNode(spawnTargetNode);
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
                scoreManager.Life--;
                if (!animationPlayer.IsPlaying())
                {
                    explode.SetVisible(true);
                    animationPlayer.Play("Explode");
                }
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
        spawnTarget.AddChild(newCar);
    }
}
