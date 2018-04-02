using Godot;
using System;

public class CarSpawn : Area2D
{
    [Export] private Car.CarDirection direction;
    [Export] private readonly PackedScene carScene;
    [Export] private readonly NodePath spawnTargetNode;
    [Export] public bool AutoSpawn { get; set; } = true;
    [Export] public float SpawnChance { get; set; } = 0.5f;

    [Node("/root/ScoreManager")] private readonly ScoreManager scoreManager;
    [Node("Explode")] private readonly Sprite explode;
    [Node("Animation")] private readonly AnimationPlayer animationPlayer;
    [Node("Timer")] private readonly Timer timer;
    [Node("Honk")] private readonly AudioStreamPlayer honk;

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
                scoreManager.PlaySound(honk);
                if (!animationPlayer.IsPlaying())
                {
                    explode.SetVisible(true);
                    animationPlayer.Play("Explode");
                }
                return;
            }
        }
        if (AutoSpawn)
        {
            var roll = random.FloatRange(0, 1);
            if (roll > SpawnChance)
            {
                return;
            }
            Spawn();
        }
    }

    private void Spawn()
    {
        var newCar = (Car)carScene.Instance();
        newCar.SetDirection(direction);
        newCar.SetGlobalPosition(GetGlobalPosition());
        spawnTarget.AddChild(newCar);
    }
}
